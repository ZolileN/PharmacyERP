<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="CrystalReportViewer.aspx.cs" Inherits="IMS.CrystalReportViewer" %>
<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
    <asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
    <webopt:bundlereference runat="server" path="~/Content/css" />
    <script type="text/javascript" src="Scripts/jquery.min.js"></script>
    <link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
    <link href="Style/theme.css" rel="stylesheet" type="text/css"  />
    <link href="Style/fonts.css" rel="stylesheet" type="text/css"  />
    <script type="text/javascript">

        function stopRKey(evt) {
            var evt = (evt) ? evt : ((event) ? event : null);
            var node = (evt.target) ? evt.target : ((evt.srcElement) ? evt.srcElement : null);
            if ((evt.keyCode == 13) && (node.type == "text")) { return false; }
        }

        document.onkeypress = stopRKey;

    </script>
    <style>
        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }
    </style>
</head>
<body>
    <div class="container ">
    <form id="form1" runat="server">
    <div class="top-head">
                
                <div class="logo">
                </div>
            </div>
     <div class="navs-cont no-print">
         <div class="navigation no-print ">
        <h1><font color="white">&nbsp &nbsp Crystal Report Viewer</font></h1>
          <div style="clear: left;"></div>
                
            </div><div style="clear: left;"></div>
          </div>

    <div style="clear: left;"></div>
    <div id="dvReport" class="body-cont" align="center">
     <asp:Button ID="btnPrint" runat="server" PostBackUrl="~/ReportPrinting.aspx" CssClass="btn btn-primary btn-large no-print" Text="Print" />
     <asp:Button ID="btnExport" runat="server" CssClass="btn btn-info btn-large no-print" Text="Export" Visible="false" />
     <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" CssClass="btn btn-default btn-large no-print" Text="Go Back" />
     <br /><br />
     <CR:CrystalReportViewer ID="CrystalReportViewer1" Visible="true" runat="server" AutoDataBind="True" Height="1202px" Width="1104px" PrintMode="Pdf" HasPrintButton="False" HasExportButton="False" ReuseParameterValuesOnRefresh="True" ShowAllPageIds="True"/>
     </div>
     </form>
     </div>
</body>
</html>
