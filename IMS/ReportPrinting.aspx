<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportPrinting.aspx.cs" Inherits="IMS.ReportPrinting" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script type="text/javascript">

        var filename;
        function winPrintDialogBox(filename) {
            var oShell = new ActiveXObject("WScript.Shell");
            sRegVal = 'HKEY_CURRENT_USER\\Software\\Microsoft\\Windows NT\\CurrentVersion\\Windows\\Device';

            var sName = oShell.RegRead(sRegVal)
            if (sName == '') {
                alert('Please, Check the Default Printer');
            }
            document.getElementById("hdnResultValue").value = sName;
            return sName;
        }
        function HandleError() {
            alert("\nNothing was printed. \n\nIf you do want to print this page, then\nclick on the printer icon in the toolbar above.")
            return false;
        }

        function printPDF(htmlPage) {
            var w = window.open("about:blank");
            w.document.write(htmlPage);
            if (navigator.appName == 'Microsoft Internet Explorer') window.print();
            else w.print();
        }


        function printPartOfPage(elementId) {
            var printContent = document.getElementById(elementId);
            var windowUrl = 'about:blank';
            var uniqueName = new Date();
            var windowName = 'Print' + uniqueName.getTime();
            var printWindow = window.open(windowUrl, windowName, 'left=50000,top=50000,width=0,height=0');

            printWindow.document.write(printContent.innerHTML);
            printWindow.document.close();
            printWindow.focus();
            printWindow.print();
            printWindow.close();
        }
        function PrintingReport() {
            printPartOfPage('printDiv')
        }

       
    </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div id="printDiv" style="display: none" runat="server"></div>
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Report Printing Done</h4></td>
            <td align="right">
                <asp:HiddenField ID="hdnResultValue" runat="server" />
                <asp:Button ID="btnPrint" runat="server" OnClientClick="PrintingReport()" />
                <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" CssClass="btn btn-default btn-large no-print" Text="Go Back" />
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
</asp:Content>
