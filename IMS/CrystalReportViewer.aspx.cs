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
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadData();
                //ViewState["CustomerID"] = 0;
                DisplayMainGrid((DataTable)Session["dtItemPurchased"]);

            }
        }

        public void DisplayMainGrid(DataTable dt)
        {
            DataSet ds = new DataSet();
            ds.Tables.Add(dt);
            ds.AcceptChanges();
            try
            {


                //ItemPurchase myReportDocument = new ItemPurchase();
                ReportDocument myReportDocument = new ReportDocument();
                myReportDocument.Load(Server.MapPath("~/ItemPurchaseDetailReport.rpt"));
                myReportDocument.SetDatabaseLogon("sahmed", "dps123!@#");
                App_Code.Barcode dsReport = new App_Code.Barcode();
                dsReport.Tables[0].Merge(dt);
                myReportDocument.SetDataSource(dsReport.Tables[0]);

                //CrystalReportSource1.ReportDocument.SetDataSource(dt);

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
        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID, SalesID;
            ProdID = DeptID = CatID = SubCatID = CustID = SalesID = 0;
            String BarterValue = "";
            DateTime Expiry = new DateTime();
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("sp_rpt_ItemPurchase", connection);
                command.CommandType = CommandType.StoredProcedure;

                #region Applying Filters

                BarterValue = Session["rptBarterCustomers"].ToString();

                if (Session["rptCustomerID"] != null && Session["rptCustomerID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCustomerID"].ToString(), out CustID))
                    {

                    }
                }

                if (Session["rptDepartmentID"] != null && Session["rptDepartmentID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptDepartmentID"].ToString(), out DeptID))
                    {

                    }
                }

                if (Session["rptCategoryID"] != null && Session["rptCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCategoryID"].ToString(), out CatID))
                    {

                    }
                }

                if (Session["rptSubCategoryID"] != null && Session["rptSubCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptSubCategoryID"].ToString(), out SubCatID))
                    {

                    }
                }


                if (Session["rptProductID"] != null && Session["rptProductID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptProductID"].ToString(), out ProdID))
                    {

                    }
                }

                if (Session["rptItemPurchaseExpiry"] != null && Session["rptItemPurchaseExpiry"].ToString() != "")
                {

                    Expiry = Convert.ToDateTime(Session["rptItemPurchaseExpiry"].ToString());
                }


                #endregion

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                if ((Session["rptItemPurchaseDateF"] != null && Session["rptItemPurchaseDateF"].ToString() != "") &&
                    (Session["rptItemPurchaseDateT"] != null && Session["rptItemPurchaseDateT"].ToString() != ""))
                {
                    DateTime dtFROM = Convert.ToDateTime(Session["rptItemPurchaseDateF"].ToString());
                    DateTime dtTo = Convert.ToDateTime(Session["rptItemPurchaseDateT"].ToString());

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "OrderDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND OrderDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";

                    DataTable dtfilterSet = dv.ToTable();
                    dv = dtfilterSet.DefaultView;

                    if (CustID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "OrderRequestedFor = '" + CustID + "'";
                        dtfilterSet = dv.ToTable();

                    }

                    if (BarterValue.Equals("Exclude"))
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "BarterExchangeID IS NULL";
                        dtfilterSet = dv.ToTable();
                    }


                    if (DeptID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "DeptID = '" + DeptID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (CatID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "CatID = '" + CatID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (SubCatID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "SubCategoryID = '" + SubCatID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (ProdID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "ProductID = '" + ProdID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (Expiry != null && (Session["rptItemPurchaseExpiry"] != null && Session["rptItemPurchaseExpiry"].ToString() != ""))
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "ExpiryDate = '" + Expiry.ToString("yyyy-MM-dd") + "'";
                        dtfilterSet = dv.ToTable();
                    }


                    DataTable dtFiltered = dtfilterSet;
                    Session["dtItemPurchased"] = dtFiltered;
                }
                else
                {
                    Session["dtItemPurchased"] = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

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
    }
}