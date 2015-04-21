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
    public partial class WarehouseSalesOrder : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FirstOrder = false;
                systemSet = new DataSet();
                ProductSet = new DataSet();
                LoadData();

            }
        }

        private void LoadData()
        {
            #region Getting Systems
            try
            {
                connection.Open();
                DataSet ds = new DataSet();
                SqlCommand command = new SqlCommand("sp_GetSystemsForStore", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_UserSys", Convert.ToInt32(Session["UserSys"].ToString()));
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                RequestTo.DataSource = ds.Tables[0];
                RequestTo.DataTextField = "SystemName";
                RequestTo.DataValueField = "SystemID";
                RequestTo.DataBind();
                if (RequestTo != null)
                {
                    RequestTo.Items.Insert(0, "Select System");
                    RequestTo.SelectedIndex = 0;
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
        protected void RequestTo_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {

        }

        protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {

        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {

        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {

        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}