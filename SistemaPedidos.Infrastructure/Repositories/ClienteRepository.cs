using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using SistemaPedidos.Infrastructure.Data;

namespace SistemaPedidos.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ClienteRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public void Insertar(Cliente cliente)
        {
            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = @"INSERT INTO Clientes (Nombre, Correo, Telefono, FechaRegistro) 
                                 VALUES (@Nombre, @Correo, @Telefono, @FechaRegistro);
                                 SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", cliente.Nombre);
                    command.Parameters.AddWithValue("@Correo", cliente.Correo);
                    command.Parameters.AddWithValue("@Telefono", (object)cliente.Telefono ?? DBNull.Value);
                    command.Parameters.AddWithValue("@FechaRegistro", cliente.FechaRegistro);

                    connection.Open();
                    cliente.ClienteId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public Cliente ObtenerPorId(int clienteId)
        {
            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = "SELECT ClienteId, Nombre, Correo, Telefono, FechaRegistro FROM Clientes WHERE ClienteId = @ClienteId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ClienteId", clienteId);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Cliente
                            {
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Cliente> ObtenerTodos()
        {
            var clientes = new List<Cliente>();

            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = "SELECT ClienteId, Nombre, Correo, Telefono, FechaRegistro FROM Clientes ORDER BY Nombre ASC";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            clientes.Add(new Cliente
                            {
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Correo = reader.GetString(reader.GetOrdinal("Correo")),
                                Telefono = reader.IsDBNull(reader.GetOrdinal("Telefono")) ? null : reader.GetString(reader.GetOrdinal("Telefono")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                            });
                        }
                    }
                }
            }
            return clientes;
        }
    }
}
