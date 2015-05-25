<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="InvoicePrint.aspx.cs" Inherits="IMS.InvoicePrint" %>

<!DOCTYPE html>
 <meta http-equiv="Content-Type" content="text/html; charset=utf-8" />

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
<title>MetrixInvoice</title>
<style>
		body{
			padding:0px;
			margin:0px;
			
			}
		.main{
			font-family:"Calibri", Arial, Helvetica, sans-serif;
			font-size:16px;
			margin:0px auto;
			width:800px;
			position:relative;
			page-break-after:always;
			height:750px;
			
			}
			
		.accNum{
	position: absolute;
	top: -6px;
	left: 91px;
			}
			
		.billTo{
	width: 300px;
	position: absolute;
	top: 25px;
	left: 0px;
	line-height: 22px;
			}
		.invoNum{
			position: absolute;
			top: -6px;
			left: 591px;
			}
		
		.invoDate{
	position: absolute;
	top: 14px;
	left: 602px;
			}
		.dateAcc{
	position: absolute;
	top: 37px;
	left: 605px;
			}
		.salesRep{
	position: absolute;
	top: 60px;
	left: 595px;
			}
			
		.checkedBy{
	position: absolute;
	left: 88px;
	top: 1093px;
			}
			
		.issuedBy{
	position: absolute;
	left: 356px;
	top: 1092px;
			}
			
		.receipient{
	position: absolute;
	left: 630px;
	top: 1092px;
			}
			
		.tblItems{
	width: 100%;
	top: 137px;
	left: -5px;
	position: absolute;
			}
		
		.grandTotal{
	top: 667px;
	left: 728px;
	position: absolute;
			}
			
		
		

  
  
  @media print {
  
	.genCode{
		display:none;
		}
	}
	
  </style>


</head>


<body>
    <form id="form1" runat="server">
     <div class="main">
          <div class="accNum drg" id="accNum">1111111111</div>
          <div class="billTo drg" id="billTo">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="To" runat="server" style="font-weight: bold" Width="300px"></asp:Label>&nbsp;
       	       <asp:Label ID="ToAddress" runat="server"  Width="300px"></asp:Label>         
          </div>
          <div class="invoNum drg" id="invoNum"><asp:Literal ID="Invoice" runat="server"></asp:Literal></div>
          <div class="invoDate drg" id="invoDate"><asp:Literal ID="InvoiceDate" runat="server"></asp:Literal></div>
          <div class="dateAcc drg" id="dateAcc"> <asp:Literal ID="DueDate" runat="server"></asp:Literal></div>
          <div class="salesRep drg" id="salesRep"><asp:Literal ID="SalesMan" runat="server"></asp:Literal></div>

        <div class="tblItems drg" id="tblItems">
          <asp:GridView ID="StockDisplayGrid" Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false"  Width="100%" BorderWidth="0" GridLines="None"
                ShowHeader="false" BorderColor="Transparent">

                <Columns>
                            <asp:TemplateField>
                                <ItemTemplate>
                                    <table cellpadding="5" width="100%" cellspacing="0" class="invo-tbl">

                                        <tr>
                                            <td width="5%"><%# Container.DataItemIndex + 1 %></td>
                                            <td width="34%" align="left"><%#Eval("Description") %></td>
                                            <td width="13%" align="center"><%# Convert.ToDateTime(Eval("ExpiryDate")).ToString("dd/MM/yyyy") %></td>
                                            <td width="10%" align="center"><%#Eval("BatchNumber") %></td>
                                            <td width="7%" align="center"><%#Eval("SendQuantity") %></td>
                                            <td width="10%" align="center"><%# Convert.ToDecimal(Eval("CostPrice")).ToString("0.##") %></td>
                                            <td width="9%" align="center"><%#Eval("DiscountPercentage") %></td>
                                            <td width="12%" align="center"><%# Convert.ToDecimal(Eval("Amount")).ToString("0.##") %></td>
                                        </tr>
                                    </table>
                                </ItemTemplate>
                            </asp:TemplateField>
                  </Columns>

              
                
             </asp:GridView>
        </div>
        
        <div class="grandTotal drg" id="grandTotal">
            <asp:Label ID="lblTotalSentAmount" runat="server" CssClass="control-label">Invoice No </asp:Label>
             <asp:Label runat="server" ID="lblTotalBonusAmount" CssClass="control-label">Invoice Date</asp:Label>
            <asp:Button ID="btnPrint" runat="server" CssClass="genCode" Text="Print" OnClientClick="window.print();"/>
        </div>
        <br />
         
     </div>
    </form>
</body>
</html>
