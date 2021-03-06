﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddSystem.aspx.cs" Inherits="IMS.AddSystem" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Style/chosen.css" />
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <table width="100%">
    <tr><td>

     <h4 id="regTitleWH" runat="server">Add Store</h4>
        <h4 id="EditTitleWH" visible="false" runat="server">Edit Store</h4>
        <h4 id="regTitleSt" runat="server" visible="false">Add Pharmacy</h4>
        <h4 id="EditTitleSt" visible="false" runat="server">Edit Pharmacy</h4>

    </td>
    <td align="right"> 
                <asp:Button ID="btnAddSystem" runat="server" OnClick="btnAddSystem_Click" Text="SAVE"  CssClass="btn btn-primary" ValidationGroup="exSave" />
                <asp:Button ID="btnEditSystem" runat="server" OnClick="btnEditSystem_Click" Text="SAVE" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnDeleteSystem" runat="server" OnClick="btnDeleteSystem_Click" Text="DELETE" CssClass="btn btn-default" visible="false"/>
                <asp:Button ID="btnCancelSystem" runat="server" OnClick="btnCancelSystem_Click" Text="CANCEL" CssClass="btn btn-default"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>
        </td>
                </tr>
                <tr><td height="5"></td></tr>
                </table>
    <hr>

     <br />

 
        
        <table cellpadding="3" cellspacing="3" width="100%" class="form-fields">
            <tr>

                <td>
                    <asp:Label runat="server" ID="lblStoreType" AssociatedControlID="ddlPharmacyType" Visible="false" CssClass="control-label">Pharmacy Type</asp:Label>
                </td>
                <td>

                    <asp:DropDownList ID="ddlPharmacyType" runat="server"  CssClass="form-control" Visible="false">
                         
                    </asp:DropDownList>

                </td>

                <td>
                    <asp:Label runat="server" ID="selSys" AssociatedControlID="SysDDL" Visible="false" CssClass="control-label">Select System</asp:Label>
                </td>
                <td>
                    <asp:DropDownList Visible="false" runat="server" ID="SysDDL" CssClass="form-control" Width="" AutoPostBack="true" OnSelectedIndexChanged="SysDDL_SelectedIndexChanged"/>
                </td>
                
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="sysName" CssClass="control-label"> Name</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="sysName" CssClass="form-control" Enabled="True" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="sysName" CssClass="text-danger" ErrorMessage="System Name field is required." ValidationGroup="exSave" />
                </td>
                <td>
                    <asp:Label runat="server" AssociatedControlID="sysDesc" CssClass="control-label"> Description</asp:Label>
                </td>
                <td>
                     <asp:TextBox runat="server" ID="sysDesc" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="sysAddress" CssClass="control-label"> Address</asp:Label></td>
                <td>
                     <asp:TextBox runat="server" ID="sysAddress" CssClass="form-control" />
                </td>
                <td>
                     <asp:Label runat="server" AssociatedControlID="sysPhone" CssClass="control-label"> Phone</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="sysPhone" CssClass="form-control" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="sysFax" CssClass="control-label"> Fax</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="sysFax" CssClass="form-control" />
                </td>
                 <td>
                    <asp:Label runat="server" id="lblPhar" AssociatedControlID="pharmacyID" CssClass="control-label" Visible="false">Pharmacy ID</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="pharmacyID" CssClass="form-control" Visible="false"/>
                </td>
            </tr>
            <tr>
               
                <td> <asp:Label runat="server" ID="lblBarterID" Visible="false" AssociatedControlID="txtBarterValue" CssClass="control-label">Barter Exchange ID</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" Visible="false" ID="txtBarterValue" CssClass="form-control" />
                </td>
                <td>
                    <asp:Label runat="server" Visible="false" AssociatedControlID="sysID" CssClass="control-label">SystemID</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" Visible="false" ID="sysID" CssClass="form-control" />
                </td>
                
            </tr>
            <tr>
                <td colspan="100%">&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td colspan="100%">

               

                </td>
            </tr>

        </table>

    

        
        
</asp:Content>
