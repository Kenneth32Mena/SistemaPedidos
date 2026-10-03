using System.Collections.Generic;
using SistemaPedidos.Domain.Entities;

namespace SistemaPedidos.Application.Interfaces
{
    public interface IProductoService
    {
        void RegistrarProducto(string nombre, decimal precio, int stock);
        Producto ObtenerPorId(int productoId);
        IEnumerable<Producto> ListarInventario();
    }
}
