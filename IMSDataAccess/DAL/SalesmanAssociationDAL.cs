using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class SalesmanAssociationDAL :DataAccessbase
    {
        public SalesmanAssociationDAL() 
        {
        }

        #region select
       

        public System.Data.DataSet SelectUnAssociatedStores(long userID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetUnAssociatedStores.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public DataSet SelectAssociatedStores(long userID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetAssociatedStores.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }
        #endregion

        #region Delete
        public void Delete(long userID)
        {
            String StoredProcedureName = StoredProcedure.Delete.Sp_DeleteSalesmanSystem.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);

        } 
        #endregion

        public void Insert(long userID, long SystemID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddSalesmanSystems.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID),
                                            new SqlParameter("@p_systemID", SystemID)
                                            
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }
    }
}
