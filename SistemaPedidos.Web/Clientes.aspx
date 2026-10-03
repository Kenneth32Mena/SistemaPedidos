<%@ Page Title="Gestión de Clientes" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Clientes.aspx.cs" Inherits="SistemaPedidos.Web.Clientes" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-3">
        <div class="col">
            <h2 class="text-primary"><i class="bi bi-people-fill me-2"></i>Gestión de Clientes</h2>
            <p class="text-muted">Registro y consulta de clientes en la base de datos.</p>
        </div>
    </div>

    <!-- Mensajes de feedback -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
        <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="row">
        <!-- Formulario de Registro -->
        <div class="col-lg-4 mb-4">
            <div class="card shadow-sm">
                <div class="card-header bg-white">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-person-plus me-1"></i>Nuevo Cliente</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtNombre" class="form-label">Nombre Completo <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Roberto Gomez"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtCorreo" class="form-label">Correo Electrónico <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCorreo" runat="server" CssClass="form-control" placeholder="Ej: roberto@gmail.com" TextMode="Email"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtTelefono" class="form-label">Teléfono</label>
                        <asp:TextBox ID="txtTelefono" runat="server" CssClass="form-control" placeholder="Ej: 555-7890"></asp:TextBox>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnGuardarCliente" runat="server" Text="Registrar Cliente" CssClass="btn btn-primary" OnClick="btnGuardarCliente_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Tabla de Consulta de Clientes -->
        <div class="col-lg-8">
            <div class="card shadow-sm">
                <div class="card-header bg-white d-flex justify-content-between align-items-center">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-list-check me-1"></i>Listado de Clientes</h5>
                    <asp:Button ID="btnRefrescar" runat="server" Text="Actualizar" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnRefrescar_Click" />
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvClientes" runat="server" AutoGenerateColumns="False" 
                                      CssClass="table table-striped table-hover mb-0 align-middle"
                                      EmptyDataText="No hay clientes registrados en el sistema." 
                                      GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="ClienteId" HeaderText="ID" ItemStyle-Width="60px" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre" />
                                <asp:BoundField DataField="Correo" HeaderText="Correo" />
                                <asp:BoundField DataField="Telefono" HeaderText="Teléfono" NullDisplayText="-" />
                                <asp:BoundField DataField="FechaRegistro" HeaderText="Fecha Registro" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                            </Columns>
                            <HeaderStyle CssClass="table-light" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
