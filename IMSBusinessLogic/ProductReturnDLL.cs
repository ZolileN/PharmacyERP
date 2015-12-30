using IMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class ProductReturnDLL : DataAccessbase
    {
       


        public DataSet GetStockByName(string ProductName, int SysID, bool isStore)
        {
            StoredProcedureName = StoredProcedure.Select.Sp_GetStockByName.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_prodName", ProductName), 
                                             new SqlParameter("@p_SysID", SysID), 
                                             new SqlParameter("@p_isStore", isStore), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }

        public DataSet GetStockBatches(long ProductID, int SysID)
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetStockBatches.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@ProductID", ProductID), 
                                             new SqlParameter("@StoredAt", SysID) 
                                           
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }


        public DataSet GenerateItemReturn(long VendorID, long PharmaIDStoreID, Int32 UserID, string ReturnMode, DataTable TableVar)
        {
            
            StoredProcedureName = StoredProcedure.Insert.SP_GenerateItemReturn.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@VendorID", VendorID), 
                                            new SqlParameter("@PharmaIDStoreID", PharmaIDStoreID), 
                                            new SqlParameter("@UserID", UserID),  
                                            new SqlParameter("@ReturnMode", ReturnMode), 
                                            new SqlParameter("@TableVar", TableVar),  
                                               
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return  dbHelper.Run(base.ConnectionString, parameters);
           
        }

        public DataSet GetItemReturns(long ReturnID)
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetItemReturns.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@ReturnID", ReturnID)
                                          
                                           
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }
        public DataSet GetItemReturnsDetails(long ReturnID)
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetItemReturnsDetails.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@ReturnID", ReturnID)
                                          
                                           
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }
    }
}
