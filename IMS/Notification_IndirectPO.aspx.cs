using IMSBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

// --- Created By Shahid
// --- 31-12-2015

namespace IMS
{
    public partial class Notification_IndirectPO : System.Web.UI.Page
    {
        NotificationBLL _notificationbll = null;
        NotificationBLL NotifBLL
        {

            get
            {
                if (_notificationbll == null)
                    _notificationbll = new NotificationBLL();
                return _notificationbll;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            
    
            
            string method = Request.QueryString["Method"];


            int StoreID =0;
            try
            {

                 StoreID = Convert.ToInt32(Session["UserSys"].ToString());

            }
            catch (NullReferenceException ex) { 
                
            }

            if (StoreID > 0 && (method != null || !method.Equals("")))
            {

            
                if (method.Equals("NewNotificationPO"))
                {//Get A New Notification of PO Request
                    
                    DataSet ds = NotifBLL.GetNewNotificationIndirectPO();
                    NewNotification.DataSource = ds;
                    NewNotification.DataBind();
                }
                
                else if (method.Equals("AllPendingPO")) { //Get All Pending PO Requests

                    DataSet ds = NotifBLL.sp_GetAllPendingIndirectPO(StoreID);
                    AllPendingRequests.DataSource = ds;
                    AllPendingRequests.DataBind();
                }
                else if (method.Equals("SetSeen")) {

                    int TID = 0;

                    try
                    {
                        TID = Convert.ToInt32(Request.QueryString["TID"].ToString());
                    }
                    catch (Exception ex) { }

                    if (TID > 0) {
                        NotifBLL.SetSeen(TID);
                    }
                    


                }

                else if (method.Equals("SetSeenPO"))
                {

                    int OrderID = 0;

                    try
                    {
                        OrderID = Convert.ToInt32(Request.QueryString["OrderID"].ToString());
                    }
                    catch (Exception ex) { }

                    if (OrderID > 0)
                    {
                        NotifBLL.SetSeenIndirectPO(OrderID);
                    }



                }


            }


        }

        protected void NewNotification_DataBinding(object sender, EventArgs e)
        {

        }
    }
}