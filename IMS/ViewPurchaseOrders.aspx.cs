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
    public partial class ViewPurchaseOrders : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            #region Display Requests
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetPendingPOs", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_RequestedByID", Session["UserSys"]);
                if (StockAt.SelectedIndex > 0)
                {
                    command.Parameters.AddWithValue("@p_RequestedForID", Convert.ToInt32(StockAt.SelectedValue.ToString()));

                }
                else
                {
                    command.Parameters.AddWithValue("@p_RequestedForID", DBNull.Value);

                }
                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
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
            LoadData();
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            LoadData();
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
                    Label RequesteeRole = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblSysRole");
                    Label RequesteeID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedFromID");
                    Session["RequestedNO"] = RequestNo.Text.ToString();
                    Session["RequestedFrom"] = RequestFrom.Text.ToString();
                    Session["RequestedDate"] = RequestDate.Text.ToString();
                    Session["RequestDesRole"] = RequesteeRole.Text.ToString();
                    Session["RequestDesID"] = RequesteeID.Text.ToString();
                    Response.Redirect("AcceptPurchaseOrder.aspx");
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageOrders.aspx");
        }
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
            //DataView dv = ProductSet.Tables[0].DefaultView;
            //dv.RowFilter = "SuppID = " + StockAt.SelectedValue;
            //DataTable dt = dv.ToTable();
            //StockDisplayGrid.DataSource = null;
            //StockDisplayGrid.DataSource = dt;
            //StockDisplayGrid.DataBind();
            // }
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (SelectProduct.Text.Length >= 3)
            {
                PopulateDropDown(SelectProduct.Text);
                StockAt.Visible = true;
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("Select * From tblVendor Where tblVendor.SupName LIKE '" + Text + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (StockAt.DataSource != null)
                {
                    StockAt.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                StockAt.DataSource = ds.Tables[0];
                StockAt.DataTextField = "SupName";
                StockAt.DataValueField = "SuppID";
                StockAt.DataBind();
                if (StockAt != null)
                {
                    StockAt.Items.Insert(0, "Select Vendor");
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
        }
    }
}