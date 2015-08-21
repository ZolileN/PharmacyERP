<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="uc_Order_Display.ascx.cs" Inherits="IMS.UserControl.uc_Order_Display" %>

 <div class="popupMain" id="products">
<div class="popupHead">
    
    Queued PO(s) for Generation
    <input type="submit" class="close" value="" />
   
    </div>


<div class="bodyPop">
     <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnSelectedIndexChanged="StockDisplayGrid_SelectedIndexChanged" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" onrowediting="StockDisplayGrid_RowEditing" OnRowDeleting="StockDisplayGrid_RowDeleting">
                 <Columns>
                    <asp:TemplateField HeaderText="Request No."   HeaderStyle-Width ="60px" visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedNO"  Text='<%# Eval("OrderID") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedNO" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderID") %>' Width="100px" ></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Request Date" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedDate" Text='<%# Eval("OrderDate") %>' ></asp:Label>
                            <%--<asp:Label ID="RequestedDate" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("OrderDate") %>'  Width="140px"></asp:Label>--%>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Requested From" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedFrom" Text='<%# Eval("reqFrom") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("reqFrom") %>'  Width="200px" ></asp:Label>--%>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Requested From ID" HeaderStyle-Width ="200px" Visible="false">
                        <ItemTemplate>
                            <asp:Label runat="server" ID="RequestedFromID" Text='<%# Eval("reqFromID") %>'></asp:Label>
                            <%--<asp:Label ID="RequestedFromID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("reqFromID") %>'  Width="200px" ></asp:Label>--%>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="role" HeaderStyle-Width ="200px" Visible="false">
                        <ItemTemplate>
                            
                            <asp:Label ID="lblSysRole" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("roleName") %>'  Width="200px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Request Status" HeaderStyle-Width ="130px">
                        <ItemTemplate>
                            <asp:Label ID="RequestStatus" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Status") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Generate Order" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>'/>
                             <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                 </Columns>
                <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
      <asp:Button ID="btnIgnore" runat="server" CssClass="btn btn-primary fl-r btn-sm" Text="Ignore" OnClick="btnIgnore_Click" Visible="true" />
</div>

     </div>