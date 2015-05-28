<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ProductStoreSelect.aspx.cs" Inherits="IMS.ProductStoreSelect" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="StorePopup" TagPrefix="UCStoresPopup"  Src="~/UserControl/StoresPopup.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <table width="100%"> 
        <tbody><tr>
        	<td> <h4 id="topHead">Assign Products to Store</h4></td>
           <td align="right">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Go Back" OnClick="btnBack_Click"/>
              
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
    
   <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label id="lblSelectStore" runat="server" >Select Store</label></td>
           <td>
               <asp:TextBox ID="txtStore" runat="server" CssClass="form-control product" ></asp:TextBox>
               <asp:Label ID="lblStoreId" runat="server" Visible="false"></asp:Label>
               <asp:Button ID="btnSearch" runat="server" CssClass="search-btn getProducts" OnClick="btnSearch_Click" />
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

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
                            <UCStoresPopup:StorePopup  id="StoresPopupGrid" runat="server"/>
                        </div>
             
	<img src="images/po-img.png" width="344" height="344" class="poImg">
</asp:Content>
