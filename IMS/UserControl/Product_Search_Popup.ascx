<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="Product_Search_Popup.ascx.cs" Inherits="IMS.UserControl.Product_Search_Popup" %>


<div class="popupMain" id="products">
<div class="popupHead">
     <script src="Scripts/FindPharmacyName.js"></script>
       <style>

           .suggest_link 
	       {
	       background-color: #FFFFFF;
	       padding: 2px 6px 2px 6px;
	       }	
	       .suggest_link_over
	       {
	       background-color: #3366CC;
	       padding: 2px 6px 2px 6px;	
	       }	
	       #search_suggest 
	       {
	       position: absolute;
	       background-color: #FFFFFF;
	       text-align: left;
	       border: 1px solid #000000;	
           overflow:auto;
       		
	       }

       </style>
    Products List
    <input type="submit" class="close" value="" />
    </div>
     
    <div class="bodyPop">
         <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl"  id="vendorSelect" width="">

       <tr>
           <td><label id="lblSelectVendor" visible="false" runat="server" >Select Pharmacy</label></td>
           <td>
              <input type="text" id="txtSearch" visible="false" runat="server" name="txtSearch"   onkeyup="searchSuggest(event);" autocomplete="off"  /> 
                <div id="search_suggest" style="visibility: hidden;" ></div>
               <asp:Button ID="btnSearchStore" visible="false" runat="server" CssClass="search-btn getProducts" OnClick="btnSearchStore_Click" />
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
           </tr>
      </table>
          <hr />
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

                     <%--<asp:BoundField DataField="Desc_Name" HeaderText="ProductName"   />--%>

                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:BoundField DataField="Desc_Name" HeaderText="ProductName"   /> 
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