using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Application.Services;
using SistemaPedidos.Domain.Interfaces;
using SistemaPedidos.Infrastructure.Data;
using SistemaPedidos.Infrastructure.Repositories;

namespace SistemaPedidos.Web.Composition
{
    /// <summary>
    /// Composition Root: Punto centralizado donde se configuran y resuelven las dependencias
    /// demostrando el Principio de Inversión de Dependencias (DIP) y Responsabilidad Única (SRP).
    /// </summary>
    public static class ServiceFactory
    {
        private static readonly SqlConnectionFactory ConnectionFactory = new SqlConnectionFactory();

        public static IClienteRepository CreateClienteRepository()
        {
            return new ClienteRepository(ConnectionFactory);
        }

        public static IProductoRepository CreateProductoRepository()
        {
            return new ProductoRepository(ConnectionFactory);
        }

        public static IPedidoRepository CreatePedidoRepository()
        {
            return new PedidoRepository(ConnectionFactory);
        }

        public static IClienteService CreateClienteService()
        {
            return new ClienteService(CreateClienteRepository());
        }

        public static IProductoService CreateProductoService()
        {
            return new ProductoService(CreateProductoRepository());
        }

        public static IPedidoService CreatePedidoService()
        {
            return new PedidoService(
                CreateProductoRepository(),
                CreatePedidoRepository(),
                CreateClienteRepository()
            );
        }
    }
}
