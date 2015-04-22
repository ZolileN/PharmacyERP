<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WarehouseSalesOrder.aspx.cs" Inherits="IMS.WarehouseSalesOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Sale Order(s) Generation Form</h3>
    <br />
    <br />
    <div class="row">
     
    
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="RequestTo" CssClass="col-md-2 control-label">Send Order To</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="RequestTo" CssClass="form-control" Width="29%" AutoPostBack="true" OnSelectedIndexChanged="RequestTo_SelectedIndexChanged"/>
                <br />
            </div>
    </div>
    
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductList" CssClass="col-md-2 control-label">Select Product</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="ProductList" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="35px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <br />
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                <br/>
            </div>
            </div>

    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="SelectQuantity" CssClass="col-md-2 control-label">Enter Quantity</asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="SelectQuantity" CssClass="form-control" Width="29%"/>
                <br />
            </div>
    </div>
    
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnCreateOrder" runat="server" OnClick="btnCreateOrder_Click" Text="ADD" CssClass="btn btn-default" />
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Text="REFRESH" CssClass="btn btn-default" />
                <asp:Button ID="btnCancelOrder" runat="server" OnClick="btnCancelOrder_Click" Text="GO BACK" CssClass="btn btn-primary btn-large" />
            </div>
        </div>
    </div>
    
    <br />
    <br />

    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="From" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("FromPlace") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ToPlace") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Name" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductName") %>'  Width="250px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("Qauntity") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Request Status" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>'  Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:Button CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:Button CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
        <br />
         <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="CREATE ORDER" CssClass="btn btn-large" Visible="false"/>
         <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="CANCEL ORDER" CssClass="btn btn-large" Visible="false" />
    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
</asp:Content>
