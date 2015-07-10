<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReportViewer.aspx.cs" Inherits="IMS.CrystalReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form2" runat="server">
    <asp:Button ID="btnPrint" runat="server" PostBackUrl="~/ReportPrinting.aspx" Text="Print" />
        &nbsp;
    <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" Text="Back" />

    <div id="dvReport">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" Visible="true" runat="server" AutoDataBind="True" Height="1202px" Width="1104px" PrintMode="Pdf" HasPrintButton="False" HasExportButton="False"/>
    </div>
    </form>
</body>
</html>