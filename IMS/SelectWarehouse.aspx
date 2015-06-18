<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectWarehouse.aspx.cs" Inherits="IMS.SelectWarehouse" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            
      <table width="100%"> 
        <tbody><tr>
        	<td> <h4 id="topHead">Item Request to Warehouse</h4></td>
            <td>
            
            </td> 
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
    <hr />

     <tr>
          <td> <asp:Label runat="server" AssociatedControlID="ddlWH" CssClass="control-label" Visible="true">Select Warehouse</asp:Label></td>
           <td>
               <asp:DropDownList ID="ddlWH" runat="server"  CssClass="form-control" Width="29%" Visible="true"> </asp:DropDownList>
            
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
           <td>
               <asp:Button ID="btnContinue" runat="server"  Text="Continue" CssClass="btn btn-primary btn-sm continue" OnClick="btnContinue_Click"/>
           	  
           </td>
           
                       
           </tr>
</asp:Content>
