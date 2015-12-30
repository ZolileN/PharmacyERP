<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewDirectPurchaseRequests.aspx.cs" Inherits="IMS.ViewDirectPurchaseRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <%-- <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>--%>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <table width="100%">

        <tbody><tr>
        	<td> <h4>View Direct Purchase Order(s)</h4></td>
            <td align="right">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Enabled="true" Text="SEARCH" CssClass="btn btn-primary"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="REFRESH" CssClass="btn btn-info"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>

        <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
               <td>
                   <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Vendor Name </asp:Label>
               </td>

             <td>
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control product" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged"  DataTextField="SupName" DataValueField="SuppID"  />

                   <%--<asp:SqlDataSource ID="StockAtDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSConnectionString %>" SelectCommand="SELECT [SuppID], [SupName] FROM [tblVendor]"></asp:SqlDataSource>--%>

                   <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product" Visible="false"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Visible="false" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
                
               </td>
        
               <td>
                   <asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass="control-label">Order Date</asp:Label>
               </td>

             <td>
                    <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" />
                
               </td>
         </tr>
            <tr>
               <td>
                   <asp:Label runat="server" AssociatedControlID="txtOrderNO"  CssClass="control-label">Order Number</asp:Label>
               </td>

             <td>
                     <asp:TextBox runat="server" ID="txtOrderNO" CssClass="form-control" />
                
               </td>
       
               <td>
                   <asp:Label runat="server" AssociatedControlID="OrderStatus" CssClass="control-label">Order Status </asp:Label>
               </td>

             <td>
                     <asp:DropDownList runat="server" ID="OrderStatus" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="OrderStatus_SelectedIndexChanged"/>
                
               </td>
         </tr>
            <tr>
             <td></td>
             <td colspan="100%">
                 
            </td>
         </tr>     
        </table>


    <%--<div class="form-horizontal">
     <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="col-md-2 control-label">Vendor Name </asp:Label>
            <div class="col-md-6">
                <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Text="SearchProduct" Height="35px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                <br />
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged" Visible="false"/>
                <br/>
            </div>
    </div>

     <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass="col-md-2 control-label">Order Date</asp:Label>
            <div class="col-md-6">
                 <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" />
                 <br />
            </div>
            
    </div>

     <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="txtOrderNO"  CssClass="col-md-2 control-label">Order Number</asp:Label>
            <div class="col-md-6">
                 <asp:TextBox runat="server" ID="txtOrderNO" CssClass="form-control" />
                 <br />
            </div>
            
    </div>

     <div class="form-group">
        <asp:Label runat="server" AssociatedControlID="OrderStatus" CssClass="col-md-2 control-label">Order Status </asp:Label>
            <div class="col-md-6">
                <asp:DropDownList runat="server" ID="OrderStatus" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="OrderStatus_SelectedIndexChanged"/>
                <br/>
            </div>
    </div>

     <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Enabled="true" Text="SEARCH" CssClass="btn btn-default"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="REFRESH" CssClass="btn btn-default"/>
            </div>
        </div>
    </div>--%>

    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound" OnRowDeleting="StockDisplayGrid_RowDeleting">
                 <Columns>
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" Visible="false" CommandName="Edit"/>
                            <asp:Button CssClass="btn btn-default details-btn" ID="btnDetails" Text="Details" runat="server" CommandName="Details" CommandArgument='<%# Container.DisplayIndex  %>'/>
                             <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                           
                              </ItemTemplate>
                          <EditItemTemplate>
                            <asp:LinkButton ID="btnUpdate" CssClass="btn btn-primary btn-sm" Text="Re-Generate Order" runat="server" CommandName="ReGen"></asp:LinkButton>
                            <%--<asp:LinkButton  ID="btnView" CssClass="btn btn-primary btn-sm"  Text="Update Accepted Order"  runat="server" CommandName="UpdateRec"  Visible='<%# IsStatusComplete((String) Eval("Status")) %>' />--%>
                            <asp:LinkButton ID="btnCancel" Text="Cancel" CssClass="btn btn-default btn-sm"  runat="server" CommandName="Cancel" />

                        </EditItemTemplate>
                         <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Order No." HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="OrderNo" runat="server" Text='<%# Eval("OrderID") %>'></asp:Label>
                            <%--<asp:Label ID="OrderNO" CssClass="control-label" runat="server" Text='<%# Eval("OrderID") %>' Width="100px" ></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Order Date" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="OrderDate" Text='<%# Eval("OrderDate") %>'></asp:Label>
                            <%--<asp:Label ID="OrderDate" CssClass="control-label" runat="server" Text='<%# Eval("OrderDate") %>'  Width="180px"></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Order To" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="OrderTo" Text='<%# Eval("Location") %>'></asp:Label>
                            <%--<asp:Label ID="OrderTo" CssClass="control-label" runat="server" Text='<%# Eval("Location") %>'  Width="300px" ></asp:Label>--%>
                        </ItemTemplate>
                         <ItemStyle  Width="300px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Order Status" HeaderStyle-Width ="130px">
                        <ItemTemplate>
                            <%--<asp:Literal runat="server" ID="OrderStatus" Text='<%# Eval("Status") %>'></asp:Literal>--%>
                            <asp:Label ID="OrderStatus" CssClass="control-label" runat="server" Text='<%# Eval("Status") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     
                 </Columns>
                <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
    </div>
   
</asp:Content>
