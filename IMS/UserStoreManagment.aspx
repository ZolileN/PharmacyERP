<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserStoreManagment.aspx.cs" Inherits="IMS.UserStoreManagment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- <asp:Label ID="lblAvailableStore" runat="server" Text="Available Store"></asp:Label>--%>
    <table>
        <tr>
            <td>
                <asp:GridView ID="gvAllAvailableStore" CssClass="table table-striped table-bordered table-condensed" ShowHeaderWhenEmpty="true" runat="server" AllowPaging="True" PageSize="10"
                    AutoGenerateColumns="false" OnRowCommand="gvAllAvailableStore_RowCommand" OnPageIndexChanging="gvAllAvailableStore_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="System ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Select-to-Associate">
                            <ItemTemplate>
                                <asp:CheckBox ID="CCheckbox" CssClass="col-md-2 control-label" runat="server" AutoPostBack="true" OnCheckedChanged="SelectCheckBox_OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available System">
                            <ItemTemplate>
                                <asp:Label ID="lblNameAvailable" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemName") %>' Width="160px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemName") %>' Width="160px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>
            </td>
            <td>
                <asp:Button ID="btnSwapeAll" runat="server" Text="-->>" OnClick="btnSwapeAll_Click" /><br />
                <asp:Button ID="btnSwapeOne" runat="server" Text="--->" OnClick="btnSwapeOne_Click" /><br />
                <asp:Button ID="btnBackSwape" runat="server" Text="<---" OnClick="btnBackSwape_Click" /><br />
                <asp:Button ID="btnBackSwapeAll" runat="server" Text="<<--" OnClick="btnBackSwapeAll_Click" /></td>

            <td>
                <asp:GridView ID="gvAssociatesStore" runat="server" ShowHeaderWhenEmpty="true" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="System ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAssociatedSystemID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select-to-DeAssociate">
                            <ItemTemplate>
                                <asp:CheckBox ID="CCheckboxAssocaited"  CssClass="col-md-2 control-label" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAssociatedCheckBox_OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Associated System">
                            <ItemTemplate>
                                <asp:Label ID="lblNameAssociated" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemName") %>' Width="160px"></asp:Label>
                            </ItemTemplate>
                            <EditItemTemplate>
                                <asp:TextBox ID="txtBoxName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemName") %>' Width="160px"></asp:TextBox>
                            </EditItemTemplate>
                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                    </Columns>
                </asp:GridView>

            </td>
        </tr>

    </table>
    <asp:Button ID="btnSave" runat="server" Text="Save" CssClass="btn btn-default" OnClick="btnSave_Click" />

<asp:Button ID="btnBack" runat="server" Text="Go Back" CssClass="btn btn-primary btn-large" OnClick="btnBack_Click"/>


</asp:Content>
