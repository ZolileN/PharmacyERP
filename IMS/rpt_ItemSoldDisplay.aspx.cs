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
    public partial class rpt_ItemSoldDisplay : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                LoadData();
                //ViewState["CustomerID"] = 0;
                DisplayMainGrid((DataTable)Session["dtItemSoldALL"]);

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
                if((Session["rptProductID"]!=null && Session["rptProductID"].ToString()!="") ||
                   (Session["rptSubCategoryID"] != null && Session["rptSubCategoryID"].ToString() != "") ||
                   (Session["rptCategoryID"] != null && Session["rptCategoryID"].ToString() != "") ||
                   (Session["rptDepartmentID"] != null && Session["rptDepartmentID"].ToString() != "") ||
                   (Session["rptCustomerID"] != null && Session["rptCustomerID"].ToString() != "") ||
                   (Session["rptSalesManID"] != null && Session["rptSalesManID"].ToString() != ""))
                {
                    #region With Filters
                    SqlCommand command = new SqlCommand("sp_rpt_ItemSold_fullFilter", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    #region Product Parameters
                    if (Session["rptProductID"]!=null && Session["rptProductID"].ToString()!="")
                    {
                        if(int.TryParse(Session["rptProductID"].ToString(),out ProdID))
                        {
                            command.Parameters.AddWithValue("@p_ProdID", ProdID);
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
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

                    #region SalesMan Parameters
                    if (Session["rptSalesManID"] != null && Session["rptSalesManID"].ToString() != "")
                    {
                        if (int.TryParse(Session["rptSalesManID"].ToString(), out SalesID))
                        {
                            command.Parameters.AddWithValue("@SalesManID", SalesID);
                        }
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@SalesManID", DBNull.Value);
                    }
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
                        dv.RowFilter = "OrderDate >= '" + dtFROM + "' AND OrderDate <= '" + dtTo + "'";

                        DataTable dtFiltered = dv.ToTable();
                        Session["dtItemSoldALL"] = dtFiltered;
                    }
                    else
                    {
                        Session["dtItemSoldALL"] = ds.Tables[0];
                    }
                    #endregion
                }
                else
                {
                    #region Without Filters
                    SqlCommand command = new SqlCommand("sp_rpt_ItemSold", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    
                    DataSet ds = new DataSet();
                    SqlDataAdapter dA = new SqlDataAdapter(command);
                    dA.Fill(ds);

                    if (Session["rptSalesDateFrom"] != null && Session["rptSalesDateFrom"].ToString() != "" &&
                        Session["rptSalesDateTo"] != null && Session["rptSalesDateTo"].ToString() != "")
                    {
                        DateTime dtFROM = Convert.ToDateTime(Session["rptSalesDateFrom"]);
                        DateTime dtTo = Convert.ToDateTime(Session["rptSalesDateTo"]);

                        DataView dv = ds.Tables[0].DefaultView;
                        dv.RowFilter = "OrderDate >= '" + dtFROM + "' AND OrderDate <= '" + dtTo + "'";

                        DataTable dtFiltered = dv.ToTable();
                        Session["dtItemSoldALL"] = dtFiltered;
                    }
                    else
                    {
                        Session["dtItemSoldALL"] = ds.Tables[0];
                    }
                    #endregion
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

            Session["rptSalesDateFrom"] = null;

            Session["rptSalesDateTo"] = null;

            Response.Redirect("rpt_ItemSold_Selection.aspx");
        }

    }    
}