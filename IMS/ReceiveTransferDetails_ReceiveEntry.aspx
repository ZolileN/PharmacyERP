<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveTransferDetails_ReceiveEntry.aspx.cs" Inherits="IMS.ReceiveTransferDetails_ReceiveEntry" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">

        <tbody><tr>
        	<td> <h4>Request Transfer</h4></td>
            
          <td align="right"> 
              <asp:Button ID="btnUpdate" runat="server" CssClass="btn btn-success btn-large" Text="Update" OnClick="btnUpdate_Click"  OnClientClick="return VerifyValidate()"  />
              <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />
  
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     Total Sent Quantity: <asp:Label ID="lblTotalTransferQty" runat="server"></asp:Label>
     <br /><br />    
     

    <asp:GridView ID="dgvReceiveTransferDetailsReceive" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                            AutoGenerateColumns="false" OnRowDataBound="dgvReceiveTransferDetailsReceive_RowDataBound"  >
                            <Columns>

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferDetailsID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferDetailID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                              
                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblBarCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BarCode") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedDate") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblSP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SP") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblCP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("CP") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField Visible="false" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblStockID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("StockID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Product Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductDescription" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductDescription") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                

                                <asp:TemplateField HeaderText="Expiry">
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ExpiryDate") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Batch<br/> Number">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchNumber" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BatchNumber") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Available<br/>Stock">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableStock" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("AvailableQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Requested Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sent Qty">
                                    <ItemTemplate>
                                         <asp:TextBox ID="txtSendQty" runat="server" CssClass="grid-input-form" Text='<%# Eval("SentQty") %>' Width="140px"></asp:TextBox>

                                    </ItemTemplate>
                                    
                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Bonus Qty">
                                    <ItemTemplate>
                                         <asp:TextBox ID="txtBonusQty" runat="server" CssClass="grid-input-form" Text='<%# Eval("BonusQty") %>' Width="140px"></asp:TextBox>

                                    </ItemTemplate>
                                    
                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 
                            </Columns>
                        </asp:GridView>

    <script type="text/javascript">
 
        function VerifyValidate() {
            var rows = document.getElementById('MainContent_dgvReceiveTransferDetailsReceive').getElementsByTagName('TR');
            
            var TotalSentQty = 0;
            for (var j = 0; j < rows.length ; j++) {

                var availableStockID = "MainContent_dgvReceiveTransferDetailsReceive_lblAvailableStock_" + j;
                var SentQtyIDs = "MainContent_dgvReceiveTransferDetailsReceive_txtSendQty_" + j;
                var availableQty = document.getElementById(availableStockID).innerHTML;
                var SentQts = document.getElementById(SentQtyIDs).value;
                var sendQty = Number(SentQts);
                var availQty = Number(availableQty);
 
                if (availQty < sendQty) {
                    alert("Available Stock  " + availQty + " is less than Transfered Stock  " + sendQty);
                    return false;
                }

                var SentQtyID = "MainContent_dgvReceiveTransferDetailsReceive_txtSendQty_" + j;
                var SentQty = document.getElementById(SentQtyID).value;

                var sent = Number(SentQty)
                TotalSentQty = Number(TotalSentQty) + sent;
                var TotalSent = document.getElementById("MainContent_lblTotalTransferQty").innerHTML;

                if (TotalSent < TotalSentQty) {
                    alert("Total Sent Quantity  " + TotalSent + " is less than sum of Sent " + TotalSentQty);
                    return false;
                }
                
            }

        }
         
        function Validate(id)
        {
             
            var availableStockID = "MainContent_dgvReceiveTransferDetailsReceive_lblAvailableStock_" + id;
            var SentQtyID = "MainContent_dgvReceiveTransferDetailsReceive_txtSendQty_" + id;
            var availableQty = document.getElementById(availableStockID).innerHTML;
            var SentQty = document.getElementById(SentQtyID).value;
            
            var sendQty = Number(SentQty);
            var availQty = Number(availableQty);


            if (availQty < sendQty)
            {
                alert("Available Stock  " + availQty + " is less than Transfered Stock  " + sendQty);
                return false;
            }
        }
    </script>


</asp:Content>
