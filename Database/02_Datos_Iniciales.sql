-- =========================================================================
-- ARCHIVO 02: INSERCIÓN DE DATOS INICIALES DE PRUEBA
-- =========================================================================

USE ArquitecturaCapasPedidosDB;
GO

-- Inserción de Clientes
IF NOT EXISTS (SELECT 1 FROM Clientes)
BEGIN
    INSERT INTO Clientes (Nombre, Correo, Telefono) VALUES
    ('Juan Perez', 'juan.perez@example.com', '555-1234'),
    ('Maria Lopez', 'maria.lopez@example.com', '555-5678'),
    ('Carlos Sanchez', 'carlos.sanchez@example.com', '555-9012'),
    ('Ana Martinez', 'ana.martinez@example.com', '555-3456');
END
GO

-- Inserción de Productos
IF NOT EXISTS (SELECT 1 FROM Productos)
BEGIN
    INSERT INTO Productos (Nombre, Precio, Stock) VALUES
    ('Laptop Lenovo ThinkPad', 850.00, 15),
    ('Monitor Dell 27 4K', 320.50, 20),
    ('Teclado Mecanico RGB', 75.00, 35),
    ('Mouse Inalambrico Logitech', 45.00, 50),
    ('Auriculares HyperX Cloud II', 95.00, 10),
    ('Disco Solido SSD 1TB NVMe', 110.00, 25);
END
GO
