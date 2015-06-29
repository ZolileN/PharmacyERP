using IMSCommon.Util;
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
    public partial class WarehouseManagement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                DataSet dsResults = new DataSet();
                if(connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_ManageWarehouse_GetWarehouse", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.ExecuteNonQuery();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(dsResults);
                dgvWarehouse.DataSource = dsResults;
                dgvWarehouse.DataBind();

            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        protected void btnAddWH_Click(object sender, EventArgs e)
        {
            Session["Action"] = "Add";
            Session["SysToAdd"] = RoleNames.warehouse;
            Response.Redirect("AddSystem.aspx", false);
        }

        protected void dgvWarehouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            e.NewPageIndex = dgvWarehouse.PageIndex;
            BindGrid();

        }

        protected void dgvWarehouse_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int SystemId;
                Label lblSystemID = (Label)dgvWarehouse.Rows[e.RowIndex].FindControl("lblSystemID");
                int.TryParse(lblSystemID.Text.ToString(),out SystemId);
                SqlCommand command = new SqlCommand("Sp_DeleteSystem", connection);
                command.Parameters.AddWithValue("@p_SystemID", SystemId);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

            }
            catch
            {

            }
            finally
            {
                connection.Close();
                BindGrid();
            }
        }

        protected void dgvWarehouse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if(e.CommandName == "Edit")
            {
                int SystemId;
                Label lblSystemID = (Label)dgvWarehouse.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSystemID");
                int.TryParse(lblSystemID.Text.ToString(), out SystemId);
                Session["SystemId"] = SystemId;
                Session["Action"] = "Edit";
                Session["SysToAdd"] = RoleNames.warehouse;
                Response.Redirect("AddSystem.aspx", false);
            }
        }

        protected void dgvWarehouse_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvWarehouse.EditIndex = e.NewEditIndex;
            BindGrid();
        }
    }
}