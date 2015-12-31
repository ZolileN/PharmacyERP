<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_InventoryListDetailsReport.aspx.cs" Inherits="IMS.rpt_InventoryListDetailsReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="print_uc" Src="~/UserControl/uc_printBarcode.ascx" %>


<%@ Register TagName="ProductsPopup" TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>

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
                    <h4>Inventory List Details Report</h4>
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
                <asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ProductDept_SelectedIndexChanged" /></td>
            <td>
                <asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="control-label" Visible="true">Product Category</asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" Visible="true" AutoPostBack="True" OnSelectedIndexChanged="ProductCat_SelectedIndexChanged" /></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass=" control-label" Visible="true"> Product SubCategory </asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%" Visible="true" /></td>
            
            <td>
                <asp:Label runat="server" ID="lblProd" AssociatedControlID="txtSearch" CssClass="control-label">Product Criteria</asp:Label></td>
           <td>
                <input type="text" id="txtSearch" runat="server" name="txtSearch"  /><br />
                <sub>&nbsp;e.g Panadol</sub>
            </td>
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

    <asp:Button ID="_editPopupButton" runat="server" Style="display: none" />
    <%--<cc1:ModalPopupExtender ID="mpeEditProduct" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" 
            PopupDragHandleControlID="_prodEditPanel" TargetControlID="_editPopupButton" PopupControlID="_prodEditPanel" BehaviorID="EditModalPopupMessage">
        </cc1:ModalPopupExtender>--%>

    <asp:Panel ID="_prodEditPanel" runat="server" Width="100%" Style="display: none">
        <asp:UpdatePanel ID="_prodEdit" runat="server">
            <ContentTemplate>
                <uc:print_uc ID="ucPrint" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
