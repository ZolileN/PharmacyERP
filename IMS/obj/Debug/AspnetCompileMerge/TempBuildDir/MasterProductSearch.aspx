<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="MasterProductSearch.aspx.cs" Inherits="IMS.MasterProductSearch" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
      <h3>  MASTER PRODUCT DETAILS</h3>
    <div class="form-group">
        </div>
 </div>
 <br />
 <br />
 <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" runat="server" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" 
            onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound">
                 <Columns>
                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnEdit" Text="Select Product" runat="server" CommandName="Select" CommandArgument='<%# Container.DisplayIndex  %>' />
                            <br />
                        </ItemTemplate>
                         <ItemStyle  Width="70px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Item No.">
                        <ItemTemplate>
                            <asp:Label ID="DrugID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Drug_id") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Item Name">
                        <ItemTemplate>
                            <asp:Label ID="DrugName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DrugName") %>' Width="310px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="320px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Item Type">
                        <ItemTemplate>
                            <asp:Label ID="DrugType" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("DrugType") %>' Width="90px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="90px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Manufacturer">
                        <ItemTemplate>
                            <asp:Label ID="Manufacterer" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("Manufacterer") %>' Width="120px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="120px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Category">
                        <ItemTemplate>
                            <asp:Label ID="Category" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Category") %>' Width="130px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                       
                    </asp:TemplateField>

                       <asp:TemplateField HeaderText="Unit Sale Price">
                        <ItemTemplate>
                            <asp:Label ID="UnitSale" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitSale") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Unit Cost Price">
                        <ItemTemplate>
                            <asp:Label ID="UnitCost" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("UnitCost") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Item WholeSale Price">
                        <ItemTemplate>
                            <asp:Label ID="itemAWT" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemAWT") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Generic Name">
                        <ItemTemplate>
                            <asp:Label ID="GenericName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("GenericName") %>' Width="170px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="170px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Control">
                        <ItemTemplate>
                            <asp:Label ID="Control" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Control") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Bin Number">
                        <ItemTemplate>
                            <asp:Label ID="Bin_Number" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bin_Number") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="GreenRain Code">
                        <ItemTemplate>
                            <asp:Label ID="GreenRainCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("GreenRainCode") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Brand Name">
                        <ItemTemplate>
                            <asp:Label ID="Brand_Name" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Brand_Name") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="MaxiMum Discount">
                        <ItemTemplate>
                            <asp:Label ID="MaxiMumDiscount" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("MaxiMumDiscount") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Line ID">
                        <ItemTemplate>
                            <asp:Label ID="LineID" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("LineID") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Item Form">
                        <ItemTemplate>
                            <asp:Label ID="itemForm" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemForm") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Item Strength">
                        <ItemTemplate>
                            <asp:Label ID="itemStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemStrength") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Item Pack Type">
                        <ItemTemplate>
                            <asp:Label ID="itemPackType" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemPackType") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Item Pack Size">
                        <ItemTemplate>
                            <asp:Label ID="itemPackSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("itemPackSize") %>' Width="110px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                    
                 </Columns>
             </asp:GridView>
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
    </div>
    </div>
</asp:Content>
