using IMSDataAccess.DAL;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class NotificationBLL
    {
        NotificationDAL notificationdal = null;

        public NotificationBLL() {
            notificationdal = new NotificationDAL();
        }

        public void SetSeen(int TransferID) {

            notificationdal.SetSeen(TransferID);
        }

        public DataSet getNewNotification(int StoreID)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = notificationdal.getNewNotification(StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet getAllPendingTransferRequests(int StoreID)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = notificationdal.getAllPendingTransferRequests(StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }



        public void SetSeenIndirectPO(int OrderID)
        {

            notificationdal.SetSeenIndirectPO(OrderID);
        }

        public DataSet sp_GetAllPendingIndirectPO(int StoreID)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = notificationdal.GetAllPendingIndirectPO(StoreID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet GetNewNotificationIndirectPO()
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = notificationdal.GetNewNotificationIndirectPO();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

    }
}
