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

        
    }
}
