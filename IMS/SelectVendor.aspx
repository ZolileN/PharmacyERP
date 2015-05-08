<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectVendor.aspx.cs" Inherits="IMS.SelectVendor" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="VendorsPopup" TagPrefix="UCVendorsPopup"  Src="~/UserControl/VendorsPopupGrid.ascx"%>

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
        <tbody><tr>
        	<td> <h4 id="topHead">Manual PO(s)</h4></td>
            <td  >
            <%--<input type="submit" class="btn btn-success" value="Generate PO" onClick="window.location.href = 'purchase-order.html'" id="genPO">
                <input type="submit" class="btn btn-danger" value="Delete PO" id="delPO">
		    <input type="submit" class="btn btn-default" id="backPO" value="Back" onClick="location.reload();">--%>   
                
                
            </td> 
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
    
   <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label for="MainContent_RequestTo" >Select Vendor</label></td>
           <td>
               <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control product" ></asp:TextBox>
                <input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />

           </td>
           <td>
               <asp:Button ID="btnContinue" runat="server"  Text="Continue" CssClass="btn btn-primary btn-sm continue" OnClick="btnContinue_Click" Visible="false"/>
           	  
           </td>
            <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="btnSearchVendor" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                    
                       
           </tr>

        
    </table>
               
                <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:VendorsPopup  id="VendorsPopupGrid" runat="server"/>
                        </div>
             
	<img src="images/po-img.png" width="344" height="344" class="poImg">
   
        
</asp:Content>
