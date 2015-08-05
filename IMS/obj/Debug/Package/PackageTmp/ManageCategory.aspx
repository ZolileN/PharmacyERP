<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageCategory.aspx.cs" Inherits="IMS.ManageCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <br />
      <br />
    <h4>Manage Category</h4>
     
        <hr />
    <asp:GridView ID="CategoryDisplayGrid" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="false" OnPageIndexChanging="CategoryDisplayGrid_PageIndexChanging" OnRowCancelingEdit="CategoryDisplayGrid_RowCancelingEdit" ShowFooter="true"
                OnRowCommand="CategoryDisplayGrid_RowCommand"
                OnRowDataBound="CategoryDisplayGrid_RowDataBound" AllowSorting="true" OnSorting="CategoryDisplayGrid_Sorting"
                OnRowDeleting="CategoryDisplayGrid_RowDeleting"
                OnRowEditing="CategoryDisplayGrid_RowEditing ">

                <Columns>
                    <asp:TemplateField HeaderText="Category ID">
                        <ItemTemplate>
                            <asp:Label ID="lblCat_ID" runat="server" Text='<%# Eval("categoryId") %>'></asp:Label>
                        </ItemTemplate>

                        <FooterTemplate>
                            <asp:Label ID="lblAdd" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Name" SortExpression="categoryName">
                        <ItemTemplate>
                            <asp:Label ID="lblCat_Name" runat="server" Text='<%# Eval("categoryName") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtname" runat="server" Text='<%#Eval("categoryName") %>'></asp:TextBox>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <asp:TextBox ID="txtAddname" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Department Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Id" runat="server" Text='<%#Eval("DepartmentName") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:DropDownList ID="ddlDepName" runat="server">
                            </asp:DropDownList>

                        </EditItemTemplate>

                        <FooterTemplate>
                            <asp:TextBox ID="txtAddDepID" runat="server" Visible="false"></asp:TextBox>
                            <asp:DropDownList ID="ddlAddDepName" runat="server">
                            </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="btn btn-default"  Text="Edit" runat="server" CommandName="Edit"></asp:LinkButton>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton ID="btnDelete" CssClass="btn btn-default"  Text="Delete" runat="server" CommandName="Delete"></asp:LinkButton>
                            </span>
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:LinkButton ID="btnUpdate" CssClass="btn btn-default"  Text="Update" runat="server" CommandName="UpdateCategory"></asp:LinkButton>
                            <asp:LinkButton ID="btnCancel" CssClass="btn btn-default"  Text="Cancel" runat="server" CommandName="Cancel"></asp:LinkButton>
                        </EditItemTemplate>

                        <FooterTemplate>
                            <asp:Button ID="btnAddRecord" CssClass="btn btn-default"  runat="server" Text="Add" CommandName="Add"></asp:Button>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
</asp:Content>
