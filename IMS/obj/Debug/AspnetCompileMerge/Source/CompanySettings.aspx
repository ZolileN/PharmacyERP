<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CompanySettings.aspx.cs" Inherits="IMS.CompanySettings" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="form-horizontal">
        <h4 id="regTitle" runat="server">Company Settings</h4>
     
        <hr />
        <br />
     
       <table class="form-fields" width="100%">
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="cmpName" CssClass="control-label">Company Name</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="cmpName" CssClass="form-control" Enabled="True" />
                </td>
                <td>
                     <asp:Label runat="server" AssociatedControlID="cmpPhne" CssClass="control-label">Company Contact</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="cmpPhne" CssClass="form-control" />
                </td>
            </tr> 
           <tr>
               <td>
                   <asp:Label runat="server" AssociatedControlID="cmpAddress" CssClass="control-label">Company Address</asp:Label>
               </td>
               <td>
                    <asp:TextBox runat="server" ID="cmpAddress" CssClass="form-control" />
               </td>
               <td>
                   <asp:Label runat="server" AssociatedControlID="cmpFax" CssClass="control-label">Company Fax</asp:Label>
               </td>
               <td>
                    <asp:TextBox runat="server" ID="cmpFax" CssClass="form-control" />
               </td>
           </tr>
           <tr>
               <td colspan="100%">&nbsp;</td>
           </tr>
           <tr>
               <td></td>
               <td colspan="100%"><asp:Button ID="btnAddSystem" runat="server"  Text="ADD"  CssClass="btn btn-primary" ValidationGroup="exSave" />
                <asp:Button ID="btnEditSystem" runat="server" Text="EDIT" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnCancelSystem" runat="server" Text="CANCEL" CssClass="btn btn-default"/>
               <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click" /></td>
           </tr>
       </table>
       
     
    </div>
</asp:Content>
