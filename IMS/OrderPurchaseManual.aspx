<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="OrderPurchaseManual.aspx.cs" Inherits="IMS.OrderPurchaseManual" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="VendorsPopup" TagPrefix="UCVendorsPopup"  Src="~/UserControl/VendorsPopupGrid.ascx"%>



<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

     <script type="text/javascript" src="http://ajax.googleapis.com/ajax/libs/jquery/1.4/jquery.min.js"></script>
    <script src="Scripts/fancybox/jquery.easing-1.3.pack.js" type="text/javascript"></script>
    <link href="Scripts/fancybox/jquery.fancybox-1.3.4.css" rel="stylesheet" type="text/css" />
    <script src="Scripts/fancybox/jquery.fancybox-1.3.4.js" type="text/javascript"></script>
    <script src="Scripts/fancybox/jquery.fancybox-1.3.4.pack.js" type="text/javascript"></script>
    <script src="Scripts/fancybox/jquery.mousewheel-3.0.4.pack.js" type="text/javascript"></script>
    <script type="text/javascript">
        $(document).ready(function () {


            $("a.popup").fancybox({
                'overlayShow': false,
                'transitionIn': 'elastic',
                'transitionOut': 'elastic'
            });

        });
    </script>

    <link rel="stylesheet" href="//code.jquery.com/ui/1.11.4/themes/smoothness/jquery-ui.css">
    <link href="Style/theme.css" rel="stylesheet" />
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
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:HiddenField ID="hdnVendorName" runat="server" Value="" />
    <asp:HiddenField ID="hdnVendorId" runat="server" Value="" />

      <h4>Manual PO(s)</h4>
       <hr />
    
     
    
 
            <%--<asp:TextBox runat="server" ID="SelectQuantity" CssClass="form-control" autocomplete="off" />--%>                <%--<asp:Label runat="server" AssociatedControlID="SelectPrice" CssClass="col-md-2 control-label">Enter Bonus Quantity</asp:Label>--%><%--<asp:TextBox runat="server" ID="SelectPrice" CssClass="form-control" autocomplete="off" />--%>       <%--<asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-default" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-primary btn-large" />--%>
    
   <table cellspacing="5" cellpadding="5" border="0" width="100%">
     
       <tr>
           <td><asp:Label runat="server"  AssociatedControlID="RequestTo" CssClass="control-label">Select Vendor</asp:Label></td>
           <td >
              <cc1:ComboBox ID="CmbVendors" runat="server" AutoCompleteMode="SuggestAppend" DataSourceID="VendorsDataSource" DataTextField="SupName" DataValueField="SuppID" MaxLength="0" style="display: inline; width: 10px;" OnSelectedIndexChanged="CmbVendors_SelectedIndexChanged">
                </cc1:ComboBox> 
               <%--<a href="VendorsPopup.aspx" class="popup" ><img src="/Images/search-icon-512.png" alt="img" /></a>--%>
                 
                <asp:ImageButton ID="btnSearchVendor" runat="server"  Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="40px" />
                  <asp:SqlDataSource ID="VendorsDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSConnectionString %>" SelectCommand="SELECT DISTINCT [SupName], [SuppID] FROM [tblVendor]"></asp:SqlDataSource>
                    
                
                 <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnSearchVendor" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv"  OkControlID="_okPopupButton" CancelControlID="_cancelPopupButton"
                            BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                    <div class="_popupButtons" style="display: none">
                             <input id="_okPopupButton" value="OK" type="button" />
                             <input id="_cancelPopupButton" value="Cancel" type="button" />
                        </div>
                        <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:VendorsPopup  id="VendorsPopupGrid" runat="server"/>
                        </div>
                     
                <asp:TextBox runat="server" ID="txtVendor"  class="autosuggest" Visible="False"/>
                
                <asp:DropDownList runat="server" ID="RequestTo" CssClass="form-control" Width="280" AutoPostBack="true" OnSelectedIndexChanged="RequestTo_SelectedIndexChanged" Visible="false" >
                       
                 </asp:DropDownList>
                
                
           </td>
       
            <td><asp:Label runat="server" AssociatedControlID="txtProduct" CssClass="control-label">Select Product</asp:Label></td>

            <td>

                <input type="text" id="txtSearch" runat="server" name="txtSearch"   onkeyup="searchSuggest(event);" autocomplete="off"  /> 
                <div id="search_suggest"  ></div>

                  <%--<asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click"  Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />--%>

            
                
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product" Visible="False"/>
                 
               </td>
           </tr>
        <tr>
            
            <td colspan="100%">
                <div class="qtyBox">
                    <table>
                    <tr>
                        <td>
                            <asp:Label runat="server" AssociatedControlID="SelectQuantity" CssClass="control-label">Quantity</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="SelectQuantity" CssClass="form-control" autocomplete="off" Width="60px" />
                        </td>
                        <td>
                            <asp:Label runat="server" AssociatedControlID="SelectPrice" CssClass="control-label">Bonus Quantity</asp:Label>
                        </td>
                        <td>
                            <asp:TextBox runat="server" ID="SelectPrice" CssClass="form-control" autocomplete="off" Width="60px" />
                        </td>
                    </tr>
                    </table>
                </div>
       
          
           
       </tr>
       <tr>

       <td colspan="100%">&nbsp;</td>
       </tr>
       
       <tr>
           <td></td>
           <td colspan="100%">
               <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="ExSave"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-default btn-large" />

           </td>
       </tr>
    </table>
   
            <%--<asp:Label runat="server" AssociatedControlID="SelectPrice" CssClass="col-md-2 control-label">Enter Bonus Quantity</asp:Label>--%>                <%--<asp:TextBox runat="server" ID="SelectPrice" CssClass="form-control" autocomplete="off" />--%>            <%--<asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-default" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-primary btn-large" />--%>           
                <%--<asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-default" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-primary btn-large" />--%>
             

    
            <%--<asp:Label runat="server" AssociatedControlID="SelectPrice" CssClass="col-md-2 control-label">Enter Bonus Quantity</asp:Label>--%>
          
                <%--<asp:TextBox runat="server" ID="SelectPrice" CssClass="form-control" autocomplete="off" />--%>
            

   
                <%--<asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-default" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" Visible="False" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-primary btn-large" />--%>
         
    <br />

   
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand"  onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound" >
                 <Columns>
                     <asp:TemplateField Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="From" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("FromPlace") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ToPlace") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>
                          
                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default btn-xs" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            
                            <asp:LinkButton CssClass="btn btn-default btn-xs" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Product Description" Visible="true" HeaderStyle-Width ="430px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="control-label ProductDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("strength") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PackageSize") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Quantity"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Qauntity") %>' Width="47px" ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Qty"  HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblBonusQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtBonusQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Bonus") %>' Width="47px" ></asp:TextBox>
                            </EditItemTemplate>
                          <ItemStyle  Width="90px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Unit<br>Cost Price" HeaderStyle-Width="110px">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total<br>Cost Price" HeaderStyle-Width="110px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalCostPrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order<br>Status" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                   <%--Hidden Fields--%>   
                     <asp:TemplateField HeaderText="Quantity Org" Visible="false"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantityOrg" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>
                     </asp:TemplateField>
                 </Columns>
             </asp:GridView>
          <table ID="tblDsp" cellpadding="4" cellspacing="0" align="right" visible="false">
        	<tr>
            	<td><asp:Label ID="lblttlcst" runat="server" AssociatedControlID="lblTotalCostALL" Visible="false">Total Cost:</asp:Label></td>
                <td><asp:Label ID="lblTotalCostALL" Visible="false" runat="server" Style="font-weight: 700"></asp:Label></td>


	           
            </tr>
         </table>
        <br />
         <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="GENERATE ORDER" CssClass="btn btn-large" Visible="false"/>
     <span onclick="return confirm('Are you sure you want to delete this order?')">
         <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="DELETE ORDER" CssClass="btn btn-large" Visible="false" />
   </span>



    <script type="text/javascript">

        //$(document).ready(function () {

        //    $("#btnSearchVendor").click(function () {
        //        document.getElementById("vendors").style.display = "block";
        //        //$("#vendors").show();
        //    });

        //});

        function ValidateForm() {
            
           
            if (document.getElementById("MainContent_txtProduct").value == null || document.getElementById("MainContent_txtProduct").value == '') {
                alert("Please enter at least three words to search product");
                return false;
            }
            if (document.getElementById("MainContent_SelectQuantity").value == null || document.getElementById("MainContent_SelectQuantity").value == '') {
                alert("Please enter Quantity");
                return false;
            }
         
            return true;

            //var e = document.getElementById("SelectProduct");
            //var strProduct = e.options[e.selectedIndex].value;

        }
        //window.onload = function () {
        //    document.getElementById("vendors").style.display = "none";
        //    document.getElementById("overLaypop").style.display = "none";
             
        //}
       
</script>
    
</asp:Content>
