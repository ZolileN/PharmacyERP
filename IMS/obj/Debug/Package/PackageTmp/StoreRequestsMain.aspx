<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreRequestsMain.aspx.cs" Inherits="IMS.StoreRequestsMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <h2>Store Requests</h2>
         <asp:Button ID="btnAutoGenerateRequest" runat="server" CssClass="btn btn-primary btn-large" Text="Auto Generate Requests" OnClick="btnAutoGenerateRequest_Click"/>
         <asp:Button ID="btnManualGenerateRequest" runat="server" CssClass="btn btn-primary btn-large" Text="Manual Generate Requests" OnClick="btnManualGenerateRequest_Click"/>
         <asp:Button ID="ButtonBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" Visible="true" OnClick="ButtonBack_Click"/>
</asp:Content>
