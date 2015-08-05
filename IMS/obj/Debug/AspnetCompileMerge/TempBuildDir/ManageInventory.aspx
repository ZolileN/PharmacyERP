<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageInventory.aspx.cs" Inherits="IMS.ManageInventory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h3>Inventory Management</h3>
         <asp:Button ID="btnManageProducts" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Products" OnClick="btnManageProducts_Click"/>
         <asp:Button ID="btnManageSubCategories" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Subcategories" OnClick="btnManageSubCategories_Click"/>
         <asp:Button ID="btnManageCategories" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Categories" OnClick="btnManageCategories_Click"/>
         <asp:Button ID="btnManageDepartments" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Departments" OnClick="btnManageDepartments_Click"/>
         <asp:Button ID="btnStocks" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Stocks" OnClick="btnStocks_Click"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
     </div>
    
</asp:Content>
