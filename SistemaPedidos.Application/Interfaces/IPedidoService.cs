using System.Collections.Generic;
using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Application.Interfaces
{
    public interface IPedidoService
    {
        int Registrar(PedidoRequest request);
        Pedido ObtenerPorId(int pedidoId);
        IEnumerable<Pedido> ListarTodos();
    }
}
