<%@ Page Language="C#"  MasterPageFile="~/Site.Master"  AutoEventWireup="true" CodeBehind="SearchVendorReturn.aspx.cs" Inherits="IMS.SearchVendorReturn" %>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <asp:UpdatePanel ID="up1" runat="server">
        <ContentTemplate> 
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Select Vendors</h4></td>
            <td align="right">
                <asp:Button ID="btnSelect" runat="server"  Text="Select" CssClass="btn btn-success btn-large" OnClick="btnSelect_Click" />
               
                <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" />       
          </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
    
        <table cellspacing="5" cellpadding="5" border="0" width="50%" class="formTbl">
            <tr>
                <td>
                     <asp:Label runat="server" id="lblSearchBy" CssClass="control-label">Search by Name </asp:Label>
                </td>
                <td>                <asp:TextBox runat="server" ID="txtVendorName" CssClass="form-control product"  />
               
            
                 <asp:ImageButton ID="btnSearchProduct" runat="server" Text="SearchProduct" Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" OnClick="btnSearchProduct_Click" />
                 </td>
            
                </tr>
          
  
    
   </table>

    <br />
    
    <asp:GridView ID="gdvVendor" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True"
                AutoGenerateColumns="False" OnPageIndexChanging="gdvVendor_PageIndexChanging" DataKeyNames="SuppID" >
            <Columns>
               
          <asp:TemplateField>
            <ItemTemplate>
                <asp:CheckBox ID="chkRow" runat="server" AutoPostBack="True" OnCheckedChanged="chkRow_CheckedChanged" />
            </ItemTemplate>
        </asp:TemplateField>


                <asp:BoundField DataField="SuppID" HeaderText="SuppID" Visible="False" />
                <asp:BoundField DataField="SupName" HeaderText="SupName" />
                <asp:BoundField DataField="Address" HeaderText="Address" />
                <asp:BoundField DataField="City" HeaderText="City" />
                <asp:BoundField DataField="Country" HeaderText="Country" />
                <asp:BoundField DataField="Phone" HeaderText="Phone" />
                <asp:BoundField DataField="Fax" HeaderText="Fax" />
                <asp:BoundField DataField="Email" HeaderText="Email" />
            </Columns>
            <PagerStyle CssClass = "GridPager" />
    </asp:GridView>
            <asp:HiddenField ID="hdfSupId" runat="server" Value="-1" />
      </ContentTemplate>
    </asp:UpdatePanel>
</asp:Content>

