<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddProduct.aspx.cs" Inherits="IMS.AddProduct" %>
<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <style>
   .masterSearch{
    height:30px !important;
   }
  </style>
     <div class="form-horizontal">
    
             
     </div>
     <div class="form-horizontal" style="display:none;">
     <asp:Label runat="server" AssociatedControlID="txtProduct" CssClass="col-md-2 control-label">Search Product </asp:Label>
            <div class="col-md-10">
               
            </div>
     </div>

                <table width="100%">
                     
                     <tr><td>
                    <h4>Add Product</h4>
                    </td>
                         
                    <td align="right">
                        <asp:Button ID="btnCreateProduct" runat="server" OnClick="btnCreateProduct_Click"  Text="Save" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" ValidationGroup="exSave"/>
                         <asp:Button ID="btnCancelProduct" runat="server" OnClick="btnCancelProduct_Click" Text="CANCEL" CssClass="btn btn-default" />
                         <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click"/>
                     
                    </tr>
                    <tr>
        	            <td height="6"></td>
                    </tr>
                   <%-- <tr>
                       <td>
                             Search Product in Master Archive
                         </td>
                        
                         <td>
                              <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product master-search"/>
                            <asp:Button runat="server" ID="btnMasterSearch" CssClass ="btn btn-sm btn-primary" Text="Master Search" OnClick="btnMasterSearch_Click"/>
                         </td>
                     </tr>--%>

                    </table>
                    <table>
                    <tr><td>
                    <label>Search Product in Master Archive:</label>
                        </td>
                        <td>&nbsp;&nbsp;&nbsp;&nbsp;</td>
                        <td>
                        <asp:TextBox runat="server" ID="txtProduct" CssClass="masterSearch"/>
                        </td>
                        <td>
                        &nbsp;&nbsp;<asp:Button runat="server" ID="btnMasterSearch" CssClass ="btn btn-primary btn-sm" Text="Master Search" OnClick="btnMasterSearch_Click"/>
                            
                        </td>
                    </tr>
                    <tr>
                    <td height="16""></td>
                    </tr>
                    </table>
        <hr>
      
              

         <table cellspacing="5" cellpadding="5" border="0" width="100%" class="formTbl">
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="chkActive" CssClass="control-label">Active</asp:Label> </td>
                 <td>  <asp:CheckBox ID="chkActive" runat="server">
                       
                     </asp:CheckBox>
                 </td>
                 <td><asp:Label runat="server" AssociatedControlID="checkProductALL" CssClass="control-label">Product for ALL</asp:Label> </td>
                 <td>
                      <asp:CheckBox ID="checkProductALL" runat="server">
                       
                     </asp:CheckBox>
                 </td>
                 <%--<td colspan="100%"><asp:Button ID="btnCreateProduct" runat="server" OnClick="btnCreateProduct_Click"  Text="ADD" CssClass="btn btn-primary" ValidationGroup="exSave"/>
                 <asp:Button ID="btnCancelProduct" runat="server" OnClick="btnCancelProduct_Click" Text="CANCEL" CssClass="btn btn-default" />
                 <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click"/></td>--%>
             </tr>
             <tr>
                 <td>
                <asp:Label runat="server" AssociatedControlID="BarCodeSerial" CssClass="control-label">BarCode Serial</asp:Label>
                 </td>
                 <td><asp:TextBox runat="server" ID="BarCodeSerial" CssClass="form-control" Enabled="false" Visible="true" />
                 </td>
                 <td><asp:Label runat="server" AssociatedControlID="GreenRainCode" CssClass="control-label">GreenRain Code</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="GreenRainCode" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td> <asp:Label runat="server" AssociatedControlID="ProductName" CssClass="control-label">Product Name</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="ProductName" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ProductName" CssClass="text-danger" ErrorMessage="The product name field is required." ValidationGroup="exSave"/></td>
                <td> <asp:Label runat="server" AssociatedControlID="ProdcutDesc" CssClass=" control-label">Product Description</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="ProdcutDesc" CssClass="form-control" /></td>
             </tr>
             <tr>
             <td><asp:Label runat="server" AssociatedControlID="ProdcutBrand" CssClass=" control-label">Product Brand</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="ProdcutBrand" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="ProductType" CssClass="control-label">Product Type</asp:Label></td>
                 <td>        <asp:DropDownList runat="server" ID="ProductType" CssClass="form-control" Width="29%"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ProductType" CssClass="text-danger" ErrorMessage="The product type field is required." ValidationGroup="exSave"/></td>
              
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label">Product Department</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ProductDept_SelectedIndexChanged"/></td>
            <td><asp:Label runat="server" AssociatedControlID="ProductCat" CssClass=" control-label">Product Category</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="ProductCat_SelectedIndexChanged" /></td>
                  </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass="control-label">Product SubCategory</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%"/></td>
                 <td><asp:Label runat="server" AssociatedControlID="ddlProductOrderType" CssClass=" control-label">Product Order Type</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ddlProductOrderType" CssClass="form-control" Width="29%"/></td>
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="ProductCost" CssClass=" control-label">Unit Cost Price</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="ProductCost" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="ProductSale" CssClass=" control-label">Unit Sale Price</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="ProductSale" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="WholeSalePrice" CssClass=" control-label">WholeSale Price</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="WholeSalePrice" CssClass="form-control" /></td>
                 <td> <asp:Label runat="server" AssociatedControlID="ProductDiscount" CssClass=" control-label">Maximum Discount</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="ProductDiscount" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="ItemForm" CssClass="control-label">Product Form</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="ItemForm" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="ItemStrength" CssClass=" control-label">Product Strength</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="ItemStrength" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="PackType" CssClass=" control-label">Pack Type</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="PackType" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="PackSize" CssClass=" control-label">Pack Size</asp:Label></td>
                 <td>  <asp:TextBox runat="server" ID="PackSize" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td><asp:Label runat="server" AssociatedControlID="shelfNumber" CssClass=" control-label">Shelf Number</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="shelfNumber" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="rackNumber" CssClass=" control-label">Rack Number</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="rackNumber" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td> <asp:Label runat="server" AssociatedControlID="binNumber" CssClass=" control-label">Bin Number</asp:Label></td>
                 <td>   <asp:TextBox runat="server" ID="binNumber" CssClass="form-control" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="bonus12" CssClass=" control-label">Bonus Quantity 12</asp:Label></td>
                 <td><asp:TextBox runat="server" ID="bonus12" CssClass="form-control" /></td>
             </tr>
             <tr>
                 <td>  <asp:Label runat="server" AssociatedControlID="bonus25" CssClass="control-label">Bonus Quantity 25</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="bonus25" CssClass="form-control" /></td>
                 <td><asp:Label runat="server" AssociatedControlID="bonus50" CssClass="control-label">Bonus Quantity 50</asp:Label></td>
                 <td> <asp:TextBox runat="server" ID="bonus50" CssClass="form-control" /></td>
             </tr>
             
                </table>
     <script type="text/javascript">
         function ValidateForm() {

             if (document.getElementById("MainContent_ProductCost").value == null || document.getElementById("MainContent_ProductCost").value == '') {
                 alert("Please enter Cost Price");
                 return false;
             }
             if (document.getElementById("MainContent_ProductSale").value == null || document.getElementById("MainContent_ProductSale").value == '') {
                 alert("Please enter Sale Price");
                 return false;
             }

             return true;

         }
        </script>
</asp:Content>
