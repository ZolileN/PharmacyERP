﻿<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderSalesManual.aspx.cs" Inherits="IMS.OrderSalesManual" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%@ Register TagName="NonGeneratedSOPopup" TagPrefix="UCNonGeneratedSOPopup" Src="~/UserControl/uc_PendingSalesOrderPopUp.ascx" %>

<%@ Register TagName="ProductsPopup" TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

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

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">



    <table width="100%">

        <tbody>
            <tr>
 
        	<td> <h4>Generate Sale Orders</h4></td>
            <td align="right">
            		                		                 	 
                <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD"  CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="ExSave"/>
                <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="Generate SO" CssClass="btn btn-success btn-large" Visible="false"/>
                <span onclick="return confirm('Are you sure you want to delete this record?')">
                    <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="Delete SO" CssClass="btn btn-danger btn-large" Visible="false" />
                </span>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default btn-large" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="Go Back" CssClass="btn btn-default btn-large" />
                <asp:Button ID="btnMapPreviousOrders" runat="server" OnClick="btnMapPreviousOrders_Click" Visible="false" Text="Map Prev. Orders" CssClass="btn btn-default btn-large" />
            </td>
        </tr>
		<%--<tr>
            <td height="5"></td>
=======
                <td>
                    <h4>Generate Sale Orders</h4>
                </td>
                <td align="right">

                    <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="ExSave" />
                    <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="Generate SO" CssClass="btn btn-success btn-large" Visible="false" />
                    <span onclick="return confirm('Are you sure you want to delete this record?')">
                        <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="Delete SO" CssClass="btn btn-danger btn-large" Visible="false" />
                    </span>
                    <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default btn-large" Visible="False" />
                    <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="Go Back" CssClass="btn btn-default btn-large" />
                    <asp:Button ID="btnMapPreviousOrders" runat="server" OnClick="btnMapPreviousOrders_Click" Visible="false" Text="Map Prev. Orders" CssClass="btn btn-default btn-large" />
                </td>
            </tr>--%>
            <tr>
                <td height="5"></td>
 

            </tr>
        </tbody>

    </table>
    <hr>
    <table cellspacing="5" cellpadding="5" border="0" width="100%" class="formTbl">

        <%--<tr>
            <td>
                <asp:Label runat="server" AssociatedControlID="txtIvnoice" CssClass="control-label" Visible="false">Invoice No </asp:Label></td>
            <td>
                <asp:TextBox runat="server" ID="txtIvnoice" CssClass="form-control"  Visible="false" /></td>



        </tr>--%>
        <tr>

            <td>
                <asp:Label runat="server" ID="lblSelectSalesman" AssociatedControlID="ddlSalesman" CssClass="control-label">Select Salesman</asp:Label></td>
            <td>
                <asp:DropDownList ID="ddlSalesman" runat="server" AutoPostBack="true" CssClass="form-control" Width="280" OnSelectedIndexChanged="ddlSalesman_SelectedIndexChanged"></asp:DropDownList>
            </td>

            <td>
                <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Select Pharmacy </asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="280" OnSelectedIndexChanged="StockAt_SelectedIndexChanged1" AutoPostBack="true"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="StockAt" CssClass="text-danger" ErrorMessage="The Store field is required." ValidationGroup="ExSave" /></td>



        </tr>
        <tr>

            <td>
                <asp:Label runat="server" ID="lblProd" AssociatedControlID="txtProduct" CssClass="control-label">Select Product</asp:Label></td>
            <td>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="product"></asp:TextBox>
                <asp:Label ID="lblProductId" runat="server" Visible="false"></asp:Label>
                <asp:ImageButton ID="btnSearchProduct" runat="server" CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click1" />

                <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                    RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblProd" ClientIDMode="AutoID"
                    PopupControlID="_CongratsMessageDiv2" BehaviorID="EditModalPopupMessage">
                </cc1:ModalPopupExtender>

                <div id="_CongratsMessageDiv2" class="congrats-cont" style="display: none;">
                    <UCProductsPopup:ProductsPopup ID="ProductsPopupGrid" runat="server" />
                </div>


                <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product" Visible="false" />

                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged" />

                <asp:Label runat="server" ID="lblSosPopup"  CssClass="control-label"> </asp:Label> 

                <asp:Button ID="lblSosPopups" runat="server" Visible="false" OnClick="lblSosPopup_Click"></asp:Button>
                <cc1:ModalPopupExtender ID="mpeNonGeneratedSOsPopup" runat="server" BackgroundCssClass="overLaypop"
                    RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSosPopup" ClientIDMode="AutoID"
                    PopupControlID="divSOs" BehaviorID="EditModalPopupMessage123" >
                </cc1:ModalPopupExtender>

                
                <div id="divSOs" class="congrats-cont" style="display: none;">
                    <UCNonGeneratedSOPopup:NonGeneratedSOPopup ID="uc_PendingSalesOrderPopUp" runat="server" />
                </div>
            </td>

            <td>
                <asp:Label runat="server" AssociatedControlID="SelectQuantity" CssClass="control-label">Enter Send Quantity</asp:Label></td>
            <td>
                <asp:TextBox runat="server" ID="SelectQuantity" CssClass="form-control" /></td>

        </tr>
        <tr>

            <td>
                <asp:Label runat="server" AssociatedControlID="SelectBonus" CssClass="control-label">Enter Bonus Quantity</asp:Label></td>
            <td>
                <asp:TextBox runat="server" ID="SelectBonus" CssClass="form-control" /></td>

            <td>
                <asp:Label runat="server" AssociatedControlID="SelectDiscount" CssClass="control-label">Enter Discount %</asp:Label></td>
            <td>
                <asp:TextBox runat="server" ID="SelectDiscount" CssClass="form-control" /></td>
        </tr>

    </table>




    <br />
    <br />

    <div class="form-horizontal">
        <div class="form-group">
            <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowCancelingEdit="StockDisplayGrid_RowCancelingEdit"
                OnRowCommand="StockDisplayGrid_RowCommand" OnRowDeleting="StockDisplayGrid_RowDeleting" OnRowEditing="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound">
                <Columns>
                    <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="1px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="ProductID" Visible="false" HeaderStyle-Width="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="110px" HorizontalAlign="Left" />

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="To" Visible="false" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ToPlace") %>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Action" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
                            <asp:Button CssClass="btn btn-default details-btn" ID="btnDetails" Text="Details" runat="server" CommandName="Details" CommandArgument='<%# Container.DataItemIndex %>' />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DataItemIndex %>' />
                            </span>
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-primary btn-sm" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default btn-sm" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>

                        <ItemStyle Width="100px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField Visible="false" HeaderText="Name : Strength : Form : Pack Size" HeaderStyle-Width="500" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName2" padding-right="5px" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                            <asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2" runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px"></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px"></asp:Label>
                        </ItemTemplate>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Product<br>Description" Visible="true" HeaderStyle-Width="60px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Left" Wrap="true" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="" runat="server" Text='<%# Eval("strength") %>' Width="150px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="160px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="" runat="server" Text='<%# Eval("dosageForm") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="110px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="" runat="server" Text='<%# Eval("PackageSize") %>' Width="150px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="160px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Available<br>Stock" HeaderStyle-Width="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("AvailableStock") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Sent<br>Quantity" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control grid-input-form" runat="server" Text='<%#Eval("Qauntity") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Bonus<br>Quantity" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="" runat="server" Text='<%# Eval("Bonus") %>'></asp:Label>
                        </ItemTemplate>

                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonus" CssClass="form-control grid-input-form" runat="server" Text='<%#Eval("Bonus") %>'></asp:TextBox>
                            <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                        <ItemStyle Width="60px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Cost<br>Price" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Convert.ToDecimal(Eval("SalePrice")).ToString("#.##") %>'></asp:Label>
                        </ItemTemplate>

                        <ItemStyle Width="60px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Discount %"  >
                                        <ItemTemplate>
                                            <asp:Label ID="DiscountPercentage" CssClass="" runat="server" Text='<%# Eval("Discount") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                     </asp:TemplateField>


                    <asp:TemplateField HeaderText="Total Sale<br>Price" Visible="true" HeaderStyle-Width="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="" runat="server" Text='<%# Eval("itemTOTALPrice") %>' Width="80px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="80px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="DETAILS" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:GridView ID="StockDetailDisplayGrid" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10">
                                <Columns>

                                    <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="Batch" CssClass="" runat="server" Text='<%# Eval("BatchNumber") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="70px">
                                        <ItemTemplate>
                                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="70px" HorizontalAlign="Left" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sent Quantity" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="" runat="server" Text='<%# Eval("SendQuantity") %>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bonus Quantity" HeaderStyle-Width="50px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="" runat="server" Text='<%# Eval("BonusQuantity") %>' Width="50px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="50px" Visible="true">
                                        <ItemTemplate>
                                            <asp:Label ID="DiscountPer" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="50px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                        <ItemStyle Width="200px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                </Columns>
            </asp:GridView>
            <br />
            <table ID="tblDsp" cellpadding="4" cellspacing="0" align="right" visible="false">
        	<tr>
            	<td><asp:Label ID="lblttlcst" runat="server" AssociatedControlID="lblTotalCostALL" Visible="false">Total Cost:</asp:Label></td>
                <td><asp:Label ID="lblTotalCostALL" Visible="false" runat="server" Style="font-weight: 700"></asp:Label></td>


	           
            </tr>
         </table>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPacking" runat="server" OnClick="btnPacking_Click" Text="GENERATE PACKING LIST" CssClass="btn btn-large" Visible="false" />
                <asp:Button ID="btnPrint" runat="server" OnClick="btnPrint_Click" Text="GENERATE PRINT" CssClass="btn btn-large" Visible="false" />
            </div>
        </div>
    </div>

    <script type="text/javascript">
        $(document).ready(function () {

            
            var defaultVal = $("#MainContent_StockAt option:selected").text();
            $("#MainContent_StockAt").change(function (e) {
                var newVal = $("#MainContent_StockAt option:selected").text();
                if (defaultVal != "Select System" && defaultVal != newVal) {
                    alert("You're changing pharmacy");
                }
            })

            var isAccepting = false;

            $("#MainContent_btnAccept").click(function (e) {
                
                if (isAccepting == false) {
                    isAccepting = true;
                    $("#ctl01").submit(function (e) {

                        if ($("#MainContent_StockAt").val() == 'Select System') {
                            e.preventDefault();
                            alert("Please select pharmacy");
                        }

                    });

                }
                else if (isAccepting == true) {
                    e.preventDefault();
                }
            })
        });

        function ValidateForm() {
    var e = document.getElementById("MainContent_StockAt");
    var ddStockAt = e.options[e.selectedIndex].value;
    if (ddStockAt == 'Select System') {
        alert("Please select Store");
        return false;
    }
    if (document.getElementById("MainContent_txtSearch").value == null || document.getElementById("MainContent_txtSearch").value == '') {
        alert("Please enter at least three words to search product");
        return false;
    }
 
}
        

                   </script>
</asp:Content>

