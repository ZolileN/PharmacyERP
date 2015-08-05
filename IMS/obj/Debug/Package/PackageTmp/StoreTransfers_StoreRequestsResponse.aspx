<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StoreTransfers_StoreRequestsResponse.aspx.cs" Inherits="IMS.StoreTransfers_StoreRequestsResponse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h3> Respond to Store(s)</h3> 
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
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                    <asp:TemplateField>
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Product Description" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>'  Width="250px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="Available Stock" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="Stock" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Stock") %>'  Width="250px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     
                    <asp:TemplateField HeaderText="Requested Quantity" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ReqQuantity") %>'  Width="250px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Send Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SndQauntity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("SndQauntity") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
               
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="170px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                          
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         <ItemStyle  Width="170px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
        <br />
         <asp:Button ID="btnResponse" runat="server" OnClick="btnAccept_Click1" Text="ACCEPT LIST" CssClass="btn btn-large"/>
         <asp:Button ID="btnCancel" runat="server" OnClick="btnDecline_Click" Text="CANCEL LIST" CssClass="btn btn-large" />
    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
</asp:Content>
