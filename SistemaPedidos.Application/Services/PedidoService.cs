using System;
using System.Collections.Generic;
using SistemaPedidos.Application.DTOs;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Domain.Interfaces;

namespace SistemaPedidos.Application.Services
{
    public class PedidoService : IPedidoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IPedidoRepository _pedidoRepository;
        private readonly IClienteRepository _clienteRepository;

        public PedidoService(
            IProductoRepository productoRepository, 
            IPedidoRepository pedidoRepository,
            IClienteRepository clienteRepository)
        {
            _productoRepository = productoRepository ?? throw new ArgumentNullException(nameof(productoRepository));
            _pedidoRepository = pedidoRepository ?? throw new ArgumentNullException(nameof(pedidoRepository));
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        }

        public int Registrar(PedidoRequest request)
        {
            if (request == null)
                throw new DomainException("La solicitud de pedido no puede ser nula.");

            if (request.ClienteId <= 0)
                throw new DomainException("Debe seleccionar un cliente válido.");

            var cliente = _clienteRepository.ObtenerPorId(request.ClienteId);
            if (cliente == null)
                throw new DomainException($"El cliente con ID {request.ClienteId} no existe.");

            if (request.Items == null || request.Items.Count == 0)
                throw new DomainException("No se puede registrar un pedido sin productos.");

            var pedido = new Pedido(request.ClienteId);

            foreach (var item in request.Items)
            {
                var producto = _productoRepository.ObtenerPorId(item.ProductoId);
                if (producto == null)
                    throw new DomainException($"El producto con ID {item.ProductoId} no existe en el catálogo.");

                pedido.AgregarDetalle(producto, item.Cantidad);
            }

            pedido.Validar();

            // Persistencia transaccional mediante el repositorio
            int pedidoId = _pedidoRepository.RegistrarPedidoConTransaccion(pedido);
            return pedidoId;
        }

        public Pedido ObtenerPorId(int pedidoId)
        {
            return _pedidoRepository.ObtenerPorId(pedidoId);
        }

        public IEnumerable<Pedido> ListarTodos()
        {
            return _pedidoRepository.ObtenerTodos();
        }
    }
}
