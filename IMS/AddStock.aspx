<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddStock.aspx.cs" Inherits="IMS.AddStock" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="sel_uc" Src="~/UserControl/ProductSearch.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />

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
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>
    <br />
    <br />
    <div class="row">
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="col-md-2 control-label">Stock At</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged"/>
                <br/>
            </div>
    </div>    
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SelectProduct" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
                 <input type="text" id="txtSearch" runat="server" name="txtSearch"   onkeyup="searchSuggest(event);" autocomplete="off"  /> 
                 <div id="search_suggest" style="visibility: hidden;" ></div>

                <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product" Visible="false" />
               

                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click"  Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <br />
                <asp:DropDownList runat="server" ID="ProductList" Visible="false" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="ProductList_SelectedIndexChanged"/>
                <br/>
            </div>
    </div>
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductList" Visible="false" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
               
            </div>
    </div>   
    </div>
    
    <div class="form-horizontal">
        <h4>Add Stock</h4>
        <hr />
        <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="BarCodeSerial" CssClass="control-label">BarCode Serial</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="BarCodeSerial" CssClass="form-control" Enabled="False" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="ProductName" CssClass=" control-label">Product Name</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="ProductName" CssClass="form-control" Enabled="False" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="Quantity" CssClass=" control-label">Stock Quantity</asp:Label></td>
                <td><asp:TextBox runat="server" ID="Quantity" CssClass="form-control" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="Quantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." ValidationGroup="ExSave"/></td>
            <td><asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass=" control-label">Stock Expiry</asp:Label></td>
                <td><asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="DateTextBox" CssClass="text-danger" ErrorMessage="The product expiry field is required." ValidationGroup="ExSave"/></td>
              </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="ProductSale" CssClass=" control-label">Unit Sale Price</asp:Label></td>
                <td><asp:TextBox runat="server" ID="ProductSale" CssClass="form-control" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="ProductCost" CssClass="control-label">Unit Cost Price</asp:Label></td>
                <td><asp:TextBox runat="server" ID="ProductCost" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="100%"><asp:Button ID="btnCreateProduct" runat="server" OnClick="btnCreateProduct_Click" Enabled="false" Text="ADD" CssClass="btn btn-primary" ValidationGroup="ExSave"/>
                <asp:Button ID="btnCancelProduct" runat="server" OnClick="btnCancelProduct_Click" Text="CANCEL" CssClass="btn btn-default" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/></td>
            </tr>
            </table>
    </div>
</asp:Content>
