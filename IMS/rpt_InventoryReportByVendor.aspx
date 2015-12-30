<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_InventoryReportByVendor.aspx.cs" Inherits="IMS.rpt_InventoryReportByVendor" %>






<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   
    <style>
        .suggest_link {
            background-color: #FFFFFF;
            padding: 2px 6px 2px 6px;
        }

        .suggest_link_over {
            background-color: #3366CC;
            padding: 2px 6px 2px 6px;
        }

        #search_suggest {
            position: absolute;
            background-color: #FFFFFF;
            text-align: left;
            border: 1px solid #000000;
            overflow: auto;
        }
    </style>

    <asp:Label runat="server" ID="NoProductMessage" CssClass="control-label" Visible="false" Text="No Stock Available"></asp:Label>

    <table width="100%">

        <tbody>
            <tr>
                <td>
                    <h4>Inventory Report By Vendor</h4>
                </td>
                <td align="right">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnShowREPORT_Click" OnClientClick="return VerifyCredentials();"  Enabled="true" Text="Show REPORT" CssClass="btn btn-primary" />
                    
                    
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click" />
                    
                    
                </td>
            </tr>
            <tr>
                <td height="5"></td>
            </tr>
        </tbody>

    </table>
    <hr />
    <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
        <tr>
            <td>
                <asp:Label runat="server" AssociatedControlID="VendorID" CssClass="control-label" Visible="true">Select Vendor</asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="VendorID" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True" /></td>
           
        </tr>
    </table>

</asp:Content>
