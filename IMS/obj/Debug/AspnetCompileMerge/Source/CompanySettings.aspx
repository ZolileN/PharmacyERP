<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompanySettings.aspx.cs" Inherits="IMS.CompanySettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4 id="regTitle" runat="server">Company Settings</h4>
     
        <hr />
        <br />
     
       
             
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="cmpName" CssClass="col-md-2 control-label">Company Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="cmpName" CssClass="form-control" Enabled="True" />
                
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="cmpPhne" CssClass="col-md-2 control-label">Company Contact</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="cmpPhne" CssClass="form-control" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="cmpAddress" CssClass="col-md-2 control-label">Company Address</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="cmpAddress" CssClass="form-control" />
            </div>
        </div>

         

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="cmpFax" CssClass="col-md-2 control-label">Company Fax</asp:Label>
           <div class="col-md-10">
                <asp:TextBox runat="server" ID="cmpFax" CssClass="form-control" />
            </div>
        </div>  

      
        
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnAddSystem" runat="server"  Text="ADD"  CssClass="btn btn-default" ValidationGroup="exSave" />
                <asp:Button ID="btnEditSystem" runat="server" Text="EDIT" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnCancelSystem" runat="server" Text="CANCEL" CssClass="btn btn-default"/>
               <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
               
            </div>
        </div>
    </div>
</asp:Content>
