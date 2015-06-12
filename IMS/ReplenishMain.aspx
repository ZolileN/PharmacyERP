<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishMain.aspx.cs" Inherits="IMS.ReplenishMain" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="body-cont">
            

		
     
	  
         <table width="100%">

        <tbody>
            <tr>
        	<td> <h4 id="topHead">Replenish ( Movement )</h4></td>
            <td align="right">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" />
		    </td>
            </tr>
		    <tr><td height="5"></td></tr>
        </tbody>
        </table>
        <hr><br />

     
    
     	 <table class="formTbl" border="0" cellpadding="5" cellspacing="5" width="50%">

        <tbody><tr>
            <td><label>Vendor Name </label></td>
            <td>
                <asp:DropDownList ID ="ddlVendorNames" runat="server" OnSelectedIndexChanged="ddlVendorNames_SelectedIndexChanged"></asp:DropDownList>
            </td>
            
            <td><asp:Button ID="btnGenerate" runat="server" CssClass="btn btn-success" Text="Generate Replenishment" OnClick="btnGenerate_Click" /></td>
          <td></td>
      </tr></tbody></table>
         
         <img src="images/po-img.png" width="344" height="344" class="poImg">
         <br />

</asp:Content>
