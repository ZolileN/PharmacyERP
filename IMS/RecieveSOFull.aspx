<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecieveSOFull.aspx.cs" Inherits="IMS.RecieveSOFull" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <table width="100%">

        <tbody>
            <tr>
        	<td> <h4>Receive Sale Orders</h4></td>
            <td align="right">
            		                		                 	 
                <asp:Button ID="btnAcceptAll" runat="server" OnClick="btnAcceptAll_Click" Text="Accept All" CssClass="btn btn-success"  />
                <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" Text="Go Back" CssClass="btn btn-default btn-large" />
                
            </td>
        </tr>
		<tr>
            <td height="5"></td>

		</tr>
    </tbody>

     </table>

      <table Width="100%" class="formStriped table-striped">
        <tr>
            <td><b>Sales Order ID</b></td>
            <td><asp:Label runat="server" ID="lblSOID" CssClass="" /></td>
            <td class="auto-style1"></td>
            <td class="auto-style2"><b>Order Date</b></td>
            <td><asp:Label ID="SODate" runat="server" CssClass=""></asp:Label></td>
            
        </tr>
           <tr>
             <td><b>Order To</b></td>
             <td><asp:Label runat="server" ID="OrderTo" CssClass="" /></td>
             <td class="auto-style1"></td>
             <td><b>Order Status</b></td>
            <td> <asp:Label runat="server" ID="OrderStatus" CssClass="" /></td>
            
           
        </tr>

         <tr>
             <td><b>Sent Quantity</b></td>
             <td><asp:Label runat="server" ID="sendQty" CssClass="" /></td>
             <td class="auto-style1"></td>
             <td><b>Returned Quantity</b></td>
            <td> <asp:Label runat="server" ID="RetQty" CssClass="" /></td>
            
           
        </tr>
      
       
    </table>
   

     <hr>

        <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false" OnRowDataBound="StockDisplayGrid_RowDataBound"   onrowcommand="StockDisplayGrid_RowCommand" >
                 <Columns>
                     <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="ProductID" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Literal ID="lblProductID" runat="server" Text='<%# Eval("ProductID") %>'   ></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle   HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" Visible="false"  >
                        <ItemTemplate>
                            <asp:Literal ID="RequestedTo"  runat="server" Text='<%# Eval("ToPlace") %>'   ></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Action"  >
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit"  CommandArgument='<%# Container.DataItemIndex %>'/>
                            <asp:Button CssClass="btn btn-default details-btn" Visible="false" ID="btnDetails" Text="Details" runat="server" CommandName="Details" CommandArgument='<%# Container.DataItemIndex %>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" Visible="false" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"  CommandArgument='<%# Container.DataItemIndex %>'/>
                            </span>
                        </ItemTemplate>
                          
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
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

                     <asp:TemplateField HeaderText="Product<br>Description" Visible="true" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description")   %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle    HorizontalAlign="Left" Wrap="true"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Strength" Visible="false" >
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="" runat="server" Text='<%# Eval("strength") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  HorizontalAlign="Left"/>
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

                      <asp:TemplateField HeaderText="Available<br>Stock"   >
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("AvailableStock") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Sent<br>Quantity"   > 
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("Qauntity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control grid-input-form" runat="server" Text='<%#Eval("Qauntity") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Quantity"   > 
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="" runat="server" Text='<%# Eval("Bonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonus" CssClass="form-control grid-input-form" runat="server" Text='<%#Eval("Bonus") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Accepted<br>Qty"  >
                                        <ItemTemplate>
                                            <asp:Literal ID="lbAcceptedQuantity"  runat="server" Text='<%# Eval("AcceptedQuantity") %>'  ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Accepted<br>Bonus Qty" >
                                        <ItemTemplate>
                                            <asp:Literal ID="DelieveredBonusQuantity"  runat="server" Text='<%# Eval("DelieveredBonusQuantity") %>' ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Returned<br>Quantity" >
                                        <ItemTemplate>
                                            <asp:Literal ID="ReturnedQuantity"  runat="server" Text='<%# Eval("ReturnedQuantity") %>' ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle  HorizontalAlign="Left" />
                                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Cost<br>Price"  >
                        <ItemTemplate>
                            <asp:Literal ID="lblSales"  runat="server" Text='<%# Eval("SalePrice") %>' ></asp:Literal>
                        </ItemTemplate>
                        
                          <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    
                     <asp:TemplateField HeaderText="OrderStatus" Visible ="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="" runat="server" Text='<%# Eval("Status") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="DETAILS"  >
                        <ItemTemplate>
                            <asp:GridView ID="StockDetailDisplayGrid" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10">
                                <Columns>

                                     <asp:TemplateField HeaderText="Batch No"  >
                                        <ItemTemplate>
                                            <asp:Literal ID="Batch"  runat="server" Text='<%# Eval("BatchNumber") %>'  ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle   HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expiry Date"  >
                                        <ItemTemplate>
                                            <asp:Literal ID="lblExpiryDate"  runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>'  ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle   HorizontalAlign="Left" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sent Quantity"   >
                                        <ItemTemplate>
                                            <asp:Literal ID="SendQuantity"  runat="server" Text='<%# Eval("SendQuantity") %>'  ></asp:Literal>
                                        </ItemTemplate>
                                        <ItemStyle   HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bonus Quantity"  >
                                        <ItemTemplate>
                                            <asp:Label ID="BonusQuantity"   runat="server" Text='<%# Eval("BonusQuantity") %>'  ></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle   HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    

                                    <asp:TemplateField HeaderText="Discount %"  Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="DiscountPercentage" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' Width="110px"></asp:Label>
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
         
    </div>
   
    </div>
</asp:Content>
