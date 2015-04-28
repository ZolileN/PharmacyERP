<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPurchaseOrders.aspx.cs" Inherits="IMS.ViewPurchaseOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h3>Receive PO(s) </h3> 
     <div class="form-horizontal">
          <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="col-md-2 control-label">Search by Vendor Name </asp:Label>
            <div class="col-md-10">
                <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <br />
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged" Visible="false"/>
                <br/>
            </div>
    </div>
         <br />
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                    <asp:TemplateField HeaderText="Request No."   HeaderStyle-Width ="60px" >
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedNO"  Text='<%# Eval("OrderID") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedNO" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderID") %>' Width="100px" ></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedDate" Text='<%# Eval("OrderDate") %>' ></asp:Label>
                            <%--<asp:Label ID="RequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDate") %>'  Width="140px"></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Requested From" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedFrom" Text='<%# Eval("reqFrom") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("reqFrom") %>'  Width="200px" ></asp:Label>--%>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Requested From ID" HeaderStyle-Width ="200px" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedFromID" Text='<%# Eval("reqFromID") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedFromID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("reqFromID") %>'  Width="200px" ></asp:Label>--%>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="role" HeaderStyle-Width ="200px" Visible="false">
                        <ItemTemplate>
                            
                            <asp:Label ID="lblSysRole" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("roleName") %>'  Width="200px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Request Status" HeaderStyle-Width ="130px">
                        <ItemTemplate>
                            <asp:Label ID="RequestStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-sm btn-default" ID="btnEdit" Text="View Request Details" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>'/>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                 </Columns>
                <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
              <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="BACK" CssClass="btn btn-large" Visible="true" />
            </div>
        </div>
    </div>
</asp:Content>
