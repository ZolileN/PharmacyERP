<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="inDirectPO_GENERATE.aspx.cs" Inherits="IMS.inDirectPO_GENERATE" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
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

        @media print {
            .no-print, .no-print * {
                display: none !important;
            }
        }
	</style>
    
        
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <div class="main" id="MAINDIV" runat="server">
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
        <h1 class="main-h">INDIRECT PURCHASE ORDER REQUEST</h1>

        <table cellpadding="5" width="100%" cellspacing="0">
        	<tr>
            	<td> <asp:Label runat="server" ID="PO_Numberlbl" CssClass="control-label">P.O Number : </asp:Label> <asp:Label runat="server" ID="PO_Number" CssClass="control-label" Text="---" Width="100px"></asp:Label></td></td>
                <td align="right"><asp:Label runat="server" ID="PO_Datelbl" CssClass="control-label">P.O Date : </asp:Label> <span class="date"><asp:Label runat="server" ID="PO_Date" CssClass="control-label" Text="---" Width="300px"></asp:Label></span></td>
               
        </tr>
        </table>
        <br /><br />

        <table width="100%">
            <tr>

                <td>
                    <table cellpadding="5" width="100%" cellspacing="0">
        	            <tr>
                           <td valign="top"><strong>ORDER TO:</strong><br />
           	               <asp:Label runat="server" ID="PO_ToName" CssClass="control-label" Text="---" Width="100%"></asp:Label><br />
       	                   <asp:Label runat="server" ID="PO_ToAddress" CssClass="control-label" Text="---" Width="100%"></asp:Label><br />           
                           <asp:Label runat="server" ID="PO_ToPhone" CssClass="control-label" Text="---" Width="100%"></asp:Label><br />
                           <asp:Label runat="server" ID="PO_ToEmail" CssClass="control-label" Text="---" Width="100%"></asp:Label><br />
                           </td>
                        </tr>
                    </table>
                </td>
                <td align="right">
                    <table cellpadding="5" cellspacing="0">
        	            <tr>
            	            <td valign="top"><strong>REQUESTED BY:</strong><br />
           	                <asp:Label runat="server" ID="PO_FromName" CssClass="col-md-2 control-label" Text="---" Width="100%" Visible ="true"></asp:Label><br />
           	                <asp:Label runat="server" ID="PO_FromAddress" CssClass="col-md-2 control-label" Text="---" Width="100%" Visible ="true"></asp:Label><br />
       	                    <asp:Label runat="server" ID="PO_FromPhone" CssClass="col-md-2 control-label" Text="---" Width="100%" Visible ="true"></asp:Label><br /></td>
                
           	   
                        </tr>
                    </table>
                </td>

            </tr>

        </table>
          

         
             

        <br /><br /><br />
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
            AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowDataBound="StockDisplayGrid_RowDataBound">
            <Columns>
                <asp:TemplateField HeaderText="Ordered Product" Visible="true" HeaderStyle-Width="250px">
                    <ItemTemplate>
                        <asp:Label ID="ProductName" CssClass="control-label" runat="server" Text='<%# Eval("Description") %>' Width="250px"></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="250px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ordered<br>Quantity" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Ordered<br>Bonus" HeaderStyle-Width="120px">
                    <ItemTemplate>
                        <asp:Label ID="lblBonus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Unit<br>Cost Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblUnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

                <asp:TemplateField HeaderText="Total<br>Cost Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalCostPrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>
                 <asp:TemplateField HeaderText="Discount %"  HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblDiscount" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Discount") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="90px" HorizontalAlign="Left"/>
               </asp:TemplateField>
                <asp:TemplateField HeaderText="Discounted<br>Cost Price" HeaderStyle-Width="150px">
                    <ItemTemplate>
                        <asp:Label ID="lblTotalCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("totalDiscountPrice") %>'></asp:Label>
                    </ItemTemplate>
                    <ItemStyle Width="110px" HorizontalAlign="Left" />
                </asp:TemplateField>

            </Columns>
        </asp:GridView>
        <br />
        <br />

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
        
              <br /><br /> 
    </div>
    <br /><br />
    <div class="clear">

    </div>
    <div class="main">
     <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print();" Text="PRINT" CssClass="btn btn-primary btn-large no-print" Visible="true" />
                <asp:Button ID="btnEmail" runat="server" OnClick="btnEmail_Click" Text="EMAIL" CssClass="btn btn-info btn-large no-print" Visible="true" />
                <asp:Button ID="btnFax" runat="server" OnClick="btnBack_Click" Text="BACK" CssClass="btn btn-default btn-large no-print" />
        </div>
   
</asp:Content>
