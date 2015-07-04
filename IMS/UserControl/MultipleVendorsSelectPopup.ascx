<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="MultipleVendorsSelectPopup.ascx.cs" Inherits="IMS.UserControl.MultipleVendorsSelectPopup" %>

 <div class="popupMain" id="vendors">
<div class="popupHead">
     <script src="Scripts/SuggestPharmacyName.js"></script>
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
    Vendor List
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
    <asp:GridView ID="gdvVendor" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing">
        <Columns>
           
              <asp:TemplateField HeaderText="Action">
                         <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll"  runat="server" onclick="CheckAllEmp(this);" />
                        </HeaderTemplate>
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCtrl" runat="server" />
                        </ItemTemplate>
             </asp:TemplateField>

            <asp:BoundField DataField="SupName" HeaderText="Vendor Name"   />
         
            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="City">
                <ItemTemplate>
                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Country">
                <ItemTemplate>
                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Fax">
                <ItemTemplate>
                    <asp:Label ID="lblFax" runat="server" Text='<%# Eval("Fax") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField HeaderText="ID" DataField="SuppID" />
            <asp:TemplateField HeaderText="ID" >
                <ItemTemplate>
                    <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
     
     <asp:Button ID="SelectMultipleVendor" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectMultipleVendor_Click" />
 </div>
</div>
   <script type="text/javascript"  >
       function CheckAllEmp(Checkbox) {
           var GridVwHeaderChckbox = document.getElementById("<%=gdvVendor.ClientID %>");
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
            }
        }
    </script>