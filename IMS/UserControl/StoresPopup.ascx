<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoresPopup.ascx.cs" Inherits="IMS.UserControl.StoresPopup" %>


 <div class="popupMain" id="products">
<div class="popupHead">
    Products List
    <input type="submit" class="close" value="" />
    </div>
     
    
    <asp:GridView ID="dgvStoresPopup" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="dgvStoresPopup_PageIndexChanging"   onrowcancelingedit="dgvStoresPopup_RowCancelingEdit" 
            onrowcommand="dgvStoresPopup_RowCommand" OnRowDataBound="dgvStoresPopup_RowDataBound" onrowdeleting="dgvStoresPopup_RowDeleting" 
            onrowediting="dgvStoresPopup_RowEditing" OnSelectedIndexChanged="dgvStoresPopup_SelectedIndexChanged" >
                 <Columns>

                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:CheckBox ID="chkCtrl" runat="server"  onClick="CheckSingleCheckbox(this);"  />
                        </ItemTemplate>
                    </asp:TemplateField>

                     <asp:BoundField DataField="StoreName" HeaderText="Store Name"   />

                      <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:BoundField DataField="SystemName" HeaderText="Store Name"   /> 
                     <asp:BoundField DataField="System_PharmacyID" HeaderText="Store ID"   />
                     <asp:BoundField DataField="SystemAddress" HeaderText="Store Address"   />
                     
                     <asp:BoundField DataField="SystemPhone" HeaderText="Store Phone"   />
                    <asp:BoundField DataField="SystemFax" HeaderText="Store Fax" />

                    
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
     
     <asp:Button ID="SelectStore" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectStore_Click"  />
 </div>