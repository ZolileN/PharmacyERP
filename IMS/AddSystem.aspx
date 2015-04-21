<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSystem.aspx.cs" Inherits="IMS.AddSystem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Style/chosen.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4 id="regTitleWH" runat="server">Register Warehouse</h4>
        <h4 id="EditTitleWH" visible="false" runat="server">Edit Warehouse</h4>
        <h4 id="regTitleSt" runat="server" visible="false">Register Store</h4>
        <h4 id="EditTitleSt" visible="false" runat="server">Edit Store</h4>
        <hr />
        <br />
     
        <div class="form-group">
            <asp:Label runat="server" ID="selSys" AssociatedControlID="SysDDL" Visible="false" CssClass="col-md-2 control-label">Select System</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList Visible="false" runat="server" ID="SysDDL" CssClass="form-control" Width="29%" AutoPostBack="true" OnSelectedIndexChanged="SysDDL_SelectedIndexChanged"/>
                <br />               
            </div>
        </div>
            
             
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="sysName" CssClass="col-md-2 control-label">System Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="sysName" CssClass="form-control" Enabled="True" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="sysName" CssClass="text-danger" ErrorMessage="System Name field is required." ValidationGroup="exSave" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="sysDesc" CssClass="col-md-2 control-label">System Description</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="sysDesc" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="sysAddress" CssClass="col-md-2 control-label">System Address</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="sysAddress" CssClass="form-control" />
            </div>
        </div>

         <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="sysPhone" CssClass="col-md-2 control-label">System Contact</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="sysPhone" CssClass="form-control" />
            </div>
        </div>    

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="sysFax" CssClass="col-md-2 control-label">System Fax</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="sysFax" CssClass="form-control" />
            </div>
        </div>  

        <div class="form-group">
            <asp:Label runat="server" id="lblPhar" AssociatedControlID="pharmacyID" CssClass="col-md-2 control-label" Visible="false">Pharmacy ID</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="pharmacyID" CssClass="form-control" Visible="false"/>
            </div>
        </div>  
          <div class="form-group">
            <asp:Label runat="server" Visible="false" AssociatedControlID="sysID" CssClass="col-md-2 control-label">SystemID</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" Visible="false" ID="sysID" CssClass="form-control" />
            </div>
        </div>     
        
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnAddSystem" runat="server" OnClick="btnAddSystem_Click" Text="ADD"  CssClass="btn btn-default" ValidationGroup="exSave" />
                <asp:Button ID="btnEditSystem" runat="server" OnClick="btnEditSystem_Click" Text="EDIT" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnDeleteSystem" runat="server" OnClick="btnDeleteSystem_Click" Text="DELETE" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnCancelSystem" runat="server" OnClick="btnCancelSystem_Click" Text="CANCEL" CssClass="btn btn-default"/>
               <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
               
            </div>
        </div>
    </div>
</asp:Content>
