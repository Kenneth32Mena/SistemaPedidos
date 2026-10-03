# Sistema Web de Gestión de Pedidos

Aplicación web desarrollada en **C# / ASP.NET Web Forms (.NET Framework 4.6)** conectada a **SQL Server**, aplicando una **Arquitectura en Capas (N-Tier)** y principios **SOLID**.

---

## 🏛️ Estructura en Capas

- **`SistemaPedidos.Web` (Presentación):** Páginas Web Forms (`Default.aspx`, `Productos.aspx`, `Clientes.aspx`, `Pedidos.aspx`) y punto de configuración (`Composition/ServiceFactory.cs`).
- **`SistemaPedidos.Application` (Aplicación):** Casos de uso y coordinación (`ProductoService`, `ClienteService`, `PedidoService`, DTOs).
- **`SistemaPedidos.Domain` (Dominio):** Entidades (`Cliente`, `Producto`, `Pedido`, `DetallePedido`), interfaces y reglas de negocio.
- **`SistemaPedidos.Infrastructure` (Infraestructura):** Conexión y persistencia con ADO.NET (`SqlConnectionFactory`, Repositorios) bajo transacción atómica (`SqlTransaction`).
- **`Database/`:** Scripts SQL de creación, datos iniciales y verificación.

---

## 🎯 Principios OO Aplicados

- **SRP:** `Pedido` maneja reglas de negocio, `PedidoService` coordina la operación y `PedidoRepository` gestiona la persistencia en SQL.
- **DIP:** La capa de Aplicación depende de abstracciones (`IPedidoRepository`, `IProductoRepository`) inyectadas en `ServiceFactory`.
- **OCP:** La persistencia puede reemplazarse sin modificar las reglas de Dominio ni los Servicios de Aplicación.

---

## 🔒 Persistencia y Transacciones

- **Consultas Parametrizadas:** Prevención de inyección SQL en todos los comandos.
- **Transacción Atómica:** `INSERT Pedido` + `INSERT Detalle` + `UPDATE Stock` asegurados mediante `COMMIT` / `ROLLBACK`.

---

## 🚀 Ejecución

1. Ejecutar scripts en SQL Server:
   - `Database/01_Crear_Base_Datos.sql`
   - `Database/02_Datos_Iniciales.sql`
2. Abrir `SistemaPedidos.sln` en **Visual Studio 2015+**.
3. Establecer `SistemaPedidos.Web` como proyecto de inicio y presionar **F5**.
