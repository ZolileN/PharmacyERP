<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportPrinting.aspx.cs" Inherits="IMS.ReportPrinting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Report Printing Done</h4></td>
            <td align="right">
                <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" CssClass="btn btn-default btn-large no-print" Text="Go Back" />
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
</asp:Content>
