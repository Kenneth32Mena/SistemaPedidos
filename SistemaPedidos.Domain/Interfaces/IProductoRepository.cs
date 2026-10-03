using System.Collections.Generic;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Domain.Interfaces
{
    public interface IProductoRepository
    {
        void Insertar(Producto producto);
        Producto ObtenerPorId(int productoId);
        IEnumerable<Producto> ObtenerTodos();
        void ActualizarStock(int productoId, int cantidadADescontar);
    }
}
