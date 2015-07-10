using AjaxControlToolkit;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;

namespace IMS
{
    public partial class CrystalReportViewer : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static ReportDocument myReportDocument = new ReportDocument();
        
        protected void Page_Init(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["ReportDocument"];
                CrystalReportViewer1.DisplayToolbar = true;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (IsPostBack)
            {
                CrystalReportViewer1.ReportSource = (ReportDocument)Session["ReportDocument"];
                CrystalReportViewer1.DisplayToolbar = true;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;
            }

            //CrystalReportViewer1.ReportSource = myReportDocument;
        }

        public void DisplayMainGrid(DataTable dt)
        {
            try
            {
                DataSet ds = new DataSet();
                ds.Tables.Add((DataTable)Session["dtItemPurchased"]);
                ds.AcceptChanges();

                myReportDocument.Load(Server.MapPath("~/ItemPurchaseDetailReport.rpt"));
                App_Code.Barcode dsReport = new App_Code.Barcode();
                dsReport.Tables[0].Merge((DataTable)Session["dtItemPurchased"]);
                myReportDocument.SetDataSource(dsReport.Tables[0]);

                CrystalReportViewer1.ReportSource = myReportDocument;
                CrystalReportViewer1.DisplayToolbar = true;
                CrystalReportViewer1.HasToggleGroupTreeButton = false;
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None;

                ViewState["ReportDoc"] = myReportDocument;
                Session["ReportDoc_Printing"] = myReportDocument;
                Session["ReportPrinting_Redirection"] = "rpt_ItemPurchase_Selection.aspx";

            }
            catch (Exception ex)
            {

            }

            DataTable displayTable = new DataTable();
            displayTable.Clear();
            displayTable.Columns.Add("OrderID", typeof(int));
            displayTable.Columns.Add("SupName", typeof(String));
            displayTable.Columns.Add("OrderDate", typeof(String));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(dt.Rows[i]["OrderID"].ToString());
                String Name = dt.Rows[i]["SupName"].ToString();
                String OrderDate = dt.Rows[i]["OrderDate"].ToString();
                displayTable.Rows.Add(ID, Name, OrderDate);
                displayTable.AcceptChanges();
            }

            DataView dv = displayTable.DefaultView;
            displayTable = null;
            displayTable = dv.ToTable(true, "OrderID", "SupName", "OrderDate");

            //gdvSalesSummary.DataSource = null;
           // gdvSalesSummary.DataSource = displayTable;
          //  gdvSalesSummary.DataBind();

        }
        

        public string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }

        public string GetDefaultprinterName()
        {
            throw new NotImplementedException();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect(Session["ReportPrinting_Redirection"].ToString());
        }
    }
}