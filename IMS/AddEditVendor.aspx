<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AddEditVendor.aspx.cs" Inherits="IMS.AddEditVendor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <br />
     <div class="form-horizontal">
        <h4 id="regTitleWH" runat="server">Add Vendor</h4>
        <h4 id="EditTitleWH" visible="false" runat="server">Edit Vendor</h4>
        <hr />
        <table cellspacing="0" cellpadding="5" border="0" width="100%" class="formTbl">
            <tr>
                <td> <asp:Label runat="server" AssociatedControlID="txtVendorName" CssClass=" control-label">Vendor Name</asp:Label></td>
                <td>   <asp:TextBox runat="server" ID="txtVendorName" CssClass="form-control" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="txtEmail" CssClass=" control-label">Email</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtEmail" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="txtcity" CssClass=" control-label">City</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtcity" CssClass="form-control" /></td>
                <td><asp:Label runat="server" AssociatedControlID="txtState" CssClass=" control-label">State</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="txtState" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="txtCounty" CssClass=" control-label">Country</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="txtCounty" CssClass="form-control" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="txtaddress" CssClass=" control-label">Address</asp:Label></td>
                <td>  <asp:TextBox runat="server" ID="txtaddress" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="txtphone" CssClass=" control-label">Phone</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtphone" CssClass="form-control" /></td>
                <td> <asp:Label runat="server" AssociatedControlID="txtfax" CssClass=" control-label">Fax</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="txtfax" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td><asp:Label runat="server" AssociatedControlID="txtmobile" CssClass=" control-label">Mobile</asp:Label></td>
                <td> <asp:TextBox runat="server" ID="txtmobile" CssClass="form-control" /></td>
                <td><asp:Label runat="server" AssociatedControlID="txtpager" CssClass=" control-label">Pager</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtpager" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td> <asp:Label runat="server" AssociatedControlID="txtpager" CssClass=" control-label">Contact Person</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtConPerson" CssClass="form-control" /></td>
                <td>  <asp:Label runat="server" AssociatedControlID="txtDiscount" CssClass=" control-label">Discount</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtDiscount" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td>     <asp:Label runat="server" AssociatedControlID="txtCredit" CssClass="control-label">Credit</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtCredit" CssClass="form-control" /></td>
                <td><asp:Label runat="server" AssociatedControlID="txtPincode" CssClass=" control-label">Pincode</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtPincode" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td> <asp:Label runat="server" AssociatedControlID="txtID" Visible="false" CssClass=" control-label">id</asp:Label></td>
                <td><asp:TextBox runat="server" ID="txtID" Visible="false" CssClass="form-control" /></td>
                <td></td>
                <td></td>
            </tr>
            <tr>
                <td></td>
                <td colspan="100%">
              <asp:Button ID="btnCreateVendor" runat="server" OnClick="btnCreateVendor_Click"  Text="ADD" CssClass="btn btn-default" OnClientClick="return ValidateForm();" ValidationGroup="exSave"/>
                 <asp:Button ID="btnUpdateVendor" runat="server" OnClick="btnUpdateVendor_Click"  Text="UPDATE" CssClass="btn btn-default" Visible="false"/>
                <asp:Button ID="btnCancelVendor" runat="server" OnClick="btnCancelVendor_Click" Text="CANCEL" CssClass="btn btn-default" />
                <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnGoBack_Click"/>
                </td>
            </tr>
            </table>
    </div>


    <script type="text/javascript">
        function ValidateForm() {

            if (document.getElementById("MainContent_txtVendorName").value == null || document.getElementById("MainContent_txtVendorName").value == '') {
                alert("Please enter Vendor name");
                return false;
            }
            if (document.getElementById("MainContent_txtcity").value == null || document.getElementById("MainContent_txtcity").value == '') {
                alert("Please enter City");
                return false;
            }
            if (document.getElementById("MainContent_txtCounty").value == null || document.getElementById("MainContent_txtCounty").value == '') {
                alert("Please enter Country");
                return false;
            }
            return true;
 
        }


</script>
</asp:Content>
