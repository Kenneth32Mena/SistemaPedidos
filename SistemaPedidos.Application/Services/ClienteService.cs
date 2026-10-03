using System;
using System.Collections.Generic;
using SistemaPedidos.Application.Interfaces;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Domain.Interfaces;

namespace SistemaPedidos.Application.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository ?? throw new ArgumentNullException(nameof(clienteRepository));
        }

        public void RegistrarCliente(string nombre, string correo, string telefono)
        {
            var cliente = new Cliente
            {
                Nombre = nombre,
                Correo = correo,
                Telefono = telefono
            };

            cliente.Validar();
            _clienteRepository.Insertar(cliente);
        }

        public Cliente ObtenerPorId(int clienteId)
        {
            return _clienteRepository.ObtenerPorId(clienteId);
        }

        public IEnumerable<Cliente> ListarTodos()
        {
            return _clienteRepository.ObtenerTodos();
        }
    }
}
