<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ViewPackingList.aspx.cs" Inherits="IMS.ViewPackingList" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   <style>
		.main{
			font-family:"Calibri", Arial, Helvetica, sans-serif;
			font-size:16px;
			margin:0px auto;
			width:1024px;
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

			width:1024px;
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
    <div class="main">
    <h1 class="main-h">Packing List</h1>
    <br />
    <br />
    <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td align="right"><asp:Label ID="SendDate" CssClass="col-md-2 control-label" runat="server" style="font-weight: bold" Width="300px"></asp:Label></td>
               
    </tr>
    </table>
     <br /><br />
    <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td width="403" valign="top"><strong>FROM:</strong><br />
           	     <asp:Label ID="From" CssClass="col-md-2 control-label" runat="server" style="font-weight: bold" Width="300px"></asp:Label><br />
           	     <asp:Label ID="FromAddress" CssClass="col-md-2 control-label" runat="server"  Width="300px"></asp:Label><br />
                 <td width="322" valign="top" ><strong>TO:</strong><br />
           	     <asp:Label ID="To" CssClass="col-md-2 control-label" runat="server" style="font-weight: bold" Width="300px"></asp:Label><br />
           	     <asp:Label ID="ToAddress" CssClass="col-md-2 control-label" runat="server"  Width="300px"></asp:Label><br />
              </td>
            </tr>
        </table>
    <br /><br /><br />
    <div class="form-horizontal">
        <div class="form-group">
            <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
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
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Description") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="330px" HorizontalAlign="Left" />

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Requested Quantity" HeaderStyle-Width="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderedQuantity") %>' Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle Width="150px" HorizontalAlign="Left" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Total Sent Quantity" HeaderStyle-Width="200px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SendQuantity") %>' Width="140px"></asp:Label>
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
                                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ExpiryDate") %>' Width="250px"></asp:Label>
                                        </ItemTemplate>
                                        <ItemStyle Width="250px" HorizontalAlign="Left" />

                                    </asp:TemplateField>
                                    <asp:TemplateField HeaderText="Sent Quantity" HeaderStyle-Width="200px">
                                        <ItemTemplate>
                                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Quantity") %>' Width="110px"></asp:Label>
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
    </div>
     <div class="form-horizontal">
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-default btn-large no-print" Text="Print" OnClientClick="window.print();"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large no-print" Text="Go Back" OnClick="btnBack_Click" />
            </div>
        </div>
    </div>
</asp:Content>
