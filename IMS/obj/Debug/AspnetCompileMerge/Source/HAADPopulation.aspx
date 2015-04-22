<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HAADPopulation.aspx.cs" Inherits="IMS.HAADPopulation" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:FileUpload ID="HaadFileImport" runat="server" />
    <asp:Button ID="Button1" runat="server" Text="Button" OnClick="Button1_Click" />
    <asp:GridView ID="gvAPI" runat="server" AutoGenerateColumns="true"></asp:GridView>
</asp:Content>
