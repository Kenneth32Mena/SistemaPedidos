using System;
using System.Collections.Generic;
using System.Linq;

namespace SistemaPedidos.Domain.Entities
{
    public class Pedido
    {
        public int PedidoId { get; set; }
        public int ClienteId { get; set; }
        public Cliente Cliente { get; set; }
        public DateTime Fecha { get; set; }
        public List<DetallePedido> Detalles { get; set; }

        public decimal Total => Detalles != null ? Detalles.Sum(d => d.Subtotal) : 0;

        public Pedido()
        {
            Fecha = DateTime.Now;
            Detalles = new List<DetallePedido>();
        }

        public Pedido(int clienteId) : this()
        {
            ClienteId = clienteId;
        }

        public void AgregarDetalle(Producto producto, int cantidad)
        {
            if (producto == null)
                throw new Exceptions.DomainException("El producto no existe o no es válido.");

            if (cantidad <= 0)
                throw new Exceptions.DomainException("La cantidad debe ser mayor a cero.");

            if (producto.Stock < cantidad)
                throw new Exceptions.DomainException($"No hay stock suficiente para '{producto.Nombre}'. Stock actual: {producto.Stock}, Solicitado: {cantidad}.");

            var existente = Detalles.FirstOrDefault(d => d.ProductoId == producto.ProductoId);
            if (existente != null)
            {
                int nuevaCantidad = existente.Cantidad + cantidad;
                if (producto.Stock < nuevaCantidad)
                    throw new Exceptions.DomainException($"Stock insuficiente para '{producto.Nombre}'. Ya agregó {existente.Cantidad} y el stock total es {producto.Stock}.");
                existente.Cantidad = nuevaCantidad;
            }
            else
            {
                Detalles.Add(new DetallePedido
                {
                    ProductoId = producto.ProductoId,
                    Producto = producto,
                    Cantidad = cantidad,
                    PrecioUnitario = producto.Precio
                });
            }
        }

        public void Validar()
        {
            if (ClienteId <= 0)
                throw new Exceptions.DomainException("Debe seleccionar un cliente válido para el pedido.");

            if (Detalles == null || !Detalles.Any())
                throw new Exceptions.DomainException("No se puede registrar un pedido sin productos.");

            foreach (var detalle in Detalles)
            {
                detalle.Validar();
            }

            if (Total <= 0)
                throw new Exceptions.DomainException("El total del pedido debe ser mayor a cero.");
        }
    }
}
