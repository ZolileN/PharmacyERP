<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="Inventory_Print.aspx.cs" Inherits="IMS.Inventory_Print" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <style type="text/css">
        .auto-style1 {
            width: 273px;
        }

        .auto-style2 {
            font-size: large;
        }

        .auto-style3 {
            width: 916px;
        }

        .auto-style4 {
            width: 758px;
        }
    </style>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
   
      <table width="100%">

        <tbody>
        <tr>
        <td><h4>Inventory List</h4></td>
        <td align="right">
                <strong>Date: </strong><%: DateTime.Now.Date %>
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-default btn-large no-print " Text="Go Back" OnClick="btnBack_Click"/>
                <asp:Button ID="btnPrint" runat="server" OnClientClick="window.print();" Text="Confirm" CssClass="btn btn-success btn-large no-print" Visible="true" />
                <asp:Button ID="btnFax" runat="server" visible="false" Text="BACK" CssClass="btn btn-info btn-large no-print" />
                <asp:Button ID="btnEmail" runat="server"  Text="EMAIL" CssClass="btn btn-default btn-large no-print" Visible="false" />
        </td>
        </tr>
		<tr><td height="5"></td></tr>
        </tbody>

        </table>
             
          
        <asp:GridView ID="StockDisplayGrid" runat="server" CssClass="table table-striped table-bordered table-condensed" AllowPaging="false" 
                AutoGenerateColumns="false" OnRowDataBound="StockDisplayGrid_RowDataBound" >
                 <Columns>
                     <asp:TemplateField HeaderText="Serial" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="Serial" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SerialNum") %>' Width="25px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="35px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Product Description" HeaderStyle-Width="300" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" padding-right="5px" runat="server" Text='<%# Eval("ProdDesc") %>'></asp:Label>
                           <!-- <asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'  ></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2"  runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px" ></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px" ></asp:Label>-->
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Strength" Visible="false" HeaderStyle-Width ="125px">
                        <ItemTemplate>
                            <asp:Label ID="ProductStrength" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("strength") %>'  Width="125px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="125px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Dosage Form" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="dosage" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("dosageForm") %>'  Width="100px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Package Size" Visible="false" HeaderStyle-Width ="160px">
                        <ItemTemplate>
                            <asp:Label ID="packSize" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("PackageSize") %>'  Width="170px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="170px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                                        
                     <asp:TemplateField HeaderText="Expiry" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="control-label"  runat="server" Text='<%# Eval(("Expiry").ToString(), "{0:dd/MM/yyyy}")%>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="100px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Cost" HeaderStyle-HorizontalAlign="Justify">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCostPrice" CssClass="control-label" runat="server" Text='<%# Eval("CostPrice") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Quantity" HeaderStyle-HorizontalAlign="Justify">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="control-label" runat="server" Text='<%# Eval("Qauntity") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Justify"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Bonus 12">
                        <ItemTemplate>
                            <asp:Label ID="lblBon12" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus12Quantity") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Bonus 25">
                        <ItemTemplate>
                            <asp:Label ID="lblBon25" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus25Quantity") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Bonus 50">
                        <ItemTemplate>
                            <asp:Label ID="lblBon50" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Bonus50Quantity") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEmp1" CssClass="col-md-2 control-label" runat="server" Text='' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEmp2" CssClass="col-md-2 control-label" runat="server" Text='' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblEmp3" CssClass="col-md-2 control-label" runat="server" Text='' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Justify"/>
                       
                    </asp:TemplateField>
                     <%-- org command argument CommandArgument='<%# Eval("BarCode") %>'--%>
                     
                 </Columns>
             </asp:GridView>
       
         
               
            
   
</asp:Content>
