<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rpt_ucCustomers.ascx.cs" Inherits="IMS.UserControl.rpt_ucCustomers" %>
<div style="margin-top: -144px; display: block;" class="popupMain" id="customers">

        <div class="popupHead">Customers List
            <a href="rpt_SalesSummary_Selection.aspx" class="close"></a>
        </div>

        <div class="bodyPop">
            <asp:GridView ID="gdvCustomers" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvCustomers_RowDataBound" OnPageIndexChanging="gdvCustomers_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvCustomers_RowCommand" OnRowDeleting="gdvCustomers_RowDeleting" OnRowEditing="gdvCustomers_RowEditing">
        <Columns>
           
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate >
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="CheckSingleCheckbox(this);"  />
                </ItemTemplate>
            </asp:TemplateField>
             
            <asp:TemplateField HeaderText="Customer Name">
                <ItemTemplate>
                    <asp:Label ID="lblCustomer" runat="server" Text='<%# Eval("SystemName") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Description">
                <ItemTemplate>
                    <asp:Label ID="lblDescription" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label ID="lblAddress" runat="server" Text='<%# Eval("SystemAddress") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Phone No.">
                <ItemTemplate>
                    <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("SystemPhone") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lblSysID" runat="server" Text='<%# Eval("SystemID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
            <asp:Button ID="btnSelectCustomer" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="btnSelectCustomer_Click" />
            
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
</div>
