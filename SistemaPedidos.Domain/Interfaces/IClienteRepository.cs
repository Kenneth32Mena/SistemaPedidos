using System.Collections.Generic;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IClienteRepository
    {
        void Insertar(Cliente cliente);
        Cliente ObtenerPorId(int clienteId);
        IEnumerable<Cliente> ObtenerTodos();
    }
}
