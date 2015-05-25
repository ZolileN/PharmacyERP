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
          <div class="billTo drg" id="billTo">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="To" runat="server" style="font-weight: bold" Width="300px"></asp:Label>&nbsp;
       	       <asp:Label ID="ToAddress" runat="server"  Width="300px"></asp:Label>         
          </div>
          <div class="invoNum drg" id="invoNum"><asp:Literal ID="Invoice" runat="server"></asp:Literal></div>
          <div class="invoDate drg" id="invoDate"><asp:Literal ID="InvoiceDate" runat="server"></asp:Literal></div>
          <div class="dateAcc drg" id="dateAcc"> <asp:Literal ID="DueDate" runat="server"></asp:Literal></div>
          <div class="salesRep drg" id="salesRep"><asp:Literal ID="SalesMan" runat="server"></asp:Literal></div>

        <div class="tblItems drg" id="tblItems">
          <asp:GridView ID="StockDisplayGrid" CssClass="invo-tbl"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false" OnRowDataBound="StockDisplayGrid_RowDataBound" Width="100%" BorderWidth="0" GridLines="None">

              
                 <Columns>
                      <asp:TemplateField HeaderText="" Visible="true">
                        <ItemTemplate>
                            <asp:Label   ID="Index" CssClass="" runat="server" Text='<%# Container.DataItemIndex + 1 %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="5%"  Wrap="true"/>
                    </asp:TemplateField>
                     

                     <asp:TemplateField HeaderText="" Visible="true">
                        <ItemTemplate>
                            <asp:Label  ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="34%"  Wrap="true"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Label  ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="13%" />
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" CssClass="" runat="server" Text='<%# Eval("BatchNumber") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="10%" />
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText=""  > 
                        <ItemTemplate>
                            <asp:Label  ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("SendQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="7%" />
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText=""> 
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="" runat="server" Text='<%# Eval("BonusQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="7%" />
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label  ID="lblSales" CssClass="" runat="server" Text='<%# Eval("CostPrice") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="10%" />
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label  ID="lblDiscount" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="9%" />
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label   ID="lblAmount" CssClass="" runat="server" Text='<%# Eval("Amount") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="12%" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label   ID="lblAmountBonus" CssClass="" runat="server" Text='<%# Eval("AmountBonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="12%" />
                    </asp:TemplateField>
                    
                    
                 </Columns>
             </asp:GridView>
        </div>
        
        <div class="grandTotal drg" id="grandTotal">
            <asp:Label ID="lblTotalSentAmount" runat="server" CssClass="control-label">Invoice No </asp:Label>
             <asp:Label runat="server" ID="lblTotalBonusAmount" CssClass="control-label">Invoice Date</asp:Label>
            <asp:Button ID="btnPrint" runat="server" CssClass="btn btn-default btn-large print" Text="Print" OnClientClick="window.print();"/>
        </div>
        <br />
         
     </div>
    </form>
</body>
</html>
