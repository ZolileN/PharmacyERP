<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishMain.aspx.cs" Inherits="IMS.ReplenishMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="body-cont">
            

		
     
	  
         <table width="100%">

        <tbody>
            <tr>
        	<td> <h4 id="topHead"><asp:Label ID="lblReplenishHeader" runat="server" Text="Replenish ( Movement )"></asp:Label></h4></td>
            <td align="right">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" />
		    </td>
            </tr>
		    <tr><td height="5"></td></tr>
        </tbody>
        </table>
        <hr><br />

     
    
     	 <table class="formTbl" border="0" cellpadding="5" cellspacing="5" width="50%">

        <tbody>
            <tr>
            <td><asp:label ID ="lblSaleDates" runat="server">Sale Dates</asp:label></td>
            <td>
            </td>
            <td></td>
            </tr>

            <tr>
            <td><asp:label ID="lblFromDate" runat="server">From:</asp:label></td>
            <td>
                <asp:TextBox ID="txtFromDate" runat="server" Visible="false"></asp:TextBox>
            </td>
            <td></td>
            </tr>

            <tr>
            <td><asp:label ID="lblToDate" runat="server">To:</asp:label></td>
            <td>
                <asp:TextBox ID="txtToDate" runat="server" Visible="false"></asp:TextBox>
            </td>
            <td></td>
            </tr>


            <tr>
            <td><asp:label ID="lblReplenishDays" runat="server">Replenisment For Days:</asp:label></td>
            <td>
                <asp:TextBox ID="txtReplenishDays" runat="server"  Visible="false"></asp:TextBox>
            </td>
            <td></td>
            </tr>

            <tr>
            <td><asp:label ID ="lblVendor" runat="server">Vendor Name </asp:label></td>
            <td>
                <asp:DropDownList ID ="ddlVendorNames" runat="server" OnSelectedIndexChanged="ddlVendorNames_SelectedIndexChanged"></asp:DropDownList>
            </td>
            
            <td><asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-success" Text="Generate Replenishment" OnClick="btnGenerate_Click" /></td>
          <td></td>
      </tr></tbody></table>
         
         <img src="images/po-img.png" width="344" height="344" class="poImg">
         <br />
          <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
           <script>
               $(function () { $("#<%= txtFromDate.ClientID %>").datepicker(); });
               $(function () { $("#<%= txtToDate.ClientID %>").datepicker(); });
          </script>
</asp:Content>
