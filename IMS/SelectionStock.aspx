<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectionStock.aspx.cs" Inherits="IMS.SelectionStock" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <div class="row">
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SelectProduct" Visible="false" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" Visible="false" ID="SelectProduct" CssClass="form-control" Width="29%" AutoPostBack="true" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <br />
            </div>
        </div>
    </div>
      <table width="100%">

        <tbody><tr>
        	<td> <h4>Edit Stock</h4></td>
            <td align="right">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Go Back" OnClick="btnBack_Click"/>
              
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    <hr>
    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                           <%-- <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandArgument='<%# Eval("StockID") %>' CommandName="Delete"/>
                            </span>--%>
                        </ItemTemplate>
                        
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="BarCode">
                        <ItemTemplate>
                            <asp:Label ID="BarCode" runat="server" Text='<%# Eval("BarCode") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" runat="server" Text='<%# Eval("ProductName") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="330px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width ="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Qauntity") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                      
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" runat="server" Width="40px" Text='<%#Eval("Qauntity") %>'></asp:TextBox>
                             <asp:RequiredFieldValidator Width="40px" runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                         <ItemStyle  Width="40px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                                 

                     <asp:TemplateField HeaderText="Unit <br> Cost" HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCostPrice"  runat="server" Text='<%# Eval("CostPrice") %>' Width="60px"></asp:Label>
                        </ItemTemplate>
                       
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitCostPrice"  runat="server" Width="60px" Text='<%#Eval("CostPrice") %>'></asp:TextBox>
                             <%--<asp:RequiredFieldValidator runat="server" Width="47px" ControlToValidate="txtUnitCostPrice" CssClass="text-danger" ErrorMessage="The product quantity field is required." />--%>
                        </EditItemTemplate>
                         <ItemStyle  Width="600px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit<br> Sale" HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" runat="server" Text='<%# Eval("SalePrice") %>' Width="60px"></asp:Label>
                        </ItemTemplate>
                       
                        <EditItemTemplate>
                            <asp:TextBox ID="txtUnitSalePrice" runat="server" Width="67px" Text='<%#Eval("SalePrice") %>'></asp:TextBox>
                            <%-- <asp:RequiredFieldValidator runat="server" Width="47px" ControlToValidate="txtUnitSalePrice" CssClass="text-danger" ErrorMessage="The product quantity field is required." />--%>
                        </EditItemTemplate>
                         <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Expiry" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry"  runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="txtExpDate" CssClass="clDate"  runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>' Width="100px"></asp:TextBox>
                         </EditItemTemplate>
                       <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width ="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" runat="server" Text='<%# Eval("BatchNumber") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                       
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBatch" runat="server" Width="47px" Text='<%#Eval("BatchNumber") %>'></asp:TextBox>
                            
                        </EditItemTemplate>
                         <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <%-- Hidden Fields --%>
                     
                     <asp:TemplateField HeaderText="StockID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblStockID" runat="server" Text='<%# Eval("StockID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Barcode serial" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBarSerial" runat="server" Text='<%# Eval("barSerial") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="ProdID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblProdID" runat="server" Text='<%# Eval("ProductID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Expiry Date" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblExpOrg" runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>'></asp:Label>
                        </ItemTemplate>
                         
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Quantity" Visible="false" HeaderStyle-Width ="50px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantityOrg" runat="server" Text='<%# Eval("Qauntity") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                         </asp:TemplateField>
                 </Columns>
             </asp:GridView>
      
    </div>
    </div>
    <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () {
                  $(".clDate").datepicker();
              });
              $(function () { $("[id$=MainContent_StockDisplayGrid_txtAddExpDate]").datepicker(); });
          </script>
</asp:Content>
