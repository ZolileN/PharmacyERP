<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="rpt_ItemSoldDisplay.aspx.cs" Inherits="IMS.rpt_ItemSoldDisplay" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

  

    <table width="100%">

        <tbody><tr>
        	<td> <h4>Item Sold Report</h4></td>
            <td align="right">
                <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print();" CssClass="btn btn-primary btn-large no-print" Text="Print" />
                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-info btn-large no-print" Text="Export" />
                <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" CssClass="btn btn-default btn-large no-print" Text="Go Back" />
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>


     <asp:GridView ID="gvMAinGrid" runat="server" Width="100%" CssClass="table table-striped table-bordered table-condensed" 
                                 AutoGenerateColumns="false">
                                    <Columns>
                                        <asp:TemplateField HeaderText="SO#">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("OrderID") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SO Date">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("OrderDate") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="SalesMan">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("SalesMan") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Pharmacy">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("SystemName") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Item Description">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Send Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("SendQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                          <asp:TemplateField HeaderText="Bonus Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("BonusQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sold Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("RecievedQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Sold Bonus Quantity">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("RecievedBonusQuantity") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Unit Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("UnitCostPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>

                                        <asp:TemplateField HeaderText="Total Price">
                                            <ItemTemplate>
                                                <asp:Label ID="lblSO" runat="server" Text='<%# Eval("TotalCostPrice") %>'></asp:Label>
                                            </ItemTemplate>
                                        </asp:TemplateField>
                                           
                                    </Columns>
                                </asp:GridView>
     
     
</asp:Content>
