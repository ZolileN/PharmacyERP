<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectionStock.aspx.cs" Inherits="IMS.SelectionStock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <div class="row">
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SelectProduct" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="SelectProduct" CssClass="form-control" Width="29%" AutoPostBack="true" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <br />
            </div>
        </div>
    </div>
     <%----%>
    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                     <asp:TemplateField HeaderText="BarCode">
                        <ItemTemplate>
                            <asp:Label ID="BarCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BarCode") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductName") %>' Width="300px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="310px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Qauntity") %>'></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("Expiry") %>' Width="190px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCostPrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("CostPrice") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitCostPrice" CssClass="form-control" runat="server" Text='<%#Eval("CostPrice") %>'></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUnitCostPrice" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Sale">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SalePrice") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitSalePrice" CssClass="form-control" runat="server" Text='<%#Eval("SalePrice") %>'></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtUnitSalePrice" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton CssClass="btn btn-default" ID="btnDelete" Text="Delete" runat="server" CommandArgument='<%# Eval("StockID") %>' CommandName="Delete"/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="StockID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStockID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("StockID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
    </div>
    </div>
</asp:Content>
