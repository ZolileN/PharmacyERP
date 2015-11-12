<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="StoresPopup.ascx.cs" Inherits="IMS_WHReports.UserControl.StoresPopup" %>


 <div class="popupMain" id="products">
<div class="popupHead">
    Pharmacy List
    <input type="submit" class="close" value="" />
    </div>
     
      <div class="bodyPop">
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


                       
                      <%--<asp:TemplateField HeaderText="UPC">

                       <%--<asp:BoundField DataField="SystemID" HeaderText="System ID" Visible="false"  />--%> 
                      <asp:TemplateField HeaderText="UPC" Visible="false">

                        <ItemTemplate>
                            <asp:Label ID="SystemID"  runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                     <asp:BoundField DataField="SystemName" HeaderText="Pharmacy Name"   /> 
                     <asp:BoundField DataField="System_PharmacyID" HeaderText="Pharmacy ID"   />
                     <asp:BoundField DataField="SystemAddress" HeaderText="Pharmacy Address"   />
                     
                     <asp:BoundField DataField="SystemPhone" HeaderText="Pharmacy Phone"   />
                    <asp:BoundField DataField="SystemFax" HeaderText="Pharmacy Fax" />

                     <asp:BoundField DataField="SystemID" HeaderText="Pharmacy ID" /> 
                     <asp:BoundField DataField="Quantity" HeaderText="Quantity" /> 
                    
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
     
     <asp:Button ID="SelectStore" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectStore_Click"  />
 </div>
     </div>
 <script type="text/javascript">
     function CheckSingleCheckbox(ob) {
         var grid = ob.parentNode.parentNode.parentNode;
         var inputs = grid.getElementsByTagName("input");
         for (var i = 0; i < inputs.length; i++) {
             if (inputs[i].type == "checkbox") {
                 if (ob.checked && inputs[i] != ob && inputs[i].checked) {
                     inputs[i].checked = false;
                 }
             }
         }
     }
    </script>