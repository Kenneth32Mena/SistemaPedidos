using System;
using SistemaPedidos.Application.DTOs;

namespace SistemaPedidos.Web.Models
{
    [Serializable]
    public class CarritoItemViewModel
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; }
        public decimal Precio { get; set; }
        public int Cantidad { get; set; }
        public decimal Subtotal => Cantidad * Precio;

        public ItemPedidoDto ToDto()
        {
            return new ItemPedidoDto
            {
                ProductoId = this.ProductoId,
                NombreProducto = this.Nombre,
                PrecioUnitario = this.Precio,
                Cantidad = this.Cantidad
            };
        }
    }
}
