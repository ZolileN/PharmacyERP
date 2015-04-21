<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PO_GENERATE.aspx.cs" Inherits="IMS.PO_GENERATE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style>
		.main{
			font-family:"Calibri", Arial, Helvetica, sans-serif;
			font-size:16px;
			margin:0px auto;
			width:860px;
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
			width:90px;
			border-bottom:1px solid #000;
			text-align:center;
			display:block;
			float:right;
			}
		h1.main-h{
			text-align:center;

			width:747px;
			margin:5px 0;
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

        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }
	</style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main" id="MAINDIV" runat="server">
        <h1 class="main-h">PURCHASE ORDER</h1>
        <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td> <asp:Label runat="server" ID="PO_Numberlbl" CssClass="col-md-2 control-label" Width="200px">P.O Number : </asp:Label> <asp:Label runat="server" ID="PO_Number" CssClass="col-md-2 control-label" Text="---" Width="100px"></asp:Label></td></td>
                <td align="right"><asp:Label runat="server" ID="PO_Datelbl" CssClass="col-md-2 control-label" Width="200px">P.O Date : </asp:Label> <span class="date"><asp:Label runat="server" ID="PO_Date" CssClass="col-md-2 control-label" Text="---" Width="300px"></asp:Label></span></td>
               
        </tr>
        </table>
        <br /><br />
         <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td width="403" valign="top"><strong>ORDER BY:</strong><br />
           	    <asp:Label runat="server" ID="PO_FromName" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
           	    <asp:Label runat="server" ID="PO_FromAddress" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
       	        <asp:Label runat="server" ID="PO_FromPhone" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br /></td>
                <td width="322" valign="top" ><strong>ORDER FROM:</strong><br />
           	    <asp:Label runat="server" ID="PO_ToName" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
           	    <asp:Label runat="server" ID="PO_ToAddress" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
       	        <asp:Label runat="server" ID="PO_ToPhone" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
                <asp:Label runat="server" ID="PO_ToEmail" CssClass="col-md-2 control-label" Text="---" Width="100%"></asp:Label><br />
              </td>
            </tr>
        </table>
        <br /><br /><br />


   
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
            AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowDataBound="StockDisplayGrid_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Ordered Product" Visible="true" HeaderStyle-Width="350px">
                    <ItemTemplate>
                        <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>' Width="350px"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="350px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ordered Quantity" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ordered Bonus" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Label ID="lblBonus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Unit Cost Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total Cost Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalCostPrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

            </Columns>
        </asp:GridView>

        <table cellpadding="4" cellspacing="0" align="right">
        	<tr>
            	<td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>


	            <td class="signs">Total Cost:</td><td><span class="underline"><asp:Label ID="lblTotalCostALL" runat="server" Style="font-weight: 700"></asp:Label></span></td>
            </tr>
            <tr>
            	<td colspan="100%" height="5"></td>
            </tr>
            
            <tr>
              	<td >&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
                <td>&nbsp;</td>
            
                <td class="signs">Sign:</td><td><span class="underline">&nbsp;</span></td>
            </tr>
            
            <tr>
            	<td colspan="100%" height="5"></td>
            </tr>
            
            <tr>
            <td class="signs">Prepared By:</td><td><span class="underline">&nbsp;</span></td>
            <td class="signs">Checked By:</td><td><span class="underline">&nbsp;</span></td>
            <td class="signs">Received By:</td><td><span class="underline">&nbsp;</span></td>
                <td class="signs">Company Stamp:</td><td><span class="underline">&nbsp;</span></td>
            </tr>
           
        </table>
        <br />
    </div>
    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print();" Text="PRINT" CssClass="btn btn-large no-print" Visible="true" />
                <asp:Button ID="btnFax" runat="server" OnClick="btnFax_Click" Text="BACK" CssClass="btn btn-large no-print" />
                <asp:Button ID="btnEmail" runat="server" OnClick="btnEmail_Click" Text="EMAIL" CssClass="btn btn-large no-print" Visible="true" />
            </div>
        </div>
    </div>
</asp:Content>
