<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReceivedTransferOrders.aspx.cs" Inherits="IMS.ReceivedTransferOrders" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

      <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Completed Transfer Request(s)</h4></td>
            <td align="center">
                     <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back"   />
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    


      <asp:Repeater ID="repReceivedTransferOrders" runat="server" OnItemDataBound="repReceivedTransferOrders_ItemDataBound" >

        <ItemTemplate>
            <table class="table table-striped table-bordered table-condensed" rules="all" id="MainContent_StockDisplayGrid" style="border-collapse: collapse;" border="1" cellspacing="0">
                <tbody>
                    <tr>
                        <td colspan="10">
                            <h4 class="fl-l">
                                 Request No: 
                                 <asp:Literal ID="litReqNo" runat="server" ></asp:Literal>
                                
                            </h4>
                        </td>
                    </tr> 
                    <tr>
                        <td>
                        <asp:GridView ID="dgvReceivedTransferOrders" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                            AutoGenerateColumns="false"    >
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
                                
                                 <asp:TemplateField HeaderText="TransferedBonusQty" Visible="false" >
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
                                
                            </Columns>
                        </asp:GridView>
                        </td>
                    </tr>
                </tbody>
            </table>


        </ItemTemplate>


    </asp:Repeater>

     

</asp:Content>
