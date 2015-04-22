<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageWarehouse.aspx.cs" Inherits="IMS.ManageWarehouse" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="sel_uc" Src="~/UserControl/uc_Select_System.ascx" %>
<asp:Content ID="Content2" ContentPlaceHolderID="HeadContent" runat="server">
    <link rel="stylesheet" href="Style/chosen.css" />
</asp:Content>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h2>Manage WareHouse</h2>
         <br />
         <br />
         <asp:Button ID="btnAddWH" runat="server" CssClass="btn btn-primary btn-large" Text="Add WareHouse" OnClick="btnAddWH_Click"/>
         <asp:Button ID="btnEdit" runat="server" CssClass="btn btn-primary btn-large" Text="Edit WareHouse" OnClick="btnEdit_Click"/>
         <asp:Button ID="btnViewWareHouse" runat="server" CssClass="btn btn-primary btn-large" Text="View Warehouse" OnClick="btnViewWareHouse_Click"/>
         <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
         <br/>
        <asp:Button ID="_editPopupButton" runat="server" Style="display: none" />
        <cc1:ModalPopupExtender ID="mpeEditProduct" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" 
            PopupDragHandleControlID="_prodEditPanel" TargetControlID="_editPopupButton" PopupControlID="_prodEditPanel" BehaviorID="EditModalPopupMessage">
        </cc1:ModalPopupExtender>

        <asp:Panel ID="_prodEditPanel" runat="server" Width="100%" Style="display: none">
            <asp:UpdatePanel ID="_prodEdit" runat="server">
                <ContentTemplate>
                    <uc:sel_uc ID="ucSel" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
        
</asp:Content>
