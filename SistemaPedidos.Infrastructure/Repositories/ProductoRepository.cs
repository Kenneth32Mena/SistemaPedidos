using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Interfaces;
using SistemaPedidos.Infrastructure.Data;

namespace SistemaPedidos.Infrastructure.Repositories
{
    public class ProductoRepository : IProductoRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public ProductoRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public void Insertar(Producto producto)
        {
            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = @"INSERT INTO Productos (Nombre, Precio, Stock, FechaRegistro) 
                                 VALUES (@Nombre, @Precio, @Stock, @FechaRegistro);
                                 SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Nombre", producto.Nombre);
                    command.Parameters.AddWithValue("@Precio", producto.Precio);
                    command.Parameters.AddWithValue("@Stock", producto.Stock);
                    command.Parameters.AddWithValue("@FechaRegistro", producto.FechaRegistro);

                    connection.Open();
                    producto.ProductoId = Convert.ToInt32(command.ExecuteScalar());
                }
            }
        }

        public Producto ObtenerPorId(int productoId)
        {
            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = "SELECT ProductoId, Nombre, Precio, Stock, FechaRegistro FROM Productos WHERE ProductoId = @ProductoId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ProductoId", productoId);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Producto
                            {
                                ProductoId = reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                            };
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<Producto> ObtenerTodos()
        {
            var productos = new List<Producto>();

            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = "SELECT ProductoId, Nombre, Precio, Stock, FechaRegistro FROM Productos ORDER BY Nombre ASC";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            productos.Add(new Producto
                            {
                                ProductoId = reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                Nombre = reader.GetString(reader.GetOrdinal("Nombre")),
                                Precio = reader.GetDecimal(reader.GetOrdinal("Precio")),
                                Stock = reader.GetInt32(reader.GetOrdinal("Stock")),
                                FechaRegistro = reader.GetDateTime(reader.GetOrdinal("FechaRegistro"))
                            });
                        }
                    }
                }
            }
            return productos;
        }

        public void ActualizarStock(int productoId, int cantidadADescontar)
        {
            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string query = "UPDATE Productos SET Stock = Stock - @Cantidad WHERE ProductoId = @ProductoId";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Cantidad", cantidadADescontar);
                    command.Parameters.AddWithValue("@ProductoId", productoId);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
