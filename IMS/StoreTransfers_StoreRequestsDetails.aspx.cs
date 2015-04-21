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
    public partial class StoreTransfers_StoreRequestsDetails : System.Web.UI.Page
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
            RequestFrom.Text = Session["RequestedFrom"].ToString();
            RequestDate.Text = Session["RequestedDate"].ToString();
            #region Display Products
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetStoreRequest_byOrderID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                DataSet ds = new DataSet();

                if (int.TryParse(Session["RequestedNO"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }

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
        protected void btnResponse_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreTransfers_StoreRequestsResponse.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreTransfers_StoreRequests.aspx");
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}