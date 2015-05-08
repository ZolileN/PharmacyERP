<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageProduct.aspx.cs" Inherits="IMS.ManageProduct" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


     <table width="100%">
         <tr><td>
        <h4>Add Product</h4>
        </td>
        <td align="right">
        
        	 <input value="ADD"  class="btn btn-primary" type="submit" onClick="window.location.href = 'AddStockEx.html'">
                <input name="ctl00$MainContent$btnCancelProduct" value="CANCEL" id="MainContent_btnCancelProduct" class="btn btn-default" type="submit" onClick="window.location.href = 'ViewInventory.html'">
        	<input type="submit" value="Go Back" class="btn btn-default btn-large" onClick="window.history.back();">
            
        </td>
        </tr>
        <tr>
        	<td height="6"></td>
        </tr>
        </table>
        <hr>
        
		
		<table width="100%" class="formTbl">
			<tr>
                 <td><asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label">Product Department</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True" OnSelectedIndexChanged="ProductDept_SelectedIndexChanged"/>

                 </td>
                
            
            <td><label>Category Name</label></td>
            <td><select>
            	<option selected="selected" value="Select System">Select Category</option>
            	<option value="1">AL AHLIYA PHARMACEUTICAL TRADING</option>
            	<option value="2">Al Ahliya-PF1137</option>
            	<option value="6">Al Ahliya International-PF1692</option>
            	<option value="10006">Head Office</option>
            	<option value="10007">OutSide Outlet</option>
            	<option value="10008">MANSOUR PHARMACY</option>
            	<option value="10009">GRAND PHARMACY</option>
            	</select></td>
				
				<td><label>Sub-Category Name</label></td>
            <td><select>
            	<option selected="selected" value="Select System">Select Sub-Category</option>
            	<option value="1">AL AHLIYA PHARMACEUTICAL TRADING</option>
            	<option value="2">Al Ahliya-PF1137</option>
            	<option value="6">Al Ahliya International-PF1692</option>
            	<option value="10006">Head Office</option>
            	<option value="10007">OutSide Outlet</option>
            	<option value="10008">MANSOUR PHARMACY</option>
            	<option value="10009">GRAND PHARMACY</option>
            	</select></td>
         
        </tr>

		</table>
		    
</asp:Content>
