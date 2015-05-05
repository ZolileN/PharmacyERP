<%@ Page Title="" Language="C#" MasterPageFile="~/Site.Master" AutoEventWireup="true" CodeBehind="SelectVendor.aspx.cs" Inherits="IMS.SelectVendor" %>
<asp:Content ID="Content1" ContentPlaceHolderID="HeadContent" runat="server">

        <script src="Scripts/modernizr-2.6.2.js"></script>
        <link href="Content/bootstrap.css" rel="stylesheet"/>
        <link href="Content/Site.css" rel="stylesheet"/>

        <script type="text/javascript" src="Scripts/jquery.min.js"></script>
        <link href="favicon.ico" rel="shortcut icon" type="image/x-icon" />
        <link href="Style/theme.css" rel="stylesheet" type="text/css" />
        <link href="Style/fonts.css" rel="stylesheet" type="text/css" />
        <script src="Scripts/jquery-ui.js"></script>
        <script src="Scripts/general.js"></script>

</asp:Content>
<asp:Content ID="Content2" ContentPlaceHolderID="MainContent" runat="server">

    <div class="popupMain" id="vendors">

        <div class="popupHead">Vendor List
            <a href="#" class="close"></a>
        </div>
        <div class="bodyPop">
        	<table class="table table-striped table-bordered table-condensed">
                <asp:GridView ID="gdvVendor" runat="server"  Width="100%" CssClass="table table-striped table-bordered table-condensed" AllowPaging="True" PageSize="10"
                AutoGenerateColumns="false" OnRowDataBound="gdvVendor_RowDataBound" OnPageIndexChanging="gdvVendor_PageIndexChanging"  ShowFooter="true"
                OnRowCommand="gdvVendor_RowCommand" OnRowDeleting="gdvVendor_RowDeleting" OnRowEditing="gdvVendor_RowEditing" >
                <Columns>
                    <asp:TemplateField HeaderText="Vendor Name" SortExpression="SupName">
                        <ItemTemplate>
                            <asp:Label ID="lblVendor" runat="server" Text='<%# Eval("SupName") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Address">
                        <ItemTemplate>
                            <asp:Label ID="lblAdd" runat="server" Text='<%# Eval("Address") %>'></asp:Label>
                            <asp:Label ID="Label3" runat="server" Text='<%# Eval("City") %>'></asp:Label>
                             <asp:Label ID="Label4" runat="server" Text='<%# Eval("Country") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Email">
                        <ItemTemplate>
                            <asp:Label ID="lblEmail" runat="server" Text='<%# Eval("Email") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    
                    <asp:TemplateField HeaderText="Phone">
                        <ItemTemplate>
                            <asp:Label ID="lblPhne" runat="server" Text='<%# Eval("Phone") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Fax">
                        <ItemTemplate>
                            <asp:Label ID="lblFax" runat="server" Text='<%# Eval("Fax") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <asp:TemplateField HeaderText="Contact Person">
                        <ItemTemplate>
                            <asp:Label ID="lblConPer" runat="server" Text='<%# Eval("ConPerson") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                    <%-- hidden fields --%>
                    <asp:TemplateField HeaderText="ID" Visible="false">
                        <ItemTemplate>
                            <asp:Label ID="lblSupID" runat="server" Text='<%# Eval("SuppID") %>'></asp:Label>
                        </ItemTemplate>
                    </asp:TemplateField>
                  <asp:TemplateField HeaderText="Action">
                        <ItemTemplate> 
                          
                       <asp:Label ID="Label2" runat="server" Text=''>
                           
                              <asp:LinkButton ID="lnkDelete"  CssClass=""  runat="server" CommandName="Delete">
                             Select </asp:LinkButton></asp:Label>  
                      </ItemTemplate>
                    </asp:TemplateField>
                </Columns>
            <PagerStyle CssClass = "GridPager" />

         
    </asp:GridView>
               <%-- <tr>
                    <th></th>
                    <th>Vendor Name</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>State</th>
                    <th>Country</th>
                    <th>Phone</th>
                    <th>Fax</th>
                    <th>Email</th>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>026224600</td>
                    <td>026224535</td>
                    <td>vendor@vendor.com</td>

                </tr>
                <tr>
                     <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>026224600</td>
                    <td>026224535</td>
                    <td>vendor@vendor.com</td>

                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>026224600</td>
                    <td>026224535</td>
                    <td>vendor@vendor.com</td>

                </tr>
                <tr>
                     <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>026224600</td>
                    <td>026224535</td>
                    <td>vendor@vendor.com</td>

                </tr>--%>
            </table>
            <input type="submit" class="btn btn-primary fl-r btn-sm" id="closeVendor" value="Select" />
            
        </div>
    </div>
     
   
    
    <div class="overLaypop"></div>

   <div class="container ">
      
          <%--<div class="navs-cont" id="loadNav">
                <div style="clear: left;"></div>

          </div>--%>
          <%--<div style="clear: left;"></div>--%>

        <div class="body-cont">
             
      <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Manual PO(s)</h4></td>
            <%--<td  >
           <%--<input type="submit" class="btn btn-success" value="Generate PO" onClick="window.location.href = 'purchase-order.html'" id="genPO">--%> <%--<input type="submit" class="btn btn-danger" value="Delete PO" id="delPO">--%>
		   <%--<input type="submit" class="btn btn-default" id="backPO" value="Back" onClick="location.reload();"> 
                
                
            </td>--%>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     
    
   <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label for="MainContent_RequestTo" >Select Vendor</label></td>
           <td>

                <input type="text"  class="form-control product" />
                <input type="submit" class="search-btn opPop"  />

           </td>
           <td>
           	 <input type="submit" class="btn btn-primary btn-sm continue"value="Continue" />
           </td>
           </tr>
    </table>
    
             
	<img src="images/po-img.png" width="344" height="344" class="poImg">
   
        <div>

        </div>
       </div>
        
   </div>
</asp:Content>
