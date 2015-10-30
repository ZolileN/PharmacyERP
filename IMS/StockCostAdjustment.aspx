<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="StockCostAdjustment.aspx.cs" Inherits="IMS.StockCostAdjustment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">
         <tr><td>
        <h4><asp:Literal runat="server" ID="lblHeader" Text="Stock Price Manipulation" ></asp:Literal></h4>
        </td>
        <td align="right">
       
               
                     <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-primary" OnClick="btnSave_Click" Enabled="False"/>
                     <asp:Button ID="btnCancel" runat="server" Text="Cancel" CssClass="btn btn-default" OnClick="btnCancel_Click" Enabled="False"/>
                     <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click"/>
 
            </td>
        </tr>
        <tr>
                <td height="6"></td>
        </tr>
        </table>
        <hr>
           
        <table width="100%" border="0" cellspacing="5" cellpadding="10" class="formTbl">
          <tr>
                <td><label>Bar Code</label></td>
                <td><asp:TextBox ID="txtBarcode" runat="server" Text="" Enabled="false"></asp:TextBox></td>

                <td><label>UPC</label></td>
                <td><asp:TextBox ID="txtUPC" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            </tr>
            <tr>
           
            <td><label>Greenrain Code</label></td>
            <td><asp:TextBox ID="txtGreenRain" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            
            <td><label>Product Description</label></td>
            <td><asp:TextBox ID="txtProduct" runat="server" Text="" Enabled="false"></asp:TextBox></td>
          </tr>
         
          <tr>
           
            <td><label>System Quantity</label></td>
            <td><asp:TextBox ID="txtSystemQuantity" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            <td><label>Physical Quantity</label></td>
            <td><asp:TextBox ID="txtPhysicalQuantity" runat="server" Text="" Enabled="true" ></asp:TextBox></td>
                                               
          </tr>
                                 
                                 
                                  <tr>
           
            <td><label>Adjustment in Quantity</label></td>
            <td><asp:TextBox ID="txtAdjustmentQuantity" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            <td><label>Expiry Date</label></td>
            <td><asp:TextBox ID="txtExpiry" runat="server" Text="" Enabled="false"></asp:TextBox></td>
                                               
          </tr>
                                 
          <tr>
            
            <td><label>Current Cost Price</label></td>
            <td><asp:TextBox ID="txtPrevCP" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            <td><label>New Cost Price</label></td>
            <td><asp:TextBox ID="txtNewCP" runat="server" Text="" Enabled="true"></asp:TextBox></td>
                                               
          </tr>

          <tr>
            
            <td><label>Current Sale Price</label></td>
            <td><asp:TextBox ID="txtPrevSP" runat="server" Text="" Enabled="false"></asp:TextBox></td>
            <td><label>New Sale Price</label></td>
            <td><asp:TextBox ID="txtNewSP" runat="server" Text="" Enabled="true"></asp:TextBox></td>
                                               
          </tr>
                                 
                                  <tr>
           
            <td valign="top"><label>Reason For Change</label></td>
            <td colspan="3">
             <asp:TextBox ID="txtReason" runat="server" Text="" Enabled="true" CssClass="ap-main" style="width: 776px; height: 159px; resize:none;"></asp:TextBox>
             </td>
           
                                               
          </tr>
         
          <tr><td height="6"></td></tr>
          <tr>
          <td></td>
          <td>
               
           
           
          </td>
        </table>
    <script type="text/javascript">
        function changeValues()
        {
            
            var SystemQuan = document.getElementById("MainContent_txtSystemQuantity").value;
            var PhysicalQuan = document.getElementById("MainContent_txtPhysicalQuantity").value;
            
            var Result = Number(PhysicalQuan) - Number(SystemQuan);

            document.getElementById("MainContent_txtAdjustmentQuantity").value = Result;

            return;
        }
    </script>
</asp:Content>
