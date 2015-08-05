<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Edit_DeleteProduct.aspx.cs" Inherits="IMS.Edit_DeleteProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <br />
    <br />
    <div class="row">
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtProduct" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="35px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <br />
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <br/>
            </div>
    </div>
    </div>
    
    <div class="form-horizontal">
        <h4>Product Details</h4>
        <hr />
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="BarCodeSerial" CssClass="col-md-2 control-label">BarCode Serial</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="BarCodeSerial" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="GreenRainCode" CssClass="col-md-2 control-label">GreenRain Code</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="GreenRainCode" CssClass="form-control" Enabled="False" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="GreenRainCode" CssClass="text-danger" ErrorMessage="The product greenrain Code field is required." ValidationGroup="exsave" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductName" CssClass="col-md-2 control-label">Product Name</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProductName" CssClass="form-control" Enabled="False" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ProductName" CssClass="text-danger" ErrorMessage="The product name field is required." ValidationGroup="exsave" />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProdcutDesc" CssClass="col-md-2 control-label">Product Description</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProdcutDesc" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProdcutBrand" CssClass="col-md-2 control-label">Product Brand</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProdcutBrand" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>
        
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductType" CssClass="col-md-2 control-label">Product Type</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductType" CssClass="form-control" Width="29%" Enabled="False"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ProductType" CssClass="text-danger" ErrorMessage="The product type field is required." ValidationGroup="exsave"/>
            </div>
        </div>    

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="col-md-2 control-label">Product Department</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ProductDept_SelectedIndexChanged" Enabled="False"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ProductDept" CssClass="text-danger" ErrorMessage="The product department field is required." ValidationGroup="exsave" />
            </div>
        </div>  

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="col-md-2 control-label">Product Category</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="ProductCat_SelectedIndexChanged" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass="col-md-2 control-label">Product SubCategory</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%" Enabled="False" OnSelectedIndexChanged="ProductSubCat_SelectedIndexChanged"/>
               <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductCost" CssClass="col-md-2 control-label">Unit Cost Price</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProductCost" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductSale" CssClass="col-md-2 control-label">Unit Sale Price</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProductSale" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductDiscount" CssClass="col-md-2 control-label">Maximum Discount</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProductDiscount" CssClass="form-control" Enabled="False" />
                <br />
            </div>
        </div>

        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnCreateProduct" runat="server" OnClick="btnCreateProduct_Click" Text="EDIT" CssClass="btn btn-default" ValidationGroup="exsave" />
                <asp:Button ID="btnDeleteProduct" runat="server" OnClick="btnDeleteProduct_Click" Text="DELETE" CssClass="btn btn-default" ValidationGroup="exsave" />
                <asp:Button ID="btnCancelProduct" runat="server" OnClick="btnCancelProduct_Click" Text="CANCEL" CssClass="btn btn-default" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
            </div>
        </div>
    </div>
</asp:Content>
