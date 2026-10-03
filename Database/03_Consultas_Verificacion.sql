-- =========================================================================
-- ARCHIVO 03: CONSULTAS DE VERIFICACIÓN DE ESTADO Y TRANSACCIONES
-- =========================================================================

USE ArquitecturaCapasPedidosDB;
GO

-- 1. Consultar todos los Clientes
SELECT ClienteId, Nombre, Correo, Telefono, FechaRegistro 
FROM Clientes;

-- 2. Consultar catálogo de Productos y disponibilidad de Stock
SELECT ProductoId, Nombre, Precio, Stock, FechaRegistro 
FROM Productos;

-- 3. Consultar historial de Pedidos con información del Cliente
SELECT 
    P.PedidoId,
    P.Fecha,
    C.Nombre AS Cliente,
    C.Correo,
    P.Total
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteId = C.ClienteId
ORDER BY P.Fecha DESC;

-- 4. Consultar detalle consolidado de pedidos con productos
SELECT 
    P.PedidoId,
    P.Fecha,
    C.Nombre AS Cliente,
    PR.Nombre AS Producto,
    DP.Cantidad,
    DP.PrecioUnitario,
    DP.Subtotal,
    P.Total AS TotalPedido
FROM Pedidos P
INNER JOIN Clientes C ON P.ClienteId = C.ClienteId
INNER JOIN DetallePedidos DP ON P.PedidoId = DP.PedidoId
INNER JOIN Productos PR ON DP.ProductoId = PR.ProductoId
ORDER BY P.PedidoId DESC, DP.DetalleId ASC;
GO
