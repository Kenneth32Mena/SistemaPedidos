using System;

namespace SistemaPedidos.Domain.Entities
{
    public class Cliente
    {
        public int ClienteId { get; set; }
        public string Nombre { get; set; }
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public DateTime FechaRegistro { get; set; }

        public Cliente()
        {
            FechaRegistro = DateTime.Now;
        }

        public Cliente(int clienteId, string nombre, string correo, string telefono)
        {
            ClienteId = clienteId;
            Nombre = nombre;
            Correo = correo;
            Telefono = telefono;
            FechaRegistro = DateTime.Now;
        }

        public void Validar()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                throw new Exceptions.DomainException("El nombre del cliente es obligatorio.");

            if (string.IsNullOrWhiteSpace(Correo))
                throw new Exceptions.DomainException("El correo del cliente es obligatorio.");
        }
    }
}
