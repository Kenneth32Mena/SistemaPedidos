using System;
using System.Web.UI;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Web.Composition;

namespace SistemaPedidos.Web
{
    public partial class Productos : Page
    {
        private readonly IProductoService _productoService;

        public Productos()
        {
            _productoService = ServiceFactory.CreateProductoService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarProductos();
            }
        }

        private void CargarProductos()
        {
            try
            {
                var productos = _productoService.ListarInventario();
                gvProductos.DataSource = productos;
                gvProductos.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar el inventario: {ex.Message}", "danger");
            }
        }

        protected void btnGuardarProducto_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                if (!decimal.TryParse(txtPrecio.Text.Trim(), out decimal precio))
                {
                    MostrarMensaje("Ingrese un precio numérico válido.", "warning");
                    return;
                }

                if (!int.TryParse(txtStock.Text.Trim(), out int stock))
                {
                    MostrarMensaje("Ingrese un stock numérico entero válido.", "warning");
                    return;
                }

                _productoService.RegistrarProducto(nombre, precio, stock);

                MostrarMensaje("¡Producto registrado exitosamente en el inventario!", "success");
                LimpiarFormulario();
                CargarProductos();
            }
            catch (DomainException ex)
            {
                MostrarMensaje(ex.Message, "warning");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al registrar producto: {ex.Message}", "danger");
            }
        }

        protected void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarProductos();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtPrecio.Text = string.Empty;
            txtStock.Text = string.Empty;
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show";
            litMensaje.Text = mensaje;
        }
    }
}
