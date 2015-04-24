<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewInventory.aspx.cs" Inherits="IMS.ViewInventory" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="print_uc" Src="~/UserControl/uc_printBarcode.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <div class="form-group">
            <asp:Label runat="server" ID="NoProductMessage" CssClass="col-md-2 control-label" Visible="false" Text="No Stock Available"></asp:Label> 
    </div>
    <br />
    <div class="row">
    <div class="form-group">
            <h1>CURRENT STOCK</h1>
        </div>
    </div>
    <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
        <tr>
            <td> <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label" Visible="true">Product Department</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ProductDept_SelectedIndexChanged"/></td>
            <td><asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="control-label" Visible="true">Product Category</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" AutoPostBack="True" Visible="true" OnSelectedIndexChanged="ProductCat_SelectedIndexChanged" /></td>
        </tr>
        <tr>
            <td> <asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass=" control-label" Visible="true"> Product SubCategory </asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%" Visible="true" OnSelectedIndexChanged="ProductSubCat_SelectedIndexChanged"/></td>
            <td><asp:Label runat="server" AssociatedControlID="ddlProductOrderType" CssClass="control-label">Product Order Type</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ddlProductOrderType" CssClass="form-control" Width="29%"/></td>
             </tr>
        <tr>
            <td><asp:Label runat="server" AssociatedControlID="SelectProduct" CssClass="control-label">Select Product</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <asp:DropDownList runat="server" ID="ProductList" Visible="false" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="ProductList_SelectedIndexChanged"/></td>
            <td><asp:Label runat="server" AssociatedControlID="ProductType" Visible="true" CssClass=" control-label">Product Type</asp:Label></td>
             <td><asp:DropDownList runat="server" ID="ProductType" Visible="true" OnSelectedIndexChanged="ProductType_SelectedIndexChanged" CssClass="form-control" Width="29%"/></td>
             </tr>
        <tr>
            <td></td>
            <td colspan="100%"> <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Enabled="true" Text="SEARCH" CssClass="btn btn-default"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="REFRESH" CssClass="btn btn-default"/></td>
        </tr>
        </table>
    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" runat="server" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound">
                 <Columns>
                     <asp:TemplateField HeaderText="BarCode">
                        <ItemTemplate>
                            <asp:Label ID="BarCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BarCode") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField Visible="true" HeaderText="Product Description" HeaderStyle-Width="330px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("prodDesc") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="330px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width ="125px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("strength") %>'  Width="125px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="125px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PackageSize") %>'  Width="170px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="170px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                   
                     <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval(("Expiry").ToString(), "{0:dd/MM/yyyy}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCostPrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("CostPrice") %>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Sale">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SalePrice") %>' Width="60px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                       
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <%-- org command argument CommandArgument='<%# Eval("BarCode") %>'--%>
                     
                 </Columns>
             </asp:GridView>
       
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
                <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="PRINT" CssClass="btn btn-large no-print" Visible="true" />
                <asp:Button ID="btnFax" runat="server" Text="FAX" CssClass="btn btn-large no-print" Visible="false" />
                <asp:Button ID="btnEmail" runat="server"  Text="EMAIL" CssClass="btn btn-large no-print" Visible="false" />
            </div>
        </div>
    </div>
    </div>

     <asp:Button ID="_editPopupButton" runat="server" Style="display: none" />
        <cc1:ModalPopupExtender ID="mpeEditProduct" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" 
            PopupDragHandleControlID="_prodEditPanel" TargetControlID="_editPopupButton" PopupControlID="_prodEditPanel" BehaviorID="EditModalPopupMessage">
        </cc1:ModalPopupExtender>

        <asp:Panel ID="_prodEditPanel" runat="server" Width="100%" Style="display: none">
            <asp:UpdatePanel ID="_prodEdit" runat="server">
                <ContentTemplate>
                    <uc:print_uc ID="ucPrint" runat="server" />
                </ContentTemplate>
            </asp:UpdatePanel>
        </asp:Panel>
</asp:Content>
