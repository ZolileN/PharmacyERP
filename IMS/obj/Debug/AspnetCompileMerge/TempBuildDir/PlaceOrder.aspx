<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PlaceOrder.aspx.cs" Inherits="IMS.PlaceOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
        <h3>Place Order</h3><br /><br />
         <asp:Button ID="btnAutoPurchase" runat="server" CssClass="btn btn-primary btn-large" Text="Auto Purchase" OnClick="btnAutoPurchase_Click"/>
         <asp:Button ID="btnManualPurchase" runat="server" CssClass="btn btn-primary btn-large" Text="Manual Purchase" OnClick="btnManualPurchase_Click"/>
         <asp:Button ID="btnEditOrder" runat="server" CssClass="btn btn-primary btn-large" Text="View Placed Orders" OnClick="btnEditOrder_Click" Visible="false"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
   
</asp:Content>
