<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_ItemPurchaseDisplay_byDate.aspx.cs" Inherits="IMS.rpt_ItemPurchaseDisplay_byDate" EnableEventValidation="false" %>

<%@ Register Assembly="CrystalDecisions.Web, Version=13.0.2000.0, Culture=neutral, PublicKeyToken=692fbea5521e1304" Namespace="CrystalDecisions.Web" TagPrefix="CR" %>
<%@ Register assembly="Microsoft.ReportViewer.WebForms, Version=11.0.0.0, Culture=neutral, PublicKeyToken=89845dcd8080cc91" namespace="Microsoft.Reporting.WebForms" tagprefix="rsweb" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <!--<script type="text/javascript">
    function Print() {
        var dvReport = document.getElementById("dvReport");
        var frame1 = dvReport.getElementsByTagName("iframe")[0];
        if (navigator.appName.indexOf("Internet Explorer") != -1) {
            frame1.name = frame1.id;
            window.frames[frame1.id].focus();
            window.frames[frame1.id].print();
        }
        else {
            var frameDoc = frame1.contentWindow ? frame1.contentWindow : frame1.contentDocument.document ? frame1.contentDocument.document : frame1.contentDocument;
            frameDoc.print();
        }
    }
</script>-->
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <table width="100%">

        <tbody><tr>
        	<td> <h4>Item Purchased Detail Report</h4></td>
            <td align="right">
                <asp:Button ID="btnPrint" runat="server" PostBackUrl="~/ReportPrinting.aspx" CssClass="btn btn-primary btn-large no-print" Text="Print" />
                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-info btn-large no-print" Text="Export" Visible="false"  OnClick="btnExport_Click"/>
                <asp:Button ID="btnGoBack" runat="server" PostBackUrl="~/rpt_ItemPurchase_Selection.aspx" CssClass="btn btn-default btn-large no-print" Text="Go Back" />
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>

   

    

    <table cellpadding="0" cellspacing="3" border="0" width="100%" class="table table-striped table-bordered table-condensed">
        <tr>
            <td><h4>Selection Criteria</h4></td>
        </tr>        
        <tr>
                <td width="12.5%"><label>Date From:</label></td>
                <td width="12.5%"><asp:Label ID="lblDateFrom" runat="server" Text="---"></asp:Label></td>
                <td width="12.5%"><label>Date To:</label></td>
                <td width="12.5%"><asp:Label ID="lblDateTo" runat="server" Text="---"></asp:Label></td>
                <td width="12.5%"><label>Customers:</label></td>
                <td width="12.5%"><asp:Label ID="lblCustomers" runat="server" Text="---"></asp:Label></td>
                <td width="12.5%"><label>Barter Customer:</label></td>
                <td width="12.5%"><asp:Label ID="lblBarterCustomer" runat="server" Text="---"></asp:Label></td>
        </tr>
        <tr>
                <td><label>Department:</label></td>
                <td><asp:Label ID="lblDepartment" runat="server" Text="---"></asp:Label></td>
                <td><label>Category:</label></td>
                <td><asp:Label ID="lblCategory" runat="server" Text="---"></asp:Label></td>
                <td><label>Sub Category:</label></td>
                <td><asp:Label ID="lblSubCategory" runat="server" Text="---"></asp:Label></td>
                <td><label>Product:</label></td>
                <td><asp:Label ID="lblProduct" runat="server" Text="---"></asp:Label></td>
        </tr>
     </table>

    <div id="dvReport">
    <CR:CrystalReportViewer ID="CrystalReportViewer1" Visible="true" runat="server" AutoDataBind="True" Height="1202px" Width="1104px" PrintMode="Pdf" HasPrintButton="False" HasExportButton="False"/>
    </div>

       

       <asp:GridView ID="gdvSalesSummary" runat="server" Visible="false" Width="100%" 
        AutoGenerateColumns="false" OnRowDataBound="gdvSalesSummary_RowDataBound" BorderWidth="0px">

        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table style="border-collapse: collapse;" id="MainContent_StockDisplayGrid" rules="all" cellspacing="0" class="table table-striped table-bordered table-condensed">

                        <tbody>
                            <tr>
                                <td colspan="15">
                                    <h4 class="fl-l">
                                        
                                        PO# &nbsp; <asp:Label ID="lblSO" runat="server" Text='<%# Eval("OrderID") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp;
                                        - &nbsp;&nbsp;<asp:Label ID="lblSystemName" runat="server" Text='<%# Eval("SupName") %>'></asp:Label>&nbsp;&nbsp;&nbsp;&nbsp
                                        - &nbsp;&nbsp;<asp:Label ID="lblSODate" runat="server" Text='<%# Convert.ToDateTime(Eval("OrderDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                    </h4>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                <asp:GridView ID="gvMAinGrid" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" 
                                 AutoGenerateColumns="false">
                                     <Columns>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblItemDesc" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                         <asp:TemplateField HeaderText="ExpiryDate">
                                            <ItemTemplate>
                                                <asp:Label ID="lblExpiry" runat="server" Text='<%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd/MM/yyyy") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Ordered Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSendQuan" runat="server" Text='<%# Eval("SendQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Bonus Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblBonus" runat="server" Text='<%# Eval("BonusQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Purchased Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoldQuan" runat="server" Text='<%# Eval("RecievedQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Purchased Bonus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSoldBonus" runat="server" Text='<%# Eval("RecievedBonusQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOPrice" runat="server" Text='<%# Eval("TotalCostPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           
                                    </Columns>
                                </asp:GridView>
                                </td>
                            </tr>

                            <tr>
                                <td colspan="10">
                                    <h4>Total</h4>
                                </td>
                            </tr>

                            <tr>
                                <td>
                                <asp:GridView ID="gdvTotal" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" 
                                 AutoGenerateColumns="false">
                                     <Columns>
                                        <asp:TemplateField HeaderText="" Visible="false">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="6%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl1" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="" HeaderStyle-Width="31%">
                                            <ItemTemplate>
                                                <asp:Label ID="lbl2" runat="server" Text=""></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Ordered Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalSend" runat="server" Text='<%# Eval("TotalSendQaun") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Bonus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalBonus" runat="server" Text='<%# Eval("TotalBonusQuan") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Purchased Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalSold" runat="server" Text='<%# Eval("TotalSoldQuan") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Toal Purchased Bonus">
                                            <ItemTemplate>
                                                <asp:Label ID="lblTotalSoldBonus" runat="server" Text='<%# Eval("TotalSoldBonus") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSOTotalPrice" runat="server" Text='<%# Eval("TotalCostPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           
                                    </Columns>
                                </asp:GridView>
                                </td>
                            </tr>

                        </tbody>
                    </table>
                </ItemTemplate>
            </asp:TemplateField>
        </Columns>

    </asp:GridView>
</asp:Content>
