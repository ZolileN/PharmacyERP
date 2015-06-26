<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="rpt_ucSubCategory.ascx.cs" Inherits="IMS.UserControl.rpt_ucSubCategory" %>
<div style="margin-top: -144px; display: block;" class="popupMain" id="department">

        <div class="popupHead">SubCategory List
            <a href="#" class="close"></a>
        </div>
        <div class="bodyPop">
        	<asp:GridView ID="gdvDepartment" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnRowDataBound="gdvDepartment_RowDataBound" OnPageIndexChanging="gdvDepartment_PageIndexChanging" ShowFooter="true"
        OnRowCommand="gdvDepartment_RowCommand" OnRowDeleting="gdvDepartment_RowDeleting" OnRowEditing="gdvDepartment_RowEditing">
        <Columns>
           
            <asp:TemplateField HeaderText="Action">
                <ItemTemplate >
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="CheckSingleCheckbox(this);"  />
                </ItemTemplate>
            </asp:TemplateField>
             
             <asp:TemplateField HeaderText="SubCategory Name">
                <ItemTemplate>
                    <asp:Label ID="lblSubCatName" runat="server" Text='<%# Eval("SubCategory") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Category Name">
                <ItemTemplate>
                    <asp:Label ID="lblCatName" runat="server" Text='<%# Eval("Category") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Department Name">
                <ItemTemplate>
                    <asp:Label ID="lblDeptName" runat="server" Text='<%# Eval("Department") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            
            <asp:TemplateField HeaderText="ID" Visible="false">
                <ItemTemplate>
                    <asp:Label ID="lbSubCatID" runat="server" Text='<%# Eval("Sub_CatID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


            </asp:GridView>
            <asp:Button ID="btnSelectDepartment" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="btnSelectDepartment_Click" />
            
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