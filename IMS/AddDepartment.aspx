<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddDepartment.aspx.cs" Inherits="IMS.AddDepartment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

     <table width="100%">
         <tr><td>
        <h4>Add Department</h4>
        </td>
        <td align="right">
        
        	<asp:Button ID="btnSaveDepartment" runat="server"  Text="Save" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" OnClick="btnSaveDepartment_Click" />
            <asp:Button ID="btnCancel" runat="server"  Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" />
            <asp:Button ID="btnGoBack" runat="server"  Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click" />

                     <%--<input value="Save" class="btn btn-primary" type="submit" onClick="window.location.href = 'ManageDepartment.html'">--%>
                     <%--<input value="Cancel" class="btn btn-default" type="submit" onClick="window.location.href = 'ManageDepartment.html'">--%>
                     <%--<input type="submit" value="Go Back" class="btn btn-default btn-large" onClick="window.location.href = 'ManageDepartment.html'">--%>

            </td>
        </tr>
        <tr>
        	<td height="6"></td>
        </tr>
        </table>

    <hr>
         
            
            
        <table width="" border="0" cellspacing="5" cellpadding="10" class="formTbl">
          <tr>
               <td><asp:Label runat="server" AssociatedControlID="DepartmentName" CssClass=" control-label">Department Name</asp:Label> </td>
                <td><asp:TextBox runat="server" ID="DepartmentName" CssClass="form-control" /></td>
             
            </tr>
            <tr>
            
            <td><asp:Label runat="server" AssociatedControlID="DepartmentCode" CssClass=" control-label">Department Code</asp:Label> </td>
            <td><asp:TextBox runat="server" ID="DepartmentCode" CssClass="form-control" /></td>
          </tr>
          <tr><td height="6"></td></tr>
          <tr>
          <td></td>
          <td>
          	
            
            
          </td>
        </table>

    <script type="text/javascript">

        function ValidateForm() {

            if (document.getElementById("MainContent_DepartmentName").value == null || document.getElementById("MainContent_DepartmentName").value == '') {
                alert("Please enter Department name");
                return false;
            }
            if (document.getElementById("MainContent_DepartmentCode").value == null || document.getElementById("MainContent_DepartmentCode").value == '') {
                alert("Please enter Code");
                return false;
            }
             
            return true;

        }
       
     </script>

</asp:Content>
