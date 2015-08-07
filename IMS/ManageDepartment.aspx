<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageDepartment.aspx.cs" Inherits="IMS.ManageDepartment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     
    <%--<h4>Manage Department</h4>--%>
    <table width="100%">

        <tbody><tr>
        <td> <h4>Manage Department</h4> </td>
            <td align="right">
            	 
                <asp:Button ID="btnAddDepartment" runat="server"  Text=" + Add Department" CssClass="btn btn-success btn-large" OnClick="btnAddDepartment_Click"  />
                <asp:Button ID="btnGoBack" runat="server"  Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click"  />
         
               		 <%--<input type="submit" value=" + Add Department" class="btn btn-success btn-large">--%>
                     <%--<a href="imsMain.html" class="btn btn-default btn-large">Go Back</a>--%>
               
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>

     
        <hr />
    <asp:GridView ID="DepDisplayGrid" runat="server" width="100%" AllowPaging="True" PageSize="10" CssClass="table table-striped table-bordered table-condensed DeptTbl"
                AutoGenerateColumns="false" OnPageIndexChanging="DepDisplayGrid_PageIndexChanging"   onrowcancelingedit="DepDisplayGrid_RowCancelingEdit" ShowFooter="true"
            onrowcommand="DepDisplayGrid_RowCommand"  onrowdeleting="DepDisplayGrid_RowDeleting" onrowediting="DepDisplayGrid_RowEditing" OnRowDataBound ="DepDisplayGrid_RowDataBound">
                <Columns>
                    <asp:TemplateField HeaderText="Department ID">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_ID" runat="server"  Text='<%# Eval("DepId") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <%--<FooterTemplate>
                            <asp:Label ID="lblAdd" runat="server"></asp:Label>
                        </FooterTemplate>--%>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <%--<EditItemTemplate>

                            <asp:TextBox ID="txtname" runat="server"  Text='<%#Eval("Name") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                        
                        <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddname" runat="server"></asp:TextBox>
                        </FooterTemplate>--%>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Code">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Code" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                        </ItemTemplate>
                         <%--<EditItemTemplate>
                            <asp:TextBox ID="txtCode" runat="server" Text='<%#Eval("Code") %>'></asp:TextBox>
                        </EditItemTemplate>--%>
                        
                        <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddCode" runat="server"></asp:TextBox>
                        </FooterTemplate>--%>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="btn btn-default edit-btn"  Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton ID="btnDelete" Text="Delete" CssClass="btn btn-default del-btn"  runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>

                        <EditItemTemplate>

                            <%--<asp:LinkButton ID="btnUpdate" Text="Update" CssClass="btn btn-primary btn-xs"  runat="server" CommandName="UpdateDep" />--%>
                            <asp:LinkButton ID="btnCancel" Text="Cancel" CssClass="btn btn-default btn-xs"  runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                        
                        <%--<FooterTemplate>
                            <asp:Button ID="btnAddRecord" runat="server" Text="Add" CssClass="btn btn-default btn-sm"  OnClientClick="return ValidateForm();" CommandName="Add"></asp:Button>
                        </FooterTemplate>--%>
                    </asp:TemplateField>
                </Columns>
                <PagerStyle CssClass = "GridPager" />
            </asp:GridView>
    <%--<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>--%>

    <script type="text/javascript">
        function ValidateForm() {

            if (document.getElementById("MainContent_DepDisplayGrid_txtAddname").value == null || document.getElementById("MainContent_DepDisplayGrid_txtAddname").value == '') {
                alert("Please enter Department name");
                return false;
            }
            if (document.getElementById("MainContent_DepDisplayGrid_txtAddCode").value == null || document.getElementById("MainContent_DepDisplayGrid_txtAddCode").value == '') {
                alert("Please enter Code");
                return false;
            }
             
            return true;

        }


</script>
</asp:Content>
