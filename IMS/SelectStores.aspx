<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectStores.aspx.cs" Inherits="IMS.SelectStores" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="StoresPopup" TagPrefix="UCVendorsPopup" Src="~/UserControl/StoresPopup.ascx" %>

<%--<UCVendorsPopup:StoresPopup runat="server" ID="StoresPopup" />--%>


<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

            <script src="Scripts/modernizr-2.6.2.js"></script>
        <link href="Content/bootstrap.css" rel="stylesheet"/>
        <link href="Content/Site.css" rel="stylesheet"/>

        <script type="text/javascript" src="Scripts/jquery.min.js"></script>
        <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <link href="Style/theme.css" rel="stylesheet" type="text/css" />
        <link href="Style/fonts.css" rel="stylesheet" type="text/css" />
        <script src="Scripts/jquery-ui.js"></script>
        <script src="Scripts/general.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  <table width="100%">
    <tr><td width="81%">
    <h4>Assign Vendors to Pharmacy</h4>
    </td>
    <td width="19%" align="right"> 

        <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click" />
         </td>
                </tr>
                <tr><td height="5"></td></tr>
                </table>
     
    <hr />

    <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label id="lblSelectStore" runat="server" >Select Pharmacy</label></td>
           <td>
               <asp:TextBox ID="txtStore" runat="server" CssClass="form-control product" ></asp:TextBox>
               <asp:Button ID="btnSearch" runat="server" CssClass="search-btn getProducts" OnClick="btnSearch_Click" />
                 
               <asp:Label ID="lblStoreId" runat="server" Visible="false"></asp:Label>

           </td>
           <td>
               <asp:Button ID="btnContinue" runat="server"  Text="Continue" CssClass="btn btn-primary btn-sm continue" OnClick="btnContinue_Click" Visible="false"/>
           	  
           </td>
            <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectStore" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                    
                       
           </tr>

        
    </table>
      <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:StoresPopup  id="StoresPopupGrid" runat="server"/>
                        </div>

    	<img src="images/po-img.png" width="344" height="344" class="poImg">
   

</asp:Content>
