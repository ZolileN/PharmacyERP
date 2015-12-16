<%@ Page Language="C#" MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="rpt_HaadMedicinesList.aspx.cs" Inherits="IMS.rpt_HaadMedicinesList" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">

        <tbody><tr>
        	<td> <h4>HAAD List Report</h4></td>
            <td align="right">
            <asp:Button ID="btnCreateReport" runat="server" CssClass="btn btn-success btn-default" Text="CREATE REPORT" OnClick="btnCreateReport_Click" />
            <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    <table class="formTbl" width="100%">
          
        <tr>
            <td>

                <asp:Label ID="Label1" runat="server" Text="Category"></asp:Label>
            </td>
            <td>
                <asp:DropDownList ID="drpCat" runat="server" AutoPostBack="True" OnSelectedIndexChanged="drpCat_SelectedIndexChanged"></asp:DropDownList>

            </td>
            <td>
                 <asp:Label ID="Label2" runat="server" Text="Sub Category"></asp:Label>
            </td>
            <td>

                 <asp:DropDownList ID="DrpSubCat" runat="server"></asp:DropDownList>
            </td>
        </tr>
        </table>

</asp:Content>