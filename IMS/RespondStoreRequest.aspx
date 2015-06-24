<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RespondStoreRequest.aspx.cs" Inherits="IMS.RespondStoreRequest" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
      <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Manage Requests</h4>
             
        	</td>
            <td align="right">
               <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Back" OnClick="btnBack_Click" />

            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
    <hr>
     
     <br>
    <asp:Repeater ID="repReceiveTransfer" runat="server" OnItemDataBound="repReceiveTransfer_ItemDataBound" OnItemCommand="repReceiveTransfer_ItemCommand">

        <ItemTemplate>
            <table class="table table-striped table-bordered table-condensed" rules="all" id="MainContent_StockDisplayGrid" style="border-collapse: collapse;" border="1" cellspacing="0">
                <tbody>
                    <tr>
                        <td colspan="10">
                            <h4 class="fl-l">

                                <asp:Literal ID="litStoreName" runat="server" ></asp:Literal>
                            </h4>
                            <asp:Button ID="btnGenTransfer" runat="server" CssClass="btn btn-info fl-r" Text="Generate SO" CommandName="GenTransferOrder" />
                            <asp:Button ID="btnAcceptTransferOrder" runat="server" CssClass="btn btn-success fl-r acceptAll" Text="Accept All" CommandName="AcceptTransferOrder"/>

                        </td>
                    </tr>


                    <tr>
                        <asp:GridView ID="dgvReceiveTransfer" CssClass="table table-striped table-bordered table-condensed" Visible="true" runat="server" AllowPaging="false" PageSize="10"
                            AutoGenerateColumns="false"  OnRowCommand="dgvReceiveTransfer_RowCommand" OnRowDataBound="dgvReceiveTransfer_RowDataBound"  >
                            <Columns>

                                <asp:TemplateField HeaderText="Action">
                                    <ItemTemplate>
                                        <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" Visible= '<%# !IsStatusComplete((String) Eval("Status")) %>' CommandName="Edit" CommandArgument='<%# Container.DataItemIndex %>' />
                                    </ItemTemplate>

                                     
                                    <ItemStyle Width="100px" HorizontalAlign="Left" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Product Description">
                                    <ItemTemplate>
                                        <asp:Label ID="lblDescription" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductDescription") %>' Width="140px"></asp:Label>
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
                                        <asp:Label ID="lblTransferDetailsID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferDetailID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Request No." Visible="false">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestNo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TransferID") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>


                                <asp:TemplateField HeaderText="Request Date">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedDate") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />

                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Request Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblRequestedQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RequestedQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Available Stock">
                                    <ItemTemplate>
                                        <asp:Label ID="lblAvailableQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("AvailableQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="Sent Qty">
                                    <ItemTemplate>
                                        <asp:Label ID="lblSentQty" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SentQty") %>' Width="140px"></asp:Label>
                                    </ItemTemplate>

                                    <ItemStyle Width="150px" HorizontalAlign="Left" />
                                </asp:TemplateField>

                                <asp:TemplateField HeaderText="">
                                    <ItemTemplate>
                                        <asp:Button CssClass="btn btn-success btn-sm acceptReq" ID="btnAccept" Text="Accept" runat="server" Visible= '<%# !IsStatusComplete((String) Eval("Status")) %>' CommandName="AcceptProductTransfer"  CommandArgument='<%# Container.DataItemIndex %>' />
                                       
                                        <span onclick="return confirm('Are you sure you want to deny?')">
                                          <asp:Button CssClass="btn btn-danger btn-sm denyReq" ID="btnDeny" Text="Deny"  Visible= '<%# !IsStatusComplete((String) Eval("Status")) %>' runat="server" CommandName="DenyProductTransfer"  CommandArgument='<%# Container.DataItemIndex %>' />

                                        </span>
                                        <asp:Button ID="btnStaticAccepted" runat="server" CssClass="btn btn-success btn-sm accepted" Text="Accepted" Visible='<%# IsStatusAccepted((String) Eval("Status")) %>' />
                                        <asp:Label ID="lblStaticDeny" runat="server" CssClass="btn btn-success btn-sm denyReq" Text="Denied" Visible='<%# IsStatusDenied((String) Eval("Status")) %>' />
                                       
                                    </ItemTemplate>
                                </asp:TemplateField>
                            </Columns>
                        </asp:GridView>
                    </tr>
                </tbody>
            </table>


        </ItemTemplate>


    </asp:Repeater>
    
  
  
  
  
  
</asp:Content>
