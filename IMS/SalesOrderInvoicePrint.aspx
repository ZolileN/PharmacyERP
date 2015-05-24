<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrderInvoicePrint.aspx.cs" Inherits="IMS.SalesOrderInvoicePrint" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <title>Sales Packing List</title>
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
			
		
		

  .drg {border:1px solid #ccc; cursor:move;}
  
  @media print {
    .drg {border:0;}
	.genCode{
		display:none;
		}
	}
	
  </style>
  <script>
      $(function () {

          $("#accNum, #billTo, #dateAcc, #invoDate, #invoNum, #salesRep, #tblItems, #grandTotal").draggable();

          $(".genCode").click(function () {
              console.log("accNum");
              console.log($(".accNum").css("top"));
              console.log($(".accNum").css("left"));

              console.log("billTo");
              console.log($(".billTo").css("top"));
              console.log($(".billTo").css("left"));

              console.log("invoNum");
              console.log($(".invoNum").css("top"));
              console.log($(".invoNum").css("left"));


              console.log("invoDate");
              console.log($(".invoDate").css("top"));
              console.log($(".invoDate").css("left"));

              console.log("dateAcc");
              console.log($(".dateAcc").css("top"));
              console.log($(".dateAcc").css("left"));

              console.log("salesRep");
              console.log($(".salesRep").css("top"));
              console.log($(".salesRep").css("left"));


              console.log("tblItems");
              console.log($(".tblItems").css("top"));
              console.log($(".tblItems").css("left"));

              console.log("grandTotal");
              console.log($(".grandTotal").css("top"));
              console.log($(".grandTotal").css("left"));
          });

      });
  </script>
</asp:Content>

<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="main">
          <div class="accNum drg" id="accNum">1111111111</div>
          <div class="billTo drg" id="billTo">&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;&nbsp;
               <asp:Label ID="To" CssClass="col-md-2 control-label" runat="server" style="font-weight: bold" Width="300px"></asp:Label><br />
       	       <asp:Label ID="ToAddress" CssClass="col-md-2 control-label" runat="server"  Width="300px"></asp:Label><br />           
          </div>
          <div class="invoNum drg" id="invoNum"><asp:Literal ID="Invoice" runat="server"></asp:Literal></div>
          <div class="invoDate drg" id="invoDate"><asp:Literal ID="InvoiceDate" runat="server"></asp:Literal></div>
          <div class="dateAcc drg" id="dateAcc"> <asp:Literal ID="DueDate" runat="server"></asp:Literal></div>
          <div class="salesRep drg" id="salesRep"><asp:Literal ID="SalesMan" runat="server"></asp:Literal></div>

        <div class="tblItems drg" id="tblItems">
          <asp:GridView ID="StockDisplayGrid" CssClass="invo-tbl"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false" OnRowDataBound="StockDisplayGrid_RowDataBound" Width="100%">
                 <Columns>
                      <asp:TemplateField HeaderText="" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="Index" CssClass="" runat="server" Text='<%# Container.DataItemIndex %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="5%"  Wrap="true"/>
                    </asp:TemplateField>
                     

                     <asp:TemplateField HeaderText="" Visible="true">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="34%"  Wrap="true"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="" >
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' ></asp:Label>
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
                            <asp:Label ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("SendQuantity") %>' ></asp:Label>
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
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("CostPrice") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="10%" />
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="9%" />
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("Amount") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="12%" />
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("AmountBonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="12%" />
                    </asp:TemplateField>
                    
                    
                 </Columns>
             </asp:GridView>
        </div>
        
        <div class="grandTotal drg" id="grandTotal"><asp:Label ID="lblTotalSentAmount" runat="server" CssClass="control-label">Invoice No </asp:Label>
             <asp:Label runat="server" ID="lblTotalBonusAmount" CssClass="control-label">Invoice Date</asp:Label>
        </div>
    </div>
</asp:Content>
