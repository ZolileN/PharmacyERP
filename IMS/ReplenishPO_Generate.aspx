<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReplenishPO_Generate.aspx.cs" Inherits="IMS.ReplenishPO_Generate" %>
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
    <div class="container ">
     
          <div style="clear: left;"></div>

        <div class="body-cont">
            <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead"> Replenish ( Movement ) Transfer Request</h4></td>
            <td align="right">
            
           <!-- onClick="window.location.href='purchase-order.html'" -->
            <asp:Button ID="btnPrint" runat="server" onclick="window.print();" CssClass="btn btn-primary btn-large no-print" />
            <asp:Button ID="btnExport" runat="server" CssClass="btn btn-info btn-large no-print" />
            <asp:Button ID ="btnGoBack" runat="server" CssClass="btn btn-default btn-large no-print" OnClick="btnGoBack_Click" />
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    <hr>
    <div id="MainContent_MAINDIV" class="main">
      <h1 class="main-h">TRANSFER REQUEST</h1>

        
        <hr>

          <table cellpadding="5" width="100%" cellspacing="0">
        	<tbody><tr>
               <td valign="top" class="invoLeftHighlighted">
 					              
               <h4>ORDER FROM:</h4>
           	    <asp:Label ID="lblOrderFrom" runat="server" Text="---"></asp:Label>
               </td>
               <td width="2%"></td>
               
               <td valign="top" class="invoLeftHighlighted">
 					              
               <h4>ORDER TO:</h4>
           	   <asp:Label ID="lblOrderTo" runat="server" Text="---"></asp:Label>
               
               </td>
               
                               
                        </tr>
                        </tbody></table>
               
               </td>
            </tr>
        </tbody></table>

         

        <br>

    <asp:GridView ID="gvReplenismentPO" runat="server" AutoGenerateColumns="false" OnRowDataBound="gvReplenismentPO_RowDataBound" Width="100%">
        <Columns>
            <asp:TemplateField>
                <ItemTemplate>
                    <table class="table table-striped table-bordered table-condensed" cellspacing="0">
		            <tbody>
                        <tr>
			            <th>Requested Product</th>
                        <th>Requested Quantity</th>
                        </tr>
                        <tr>
			            <td><%#Eval("descp") %></td>
                        <td><%#Eval("OrderedQuantity") %></td>
                        </tr>
	                </tbody>
	                </table>
                </ItemTemplate>
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
            
                <td class="signs">Company Stamp:</td><td><span class="underline">&nbsp;</span></td>
            </tr>
            
            <tr>
            	<td colspan="100%" height="10">&nbsp;</td>
            </tr>
            
            <tr>
            
            <td class="signs">Prepared By:</td><td><span class="underline">&nbsp;</span></td>
            <td class="signs">Checked By:</td><td><span class="underline">&nbsp;</span></td>
                <td class="signs">Received By:</td><td><span class="underline">&nbsp;</span></td>
            </tr>
           
        </tbody></table>
        
        <div class="clear"></div>
        <br>
         
    </div>
            
        </div>

        

  
   </div>
</asp:Content>
