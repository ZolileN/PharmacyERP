<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="AutoRequest_Store.aspx.cs" Inherits="IMS.AutoRequest_Store" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
    <script src="Scripts/jquery.js"  type="text/javascript"></script>
          <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
          <link rel="stylesheet" href="Style/jquery-ui.css" />
          <script>
              $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

          </script>
          
         <script>
             $(function () { $("#<%= DateTextBox2.ClientID %>").datepicker(); });

          </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
     <div class="row">
         <h3>Generate Request From Sales</h3>
         <br />
     </div>
     <div class="form-horizontol">
        <div class="form-group">
            <asp:Label runat="server" ID="Header"  CssClass="col-md-2 control-label">Select Sale Dates to generate auto request </asp:Label>
            <br />
            <br />
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass="col-md-2 control-label">SALE FROM : </asp:Label>
            <div class="col-md-10">
                 <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" OnTextChanged="DateTextBox_TextChanged" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="DateTextBox" CssClass="text-danger" ErrorMessage="The from Sale field is required." ValidationGroup="ExSave"/>
                 <br />
            </div>
            
        </div>
        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="DateTextBox2"  CssClass="col-md-2 control-label">SALE TO : </asp:Label>
            <div class="col-md-10">
                 <asp:TextBox runat="server" ID="DateTextBox2" CssClass="form-control" OnTextChanged="DateTextBox2_TextChanged" />
                 <asp:RequiredFieldValidator runat="server" ControlToValidate="DateTextBox2" CssClass="text-danger" ErrorMessage="The to Sale field is required." ValidationGroup="ExSave"/>
                 <br />
            </div>
            
        </div>
        <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                <asp:Button ID="btnCreateRequest" runat="server" OnClick="btnCreateRequest_Click"  Text="SHOW " CssClass="btn btn-default" ValidationGroup="ExSave"/>
                <asp:Button ID="btnCancelRequest" runat="server" OnClick="btnCancelRequest_Click" Text="REFRESH" CssClass="btn btn-default" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
            </div>
        </div>
         <br />
    </div>

      <div class="form-horizontal">
    <div class="form-group">
        <br />
    <br />
        <asp:GridView ID="StockDisplayGrid" CssClass="table table-striped table-bordered table-condensed"  Visible="true" runat="server" AllowPaging="True" PageSize="10" 
                AutoGenerateColumns="false" OnPageIndexChanging="StockDisplayGrid_PageIndexChanging"   onrowcancelingedit="StockDisplayGrid_RowCancelingEdit" 
                onrowcommand="StockDisplayGrid_RowCommand" OnRowDataBound="StockDisplayGrid_RowDataBound" onrowdeleting="StockDisplayGrid_RowDeleting" onrowediting="StockDisplayGrid_RowEditing" >
                 <Columns>
                    <asp:TemplateField HeaderText="From" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedFrom" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("FROMPLACE") %>' Width="100px" ></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="110px" HorizontalAlign="Left"/>

                    </asp:TemplateField>
                     
                     <asp:TemplateField HeaderText="To" HeaderStyle-Width ="150px">
                        <ItemTemplate>
                            <asp:Label ID="RequestedTo" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("TOPLACE") %>'  Width="140px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="150px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Name" HeaderStyle-Width ="250px">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Product_Name") %>'  Width="250px" ></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="250px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Quantity" HeaderStyle-Width ="110px">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SaleQuantity") %>' ></asp:Label>
                        </ItemTemplate>
                        
                        <EditItemTemplate>
                            <asp:TextBox ID="txtQuantity" CssClass="form-control" runat="server" Text='<%#Eval("SaleQuantity") %>' ></asp:TextBox>
                             <asp:RequiredFieldValidator runat="server" ControlToValidate="txtQuantity" CssClass="text-danger" ErrorMessage="The product quantity field is required." />
                        </EditItemTemplate>
                          <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    

                     <asp:TemplateField HeaderText="Action" HeaderStyle-Width ="200px">
                        <ItemTemplate>
                            <asp:Button CssClass="btn btn-default" ID="btnEdit" Text="Edit" runat="server" CommandName="Edit" />
                            
                        </ItemTemplate>

                        <EditItemTemplate>

                            <asp:LinkButton CssClass="btn btn-default" ID="btnUpdate" Text="Update" runat="server" CommandName="UpdateStock" />
                            <asp:LinkButton CssClass="btn btn-default" ID="btnCancel" Text="Cancel" runat="server" CommandName="Cancel" />
                        </EditItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
        <br />
         <asp:Button ID="btnAccept" runat="server" OnClick="btnAccept_Click" Text="GENERATE REQUEST" CssClass="btn btn-large" Visible="false"/>
         <asp:Button ID="btnDecline" runat="server" OnClick="btnDecline_Click" Text="CANCEL REQUEST" CssClass="btn btn-large" Visible="false" />
    </div>
    <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
               
            </div>
        </div>
    </div>
</asp:Content>
