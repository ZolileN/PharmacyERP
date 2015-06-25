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
    public partial class rpt_ItemSoldDisplay_bySalesMan : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadData();
                //ViewState["CustomerID"] = 0;
                DisplayMainGrid((DataTable)Session["dtItemSoldSalesMan"]);
                lblDateFrom.Text = Session["rptItemSoldDateFrom"].ToString();
                lblDateTo.Text = Session["rptItemSoldDateTo"].ToString();
                lblCustomers.Text = Session["selectionCustomers"].ToString();
                lblDepartment.Text = Session["selectionDepartment"].ToString();
                lblCategory.Text = Session["selectionCategory"].ToString();
                lblSubCategory.Text = Session["selectionSubCategory"].ToString();
                lblProduct.Text = Session["selectionProduct"].ToString();
                lblInternalCustomer.Text = Session["rptInternalCustomers"].ToString();

            }
        }

        public void DisplayMainGrid(DataTable dt)
        {
            DataTable displayTable = new DataTable();
            displayTable.Clear();
            displayTable = dt;

            gvMAinGrid.DataSource = null;
            gvMAinGrid.DataSource = displayTable;
            gvMAinGrid.DataBind();
        }
        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID, SalesID;
            ProdID = DeptID = CatID = SubCatID = CustID = SalesID = 0;
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("sp_rpt_ItemSold", connection);
                command.CommandType = CommandType.StoredProcedure;

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
                    dv.RowFilter = "OrderDate >= '" + dtFROM + "' AND OrderDate <= '" + dtTo + "'";

                    if (SalesID != 0)
                    {
                        dv.RowFilter = "SalesMan = '" + SalesID + "'";
                    }
                    if (CustID != 0)
                    {
                        dv.RowFilter = "OrderRequestedFor = '" + CustID + "'";
                    }
                    if (DeptID != 0)
                    {
                        dv.RowFilter = "DeptID = '" + DeptID + "'";
                    }
                    if (CatID != 0)
                    {
                        dv.RowFilter = "CatID = '" + CatID + "'";
                    }
                    if (SubCatID != 0)
                    {
                        dv.RowFilter = "SubCategoryID = '" + SubCatID + "'";
                    }
                    if (ProdID != 0)
                    {
                        dv.RowFilter = "ProductID = '" + ProdID + "'";
                    }



                    DataTable dtFiltered = dv.ToTable();
                    Session["dtItemSoldSalesMan"] = dtFiltered;
                }
                else
                {
                    Session["dtItemSoldSalesMan"] = ds.Tables[0];
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
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session["rptProductID"] = null;

            Session["rptSubCategoryID"] = null;

            Session["rptCategoryID"] = null;

            Session["rptDepartmentID"] = null;

            Session["rptCustomerID"] = null;

            //Session["rptSalesDateFrom"] = null;

            //Session["rptSalesDateTo"] = null;

            Response.Redirect("rpt_ItemSold_Selection.aspx");
        }
    }
}