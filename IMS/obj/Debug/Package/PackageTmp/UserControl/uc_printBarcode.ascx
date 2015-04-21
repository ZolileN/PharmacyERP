<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_printBarcode.ascx.cs" Inherits="IMS.UserControl.uc_printBarcode" %>
<%@ Register Assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" Namespace="Microsoft.Reporting.WebForms" TagPrefix="rsweb" %>

<%--<asp:ScriptManager ID="ScriptManager1" runat="server"></asp:ScriptManager>--%>
<script type="text/javascript">
    function ShowModalPopup() {
        $find("mpe").show();
        return false;
    }
    function HideModalPopup() {
        $find("mpe").hide();
        return false;
    }
</script>
 
<div id="Div1" style="overflow:auto; background-color: #ece9ec">
   <rsweb:ReportViewer ID="ReportViewer1" runat="server"></rsweb:ReportViewer>
    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
    <%--<asp:Button ID="BtnHide" runat="server" CssClass="btn btn-primary btn-large" Text="Hide" OnClientClick="return HideModalPopup()"/>--%>
</div>