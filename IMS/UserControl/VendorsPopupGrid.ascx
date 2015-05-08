<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorsPopupGrid.ascx.cs" Inherits="IMS.UserControl.VendorsPopupGrid" %>

 <div class="popupMain" id="vendors">
<div class="popupHead">
    Vendor List
    <input type="submit" class="close" value="" />
    </div>
     
    
    <asp:GridView ID="gdvVendor" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing">
        <Columns>
           
             <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:CheckBox ID="chkCtrl" runat="server" />
                   
                </ItemTemplate>
            </asp:TemplateField>
            <asp:BoundField DataField="SupName" HeaderText="Vendor Name"   />
            <%--<asp:TemplateField HeaderText="Vendor Name" SortExpression="SupName">
                <ItemTemplate>
                    <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("SupName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>--%>
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
            <asp:TemplateField HeaderText="ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
     
     <asp:Button ID="SelectVendor" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectVendor_Click" />
 </div>