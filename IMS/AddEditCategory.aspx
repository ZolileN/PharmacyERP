<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditCategory.aspx.cs" Inherits="IMS.AddEditCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


     <table width="100%">
         <tr><td>
        <h4 ><asp:Label ID="lblHeading" runat="server" Text="Add Category"></asp:Label> </h4>
        </td>
        <td align="right">
        	
            <asp:Button ID="btnSaveCategory" runat="server"  Text="Save" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" OnClick="btnSaveCategory_Click" />
            <asp:Button ID="btnCancel" runat="server"  Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" />
            <asp:Button ID="btnGoBack" runat="server"  Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click" />

             
        </td>
        </tr>
        <tr>
        	<td height="6"></td>
        </tr>
        </table>
        <hr>


    <table width="" border="0" cellspacing="5" cellpadding="10" class="formTbl">
          <tr>
              <td><asp:Label runat="server" AssociatedControlID="CategoryName" CssClass=" control-label">Category Name</asp:Label> </td>
              <td><asp:TextBox runat="server" ID="CategoryName" CssClass="form-control" /></td>
               
            </tr>
            <tr>
            
                 <td><asp:Label runat="server" AssociatedControlID="CategoryDepartment" CssClass="control-label">Department Name</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="CategoryDepartment" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True"  />

                 </td>
             
          </tr>
          <tr><td height="6"></td></tr>
          <tr>
          <td></td>
          <td>
          	
           
            
          </td>
        </table>

    <script type="text/javascript">
        function ValidateForm() {

            if (document.getElementById("MainContent_CategoryName").value == null || document.getElementById("MainContent_CategoryName").value == '') {
                alert("Please enter Category name");
                return false;
            }
              
            return true;

        }
        </script>

</asp:Content>
