<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="UserStoreManagment.aspx.cs" Inherits="IMS.UserStoreManagment" %>

<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">


</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <%-- <asp:Label ID="lblAvailableStore" runat="server" Text="Available Store"></asp:Label>--%>

    <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Associate - DeAssociate Systems</h4></td>
            <td align="right">
                 <asp:Button ID="btnSave" runat="server" Text="SAVE" CssClass="btn btn-success" OnClick="btnSave_Click" />
                <asp:Button ID="btnBack" runat="server" Text="Go Back" CssClass="btn btn-default" OnClick="btnBack_Click"/>

            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
    
    <br />

    
    	<style>
				.arrImg{
					width:10%;
                    text-align:center;
					}
					
				.arrImg img{
					margin-bottom:7px;
					}

    	    .associatetbl {
    	        border:0px !important;
            }
    	        .associatetbl tr td {
    	        
                }
	 </style>
    <table class="table table-striped table-bordered table-condensed tblBtm associatetbl" style="border:0px !important;">
        <tr>
               <td>Search Pharmacy :
                    <asp:TextBox ID="txtSearchPharma" runat="server"></asp:TextBox>
                    <asp:Button ID="btnSearchPharma" runat="server" CssClass="btn btn-success btn-large" Text="Search" OnClick="btnSearchPharma_Click"/></td>
        </tr>
        <tr>
            <td style="vertical-align:top;background:none !important; border:0px !important;" >
                <asp:GridView ID="gvAllAvailableStore" CssClass="table table-striped table-bordered table-condensed tblBtm" ShowHeaderWhenEmpty="true" runat="server" AllowPaging="True" PageSize="10"
                    AutoGenerateColumns="false" OnRowCommand="gvAllAvailableStore_RowCommand" OnPageIndexChanging="gvAllAvailableStore_PageIndexChanging">
                    <Columns>
                        <asp:TemplateField HeaderText="System ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblSystemID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>

                        <asp:TemplateField HeaderText="Select-to-Associate" ItemStyle-Width="24%" >
                            <ItemTemplate>
                                <asp:CheckBox ID="CCheckbox" CssClass="col-md-2 control-label" runat="server" AutoPostBack="true" OnCheckedChanged="SelectCheckBox_OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Available System"  ItemStyle-Width="76%">
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
            <td style="vertical-align:central; background:none !important; border:0px !important;" class="arrImg" >

                <asp:ImageButton ID="ImgbtnSwapeAll" runat="server" ImageUrl="~/Images/arrowDRight.png"  OnClick="btnSwapeAll_Click"  /><br />
                <asp:ImageButton ID="ImgbtnSwapeOne" runat="server" ImageUrl="~/Images/arrowRight.png"  OnClick="btnSwapeOne_Click"  /><br />
                <asp:ImageButton ID="ImgbtnBackSwape" runat="server" ImageUrl="~/Images/arrowLeft.png"  OnClick="btnBackSwape_Click" /><br />
                <asp:ImageButton ID="ImgbtnBackSwapeAll" runat="server" ImageUrl="~/Images/arrowDLeft.png"  OnClick="btnBackSwapeAll_Click" /></td>

                
            <td  style="vertical-align:top;background:none !important; border:0px !important;" >
                <asp:GridView ID="gvAssociatesStore" runat="server" ShowHeaderWhenEmpty="true" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                    AutoGenerateColumns="false">
                    <Columns>
                        <asp:TemplateField HeaderText="System ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblAssociatedSystemID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SystemID") %>' Width="110px"></asp:Label>
                            </ItemTemplate>

                            <ItemStyle Width="220px" HorizontalAlign="Left" />
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Select-to-DeAssociate"  ItemStyle-Width="24%">
                            <ItemTemplate>
                                <asp:CheckBox ID="CCheckboxAssocaited"  CssClass="col-md-2 control-label" runat="server" AutoPostBack="true" OnCheckedChanged="SelectAssociatedCheckBox_OnCheckedChanged" />
                            </ItemTemplate>
                        </asp:TemplateField>
                        <asp:TemplateField HeaderText="Associated System"  ItemStyle-Width="76%">
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
   

</asp:Content>
