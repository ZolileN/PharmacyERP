<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="WHResquestInvoice.aspx.cs" Inherits="IMS.WHResquestInvoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 273px;
        }

        .auto-style2 {
            font-size: large;
        }

        .auto-style3 {
            width: 916px;
        }

        .auto-style4 {
            width: 758px;
        }

        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="form-horizontal" runat="server" id="MAINDIV" visible="true">

        <table style="width: 100%;" align="center" border="0">
            <tr>
                <td class="auto-style2">
                    <h3><strong>Sales Order Packing List</strong></h3>
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>
            <tr>
                <td>
                    <br />
                </td>
                <td></td>
                <td></td>
                <td></td>

            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="PO_Numberlbl" CssClass="col-md-2 control-label" Width="200px">P.O Number : </asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="PO_Number" CssClass="col-md-2 control-label" Text="---" Width="300px"></asp:Label></td>

            </tr>
            <tr>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="auto-style1">
                    <asp:Label runat="server" ID="PO_Datelbl" CssClass="col-md-2 control-label" Width="200px">P.O Date : </asp:Label></td>
                <td>
                    <asp:Label runat="server" ID="PO_Date" CssClass="col-md-2 control-label" Text="---" Width="300px"></asp:Label></td>
            </tr>

            <tr>
                <td class="auto-style1">
                    <br />
                </td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td></td>
            </tr>

            <tr>
                <td class="text-left">
                    <h4>ORDER BY : </h4>
                    <asp:Label runat="server" ID="PO_FromName" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="text-left">
                    <h4>ORDER FROM : </h4>
                    <asp:Label runat="server" ID="PO_ToName" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td class="text-left">
                    <asp:Label runat="server" ID="PO_FromAddress" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td class="text-left">
                    <asp:Label runat="server" ID="PO_ToAddress" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label></td>
            </tr>
            <tr>
                <td class="auto-style1">
                    <asp:Label runat="server" ID="PO_FromPhone" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label></td>
                <td></td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="PO_ToPhone" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style1"></td>
                <td></td>
                <td>&nbsp;</td>
                <td>
                    <asp:Label runat="server" ID="PO_ToEmail" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label>
                </td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td></td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            </tr>
            <tr>
                <td class="auto-style1">&nbsp;</td>
                <td></td>
                <td>&nbsp;</td>
                <td></td>
            </tr>
        </table>
        <br />
    </div>

    <div class="form-horizontal">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
            AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowDataBound="StockDisplayGrid_RowDataBound">
            <Columns>
                 <asp:TemplateField HeaderText=" Ordered Product Name : Strength : Form : Pack Size" HeaderStyle-Width="500" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName2" padding-right="5px" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                            <asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'  ></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2"  runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px" ></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px" ></asp:Label>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="Ordered Product" Visible="false" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductName") %>' Width="330px"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="330px" HorizontalAlign="Left" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PackageSize") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("strength") %>'  Width="150px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="160px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                <asp:TemplateField HeaderText="Ordered Quantity" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Unit Sale Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitSale") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total Sale Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalSalePrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <table style="width: 100%;" id="TotalCostDiv" runat="server" visible="true">
            <tr>
                <td>
                    <br />
                    <br />
                </td>
                <td class="auto-style3">&nbsp;</td>
                <td>
                    <asp:Label ID="lblTotalCostALL" runat="server" Style="font-weight: 700"></asp:Label></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><strong>Sign:</strong> ________________</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><strong>Company Stamp:</strong> ____________</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><br /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><strong>Prepared By:</strong> ____________</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><br /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><strong>Checked By:</strong> ____________</td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><br /></td>
            </tr>
            <tr>
                <td>&nbsp;</td>
                <td class="auto-style3">&nbsp;</td>
                <td><strong>Received By:</strong> ____________</td>
            </tr>
        </table>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print();" Text="PRINT" CssClass="btn btn-large no-print" Visible="true" />
                <asp:Button ID="btnFax" runat="server" OnClick="btnFax_Click" Text="BACK" CssClass="btn btn-large no-print" />
                <asp:Button ID="btnEmail" runat="server" OnClick="btnEmail_Click" Text="IMPORT TO EXCEL" CssClass="btn btn-large no-print" Visible="true" />
            </div>
        </div>
    </div>


</asp:Content>
