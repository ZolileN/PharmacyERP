<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="CreateTransferRequest.aspx.cs"
    Inherits="IMS.StoreManagement.StoreRequests.CreateTransferRequest" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<%--<%@ Register TagName="ProductsPopup"  TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>--%>

<%--<%@ Register TagName="StoresPopup" TagPrefix="UCVendorsPopup" Src="~/UserControl/StoresPopup.ascx" %>--%>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <style>
        .ProductName {
            width: 350px;
            white-space: normal;
            display: block;
        }
    </style>
    <table width="100%">

        <tbody>
            <tr>
                <td>
                    <h4 id="topHead">Create Transfer Request</h4>
                </td>
                <td align="right">
                    <asp:Button ID="btnGenerateRequest" runat="server" CssClass="btn btn-success" Text="Generate Request" OnClick="btnGenerateRequest_Click" />
                    <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnGoBack_Click" />

                </td>
            </tr>
            <tr>
                <td height="5"></td>
            </tr>
        </tbody>
    </table>
    <hr>

    <br />

<asp:Button ID="reset_transferDet" class="reset_transferDet" runat="server" OnClick="reset_Click" />
<div id="steps">
  <div class="step" id="step1" runat="server" data-desc="Search Product">1</div>
  <div class="step" id="step2" runat="server" data-desc="Select Product">2</div>
  <div class="step" id="step3" runat="server" data-desc="Select Pharmacy">3</div>
  <div class="step" id="step4" runat="server" data-desc="Add Quantity & Finish">4</div>
</div>

    <hr />

    <table cellspacing="5" cellpadding="5" border="0" style="margin: 0px auto;" class="formTbl" id="vendorSelect" width="">

        <tr>
            <td>
                <asp:Label ID="lblProd" AssociatedControlID="txtSearch" runat="server">Select Product</asp:Label></td>
            <td>
                <%--<asp:Label ID="lblProductId" runat="server" Visible="true"></asp:Label>--%>
                <asp:TextBox ID="txtSearch" runat="server" CssClass="form-control product" Width="70%"></asp:TextBox>
                <asp:Button ID="btnSearchProduct" runat="server" CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click" />

                <%--<cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblProd" ClientIDMode="AutoID"
                       PopupControlID="_ProductsPopup2"  >
                    </cc1:ModalPopupExtender>

               <div id="_ProductsPopup2" class="congrats-cont" style="display: none; ">
                   <UCProductsPopup:ProductsPopup  id="ProductsPopupGrid" runat="server"/>
                 </div>--%>
            </td>




       <%--     <td>--%>
                <asp:Label ID="lblSelectStore" AssociatedControlID="txtStore" Visible="false" runat="server">Select Pharmacy</asp:Label>

             <%-- </td>
          <td>--%>
               <%-- <%--<asp:Label ID="lblStoreId" runat="server" Visible="false"></asp:Label>--%>
               <asp:Label ID="lblProduct" runat="server" Visible="false"></asp:Label>
                <asp:TextBox ID="txtStore" runat="server" CssClass="form-control product" Width="70%" Visible="false"></asp:TextBox>
                <asp:Button ID="btnSelectStore" runat="server" CssClass="search-btn getProducts" Visible="false" OnClick="btnSelectStore_Click" />

                <%--<cc1:ModalPopupExtender ID="mpeStoresPopupDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectStore" ClientIDMode="AutoID"
                       PopupControlID="StoresPopupG"   >
                    </cc1:ModalPopupExtender>

                <div id="StoresPopupG" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:StoresPopup  id="StoresPopupGrid" runat="server"/>
                        </div>--%>
            <%--</td>--%>
            <td>
                <asp:Label ID="lbQty" AssociatedControlID="txtTransferredQty" runat="server">Quantity</asp:Label>
            </td>
            <td>
                <asp:TextBox ID="txtTransferredQty" runat="server" Style="width: 60px" CssClass="form-control product"></asp:TextBox>
            </td>
            <%--<td>
               <asp:Label ID="lblBonusQty"  AssociatedControlID="txtBonusQty" runat="server">Bonus Quantity</asp:Label>
           </td>
           <td>
               <asp:TextBox ID="txtBonusQty" runat="server" style="width:40px" CssClass="form-control product" ></asp:TextBox>
            </td>
           <td>
               <asp:Label ID="lblDiscount"  AssociatedControlID="txtPercentageDiscount" runat="server">Discount %</asp:Label>
           </td>
           <td>
               <asp:TextBox ID="txtPercentageDiscount" runat="server" style="width:60px" CssClass="form-control product" ></asp:TextBox>
            </td>--%>
            <td>
                <asp:Button ID="btnAddRequest" runat="server" Text="Add Request" CssClass="btn btn-primary btn-sm" OnClick="btnAddRequest_Click" />

            </td>


        </tr>
    </table>
    <asp:GridView ID="StoresPopup" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnPageIndexChanging="StoresPopup_PageIndexChanging">
        <Columns>

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="checkBoxOperation(this);" />
                </ItemTemplate>
            </asp:TemplateField>



            <%--<asp:TemplateField HeaderText="UPC">

                       <%--<asp:BoundField DataField="SystemID" HeaderText="System ID" Visible="false"  />--%>
            <asp:TemplateField HeaderText="UPC" Visible="false">

                <ItemTemplate>
                    <asp:Label ID="SystemID" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                </ItemTemplate>

            </asp:TemplateField>
            <%--<asp:BoundField DataField="SystemName" HeaderText="Pharmacy Name"   /> --%>

            <asp:TemplateField HeaderText="Pharmacy" Visible="true">

                <ItemTemplate>
                    <asp:Label ID="SystemName" runat="server" Text='<%# Eval("SystemName") %>' Width="110px"></asp:Label>
                </ItemTemplate>

            </asp:TemplateField>

            <asp:BoundField DataField="System_PharmacyID" HeaderText="Pharmacy ID" />
            <asp:BoundField DataField="SystemAddress" HeaderText="Pharmacy Address" />

            <asp:BoundField DataField="SystemPhone" HeaderText="Pharmacy Phone" />
            <asp:BoundField DataField="SystemFax" HeaderText="Pharmacy Fax" />

            <asp:BoundField DataField="SystemID" HeaderText="Pharmacy ID" />
            <asp:BoundField DataField="Quantity" HeaderText="Quantity" />

        </Columns>
        <PagerStyle CssClass="GridPager" />
    </asp:GridView>

    <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="StockDisplayGrid_RowDataBound" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging">
        <Columns>

            <asp:TemplateField HeaderText="Action">
                <ItemTemplate>
                    <asp:CheckBox ID="chkCtrl" runat="server" onclick="checkBoxOperation(this);" />
                </ItemTemplate>
            </asp:TemplateField>

            <%-- <asp:BoundField DataField="Product_Name" HeaderText="Product Name"   />--%>

            <asp:TemplateField HeaderText="Product Name">
                <ItemTemplate>
                    <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>' Width="320px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="320px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="UPC">
                <ItemTemplate>
                    <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="120px" HorizontalAlign="Left" />
            </asp:TemplateField>




            <%--<asp:BoundField DataField="Description" HeaderText="Description"   /> --%>
            <%--<asp:BoundField DataField="itemStrength" HeaderText="Item Strength"   />   ProductName,UnitCost --%>
            <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" />
            <%-- <asp:BoundField  DataField="ProductID" HeaderText="Product" />--%>

            <asp:TemplateField HeaderText="Product ID">
                <ItemTemplate>
                    <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="110px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="120px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:BoundField DataField="QtyinHand" HeaderText="QtyinHand" />



        </Columns>
        <PagerStyle CssClass="GridPager" />
    </asp:GridView>

    <asp:Button ID="SelectProduct" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectProduct_Click" />
    <asp:Button ID="SelectStore" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectStore_Click" />

    <div id="noPharmacy" runat="server">No Pharmacy found against this product. Please click here to <asp:Button runat="server" ID="startOver" OnClick="reset_Click" Text="Star Over" /></div>

    <asp:GridView ID="dgvCreateTransfer" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
        AutoGenerateColumns="false" OnPageIndexChanging="dgvCreateTransfer_PageIndexChanging" OnRowEditing="dgvCreateTransfer_RowEditing" OnRowDeleting="dgvCreateTransfer_RowDeleting"
        OnRowCommand="dgvCreateTransfer_RowCommand">
        <Columns>

            <asp:TemplateField HeaderText="Action" HeaderStyle-Width="100px">
                <ItemTemplate>
                    <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
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


            <%--<asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="TransferDetailID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferDetailID") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>--%>

            <asp:TemplateField HeaderText="ProductID" Visible="false" HeaderStyle-Width="110px">
                <ItemTemplate>
                    <asp:Label ID="lblProductID" CssClass="col-md-2 control-label  " runat="server" Text='<%# Eval("ProductID") %>' Width="100px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="110px" HorizontalAlign="Left" />

            </asp:TemplateField>

            <asp:TemplateField HeaderText="SystemID" Visible="false" HeaderStyle-Width="110px">
                <ItemTemplate>
                    <asp:Label ID="lblSystemID" CssClass="col-md-2 control-label ProductName" runat="server" Text='<%# Eval("SystemID") %>' Width="100px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="110px" HorizontalAlign="Left" />

            </asp:TemplateField>




            <asp:TemplateField HeaderText="Product<br>Description" Visible="true" HeaderStyle-Width="60px" HeaderStyle-Wrap="true">
                <ItemTemplate>
                    <asp:Label ID="ProductName" CssClass="ProductName" runat="server" Text='<%# Eval("Product_Name") %>'></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="60px" HorizontalAlign="Left" Wrap="true" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Requested To" HeaderStyle-Width="150px">
                <ItemTemplate>
                    <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedTo") %>' Width="140px"></asp:Label>
                </ItemTemplate>
                <ItemStyle Width="150px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width="150px">
                <ItemTemplate>
                    <asp:Label ID="TransferedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:Label>
                </ItemTemplate>

                <EditItemTemplate>
                    <asp:TextBox ID="txtTransferQty" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:TextBox>
                </EditItemTemplate>
                <ItemStyle Width="150px" HorizontalAlign="Left" />
            </asp:TemplateField>

            <%--<asp:TemplateField HeaderText="Bonus Qty"  HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblBonusQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BonusQty") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>

                          <EditItemTemplate>
                              <asp:TextBox ID="txtBonusQty" runat="server" Text='<%# Eval("BonusQty") %>'  Width="140px" ></asp:TextBox>
                          </EditItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField> 
                     
                      <asp:TemplateField HeaderText="Discount %"  HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblPercentageDiscount" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PercentageDiscount") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>

                          <EditItemTemplate>
                              <asp:TextBox ID="txtPercentageDiscount" runat="server" Text='<%# Eval("PercentageDiscount") %>'  Width="140px" ></asp:TextBox>
                          </EditItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField> --%>
        </Columns>
    </asp:GridView>


    <script>

        $(document).ready(function () {
            $('.step').each(function (index, element) {
                // element == this
                $(element).not('.active').addClass('done');
                $('.done').html('<i class="icon-ok"></i>');
                if ($(this).is('.active')) {
                    return false;
                }
            });
        });

        function checkBoxOperation(checkbox) {

            var GridVwHeaderChckbox = document.getElementById("<%=StockDisplayGrid.ClientID %>");
            var count = 0;
            for (i = 1; i < GridVwHeaderChckbox.rows.length; i++) {
                if (GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0] != checkbox) {
                    GridVwHeaderChckbox.rows[i].cells[0].getElementsByTagName("INPUT")[0].checked = false;
                }


            }



        }

    </script>


</asp:Content>


