<%@ Control Language="C#" AutoEventWireup="true" CodeBehind="SuperMasterProducts.ascx.cs" Inherits="IMS.UserControl.SuperMasterProducts" %>
<html xmlns="http://www.w3.org/1999/xhtml">
<head>
<meta http-equiv="Content-Type" content="text/html; charset=utf-8" />
<title>Super Master Products</title>
<link href="../Style/popup-theme.css" rel="stylesheet" />
<script type="text/javascript" src="../Scripts/jquery.min.js"></script>
<script type="text/javascript">
    $(document).ready(function () {
        var docWidth = $(document).width();
        var docHeight = $(document).height();
        $(".popupOverlay").css({ width: docWidth, height: docHeight });
        var popUpWidth = Math.round(docWidth / 2);
        var popUpHeight = Math.round(docHeight / 2);
        var marLeft = -(Math.round(popUpWidth / 2));
        var marTop = -(Math.round(popUpHeight / 2));

        $(".popupCont").css({ width: popUpWidth, height: popUpHeight, top: marTop, left: marLeft });

        $(".close-ico").click(function () {
            $(".popupCont").hide();
            $(".popupOverlay").hide();
        })
    });
</script>

<asp:PlaceHolder runat="server">
        <%: Scripts.Render("~/bundles/modernizr") %>
    </asp:PlaceHolder>
<webopt:bundlereference runat="server" path="~/Content/css" />
<link href="~/favicon.ico" rel="shortcut icon" type="image/x-icon" />
</head>

<body>
<div class="popupCont">    
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
                     <asp:TemplateField HeaderText="BarCode">
                        <ItemTemplate>
                            <asp:Label ID="BarCode" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("BarCode") %>' Width="100px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="110px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Name">
                        <ItemTemplate>
                            <asp:Label ID="ProductName" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("ProductName") %>' Width="310px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="320px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Quantity">
                        <ItemTemplate>
                            <asp:Label ID="lblQuantity" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("Qauntity") %>' Width="40px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="50px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                    
                     <asp:TemplateField HeaderText="Expiry">
                        <ItemTemplate>
                            <asp:Label ID="lblExpiry" CssClass="col-md-2 control-label"  runat="server" Text='<%# Eval("Expiry") %>' Width="190px"></asp:Label>
                        </ItemTemplate>
                         <ItemStyle  Width="200px" HorizontalAlign="Left"/>
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Cost">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitCostPrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("CostPrice") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Unit Sale">
                        <ItemTemplate>
                            <asp:Label ID="lblUnitSalePrice" CssClass="col-md-2 control-label" runat="server" Text='<%# Eval("SalePrice") %>' Width="50px"></asp:Label>
                        </ItemTemplate>
                        <ItemStyle  Width="60px" HorizontalAlign="Left"/>
                       
                    </asp:TemplateField>

                     <asp:TemplateField HeaderText="Action">
                        <ItemTemplate>
                            <asp:LinkButton CssClass="btn btn-default" ID="btnEdit" Text="Select Product" runat="server" CommandName="Print" CommandArgument='<%# Container.DisplayIndex  %>' />
                            <br />
                        </ItemTemplate>
                         <ItemStyle  Width="70px" HorizontalAlign="Left"/>
                    </asp:TemplateField>
                 </Columns>
             </asp:GridView>
        <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
    </div>
    </div>
</div>
</body>

</html>