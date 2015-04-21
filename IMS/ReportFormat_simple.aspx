<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="ReportFormat_simple.aspx.cs" Inherits="IMS.ReportFormat_simple" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">
     <script src="Scripts/jquery.js"  type="text/javascript"></script>
     <script src="Scripts/jquery-ui.js" type="text/javascript"></script>
     <link rel="stylesheet" href="Style/jquery-ui.css" />
     <script>
      $(function () { $("#<%= DateTextBox.ClientID %>").datepicker(); });

      </script>
</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">
    <h4>Report</h4>
     <hr />
    <div class="row">
    <div class="form-group">
     <div class="form-group">

         <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductDept" CssClass="col-md-2 control-label">Requested By</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductDept" CssClass="form-control" Width="29%" AppendDataBoundItems="True" AutoPostBack="True" />
                
                <br />
            </div>
        </div>  

        <div class="form-group">
            <asp:Label runat="server" AssociatedControlID="ProductCat" CssClass="col-md-2 control-label">Requested From</asp:Label>
            <div class="col-md-10">
                <asp:DropDownList runat="server" ID="ProductCat" CssClass="form-control" Width="29%" AutoPostBack="True"  />
                
                <br />
            </div>
        </div>
            <asp:Label runat="server" AssociatedControlID="DateTextBox"  CssClass="col-md-2 control-label">Select Date</asp:Label>
            <div class="col-md-10">
                 <asp:TextBox runat="server" ID="DateTextBox" CssClass="form-control" />
                 
                <br />
            </div>
            
        </div>
        </div>
     <div class="form-group">
            <div class="col-md-offset-2 col-md-10">
                
                <asp:Button ID="btnView" runat="server" OnClick="btnView_Click" Text="View" CssClass="btn btn-default" />
                <asp:Button ID="btnBack" runat="server" CssClass="btn btn-primary btn-large" Text="Go Back" OnClick="btnBack_Click"/>
            </div>
         </div>
        </div>
</asp:Content>
