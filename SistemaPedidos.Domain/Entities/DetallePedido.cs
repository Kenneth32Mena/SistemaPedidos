using System;

namespace SistemaPedidos.Domain.Entities
{
    public class DetallePedido
    {
        public int DetalleId { get; set; }
        public int PedidoId { get; set; }
        public int ProductoId { get; set; }
        public Producto Producto { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal => Cantidad * PrecioUnitario;

        public DetallePedido()
        {
        }

        public DetallePedido(int productoId, int cantidad, decimal precioUnitario, Producto producto = null)
        {
            ProductoId = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            Producto = producto;
            Validar();
        }

        public void Validar()
        {
            if (ProductoId <= 0)
                throw new Exceptions.DomainException("Debe especificarse un producto válido.");

            if (Cantidad <= 0)
                throw new Exceptions.DomainException("La cantidad solicitada debe ser mayor que cero.");

            if (PrecioUnitario <= 0)
                throw new Exceptions.DomainException("El precio unitario debe ser mayor que cero.");
        }
    }
}
