<%@ Page Language="C#" AutoEventWireup="true" CodeBehind="Notification_IndirectPO.aspx.cs" Inherits="IMS.Notification_IndirectPO" %>

<!DOCTYPE html>

<html xmlns="http://www.w3.org/1999/xhtml">
<head runat="server">
    <title></title>
</head>
<body>
    <form id="form1" runat="server">
    <div class="notifications">
        <asp:Repeater runat="server" ID="NewNotification" OnDataBinding="NewNotification_DataBinding">

            <ItemTemplate>

               
                    
                    <div class="notify gr" onclick="window.location.href='ViewIndirectPOWarehouse.aspx'">
                            <div style="display:none" id="OrderID"><%#Eval("OrderID") %></div>
                            <div class="circle s">
                              <svg class="svg-light" viewBox="0 0 512 512" xmlns="http://www.w3.org/2000/svg">
                                    <g class="kokokello" >
	                                    <g>
		                                    <rect x="182.508" y="-46.756" fill="none" width="146.983" height="388.404"/>
		                                    <path class="heilu" d="M182.508,341.648c-19.196,0-15.356-22.486-12.066-25.777c3.291-3.291,2.396-58.963,9.828-97.075
			                                    c7.432-38.112,49.958-57.82,59.669-63.422c0-5.976,4.855-17.928,16.061-17.928c11.205,0,16.061,11.952,16.061,17.928
			                                    c9.711,5.603,52.237,25.311,59.669,63.422s6.538,93.784,9.828,97.075c3.291,3.291,7.13,25.777-12.066,25.777H182.508z"/>
	                                    </g>
	                                    <path class="kili" d="M221.996,354.81c0,10.904-8.84,19.744-19.744,19.744s-19.744-8.84-19.744-19.744H221.996z"/>
                                    </g>
                        </svg>
                            </div>
                               

                                <div class="info">
                                    <span>PO Request</span>
                                    <span>From: <%#Eval("Requester") %></span><br />
                                    <span>To Vendor: <%#Eval("RequestFrom") %></span>
                                </div>
                        
                                
                         
                          </div>  

                
               
                   
                
                

                    
  
                    

            </ItemTemplate>


        </asp:Repeater>
        </div> 

        <div class="Transfers">
            <asp:Repeater runat="server" ID="AllPendingRequests">

                <ItemTemplate>

               
                     <div class="TransferCount"><%#Eval("TransferCount") %></div>
                    <div class="Method"><%#Eval("Method") %></div>
                   
                </ItemTemplate>

            </asp:Repeater>
        </div>
    </form>
</body>
</html>
