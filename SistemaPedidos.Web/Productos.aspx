<%@ Page Title="Gestión de Productos" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Productos.aspx.cs" Inherits="SistemaPedidos.Web.Productos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-3">
        <div class="col">
            <h2 class="text-primary"><i class="bi bi-boxes me-2"></i>Gestión de Productos e Inventario</h2>
            <p class="text-muted">Registro de nuevos productos y control de stock en inventario.</p>
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
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-plus-circle me-1"></i>Nuevo Producto</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="txtNombre" class="form-label">Nombre del Producto <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtNombre" runat="server" CssClass="form-control" placeholder="Ej: Laptop ASUS ZenBook"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtPrecio" class="form-label">Precio Unitario ($) <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtPrecio" runat="server" CssClass="form-control" placeholder="Ej: 750.00" type="number" step="0.01"></asp:TextBox>
                    </div>
                    <div class="mb-3">
                        <label for="txtStock" class="form-label">Stock Inicial <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtStock" runat="server" CssClass="form-control" placeholder="Ej: 20" type="number"></asp:TextBox>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnGuardarProducto" runat="server" Text="Registrar Producto" CssClass="btn btn-primary" OnClick="btnGuardarProducto_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Tabla de Inventario de Productos -->
        <div class="col-lg-8">
            <div class="card shadow-sm">
                <div class="card-header bg-white d-flex justify-content-between align-items-center">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-box-seam me-1"></i>Inventario Actual</h5>
                    <asp:Button ID="btnRefrescar" runat="server" Text="Actualizar" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnRefrescar_Click" />
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvProductos" runat="server" AutoGenerateColumns="False" 
                                      CssClass="table table-striped table-hover mb-0 align-middle"
                                      EmptyDataText="No hay productos registrados en el inventario." 
                                      GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="ProductoId" HeaderText="ID" ItemStyle-Width="60px" ItemStyle-CssClass="fw-bold" />
                                <asp:BoundField DataField="Nombre" HeaderText="Nombre del Producto" />
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="${0:N2}" ItemStyle-CssClass="fw-semibold text-success" />
                                <asp:TemplateField HeaderText="Stock Disponible">
                                    <ItemTemplate>
                                        <span class='<%# Convert.ToInt32(Eval("Stock")) > 0 ? "badge bg-success" : "badge bg-danger" %>'>
                                            <%# Eval("Stock") %> unidades
                                        </span>
                                    </ItemTemplate>
                                </asp:TemplateField>
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
