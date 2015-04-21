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

namespace IMS
{
    public partial class StoreTransfers_StoreRequests : System.Web.UI.Page
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
                SqlCommand command = new SqlCommand("sp_GetPendingStoreTransfers_SysID", connection); // change SP name and add parameter
                command.CommandType = CommandType.StoredProcedure;
                int UserSys = 0;
                if (int.TryParse(Session["UserSys"].ToString(), out UserSys))
                {
                    command.Parameters.AddWithValue("@p_SysID", UserSys);

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
        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
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
                    Session["RequestedNO"] = RequestNo.Text.ToString();
                    Session["RequestedFrom"] = RequestFrom.Text.ToString();
                    Session["RequestedDate"] = RequestDate.Text.ToString();
                    Response.Redirect("StoreTransfers_StoreRequestsDetails.aspx"); //change Page Name
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
            LoadData();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            LoadData();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreMain.aspx");
        }
    }
}