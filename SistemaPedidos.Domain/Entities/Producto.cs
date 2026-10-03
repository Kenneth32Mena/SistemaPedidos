using System;

namespace SistemaPedidos.Domain.Entities
{
    public class Producto
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Producto()
        {
            FechaRegistro = DateTime.Now;
        }

        public Producto(int productoId, string nombre, decimal precio, int stock)
        {
            ProductoId = productoId;
            Nombre = nombre;
            Precio = precio;
            Stock = stock;
            FechaRegistro = DateTime.Now;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new Exceptions.DomainException("El nombre del producto es obligatorio.");

            if (Precio <= 0)
                throw new Exceptions.DomainException("El precio del producto debe ser mayor que cero.");

            if (Stock < 0)
                throw new Exceptions.DomainException("El stock no puede ser negativo.");
        }

        public void DescontarStock(int cantidad)
        {
            if (cantidad <= 0)
                throw new Exceptions.DomainException("La cantidad a descontar debe ser mayor que cero.");

            if (Stock < cantidad)
                throw new Exceptions.DomainException($"Stock insuficiente para el producto '{Nombre}'. Disponible: {Stock}, Solicitado: {cantidad}.");

            Stock -= cantidad;
        }
    }
}
