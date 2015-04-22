<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecieveOrder.aspx.cs" Inherits="IMS.RecieveOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <div class="row">
        <h3>Recieve Order</h3>
         <asp:Button ID="btnAutoRecieving" runat="server" CssClass="btn btn-primary btn-large" Text="Auto Recieving" OnClick="btnAutoRecieving_Click"/>
         <asp:Button ID="btnManualRecieving" runat="server" CssClass="btn btn-primary btn-large" Text="Manual Recieving" OnClick="btnManualRecieving_Click"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
     </div>
</asp:Content>
