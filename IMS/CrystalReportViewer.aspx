<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReportViewer.aspx.cs" Inherits="IMS.CrystalReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <script type="text/javascript">
        function Print() {
            var dvReport = document.getElementById("dvReport");
            var frame1 = dvReport.getElementsByTagName("iframe")[0];
            if (navigator.appName.indexOf("Internet Explorer") != -1) {
                frame1.name = frame1.id;
                window.frames[frame1.id].focus();
                window.frames[frame1.id].print();
            }
            else {
                var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
                frameDoc.print();
            }
        }
    </script>
</head>
<body>
    <form id="form2" runat="server">
    <asp:Button ID="btnPrint" runat="server" OnClientClick="Print()" Text="Print" />
        &nbsp;
    <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" Text="Back" />

    <div id="dvReport">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" SeparatePages="False" Visible="true" runat="server" AutoDataBind="True" PrintMode="Pdf" HasPrintButton="True" HasExportButton="False"/>
    </div>
    </form>
</body>
</html>