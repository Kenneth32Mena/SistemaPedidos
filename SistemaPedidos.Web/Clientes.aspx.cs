using System;
using System.Web.UI;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Web.Composition;

namespace SistemaPedidos.Web
{
    public partial class Clientes : Page
    {
        private readonly IClienteService _clienteService;

        public Clientes()
        {
            _clienteService = ServiceFactory.CreateClienteService();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CargarClientes();
            }
        }

        private void CargarClientes()
        {
            try
            {
                var clientes = _clienteService.ListarTodos();
                gvClientes.DataSource = clientes;
                gvClientes.DataBind();
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al cargar los clientes: {ex.Message}", "danger");
            }
        }

        protected void btnGuardarCliente_Click(object sender, EventArgs e)
        {
            try
            {
                string nombre = txtNombre.Text.Trim();
                string correo = txtCorreo.Text.Trim();
                string telefono = txtTelefono.Text.Trim();

                _clienteService.RegistrarCliente(nombre, correo, telefono);

                MostrarMensaje("¡Cliente registrado exitosamente!", "success");
                LimpiarFormulario();
                CargarClientes();
            }
            catch (DomainException ex)
            {
                MostrarMensaje(ex.Message, "warning");
            }
            catch (Exception ex)
            {
                MostrarMensaje($"Error al guardar cliente: {ex.Message}", "danger");
            }
        }

        protected void btnRefrescar_Click(object sender, EventArgs e)
        {
            CargarClientes();
        }

        private void LimpiarFormulario()
        {
            txtNombre.Text = string.Empty;
            txtCorreo.Text = string.Empty;
            txtTelefono.Text = string.Empty;
        }

        private void MostrarMensaje(string mensaje, string tipo)
        {
            pnlMensaje.Visible = true;
            pnlMensaje.CssClass = $"alert alert-{tipo} alert-dismissible fade show";
            litMensaje.Text = mensaje;
        }
    }
}
