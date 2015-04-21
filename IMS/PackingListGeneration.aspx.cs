using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using IMSCommon.Util;

namespace IMS
{
    public partial class PackingListGeneration : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Populating System Types
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tbl_System WHERE System_RoleID =2;", connection); // needs to be completed
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    StockAt.DataSource = ds.Tables[0];
                    StockAt.DataTextField = "SystemName";
                    StockAt.DataValueField = "SystemID";
                    StockAt.DataBind();
                    if (StockAt != null)
                    {
                        StockAt.Items.Insert(0, "Select System");
                        StockAt.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
                #endregion

                if (StockAt.SelectedIndex == -1)
                {
                    LoadData(null);
                }
                else
                {
                    LoadData(StockAt.SelectedValue);
                }

            }
        }

        public void LoadData(String StoreID)
        {
            #region Display Requests
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetGeneratedOrder_store_warehouse", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_StoreID", StoreID);
                Session["RequestedFromID"] = StoreID;
                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }
        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.Equals("Edit"))
                {
                    int RowNumber = 0;
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label RequestNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedNO");
                    Label RequestFrom = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedFrom");
                    Label RequestDate = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedDate");
                    Session["RequestedNO"] = RequestNo.Text.ToString();
                    Session["RequestedFrom"] = RequestFrom.Text.ToString();
                    
                    Session["RequestedDate"] = RequestDate.Text.ToString();
                    Response.Redirect("ViewPackingList.aspx");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
                Session["SelectedSys"] = StockAt.SelectedValue;
                Session["SelectedSysName"] = StockAt.SelectedItem;
            }

            if (StockDisplayGrid.DataSource != null)
            {
                //btnPackingList.Visible = true;
               // btnPackingList.Enabled = true;
            }
        }

        protected void btnPackingList_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingListGeneration.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }

        protected void StockAt_SelectedIndexChanged1(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
                Session["SelectedSys"] = StockAt.SelectedValue;
                Session["SelectedSysName"] = StockAt.SelectedItem;
            }

            if (StockDisplayGrid.DataSource != null)
            {
                //btnPackingList.Visible = true;
                // btnPackingList.Enabled = true;
            }
        }
    }
}