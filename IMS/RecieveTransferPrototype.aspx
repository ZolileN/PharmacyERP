<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="RecieveTransferPrototype.aspx.cs" Inherits="IMS.RecieveTransferPrototype" %>



<!DOCTYPE html>

<html lang="en">
<head><meta charset="utf-8" /><meta name="viewport" content="width=device-width, initial-scale=1.0" /><title>
	 IMS
</title>

<script src="Prototype Material (6-13-2015)/STORE/Scripts/modernizr-2.6.2.js"></script>
<link href="Prototype Material (6-13-2015)/STORE/Content/bootstrap.css" rel="stylesheet"/>
<link href="Prototype Material (6-13-2015)/STORE/Content/Site.css" rel="stylesheet"/>
<script type="Prototype Material (6-13-2015)/STORE/text/javascript" src="Scripts/jquery.min.js"></script>
<link href="favicon.ico" rel="shortcut icon" type="image/x-icon" /><link href="Prototype Material (6-13-2015)/STORE/Style/theme.css" rel="stylesheet" type="text/css" />
<link href="Prototype Material (6-13-2015)/STORE/Style/fonts.css" rel="stylesheet" type="text/css" />
    
</head>
<body>

	<div class="popupMain" id="vendors">

        <div class="popupHead">Store List --- FINALLERGE SYP 100ml (Available Stock)
            <a href="#" class="close"></a>
        </div>
        <div class="bodyPop">
        	<table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th></th>
                    <th>Store Name</th>
                    <th>Address</th>
                    <th>City</th>
                    <th>State</th>
                    <th>Country</th>
                    <th>Available Stock</th>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>545225</td>
                </tr>
                <tr>
                     <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>25000</td>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>3255</td>
                </tr>
                <tr>
                     <td align="center"><input type="checkbox" /></td>
                    <td>AGENCIES & TRADING CO</td>
                    <td>PO BOX: 35460</td>
                    <td>ABU DHABI</td>
                    <td>ABUDHABI</td>
                    <td>UAE</td>
                    <td>12555</td>
                </tr>
            </table>
            <input type="submit" class="btn btn-primary fl-r btn-sm clS" id="closeProduct" value="Select" />
            
        </div>
    </div>
    
    
    
    
    
    
    <div class="popupMain" id="products">

        <div class="popupHead">Products List
            <a href="#" class="close"></a>
        </div>
        <div class="bodyPop">
        	<table class="table table-striped table-bordered table-condensed">
                <tr>
                    <th></th>
                    <th>Product Name</th>
                    <th>Description</th>
                    <th>Unit Sale</th>
                    <th>Unit Cost</th>
                    <th>ID</th>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>FINALLERGE SYP 100ml</td>
                    <td>Description goes here</td>
                    <td>13</td>
                    <td>8.2575</td>
                    <td>34856</td>	
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>FINALLERGE SYP 100ml</td>
                    <td>Description goes here</td>
                    <td>13</td>
                    <td>8.2575</td>
                    <td>34856</td>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>FINALLERGE SYP 100ml</td>
                    <td>Description goes here</td>
                    <td>13</td>
                    <td>8.2575</td>
                    <td>34856</td>
                </tr>
                <tr>
                    <td align="center"><input type="checkbox" /></td>
                    <td>FINALLERGE SYP 100ml</td>
                    <td>Description goes here</td>
                    <td>13</td>
                    <td>8.2575</td>
                    <td>34856</td>
                </tr>
            </table>
            <input type="submit" class="btn btn-primary fl-r btn-sm"id="closeVendor"  value="Select" />
            
        </div>
    </div>
    
    
    
    
    
    
    
    
    <div class="overLaypop"></div>
    
    
    <div class="container ">
     

        <div class="top-head">
                
                <div class="logo">
                </div>
                <div class="user-menu no-print">
                   
                       <a href="IMSLogin.html" onclick="Unnamed_Click">Logout</a>
                    
                </div>
            </div>

   
       

          <div class="navs-cont" id="loadNav">
                <div style="clear: left;"></div>

          </div>
          <div style="clear: left;"></div>

        <div class="body-cont">
            

		
     
	  
      <table width="100%">

        <tbody><tr>
        	<td> <h4 id="topHead">Manage Transfers</h4></td>
            <td align="right"><input type="submit" class="btn btn-success" value="New Transfer Request" onClick="window.location.href = '#'">              <input type="submit" class="btn btn-default" value="Back" onClick="    window.location.href = 'StoreMain.aspx'">
                
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
     
     <br>
     <table class="table table-striped table-bordered table-condensed" rules="all" id="MainContent_StockDisplayGrid" style="border-collapse:collapse;" border="1" cellspacing="0">
		<tbody>
        <tr>
        	<td colspan="10"><h4 class="fl-l">Al Bustanni Sports & Marine Materials</h4>
        	  <input type="submit" class="btn btn-info fl-r" value="Generate Transfer" onClick="window.location.href = 'TR_Accept.html'">
              <input type="submit" class="btn btn-success fl-r acceptAll" value="Accept All" >
                
            </td>
        </tr>
        <tr>
			<th>Action</th>
			<th>Product Description</th>
            <th>Request No.</th>
            <th>Request Date</th>
            
            <th>Request Qty</th>
            <th>Available Stock</th>
            <th>Sent Qty</th>
            <th>&nbsp;</th>
            
		</tr>
        <tr>
			<td>
                            <input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
			<td>PANALDOL ADVANCE 1000MG</td>
		  <td>4
                        </td><td>4/17/2015 8:52:59 PM
                        </td>
                        <td>5265</td>
                        <td>52545</td>
                        <td>0</td>
                        <td>
                        	<input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >
                            <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny">
                        </td>
		</tr>
        
        <tr>
			<td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
			<td>TROPEX</td>
		  <td>4
                        </td><td>4/17/2015 8:52:59 PM
                        </td>
                        <td>2541</td>
                        <td>85487</td>
                        <td>0</td>
                        <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >                          <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny">
                        </td>
		</tr>
        
        <tr>
			<td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
			<td>MANGESIUM SULPHATE</td>
		  <td>5
                        </td><td>4/17/2015 8:52:59 PM
                        </td>
                        <td>2548</td>
                        <td>65235</td>
                        <td>0</td>
                        <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >                          <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny">
                        </td>
		</tr>
        
        <tr>
			<td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
			<td>DISOPHROL S.A</td>
		  <td>5
                        </td><td>4/17/2015 8:52:59 PM
                        </td>
                        <td>5254</td>
                        <td>32548</td>
                        <td>0</td>
                        <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >                          <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny">
                        </td>
		</tr>
        
        <tr>
			<td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
			<td>PANADOL ADVANCE 500MG</td>
		  <td>11
                        </td><td>4/17/2015 8:52:59 PM
                        </td>
                        <td>1254</td>
                        <td>12548</td>
                        <td>0</td>
                        <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >                          <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny">
                        </td>
		</tr>
        
        
        
        
        
        
	</tbody></table>
     <table class="table table-striped table-bordered table-condensed" rules="all" id="MainContent_StockDisplayGrid2" style="border-collapse:collapse;" border="1" cellspacing="0">
       <tbody>
         <tr>
           <td colspan="10"><h4 class="fl-l">MANSOUR PHARMACY</h4>
              <input type="submit" class="btn btn-info fl-r" value="Generate Transfer" onClick="window.location.href = 'TR_Accept.html'">
           <input type="submit" class="btn btn-success fl-r acceptAll" value="Accept All" ></td>
         </tr>
         <tr>
           <th>Action</th>
           <th>Product Description</th>
           <th>Request No.</th>
           <th>Request Date</th>
           
           <th>Request Qty</th>
           <th>Available Stock</th>
           <th>Sent Qty</th>
           <th>&nbsp;</th>
         </tr>
         <tr>
           <td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
           <td>PANALDOL ADVANCE 1000MG</td>
           <td>4 </td>
           <td>4/17/2015 8:52:59 PM </td>
           <td>5265</td>
           <td>52545</td>
           <td>0</td>
           <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >             <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny"></td>
         </tr>
         <tr>
           <td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
           <td>TROPEX</td>
           <td>4 </td>
           <td>4/17/2015 8:52:59 PM </td>
           <td>2541</td>
           <td>85487</td>
           <td>0</td>
           <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >             <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny"></td>
         </tr>
         <tr>
           <td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
           <td>MANGESIUM SULPHATE</td>
           <td>5 </td>
           <td>4/17/2015 8:52:59 PM </td>
           <td>2548</td>
           <td>65235</td>
           <td>0</td>
           <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >             <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny"></td>
         </tr>
         <tr>
           <td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
           <td>DISOPHROL S.A</td>
           <td>5 </td>
           <td>4/17/2015 8:52:59 PM </td>
           <td>5254</td>
           <td>32548</td>
           <td>0</td>
           <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >             <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny"></td>
         </tr>
         <tr>
           <td><input value="Edit" class="btn btn-default edit-btn" onclick="window.location.href = 'TR_Details.html'" type="submit"></td>
           <td>PANADOL ADVANCE 500MG</td>
           <td>11 </td>
           <td>4/17/2015 8:52:59 PM </td>
           <td>1254</td>
           <td>12548</td>
           <td>0</td>
           <td><input type="submit" class="btn btn-success btn-sm acceptReq" value="Accept" >             <input type="submit" class="btn btn-danger btn-sm denyReq" value="Deny"></td>
         </tr>
       </tbody>
     </table>
     <br />

   
        <div>

</div>
     
        <br />
         
     <span onclick="return confirm('Are you sure you want to delete this order?')">
         
   </span>

   


            
            
            
        </div>

        

    


   </div>

      <div class="footer no-print">

              <div class="fcont">

            
                <p>&copy; 2015 - Software Alliance LLC.</p>

                       
            

              </div>

          </div>
	   <script src="Prototype Material (6-13-2015)/STORE/Scripts/jquery-ui.js"></script>
       <script src="Prototype Material (6-13-2015)/STORE/Scripts/general.js"></script>


</body>
</html>
