<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddVendorsToStore.aspx.cs" Inherits="IMS.AddVendorsToStore" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="MultipleVendorsSelectPopup" TagPrefix="uc1" Src="~/UserControl/MultipleVendorsSelectPopup.ascx"%>




<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">
    <tr><td width="81%">
    <h4>Assign Vendors to Store -  <span id="spnStoreName" runat="server" style="color:#2c81da"> </span></h4>
    </td>
    <td width="19%" align="right"> 
        <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click"/>

        </td>
                </tr>
                <tr><td height="5"></td></tr>
                </table>
    <hr>


     <table width="100%">

        <tbody>
           <tr>
           <td><label id="lblSelectVendor" runat="server" >Select Vendor</label></td>
           <td>
               <asp:TextBox ID="txtVendor" runat="server" CssClass="form-control product" ></asp:TextBox>
               <asp:Button ID="btnSearch" runat="server" CssClass="search-btn getProducts" OnClick="btnSearch_Click" />

               <asp:Label ID="lblVendorIds" runat="server" Visible="false"></asp:Label>

           </td>
           <td>
          
           	  
           </td>
            <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectVendor" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                    
                       
           </tr>

    </tbody></table>
         <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                         <uc1:MultipleVendorsSelectPopup runat="server" id="MultipleVendorsSelectPopup" />
              
                        </div>

</asp:Content>
