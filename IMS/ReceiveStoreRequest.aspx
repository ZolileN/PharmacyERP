<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveStoreRequest.aspx.cs" Inherits="IMS.ReceiveStoreRequest" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="StoresPopup" TagPrefix="UCVendorsPopup" Src="~/UserControl/StoresPopup.ascx" %>
<%@ Register TagName="SalemanPopup" TagPrefix="UCSaleman" Src="~/UserControl/uc_Select_Salesman.ascx" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">\
      <table width="100%">

        <tbody><tr>
        	<td> <h4>View Store Request(s)</h4></td>
            <td align="right">
                <asp:Button ID="btnSearch" runat="server" OnClick="btnSearch_Click" Enabled="true" Text="SEARCH" CssClass="btn btn-primary"/>
                <asp:Button ID="btnRefresh" runat="server" OnClick="btnRefresh_Click" Enabled="true" Text="REFRESH" CssClass="btn btn-info"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     
     <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
             <td>
                   <asp:Label runat="server" AssociatedControlID="ddlReqFrom"  CssClass="control-label">Request From</asp:Label>
               </td>

             <td>
                     <asp:DropDownList runat="server" ID="ddlReqFrom" CssClass="form-control" Width="280"  />
                
               </td>
             <td>
                   <asp:Label runat="server" AssociatedControlID="ddlReqStatus" CssClass="control-label">Request Status </asp:Label>
               </td>

             <td>
                     <asp:DropDownList runat="server" ID="ddlReqStatus" CssClass="form-control" Width="280" />
                
               </td>
              
         </tr>   
         
            <tr>
                <td><asp:Label id="lblSelectStore" runat="server" >Select Store</asp:Label></td>
            <td>
                <asp:Label ID="lblStoreId" runat="server" Visible="false"></asp:Label>

            	 <asp:TextBox ID="txtStore" runat="server" CssClass="form-control product" ></asp:TextBox>
           	   <asp:Button ID="btnSelectStore" runat="server" CssClass="search-btn getProducts" OnClick="btnSelectStore_Click"   />

                <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectStore" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" >
                    </cc1:ModalPopupExtender>

              <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:StoresPopup  id="StoresPopupGrid" runat="server"/>
                        </div>
            </td>
        
                <td><asp:Label id="LblSelectSalesman" runat="server" >Select Salesman</asp:Label></td>
            <td>
                <asp:Label ID="lblSlmanID" runat="server" Visible="false"></asp:Label>

            	 <asp:TextBox ID="txtSlman" runat="server" CssClass="form-control product" ></asp:TextBox>
           	   <asp:Button ID="btnSelectSaleman" runat="server" CssClass="search-btn getProducts" OnClick="btnSelectSaleman_Click"   />

                <cc1:ModalPopupExtender ID="mpecMessageDic" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="LblSelectSalesman" ClientIDMode="AutoID"
                       PopupControlID="_cMessageDiv" >
                    </cc1:ModalPopupExtender>

              <div id="_cMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCSaleman:SalemanPopup  id="salesPopupGrid" runat="server"/>
                        </div>
            </td>
         </tr>
        
             <tr>
               <td>
                   <asp:Label runat="server" AssociatedControlID="txtOrderNO"  CssClass="control-label">Request Number</asp:Label>
               </td>

                <td>
                     <asp:TextBox runat="server" ID="txtOrderNO" CssClass="form-control" />
                
               </td>
                <td>
                   <asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass="control-label">Request Date</asp:Label>
               </td>

                <td>
                    <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" />
                
               </td>
              
         </tr>
            <tr>
             <td></td>
             <td colspan="100%">
                 
            </td>
         </tr>     
        </table>
    <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>

      <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                       <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default details-btn" ID="btnEdit" Text="View Request Details" runat="server" CommandName="View" CommandArgument='<%# Container.DisplayIndex  %>'/>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
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

                     
                     <asp:TemplateField HeaderText="Request Status" HeaderStyle-Width ="130px">
                        <ItemTemplate>
                            <asp:Label ID="RequestStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                   
                     
                 </Columns>
                <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
    </div>
</asp:Content>
