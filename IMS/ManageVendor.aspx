﻿<%@ Page Title="    " Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageVendor.aspx.cs" Inherits="IMS.ManageVendor" %>
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
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Manage Vendors</h4></td>
            <td align="right">
                <asp:Button ID="btnAddVendor" runat="server" OnClick="btnAddVendor_Click" Text=" + Add Vendor" CssClass="btn btn-success btn-large" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="Refresh List" CssClass="btn btn-info btn-large" />
                <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click"/>       
          </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
    
        <table cellspacing="5" cellpadding="5" border="0" width="50%" class="formTbl">
            <tr>
                <td>
                     <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Search by Name </asp:Label>
                </td>

            <td>
                <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product"  />
                <asp:Button ID="btnSearch" runat="server" CssClass="btn btn-sm btn-primary" Text="Search" OnClick="btnSearch_Click" />
              


                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control product" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged" DataSourceID="StockAtSqlDataSource" DataTextField="SupName" Visible="false" DataValueField="SuppID" />
                <asp:SqlDataSource ID="StockAtSqlDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSConnectionString %>" SelectCommand="SELECT [SuppID], [SupName] FROM [tblVendor]"></asp:SqlDataSource>

                <asp:ImageButton ID="btnSearchProduct" runat="server" Visible="false" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
            
            </td>
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
                            <asp:LinkButton CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex%>'/>
                            <%--<asp:Label ID="Label1"  CssClass="btn btn-default edit-btn" runat="server" Text=''> 
                                 <a href="AddEditVendor.aspx?Id=<%# Eval("SuppID")%>">
                                     Edit
                            </asp:Label>--%>
                            <asp:Label ID="Label2" runat="server" Text=''>
                              <%--  CommandName="del" CommandArgument='<%# Eval("SuppID")%>' --%>
                                <span onclick="return confirm('Are you sure you want to delete this record?')">
                                    <asp:LinkButton ID="lnkDelete"  CssClass="btn btn-default del-btn"  runat="server" CommandName="Delete">
                                    Delete
                                    </asp:LinkButton>
                                </span>
                             </asp:Label>
                             
                        </ItemTemplate>
                     </asp:TemplateField>
                </Columns>
            <PagerStyle CssClass = "GridPager" />
    </asp:GridView>
    
</asp:Content>
