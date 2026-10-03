using System.Collections.Generic;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Application.Interfaces
{
    public interface IClienteService
    {
        void RegistrarCliente(string nombre, string correo, string telefono);
        Cliente ObtenerPorId(int clienteId);
        IEnumerable<Cliente> ListarTodos();
    }
}
