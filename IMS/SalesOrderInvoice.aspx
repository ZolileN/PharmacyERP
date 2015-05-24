<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SalesOrderInvoice.aspx.cs" Inherits="IMS.SalesOrderInvoice" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 101px;
        }
        .auto-style2 {
            width: 448px;
        }
    </style>
    <script>
        $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>
    <script>
        $(function () { $("#<%= DateTextBox2.ClientID %>").datepicker(); });

          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
       <hr>
    <table cellspacing="5" cellpadding="5" border="0" width="100%" class="formTbl">

        <tr>
            <td><asp:Label ID="Label4" runat="server" AssociatedControlID="txtIvnoice" CssClass="control-label">Invoice No </asp:Label></td>
            <td><asp:TextBox runat="server" ID="txtIvnoice" CssClass="form-control" Enabled="False" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator1" runat="server" ControlToValidate="txtIvnoice" CssClass="text-danger" ErrorMessage="This field is required." ValidationGroup="ExSave"/></td>

            </td>
        
            <td>&nbsp;</td>
            <td>&nbsp;</td>
            
                        
        </tr>
        <tr>
            <td><asp:Label runat="server" ID="lblProd" AssociatedControlID="DateTextBox" CssClass="control-label">Invoice Date</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control product" Visible="true"/>
                <asp:RequiredFieldValidator ID="RequiredFieldValidator2" runat="server" ControlToValidate="DateTextBox" CssClass="text-danger" ErrorMessage="This field is required." ValidationGroup="ExSave"/>
             
           </td>
            <td>&nbsp;</td>
            <td> &nbsp;</td>
        </tr>
         <tr>
            
            <td><asp:Label ID="Label7" runat="server" AssociatedControlID="DateTextBox2" CssClass="control-label">Due Date</asp:Label></td>
            <td> <asp:TextBox runat="server" ID="DateTextBox2" CssClass="form-control" />
                <asp:RequiredFieldValidator ID="RequiredFieldValidator3" runat="server" ControlToValidate="DateTextBox2" CssClass="text-danger" ErrorMessage="This field is required." ValidationGroup="ExSave"/>
             
            </td>
            
            <td>&nbsp;</td>
            <td> &nbsp;</td>
        </tr>
        <tr>
            
            <td><asp:Label ID="Label1" runat="server" AssociatedControlID="DateTextBox2" CssClass="control-label">Sales Man</asp:Label></td>
            <td> <asp:DropDownList runat="server" ID="ddlSalesMan" CssClass="form-control" />
            </td>
            
            <td>&nbsp;</td>
            <td> &nbsp;</td>
        </tr>
       
    </table>

    
        <br />
         <br />
      <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="false" PageSize="10" 
                AutoGenerateColumns="false">
                 <Columns>

                     

                     <asp:TemplateField HeaderText="Product<br>Description" Visible="true" HeaderStyle-Width ="60px" HeaderStyle-Wrap="true">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="" runat="server" Text='<%# Eval("Description") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="60px" HorizontalAlign="Left" Wrap="true"/>
                    </asp:TemplateField>
                    
                      <asp:TemplateField HeaderText="Expriy<br>Date"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("ExpiryDate")==DBNull.Value?"":Convert.ToDateTime( Eval("ExpiryDate")).ToString("MMM dd ,yyyy") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Batch<br>Number"  HeaderStyle-Width ="30px">
                        <ItemTemplate>
                            <asp:Label ID="lblAvStock" CssClass="" runat="server" Text='<%# Eval("BatchNumber") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Discount<br>Percentage"  HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("DiscountPercentage") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Cost<br>Price<br>(AED)"  HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("CostPrice") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      

                     <asp:TemplateField HeaderText="Sent<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="" runat="server" Text='<%# Eval("SendQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Sent<br>Amount<br>(AED)"  HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("Amount") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Bonus<br>Quantity"  HeaderStyle-Width ="60px"> 
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" CssClass="" runat="server" Text='<%# Eval("BonusQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                       
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Bonus<br>Amount<br>(AED)"  HeaderStyle-Width ="60px">
                        <ItemTemplate>
                            <asp:Label ID="lblSales" CssClass="" runat="server" Text='<%# Eval("AmountBonus") %>' ></asp:Label>
                        </ItemTemplate>
                        
                          <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                    
                 </Columns>
             </asp:GridView>
        <br />
         
    </div>
            <hr>
    <table cellspacing="5" cellpadding="5" border="0" width="100%" class="formTbl">

        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style1"><asp:Label ID="Label8" runat="server" CssClass="control-label">Total Sent Amount (AED)</asp:Label></td>
        
            <td><asp:Label ID="lblTotalSentAmount" runat="server" CssClass="control-label">Invoice No </asp:Label></td>
            <td>&nbsp;</td>
            
                        
        </tr>
        <tr>
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style1">
                 <asp:Label runat="server" ID="Label9"  CssClass="control-label">Total Bonus Amount (AED)</asp:Label>
                
             
           </td>
            <%--<td>
                <input type="text" id="txtSearch" runat="server" name="txtSearch"   onkeyup="searchSuggest(event);" autocomplete="off"  /> 
                <div id="search_suggest" style="visibility: hidden;" ></div>

                <asp:TextBox runat="server" ID="txtProduct" CssClass="form-control product" Visible="false"/>
                <asp:ImageButton ID="btnSearchProduct" runat="server" Visible="false" OnClick="btnSearchProduct_Click"  Height="30px" ImageUrl="~/Images/search-icon-512.png" Width="45px" />
                
                <asp:DropDownList runat="server" ID="SelectProduct" Visible="false" CssClass="form-control" Width="280" AutoPostBack="True" OnSelectedIndexChanged="SelectProduct_SelectedIndexChanged"/>
                 
                </td>--%>
            <td><asp:Label runat="server" ID="lblTotalBonusAmount" CssClass="control-label">Invoice Date</asp:Label></td>
            <td> &nbsp;</td>
        </tr>
         <tr>
            
            <td class="auto-style2">&nbsp;</td>
            <td class="auto-style1"> &nbsp;</td>
            
            <td>&nbsp;</td>
            <td> &nbsp;</td>
        </tr>
       
       
    </table>

    
        <br />
         <br />
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnSave" runat="server" OnClick="btnSave_Click" Text="Save" CssClass="btn btn-default btn-large no-print" ValidationGroup="ExSave"/>
                <asp:Button ID="btnPrintActual" runat="server" OnClick="btnPrintActual_Click" Text="Print Actual Invoice" CssClass="btn btn-default btn-large no-print" ValidationGroup="ExSave"/>
                <asp:Button ID="btnPrintBonus" runat="server" OnClick="btnPrintBonus_Click" Text="Print Bonus Invoice" CssClass="btn btn-default btn-large no-print" ValidationGroup="ExSave"/>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large no-print" Text="Go Back" OnClick="btnBack_Click" />            
            </div>
        </div>
    </div>
</asp:Content>
