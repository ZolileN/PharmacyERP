<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddVendorsToStore.aspx.cs" Inherits="IMS.AddVendorsToStore" %>

<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
 
<%@ Register Src="~/UserControl/MultipleVendorsSelectPopup.ascx" TagPrefix="ucVendorsPopup" TagName="VendorsPopupGrid" %>


<%--<%@ Register Src="~/UserControl/AssociatedVendorsPopup.ascx" TagPrefix="ucVendorsPopups" TagName="AssociatedVendorsPopup" %>--%>




 


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
                <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                         <ucVendorsPopup:VendorsPopupGrid runat="server" id="MultipleVendorsSelectPopup" />
              
                        </div> 
           </td>
           <td>
                <label id="lblSelectVendorAsStore" runat="server" >Select Vendor as Store</label>
           	  
           </td>
               <td>
                   
                   <asp:DropDownList ID="ddlStoreVendors" runat="server"  OnSelectedIndexChanged="ddlStoreVendors_SelectedIndexChanged" >
                   </asp:DropDownList>
                   
                      <asp:Button ID="btnShow" runat="server" Text="Show Vendors" CssClass="btn btn-default btn-large" OnClick="btnShow_Click"/>
                   
               </td>
               <%--<div id="AssociatedVendorsGrid" style="display:none;">
                       <ucVendorsPopups:AssociatedVendorsPopup runat="server" id="AssociatedVendorsPopup" />

                       <cc1:ModalPopupExtender ID="mpeAssociatedVendorsGrid" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectVendorAsStore" ClientIDMode="AutoID"
                       PopupControlID="AssociatedVendorsGrid" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>

 
                   </div>--%>

            <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectVendor" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                   
           </tr> 

    </tbody></table>

    <br>
   <table class="table table-striped table-bordered table-condensed">
                <asp:GridView ID="dgvVendors" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                        AutoGenerateColumns="false"  ShowFooter="true" OnRowCommand="dgvVendors_RowCommand" OnRowDeleting="dgvVendors_RowDeleting" OnPageIndexChanging="dgvVendors_PageIndexChanging"  >
                        <Columns>

                             <asp:TemplateField HeaderText="Action"  >
                                <ItemTemplate>
                                    <span onclick="return confirm('Are you sure you want to delete this record?')">
                                        <asp:Button CssClass="btn btn-default del-btn"  ID="btnDelete" Text="Delete" runat="server" CommandName="Delete"  CommandArgument='<%# Container.DataItemIndex %>'/>
                                    </span>
                                </ItemTemplate> 
                                 <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                            </asp:TemplateField>

            
                            <asp:BoundField DataField="VendorName" HeaderText="Vendor Name"   />
                                  
                            <asp:TemplateField HeaderText="Address">
                                <ItemTemplate>
                                    <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="City">
                                <ItemTemplate>
                                    <asp:Label ID="Label3" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Country">
                                <ItemTemplate>
                                    <asp:Label ID="Label4" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Phone">
                                <ItemTemplate>
                                    <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Fax">
                                <ItemTemplate>
                                    <asp:Label ID="lblFax" runat="server" Text='<%# Eval("Fax") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="Email">
                                <ItemTemplate>
                                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
                            <asp:TemplateField HeaderText="ID" Visible="false">
                                <ItemTemplate>
                                    <asp:Label ID="lblStoreID" runat="server" Text='<%# Eval("StoreID") %>'></asp:Label>
                                </ItemTemplate>
                            </asp:TemplateField>
           
                        </Columns>
                        <PagerStyle CssClass="GridPager" />


                    </asp:GridView>
       </table>
    


         
</asp:Content>
