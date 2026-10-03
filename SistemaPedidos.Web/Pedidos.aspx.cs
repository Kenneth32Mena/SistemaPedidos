using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.UI;
using System.Web.UI.WebControls;
using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Web.Composition;
using SistemaPedidos.Web.Models;

namespace SistemaPedidos.Web
{
    public partial class Pedidos : Page
    {
        private readonly IPedidoService _pedidoService;
        private readonly IProductoService _productoService;
        private readonly IClienteService _clienteService;

        private const string SESSION_CARRITO = "CarritoDePedidos_Session";

        public Pedidos()
        {
            _pedidoService = ServiceFactory.CreatePedidoService();
            _productoService = ServiceFactory.CreateProductoService();
            _clienteService = ServiceFactory.CreateClienteService();
        }

        private List<CarritoItemViewModel> Carrito
        {
            get
            {
                if (Session[SESSION_CARRITO] == null)
                {
                    Session[SESSION_CARRITO] = new List<CarritoItemViewModel>();
                }
                return (List<CarritoItemViewModel>)Session[SESSION_CARRITO];
            }
            set
            {
                Session[SESSION_CARRITO] = value;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientesDropdown();
                CargarProductosDropdown();
                ActualizarVistaCarrito();
                CargarHistorialPedidos();
            }
        }

        private void CargarClientesDropdown()
        {
            try
            {
                var clientes = _clienteService.ListarTodos().ToList();
                ddlClientes.DataSource = clientes;
                ddlClientes.DataTextField = "Nombre";
                ddlClientes.DataValueField = "ClienteId";
                ddlClientes.DataBind();

                ddlClientes.Items.Insert(0, new ListItem("-- Seleccione un Cliente --", "0"));
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar clientes: {ex.Message}", "danger");
            }
        }

        private void CargarProductosDropdown()
        {
            try
            {
                var productos = _productoService.ListarInventario().ToList();
                ddlProductos.DataSource = productos;
                ddlProductos.DataTextField = "Nombre";
                ddlProductos.DataValueField = "ProductoId";
                ddlProductos.DataBind();

                ddlProductos.Items.Insert(0, new ListItem("-- Seleccione un Producto --", "0"));
                txtPrecioProducto.Text = string.Empty;
                txtStockDisponible.Text = string.Empty;
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar productos: {ex.Message}", "danger");
            }
        }

        protected void ddlProductos_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (int.TryParse(ddlProductos.SelectedValue, out int productoId) && productoId > 0)
            {
                var producto = _productoService.ObtenerPorId(productoId);
                if (producto != null)
                {
                    txtPrecioProducto.Text = producto.Precio.ToString("N2");
                    txtStockDisponible.Text = producto.Stock.ToString();
                }
            }
            else
            {
                txtPrecioProducto.Text = string.Empty;
                txtStockDisponible.Text = string.Empty;
            }
        }

        protected void btnAgregarCarrito_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(ddlProductos.SelectedValue, out int productoId) || productoId <= 0)
                {
                    MostrarMensaje("Seleccione un producto válido de la lista.", "warning");
                    return;
                }

                if (!int.TryParse(txtCantidad.Text.Trim(), out int cantidad) || cantidad <= 0)
                {
                    MostrarMensaje("La cantidad solicitada debe ser un número entero mayor a 0.", "warning");
                    return;
                }

                var producto = _productoService.ObtenerPorId(productoId);
                if (producto == null)
                {
                    MostrarMensaje("El producto seleccionado no existe.", "danger");
                    return;
                }

                // Verificar acumulado en carrito vs stock de la base de datos
                var itemExistente = Carrito.FirstOrDefault(c => c.ProductoId == productoId);
                int cantidadTotalDeseada = (itemExistente != null ? itemExistente.Cantidad : 0) + cantidad;

                if (cantidadTotalDeseada > producto.Stock)
                {
                    MostrarMensaje($"No se puede agregar esa cantidad. Stock disponible: {producto.Stock}. Ya tiene { (itemExistente != null ? itemExistente.Cantidad : 0) } en el carrito.", "warning");
                    return;
                }

                if (itemExistente != null)
                {
                    itemExistente.Cantidad += cantidad;
                }
                else
                {
                    Carrito.Add(new CarritoItemViewModel
                    {
                        ProductoId = producto.ProductoId,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        Cantidad = cantidad
                    });
                }

                ActualizarVistaCarrito();
                MostrarMensaje($"Producto '{producto.Nombre}' agregado al carrito correctamente.", "info");
                txtCantidad.Text = "1";
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al agregar producto al carrito: {ex.Message}", "danger");
            }
        }

        protected void gvCarrito_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "EliminarItem")
            {
                int productoId = Convert.ToInt32(e.CommandArgument);
                var lista = Carrito;
                lista.RemoveAll(i => i.ProductoId == productoId);
                Carrito = lista;
                ActualizarVistaCarrito();
                MostrarMensaje("Producto retirado del carrito.", "info");
            }
        }

        protected void btnLimpiarCarrito_Click(object sender, EventArgs e)
        {
            Carrito.Clear();
            ActualizarVistaCarrito();
            MostrarMensaje("El carrito de compras ha sido vaciado.", "secondary");
        }

        private void ActualizarVistaCarrito()
        {
            gvCarrito.DataSource = Carrito;
            gvCarrito.DataBind();

            decimal total = Carrito.Sum(c => c.Subtotal);
            litTotalCarrito.Text = total.ToString("C2");
        }

        protected void btnConfirmarPedido_Click(object sender, EventArgs e)
        {
            try
            {
                if (!int.TryParse(ddlClientes.SelectedValue, out int clienteId) || clienteId <= 0)
                {
                    MostrarMensaje("Debe seleccionar un cliente antes de registrar el pedido.", "warning");
                    return;
                }

                if (Carrito.Count == 0)
                {
                    MostrarMensaje("El carrito está vacío. Agregue productos antes de procesar el pedido.", "warning");
                    return;
                }

                // Construcción de DTO Request para enviar a la capa de Aplicación
                var request = new PedidoRequest
                {
                    ClienteId = clienteId,
                    Items = Carrito.Select(c => c.ToDto()).ToList()
                };

                // Invocación del Servicio de Aplicación
                int nuevoPedidoId = _pedidoService.Registrar(request);

                // Limpieza tras confirmación exitosa
                Carrito.Clear();
                ActualizarVistaCarrito();
                ddlClientes.SelectedIndex = 0;
                CargarProductosDropdown();
                CargarHistorialPedidos();

                MostrarMensaje($"¡Pedido #{nuevoPedidoId} registrado con éxito! El inventario ha sido actualizado bajo transacción.", "success");
            }
            catch (DomainException ex)
            {
                MostrarMensaje($"Regla de Negocio no satisfecha: {ex.Message}", "warning");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al procesar el pedido (Transacción revertida): {ex.Message}", "danger");
            }
        }

        protected void btnRefrescarHistorial_Click(object sender, EventArgs e)
        {
            CargarHistorialPedidos();
        }

        private void CargarHistorialPedidos()
        {
            try
            {
                var pedidos = _pedidoService.ListarTodos();
                gvHistorialPedidos.DataSource = pedidos;
                gvHistorialPedidos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar historial: {ex.Message}", "danger");
            }
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show";
            litMensaje.Text = mensaje;
        }
    }
}
