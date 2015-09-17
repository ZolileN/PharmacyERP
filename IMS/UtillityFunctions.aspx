<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UtillityFunctions.aspx.cs" Inherits="IMS.UtillityFunctions" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <asp:Button ID="btnImportManualSO" runat="server" Text="Import Manual Sale Orders" CssClass="btn btn-success btn-default" OnClick="btnImportManualSO_Click" />
    <asp:Button ID="btnImPortManualPO" runat ="server" Text="Adjustment PO" CssClass="btn btn-success btn-default" OnClick="btnImPortManualPO_Click" />
    <asp:Button ID="btnAcceptAdjustmentPO" runat ="server" Text="Accept Adjustment PO" CssClass="btn btn-success btn-default" OnClick="btnAcceptAdjustmentPO_Click" />
</asp:Content>
