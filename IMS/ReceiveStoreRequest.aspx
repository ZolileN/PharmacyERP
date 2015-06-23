<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveStoreRequest.aspx.cs" Inherits="IMS.ReceiveStoreRequest" %>
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
     <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>
     <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
               <td>
                   <asp:Label runat="server" AssociatedControlID="StockAt" CssClass="control-label">Request From </asp:Label>
               </td>

             <td>
                <asp:DropDownList runat="server" ID="StockAt" CssClass="form-control product" Width="29%" AutoPostBack="True" OnSelectedIndexChanged="StockAt_SelectedIndexChanged"  DataTextField="SupName" DataValueField="SuppID"  />

                   <%--<asp:SqlDataSource ID="StockAtDataSource" runat="server" ConnectionString="<%$ ConnectionStrings:IMSConnectionString %>" SelectCommand="SELECT [SuppID], [SupName] FROM [tblVendor]"></asp:SqlDataSource>--%>

                   <asp:TextBox runat="server" ID="SelectProduct" CssClass="form-control product" Visible="false"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" OnClick="btnSearchProduct_Click" Visible="false" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
                
               </td>
        
                <td>
                   <asp:Label runat="server" AssociatedControlID="OrderStatus" CssClass="control-label">Request Status </asp:Label>
               </td>

             <td>
                     <asp:DropDownList runat="server" ID="OrderStatus" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="OrderStatus_SelectedIndexChanged"/>
                
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
                   <asp:Label runat="server" AssociatedControlID="OrderStatus" CssClass="control-label">Request Status </asp:Label>
               </td>

             <td>
                     <asp:DropDownList runat="server" ID="DropDownList1" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="OrderStatus_SelectedIndexChanged"/>
                
               </td>
              
         </tr>
            <tr>
             <td></td>
             <td colspan="100%">
                 
            </td>
         </tr>     
        </table>
</asp:Content>
