using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using SistemaPedidos.Domain.Entities;
using SistemaPedidos.Domain.Exceptions;
using SistemaPedidos.Domain.Interfaces;
using SistemaPedidos.Infrastructure.Data;

namespace SistemaPedidos.Infrastructure.Repositories
{
    public class PedidoRepository : IPedidoRepository
    {
        private readonly SqlConnectionFactory _connectionFactory;

        public PedidoRepository(SqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory ?? throw new ArgumentNullException(nameof(connectionFactory));
        }

        public int RegistrarPedidoConTransaccion(Pedido pedido)
        {
            if (pedido == null)
                throw new ArgumentNullException(nameof(pedido));

            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction(IsolationLevel.ReadCommitted))
                {
                    try
                    {
                        // 1. Validar existencias actuales de cada producto con bloqueo
                        foreach (var detalle in pedido.Detalles)
                        {
                            string sqlCheckStock = "SELECT Stock, Nombre, Precio FROM Productos WITH (UPDLOCK, ROWLOCK) WHERE ProductoId = @ProductoId";
                            using (var cmdStock = new SqlCommand(sqlCheckStock, connection, transaction))
                            {
                                cmdStock.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                                using (var reader = cmdStock.ExecuteReader())
                                {
                                    if (!reader.Read())
                                    {
                                        throw new DomainException($"El producto con ID {detalle.ProductoId} no existe en la base de datos.");
                                    }

                                    int currentStock = reader.GetInt32(reader.GetOrdinal("Stock"));
                                    string nombre = reader.GetString(reader.GetOrdinal("Nombre"));

                                    if (currentStock < detalle.Cantidad)
                                    {
                                        throw new DomainException($"Stock insuficiente en base de datos para el producto '{nombre}'. Stock disponible: {currentStock}, requerido: {detalle.Cantidad}.");
                                    }
                                }
                            }
                        }

                        // 2. INSERT Pedido (Cabecera)
                        string sqlInsertPedido = @"INSERT INTO Pedidos (ClienteId, Fecha, Total)
                                                   VALUES (@ClienteId, @Fecha, @Total);
                                                   SELECT SCOPE_IDENTITY();";

                        int nuevoPedidoId;
                        using (var cmdPedido = new SqlCommand(sqlInsertPedido, connection, transaction))
                        {
                            cmdPedido.Parameters.AddWithValue("@ClienteId", pedido.ClienteId);
                            cmdPedido.Parameters.AddWithValue("@Fecha", pedido.Fecha);
                            cmdPedido.Parameters.AddWithValue("@Total", pedido.Total);

                            nuevoPedidoId = Convert.ToInt32(cmdPedido.ExecuteScalar());
                            pedido.PedidoId = nuevoPedidoId;
                        }

                        // 3. INSERT DetallePedido y UPDATE Stock por cada producto
                        foreach (var detalle in pedido.Detalles)
                        {
                            string sqlInsertDetalle = @"INSERT INTO DetallePedidos (PedidoId, ProductoId, Cantidad, PrecioUnitario, Subtotal)
                                                        VALUES (@PedidoId, @ProductoId, @Cantidad, @PrecioUnitario, @Subtotal);";

                            using (var cmdDetalle = new SqlCommand(sqlInsertDetalle, connection, transaction))
                            {
                                cmdDetalle.Parameters.AddWithValue("@PedidoId", nuevoPedidoId);
                                cmdDetalle.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);
                                cmdDetalle.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                cmdDetalle.Parameters.AddWithValue("@PrecioUnitario", detalle.PrecioUnitario);
                                cmdDetalle.Parameters.AddWithValue("@Subtotal", detalle.Subtotal);

                                cmdDetalle.ExecuteNonQuery();
                            }

                            string sqlUpdateStock = @"UPDATE Productos 
                                                      SET Stock = Stock - @Cantidad 
                                                      WHERE ProductoId = @ProductoId";

                            using (var cmdStockUpdate = new SqlCommand(sqlUpdateStock, connection, transaction))
                            {
                                cmdStockUpdate.Parameters.AddWithValue("@Cantidad", detalle.Cantidad);
                                cmdStockUpdate.Parameters.AddWithValue("@ProductoId", detalle.ProductoId);

                                int rowsAffected = cmdStockUpdate.ExecuteNonQuery();
                                if (rowsAffected == 0)
                                {
                                    throw new DomainException($"No se pudo actualizar el stock del producto con ID {detalle.ProductoId}.");
                                }
                            }
                        }

                        // 4. Confirmar Transacción (COMMIT)
                        transaction.Commit();
                        return nuevoPedidoId;
                    }
                    catch (Exception)
                    {
                        // En caso de cualquier error se revierte la transacción completa (ROLLBACK)
                        try
                        {
                            transaction.Rollback();
                        }
                        catch
                        {
                            // Error al hacer rollback si la conexión fue cerrada
                        }
                        throw;
                    }
                }
            }
        }

        public Pedido ObtenerPorId(int pedidoId)
        {
            Pedido pedido = null;

            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string sqlPedido = @"SELECT P.PedidoId, P.ClienteId, P.Fecha, P.Total,
                                            C.Nombre AS ClienteNombre, C.Correo AS ClienteCorreo, C.Telefono AS ClienteTelefono
                                     FROM Pedidos P
                                     INNER JOIN Clientes C ON P.ClienteId = C.ClienteId
                                     WHERE P.PedidoId = @PedidoId";

                connection.Open();
                using (var cmd = new SqlCommand(sqlPedido, connection))
                {
                    cmd.Parameters.AddWithValue("@PedidoId", pedidoId);
                    using (var reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            pedido = new Pedido
                            {
                                PedidoId = reader.GetInt32(reader.GetOrdinal("PedidoId")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Cliente = new Cliente
                                {
                                    ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                    Nombre = reader.GetString(reader.GetOrdinal("ClienteNombre")),
                                    Correo = reader.GetString(reader.GetOrdinal("ClienteCorreo")),
                                    Telefono = reader.IsDBNull(reader.GetOrdinal("ClienteTelefono")) ? null : reader.GetString(reader.GetOrdinal("ClienteTelefono"))
                                },
                                Detalles = new List<DetallePedido>()
                            };
                        }
                    }
                }

                if (pedido != null)
                {
                    string sqlDetalles = @"SELECT D.DetalleId, D.PedidoId, D.ProductoId, D.Cantidad, D.PrecioUnitario, D.Subtotal,
                                                  PR.Nombre AS ProductoNombre
                                           FROM DetallePedidos D
                                           INNER JOIN Productos PR ON D.ProductoId = PR.ProductoId
                                           WHERE D.PedidoId = @PedidoId";

                    using (var cmdDetalle = new SqlCommand(sqlDetalles, connection))
                    {
                        cmdDetalle.Parameters.AddWithValue("@PedidoId", pedidoId);
                        using (var reader = cmdDetalle.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                pedido.Detalles.Add(new DetallePedido
                                {
                                    DetalleId = reader.GetInt32(reader.GetOrdinal("DetalleId")),
                                    PedidoId = reader.GetInt32(reader.GetOrdinal("PedidoId")),
                                    ProductoId = reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                    Cantidad = reader.GetInt32(reader.GetOrdinal("Cantidad")),
                                    PrecioUnitario = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario")),
                                    Producto = new Producto
                                    {
                                        ProductoId = reader.GetInt32(reader.GetOrdinal("ProductoId")),
                                        Nombre = reader.GetString(reader.GetOrdinal("ProductoNombre")),
                                        Precio = reader.GetDecimal(reader.GetOrdinal("PrecioUnitario"))
                                    }
                                });
                            }
                        }
                    }
                }
            }

            return pedido;
        }

        public IEnumerable<Pedido> ObtenerTodos()
        {
            var pedidos = new List<Pedido>();

            using (var connection = _connectionFactory.CreateSqlConnection())
            {
                string sql = @"SELECT P.PedidoId, P.ClienteId, P.Fecha, P.Total,
                                      C.Nombre AS ClienteNombre, C.Correo AS ClienteCorreo, C.Telefono AS ClienteTelefono
                               FROM Pedidos P
                               INNER JOIN Clientes C ON P.ClienteId = C.ClienteId
                               ORDER BY P.PedidoId DESC";

                using (var cmd = new SqlCommand(sql, connection))
                {
                    connection.Open();
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            pedidos.Add(new Pedido
                            {
                                PedidoId = reader.GetInt32(reader.GetOrdinal("PedidoId")),
                                ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                Fecha = reader.GetDateTime(reader.GetOrdinal("Fecha")),
                                Cliente = new Cliente
                                {
                                    ClienteId = reader.GetInt32(reader.GetOrdinal("ClienteId")),
                                    Nombre = reader.GetString(reader.GetOrdinal("ClienteNombre")),
                                    Correo = reader.GetString(reader.GetOrdinal("ClienteCorreo")),
                                    Telefono = reader.IsDBNull(reader.GetOrdinal("ClienteTelefono")) ? null : reader.GetString(reader.GetOrdinal("ClienteTelefono"))
                                },
                                Detalles = new List<DetallePedido>()
                            });
                        }
                    }
                }
            }

            return pedidos;
        }
    }
}
