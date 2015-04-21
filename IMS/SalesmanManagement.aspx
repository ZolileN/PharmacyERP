<%@ Page Language="C#" AutoEventWireup="true" MasterPageFile="~/Site.Master" CodeBehind="SalesmanManagement.aspx.cs" Inherits="IMS.SalesmanManagement" %>

<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
    <h3>User Management</h3>
    <asp:Button ID="btnAddNew" runat="server" Text="AddNew" OnClick="AddNew_Click" />
    <table>
        <tr>
            <td>
                <asp:Label ID="lblSalemanName" runat="server" Text="Name"></asp:Label>
                <asp:TextBox ID="txtSalemanName" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblSalemanAddress" runat="server" Text="Address"></asp:Label>
                <asp:TextBox ID="txtSalemanAddress" runat="server"></asp:TextBox></td>
        </tr>
        <tr>
            <td>
                <asp:Label ID="lblPhone" runat="server" Text="Phone:#"></asp:Label>
                <asp:TextBox ID="txtPhone" runat="server"></asp:TextBox>
            </td>
            <td>
                <asp:Label ID="lblCell" runat="server" Text="Cell:#"></asp:Label>
                <asp:TextBox ID="txtCell" runat="server"></asp:TextBox>
            </td>
        </tr>
        <tr>
            <td></td>
            <td>
                <asp:Button ID="btnSearch" runat="server" Text="Search" />
                <asp:Button ID="btnClear" runat="server" Text="Clear" />
            </td>
        </tr>
    </table>
    <h3>User</h3>
     <asp:GridView ID="SalemanDisplayGrid" CssClass="table table-striped table-bordered table-condensed" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false">
                 <Columns>
                     <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="Name" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="Address" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ItemCode") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="140px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <asp:Label ID="Phone" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="330px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton CssClass="btn btn-default" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>

</asp:Content>