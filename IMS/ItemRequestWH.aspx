<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ItemRequestWH.aspx.cs" Inherits="IMS.ItemRequestWH" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %> 
<%@ Register TagName="ProductsPopup"  TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Item Request to Warehouse</h4>
                <asp:Label ID="lblWH" runat="server" style="color:#2c81da"></asp:Label>
        	</td>
            <td align="right">
                <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="Generate PO" CssClass="btn btn-success"  />
             <span onclick="return confirm('Are you sure you want to delete this order?')">
                 <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="Delete PO" CssClass="btn btn-danger"  />
           </span>

            <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-default btn-large" />


          <%-- <input type="submit" class="btn btn-success" value="Generate PO" onClick="window.location.href = 'purchase-order.html'" id="genPO"> <input type="submit" class="btn btn-danger" value="Delete PO" id="delPO">
		   <input type="submit" class="btn btn-default" id="backPO" value="Back" onClick="location.reload();">
                --%>
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    <hr>
     <br />
    
   <table cellspacing="5" cellpadding="5" border="0" style="margin-left:5px;" class="formTbl" id="productAdd" width="100%">
     
       <tr>
           <td>
               <asp:Label runat="server" ID="lblProd"  CssClass="control-label">Select Product</asp:Label></td>
           <td>
               <asp:TextBox ID="txtSearch" runat="server" CssClass="product"></asp:TextBox>
               <asp:Label ID="lblProductId" runat="server" Visible="false"></asp:Label>
                
               <asp:ImageButton ID="btnSearchProduct" runat="server"   CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click1" />
 

               <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblProd" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv2" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>

               <div id="_CongratsMessageDiv2" class="congrats-cont" style="display: none; ">
                            <UCProductsPopup:ProductsPopup  id="ProductsPopupGrid" runat="server"/>
                        </div>
             
           </td>
           <td>
               <asp:Label runat="server" AssociatedControlID="SelectQuantity" CssClass="control-label">Quantity</asp:Label>
           </td>
           <td>
               <asp:TextBox runat="server" ID="SelectQuantity"  autocomplete="off" style="width:60px"  />
           </td>
           <td>
               <asp:Label runat="server" AssociatedControlID="SelectPrice" CssClass="control-label">Bonus Quantity</asp:Label>
           </td>
           <td>
               <asp:TextBox runat="server" ID="SelectPrice"  autocomplete="off"  style="width:60px"/>
           </td>
           <td>
                <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-primary btn-sm" OnClientClick="return ValidateForm();" ValidationGroup="ExSave"/>
                
           </td>
           <td align="right" width="25%"></td>

           </tr>
       <tr>

       <td colspan="100%">&nbsp;</td>
       </tr>
       
       <tr>
           <td></td>
           <td colspan="100%">
               <%--<asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="ExSave"/>--%>
                <%--<asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />--%>
                <%--<asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-default btn-large" />--%>

           </td>
       </tr>
    </table>
    
         
   <br />
   
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand"  onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound" >
                 <Columns>
                     <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="From" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("FromPlace") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ToPlace") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>
                          
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default btn-xs" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            
                            <asp:LinkButton CssClass="btn btn-default btn-xs" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Product Description" Visible="true" HeaderStyle-Width ="430px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="control-label ProductDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                      <%--<asp:TemplateField HeaderText="Strength" Visible="true" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                              
                             <asp:Label ID="ProductStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemStrength") %>'  Width="150px" ></asp:Label>
                            <asp:Label ID="itemForm" CssClass="control-label ProductDescription" runat="server" Text='<%# Eval("itemForm") %>'></asp:Label>
                            <asp:Label ID="itemPackSize" CssClass="control-label ProductDescription" runat="server" Text='<%# Eval("itemPackSize") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>--%>
                     <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PackageSize") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Quantity"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Qauntity") %>' Width="47px" ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Qty"  HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblBonusQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonusQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Bonus") %>' Width="47px" ></asp:TextBox>
                            </EditItemTemplate>
                          <ItemStyle  Width="90px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Unit<br>Cost Price" HeaderStyle-Width="110px">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total<br>Cost Price" HeaderStyle-Width="110px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalCostPrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order<br>Status" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                   <%--Hidden Fields--%>   
                     <asp:TemplateField HeaderText="Quantity Org" Visible="false"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantityOrg" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>
                     </asp:TemplateField>                    
                 </Columns>
             </asp:GridView>
          <table ID="tblDsp" cellpadding="4" cellspacing="0" align="right" visible="false">
        	<tr>
            	<td><asp:Label ID="lblttlcst" runat="server" AssociatedControlID="lblTotalCostALL" Visible="false">Total Cost:</asp:Label></td>
                <td><asp:Label ID="lblTotalCostALL" Visible="false" runat="server" Style="font-weight: 700"></asp:Label></td>


	           
            </tr>
         </table>
        <br />
</asp:Content>
