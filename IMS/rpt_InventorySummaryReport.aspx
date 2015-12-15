<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_InventorySummaryReport.aspx.cs" Inherits="IMS.rpt_InventorySummaryReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="print_uc" Src="~/UserControl/uc_printBarcode.ascx" %>




<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/SearchSuggest.js"></script>
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
                    <h4>Inventory Summary Report</h4>
                </td>
                <td align="right">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnShowREPORT_Click"  Enabled="true" Text="Show REPORT" CssClass="btn btn-primary" />
                    
                    
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
                <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label" Visible="true">Product Department</asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True" /></td>
           
        </tr>
        
    </table>

    <div class="form-horizontal">
        <div class="form-group">
           

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                </div>
            </div>
        </div>
    </div>

</asp:Content>   