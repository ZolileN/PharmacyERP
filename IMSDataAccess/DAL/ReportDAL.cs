using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace IMSDataAccess
{
    public class ReportDAL : DataAccessbase
    {
        public ReportDAL() { }
        public DataSet rpt_InventoryListDetailsReport(int DepartmentID, int CategoryID, int subCategoryID, string ProductName)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_rptInventoryListDetailsReport.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_DeptID", DepartmentID), 
                                            new SqlParameter("@p_CatID", CategoryID), 
                                            new SqlParameter("@p_subCatID", subCategoryID), 
                                            new SqlParameter("@p_prodName", ProductName), 
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }


        public DataSet rpt_InventorySummaryReport(int DepartmentID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_rptInventorySummaryReport.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_DeptID", DepartmentID), 
                                                                                   };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        
    }
}
