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
    public partial class PharmacyManagement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            try
            {
                DataSet dsResults = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetStores_Pharmacy", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.ExecuteNonQuery();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(dsResults);
                ViewState["PharmacyResultSet"] = dsResults;
                dgvWarehouse.DataSource = dsResults;
                dgvWarehouse.DataBind();

            }
            catch (Exception ex)
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
            Session["SysToAdd"] = RoleNames.store;
            Response.Redirect("AddSystem.aspx", false);
        }

        protected void dgvWarehouse_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
             dgvWarehouse.PageIndex = e.NewPageIndex ;
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
                int.TryParse(lblSystemID.Text.ToString(), out SystemId);
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
            if (e.CommandName == "Edit")
            {
                int SystemId ;
                
                Label lblSystemID = (Label)dgvWarehouse.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSystemID");
                Label lblSystemRoleID = (Label)dgvWarehouse.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSystemRoleID");
                Label lblPharmacyID = (Label)dgvWarehouse.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblPharmacyID");
                Label lblBarterExchangeID = (Label)dgvWarehouse.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblBarterExchangeID");
                int.TryParse(lblSystemID.Text.ToString(), out SystemId);
                
                Session["SystemId"] = SystemId;
                Session["SystemRoleID"] = lblSystemRoleID.Text;
                Session["BarterExchangeID"] = lblBarterExchangeID.Text;
                Session["PharmacyID"] = lblPharmacyID.Text;
                Session["Action"] = "Edit";
                Session["SysToAdd"] = RoleNames.store;
                Response.Redirect("AddSystem.aspx", false);
            }
        }

        protected void dgvWarehouse_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvWarehouse.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreMain.aspx");
        }

        protected void btnSearchPharma_Click(object sender, EventArgs e)
        {
            DataView dv = ((DataSet)ViewState["PharmacyResultSet"]).Tables[0].DefaultView;
            dv.RowFilter = "SystemName LIKE '"+ txtSearchPharma.Text +"%'";

            DataTable dt = new DataTable();
            dt = dv.ToTable();

            dgvWarehouse.DataSource = dt;
            dgvWarehouse.DataBind();
        }
    }
}


 