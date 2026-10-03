using System;
using System.Collections.Generic;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Domain.Interfaces;

namespace SistemaPedidos.Application.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;

        public ProductoService(IProductoRepository productoRepository)
        {
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
        }

        public void RegistrarProducto(string nombre, decimal precio, int stock)
        {
            var producto = new Producto
            {
                Nombre = nombre,
                Precio = precio,
                Stock = stock
            };

            producto.Validar();
            _productoRepository.Insertar(producto);
        }

        public Producto ObtenerPorId(int productoId)
        {
            return _productoRepository.ObtenerPorId(productoId);
        }

        public IEnumerable<Producto> ListarInventario()
        {
            return _productoRepository.ObtenerTodos();
        }
    }
}
