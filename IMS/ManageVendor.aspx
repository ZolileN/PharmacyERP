<%@ Page Title="    " Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageVendor.aspx.cs" Inherits="IMS.ManageVendor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script  type="text/javascript">

         function ConfirmDelete() {
             var val = confirm("Are you sure you want to delete this Vendor?");
             if (val) {
                 return true;
             }
             else {
                 return false;
             }
         }
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <h4>Manage Vendors</h4>
     
    <hr />
    
        <table cellspacing="3" cellpadding="3">
            <tr>
                <td>
                     <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Search by Name </asp:Label>
                </td>

            <td>
                <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product" OnTextChanged="SelectProduct_TextChanged"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
            
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged" Visible="false"/>
            </td>
                </tr>
          
  
    <tr>
        <td></td>
        <td> <asp:Button ID="btnAddVendor" runat="server" OnClick="btnAddVendor_Click" Text="ADD Vendor" CssClass="btn  btn-primary" />
    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh List" CssClass="btn btn-default" /></td>
    </tr>
   </table>

    <br />
    
    <asp:GridView ID="gdvVendor" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging"  ShowFooter="true"
                OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing" >
                <Columns>
                    <asp:TemplateField HeaderText="Vendor Name" SortExpression="SupName">
                        <ItemTemplate>
                            <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("SupName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                             <asp:Label ID="Label4" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
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
                    <asp:TemplateField HeaderText="Contact Person">
                        <ItemTemplate>
                            <asp:Label ID="lblConPer" runat="server" Text='<%# Eval("ConPerson") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- hidden fields --%>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>


                            <asp:Label ID="Label1"  CssClass="btn btn-default edit-btn" runat="server" Text=''> 

                                 <a href="AddEditVendor.aspx?Id=<%# Eval("SuppID")%>">
                                     Edit</asp:Label>

                            <asp:Label ID="Label2" runat="server" Text=''>
                              <%--  CommandName="del" CommandArgument='<%# Eval("SuppID")%>' --%>
                                <asp:LinkButton ID="lnkDelete"  CssClass="btn btn-default del-btn"  runat="server" CommandName="Delete">
                             Delete </asp:LinkButton></asp:Label>

                        </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
    </asp:GridView>
     <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnGoBack_Click"/> 
</asp:Content>
