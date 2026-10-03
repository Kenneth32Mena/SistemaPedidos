-- =========================================================================
-- ARCHIVO 01: CREACIÓN DE BASE DE DATOS Y TABLAS
-- BASE DE DATOS: ArquitecturaCapasPedidosDB
-- =========================================================================

IF NOT EXISTS (SELECT * FROM sys.databases WHERE name = 'ArquitecturaCapasPedidosDB')
BEGIN
    CREATE DATABASE ArquitecturaCapasPedidosDB;
END
GO

USE ArquitecturaCapasPedidosDB;
GO

-- 1. Tabla Clientes
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Clientes')
BEGIN
    CREATE TABLE Clientes (
        ClienteId INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Correo NVARCHAR(150) NOT NULL,
        Telefono NVARCHAR(20) NULL,
        FechaRegistro DATETIME DEFAULT GETDATE()
    );
END
GO

-- 2. Tabla Productos
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Productos')
BEGIN
    CREATE TABLE Productos (
        ProductoId INT IDENTITY(1,1) PRIMARY KEY,
        Nombre NVARCHAR(100) NOT NULL,
        Precio DECIMAL(10,2) NOT NULL,
        Stock INT NOT NULL,
        FechaRegistro DATETIME DEFAULT GETDATE()
    );
END
GO

-- 3. Tabla Pedidos (Cabecera)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'Pedidos')
BEGIN
    CREATE TABLE Pedidos (
        PedidoId INT IDENTITY(1,1) PRIMARY KEY,
        ClienteId INT NOT NULL,
        Fecha DATETIME DEFAULT GETDATE(),
        Total DECIMAL(10,2) NOT NULL,
        CONSTRAINT FK_Pedidos_Clientes FOREIGN KEY (ClienteId) REFERENCES Clientes(ClienteId)
    );
END
GO

-- 4. Tabla DetallePedidos (Detalle)
IF NOT EXISTS (SELECT * FROM sys.tables WHERE name = 'DetallePedidos')
BEGIN
    CREATE TABLE DetallePedidos (
        DetalleId INT IDENTITY(1,1) PRIMARY KEY,
        PedidoId INT NOT NULL,
        ProductoId INT NOT NULL,
        Cantidad INT NOT NULL,
        PrecioUnitario DECIMAL(10,2) NOT NULL,
        Subtotal DECIMAL(10,2) NOT NULL,
        CONSTRAINT FK_DetallePedidos_Pedidos FOREIGN KEY (PedidoId) REFERENCES Pedidos(PedidoId),
        CONSTRAINT FK_DetallePedidos_Productos FOREIGN KEY (ProductoId) REFERENCES Productos(ProductoId)
    );
END
GO
