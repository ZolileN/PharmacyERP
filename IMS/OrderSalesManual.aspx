<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSalesManual.aspx.cs" Inherits="IMS.OrderSalesManual" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h3>Generate Sale Orders</h3>
    <br />
    <br />

    <table cellspacing="5" cellpadding="5" border="0" width="100%">

        <tr>
            <td><asp:Label runat="server" AssociatedControlID="txtIvnoice" CssClass="control-label">Invoice No </asp:Label></td>
            <td><asp:TextBox runat="server" ID="txtIvnoice" CssClass="form-control" /></td>
        
            <td><asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Select Store </asp:Label></td>
            <td><asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StockAt" CssClass="text-danger" ErrorMessage="The Store field is required." ValidationGroup="ExSave"/></td>
            
                        
        </tr>
        <tr>
            <td><asp:Label runat="server" AssociatedControlID="txtProduct" CssClass="control-label">Select Product</asp:Label></td>
            <td><asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click"  Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="SelectProduct" CssClass="text-danger" ErrorMessage="The Product field is required." ValidationGroup="ExSave"/>
             
                </td>
            <td><asp:Label runat="server" AssociatedControlID="SelectQuantity" CssClass="control-label">Enter Send Quantity</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="SelectQuantity" CssClass="form-control" /></td>
        </tr>
         <tr>
            
            <td><asp:Label runat="server" AssociatedControlID="SelectBonus" CssClass="control-label">Enter Bonus Quantity</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="SelectBonus" CssClass="form-control" /></td>
            
            <td><asp:Label runat="server" AssociatedControlID="SelectDiscount" CssClass="control-label">Enter Discount %</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="SelectDiscount" CssClass="form-control" /></td>
        </tr>
        <tr>
            <td colspan="100%">&nbsp;</td>
        </tr>
        <tr>
            <td></td>
            <td colspan="100%">

                <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="ExSave"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-default btn-large" />

            </td>


        </tr>

    </table>

    
    <br />
    <br />

    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand"  onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound" >
                 <Columns>
                     <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="ProductID" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ToPlace") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit"  CommandArgument='<%# Container.DataItemIndex %>'/>
                            <asp:Button CssClass="btn btn-default details-btn" ID="btnDetails" Text="Details" runat="server" CommandName="Details" CommandArgument='<%# Container.DataItemIndex %>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"  CommandArgument='<%# Container.DataItemIndex %>'/>
                            </span>
                        </ItemTemplate>
                          
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField Visible="false" HeaderText="Name : Strength : Form : Pack Size" HeaderStyle-Width="500" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName2" padding-right="5px" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                            <asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'  ></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2"  runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px" ></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px" ></asp:Label>
                        </ItemTemplate>
                        
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product<br>Description" Visible="true" HeaderStyle-Width ="60px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="60px" HorizontalAlign="Left" Wrap="true"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="" runat="server" Text='<%# Eval("strength") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="" runat="server" Text='<%# Eval("PackageSize") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Available<br>Stock"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("AvailableStock") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Sent<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("Qauntity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Qauntity") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="" runat="server" Text='<%# Eval("Bonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonus" CssClass="form-control" runat="server" Text='<%#Eval("Bonus") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Cost<br>Price"  HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("SalePrice") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    
                     <asp:TemplateField HeaderText="OrderStatus" Visible ="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="" runat="server" Text='<%# Eval("Status") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="DETAILS" HeaderStyle-Width="200px" >
                        <ItemTemplate>
                            <asp:GridView ID="StockDetailDisplayGrid" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10">
                                <Columns>

                                     <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="Batch" CssClass="" runat="server" Text='<%# Eval("BatchNumber") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="70px" HorizontalAlign="Left" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sent Quantity" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="" runat="server" Text='<%# Eval("SendQuantity") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bonus Quantity" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="" runat="server" Text='<%# Eval("BonusQuantity") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="50px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    
                 </Columns>
             </asp:GridView>
        <br />
         <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="GENERATE ORDER" CssClass="btn btn-large" Visible="false"/>
         <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="DELETE ORDER" CssClass="btn btn-large" Visible="false" />
    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPacking" runat="server" OnClick="btnPacking_Click" Text="GENERATE PACKING LIST" CssClass="btn btn-large" Visible="false"/>
                <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="GENERATE PRINT" CssClass="btn btn-large" Visible="false" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        function ValidateForm() {
            var valid = true;
            if (document.getElementById("MainContent_txtProduct").value == null || document.getElementById("MainContent_txtProduct").value == '') {
                alert("Please enter at least three words to search product");
                valid = false;
            }
            return valid;

            //var e = document.getElementById("SelectProduct");
            //var strProduct = e.options[e.selectedIndex].value;
           
        }
        

</script>
</asp:Content>

