using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class rpt_ItemSoldDisplay_byDate : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadData();
                //ViewState["CustomerID"] = 0;
                DisplayMainGrid((DataTable)Session["dtItemSoldDate"]);
                lblDateFrom.Text = Session["rptItemSoldDateFrom"].ToString();
                lblDateTo.Text = Session["rptItemSoldDateTo"].ToString();
                lblCustomers.Text = Session["selectionCustomers"].ToString();
                lblDepartment.Text = Session["selectionDepartment"].ToString();
                lblCategory.Text = Session["selectionCategory"].ToString();
                lblSubCategory.Text = Session["selectionSubCategory"].ToString();
                lblProduct.Text = Session["selectionProduct"].ToString();
                lblInternalCustomer.Text = Session["rptInternalCustomers"].ToString();
                lblBarterCustomer.Text = Session["rptBarterCustomers"].ToString();

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
                myReportDocument.Load(Server.MapPath("~/ItemSaleDetailReport.rpt"));
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
                Session["ReportPrinting_Redirection"] = "rpt_ItemSold_Selection.aspx";
                
            }
            catch (Exception ex)
            {

            }

            DataTable displayTable = new DataTable();
            displayTable.Clear();
            displayTable.Columns.Add("OrderID", typeof(int));
            displayTable.Columns.Add("SystemName", typeof(String));
            displayTable.Columns.Add("OrderDate", typeof(String));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int ID = Convert.ToInt32(dt.Rows[i]["OrderID"].ToString());
                String Name = dt.Rows[i]["SystemName"].ToString();
                String OrderDate = dt.Rows[i]["OrderDate"].ToString();
                displayTable.Rows.Add(ID, Name,OrderDate);
                displayTable.AcceptChanges();
            }

            DataView dv = displayTable.DefaultView;
            displayTable = null;
            displayTable = dv.ToTable(true, "OrderID", "SystemName", "OrderDate");

            gdvSalesSummary.DataSource = null;
            gdvSalesSummary.DataSource = displayTable;
            gdvSalesSummary.DataBind();
        }
        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID, SalesID;
            ProdID = DeptID = CatID = SubCatID = CustID = SalesID = 0;
            String BarterValue = "";
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("sp_rpt_ItemSold", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InternalCustomer", Session["rptInternalCustomers"].ToString());
                
                #region Applying Filters

                if (Session["rptSalesManID"] != null && Session["rptSalesManID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptSalesManID"].ToString(), out SalesID))
                    {

                    }
                }

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

                BarterValue = Session["rptBarterCustomers"].ToString();
                #endregion

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                if (Session["rptItemSoldDateFrom"] != null && Session["rptItemSoldDateFrom"].ToString() != "" &&
                    Session["rptItemSoldDateTo"] != null && Session["rptItemSoldDateTo"].ToString() != "")
                {
                    DateTime dtFROM = Convert.ToDateTime(Session["rptItemSoldDateFrom"]);
                    DateTime dtTo = Convert.ToDateTime(Session["rptItemSoldDateTo"]);

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "OrderDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND OrderDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";
                    DataTable dtfilterSet = dv.ToTable();
                    dv = dtfilterSet.DefaultView;

                    if (SalesID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "SalesMan = '" + SalesID + "'";
                        dtfilterSet = dv.ToTable();
                    }

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



                    DataTable dtFiltered = dv.ToTable();
                    Session["dtItemSoldDate"] = dtFiltered;
                }
                else
                {
                    Session["dtItemSoldDate"] = ds.Tables[0];
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
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session["rptProductID"] = null;

            Session["rptSubCategoryID"] = null;

            Session["rptCategoryID"] = null;

            Session["rptDepartmentID"] = null;

            Session["rptCustomerID"] = null;

            Session["rptBarterCustomers"] = null;

           // Session["rptSalesDateFrom"] = null;

          //  Session["rptSalesDateTo"] = null;

            Response.Redirect("rpt_ItemSold_Selection.aspx");
        }

        protected void gdvSalesSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    GridView gvMAinDisplay = (GridView)e.Row.FindControl("gvMAinGrid");
                    GridView gdvTotal = (GridView)e.Row.FindControl("gdvTotal");

                    DataTable dt = (DataTable)Session["dtItemSoldDate"];
                    DataView dv = dt.DefaultView;
                    int OrderID = 0;
                    Label lblOrderID = (Label)e.Row.FindControl("lblSO");
                    int.TryParse(lblOrderID.Text.ToString(), out OrderID);
                    dv.RowFilter = "OrderID = '"+ OrderID +"'";

                    dt = null;
                    dt = dv.ToTable();

                    DataTable displayTable = new DataTable();
                    displayTable.Clear();
                    displayTable.Columns.Add("TotalSendQaun", typeof(Decimal));
                    displayTable.Columns.Add("TotalBonusQuan", typeof(Decimal));
                    displayTable.Columns.Add("TotalSoldQuan", typeof(Decimal));
                    displayTable.Columns.Add("TotalSoldBonus", typeof(Decimal));
                    displayTable.Columns.Add("TotalCostPrice", typeof(Decimal));

                    Decimal Accepted, Bonus, Sold, SoldBonus, Price;
                    Accepted = Bonus = Sold = SoldBonus = Price = 0;

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        Accepted += Decimal.Parse(dt.Rows[i]["SendQuantity"].ToString());
                        Bonus += Decimal.Parse(dt.Rows[i]["BonusQuantity"].ToString());
                        Sold += Decimal.Parse(dt.Rows[i]["RecievedQuantity"].ToString());
                        SoldBonus += Decimal.Parse(dt.Rows[i]["RecievedBonusQuantity"].ToString());
                        Price += Decimal.Parse(dt.Rows[i]["TotalCostPrice"].ToString());

                    }

                    displayTable.Rows.Add(Accepted, Bonus, Sold, SoldBonus, Price);
                    displayTable.AcceptChanges();


                    gdvTotal.DataSource = displayTable;
                    gdvTotal.DataBind();

                    gvMAinDisplay.DataSource = dt;
                    gvMAinDisplay.DataBind();
                }
            }
            catch(Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            ReportDocument doc = new ReportDocument();
            doc = (ReportDocument)ViewState["ReportDoc"];
            doc.PrintOptions.PrinterName = GetDefaultPrinter();
            doc.PrintToPrinter(1, false, 0, 0);
        }
    }
}