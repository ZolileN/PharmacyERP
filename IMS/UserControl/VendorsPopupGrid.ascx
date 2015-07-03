<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorsPopupGrid.ascx.cs" Inherits="IMS.UserControl.VendorsPopupGrid" %>

 <div class="popupMain" id="vendors">
<div class="popupHead">
    Vendor List
    <input type="submit" class="close" value="" />
    </div>
     
      <div class="bodyPop">
          <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl"  id="vendorSelect" width="">

       <tr>
           <td><label id="lblSelectVendor" visible="false" runat="server" >Select Vendor</label></td>
           <td>
               <asp:TextBox ID="txtVendor" visible="false" runat="server" CssClass="form-control product" ></asp:TextBox>
               <asp:Button ID="btnSearch" visible="false" runat="server" CssClass="search-btn getProducts" OnClick="btnSearch_Click" />
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
           </tr>
      </table>
    <asp:GridView ID="gdvVendor" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing">
        <Columns>
           
             <asp:TemplateField HeaderText="Action">
                <ItemTemplate >
                     
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="CheckSingleCheckbox(this);"  />
                   
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
            <asp:TemplateField HeaderText="ID" >
                <ItemTemplate>
                    <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
    
     <asp:Button ID="SelectVendor" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectVendor_Click" />
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