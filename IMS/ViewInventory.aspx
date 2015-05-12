<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewInventory.aspx.cs" Inherits="IMS.ViewInventory" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="print_uc" Src="~/UserControl/uc_printBarcode.ascx" %>

 
<%@ Register TagName="ProductsPopup"  TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
   <script src="Scripts/SearchSuggest.js"></script>
       <style>

           .suggest_link 
	       {
	       background-color: #FFFFFF;
	       padding: 2px 6px 2px 6px;
	       }	
	       .suggest_link_over
	       {
	       background-color: #3366CC;
	       padding: 2px 6px 2px 6px;	
	       }	
	       #search_suggest 
	       {
	       position: absolute;
	       background-color: #FFFFFF;
	       text-align: left;
	       border: 1px solid #000000;	
           overflow:auto;
       		
	       }

       </style>
    
            <asp:Label runat="server" ID="NoProductMessage" CssClass="control-label" Visible="false" Text="No Stock Available"></asp:Label> 
  
        <table width="100%">

        <tbody>
        <tr>
        <td><h4>Current Stock</h4></td>
        <td align="right">
                     <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Enabled="true" Text="SEARCH" CssClass="btn btn-primary"/>
                     <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="REFRESH" CssClass="btn btn-info"/>
                     <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="PRINT" CssClass="btn btn-success btn-large" Visible="true" />
                     <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>
                     <asp:Button ID="btnFax" runat="server" Text="FAX" CssClass="btn btn-large no-print" Visible="false" />
                     <asp:Button ID="btnEmail" runat="server"  Text="EMAIL" CssClass="btn btn-large no-print" Visible="false" />
        </td>
        </tr>
		<tr><td height="5"></td></tr>
        </tbody>

        </table>

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
            <td><asp:Label runat="server" ID="lblProd"   CssClass="control-label">Select Product</asp:Label></td>
             
            <td> 
                <asp:TextBox ID="txtSearch" runat="server" CssClass="product"></asp:TextBox>
                <asp:Label ID="lblProductId" runat="server" Visible="false"></asp:Label>
               <asp:ImageButton ID="btnSearchProduct" runat="server"   CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click1" />

               <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblProd" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv2" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>

               <div id="_CongratsMessageDiv2" class="congrats-cont" style="display: none; ">
                            <UCProductsPopup:ProductsPopup  id="ProductsPopupGrid" runat="server"/>
                        </div>


                  </td><td><asp:Label runat="server" AssociatedControlID="ProductType" Visible="true" CssClass=" control-label">Product Type</asp:Label></td>
             <td><asp:DropDownList runat="server" ID="ProductType" Visible="true" OnSelectedIndexChanged="ProductType_SelectedIndexChanged" CssClass="form-control" Width="29%"/></td>
             </tr>        
        </table>

     <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="dgvStockDisplayGrid" runat="server" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10" 
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
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("prodDesc")==DBNull.Value?"":Eval("prodDesc") %>' Width="330px"></asp:Label>
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
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
       
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
