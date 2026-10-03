using System.Collections.Generic;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IPedidoRepository
    {
        int RegistrarPedidoConTransaccion(Pedido pedido);
        Pedido ObtenerPorId(int pedidoId);
        IEnumerable<Pedido> ObtenerTodos();
    }
}
