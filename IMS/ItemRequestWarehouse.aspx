<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="ItemRequestWarehouse.aspx.cs" Inherits="IMS.ItemRequestWarehouse" %>



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

        <div class="popupHead">Vendor List
            <a href="#" class="close"></a>
        </div>
        <div class="bodyPop">
        	<table class="table table-striped table-bordered table-condensed">
                <tr>
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

                </tr>
            </table>
            <input type="submit" class="btn btn-primary fl-r btn-sm" id="closeVendor" value="Select" />
            
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
            <input type="submit" class="btn btn-primary fl-r btn-sm" id="cProduct" value="Select" />
            
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
        	<td> <h4 id="topHead">Item Request to Warehouse</h4></td>
            <td align="right">
           <input type="submit" class="btn btn-success" value="Save & Exit" onClick="window.location.href = 'StoreMain.aspx'" id="genPO"> <input type="submit" class="btn btn-danger" value="Delete" id="delPO">
		   <input type="submit" class="btn btn-default" id="backPO" value="Back" onClick="StoreMain.aspx">
                
                
            </td>
        </tr>
		<tr><td height="5"></td></tr>
    </tbody></table>
     <hr>
    
    <br />

     
    
 
            
            
                
                 
               
       
    
   <table cellspacing="5" cellpadding="5" border="0" style="margin-left:10px;" class="formTbl" id="vendorSelect" width="">

       <tr>
           <td><label for="MainContent_RequestTo" >Select Warehouse</label></td>
           <td>

                <select>	
                	<option>----Select Warehouse</option>
                    <option>My Warehouse</option>
                </select>
               

           </td>
           <td>
           	 <input type="submit" class="btn btn-primary btn-sm continue1"value="Continue" />
           </td>
           </tr>
    </table>
   
    <table cellspacing="5" cellpadding="5" border="0" style="margin-left:5px;" class="formTbl" id="productAdd" width="100%">

       <tr>
           <td><label>Select Product</label></td>
           <td>

                <input type="text" class="product" />
                <input type="submit" class="search-btn getProducts">
              

           </td>
           <td>
           		<label>Quantity</label>
           	</td>
            <td><input style="width:60px" type="text"></td>
            
            <td>
           		<label>Bonus Quantity</label>
           	</td>
            <td><input style="width:60px" type="text"></td>
            <td><input type="submit" value="Add" class="btn btn-primary btn-sm" id="adddProduct"></td>
            <td align="right" width="25%"></td>
           </tr>
    </table>
          
     
     
     
     
     
     
     <br>
     
     <div class="po-tbl" id="StockGrid" > 
     <table class="table table-striped table-bordered table-condensed tblBtm" width="100%">
		<tbody><tr>
			<th>Action</th>
            <th>Product Description</th>
            <th>Quantity</th>
            <th>Bonus Qty</th>
            <th>Unit Cost Price</th>
            <th>Total Cost Price</th>
            <th>Order Status</th>
		</tr>
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
        
        <tr>
			<td>
                            <input type="submit" value="Edit" id="editField" class="btn btn-default edit-btn editPO">
                            
                                <input type="submit" value="Delete" onclick="return confirm('Are you sure you want to delete this record?')" class="btn btn-default del-btn">
                           
                        </td><td>PANADOL ADVANCE 500MG
                        </td><td>50
                        </td><td>2
                        </td><td>9.77
                    </td><td>488.5
                    </td><td>Pending
                        </td>
		</tr>
		
		
	</tbody></table>
    </div>
            
           
           
           
           <table class="table table-striped table-bordered table-condensed" cellspacing="0" id="StockGridEdit">
		<tbody><tr>
			<th>Action</th>
            <th>Product Description</th>
            <th>Quantity</th>
            <th>Bonus Qty</th>
            <th>Unit Cost Price</th>
            <th>Total Cost Price</th>
            <th>Order Status</th>
		</tr><tr>
			<td>
                                                   <a id="cancelEditd" class="btn btn-primary btn-xs" href='#'>Update</a>
                            
                            <a class="btn btn-default btn-xs" href="#" id="cancelEdit">Cancel</a>
                      
                            
                        </td><td align="left">
                            PANADOL ADVANCE 500MG
                        </td><td>
                            <input type="text" value="50" class="grid-input-form">
                        </td><td>
                            <input type="text" value="2" class="grid-input-form">
                        </td><td>
                        9.77
                    </td><td>
                        488.5
                    </td><td>
                            Pending
                        </td>
		</tr>
	</tbody></table>
                
             
	<img src="images/po-img.png" width="344" height="344" class="poImg">
    
            
          
                
            

   
                
         
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

