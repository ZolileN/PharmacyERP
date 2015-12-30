<%@ Page Title="Inventory" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_InventoryAdjustmentReport.aspx.cs" Inherits="IMS.rpt_InventoryAdjustmentReport" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<%@ Register TagPrefix="uc" TagName="print_uc" Src="~/UserControl/uc_printBarcode.ascx" %>


<%@ Register TagName="ProductsPopup" TagPrefix="UCProductsPopup" Src="~/UserControl/ProductsPopupGrid.ascx" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <script src="Scripts/SearchSuggest.js"></script>
     <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () {
                  $("#<%= DateTextBoxFrom.ClientID %>").datepicker().on('changeDate', function (ev) {
                      $(this).valid();  // triggers the validation test
                      // '$(this)' refers to '$("#datepicker")'
                  });
                  $("#<%= DateTextBoxTo.ClientID %>").datepicker().on('changeDate', function (ev) {
                      $(this).valid();  // triggers the validation test
                      // '$(this)' refers to '$("#datepicker")'
                  });
              });

          </script>
    <style>
        .suggest_link {
            background-color: #FFFFFF;
            padding: 2px 6px 2px 6px;
        }

        .suggest_link_over {
            background-color: #3366CC;
            padding: 2px 6px 2px 6px;
        }

        #search_suggest {
            position: absolute;
            background-color: #FFFFFF;
            text-align: left;
            border: 1px solid #000000;
            overflow: auto;
        }
    </style>

    <asp:Label runat="server" ID="NoProductMessage" CssClass="control-label" Visible="false" Text="No Stock Available"></asp:Label>

    <table width="100%">

        <tbody>
            <tr>
                <td>
                    <h4>Inventory Adjustment Report</h4>
                </td>
                <td align="right">
                    <asp:Button ID="btnSearch" runat="server" OnClick="btnShowREPORT_Click" OnClientClick="return VerifyCredentials();"  Enabled="true" Text="Show REPORT" CssClass="btn btn-primary" />
                    
                    
                    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click" />
                    
                    
                </td>
            </tr>
            <tr>
                <td height="5"></td>
            </tr>
        </tbody>

    </table>
    <hr />
    <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
        <tr>
            <td>
                <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="control-label" Visible="true">Product Department</asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" Visible="true" AppendDataBoundItems="True" /></td>
            <td>
                <asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="control-label" Visible="true">Product Category</asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" Visible="true" /></td>
        </tr>
        <tr>
            <td>
                <asp:Label runat="server" AssociatedControlID="ProductSubCat" CssClass=" control-label" Visible="true"> Product SubCategory </asp:Label></td>
            <td>
                <asp:DropDownList runat="server" ID="ProductSubCat" CssClass="form-control" Width="29%" Visible="true" /></td>
            
            <td>
                <asp:Label runat="server" ID="lblProd" AssociatedControlID="txtSearch" CssClass="control-label">Select Product</asp:Label></td>
           <td>
                <input type="text" id="txtSearch" runat="server" name="txtSearch"  />
                
            </td>
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="lblPharmacy" AssociatedControlID="pharmacyList">Pharmacy <span style="color:red;font-weight:bold;font-size:14px;"> * </span> :</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="pharmacyList"></asp:DropDownList></td>
            <td><asp:Label runat="server" ID="Label1" AssociatedControlID="filterby">Filter By <span style="color:red;font-weight:bold;font-size:14px;"> * </span> :</asp:Label></td>
            <td><asp:DropDownList runat="server" ID="filterby"></asp:DropDownList></td>
        </tr>
        <tr>
            <td>
                 <asp:Label runat="server" AssociatedControlID="DateTextBoxFrom"  CssClass="control-label">Date From <span style="color:red;font-weight:bold;font-size:14px;"> * </span> :</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="DateTextBoxFrom" CssClass="form-control" />
            </td>


            <td>
                 <asp:Label runat="server" AssociatedControlID="DateTextBoxTo"  CssClass="control-label">Date To <span style="color:red;font-weight:bold;font-size:14px;"> * </span> :</asp:Label>
            </td>
            <td>
                <asp:TextBox runat="server" ID="DateTextBoxTo" CssClass="form-control" />
            </td>
        </tr>
        <tr>
           
           
        </tr>
    </table>

    <div class="form-horizontal">
        <div class="form-group">
           

            <div class="form-group">
                <div class="col-md-offset-2 col-md-10">
                </div>
            </div>
        </div>
    </div>

    <script>

        function VerifyCredentials() {
           // alert($("#<%= DateTextBoxFrom.ClientID %>").val() + " " + $("#<%= DateTextBoxTo.ClientID %>").val());
            if (isDate($("#<%= DateTextBoxFrom.ClientID %>").val()) && isDate($("#<%= DateTextBoxTo.ClientID %>").val())) {
                return true;
            }
            else {
                alert("Please select valid dates");
                return false;
            }
        }

        function isDate(txtDate) {
            var currVal = txtDate;
            if (currVal == '')
                return false;

            var rxDatePattern = /^(\d{1,2})(\/|-)(\d{1,2})(\/|-)(\d{4})$/; //Declare Regex
            var dtArray = currVal.match(rxDatePattern); // is format OK?

            if (dtArray == null)
                return false;

            //Checks for mm/dd/yyyy format.
            dtMonth = dtArray[1];
            dtDay = dtArray[3];
            dtYear = dtArray[5];

            if (dtMonth < 1 || dtMonth > 12)
                return false;
            else if (dtDay < 1 || dtDay > 31)
                return false;
            else if ((dtMonth == 4 || dtMonth == 6 || dtMonth == 9 || dtMonth == 11) && dtDay == 31)
                return false;
            else if (dtMonth == 2) {
                var isleap = (dtYear % 4 == 0 && (dtYear % 100 != 0 || dtYear % 400 == 0));
                if (dtDay > 29 || (dtDay == 29 && !isleap))
                    return false;
            }
            return true;
        }

    </script>

    <asp:Button ID="_editPopupButton" runat="server" Style="display: none" />
    <%--<cc1:ModalPopupExtender ID="mpeEditProduct" runat="server" RepositionMode="RepositionOnWindowResizeAndScroll" DropShadow="true" 
            PopupDragHandleControlID="_prodEditPanel" TargetControlID="_editPopupButton" PopupControlID="_prodEditPanel" BehaviorID="EditModalPopupMessage">
        </cc1:ModalPopupExtender>--%>

    <asp:Panel ID="_prodEditPanel" runat="server" Width="100%" Style="display: none">
        <asp:UpdatePanel ID="_prodEdit" runat="server">
            <ContentTemplate>
                <uc:print_uc ID="ucPrint" runat="server" />
            </ContentTemplate>
        </asp:UpdatePanel>
    </asp:Panel>
</asp:Content>
