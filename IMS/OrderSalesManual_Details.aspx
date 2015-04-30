<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSalesManual_Details.aspx.cs" Inherits="IMS.OrderSalesManual_Details" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Sale Order Generation - Stock Details</h3>
    <br />
    <div class="form-group">
        <asp:Label ID="lblTotalSent" CssClass="col-md-2 control-label" runat="server" Text="Total Sent Quantity: "  Width="180px"></asp:Label>
        <asp:Label ID="lblTotalQuantity" CssClass="col-md-2 control-label" runat="server" Text="---"  Width="50px"></asp:Label>
    </div>
    <br />
  <div class="form-horizontal">
    <div class="form-group">
         <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound" >
                 <Columns>
                      <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="220px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button CssClass="btn btn-default update-btn" ID="btnRefresh" Text="Refresh" runat="server" CommandName="Refresh" CommandArgument='<%# Container.DataItemIndex %>' />
                        </ItemTemplate>
                        <EditItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="StockID" Visible="false" HeaderStyle-Width ="350px">
                        <ItemTemplate>
                            <asp:Label ID="lblStockID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("StockID") %>'  Width="350px" ></asp:Label>
                            <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>'  Width="350px" ></asp:Label>
                            <asp:Label ID="lblBarCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BarCode") %>'  Width="350px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                      </asp:TemplateField>
                      
                      <asp:TemplateField HeaderText="Product Description" Visible="true" HeaderStyle-Width ="350px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="control-label" runat="server" Text='<%# Eval("Description") %>'  ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                      </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Expiry" HeaderStyle-Width ="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ExpiryDate") %>'  Width="180px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                        </asp:TemplateField>

                      <asp:TemplateField HeaderText="Batch<br>No." HeaderStyle-Width ="70px">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BatchNumber") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                        </asp:TemplateField>

                      <asp:TemplateField HeaderText="Available<br>Stock"  HeaderStyle-Width ="120px">
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Stock") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                       <asp:TemplateField HeaderText="Unit<br>SalePrice"  HeaderStyle-Width ="120px">
                        <ItemTemplate>
                            <asp:Label ID="lblSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("USalePrice") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Unit<br>CostPrice"  HeaderStyle-Width ="120px">
                        <ItemTemplate>
                            <asp:Label ID="lblCostPrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UCostPrice") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Sent<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SentQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("SentQuantity") %>' Width="47px"></asp:TextBox>
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BonusQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonus" CssClass="form-control" runat="server" Text='<%#Eval("BonusQuantity") %>' Width="47px" ></asp:TextBox>
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                      <asp:TemplateField HeaderText="Discount %"  HeaderStyle-Width ="47px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblDiscount" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Discount") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtDiscount" CssClass="form-control" runat="server" Text='<%#Eval("Discount") %>' Width="47px"></asp:TextBox>
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                      </asp:TemplateField>
                      
                 </Columns>
             </asp:GridView>
        <br />
         <asp:Button ID="btnAcceptStock" runat="server" OnClick="btnAcceptStock_Click" Text="Accept" CssClass="btn btn-large" Visible="true"/>
         <asp:Button ID="btnDeclineStock" runat="server" OnClick="btnDeclineStock_Click" Text="Cancel" CssClass="btn btn-large" Visible="false" />
      </div>
      </div>
</asp:Content>
