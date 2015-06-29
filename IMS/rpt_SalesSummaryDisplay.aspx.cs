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

namespace IMS
{
    public partial class rpt_SalesSummaryDisplay : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                LoadData();
                ViewState["CustomerID"] = 0;
                DisplayMainGrid((DataTable)Session["dtSalesSummary"]);
                lblDateFrom.Text = Session["rptSalesDateFrom"].ToString();
                lblDateTo.Text =  Session["rptSalesDateTo"].ToString();
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
            DataTable displayTable = new DataTable();
            displayTable.Clear();
            displayTable.Columns.Add("CustomerID", typeof(int));
            displayTable.Columns.Add("CustomerName", typeof(String));

            for (int i = 0; i < dt.Rows.Count; i++)
            {
                int CustomerID = Convert.ToInt32(dt.Rows[i]["OrderRequestedFor"].ToString());
                String CustomerName = dt.Rows[i]["SystemName"].ToString();
                displayTable.Rows.Add(CustomerID, CustomerName);
                displayTable.AcceptChanges();
            }

            DataView dv = displayTable.DefaultView;
            displayTable = null;
            displayTable = dv.ToTable(true, "CustomerID", "CustomerName");

            gdvSalesSummary.DataSource = null;
            gdvSalesSummary.DataSource = displayTable;
            gdvSalesSummary.DataBind();
        }
        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID;
            String BarterValue = "";
            ProdID = DeptID = CatID = SubCatID = CustID = 0;
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_rptSalesSummary", connection);
                command.CommandType = CommandType.StoredProcedure;

                #region Product Parameters
                if (Session["rptProductID"]!=null && Session["rptProductID"].ToString()!="")
                {
                    if(int.TryParse(Session["rptProductID"].ToString(),out ProdID))
                    {
                        command.Parameters.AddWithValue("@ProdID", ProdID);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@ProdID", DBNull.Value);
                }
                #endregion

                #region SubCategory Parameters
                if (Session["rptSubCategoryID"] != null && Session["rptSubCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptSubCategoryID"].ToString(), out SubCatID))
                    {
                        command.Parameters.AddWithValue("@SubCatID", SubCatID);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@SubCatID", DBNull.Value);
                }
                #endregion

                #region Category Parameters
                if (Session["rptCategoryID"] != null && Session["rptCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCategoryID"].ToString(), out CatID))
                    {
                        command.Parameters.AddWithValue("@CatID", CatID);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@CatID", DBNull.Value);
                }
                #endregion

                #region Department Parameters
                if (Session["rptDepartmentID"] != null && Session["rptDepartmentID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptDepartmentID"].ToString(), out DeptID))
                    {
                        command.Parameters.AddWithValue("@DeptID", DeptID);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@DeptID", DBNull.Value);
                }
                #endregion

                #region Customer Parameters
                if (Session["rptCustomerID"] != null && Session["rptCustomerID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCustomerID"].ToString(), out CustID))
                    {
                        command.Parameters.AddWithValue("@CustID", CustID);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@CustID", DBNull.Value);
                }
                #endregion

                #region Internal Customer Parameters

                command.Parameters.AddWithValue("@InternalCustomer", Session["rptInternalCustomers"].ToString());
                
                #endregion

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                if (Session["rptSalesDateFrom"] != null && Session["rptSalesDateFrom"].ToString() != "" &&
                    Session["rptSalesDateTo"] != null && Session["rptSalesDateTo"].ToString() != "")
                {
                    DateTime dtFROM = Convert.ToDateTime(Session["rptSalesDateFrom"]);
                    DateTime dtTo = Convert.ToDateTime(Session["rptSalesDateTo"]);

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "OrderDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND OrderDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";

                    if (BarterValue.Equals("Exclude"))
                    {
                        dv.RowFilter = "BarterExchangeID IS NULL OR BarterExchangeID <> ''";
                    }
                    DataTable dtFiltered = dv.ToTable();
                    Session["dtSalesSummary"] = dtFiltered;
                }
                else
                {
                    DataView dv = ds.Tables[0].DefaultView;
                    if (BarterValue.Equals("Exclude"))
                    {
                        dv.RowFilter = "BarterExchangeID IS NULL OR BarterExchangeID <> ''";
                    }
                    DataTable dtFiltered = dv.ToTable();
                    /*if (Session["rptInternalCustomers"].ToString().Equals("Exclude"))
                    {
                        command = new SqlCommand("sp_getInteralCustomers", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        DataSet dsInternal = new DataSet();
                        SqlDataAdapter sAinternal = new SqlDataAdapter(command);
                        sAinternal.Fill(dsInternal);

                        DataView dvInternal = dtFiltered.DefaultView;
                        for (int i = 0; i < dsInternal.Tables[0].Rows.Count; i++)
                        {
                            dvInternal.RowFilter = "OrderRequestedFor != '" + dsInternal.Tables[0].Rows[i][0] + "'";
                        }
                        dtFiltered = dvInternal.ToTable();
                    }*/
                    Session["dtSalesSummary"] = dtFiltered;
                }

            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
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

           // Session["rptSalesDateTo"] = null;

            Response.Redirect("rpt_SalesSummary_Selection.aspx");
        }

        protected void gdvSalesSummary_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int CustID = 0;
                    Label CustomerID = (Label)e.Row.FindControl("lblCustomerID");
                    int.TryParse(CustomerID.Text.ToString(), out CustID);

                    if (ViewState["CustomerID"].Equals(CustID))
                    {

                    }
                    else
                    {

                        GridView gvMAinDisplay = (GridView)e.Row.FindControl("gvMAinGrid");
                        GridView gdvTotal = (GridView)e.Row.FindControl("gdvTotal");

                        DataTable dt = (DataTable)Session["dtSalesSummary"];
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "OrderRequestedFor = '" + CustID + "'";

                        dt = null;
                        dt = dv.ToTable();


                        DataTable displayTable = new DataTable();
                        displayTable.Clear();
                        displayTable.Columns.Add("AcceptedQuantity", typeof(Decimal));
                        displayTable.Columns.Add("BonusQuantity", typeof(Decimal));
                        displayTable.Columns.Add("CostPrice", typeof(Decimal));
                        displayTable.Columns.Add("RealCostPrice", typeof(Decimal));
                        displayTable.Columns.Add("SalePrice", typeof(Decimal));
                        displayTable.Columns.Add("RealSalePrice", typeof(Decimal));
                        displayTable.Columns.Add("Profit", typeof(Decimal));
                        displayTable.Columns.Add("ProfitPercentage", typeof(Decimal));

                        Decimal Accepted, Bonus, CP, RCP, SP, RSP, Profit, ProfitPer;
                        Accepted = Bonus = CP = RCP = SP = RSP = Profit = ProfitPer = 0;

                        for (int i = 0; i < dt.Rows.Count;i++)
                        {
                            Accepted += Decimal.Parse(dt.Rows[i]["AcceptedQuantity"].ToString());
                            Bonus += Decimal.Parse(dt.Rows[i]["BonusQuantity"].ToString());
                            CP += Decimal.Parse(dt.Rows[i]["CostPrice"].ToString());
                            RCP += Decimal.Parse(dt.Rows[i]["RealCostPrice"].ToString());
                            SP += Decimal.Parse(dt.Rows[i]["SalePrice"].ToString());
                            RSP += Decimal.Parse(dt.Rows[i]["RealSalePrice"].ToString());

                        }

                        Profit = (RSP - RCP);
                        ProfitPer = Profit / RCP * 100;

                        Profit = Math.Round(Profit, 2);
                        ProfitPer = Math.Round(ProfitPer, 2);

                        displayTable.Rows.Add(Accepted, Bonus, CP, RCP, SP, RSP, Profit, ProfitPer);
                        displayTable.AcceptChanges();


                        gdvTotal.DataSource = displayTable;
                        gdvTotal.DataBind();

                        gvMAinDisplay.DataSource = dt;
                        gvMAinDisplay.DataBind();

                        ViewState["CustomerID"] = CustID;
                    }

                }
            }
            catch (Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
        }
    }
}