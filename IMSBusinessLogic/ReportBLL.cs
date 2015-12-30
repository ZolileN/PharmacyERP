using IMSDataAccess;
using System;
using System.Data;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IMSBusinessLogic
{
    public class ReportBLL
    {
        ReportDAL reportdal = null;
        public ReportBLL() {
            reportdal = new ReportDAL();
        }

        public DataSet rpt_InventoryListDetailsReport(int DepartmentID, int CategoryID, int subCategoryID, string ProductName)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = reportdal.rpt_InventoryListDetailsReport(DepartmentID, CategoryID, subCategoryID, ProductName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }


        public DataSet rpt_InventorySummaryReport(int DepartmentID)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = reportdal.rpt_InventorySummaryReport(DepartmentID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }



        public DataSet rpt_InventoryAdjustmentReport(int DepartmentID, int CategoryID, int subCategoryID, string ProductName,
            DateTime From, DateTime To, int FilterBy, int SystemID)
		
		{
            DataSet dsResults = new DataSet();
            try
            {

                dsResults = reportdal.rpt_InventoryAdjustmentReport(DepartmentID, CategoryID, subCategoryID, ProductName,
            From, To, FilterBy, SystemID);

                

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet rpt_HaadNonHaadMedicinesList(int DeptID, int? CatID, int? Sub_CategoryID)

        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = reportdal.rpt_HaadNonHaadMedicinesList(DeptID, CatID, Sub_CategoryID);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }



        public DataSet rpt_InventoryReportByVendor(int Vendor)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = reportdal.rpt_InventoryReportByVendor(Vendor);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        


    }
}
