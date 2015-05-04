<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageProducts.aspx.cs" Inherits="IMS.ManageProducts" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Product Management</h4>
    <hr />
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

    <table width="100%">

        <tr>
            <td width="100">
                <asp:Label runat="server" AssociatedControlID="txtProduct" CssClass="control-label">Select Product</asp:Label>
            </td>
            <td>
                 <input type="text" id="txtSearch" runat="server" name="txtSearch"   onkeyup="searchSuggest(event);" autocomplete="off"  /> 
                <div id="search_suggest" style="visibility: hidden;" ></div>


                <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product" Visible="false"/>
              
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" cssClass="search-btn" />
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                
            </td>
            <td align="right">
                <asp:Button ID="btnAddProduct" runat="server" CssClass="btn btn-success btn-large" Text=" + Add Product" OnClick="btnAddProduct_Click"/>
                <asp:Button ID="btnDeleteProduct" runat="server" CssClass="btn btn-danger btn-large" Text="Delete Product" OnClick="btnDeleteProduct_Click" Visible="False"/>
                <asp:Button ID="btnEditProduct" runat="server" CssClass="btn btn-info btn-large" Text="Edit Product" OnClick="btnEditProduct_Click" Visible="False"/>
            </td>
        </tr>

    </table>
    
   

    
        
         
        <%--<asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>--%>
         
     

    
    <div class="form-horizontal">
    <div class="form-group">
        <br />
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" 
            onrowediting="StockDisplayGrid_RowEditing" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" >
                 <Columns>
                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GreenRain Code">
                        <ItemTemplate>
                            <asp:Label ID="GreenRain" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ItemCode") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="140px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Name">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="330px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Type">
                        <ItemTemplate>
                            <asp:Label ID="Type" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DrugType") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Unit Cost Price">
                        <ItemTemplate>
                            <asp:Label ID="UnitCost" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("UnitCost") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Sale Price">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SP") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex%>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
        
    </div>
    </div>
</asp:Content>
