<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Select_Salesman.ascx.cs" Inherits="IMS_WHReports.UserControl.uc_Select_Salesman" %>
<div class="popupMain" id="vendors">
<div class="popupHead">
    Salesman List
    <input type="submit" class="close" value="" />
    </div>
     
      <div class="bodyPop">
          
    <asp:GridView ID="gdvSalesman" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
        AutoGenerateColumns="false" OnPageIndexChanging="gdvSalesman_PageIndexChanging" ShowFooter="true">
        <Columns>
           
             <asp:TemplateField HeaderText="Action">
                <ItemTemplate >
                     
                    <asp:CheckBox ID="chkCtrl" runat="server" onClick="CheckSingleCheckbox(this);"  />
                   
                </ItemTemplate>
            </asp:TemplateField>
            
             <asp:TemplateField HeaderText="Salesman Name">
                <ItemTemplate>
                    <asp:Label ID="lblName" runat="server" Text='<%# Eval("name") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>

            <asp:TemplateField HeaderText="Address">
                <ItemTemplate>
                    <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            
            <asp:TemplateField HeaderText="Phone">
                <ItemTemplate>
                    <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("Contact") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
            <asp:TemplateField HeaderText="Email">
                <ItemTemplate>
                    <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("U_Email") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
            <asp:TemplateField HeaderText="ID" >
                <ItemTemplate>
                    <asp:Label ID="lblSalemanID" runat="server" Text='<%# Eval("UserID") %>'></asp:Label>
                </ItemTemplate>
            </asp:TemplateField>
           
        </Columns>
        <PagerStyle CssClass="GridPager" />


    </asp:GridView>
     
     <asp:Button ID="SelectSalesman" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Select" OnClick="SelectSalesman_Click" />
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