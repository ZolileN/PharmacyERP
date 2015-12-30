<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectWarehousePharmacy.aspx.cs" Inherits="IMS.SelectWarehousePharmacy" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
            
      <table width="100%"> 
        <tbody><tr>
        	<td> <h4 id="topHead">Item Request to Store</h4></td>
            <td>
            
            </td> 
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
    <hr />
    <table>
     <tr>

         <td> <asp:Label runat="server" AssociatedControlID="ddlPH" CssClass="control-label" Visible="true">Select Pharmacy</asp:Label></td>
           <td>
               <asp:DropDownList ID="ddlPH" runat="server"  CssClass="form-control" Width="29%" Visible="true"> </asp:DropDownList>
            
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
         </tr>
        <tr><td colspan="2" height="12"></td></tr>
        <tr>
          <td> <asp:Label runat="server" AssociatedControlID="ddlWH" CssClass="control-label" Visible="true">Select Store</asp:Label></td>
           <td>
               <asp:DropDownList ID="ddlWH" runat="server"  CssClass="form-control" Width="29%" Visible="true"> </asp:DropDownList>
            
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
        </tr>
        <tr><td colspan="2" height="12"></td></tr>
        <tr>
           <td>
               <asp:Button ID="btnContinue" runat="server"  Text="Continue" CssClass="btn btn-primary btn-sm continue" OnClick="btnContinue_Click"/>
           	  
           </td>
           
                       
           </tr>
    </table>
</asp:Content>
