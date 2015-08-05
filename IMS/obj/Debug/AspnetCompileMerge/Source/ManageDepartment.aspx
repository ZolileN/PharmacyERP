<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageDepartment.aspx.cs" Inherits="IMS.ManageDepartment" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <br />
      <br />
    <h4>Manage Department</h4>
     
        <hr />
    <asp:GridView ID="DepDisplayGrid" runat="server" width="100%" AllowPaging="True" PageSize="10" CssClass="table table-striped table-bordered table-condensed"
                AutoGenerateColumns="false" OnPageIndexChanging="DepDisplayGrid_PageIndexChanging"   onrowcancelingedit="DepDisplayGrid_RowCancelingEdit" ShowFooter="true"
            onrowcommand="DepDisplayGrid_RowCommand"  onrowdeleting="DepDisplayGrid_RowDeleting" onrowediting="DepDisplayGrid_RowEditing">
                <Columns>
                    <asp:TemplateField HeaderText="Department ID">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_ID" runat="server"  Text='<%# Eval("DepId") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <FooterTemplate>
                            <asp:Label ID="lblAdd" runat="server"></asp:Label>
                        </FooterTemplate>
                    </asp:TemplateField>
                   
                    <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Name" runat="server" Text='<%# Eval("Name") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>

                            <asp:TextBox ID="txtname" runat="server"  Text='<%#Eval("Name") %>'></asp:TextBox>
                        </EditItemTemplate>
                        
                        <FooterTemplate>
                            <asp:TextBox ID="txtAddname" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Code">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Code" runat="server" Text='<%# Eval("Code") %>'></asp:Label>
                        </ItemTemplate>
                         <EditItemTemplate>
                            <asp:TextBox ID="txtCode" runat="server" Text='<%#Eval("Code") %>'></asp:TextBox>
                        </EditItemTemplate>
                        
                        <FooterTemplate>
                            <asp:TextBox ID="txtAddCode" runat="server"></asp:TextBox>
                        </FooterTemplate>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="btn btn-default"  Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton ID="btnDelete" Text="Delete" CssClass="btn btn-default"  runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:LinkButton ID="btnUpdate" Text="Update" CssClass="btn btn-default"  runat="server" CommandName="UpdateDep" />
                            <asp:LinkButton ID="btnCancel" Text="Cancel" CssClass="btn btn-default"  runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                        
                        <FooterTemplate>
                            <asp:Button ID="btnAddRecord" runat="server" Text="Add" CssClass="btn btn-default"  CommandName="Add"></asp:Button>
                        </FooterTemplate>
                    </asp:TemplateField>
                </Columns>
            </asp:GridView>
    <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
</asp:Content>
