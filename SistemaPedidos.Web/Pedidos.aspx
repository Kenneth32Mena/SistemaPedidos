<%@ Page Title="Crear Pedido" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Pedidos.aspx.cs" Inherits="SistemaPedidos.Web.Pedidos" %>

<asp:Content ID="BodyContent" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row mb-3">
        <div class="col">
            <h2 class="text-primary"><i class="bi bi-cart-plus-fill me-2"></i>Módulo de Registro de Pedidos</h2>
            <p class="text-muted">Seleccione un cliente, agregue productos al carrito y confirme la orden mediante una transacción segura.</p>
        </div>
    </div>

    <!-- Mensajes de feedback -->
    <asp:Panel ID="pnlMensaje" runat="server" Visible="false" CssClass="alert alert-dismissible fade show" role="alert">
        <asp:Literal ID="litMensaje" runat="server"></asp:Literal>
        <button type="button" class="btn-close" data-bs-dismiss="alert" aria-label="Close"></button>
    </asp:Panel>

    <div class="row">
        <!-- Panel Izquierdo: Selección de Cliente y Agregado de Productos -->
        <div class="col-lg-5 mb-4">
            <!-- 1. Selección de Cliente -->
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-person-check me-1"></i>1. Seleccionar Cliente</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="ddlClientes" class="form-label">Cliente para el Pedido <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlClientes" runat="server" CssClass="form-select">
                        </asp:DropDownList>
                    </div>
                </div>
            </div>

            <!-- 2. Selección de Productos y Cantidad -->
            <div class="card shadow-sm">
                <div class="card-header bg-white">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-cart-plus me-1"></i>2. Agregar Producto al Carrito</h5>
                </div>
                <div class="card-body">
                    <div class="mb-3">
                        <label for="ddlProductos" class="form-label">Seleccionar Producto <span class="text-danger">*</span></label>
                        <asp:DropDownList ID="ddlProductos" runat="server" CssClass="form-select" AutoPostBack="true" OnSelectedIndexChanged="ddlProductos_SelectedIndexChanged">
                        </asp:DropDownList>
                    </div>
                    <div class="row mb-3">
                        <div class="col-md-6">
                            <label class="form-label">Precio Unitario:</label>
                            <div class="input-group">
                                <span class="input-group-text">$</span>
                                <asp:TextBox ID="txtPrecioProducto" runat="server" CssClass="form-control bg-light" ReadOnly="true"></asp:TextBox>
                            </div>
                        </div>
                        <div class="col-md-6">
                            <label class="form-label">Stock Actual:</label>
                            <asp:TextBox ID="txtStockDisponible" runat="server" CssClass="form-control bg-light" ReadOnly="true"></asp:TextBox>
                        </div>
                    </div>
                    <div class="mb-3">
                        <label for="txtCantidad" class="form-label">Cantidad a Solicitar <span class="text-danger">*</span></label>
                        <asp:TextBox ID="txtCantidad" runat="server" CssClass="form-control" type="number" min="1" Text="1"></asp:TextBox>
                    </div>
                    <div class="d-grid">
                        <asp:Button ID="btnAgregarCarrito" runat="server" Text="Agregar al Carrito" CssClass="btn btn-outline-primary" OnClick="btnAgregarCarrito_Click" />
                    </div>
                </div>
            </div>
        </div>

        <!-- Panel Derecho: Carrito de Pedido Temporal (Session) y Confirmación -->
        <div class="col-lg-7">
            <div class="card shadow-sm mb-4">
                <div class="card-header bg-white d-flex justify-content-between align-items-center">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-cart3 me-1"></i>3. Carrito de Pedido</h5>
                    <asp:Button ID="btnLimpiarCarrito" runat="server" Text="Vaciar Carrito" CssClass="btn btn-sm btn-outline-danger" OnClick="btnLimpiarCarrito_Click" />
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvCarrito" runat="server" AutoGenerateColumns="False" 
                                      CssClass="table table-striped table-hover mb-0 align-middle"
                                      EmptyDataText="No hay productos en el carrito temporal." 
                                      GridLines="None" OnRowCommand="gvCarrito_RowCommand">
                            <Columns>
                                <asp:BoundField DataField="ProductoId" HeaderText="ID" ItemStyle-Width="50px" />
                                <asp:BoundField DataField="Nombre" HeaderText="Producto" />
                                <asp:BoundField DataField="Precio" HeaderText="Precio" DataFormatString="${0:N2}" />
                                <asp:BoundField DataField="Cantidad" HeaderText="Cant." ItemStyle-Width="60px" />
                                <asp:BoundField DataField="Subtotal" HeaderText="Subtotal" DataFormatString="${0:N2}" ItemStyle-CssClass="fw-semibold" />
                                <asp:TemplateField ItemStyle-Width="80px">
                                    <ItemTemplate>
                                        <asp:LinkButton ID="btnEliminar" runat="server" CommandName="EliminarItem" CommandArgument='<%# Eval("ProductoId") %>' CssClass="btn btn-sm btn-outline-danger">
                                            <i class="bi bi-trash"></i>
                                        </asp:LinkButton>
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                            <HeaderStyle CssClass="table-light" />
                        </asp:GridView>
                    </div>
                </div>
                <div class="card-footer bg-light d-flex justify-content-between align-items-center py-3">
                    <div>
                        <span class="fs-6 text-muted">Total a Pagar:</span>
                        <h3 class="text-primary mb-0 fw-bold"><asp:Literal ID="litTotalCarrito" runat="server">$0.00</asp:Literal></h3>
                    </div>
                    <asp:Button ID="btnConfirmarPedido" runat="server" Text="Confirmar y Registrar Pedido" CssClass="btn btn-success btn-lg px-4" OnClick="btnConfirmarPedido_Click" />
                </div>
            </div>

            <!-- Historial de Pedidos Realizados -->
            <div class="card shadow-sm">
                <div class="card-header bg-white d-flex justify-content-between align-items-center">
                    <h5 class="card-title mb-0 text-dark"><i class="bi bi-receipt me-1"></i>Historial de Pedidos Registrados</h5>
                    <asp:Button ID="btnRefrescarHistorial" runat="server" Text="Actualizar" CssClass="btn btn-sm btn-outline-secondary" OnClick="btnRefrescarHistorial_Click" />
                </div>
                <div class="card-body p-0">
                    <div class="table-responsive">
                        <asp:GridView ID="gvHistorialPedidos" runat="server" AutoGenerateColumns="False" 
                                      CssClass="table table-sm table-hover mb-0 align-middle"
                                      EmptyDataText="Aún no se han registrado pedidos." 
                                      GridLines="None">
                            <Columns>
                                <asp:BoundField DataField="PedidoId" HeaderText="# Pedido" ItemStyle-Width="90px" ItemStyle-CssClass="fw-bold text-center" HeaderStyle-CssClass="text-center" />
                                <asp:BoundField DataField="Fecha" HeaderText="Fecha" DataFormatString="{0:dd/MM/yyyy HH:mm}" />
                                <asp:TemplateField HeaderText="Cliente">
                                    <ItemTemplate>
                                        <%# Eval("Cliente.Nombre") %>
                                    </ItemTemplate>
                                </asp:TemplateField>
                                <asp:BoundField DataField="Total" HeaderText="Total" DataFormatString="${0:N2}" ItemStyle-CssClass="fw-bold text-success text-end" HeaderStyle-CssClass="text-end" />
                            </Columns>
                            <HeaderStyle CssClass="table-light" />
                        </asp:GridView>
                    </div>
                </div>
            </div>
        </div>
    </div>
</asp:Content>
