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
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class WarehouseManagement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    BindGrid();
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            // Void Page_Load(System.Object, System.EventArgs)
            // Handle specific exception.
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            // Clear the error from the server.
            Server.ClearError();
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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
            dgvWarehouse.PageIndex = e.NewPageIndex;
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
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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
            if(e.CommandName == "Delete")
            {

            }
        }

        protected void dgvWarehouse_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvWarehouse.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }
    }
}