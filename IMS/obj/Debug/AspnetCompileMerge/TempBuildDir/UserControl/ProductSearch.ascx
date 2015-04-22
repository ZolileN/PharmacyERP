<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="ProductSearch.ascx.cs" Inherits="IMS.UserControl.ProductSearch" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<script>
    window.onunload = refreshParent;
    function refreshParent() {
        window.opener.location.reload();
    }
</script>
 
<div id="Div1" style="overflow:auto; background-color: #ece9ec">
    <h3> Search Product</h3>
    <br/>
    <br/>

<div class="form-horizontal">
    <div class="form-group">
        <asp:Label runat="server" ID="selSys" AssociatedControlID="SysDDL"  CssClass="col-md-2 control-label">Select Product</asp:Label>
        <div class="col-md-10">
            <asp:DropDownList  runat="server" ID="SysDDL" CssClass="form-control" Width="29%" AutoPostBack="true" OnSelectedIndexChanged="SysDDL_SelectedIndexChanged"/>
            <br />  
        </div>
    </div>
    
    <asp:Button ID="btnSelSystem" runat="server" OnClick="btnSelSystem_Click" Text="Select" CssClass="btn btn-default" /> 
</div>
</div>