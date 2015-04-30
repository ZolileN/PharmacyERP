<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcceptPurchaseOrder.aspx.cs" Inherits="IMS.AcceptPurchaseOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
   
          
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
          
      <h4>Accept PO </h4> 
    <hr />
    <table>
        <tr>
            <td>
               <b>Receipt Number :</b>
            </td>
            <td>
                
                    <asp:Label runat="server" ID="ReceiptNum" CssClass="control-label"/>
                

            </td>
            </tr>
  
    </table>
     <br />
    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed acPurOrder"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowCancelingEdit="StockDisplayGrid_RowCancelingEdit" OnRowCommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" OnRowEditing="StockDisplayGrid_RowEditing">
                 <Columns>
                      <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default edit-btn" ID="btnEdit" Text="Accept" runat="server" CommandName="Edit" Enabled= '<%# IsStatusNotComplete((String) Eval("Status")) %>' Visible= '<%# IsStatusNotComplete((String) Eval("Status")) %>'/>
                            <asp:Button CssClass="btn btn-default " ID="btnView" Text="View" runat="server" CommandName="ViewEntry" Visible= '<%# IsStatusComplete((String) Eval("Status")) %>'/>
                             <%--CommandArgument='<%# Container.DisplayIndex  %>'--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <asp:LinkButton ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <br />
                            <asp:LinkButton  ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                    </asp:TemplateField>

                     <asp:TemplateField Visible="false" HeaderText="Product Name" >
                        <ItemTemplate>
                            <asp:Label ID="ProductName"  runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Description" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName2" CssClass="ProductDescription" runat="server" Text='<%# Eval("descp") %>'></asp:Label>
                            <!--<asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'  ></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2"  runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px" ></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px" ></asp:Label>-->
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Ordered<br>Qty">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity"  runat="server" Text='<%# Eval("OrderedQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                    <asp:TemplateField Visible="false" HeaderText="Received<br>Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblSenQuan" runat="server" Text=' <%#Eval("ReceivedQuantity")==DBNull.Value?0:int.Parse( Eval("ReceivedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Accepted<br>Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblRecQuan" runat="server" Text=' <%#Eval("AcceptedQuantity")==DBNull.Value?0:int.Parse( Eval("AcceptedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="RecQuanVal"  runat="server" Text=' <%#Eval("AcceptedQuantity")==DBNull.Value?0:int.Parse( Eval("AcceptedQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Remaining<br>Qty" Visible="True" >
                        <ItemTemplate>
                            <asp:Label ID="lblRemainQuan" CssClass="control-label" runat="server" Text='<%# Eval("RemainingQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Expired<br>Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpQuan" runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="ExpQuanVal"  runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                      <asp:TemplateField HeaderText="Defected<br>Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblDefQuan" runat="server" Text=' <%#Eval("DefectedQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="defQuanVal"  runat="server" Text=' <%#Eval("DefectedQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedQuantity").ToString())  %> ' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Returned<br>Qty">
                        <ItemTemplate>
                            <asp:Label ID="lblRetQuan" runat="server" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:int.Parse( Eval("ReturnedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="retQuanVal"  runat="server" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:int.Parse( Eval("ReturnedQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Ordered<br>Bonus Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblOrderedBonus" runat="server" Text=' <%#Eval("OrderedBonusQuantity")==DBNull.Value?0:int.Parse( Eval("OrderedBonusQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                         
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Bonus<br>Qty" >
                        <ItemTemplate>
                            <asp:Label ID="lblBonus" runat="server" Text=' <%#Eval("BonusQuantity")==DBNull.Value?0:int.Parse( Eval("BonusQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtBonus" runat="server" Text=' <%#Eval("BonusQuantity")==DBNull.Value?0:int.Parse( Eval("BonusQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Cost<br>Price">
                        <ItemTemplate>
                            <asp:Label ID="lblCP"  runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                        </ItemTemplate>
                         <EditItemTemplate>
                             <asp:TextBox ID="retCP"  runat="server" Text=' <%#Eval("UnitCost")==DBNull.Value?0:float.Parse( Eval("UnitCost").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Sale<br>Price">
                        <ItemTemplate>
                            <asp:Label ID="lblSP"  runat="server" Text='<%# Eval("UnitSale") %>'></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="retSP"  runat="server" Text=' <%#Eval("UnitSale")==DBNull.Value?0:float.Parse( Eval("UnitSale").ToString())  %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Discount<br>%age" >
                        <ItemTemplate>
                            <asp:Label ID="lblDisc" Text='<%# Eval("discountPer") %>' runat="server" ></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtDisc" Text='<%# Eval("discountPer") %>'  runat="server" Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                         <ItemStyle  Width="70px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Expiry<br>Date" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpDate"  runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>'></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtExpDate" CssClass="clDate"  runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>' Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Batch<br>Number">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" runat="server" ></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtBatch"  runat="server" Width="47px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="80" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                    <asp:TemplateField HeaderText="Status" >
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                   
                     <%-- Hidden Fields --%>
                     <asp:TemplateField Visible="false" HeaderText="Delivery<br>Date">
                        <ItemTemplate>
                            <asp:Label ID="lblDDate"  runat="server" Text='<%# Eval("SendDate") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="BarCode" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblbarCode" runat="server" Text='<%# Eval("BarCode") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order<br>Detail ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOrdDet_id" runat="server" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                      <asp:TemplateField HeaderText="Bonus<br>Quantity Org" Visible="false" >
                        <ItemTemplate>
                            <asp:Label ID="lblBonusOrg" runat="server" Text=' <%#Eval("BonusQuantity")==DBNull.Value?0:int.Parse( Eval("BonusQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Product ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProd_id" runat="server" Text='<%# Eval("ProductID") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order<br>Master ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOrdMs_id" runat="server" Text='<%# Eval("OrderNO") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                     <asp:TemplateField HeaderText="Barcode Serial" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblBrSerial" runat="server" Text='<%# Eval("barcodeSerial") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                    <asp:TemplateField HeaderText="Expiry<br>Date" Visible="false" >
                        <ItemTemplate>
                            <asp:Label ID="lblExpOrg" runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>'></asp:Label>
                        </ItemTemplate>
                         
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             <PagerStyle CssClass = "GridPager" />
             </asp:GridView>
             <br />
            
             <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="BACK" CssClass="btn btn-large" Visible="true" />
    </div>

    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
    <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              //$(function () { $("[id$=MainContent_StockDisplayGrid_txtExpDate]").datepicker(); });
              $(function () {
                  $(".clDate").datepicker();
              });
          </script>
</asp:Content>
