<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Print_Request_WH.aspx.cs" Inherits="IMS.Print_Request_WH" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

     <style>
        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }
		table.table-striped tr td, table.table-striped tr th{
			height:36px !important;
			}
			
		table.table-striped tr th{
			text-align:left !important; padding-left:10px !important;
			}
			
		.prodescription{
			margin-left:7px;
			}
			
		.m0px{
			margin:0px auto;
			display:block;
			text-align:center !important;
			}
		
		.main{
			font-family:"Calibri", Arial, Helvetica, sans-serif;
			font-size:16px;
			margin:0px auto;
			width:747px;
			position:relative;
			}
			
		.invo-tbl{
			margin-bottom:5px;
			border-top:1px solid #000;
			border-left:1px solid #000;
			}
			
		.invo-tbl tr td{
			border-right:1px solid #000;
			border-bottom:1px solid #000;
			}
			
		.underline{
			width:168px;
			border-bottom:1px solid #000;
			text-align:center;
			display:block;
			float:right;
			}
		h1.main-h{
			text-align:;
			width:747px;
			margin:5px 0 0 0;
			}
		hr{
			margin:0px 0 15px 0;
			}
			
		.bold{
			font-weight:bold;
			}
			
		.datarow{
			
			}
			
		.datarow td{
			border-bottom:0px !important;
			}
		
		.fr{
			float:right;
			font-size:18px;
			}
		.fr td{
			padding:3px;
			}
			
		.date{
			display:block;

			}
		.signs{
			font-size:14px;
			}
		.arabic{
			font-family:Simplified Arabic Fixed;
			}
		.scHead{
			font-size:34px;
			font-weight:bold;
			display:block;
			margin-bottom:6px;
			color:#00b0f0;
			}
		.afH{font-size:16px;color:#a5a5a5;}
		.flt{float:left;font-weight:bold;}
		.frt{float:right;font-weight:bold;}
		.clear{clear:both}
		.instructions{
			border:1px solid #000;
			}
	
		
    </style>
    
    
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead"> Transfer Request</h4></td>
            <td align="right">
             
                <asp:Button ID="btnPrint" runat="server" Text="Print" CssClass="btn btn-primary btn-large no-print" OnClientClick="window.print();"/>
                <asp:Button ID="btnExport" runat="server" Text="Export" CssClass="btn btn-info btn-large no-print"  Visible="false" />
                <asp:Button ID="btnGoBack" runat="server" Text="Go Back" CssClass="btn btn-default btn-large no-print" OnClick="btnGoBack_Click" />
 
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>


    <hr>
           <table cellpadding="5" width="100%" cellspacing="0">
                    <tbody>
                        <tr>
                            <td valign="top" class="invoLeftHighlighted">

                                <h4>ORDER FROM:</h4>
                                
                                <asp:Label ID="lblFROMSystemName"   runat="server"></asp:Label>
                                <br>
                                <asp:Label ID="lblFROMSystemAddress"   runat="server"></asp:Label>
                                <br>
                                Phone:<asp:Label ID="lblFROMSystemPhone" runat="server"></asp:Label><br>
                                Fax:<asp:Label ID="lblFROMSystemEmail"  runat="server"></asp:Label>

                            </td>
                            <td width="2%"></td>

                            <td valign="top" class="invoLeftHighlighted">

                                <h4>ORDER TO:</h4>
                                <asp:Label ID="lblToSystemName"  runat="server"></asp:Label>
                                <br>
                                <asp:Label ID="lblToSystemAddress"  runat="server"></asp:Label>
                                <br>
                                Phone:<asp:Label ID="lblToSystemPhone" runat="server"></asp:Label><br>
                                Fax:<asp:Label ID="lblToSystemEmail"  runat="server"></asp:Label>


                            </td>
                        </tr>
                    </tbody>
                </table>


                <br>


                <asp:GridView ID="dgvTransferDisplay" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                    AutoGenerateColumns="false">
                    <Columns>

                        <%--<asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="0px">
                            <ItemTemplate>
                                <asp:Label ID="lblTransferDetailID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferDetailID") %>' Width="0px" Visible="false"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="0px" HorizontalAlign="Left" />

                        </asp:TemplateField>--%>

                        <asp:TemplateField HeaderText="Requested Product" HeaderStyle-Width="190px">
                            <ItemTemplate>
                                <asp:Label ID="lblProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>' Width="250px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="250px" HorizontalAlign="Left" />

                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Requested<br>Quantity" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Requested<br>Bonus Quantity" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedBonusQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Unit<br>Cost Price" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedQtyCP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Total<br>Cost Price" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedQtyTotalCP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalCostPrice") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedQtyDiscount" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Discount") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                         <asp:TemplateField HeaderText="Discounted<br>Cost Price" HeaderStyle-Width="50px">
                            <ItemTemplate>
                                <asp:Label ID="TransferedQtyDiscountedCP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DiscountCostPrice") %>' Width="50px"></asp:Label>
                            </ItemTemplate>
                            <ItemStyle Width="150px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                    </Columns>
                </asp:GridView>



                <table cellpadding="4" cellspacing="0" width="100%" align="right">
                    <tbody>

                        <tr>

                            <td>&nbsp;</td>
                            <td>&nbsp;</td>
                            <td><span class="signs">Sign By:</span></td>
                            <td><span class="underline">&nbsp;</span></td>

                            <td class="signs">Company Stamp:</td>
                            <td><span class="underline">&nbsp;</span></td>
                        </tr>

                        <tr>
                            <td colspan="100%" height="10">&nbsp;</td>
                        </tr>

                        <tr>

                            <td class="signs">Prepared By:</td>
                            <td><span class="underline">&nbsp;</span></td>
                            <td class="signs">Checked By:</td>
                            <td><span class="underline">&nbsp;</span></td>
                            <td class="signs">Received By:</td>
                            <td><span class="underline">&nbsp;</span></td>
                        </tr>

                    </tbody>
                </table>

                <div class="clear"></div>
                <br>
            </div>
</asp:Content>
