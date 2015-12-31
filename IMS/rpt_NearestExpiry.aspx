<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_NearestExpiry.aspx.cs" Inherits="IMS.rpt_NearestExpiry" %>

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
            function pageLoad() {
                $(function () { $("#<%= txtDateFrom.ClientID %>").datepicker(); });
                $(function () { $("#<%= txtDateTO.ClientID %>").datepicker(); });
            }

                function checkDate() {

                  

                    var startDate = $("#<%= txtDateFrom.ClientID %>").val();
                    
                    var toDate = $("#<%= txtDateTO.ClientID %>").val();
                   
                    if (toDate == "") {
                      
                        return true;
                       
                    }
                    else {
                        if (isDate(toDate) == false)
                        {
                            alert("Invalid Expiry Range From Date"); return false;
                        }

                        if (new Date(toDate) <= new Date(startDate)) {

                            alert("Expiry Range To Date can not be less than Expiry Range From Date!!!"); return false;
                        }
                        else { return true; }
                    }
                  
              }
            function isDate(txtDate){
            var currVal = txtDate;
            if(currVal == '')
                return false;
  
            //Declare Regex  
            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; 
            var dtArray = currVal.match(rxDatePattern); // is format OK?

            if (dtArray == null)
                return false;
 
            //Checks for mm/dd/yyyy format.
            dtMonth = dtArray[1];
            dtDay= dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12)
                return false;
            else if (dtDay < 1 || dtDay> 31)
                return false;
            else if ((dtMonth==4 || dtMonth==6 || dtMonth==9 || dtMonth==11) && dtDay ==31)
                return false;
            else if (dtMonth == 2)
            {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay> 29 || (dtDay ==29 && !isleap))
                    return false;
            }
            return true;
            }
        </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate> 


        <table width="100%">

        <tbody><tr>
        	<td> <h4>Nearest Expiry Item Report</h4></td>
            <td align="right">
            <asp:Button ID="btnCreateReport" runat="server" CssClass="btn btn-success btn-default" Text="CREATE REPORT" OnClientClick="return checkDate();" OnClick="btnCreateReport_Click" />
            <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>

    <table class="formTbl" width="100%">

        <tbody>
            <tr>
            <td><label>Expiry Range From: </label></td>
            <td>
                <asp:TextBox ID="txtDateFrom" runat="server" ></asp:TextBox>
            </td>
            <td>
                 <label>Expiry Range To:</label>
            </td>
           
            <td>
                <asp:TextBox ID="txtDateTO" runat="server" ></asp:TextBox>
            </td>
    
        </tr>
		<tr>
            <td><asp:label ID="lblDepartment"  runat="server"><b>Department:</b></asp:label></td>
            <td>
					<%--<asp:TextBox ID="txtDepartment" Text="" runat="server"></asp:TextBox>--%>
                <asp:DropDownList ID="drpDepartments" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpDepartments_SelectedIndexChanged"></asp:DropDownList>

                    <%--<asp:Button ID="btnSeachDepartment" runat="server" CssClass="search-btn getDepartment" OnClick="btnSeachDepartment_Click" />
                    <cc1:ModalPopupExtender ID="mpeDepartmentDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblDepartment" ClientIDMode="AutoID"
                       PopupControlID="_DepartmentDiv">
                    </cc1:ModalPopupExtender>--%>
			</td>
            	
				 <td><asp:label ID="lblCategory" runat="server"><b>Category:</b></asp:label></td>
            <td>
			
                    <%--<asp:TextBox ID="txtCategory" Text="" runat="server"></asp:TextBox>--%>
                                <asp:DropDownList ID="drpCatg" runat="server" AutoPostBack="true" OnSelectedIndexChanged="drpCatg_SelectedIndexChanged"></asp:DropDownList>

                    
                   <%-- <asp:Button ID="btnSearchCategory" runat="server" CssClass="search-btn getCategory" OnClick="btnSearchCategory_Click" />
                    <cc1:ModalPopupExtender ID="mpeCategoryDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblCategory" ClientIDMode="AutoID"
                       PopupControlID="_CategoryDiv">
                    </cc1:ModalPopupExtender>--%>
            
			</td>
        </tr>
		
		<tr>
            <td><asp:label ID="lblSubCategory" runat="server"><b>Sub Category:</b></asp:label></td>
            <td>
			
                    <%--<asp:TextBox ID="txtSubcategory" Text="" runat="server"></asp:TextBox>--%>
                                                <asp:DropDownList ID="drpSubCatg" runat="server" ></asp:DropDownList>

                    
                   
                 <%--   <asp:Button ID="btnSearchSubcat" runat="server" CssClass="search-btn getSubCategory" OnClick="btnSearchSubcat_Click" />
                    <cc1:ModalPopupExtender ID="mpeSubCategoryDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSubCategory" ClientIDMode="AutoID"
                       PopupControlID="_SubCategoryDiv">
                    </cc1:ModalPopupExtender>--%>
			
			</td>
            	
				 <td><asp:label ID="lblproduct" runat="server"><b>Product:</b></asp:label></td>
            <td>
			
                    <asp:TextBox ID="txtProduct" Text="" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchProduct" runat="server" CssClass="search-btn getProducts" OnClick="btnSearchProduct_Click" />
			        <cc1:ModalPopupExtender ID="mpeProductDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblproduct" ClientIDMode="AutoID"
                       PopupControlID="_ProductDiv">
                    </cc1:ModalPopupExtender>
			</td>
        </tr>
          

    </tbody></table>

   <%-- <div id="_DepartmentDiv" class="congrats-cont" style="display: none; ">
                            <ucDepartmentPopup:DepartmentPopup  id="DepartmentPopupGrid" runat="server"/>
                        </div>

     <div id="_CategoryDiv" class="congrats-cont" style="display: none; ">
                            <ucCategoryPopup:CategoryPopup  id="CategoryPopupGrid" runat="server"/>
                        </div>

    <div id="_SubCategoryDiv" class="congrats-cont" style="display: none; ">
                            <ucSubCategoryPopup:SubCategoryPopup  id="SubCategoryPopupGrid" runat="server"/>
                        </div>--%>

    <div id="_ProductDiv" class="congrats-cont" style="display: none; ">
                            <ucProductPopup:ProductPopup  id="ProductPopupGrid" runat="server"/>
                        </div>
    
    </ContentTemplate>
        </asp:UpdatePanel>


</asp:Content>
