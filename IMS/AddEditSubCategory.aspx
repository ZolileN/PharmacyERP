<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditSubCategory.aspx.cs" Inherits="IMS.AddEditSubCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <table width="100%">
         <tr><td>
        <h4>Add Category</h4>
        </td>
        <td align="right">
        	
             <asp:Button ID="btnSaveSubCategory" runat="server"  Text="Save" CssClass="btn btn-primary" OnClientClick="return ValidateForm();" OnClick="btnSaveSubCategory_Click"  />
            <asp:Button ID="btnCancel" runat="server"  Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" />
            <asp:Button ID="btnGoBack" runat="server"  Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click" />

        </td>
        </tr>
        <tr>
        	<td height="6"></td>
        </tr>
        </table>



    <table width="" border="0" cellspacing="5" cellpadding="10" class="formTbl">
          <tr>
              <td><asp:Label runat="server" AssociatedControlID="txtSubCategoryName" CssClass=" control-label">Sub Category Name</asp:Label> </td>
              <td><asp:TextBox runat="server" ID="txtSubCategoryName" CssClass="form-control" /></td>
               
            </tr>

        <tr>
            
                 <td><asp:Label runat="server" AssociatedControlID="ddCategory" CssClass="control-label">Category Name</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ddCategory" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True"  />

                 </td>
             

          </tr>

            <tr>
            
                 <td><asp:Label runat="server" AssociatedControlID="ddDepartment" CssClass="control-label">Department Name</asp:Label></td>
                 <td><asp:DropDownList runat="server" ID="ddDepartment" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True"  />

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

             if (document.getElementById("MainContent_txtSubCategoryName").value == null || document.getElementById("MainContent_txtSubCategoryName").value == '') {
                 document.getElementById("MainContent_txtSubCategoryName").focus();
                 alert("Please enter Sub Category name");
                 return false;
             }
             if (document.getElementById("MainContent_ddDepartment").value == 'Department' || document.getElementById("MainContent_ddDepartment").value == '') {
                 alert("Please select Departement");
                 return false;
             }

             return true;

         }


</script>


</asp:Content>
