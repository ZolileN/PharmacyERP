using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IMSDataAccess.DAL
{
    public class NotificationDAL : DataAccessbase
    {
        public NotificationDAL() { }

        public void SetSeen(int TransferID)
        {
            StoredProcedureName = StoredProcedure.Update.sp_SetSeen.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@p_TransferID", TransferID)};

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public DataSet getAllPendingTransferRequests(int StoreID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_GetAllPendingTransferRequests.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_StoreID", StoreID), 
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public DataSet getNewNotification(int StoreID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_GetNewNotificationTransferRequest.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_StoreID", StoreID), 
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

    }
}
