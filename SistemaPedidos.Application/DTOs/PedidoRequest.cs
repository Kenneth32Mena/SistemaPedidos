using System.Collections.Generic;

namespace SistemaPedidos.Application.DTOs
{
    public class PedidoRequest
    {
        public int ClienteId { get; set; }
        public List<ItemPedidoDto> Items { get; set; }

        public PedidoRequest()
        {
            Items = new List<ItemPedidoDto>();
        }
    }
}
