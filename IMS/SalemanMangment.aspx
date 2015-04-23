<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalemanMangment.aspx.cs" Inherits="IMS.SalemanMangment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">

        <asp:Button ID="btnAddNew" runat="server" Text="AddNew" CssClass="btn btn-default" OnClick="btnAddNew_Click" />

        <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="UserName" CssClass=" control-label">Name</asp:Label>

                </td>

                <td>
                    <asp:TextBox runat="server"  ID="UserName" CssClass="form-control" Enabled="True" />

                </td>

                <td>
                    <asp:Label runat="server" AssociatedControlID="UserAddress" CssClass=" control-label">Address</asp:Label>

                </td>

                <td>
                    <asp:TextBox runat="server" ID="UserAddress" CssClass="form-control" Enabled="True" />

                </td>
            </tr>

            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="UserContact" CssClass="control-label">Contact:#</asp:Label>
                </td>

                <td>
                    <asp:TextBox runat="server" ID="UserContact" CssClass="form-control" Enabled="True" />

                </td>

                <td>
                    <asp:Label runat="server" AssociatedControlID="ddlUserRole" CssClass=" control-label">User Role</asp:Label>
                </td>

                <td>
                    <asp:DropDownList runat="server" ID="ddlUserRole" CssClass="form-control" Width="29%" EnableViewState="true"  />
                    <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlUserRole" CssClass="text-danger" ErrorMessage="The user role field is required." ValidationGroup="exSave" />

                </td>
            </tr>

            <tr>
                <td></td>
                <td colspan="100%" class="btnSpace">

                    <asp:Button ID="btnSearch" runat="server" Text="Search" CssClass="btn btn-default" OnClick="btnSearch_Click" />
                    <asp:Button ID="btnClear" runat="server" Text="Clear" CssClass="btn btn-default" OnClick="btnClear_Click" />
                </td>
            </tr>

        </table>









    </div>

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
                <EditItemTemplate>


                   <%-- <asp:DropDownList ID="ddlUserRole" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("[U_RolesID]") %>' Width="220px" EnableViewState="true"></asp:DropDownList>--%>
                </EditItemTemplate>
                <ItemStyle Width="150px" HorizontalAlign="Left" />
            </asp:TemplateField>
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:LinkButton CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>' />
                    <span onclick="return confirm('Are you sure you want to delete this record?')">
                        <asp:LinkButton CssClass="btn btn-default" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>' />
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
