<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductsPopupGrid.ascx.cs" Inherits="IMS.UserControl.ProductsPopupGrid" %>


 <div class="popupMain" id="products"  >
<div class="popupHead">
    Products List
    <input type="submit" class="close" value="" />
    </div>
     
    <div class="bodyPop">
    <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" 
            onrowediting="StockDisplayGrid_RowEditing" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" >
                 <Columns>

                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCtrl" runat="server"  onClick="CheckSingleCheckbox(this);"  />
                        </ItemTemplate>
                    </asp:TemplateField>

                     <%--<asp:BoundField DataField="Product_Name" HeaderText="Product Name"   /> --%>

                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:BoundField DataField="Product_Name" HeaderText="Product Name"   />
                     <%--<asp:BoundField DataField="Description" HeaderText="Description"   /> --%>
                     <%--<asp:BoundField DataField="itemStrength" HeaderText="Item Strength"   />   ProductName,UnitCost --%>
                     <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost"   />
                    <asp:BoundField DataField="ProductID" HeaderText="Product" />
                     <asp:BoundField DataField="QtyinHand" HeaderText="QtyinHand" />

                      
                      
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
     
     <asp:Button ID="SelectProduct" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectProduct_Click"  />
        </div>
 </div>

 <script type="text/javascript">
     function CheckSingleCheckbox(ob) {
         var grid = ob.parentNode.parentNode.parentNode;
         var inputs = grid.getElementsByTagName("input");
         for (var i = 0; i < inputs.length; i++) {
             if (inputs[i].type == "checkbox") {
                 if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                     inputs[i].checked = false;
                 }
             }
         }
     }
    </script>