<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPackingList_SO.aspx.cs" Inherits="IMS.ViewPackingList_SO" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Sales Packing List</title>
	<style>
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
     <div class="main">
      <table cellpadding="5" width="100%" cellspacing="0">
    	<tr>
        	<td width="39%" valign="top">
            	<span class="scHead">AL AHLIYA</span>
                <span class="afH">PHARMACEUTICAL TRADING</span><br />
                Tel: 026584223 Fax: 026584229<br />
                Behind safeline<br />
                Street No. 16<br />
                Musaffah Industrial M-44<br />
                P.O. Box : 3032<br />
                Abu Dhabi - U.A.E.<br />
                Email : ahliya.pharmaceutical@gmail.com
            </td>
            <td width="21%"><img src="Images/apt-logo.gif" /></td>
            <td width="40%" align="right" valign="top"><span  class="arabic scHead">الأهلية</span>
            <span class="arabic afH">لتجارة الأدوية</span><br />
            هاتف : 6584223-02 فاكس:6584229-02<br />
            خلف سيف لأئن<br />
			شارع - 16<br />
            مصفح الصناعية م - 44<br />
            ص.ب: 3032<br />
			أبوظبي - أ.ع.م.<br />
            بريد الكتروني:  ahliya.pharmaceutical@gmail.com
            </td>
        </tr>
    </table>
     
         	<h1 class="main-h">Sales Order Packing List</h1>
            <br />
   	    <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td>Sales Order Number: <asp:Literal ID="SaleOrder" runat="server"></asp:Literal></td>
                <td align="right">Order Date: <asp:Literal ID="SendDate" runat="server"></asp:Literal></td>
               
            </tr>
        </table>
        <br />
      <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
               <td valign="top"><strong>ORDER TO:</strong><br />
           	   <asp:Label ID="To" CssClass="col-md-2 control-label" runat="server" style="font-weight: bold" Width="300px"></asp:Label><br />
       	       <asp:Label ID="ToAddress" CssClass="col-md-2 control-label" runat="server"  Width="300px"></asp:Label><br />           </td>
            </tr>
        </table>
        <br /><br /><br />
    <div class="form-horizontal">
        <div class="form-group">
            <asp:GridView ID="StockDisplayGrid" CssClass="invo-tbl" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowCancelingEdit="StockDisplayGrid_RowCancelingEdit"
                OnRowCommand="StockDisplayGrid_RowCommand" OnRowEditing="StockDisplayGrid_RowEditing" OnRowDataBound="StockDisplayGrid_RowDataBound">
                <Columns>

                    <asp:TemplateField HeaderText="" Visible="false" HeaderStyle-Width="0px">
                        <ItemTemplate>
                            <asp:Label ID="OrderDetailID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDetailID") %>' Width="0px" Visible="false"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="0px" HorizontalAlign="Left" />

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Product Description" HeaderStyle-Width="190px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>' Width="250px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="250px" HorizontalAlign="Left" />

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Requested Quantity" HeaderStyle-Width="150px" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="RequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderedQuantity") %>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total Sent Quantity" HeaderStyle-Width="200px" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderedQuantity") %>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="140px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="DETAILS" HeaderStyle-Width="370px">
                        <ItemTemplate>
                            <asp:GridView ID="StockDetailDisplayGrid" AutoGenerateColumns="false" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10">
                                <Columns>

                                     <asp:TemplateField HeaderText="Batch No" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="Batch" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BatchNumber") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width="110px">
                                        <ItemTemplate>
                                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' Width="120px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" HorizontalAlign="Left" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sent Quantity" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SendQuantity") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Bonus Quantity" HeaderStyle-Width="180px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BonusQuantity") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                                    </asp:TemplateField>

                                    <asp:TemplateField HeaderText="Discount %" HeaderStyle-Width="200px" Visible="false">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DiscountPercentage") %>' Width="110px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="120px" HorizontalAlign="Left" />
                                    </asp:TemplateField>
                                </Columns>
                            </asp:GridView>
                        </ItemTemplate>
                        <ItemStyle Width="370px" HorizontalAlign="Left" />
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <!-- Back Button if needed-->
            </div>
        </div>
    </div>

    <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-default btn-large no-print" Text="Print" OnClientClick="window.print();"/>
                <asp:Button ID="btnExport" runat="server" CssClass="btn btn-default btn-large no-print" Text="Export" OnClick="btnExport_Click"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large no-print" Text="Go Back" OnClick="btnBack_Click" />
            </div>
        </div>
    </div>
        </div>
</asp:Content>
