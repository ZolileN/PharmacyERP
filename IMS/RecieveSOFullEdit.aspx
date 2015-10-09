<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="RecieveSOFullEdit.aspx.cs" Inherits="IMS.RecieveSOFullEdit" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

 <script src="https://code.jquery.com/jquery-1.10.2.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">


    <table width="100%">

        <tbody><tr>
        	<td> <h4>Receive Sales Order</h4></td>
            
          <td align="right"> <asp:Button ID="btnGoBack" runat="server" OnClick="btnGoBack_Click" Text="Go Back" CssClass="btn btn-default btn-large" />
                
                
          </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     


      <asp:GridView ID="dgvReceiveSOGrid" CssClass="table table-striped table-bordered table-condensed acPurOrder"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false"   OnRowCommand="dgvReceiveSOGrid_RowCommand" OnRowEditing="dgvReceiveSOGrid_RowEditing" OnRowDataBound="dgvReceiveSOGrid_RowDataBound" >
                 <Columns>
                      <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                           <asp:LinkButton ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" CssClass="btn btn-primary btn-sm"/>
                            <br />
                            <asp:LinkButton  ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" CssClass="btn btn-default btn-sm"/>
                        </ItemTemplate>
                           
                    </asp:TemplateField>


                     <asp:TemplateField Visible="false" HeaderText="BatchNumber">
                        <ItemTemplate>
                            <asp:Label ID="BatchNumber"  runat="server" CssClass="ProductDescription" Text='<%# Eval("BatchNumber") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField Visible="false" HeaderText="UnitSale" >
                        <ItemTemplate>
                            <asp:Label ID="UnitSale"  runat="server" CssClass="ProductDescription" Text='<%# Eval("UnitSale") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField Visible="false" HeaderText="UnitCost" >
                        <ItemTemplate>
                            <asp:Label ID="UnitCost"  runat="server" CssClass="ProductDescription" Text='<%# Eval("UnitCost") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                
                     <asp:TemplateField Visible="false" HeaderText="BarCode" >
                        <ItemTemplate>
                            <asp:Label ID="BarCode"  runat="server" CssClass="ProductDescription" Text='<%# Eval("BarCode") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField Visible="false" HeaderText="Store ID" >
                        <ItemTemplate>
                            <asp:Label ID="StoreId"  runat="server" CssClass="ProductDescription" Text='<%# Eval("OrderRequestedFor") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField Visible="false" HeaderText="Order Detial Id" >
                        <ItemTemplate>
                            <asp:Label ID="OrderDetID"  runat="server" CssClass="ProductDescription" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product<br>Description" >
                        <ItemTemplate>
                            <asp:Literal ID="ProductDescription"  runat="server"  Text='<%# Eval("ProductDescription") %>'></asp:Literal>
                        </ItemTemplate>
                         <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Literal ID="lblExpiryDate"  runat="server" Text='<%# Eval("Expiry") %>' ></asp:Literal>
                        </ItemTemplate>
                          <ItemStyle    HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                    <asp:TemplateField  HeaderText="Batch<br>No." >
                        <ItemTemplate>
                            <asp:Literal ID="lblBatchNumber" runat="server" Text=' <%#Eval("BatchNumber") %>'></asp:Literal>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Available<br>Stock" >
                        <ItemTemplate>
                            <asp:Literal ID="lblAvailableStock" runat="server" Text=' <%#Eval("AvailableStock")==DBNull.Value?0:int.Parse( Eval("AvailableStock").ToString())  %>'></asp:Literal>
                        </ItemTemplate>
                         
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Sent<br>Qty" Visible="True" >
                        <ItemTemplate>
                             <asp:Label ID="SendQuantityVal"  runat="server" Text=' <%#Eval("SendQuantity")==DBNull.Value?0:int.Parse( Eval("SendQuantity").ToString())  %>'  ></asp:Label>
                        </ItemTemplate>
                          
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Bonus<br>Qty" >
                        <ItemTemplate>
                             <asp:Label ID="BonusQuantityVal"  runat="server" Text=' <%#Eval("BonusQuantity")==DBNull.Value?0:int.Parse( Eval("BonusQuantity").ToString())  %>'  ></asp:Label>
                         </ItemTemplate>
                        <EditItemTemplate>
                         </EditItemTemplate>
                        <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField> 
                      <asp:TemplateField HeaderText="Accepted<br>Qty" >
                        <ItemTemplate>
                             <asp:TextBox ID="DelieveredQtyVal" CssClass="grid-input-form"  runat="server" Text=' <%#Eval("AcceptedQuantity")==DBNull.Value? int.Parse(Eval("SendQuantity").ToString()): int.Parse(Eval("AcceptedQuantity").ToString())  %> '  ></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Accepted<br>Bonus Qty">
                       
                        <ItemTemplate>
                            
                           
                             <asp:TextBox ID="delBonusQtyVal" CssClass="grid-input-form"   runat="server" Text=' <%#Eval("AcceptedBonusQuantity")==DBNull.Value? int.Parse(Eval("BonusQuantity").ToString()): int.Parse(Eval("AcceptedBonusQuantity").ToString())  %> ' ></asp:TextBox>
                            <%--<asp:TextBox ID="delBonusQtyVal" CssClass="grid-input-form" ReadOnly="true"   runat="server" Text=' <%# int.Parse(Eval("BonusQuantity").ToString())==0? 0   : int.Parse(Eval("BonusQuantity").ToString())%> ' ></asp:TextBox>--%>
                         </ItemTemplate>
                        <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText=" " >
                         
                          <ItemTemplate>

                               <table style="margin:-3px -3px -3px -5px;width:50px; padding:20px 3px !important;">
                                <tr>
                                    <td style="border:0px; border-bottom:1px solid #ededed; padding:7px 3px !important;">

                                       Actual-->

                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:0px;padding:7px 3px !important;">

                                       Bonus-->

                                    </td>
                                </tr>
                            </table>

                             
                         </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left"/>
                    </asp:TemplateField>


                     <asp:TemplateField HeaderText="Damaged" >
                         
                          <ItemTemplate>

                               <table style="margin:-3px -3px -3px -5px;width:50px; padding:20px 3px !important;">
                                <tr>
                                    <td style="border:0px; border-bottom:1px solid #ededed; padding:7px 3px !important;">

                                        <asp:TextBox ID="DamagedQuantityVal" CssClass="grid-input-form" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("DamagedQuantity")==DBNull.Value?0:int.Parse( Eval("DamagedQuantity").ToString())  %>' ></asp:TextBox>
                              <asp:DropDownList ID="ddDamagedAction" CssClass ="grid-select-form"  runat="server"   >
                                  <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                              </asp:DropDownList>

                                    </td>
                                </tr>
                                <tr>
                                    <td style="border:0px;padding:7px 3px !important;">

                                        <asp:TextBox ID="DamagedBonusQuantityVal" CssClass="grid-input-form" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("DefectedBonusQuantity")==DBNull.Value?0:int.Parse( Eval("DefectedBonusQuantity").ToString())  %>' ></asp:TextBox>
                              <asp:DropDownList ID="ddDamagedBonusAction" CssClass ="grid-select-form"  runat="server"   >
                                  <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                              </asp:DropDownList>

                                    </td>
                                </tr>
                            </table>

                             
                         </ItemTemplate>
                        <ItemStyle HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Expired" >
                         
                          <ItemTemplate>
                              <table style="margin:-3px -3px -3px -5px;width:50px; padding:20px 3px !important;">
                                <tr>
                                    <td style="border:0px; border-bottom:1px solid #ededed; padding:7px 3px !important;">
                             <asp:TextBox ID="txtExpiredQuantity" CssClass="grid-input-form" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>' ></asp:TextBox>
                             <asp:DropDownList ID="ddExpiredAction"  CssClass="grid-select-form"  runat="server"  >
                                   <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                                 </asp:DropDownList>

                            </td>
                                </tr>
                                <tr>
                                    <td style="border:0px;padding:7px 3px !important;">
                                         <asp:TextBox ID="txtExpiredBonusQuantity" CssClass="grid-input-form" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("ExpiredBonusQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredBonusQuantity").ToString())  %>' ></asp:TextBox>
                             <asp:DropDownList ID="ddExpiredBonusAction"  CssClass="grid-select-form"  runat="server"  >
                                   <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                                 </asp:DropDownList>
                                        </td>
                                    </tr>
                          </table>


                               </ItemTemplate>
                          
                        <ItemStyle  HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Not Accepted">
                         <ItemTemplate>

                             <table style="margin:-3px -3px -3px -5px;width:50px; padding:20px 3px !important;">
                                <tr>
                                    <td style="border:0px; border-bottom:1px solid #ededed; padding:7px 3px !important;">

                             <asp:TextBox ID="txtReturnedQuantity" CssClass="grid-input-form"  runat="server" OnTextChanged="txtReturnedQuantity_TextChanged" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:float.Parse( Eval("ReturnedQuantity").ToString())  %>' ></asp:TextBox>
                             <asp:DropDownList ID="ddNotAcceptedAction"  CssClass="grid-select-form"  runat="server"  >
                                   <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                                 </asp:DropDownList>

                                        </td>
                                </tr>
                                <tr>
                                    <td style="border:0px;padding:7px 3px !important;">

                                        <asp:TextBox ID="txtReturnedBonusQuantity" CssClass="grid-input-form"  runat="server" OnTextChanged="txtReturnedQuantity_TextChanged" Text=' <%#Eval("ReturnedBonusQuantity")==DBNull.Value?0:float.Parse( Eval("ReturnedBonusQuantity").ToString())  %>' ></asp:TextBox>
                             <asp:DropDownList ID="ddNotAcceptedBonusAction"  CssClass="grid-select-form"  runat="server"  >
                                   <asp:ListItem Value="1" Text="R Return to Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="A Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="D Discard"></asp:ListItem>
                                 </asp:DropDownList>

                                         </td>
                                    </tr>
                          </table>

                         </ItemTemplate>
                         <ItemStyle  HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Discount %">
                        
                          <ItemTemplate>
                             <asp:Literal ID="txtDiscountPercentage"  runat="server" Text=' <%#Eval("DiscountPercentage")==DBNull.Value?0:float.Parse( Eval("DiscountPercentage").ToString())  %>' ></asp:Literal>
                         </ItemTemplate>
                         <ItemStyle   HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                    
                 </Columns>
             <PagerStyle CssClass = "GridPager" />
             </asp:GridView>

    <script type="text/javascript">

        document.getElementById("MainContent_dgvReceiveSOGrid_DelieveredQtyVal_0").readOnly = true;
        document.getElementById("MainContent_dgvReceiveSOGrid_delBonusQtyVal_0").readOnly = true;

        //$("input").bind("keyup", function (e) {
           
        //    //  }).change(function () {

        //    var send = "MainContent_dgvReceiveSOGrid_SendQuantityVal_" + id;
        //    var bonus = "MainContent_dgvReceiveSOGrid_BonusQuantityVal_" + id;
        //    var delbon = "MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id;
        //    var damag = "MainContent_dgvReceiveSOGrid_DamagedQuantityVal_" + id;
        //    var exp = "MainContent_dgvReceiveSOGrid_txtExpiredQuantity_" + id;
        //    var ret = "MainContent_dgvReceiveSOGrid_txtReturnedQuantity_" + id;

        //    var SentQty = document.getElementById(send).innerHTML;

        //    var BonusQty = document.getElementById(bonus).innerHTML;
        //    var DelieveredBonusQty = document.getElementById(delbon).value;
        //    var DamagedQty = document.getElementById(damag).value;
        //    var ExpiredQty = document.getElementById(exp).value;
        //    var RejectedQty = document.getElementById(ret).value;

           


            
        //  });

        function SetDelieveredQty(id) {
            
            var send = "MainContent_dgvReceiveSOGrid_SendQuantityVal_" + id;
            var bonus = "MainContent_dgvReceiveSOGrid_BonusQuantityVal_" + id;
            var delbon = "MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id;
            var damag = "MainContent_dgvReceiveSOGrid_DamagedQuantityVal_" + id;
            var exp = "MainContent_dgvReceiveSOGrid_txtExpiredQuantity_" + id;
            var ret = "MainContent_dgvReceiveSOGrid_txtReturnedQuantity_" + id;
           
            var SentQty = document.getElementById(send).innerHTML;
            
            var BonusQty = document.getElementById(bonus).innerHTML;
            var DeliveredBonusQty = document.getElementById(delbon).value;
            var DamagedQty = document.getElementById(damag).value;
            var ExpiredQty = document.getElementById(exp).value;
            var RejectedQty = document.getElementById(ret).value;
            
            
            var finalAcceptedQty = parseInt(DamagedQty) + parseInt(ExpiredQty) + parseInt(RejectedQty);
            
            
            if (finalAcceptedQty > SentQty) {
                alert("Adjustment Quantity Cannot be greater than Sent Quantity, please re-adjust");

                document.getElementById(damag).value = 0;
                document.getElementById(exp).value = 0;
                document.getElementById(ret).value = 0;
                document.getElementById("MainContent_dgvReceiveSOGrid_DelieveredQtyVal_" + id).value = SentQty;

            }
            else if (finalAcceptedQty == SentQty || finalAcceptedQty < SentQty) {

                document.getElementById("MainContent_dgvReceiveSOGrid_DelieveredQtyVal_" + id).value = parseInt(SentQty) - finalAcceptedQty;
            }
            else {
                alert("An error occured.");
                document.getElementById(damag).value = 0;
                document.getElementById(exp).value = 0;
                document.getElementById(ret).value = 0;
            }

        }


        function SetDelieveredBonusQty(id) {

            
            var bonus = "MainContent_dgvReceiveSOGrid_BonusQuantityVal_" + id;
            var delbon = "MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id;
            var damag = "MainContent_dgvReceiveSOGrid_DamagedBonusQuantityVal_" + id;
            var exp = "MainContent_dgvReceiveSOGrid_txtExpiredBonusQuantity_" + id;
            var ret = "MainContent_dgvReceiveSOGrid_txtReturnedBonusQuantity_" + id;

            

            var BonusQty = document.getElementById(bonus).innerHTML;
            var DeliveredBonusQty = document.getElementById(delbon).value;
            var DamagedQty = document.getElementById(damag).value;
            var ExpiredQty = document.getElementById(exp).value;
            var RejectedQty = document.getElementById(ret).value;


            var finalAcceptedBonusQty = parseInt(DamagedQty) + parseInt(ExpiredQty) + parseInt(RejectedQty);


            if (finalAcceptedBonusQty > BonusQty) {
                alert("Adjustment Quantity Cannot be greater than Sent Quantity, please re-adjust");

                document.getElementById(damag).value = 0;
                document.getElementById(exp).value = 0;
                document.getElementById(ret).value = 0;
                document.getElementById("MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id).value = BonusQty;

            }
            else if (finalAcceptedBonusQty == BonusQty || finalAcceptedBonusQty < BonusQty) {

                document.getElementById("MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id).value = parseInt(BonusQty) - finalAcceptedBonusQty;
            }
            else {
                alert("An error occured.");
                document.getElementById(damag).value = 0;
                document.getElementById(exp).value = 0;
                document.getElementById(ret).value = 0;
            }

        }

            //if (DelieveredBonusQty > BonusQty) {
            //    alert("Accepted Bonus Quantity should not be greate than Bonus Quantity");
                                                                                                                                                                                                                                                                                                                                                                                                                                                                 
            //    return;
            //}
            //if (Number(SentQty) == 0) {
                
            //    var AcceptedBonusQty = (Number(DelieveredBonusQty) - Number(BonusQty)) - (Number(DamagedQty) + Number(ExpiredQty) + Number(RejectedQty));
            //    var delqty = "MainContent_dgvReceiveSOGrid_delBonusQtyVal_" + id;
            //    document.getElementById(delqty).value = AcceptedBonusQty;

            //    if (Number(AcceptedBonusQty) > Number(BonusQty)) {
            //        alert("Sent bonus qty and Accepted bonus qty mismatch. Please re-adjust.")
            //        return;
            //    }

            //}
            //if (Number(SentQty) != 0)
            //{
            //    if (Number(BonusQty) - Number(DelieveredBonusQty) > 0) {
            //        var DelieveredQty = Number(SentQty) + (Number(BonusQty) - Number(DelieveredBonusQty)) - (Number(DamagedQty) + Number(ExpiredQty) + Number(RejectedQty));

            //    }
            //    else {
            //        var DelieveredQty = Number(SentQty) + (Number(BonusQty) - Number(DelieveredBonusQty)) - (Number(DamagedQty) + Number(ExpiredQty) + Number(RejectedQty));

            //    }

            //    var delqty = "MainContent_dgvReceiveSOGrid_DelieveredQtyVal_" + id;
            //    document.getElementById(delqty).value = DelieveredQty;
            //    if (Number(DelieveredQty) > Number(SentQty)) {
            //        alert("Sent Quantity and Accepted Qty mismatch. Please re-adjust it.")
            //        return;
            //    }
            //}
            

            

       

</script>

</asp:Content>

