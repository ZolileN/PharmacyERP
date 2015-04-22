<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Select_System.ascx.cs" Inherits="IMS.UserControl.uc_Select_System" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script>
    window.onunload = refreshParent;
    function refreshParent() {
        window.opener.location.reload();
    }
</script>
<div id="Div1" style="overflow:auto; background-color: #ece9ec">
    <h3> Select System</h3>
    <br/>
    <br/>
    <div class="form-group">
    <%--<asp:Label runat="server" ID="selSys" AssociatedControlID="SysDDL"  CssClass="col-md-2 control-label">Select System</asp:Label>--%>
        <div class="col-md-10">
            <asp:DropDownList  runat="server" ID="SysDDL" CssClass="form-control" Width="100%" AutoPostBack="true" />
            <br />  
        </div>
    </div>
    
    <asp:Button ID="btnSelSystem" runat="server" OnClick="btnSelSystem_Click" Text="Submit" CssClass="btn btn-default" /> 
</div>