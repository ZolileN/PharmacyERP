<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishMovement.aspx.cs" Inherits="IMS.ReplenishMovement" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="VendorsPopup" TagPrefix="UCVendorsPopup"  Src="~/UserControl/VendorsPopupGrid.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    
    <div class="body-cont">
            

		
     
	  
      <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead"><asp:Label ID="lblReplenishHeader" runat="server" Text="Replenish ( Movement )"></asp:Label></h4></td>
            <td align="right">
            
           <!-- onClick="window.location.href='purchase-order.html'" -->

		  <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" />
		    </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
      <hr>

        <asp:GridView ID="gvVendorNames" runat="server" OnRowDataBound="gvVendorNames_RowDataBound" 
                      OnSelectedIndexChanged ="gvVendorNames_SelectedIndexChanged" OnPageIndexChanging="gvVendorNames_PageIndexChanging"
                     AutoGenerateColumns="false" OnRowCommand="gvVendorNames_RowCommand" BorderWidth="0px" Width="100%">
            <Columns>                
                <asp:TemplateField>
                    <ItemTemplate>
                        <table class="table table-striped table-bordered table-condensed tblBtm vStockGrid" width:"100%">
		                <tbody>
                        <tr>
        	                <td colspan="6">
                                    <asp:Label ID="hdnVendorID" runat="server" Visible="false" Text='<%# Eval("VendorID") %>' CommandArgument='<%# Container.DataItemIndex %>'/>
    		                        <h4 class="fl-l"><%#Eval("VendorName") %></h4>
                                    <asp:Button ID="btnCreatePO" runat="server" CommandName="CreatePO"  CommandArgument='<%# Eval("VendorID").ToString() %>' CssClass="btn btn-success fl-r" Text="Create PO"/>
                            </td>
                        </tr>
                            <tr><td>
                                 <asp:GridView ID="gvVendorProducts" runat="server" OnRowDataBound="gvVendorProducts_RowDataBound" 
                                      OnSelectedIndexChanged="gvVendorProducts_SelectedIndexChanged" OnRowCommand="gvVendorProducts_RowCommand"
                                      OnPageIndexChanging="gvVendorProducts_PageIndexChanging" OnRowEditing="gvVendorProducts_RowEditing"
                                      OnRowCancelingEdit="gvVendorProducts_RowCancelingEdit" OnRowDeleting="gvVendorProducts_RowDeleting"
                                       AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-condensed tblBtm vStockGrid" BorderWidth="0px">

                                 <Columns>
                                     <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductID" runat="server" Text='<%# Eval("ProductID") %>' CommandArgument='<%# Container.DataItemIndex %>' ></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                                    </asp:TemplateField>

                                     <asp:TemplateField Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="lblVendorID" runat="server" Text='<%# Eval("VendorID") %>'  CommandArgument='<%# Container.DataItemIndex %>'></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle  Width="1px" HorizontalAlign="Left"/>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Action">
                                        <ItemTemplate>
                                            <asp:Button CssClass="btn btn-default edit-btn editPOV" ID="btnEdit" runat="server" 
                                                            CommandName="Edit"  CommandArgument='<%# Container.DataItemIndex %>'/>

                                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                            <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" runat="server" 
                                                          CommandName="Delete"  CommandArgument='<%# Eval("ProductID").ToString() + "," + Eval("VendorID").ToString() %>'/>
                                            </span>
                                        </ItemTemplate>

                                        <EditItemTemplate>

                                        <asp:LinkButton CssClass="cancelPOE btn btn-primary btn-xs" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock"
                                                        CommandArgument='<%# Eval("ProductID").ToString() + "," + Eval("VendorID").ToString() %>'/>

                                        <asp:LinkButton CssClass="btn btn-default btn-xs cancelPOE" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />

                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Product Description">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductName" runat="server" Text='<%# Eval("Description") %>' CommandArgument='<%# Container.DataItemIndex %>'></asp:Label>
                                        </ItemTemplate>
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Quantity">
                                        <ItemTemplate>
                                            <asp:Label ID="lblProductQuantity" runat="server" Text='<%# Eval("QtySold") %>' CommandArgument='<%# Container.DataItemIndex %>'></asp:Label>
                                        </ItemTemplate>
                                        <EditItemTemplate>
                                        <asp:TextBox ID="txtQuantity" CssClass="grid-input-form" runat="server" Text='<%#Eval("QtySold") %>' ></asp:TextBox>
                                        </EditItemTemplate>
                                    </asp:TemplateField>

                                     <asp:TemplateField HeaderText="Assign to different Vendor" HeaderStyle-Width="19%">
                                        <ItemTemplate>
                                            <asp:DropDownList ID="ddlPreviousVendors" runat="server" CssClass="atds" 
                                                CommandName="DropDown"  CommandArgument='<%# Container.DataItemIndex %>' AutoPostBack="true" OnSelectedIndexChanged="ddlPreviousVendors_SelectedIndexChanged"></asp:DropDownList>

                                             <asp:Button ID="btnAddNewVendor" runat="server" CssClass="btn btn-default" CommandName="NewVendor"
                                                  CommandArgument='<%# Eval("ProductID").ToString() + "," + Eval("VendorID").ToString() %>' Text="ADD"   /> 
                                        </ItemTemplate>
                                        <ItemStyle  Width="19%" HorizontalAlign="Left"/>
                                    
                                    </asp:TemplateField>
                                    <%-- <asp:TemplateField> btn btn-info btn-sm opPop
                                          CommandArgument='<%# Container.DataItemIndex %>' AutoPostBack="true"
                                           OnClick="btnAddNewVendor_Click"
                                         <ItemStyle  Width="19%" HorizontalAlign="Left"/>
                                     </asp:TemplateField>--%>

                                </Columns>
                        </asp:GridView>
                            </td></tr>
                        </tbody>
                        </table>
                    </ItemTemplate>
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
         <label id="lblSelectVendor" Text="" runat="server"> </label>
     <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectVendor" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
      <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCVendorsPopup:VendorsPopup  id="VendorsPopupGrid" runat="server"/>
                        </div>
      </div>
   
    
	<%--<script src="Scripts/jquery-ui.js"></script>
    <script src="Scripts/general.js"></script>--%>
    
    
    
   
    
    
    
</asp:Content>
