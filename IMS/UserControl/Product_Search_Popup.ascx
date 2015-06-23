<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Product_Search_Popup.ascx.cs" Inherits="IMS.UserControl.Product_Search_Popup" %>


<div class="popupMain" id="products">
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
                         <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" enabled="false" runat="server" onclick="CheckAllEmp(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCtrl" runat="server" />
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:BoundField DataField="Desc_Name" HeaderText="ProductName"   />

                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:BoundField DataField="Desc_Name" HeaderText="Description"   /> 
                     <asp:BoundField DataField="itemStrength" HeaderText="Item Strength"   />
                     <asp:BoundField DataField="itemForm" HeaderText="Item Form"   />
                     <asp:BoundField DataField="itemPackSize" HeaderText="Pack Size"   />
                     <asp:TemplateField HeaderText="Product ID">
                        <ItemTemplate>
                            <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                      
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
     <asp:Button ID="SelectProduct" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectProduct_Click" Visible="true" />
     </div>
    <script type="text/javascript" language="javascript">
        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=StockDisplayGrid.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>
 </div>