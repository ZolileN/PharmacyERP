<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="PharmacyManagement.aspx.cs" Inherits="IMS.PharmacyManagement" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <table width="100%">
    <tr><td>
    <h4>Pharmacy Management</h4>
    </td>
    <td align="right"> 
         <asp:Button ID="btnAddWH" runat="server" CssClass="btn btn-success btn-large" Text="Add Pharmacy" OnClick="btnAddWH_Click"/>
         <asp:Button ID="btnGoBack" runat="server" CssClass="btn btn-default btn-large" Text="Go Back" OnClick="btnGoBack_Click" />
        </td>
                </tr>
                <tr><td height="5"></td></tr>
                </table>
    <hr>

     <br />


      <asp:GridView ID="dgvWarehouse" CssClass="table table-striped table-bordered table-condensed"    runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false"  OnPageIndexChanging="dgvWarehouse_PageIndexChanging"  OnRowDeleting="dgvWarehouse_RowDeleting" 
           OnRowCommand ="dgvWarehouse_RowCommand" OnRowEditing="dgvWarehouse_RowEditing" >
                 <Columns>
                      
                     <asp:TemplateField HeaderText="PharmacyID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblPharmacyID" CssClass="control-label" runat="server" Text='<%# Eval("System_PharmacyID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="BarterExchangeID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblBarterExchangeID" CssClass="control-label" runat="server" Text='<%# Eval("BarterExchangeID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="System RoleID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSystemRoleID" CssClass="control-label" runat="server" Text='<%# Eval("System_RoleID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="ID">
                        <ItemTemplate>
                            <asp:Label ID="lblSystemID" CssClass="control-label" runat="server" Text='<%# Eval("SystemID") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Name"  >
                        <ItemTemplate>
                            <asp:Label ID="lblSystemName" CssClass="control-label" runat="server" Text='<%# Eval("SystemName") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                      <asp:TemplateField HeaderText="Address"  >
                        <ItemTemplate>
                            <asp:Label ID="lblSystemAddress" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemAddress") %>'  Width="180px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                      </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="Phone"  >
                        <ItemTemplate>
                            <asp:Label ID="lblSystemPhone" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemPhone") %>'  Width="180px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                      </asp:TemplateField>

                     <asp:TemplateField HeaderText="Fax" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblSystemFax" CssClass="control-label" runat="server" Text='<%# Eval("SystemFax") %>' ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:Button CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                              </ItemTemplate>
                         <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                 </Columns>
                    <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
   
</asp:Content>
