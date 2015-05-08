<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Warehouse_StoreRequests.aspx.cs" Inherits="IMS.Warehouse_StoreRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
 
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Store Requests</h4></td>
            <td align="right"> 
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
          </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>

     <table cellspacing="5" cellpadding="5" border="0" width="50%" class="formTbl">

        <tbody><tr>
            <td>          
                <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="col-md-2 control-label">Select Store </asp:Label>
            </td>
            <td>
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged"/>
            </td>
            <td></td>
          <td></td>
      </tr></tbody></table>


    <%--<div class="form-horizontal">
    <div class="form-group">
            <div class="col-md-10">
                <br/>
            </div>
    </div>
    </div>--%>

    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                    <asp:TemplateField HeaderText="Request No." HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedNO" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderID") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDate") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Request From" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Location") %>'  Width="200px" ></asp:Label>
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
                            <asp:Button CssClass="btn btn-default btn-sm" ID="btnEdit" Text="View Request Details" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>'/>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>

          <img src="images/po-img.png" width="344" height="344" class="poImg">


    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               <!-- Back Button if needed-->
            </div>
        </div>
    </div>

    <div class="form-horizontal">
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPackingList" runat="server" OnClick="btnPackingList_Click" Enabled="false" Text="Generate Packing List" CssClass="btn btn-default" Visible="false"/>
            </div>
     </div>
     </div>
</asp:Content>
