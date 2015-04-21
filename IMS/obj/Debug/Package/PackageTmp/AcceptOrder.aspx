<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AcceptOrder.aspx.cs" Inherits="IMS.AcceptOrder" %>
<asp:Content ID="Content1" ContentPlaceHolderID="MainContent" runat="server">
     <h3> Stock Request Details</h3> 
    <table>
        <tr>
            <td>
               <b>Receipt Number :</b>
            </td>
            <td>
                <div class="col-md-10">
                    <asp:Label runat="server" ID="ReceiptNum" CssClass="col-md-2 control-label"/>
                </div>

            </td>
            </tr>
   <%-- <tr>
        <td>
           Request From : 
             </td>
            <td>
            <div class="col-md-10">
                <asp:Label runat="server" ID="RequestFrom" CssClass="col-md-2 control-label"/>
            </div>
    
            </td>
    </tr>
        <tr>
            <td>
               Requested Date :
            </td>
            <td>
                <div class="col-md-10">
                 <asp:Label runat="server" ID="RequestDate" CssClass="col-md-2 control-label"/>
                </div>
            </td>
            </tr>--%>
    </table>
     <br />
    <div class="form-horizontal">
    <div class="form-group">
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging" OnRowCancelingEdit="StockDisplayGrid_RowCancelingEdit" OnRowCommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" OnRowEditing="StockDisplayGrid_RowEditing">
                 <Columns>
                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default" ID="btnEdit" Text="Accept" runat="server" CommandName="Edit" Enabled= '<%# IsStatusNotComplete((String) Eval("Status")) %>'/>
                             <%--CommandArgument='<%# Container.DisplayIndex  %>'--%>
                        </ItemTemplate>
                        <EditItemTemplate>
                           <asp:LinkButton ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <br />
                            <asp:LinkButton  ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product Name : Strength : Form : Pack Size" HeaderStyle-Width="500" HeaderStyle-HorizontalAlign="Center">
                        <ItemTemplate>
                            <asp:Label ID="ProductName2" padding-right="5px" runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                            <asp:Label ID="Label1" padding-right="5px" runat="server" Text=" : "></asp:Label>
                            <asp:Label ID="ProductStrength2" padding-right="5px" runat="server" Text='<%# Eval("strength") %>'  ></asp:Label>
                            <asp:Label ID="Label2" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="dosage2"  runat="server" Text='<%# Eval("dosageForm") %>' padding-right="5px" ></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text=" : " padding-right="5px"></asp:Label>
                            <asp:Label ID="packSize2" runat="server" Text='<%# Eval("PackageSize") %>' padding-right="5px" ></asp:Label>
                        </ItemTemplate>
                        
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Product Name" Visible="false" HeaderStyle-Width ="280px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName"  runat="server" Text='<%# Eval("ProductName") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                    <asp:TemplateField HeaderText="Requested Quantity" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity"  runat="server" Text='<%# Eval("OrderedQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     

                      <asp:TemplateField HeaderText="Received Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblSenQuan" runat="server" Text=' <%#Eval("ReceivedQuantity")==DBNull.Value?0:int.Parse( Eval("ReceivedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Accepted Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblRecQuan" runat="server" Text=' <%#Eval("AcceptedQuantity")==DBNull.Value?0:int.Parse( Eval("AcceptedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="RecQuanVal"  runat="server" Text=' <%#Eval("AcceptedQuantity")==DBNull.Value?0:int.Parse( Eval("AcceptedQuantity").ToString())  %>' Width="60px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                       <asp:TemplateField HeaderText="Expiry Date" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblExpDate" runat="server" Text='<%# Eval("Expiry") %>'></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtExpDate"  runat="server" Text='<%# Eval("Expiry")==DBNull.Value?"":Convert.ToDateTime( Eval("Expiry")).ToString("MMM dd ,yyyy") %>' Width="90px"></asp:TextBox>
                         </EditItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                      <asp:TemplateField HeaderText="Batch Number" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblBatch" Text='<%# Eval("BatchNumber")==DBNull.Value?"":Eval("BatchNumber") %>' runat="server" ></asp:Label>
                        </ItemTemplate>
                          <EditItemTemplate>
                             <asp:TextBox ID="txtBatch" Text='<%# Eval("BatchNumber")==DBNull.Value?"":Eval("BatchNumber") %>'  runat="server" Width="80px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="BarCode" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblbarCode" runat="server" Text='<%# Eval("BarCode") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Remaining Quantity" Visible="True" HeaderStyle-Width ="130px">
                        <ItemTemplate>
                            <asp:Label ID="lblRemainQuan" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("RemainingQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Expired Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblExpQuan" runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="ExpQuanVal"  runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>' Width="60px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Expired Quantity ORG"  visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblExpQuanOrg" runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                      
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Defected Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblDefQuan" runat="server" Text=' <%#Eval("DefectedQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="defQuanVal"  runat="server" Text=' <%#Eval("DefectedQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedQuantity").ToString())  %>' Width="60px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Defected Quantity Org" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblDefQuanOrg" runat="server" Text=' <%#Eval("DefectedQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                     
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Returned Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblRetQuan" runat="server" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:int.Parse( Eval("ReturnedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                        <EditItemTemplate>
                             <asp:TextBox ID="retQuanVal"  runat="server" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:int.Parse( Eval("ReturnedQuantity").ToString())  %>' Width="60px"></asp:TextBox>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Returned Quantity Org" Visible="false" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblRetQuanOrg" runat="server" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:int.Parse( Eval("ReturnedQuantity").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                       
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Cost Price" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblCP"  runat="server" Text='<%# Eval("UnitCost") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Sale Price" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblSP"  runat="server" Text='<%# Eval("UnitSale") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                   
                      <asp:TemplateField HeaderText="Status" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblStatus" runat="server" Text='<%# Eval("Status") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Delivery Date" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="lblDDate"  runat="server" Text='<%# Eval("SendDate") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                     <%-- Hidden Fields --%>
                     <asp:TemplateField HeaderText="Order Detail ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOrdDet_id" runat="server" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                      
                     <asp:TemplateField HeaderText="Product ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblProd_id" runat="server" Text='<%# Eval("ProductID") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                     <asp:TemplateField HeaderText="Order Master ID" Visible="false">
                            <ItemTemplate>
                                <asp:Label ID="lblOrdMs_id" runat="server" Text='<%# Eval("OrderNO") %>'></asp:Label>
                            </ItemTemplate>
                      </asp:TemplateField>
                   
                 </Columns>
             </asp:GridView>
             <br />
            
             <asp:Button ID="btnBack" runat="server" OnClick="btnBack_Click" Text="BACK" CssClass="btn btn-large" Visible="true" />
    </div>

    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
</asp:Content>
