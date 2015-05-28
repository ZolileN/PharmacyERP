<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Prod2Store.aspx.cs" Inherits="IMS.Prod2Store" %>
<%@ Register assembly="AjaxControlToolkit" namespace="AjaxControlToolkit" tagprefix="cc1" %>
<%@ Register TagName="StorePopup" TagPrefix="UCStoresPopup"  Src="~/UserControl/StoresPopup.ascx"%>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <table width="100%"> 
        <tbody><tr>
        	<td> <h4 id="topHead">Assign Products to Store</h4>
                <asp:Label ID="lblStore" runat="server" style="color:#2c81da"></asp:Label>
        	</td>
           <td align="right">
            <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default" Text="Go Back" OnClick="btnBack_Click"/>
              
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label id="lblSelectStore" runat="server" >Select Store</label></td>
           <td>
               <asp:TextBox ID="txtStore" runat="server" CssClass="form-control product" ></asp:TextBox>
               <asp:Label ID="lblStoreId" runat="server" Visible="false"></asp:Label>
               <asp:Button ID="btnSearch" runat="server" CssClass="search-btn getProducts" OnClick="btnSearch_Click" />
                <%--<input type="submit" runat="server" id="btnSearchVendor" class="search-btn opPop"  />--%>

           </td>
           <td>
               <asp:Button ID="btnContinue" runat="server"  Text="Continue" CssClass="btn btn-primary btn-sm continue" OnClick="btnContinue_Click" Visible="false"/>
           	  
           </td>
            <cc1:ModalPopupExtender ID="mpeCongratsMessageDiv" runat="server" BackgroundCssClass="overLaypop"
                       RepositionMode="RepositionOnWindowResizeAndScroll" TargetControlID="lblSelectStore" ClientIDMode="AutoID"
                       PopupControlID="_CongratsMessageDiv" BehaviorID="EditModalPopupMessage" >
                    </cc1:ModalPopupExtender>
                    
                       
           </tr>

        
    </table>
               
                <div id="_CongratsMessageDiv" class="congrats-cont" style="display: none; ">
                            <UCStoresPopup:StorePopup  id="StoresPopupGrid" runat="server"/>
                        </div>
             
     <div class="form-horizontal">
    <div class="form-group">
        <br />
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed" Visible="false" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" onrowdeleting="StockDisplayGrid_RowDeleting" >
                 <Columns>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                             <span onclick="return confirm('Are you sure you want to delete this record?')">
                                <asp:LinkButton CssClass="btn btn-default del-btn" ID="btnDelete" Text="Delete" runat="server" CommandName="Delete" CommandArgument='<%# Container.DisplayIndex  %>'/>
                            </span>
                        </ItemTemplate>
                         <ItemStyle  Width="180px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="UPC">
                        <ItemTemplate>
                            <asp:Label ID="UPC" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Id_Org") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GreenRain Code">
                        <ItemTemplate>
                            <asp:Label ID="GreenRain" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ItemCode") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="140px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Name">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>' Width="330px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="330px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Type">
                        <ItemTemplate>
                            <asp:Label ID="Type" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DrugType") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Unit Cost Price">
                        <ItemTemplate>
                            <asp:Label ID="UnitCost" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("UnitCost") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Sale Price">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SP") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <%-- Hidden Fields --%>
                      <asp:TemplateField HeaderText="MappingID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="mappingID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("mappingID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="ProductID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="prodID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
            <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
        
    </div>
    </div>
</asp:Content>
