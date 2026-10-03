<%@ Page Title="Inicio" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Default.aspx.cs" Inherits="SistemaPedidos.Web.Default" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="p-5 mb-4 bg-light rounded-3 shadow-sm border">
        <div class="container-fluid py-3">
            <h1 class="display-5 fw-bold text-primary"><i class="bi bi-diagram-3-fill me-2"></i>Sistema de Gestión de Pedidos</h1>
            <p class="col-md-9 fs-5 text-secondary">
                Aplicación Web implementada con <strong>Arquitectura en Capas (N-Tier)</strong>, Inversión de Dependencias (DIP), 
                Principio de Responsabilidad Única (SRP) y persistencia transaccional con ADO.NET y SQL Server.
            </p>
            <hr class="my-4">
            <div class="d-flex gap-3">
                <a href="Pedidos.aspx" class="btn btn-primary btn-lg px-4"><i class="bi bi-cart-plus-fill me-2"></i>Crear Nuevo Pedido</a>
                <a href="Productos.aspx" class="btn btn-outline-secondary btn-lg px-4"><i class="bi bi-boxes me-2"></i>Gestionar Productos</a>
                <a href="Clientes.aspx" class="btn btn-outline-info btn-lg px-4"><i class="bi bi-people me-2"></i>Gestionar Clientes</a>
            </div>
        </div>
    </div>

    <div class="row row-cols-1 row-cols-md-3 g-4 mb-4">
        <div class="col">
            <div class="card h-100 shadow-sm border-start border-4 border-primary">
                <div class="card-body">
                    <h5 class="card-title text-primary"><i class="bi bi-layers-fill me-2"></i>Arquitectura en Capas</h5>
                    <p class="card-text text-muted">
                        Separación clara de responsabilidades: <strong>Presentación</strong> (Web Forms), <strong>Aplicación</strong> (Services/DTOs), 
                        <strong>Dominio</strong> (Entidades, Interfaces y Reglas) e <strong>Infraestructura</strong> (ADO.NET, Repositories).
                    </p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card h-100 shadow-sm border-start border-4 border-success">
                <div class="card-body">
                    <h5 class="card-title text-success"><i class="bi bi-shield-check me-2"></i>Transacciones Atómicas</h5>
                    <p class="card-text text-muted">
                        Registro consistente de cabecera y detalle de pedidos con actualización automática del stock de productos bajo <strong>SqlTransaction (Commit / Rollback)</strong>.
                    </p>
                </div>
            </div>
        </div>
        <div class="col">
            <div class="card h-100 shadow-sm border-start border-4 border-warning">
                <div class="card-body">
                    <h5 class="card-title text-warning"><i class="bi bi-gear-wide-connected me-2"></i>Composition Root</h5>
                    <p class="card-text text-muted">
                        Configuración desacoplada en <code>ServiceFactory</code>, conectando abstracciones e implementaciones para cumplir con DIP y OCP.
                    </p>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
