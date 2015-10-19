using IMSBusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;

// --- Created By Shahid
// --- 13-10-2015

namespace IMS
{
    public partial class Notification : System.Web.UI.Page
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

            if (StoreID > 0 && (!method.Equals("") || method!=null)) {

            

                if (method.Equals("NewNotificationTR")) {//Get A New Notification of Transfer Request
                    
                    DataSet ds = NotifBLL.getNewNotification(StoreID);
                    NewNotification.DataSource = ds;
                    NewNotification.DataBind();
                }
                
                else if (method.Equals("AllPendingTR")) { //Get All Pending Transfer Requests
                    
                    DataSet ds = NotifBLL.getAllPendingTransferRequests(StoreID);
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


            }


        }

        protected void NewNotification_DataBinding(object sender, EventArgs e)
        {

        }
    }
}