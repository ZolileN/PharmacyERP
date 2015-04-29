<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="VendorsPopupGrid.ascx.cs" Inherits="IMS.UserControl.VendorsPopupGrid" %>
<asp:GridView ID="gdvVendor" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging"  ShowFooter="true"
                OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing" >
                <Columns>
                    <asp:TemplateField HeaderText="Vendor Name" SortExpression="SupName">
                        <ItemTemplate>
                            <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("SupName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                             <asp:Label ID="Label4" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fax">
                        <ItemTemplate>
                            <asp:Label ID="lblFax" runat="server" Text='<%# Eval("Fax") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Person">
                        <ItemTemplate>
                            <asp:Label ID="lblConPer" runat="server" Text='<%# Eval("ConPerson") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- hidden fields --%>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <asp:TemplateField HeaderText="Action">
                        <ItemTemplate> 
                            <%--<asp:Label ID="Label1"  CssClass="btn btn-default edit-btn" runat="server" Text=''> 
                       <a href="AddEditVendor.aspx?Id=<%# Eval("SuppID")%>">
                                     Edit</asp:Label>--%>
                       <asp:Label ID="Label2" runat="server" Text=''>
                              <asp:LinkButton ID="lnkDelete"  CssClass=""  runat="server" CommandName="Delete">
                             Select </asp:LinkButton></asp:Label>  
                      </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            <PagerStyle CssClass = "GridPager" />
    </asp:GridView>