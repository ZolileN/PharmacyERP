<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserManagment.aspx.cs" Inherits="IMS.UserManagment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
        <table width="100%">
            <tr><td>
            <h4>User Management</h4>
            </td>
            <td align="right"> 
                <asp:Button ID="btnAddSalesman" runat="server" Text="+ Add User" CssClass="btn btn-success btn-large" OnClick="btnAddSalesman_Click" />
                <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click" />   
				         
                 </td>
                        </tr>
                        <tr><td height="5"></td></tr>
                        </table>
            <hr>
       

    <asp:GridView ID="SalemanDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false"  OnRowEditing=" SalemanDisplayGrid_RowEditing" OnRowUpdating="SalemanDisplayGrid_RowUpdating" OnRowDeleting="SalemanDisplayGrid_RowDeleting" OnPageIndexChanging="SalemanDisplayGrid_PageIndexChanging" OnRowCancelingEdit="SalemanDisplayGrid_RowCancelingEdit">
        <Columns>
           
            <asp:TemplateField HeaderText="User ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblUserID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UserID") %>' Width="110px"></asp:Label>
                </ItemTemplate>

                <ItemStyle Width="220px" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Name">
                <ItemTemplate>
                    <asp:Label ID="lblName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("U_EmpID") %>' Width="160px"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="Name" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("U_EmpID") %>' Width="160px"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="220px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label ID="lblAddress" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Address") %>' Width="150px"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="Address" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Address") %>' Width="150px"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="240px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <asp:Label ID="lblPhone" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Contact") %>' Width="120px"></asp:Label>
                </ItemTemplate>
                <EditItemTemplate>
                    <asp:TextBox ID="Phone" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Contact") %>' Width="120px"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="220px" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Role" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblddlUserRole" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("[U_RolesID]") %>' Width="100px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="150px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Role Name" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblroleName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("[user_RoleName]") %>' Width="100px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="150px" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>' />
                    <span onclick="return confirm('Are you sure you want to delete this record?')">
                        <asp:LinkButton CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>' />
                    </span>
                </ItemTemplate>
                <EditItemTemplate>
                    <%--<asp:LinkButton CssClass="btn btn-default" ID="btnUpadte" Text="Update" runat="server" CommandName="Update" CommandArgument='<%# Container.DisplayIndex  %>' />--%>
                    <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CommandArgument='<%# Container.DisplayIndex  %>' />
                </EditItemTemplate>
                <ItemStyle Width="180px" HorizontalAlign="Left" />
            </asp:TemplateField>
        </Columns>
    </asp:GridView>
</asp:Content>
