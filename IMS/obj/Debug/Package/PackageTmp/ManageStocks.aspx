<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageStocks.aspx.cs" Inherits="IMS.ManageStocks" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
        <h3>Stock Management</h3>
         <asp:Button ID="btnAddStock" runat="server" CssClass="btn btn-primary btn-large" Text="Add Stocks" OnClick="btnAddStock_Click"/>
         <asp:Button ID="btnEditStock" runat="server" CssClass="btn btn-primary btn-large" Text="Edit Stocks" OnClick="btnEditStock_Click"/>
         <asp:Button ID="btnViewStocks" runat="server" CssClass="btn btn-primary btn-large" Text="Current Inventory" OnClick="btnViewStocks_Click"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
     </div>
</asp:Content>
