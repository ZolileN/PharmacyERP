<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_PendingSalesOrderPopUp.ascx.cs" Inherits="IMS.UserControl.uc_PendingSalesOrderPopUp" %>


 <div class="popupMain" id="products">

    <div class="popupHead">
    Queued SOs for Generation
    <input type="submit" class="close" value="" />
    </div>
     
      <div class="bodyPop">
          
    <asp:GridView ID="gdvPendingSOs" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvPendingSOs_RowDataBound" OnPageIndexChanging="gdvPendingSOs_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvPendingSOs_RowCommand" OnRowDeleting="gdvPendingSOs_RowDeleting" OnRowEditing="gdvPendingSOs_RowEditing">
        <Columns>
           
             <asp:TemplateField HeaderText="Action">
                <ItemTemplate >
                     
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="CheckSingleCheckbox(this);"  />
                   
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Order No." Visible="true">
                <ItemTemplate>
                    <asp:Label ID="lblOrderID" runat="server" Text='<%# Eval("OrderID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

             <asp:TemplateField HeaderText="Order Date" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="lblOrderDate" runat="server" Text='<%# Eval("OrderDate") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
			
            <asp:TemplateField HeaderText="Order From" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblOrderBy" runat="server" Text='<%# Eval("FromPlace") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

			   <asp:TemplateField HeaderText="Order To">
                <ItemTemplate>
                    <asp:Label ID="lblOrderTo" runat="server" Text='<%# Eval("ToPlace") %>'  Width="160px"></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText="No. Of Products" >
                <ItemTemplate>
                    <asp:Label ID="lblProductNumber" runat="server" Text='<%# Eval("NumberProducts") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Total Quantity" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="lblQuantity" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
             <asp:TemplateField HeaderText="Status" Visible="true">
                <ItemTemplate>
                    <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
         
            <asp:TemplateField HeaderText="Delete" >
                        <ItemTemplate>
                              <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
    
     <asp:Button ID="SelectSalesOrder" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectSalesOrder_Click"  />
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


   <!--<asp:BoundField DataField="ProductName" HeaderText="Product Name"  />
           <asp:BoundField DataField="FromPlace" HeaderText="Order From"   />
              <asp:TemplateField HeaderText="OrderDetailID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblOrderDetailID" runat="server" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>-->