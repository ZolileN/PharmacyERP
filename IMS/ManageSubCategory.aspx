<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ManageSubCategory.aspx.cs" Inherits="IMS.ManageSubCategory" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <br />
      <br />
   <table width="100%">

        <tbody><tr>
        <td> <h4>Manage Sub Category</h4> </td>
            <td align="right">
            	 
                <asp:Button ID="btnAddSubCategory" runat="server"  Text=" + Add SubCategory" CssClass="btn btn-success btn-large" OnClick="btnAddSubCategory_Click"   />
                <asp:Button ID="btnGoBack" runat="server"  Text="Go Back" CssClass="btn btn-default btn-large" OnClick="btnGoBack_Click"  />
         
               		 <%--<input type="submit" value=" + Add Department" class="btn btn-success btn-large">--%>
                     <%--<a href="imsMain.html" class="btn btn-default btn-large">Go Back</a>--%>
               
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
        <hr />
    <asp:GridView ID="SubCategoryDisplayGrid" runat="server"  width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="SubCategoryDisplayGrid_PageIndexChanging"   onrowcancelingedit="SubCategoryDisplayGrid_RowCancelingEdit" ShowFooter="true"
            onrowcommand="SubCategoryDisplayGrid_RowCommand" OnRowDataBound="SubCategoryDisplayGrid_RowDataBound" onrowdeleting="SubCategoryDisplayGrid_RowDeleting" onrowediting="SubCategoryDisplayGrid_RowEditing" >
                 <Columns>
                     <asp:TemplateField HeaderText="Sub-Category ID">
                        <ItemTemplate>
                            <asp:Label ID="lblSubCat_ID" runat="server" Text='<%# Eval("subCatID") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <%--<FooterTemplate>
                            <asp:Label ID="lblAdd" runat="server"></asp:Label>
                        </FooterTemplate>--%>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="lblSubCat_Name" runat="server" Text='<%# Eval("subCatName") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtname" runat="server" Text='<%#Eval("subCatName") %>'></asp:TextBox>
                        </EditItemTemplate>
                        
                        <%--<FooterTemplate>
                            <asp:TextBox ID="txtAddname" runat="server"></asp:TextBox>
                        </FooterTemplate>--%>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Category Name">
                        <ItemTemplate>
                            <asp:Label ID="lblCat_Id" runat="server" Text='<%# Eval("categoryName") %>'></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <%--<asp:TextBox ID="txtCatID" runat="server" Text='<%#Eval("categoryName") %>'></asp:TextBox>--%>
                             <asp:DropDownList ID="ddlCategoryName" AutoPostBack="true" runat="server" Visible="false" OnSelectedIndexChanged="ddlCategoryName_SelectedIndexChanged">

                             </asp:DropDownList>
                        </EditItemTemplate>
                        
                         <FooterTemplate>
                            <%--<asp:TextBox ID="txtAddCatID" runat="server"></asp:TextBox>  --%>
                             <asp:DropDownList ID="ddlAddCategoryName" AutoPostBack="true" runat="server" Visible="false"  OnSelectedIndexChanged="ddlAddCategoryName_SelectedIndexChanged">

                             </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Department Name">
                        <ItemTemplate>
                            <asp:Label ID="lblDep_Id"  runat="server" Text='<%# Eval("DepartmentName") %>'></asp:Label>
                        </ItemTemplate>
                        
                         <EditItemTemplate>
                            <%--<asp:TextBox ID="txtCatID" runat="server" Text='<%#Eval("categoryName") %>'></asp:TextBox>--%>
                             <%--<asp:DropDownList ID="ddlDepName" runat="server" >

                             </asp:DropDownList>--%>
                        </EditItemTemplate>                   
                        <FooterTemplate>
                            <%--<asp:TextBox ID="txtAddCatID" runat="server"></asp:TextBox>--%>
                             <asp:DropDownList ID="ddlAddDepName" Visible="false" runat="server">

                             </asp:DropDownList>
                        </FooterTemplate>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Edit">
                        <ItemTemplate>
                            <asp:LinkButton ID="btnEdit" CssClass="btn btn-default edit-btn"  Text="Edit" runat="server" CommandName="Edit" />
                            <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton ID="btnDelete" CssClass="btn btn-default del-btn"  Text="Delete" runat="server" CommandName="Delete"/>
                            </span>
                        </ItemTemplate>

                        <%--<EditItemTemplate>

                            <asp:LinkButton ID="btnUpdate" Text="Update" CssClass="btn btn-default"  runat="server" CommandName="UpdateSubCategory" />
                            <asp:LinkButton ID="btnCancel" Text="Cancel" CssClass="btn btn-default"  runat="server" CommandName="Cancel" />
                        </EditItemTemplate>--%>
                        
                        <%--<FooterTemplate>
                            <asp:Button ID="btnAddRecord" runat="server" CssClass="btn btn-default"   OnClientClick="return ValidateForm();"  Text="Add" CommandName="Add"></asp:Button>
                        </FooterTemplate>--%>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
    <%--<asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>--%>


     
</asp:Content>
