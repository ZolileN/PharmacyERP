<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Warehouse_StoreRequestDetails.aspx.cs" Inherits="IMS.Warehouse_StoreRequestDetails" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3> Store Request Details</h3> 
    <br />

    <div class="form-horizontal">
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="RequestFrom" CssClass="col-md-2 control-label" Text="Request From : "></asp:Label>
            <div class="col-md-10">
                <asp:Label runat="server" ID="RequestFrom" CssClass="col-md-2 control-label"/>
            </div>
    </div>
    <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="RequestDate" CssClass="col-md-2 control-label" Text="Requested Date :"></asp:Label>
            <div class="col-md-10">
                <asp:Label runat="server" ID="RequestDate" Width="250px" CssClass="col-md-2 control-label"/>
            </div>
    </div>
    </div>
   
     <br />

    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowDataBound="StockDisplayGrid_RowDataBound">
                 <Columns>

                     <asp:TemplateField HeaderText="Product Description" Visible="true" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>' Width="350px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="350px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Requested Quantity" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Request Status" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
             <br />
             <asp:Button ID="btnResponse" runat="server" OnClick="btnResponse_Click" Text="RESPOND" CssClass="btn btn-large" Visible="true"/>
             <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="BACK" CssClass="btn btn-large" Visible="true" />
    </div>

    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
</asp:Content>
