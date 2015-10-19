<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SentTransferRequests.aspx.cs" Inherits="IMS.SentTransferRequests" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Recieve Transfer Request(s)</h4></td>
            <td align="right">  
                <asp:Button ID="btnGenTransferAll" runat="server" CssClass="btn btn-success" Text="New Transfer Request" OnClick="btnGenTransferAll_Click" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" /> 
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     
     <br>

     <table class="table table-striped table-bordered table-condensed tblBtm" width="100%">
		<tbody>
        <tr>
        	<td colspan="8">
    		        <h4 class="fl-l">Our Requests</h4>
			</td>

        </tr>
        <tr>

             <asp:GridView ID="dgvReceiveOurTransfers" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="true" PageSize="100"
                            AutoGenerateColumns="false" OnRowDataBound="dgvReceiveOurTransfers_RowDataBound" OnRowCommand="dgvReceiveOurTransfers_RowCommand">
                            <Columns>

                               
                                <asp:TemplateField HeaderText="Transfer ID" Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferID") %>' Width="140px"></asp:Label>
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
                                
                                 <asp:TemplateField HeaderText="Request To" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestTo") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Quantity"  >
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="TransferedBonusQty" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferedBonusQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferedBonusQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Status" >
                                    <ItemTemplate>
                                        <asp:Label ID="lblTransferStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferStatus") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                                 <asp:TemplateField HeaderText="Action" >
                                    <ItemTemplate>
                                         <asp:Button CssClass="btn btn-info btn-sm" Visible="false" ID="btnReceive" Text="Receive" runat="server" CommandName="ReceiveProductTransfer"  CommandArgument='<%# Container.DataItemIndex %>' />
                                   </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>
                                
                            </Columns>
                        </asp:GridView>
            </tr>
            </tbody>
            </table>
</asp:Content>
