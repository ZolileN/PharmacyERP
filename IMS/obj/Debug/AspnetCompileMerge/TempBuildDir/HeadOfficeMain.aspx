<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="HeadOfficeMain.aspx.cs" Inherits="IMS.HeadOfficeMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h2>IMS Head Office</h2>
         <br />
         <br />
         <asp:Button ID="btnMngStore" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Stores" OnClick="btnMngStore_Click"/>
         <asp:Button ID="btnMngWareHouse" runat="server" CssClass="btn btn-primary btn-large" Text="Manage WareHouse" OnClick="btnMngWareHouse_Click"/>
         <asp:Button ID="btnCmpnyInfo" runat="server" CssClass="btn btn-primary btn-large" Text="Manage Company Information" OnClick="btnCmpnyInfo_Click"/>
         <asp:Button ID="btnViewReport" runat="server" CssClass="btn btn-primary btn-large" Text="View Reports" OnClick="btnViewReport_Click"/>
         <asp:Button ID="BtnRegisterUser" runat="server" CssClass="btn btn-primary btn-large" Text="Register User" OnClick="BtnRegisterUser_Click"/>
</asp:Content>

