<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockManipulation.aspx.cs" Inherits="IMS.StockManipulation" %>
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
                     <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="ADD TO PRINT" CssClass="btn btn-info"/>
                     <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="PRINT" CssClass="btn btn-success btn-large" Visible="true" />
                     <asp:Button ID="btnUpdate" runat="server" OnClick="btnUpdate_Click" Text="STOCK & PRICE ADJUSTMENT" CssClass="btn btn-success btn-large" Visible="true" />
                     <asp:Button ID="btnUpdatePrice" runat="server" OnClick="btnUpdatePrice_Click" Text="PRICE ADJUSTMENT" CssClass="btn btn-success btn-large" Visible="False" />
                     <asp:Button ID="btnUpdateStock" runat="server" OnClick="btnUpdateStock_Click" Text="STOCK ADJUSTMENT" CssClass="btn btn-success btn-large" Visible="False" />
                     <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>
                     <asp:Button ID="btnFax" runat="server" Text="FAX" CssClass="btn btn-large no-print" Visible="false" />
                     <asp:Button ID="btnEmail" runat="server"  Text="EMAIL" CssClass="btn btn-large no-print" Visible="false" />
        </td>
        </tr>
		<tr><td height="5"></td></tr>
        </tbody>

        </table>
    <hr />
     <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
        <tr>
            <td> <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label" Visible="true">Product Department</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True"/></td>
            <td><asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="control-label" Visible="true">Product Category</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" Visible="true"  /></td>
        </tr>
        <tr>
            <td> <asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass=" control-label" Visible="true"> Product SubCategory </asp:Label></td>
            <td><asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%" Visible="true" /></td>
            <td><asp:Label Visible="false" runat="server" AssociatedControlID="ddlProductOrderType" CssClass="control-label">Product Order Type</asp:Label>

                <asp:Label runat="server" AssociatedControlID="ProductType" Visible="true" CssClass=" control-label">Product Type</asp:Label>

            </td>
            <td><asp:DropDownList runat="server" Visible="false" ID="ddlProductOrderType"  CssClass="form-control" Width="29%"/>

                <asp:DropDownList runat="server" ID="ProductType" Visible="true"  CssClass="form-control" Width="29%"/>

            </td>
            <td><asp:Label runat="server" AssociatedControlID="ddlStockAt" Visible="false" CssClass="control-label">Select Pharmacy </asp:Label></td>
            <td><asp:DropDownList runat="server" Visible="false" ID="ddlStockAt" CssClass="form-control" Width="280" />
             </tr>
        <tr>
            <td><asp:Label runat="server" ID="lblProd"   CssClass="control-label">Select Product</asp:Label></td>
             
            <%--<td> 
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


                  </td>--%>
            <td>
                <input type="text" id="txtSearch" runat="server" name="txtSearch"   autocomplete="off"  /> 
                <div id="search_suggest" style="visibility: hidden;" ></div>
            </td>
            <td></td>
             <td></td>
             <td><asp:Label runat="server" Visible="false" AssociatedControlID="ddlActive" CssClass="control-label">Search Active</asp:Label></td>
            <%-- <td> <asp:CheckBox ID="chkActive" runat="server">
                       
                     </asp:CheckBox>
                 </td>--%>
            <td>
                <asp:DropDownList runat="server" ID="ddlActive" Visible="false" CssClass="form-control" Width="29%"/>
            </td>
             </tr>        
        </table>

        
     <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="dgvStockDisplayGrid" runat="server" CssClass="table table-striped table-bordered table-condensed" AllowPaging="False" PageSize="10" 
               DataKeyNames="SerialNum"  AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowEditing="dgvStockDisplayGrid_RowEditing" OnRowDeleting="dgvStockDisplayGrid_RowDeleting"
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound">
                    
             <Columns>
                
                 
                   <asp:TemplateField HeaderText="Action" >

                        <HeaderTemplate>
                            <asp:CheckBox ID="chkboxSelectAll" runat="server" onclick="CheckAllEmp(this);" />
                        </HeaderTemplate>

                        <ItemTemplate>
                             
                            
                            <asp:CheckBox ID="ProductSelector" onclick="checkBoxOperation(this);" runat="server" />

                            <asp:LinkButton Visible="false" CssClass="btn btn-default add-btn" ID="btnAdd" Text="Add" runat="server" CommandName="AddVal" CommandArgument='<%# Container.DisplayIndex%>'/>
                            <asp:LinkButton Visible="false" CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex%>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton Visible="false" CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="lblUPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UPC") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="Greenrain Code">
                        <ItemTemplate>
                            <asp:Label ID="lblGrenRainCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("GreenRainCode") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

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
                   
                   <asp:TemplateField HeaderText="Batch No.">
                        <ItemTemplate>
                            <asp:Label ID="lblBatchNo" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("batchNo")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval(("Expiry").ToString(), "{0:dd/MM/yyyy}")%>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                       <asp:TemplateField HeaderText="Batch" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("batchNo")%>' Width="100px"></asp:Label>
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
                      
                    
                     <%-- org command argument CommandArgument='<%# Eval("BarCode") %>'--%>
                     
                       <asp:TemplateField HeaderText="stockID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblstockid" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("StockID") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                    

                      <asp:TemplateField HeaderText="prodID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblprodID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
       
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                
            </div>
        </div>
    </div>
    </div>

    <script type="text/javascript" language="javascript">
        if (getParameterByName('Param') == "Adjustment") {

            document.getElementById("MainContent_dgvStockDisplayGrid_chkboxSelectAll").style.visibility = "hidden";
        }        
        function checkBoxOperation(checkbox){
            if (getParameterByName('Param') == "Adjustment") {
                var GridVwHeaderChckbox = document.getElementById("<%=dgvStockDisplayGrid.ClientID %>");
                var count = 0;
                for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                    if (GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0] != checkbox) {
                        GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                    }


                }

            }
            
        }

        function CheckAllEmp(Checkbox) {
            var GridVwHeaderChckbox = document.getElementById("<%=dgvStockDisplayGrid.ClientID %>");
            var count = 0;
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = Checkbox.checked;
                

            }
        }

        function getParameterByName(name) {
            name = name.replace(/[\[]/, "\\[").replace(/[\]]/, "\\]");
            var regex = new RegExp("[\\?&]" + name + "=([^&#]*)"),
                results = regex.exec(location.search);
            return results === null ? "" : decodeURIComponent(results[1].replace(/\+/g, " "));
        }
    </script>
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
