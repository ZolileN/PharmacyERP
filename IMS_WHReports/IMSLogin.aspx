<%@ Page Title="" Language="C#" MasterPageFile="~/LoginMaster.master" AutoEventWireup="true" CodeBehind="IMSLogin.aspx.cs" Inherits="IMS_WHReports.IMSLogin" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
      

    
        
           
                    

                   
                        <asp:Label runat="server" AssociatedControlID="UserName" CssClass="control-label">User name</asp:Label><br />
                       
                            <asp:TextBox runat="server" ID="UserName" CssClass="login" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="UserName"
                                CssClass="text-danger" ErrorMessage="The user name field is required." />
                       
                    <br />
                        <asp:Label runat="server" AssociatedControlID="Password" CssClass="control-label">Password</asp:Label> <br />
                    
                            <asp:TextBox runat="server" ID="Password" TextMode="Password" CssClass="login" />
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="Password" CssClass="text-danger" ErrorMessage="The password field is required." />
                                <br /><asp:CheckBox runat="server" ID="RememberMe" />
                                <asp:Label runat="server" AssociatedControlID="RememberMe">Remember me?</asp:Label>
                            <asp:Button ID ="btnLogin" runat="server" OnClick="btnLogin_Click" Text="Log in" CssClass="blue fl-r" />
               
     

   
</asp:Content>
