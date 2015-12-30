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



        public DataSet rpt_InventoryAdjustmentReport(int DepartmentID, int CategoryID, int subCategoryID, string ProductName,
            DateTime From, DateTime To, int FilterBy, int SystemID)
        {

            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_rptInventoryAdjustmentReport.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_DeptID", DepartmentID), 
                                            new SqlParameter("@p_CatID", CategoryID), 
                                            new SqlParameter("@p_subCatID", subCategoryID), 
                                            new SqlParameter("@p_prodName", ProductName), 
                                            new SqlParameter("@p_from", From), 
                                            new SqlParameter("@p_to", To), 
                                            new SqlParameter("@p_filterby", FilterBy), 
                                            new SqlParameter("@p_SystemID", SystemID), 
											};
			DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
		}
											
        public DataSet rpt_HaadNonHaadMedicinesList(int DeptID, int? CatID, int? Sub_CategoryID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.SP_Get_HAAD_Medicine_By_Sub_Category.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@DeptID", DeptID),
                                            new SqlParameter("@CatID", CatID),
                                            new SqlParameter("@Sub_CategoryID", Sub_CategoryID),

                                                                                   };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }




        public DataSet rpt_InventoryReportByVendor(int Vendor)
        {

            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_rptInventoryReportByVendorID.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_VendorID", Vendor), 
                                         };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }


        
    }
}
