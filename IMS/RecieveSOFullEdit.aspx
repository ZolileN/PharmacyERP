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

                     <asp:TemplateField Visible="false" HeaderText="Order Detial Id" >
                        <ItemTemplate>
                            <asp:Label ID="OrderDetID"  runat="server" CssClass="ProductDescription" Text='<%# Eval("OrderDetailID") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Product<br>Description" >
                        <ItemTemplate>
                            <asp:Label ID="ProductDescription"  runat="server" CssClass="ProductDescription" Text='<%# Eval("ProductDescription") %>'></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="280px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      
                    <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiryDate"  runat="server" Text='<%# Eval("Expiry") %>' ></asp:Label>
                        </ItemTemplate>
                          <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                    <asp:TemplateField  HeaderText="Batch<br>No." >
                        <ItemTemplate>
                            <asp:Label ID="lblBatchNumber" runat="server" Text=' <%#Eval("BatchNumber") %>'></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Available<br>Stock" >
                        <ItemTemplate>
                            <asp:Label ID="lblAvailableStock" runat="server" Text=' <%#Eval("AvailableStock")==DBNull.Value?0:int.Parse( Eval("AvailableStock").ToString())  %>'></asp:Label>
                        </ItemTemplate>
                         
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Sent<br>Qty" Visible="True" >
                        <ItemTemplate>
                             <asp:Label ID="SendQuantityVal"  runat="server" Text=' <%#Eval("SendQuantity")==DBNull.Value?0:int.Parse( Eval("SendQuantity").ToString())  %>' Width="47px"></asp:Label>
                        </ItemTemplate>
                          
                          <ItemStyle  Width="130px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Bonus<br>Qty" >
                        <ItemTemplate>
                             <asp:Label ID="BonusQuantityVal"  runat="server" Text=' <%#Eval("BonusQuantity")==DBNull.Value?0:int.Parse( Eval("BonusQuantity").ToString())  %>' Width="47px"></asp:Label>
                         </ItemTemplate>
                        <EditItemTemplate>
                         </EditItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField> 
                      <asp:TemplateField HeaderText="Delivered<br>Qty" >
                        <ItemTemplate>
                             <asp:TextBox ID="DelieveredQtyVal"  runat="server" Text=' <%#Eval("DelieveredQty")==DBNull.Value? int.Parse(Eval("DelieveredQty").ToString()) :int.Parse(Eval("SendQuantity").ToString())  %> ' Width="47px"></asp:TextBox>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Delivered<br>Bonus Qty">
                       
                        <ItemTemplate>
                             <asp:TextBox ID="delBonusQtyVal"  runat="server" Text=' <%#Eval("OrderedBonusQuantity")==DBNull.Value? int.Parse(Eval("DelieveredQty").ToString()) :int.Parse( Eval("OrderedBonusQuantity").ToString())  %> '   Width="47px"></asp:TextBox>
                         </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Damaged" >
                         
                          <ItemTemplate>
                             <asp:TextBox ID="DamagedQuantityVal" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("DamagedQuantity")==DBNull.Value?0:int.Parse( Eval("DamagedQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                              <asp:DropDownList ID="ddDamagedAction" CssClass ="grid-select-form"  runat="server"   >
                                  <asp:ListItem Value="1" Text="Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="Discard"></asp:ListItem>
                              </asp:DropDownList>
                         </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                      <asp:TemplateField HeaderText="Expired" >
                         
                          <ItemTemplate>
                             <asp:TextBox ID="txtExpiredQuantity" OnTextChanged="txtReturnedQuantity_TextChanged"  runat="server" Text=' <%#Eval("ExpiredQuantity")==DBNull.Value?0:int.Parse( Eval("ExpiredQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                             <asp:DropDownList ID="ddExpiredAction"  CssClass="grid-select-form"  runat="server"  >
                                  <asp:ListItem Value="1" Text="Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="Discard"></asp:ListItem>
                                 </asp:DropDownList>
                               </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     <asp:TemplateField HeaderText="Not Accepted">
                         <ItemTemplate>
                             <asp:TextBox ID="txtReturnedQuantity"  runat="server" OnTextChanged="txtReturnedQuantity_TextChanged" Text=' <%#Eval("ReturnedQuantity")==DBNull.Value?0:float.Parse( Eval("ReturnedQuantity").ToString())  %>' Width="47px"></asp:TextBox>
                             <asp:DropDownList ID="ddNotAcceptedAction"  CssClass="grid-select-form"  runat="server"  >
                                  <asp:ListItem Value="1" Text="Vendor"></asp:ListItem>
                                  <asp:ListItem Value="2" Text="Add to Stock"></asp:ListItem>
                                  <asp:ListItem Value="3" Text="Discard"></asp:ListItem>
                                 </asp:DropDownList>
                         </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Discount %">
                        
                          <ItemTemplate>
                             <asp:TextBox ID="txtDiscountPercentage"  runat="server" Text=' <%#Eval("DiscountPercentage")==DBNull.Value?0:float.Parse( Eval("DiscountPercentage").ToString())  %>' Width="47px"></asp:TextBox>
                         </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                     
                    
                 </Columns>
             <PagerStyle CssClass = "GridPager" />
             </asp:GridView>

    <script type="text/javascript">

        $("input").focusout(function () {
              
          }).change(function () {
              SetDelieveredQty();
          });

        function SetDelieveredQty() {
            var SentQty = document.getElementById("MainContent_dgvReceiveSOGrid_SendQuantityVal_0").innerHTML;
            var BonusQty = document.getElementById("MainContent_dgvReceiveSOGrid_BonusQuantityVal_0").innerHTML;
            var DelieveredBonusQty = document.getElementById("MainContent_dgvReceiveSOGrid_delBonusQtyVal_0").value;
            var DamagedQty = document.getElementById("MainContent_dgvReceiveSOGrid_DamagedQuantityVal_0").value;
            var ExpiredQty = document.getElementById("MainContent_dgvReceiveSOGrid_txtExpiredQuantity_0").value;
            var RejectedQty = document.getElementById("MainContent_dgvReceiveSOGrid_txtReturnedQuantity_0").value;

            var DelieveredQty = Number(SentQty) + (Number(BonusQty) - Number(DelieveredBonusQty)) - (Number(DamagedQty) + Number(ExpiredQty) + Number(RejectedQty));
  
            document.getElementById("MainContent_dgvReceiveSOGrid_DelieveredQtyVal_0").value = DelieveredQty;
        }

</script>

</asp:Content>

