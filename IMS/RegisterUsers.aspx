<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RegisterUsers.aspx.cs" Inherits="IMS.RegisterUsers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">
         <tr><td>
        <h4>Add Salesman</h4>
        </td>
        <td align="right">
        
        	  <asp:Button ID="btnAddEmployee" runat="server" OnClick="btnAddEmployee_Click" Text="ADD" CssClass="btn btn-primary" ValidationGroup="exSave" />
              <asp:Button ID="btnSave" runat="server"  Text="Update" CssClass="btn btn-default" ValidationGroup="exSave" OnClick="btnUpdate_Click" />
               <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnBack_Click"/>

             <asp:Button ID="btnAssociatedStore" runat="server" CssClass="btn btn-primary btn-large" Text="Associated Store" OnClick="btnAssociatedStore_Click"/>
            </td>
        </tr>
        <tr>
        	<td height="6"></td>
        </tr>

        </table>

 
         
        <hr />

    <table width="100%" border="0" cellspacing="5" cellpadding="10" class="formTbl">
          <tr>
                <td> <asp:Label runat="server" AssociatedControlID="EmpID" CssClass="col-md-2 control-label">User ID</asp:Label></td>
                <td><asp:TextBox runat="server" ID="EmpID" CssClass="form-control" Text="" autocomplete="off" />
                <asp:RequiredFieldValidator runat="server" ControlToValidate="EmpID" CssClass="text-danger" ErrorMessage="The Employee ID field is required." ValidationGroup="exSave" />
               </td>
                <td> <asp:Label runat="server" AssociatedControlID="uPwd" CssClass="col-md-2 control-label">User Password</asp:Label></td>
                <td><asp:TextBox runat="server" ID="uPwd" CssClass="form-control" TextMode="Password" Text="" autocomplete="off"  /></td>
            </tr>
             <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="ddlURole" CssClass="control-label">User Role</asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlURole" CssClass="form-control" Width="29%"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlURole" CssClass="text-danger" ErrorMessage="The user role field is required." ValidationGroup="exSave" />
                </td>
                <td>
                    <asp:Label runat="server" AssociatedControlID="ddlSysID" CssClass="control-label">Assigned System</asp:Label>
                </td>
                <td>
                    <asp:DropDownList runat="server" ID="ddlSysID" CssClass="form-control" Width="29%"/>
                <asp:RequiredFieldValidator runat="server" ControlToValidate="ddlSysID" CssClass="text-danger" ErrorMessage="The assigned system field is required." ValidationGroup="exSave" />
                </td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="fName" CssClass="control-label">First Name</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="fName" CssClass="form-control" />
                </td>
                <td>
                    <asp:Label runat="server" AssociatedControlID="lstName" CssClass="control-label">Last Name</asp:Label>
                </td>
                <td><asp:TextBox runat="server" ID="lstName" CssClass="form-control" /></td>
            </tr>
            <tr>
                <td>
                    <asp:Label runat="server" AssociatedControlID="Address" CssClass="control-label">Address</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="Address" CssClass="form-control" />
                </td>
                <td>
                    <asp:Label runat="server" AssociatedControlID="ContactNo" CssClass="control-label">Contact:#</asp:Label>
                </td>
                <td>
                    <asp:TextBox runat="server" ID="ContactNo" CssClass="form-control" />
                </td>
            </tr>
        </table>
     
      
</asp:Content>
