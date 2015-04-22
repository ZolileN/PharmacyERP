<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageOrders.aspx.cs" Inherits="IMS.ManageOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
        <h3>Order Management</h3><br /><br />
         <asp:Button ID="btnPlaceOrder" runat="server" CssClass="btn btn-primary btn-large" Text="Place Orders" OnClick="btnPlaceOrder_Click"/>
         <asp:Button ID="tbnSalesOrder" runat="server" CssClass="btn btn-primary btn-large" Text="Generate Sale Order" OnClick="tbnSalesOrder_Click"/>
         <asp:Button ID="btnRecievOrder" runat="server" CssClass="btn btn-primary btn-large" Text="Recieve Orders" OnClick="btnRecievOrder_Click"/>
         <asp:Button ID="btnViewOrder" runat="server" CssClass="btn btn-primary btn-large" Text="View Orders" OnClick="btnViewOrder_Click"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
    
</asp:Content>
