<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceiveRequestTransfers.aspx.cs" Inherits="IMS.ReceiveRequestTransfers" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      <table width="100%">

        <tbody><tr>
        	<td> <h4>Receive Request Transfer</h4></td>
            
          <td align="right">
          
              <asp:Button ID="btnAcceptAll" runat="server" CssClass="btn btn-success btn-large" Text="Accept All" OnClick="btnAcceptAll_Click" />
              <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" /> 
                   
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     Total Sent Quantity: <asp:Label ID="lblTotalSentQty" runat="server"></asp:Label>
     <br /><br />    
    	


    <asp:GridView ID="dgvReceiveOurTransfersEntry" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                            AutoGenerateColumns="false"  >
                            <Columns>

                               
                                <asp:TemplateField HeaderText="entryID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblentryID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("entryID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="TransferDetailID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferDetailID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferDetailID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 

                                <asp:TemplateField HeaderText="ProductID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Product Description" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Expiry" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblExpiryDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ExpiryDate") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Batch No"  >
                                    <ItemTemplate>
                                        <asp:Label ID="lblBatchNumber" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BatchNumber") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Requested Qty" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Transfered Bonus Qty" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferedBonusQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferedBonusQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                  <asp:TemplateField HeaderText="Sent Qty" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblSentQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SentQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Received Qty" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblReceivedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SentQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                

                                 <asp:TemplateField HeaderText="CP" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblCP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("CP") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="SP" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSP" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SP") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                 <asp:TemplateField HeaderText="Barcode" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblBarcode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Barcode") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                 
                                 
                            </Columns>
                        </asp:GridView>

</asp:Content>
