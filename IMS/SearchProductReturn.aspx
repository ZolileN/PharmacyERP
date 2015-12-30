<%@ Page Language="C#" MasterPageFile="~/Site.Master"   AutoEventWireup="true" CodeBehind="SearchProductReturn.aspx.cs" Inherits="IMS.SearchProductReturn"  MaintainScrollPositionOnPostback="true" %>

<%@ Register Assembly="AjaxControlToolkit" Namespace="AjaxControlToolkit" TagPrefix="cc1" %>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate> 
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Select Product</h4>
            </td>
            <td align="right">
                <asp:Button ID="btnGenerateReturns" runat="server"  Text="Generate Returns" CssClass="btn btn-success btn-large" OnClick="btnGenerateReturns_Click" />
               
                <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />       
          </td>
        </tr>
		<tr><td > <asp:Label ID="lblVendor" runat="server" style="color:#2c81da"></asp:Label></td></tr>
    </tbody></table>
     <hr>
    
        <table cellspacing="5" cellpadding="5" border="0" width="50%" class="formTbl">
            <tr>
                <td>
                     <asp:Label runat="server" id="lblSearchBy" CssClass="control-label">Search by Name </asp:Label>
                </td>
                <td>                <asp:TextBox runat="server" ID="txtProductName" CssClass="form-control product"  />
               
            
                 <asp:ImageButton ID="btnSearchProduct" runat="server" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" OnClick="btnSearchProduct_Click" />
                 </td>
            
                </tr>
          
  
    
   </table>

    <br />
     <asp:Panel ID="pnlgdvProducts" runat="server" CssClass="popupHead" Visible="false">
                Products
            </asp:Panel>
    <asp:GridView ID="gdvProducts" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True"
                AutoGenerateColumns="False" OnPageIndexChanging="gdvProducts_PageIndexChanging" EmptyDataText="No Record Found!" DataKeyNames="ProductID" >
            <Columns>
               
          <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
               
                <asp:LinkButton ID="lnkView" runat="server" CssClass="btn btn-default edit-btn" OnClick="lnkView_Click"></asp:LinkButton>
            </ItemTemplate>
        </asp:TemplateField>


                <asp:BoundField DataField="ProductID" HeaderText="ProductID" Visible="False" />
                <asp:BoundField DataField="Product_Id_Org" HeaderText="UPC" />
                <asp:BoundField DataField="Product_Name" HeaderText="Name" />
                <asp:BoundField DataField="UnitCost" HeaderText="Unit Cost" />
                <asp:BoundField DataField="QtyinHand" HeaderText="QtyinHand" />
            </Columns>
            <PagerStyle CssClass = "GridPager" />
    </asp:GridView>
          
            <asp:Panel ID="panelgrvReturns" runat="server" CssClass="popupHead" Visible="false">
                Items Returned
            </asp:Panel>
    <asp:GridView ID="grvReturns" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed"
                AutoGenerateColumns="False" OnRowCommand="grvReturns_RowCommand" DataKeyNames="StockID" >
            <Columns>
               
            <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                           
                           
                            <asp:Label ID="Label2" runat="server" Text=''>
                             
                                <span onclick="return confirm('Are you sure you want to delete this record?')">
                                    <asp:LinkButton ID="lnkDelete" CssClass="btn btn-default del-btn"  runat="server" CommandName="deleterow">
                                    Delete
                                    </asp:LinkButton>
                                </span>
                             </asp:Label>
                             
                        </ItemTemplate>
                     </asp:TemplateField>


                <asp:BoundField DataField="StockID" HeaderText="StockID" Visible="False" />
                <asp:BoundField DataField="ProductID" HeaderText="ProductID" Visible="False" />
                
               <asp:BoundField DataField="Name" HeaderText="Name" />
                <asp:BoundField DataField="BatchNo" HeaderText="BatchNo" />
                 <asp:BoundField DataField="Expiry" HeaderText="Expiry" />
                <asp:BoundField DataField="ReturnQty" HeaderText="ReturnQty" />
                 
            </Columns>
         
    </asp:GridView>





            <asp:HiddenField ID="hdfSupId" runat="server" Value="-1" />
            
            <asp:Panel ID="poupBatch" runat="server">
               <div class="popupHead">
   Batches List

                   <asp:Button ID="btnClose" runat="server" Text="" CssClass="close" OnClick="btnClose_Click" />
                 
    </div>
          <div style="border:solid;border-color:#6fb4eb;padding:0px; width:500px;">
<asp:GridView ID="grvBatches" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" 
                AutoGenerateColumns="False" EmptyDataText="No Record Found!" OnRowDataBound="grvBatches_RowDataBound" style=" margin-top: 0px; margin-bottom:1px;" DataKeyNames="StockID" >
            <Columns>
               
          <asp:TemplateField HeaderText="Action">
            <ItemTemplate>
                <asp:CheckBox ID="chkRowBtaches" runat="server"  AutoPostBack="True" OnCheckedChanged="chkRowBtaches_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>


                <asp:BoundField DataField="StockID" HeaderText="StockID" Visible="False" />
                <asp:BoundField DataField="BatchNumber" HeaderText="BtachNo" />
                <asp:BoundField DataField="ExpiryDate" HeaderText="Expiry" />
              
                <asp:BoundField DataField="Stock" HeaderText="Qty" />

                  <asp:TemplateField HeaderText="Return Qty">
            <ItemTemplate>
              <asp:TextBox ID="txtReturnQty"  runat="server" AutoPostBack="true" Enabled="false" Text="" OnTextChanged="txtReturnQty_TextChanged"></asp:TextBox>
                <cc1:FilteredTextBoxExtender ID="FilteredTextBoxExtender1" runat="server" Enabled="true" TargetControlID="txtReturnQty" FilterType="Numbers" ></cc1:FilteredTextBoxExtender>
               
            </ItemTemplate>
        </asp:TemplateField>
            </Columns>
            <PagerStyle CssClass = "GridPager" />
    </asp:GridView>
            <div style="background-color:white;margin-top:0px;">
 <asp:Button ID="btnPoupSelect" runat="server"  Text="Select" CssClass="btn btn-primary" OnClick="btnPoupSelect_Click"/>
               </div>

               
             </div>
  
            </asp:Panel>
           
            <asp:Button ID="Button1" runat="server" Text=""  Height="0px" Width="0px"  />
            <asp:HiddenField ID="hdfProductId" runat="server" Value="" />
             <asp:HiddenField ID="hdfProdutName" runat="server" Value="" />
            <cc1:ModalPopupExtender TargetControlID="Button1" CancelControlID="btnClose" ID="mPoupBatch" runat="server" PopupControlID="poupBatch"></cc1:ModalPopupExtender>
       </ContentTemplate>
        <Triggers>
            <asp:PostBackTrigger ControlID="grvBatches" />
         
            
        </Triggers>
    </asp:UpdatePanel>
    
</asp:Content>
