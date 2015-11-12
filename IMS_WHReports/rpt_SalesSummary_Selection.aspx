<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_SalesSummary_Selection.aspx.cs" Inherits="IMS.rpt_SalesSummary_Selection" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="CustomerPopup" TagPrefix="ucCustomersPopup"  Src="~/UserControl/rpt_ucCustomers.ascx"%>
<%@ Register TagName="DepartmentPopup" TagPrefix="ucDepartmentPopup"  Src="~/UserControl/rpt_ucDepartment.ascx"%>
<%@ Register TagName="CategoryPopup" TagPrefix="ucCategoryPopup"  Src="~/UserControl/rpt_ucCategory.ascx"%>
<%@ Register TagName="SubCategoryPopup" TagPrefix="ucSubCategoryPopup"  Src="~/UserControl/rpt_ucSubCategory.ascx"%>
<%@ Register TagName="ProductPopup" TagPrefix="ucProductPopup"  Src="~/UserControl/rpt_ucSalesProduct.ascx"%>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
      <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
      <script>
          $(function () { $("#<%= txtDateFrom.ClientID %>").datepicker(); });
          $(function () { $("#<%= txtDateTO.ClientID %>").datepicker(); });
          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Item Sold Summary Report</h4></td>
            <td align="right">
            <asp:Button ID="btnCreateReport" runat="server" CssClass="btn btn-success btn-default" Text="CREATE REPORT" OnClick="btnCreateReport_Click" />
            <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>

    <table class="formTbl" width="100%">

        <tbody><tr>
            <td><label>Date From: </label></td>
          <td>
                <asp:TextBox ID="txtDateFrom" runat="server" OnClientClick="return ValidateForm();"></asp:TextBox>
          </td>
            <td>
                 <label>Date To</label>
            </td>
            <td>
                <asp:TextBox ID="txtDateTO" runat="server" OnClientClick="return ValidateForm();"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td><asp:label ID="lblCustomers" runat="server"><b>Customers:</b></asp:label></td>
            <td>
			
					<asp:TextBox ID="txtCustomers" Text="" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchCustomers" runat="server" CssClass="search-btn getCustomers" OnClick="btnSearchCustomers_Click" />
                    <cc1:ModalPopupExtender ID="mpeCustomersDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblCustomers" ClientIDMode="AutoID"
                       PopupControlID="_CustomersDiv">
                    </cc1:ModalPopupExtender>
			
			</td>
            	
				 <td><label>Barter Customer:</label></td>
            <td>
			     <asp:DropDownList ID="ddlBarterCustomer" runat="server" OnSelectedIndexChanged="ddlBarterCustomer_SelectedIndexChanged" Enabled="true"></asp:DropDownList>
			</td>
        </tr>

        
		<tr>
            <td><asp:label ID="lblDepartment" runat="server"><b>Department:</b></asp:label></td>
            <td>
					<asp:TextBox ID="txtDepartment" Text="" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSeachDepartment" runat="server" CssClass="search-btn getDepartment" OnClick="btnSeachDepartment_Click" />
                    <cc1:ModalPopupExtender ID="mpeDepartmentDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblDepartment" ClientIDMode="AutoID"
                       PopupControlID="_DepartmentDiv">
                    </cc1:ModalPopupExtender>
			</td>
            	
				 <td><asp:label ID="lblCategory" runat="server"><b>Category:</b></asp:label></td>
            <td>
			
                    <asp:TextBox ID="txtCategory" Text="" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchCategory" runat="server" CssClass="search-btn getCategory" OnClick="btnSearchCategory_Click" />
                    <cc1:ModalPopupExtender ID="mpeCategoryDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblCategory" ClientIDMode="AutoID"
                       PopupControlID="_CategoryDiv">
                    </cc1:ModalPopupExtender>
            
			</td>
        </tr>
		
		<tr>
            <td><asp:label ID="lblSubCategory" runat="server"><b>Sub Category:</b></asp:label></td>
            <td>
			
                    <asp:TextBox ID="txtSubcategory"  Text=""  runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchSubcat" runat="server" CssClass="search-btn getSubCategory" OnClick="btnSearchSubcat_Click" />
                    <cc1:ModalPopupExtender ID="mpeSubCategoryDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSubCategory" ClientIDMode="AutoID"
                       PopupControlID="_SubCategoryDiv">
                    </cc1:ModalPopupExtender>
			
			</td>
            	
				 <td><asp:label ID="lblproduct" runat="server"><b>Product:</b></asp:label></td>
            <td>
			
                    <asp:TextBox ID="txtProduct" Text=""  runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchProduct" runat="server" CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click" />
			        <cc1:ModalPopupExtender ID="mpeProductDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblproduct" ClientIDMode="AutoID"
                       PopupControlID="_ProductDiv">
                    </cc1:ModalPopupExtender>
			</td>
        </tr>
		<tr>
            <td><asp:label ID="lblInternalCustomer" runat="server"><b>Internal Customer:</b></asp:label></td>
            <td><asp:DropDownList ID="ddlInternalCustomer" runat="server" OnSelectedIndexChanged="ddlInternalCustomer_SelectedIndexChanged" Enabled="true"></asp:DropDownList></td>      
        </tr>

    </tbody></table>

     <div id="_CustomersDiv" class="congrats-cont" style="display: none; ">
                            <ucCustomersPopup:CustomerPopup  id="CustomerPopupGrid" runat="server"/>
                        </div>

    <div id="_DepartmentDiv" class="congrats-cont" style="display: none; ">
                            <ucDepartmentPopup:DepartmentPopup  id="DepartmentPopupGrid" runat="server"/>
                        </div>

     <div id="_CategoryDiv" class="congrats-cont" style="display: none; ">
                            <ucCategoryPopup:CategoryPopup  id="CategoryPopupGrid" runat="server"/>
                        </div>

    <div id="_SubCategoryDiv" class="congrats-cont" style="display: none; ">
                            <ucSubCategoryPopup:SubCategoryPopup  id="SubCategoryPopupGrid" runat="server"/>
                        </div>

    <div id="_ProductDiv" class="congrats-cont" style="display: none; ">
                            <ucProductPopup:ProductPopup  id="ProductPopupGrid" runat="server"/>
                        </div>
    <script type="text/javascript">
        function ValidateForm() {

            if (document.getElementById("MainContent_txtDateFrom").value == null || document.getElementById("MainContent_txtDateFrom").value == '') {
                alert("Please enter Cost Price");
                return false;
            }
            if (document.getElementById("MainContent_txtDateTO").value == null || document.getElementById("MainContent_txtDateTO").value == '') {
                alert("Please enter Sale Price");
                return false;
            }

            return true;

        }
        </script>
</asp:Content>
