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
using log4net;
using IMS.Util;
using IMSBusinessLogic;

namespace IMS
{
    public partial class PharmacyManagement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        private SystemBLL sysBll = new SystemBLL();
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
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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
                dsResults = sysBll.SelectAllPharmacies();
                ViewState["PharmacyResultSet"] = dsResults;
                dgvWarehouse.DataSource = dsResults;
                dgvWarehouse.DataBind();

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
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
                int SystemId;
                Label lblSystemID = (Label)dgvWarehouse.Rows[e.RowIndex].FindControl("lblSystemID");
                int.TryParse(lblSystemID.Text.ToString(), out SystemId);
                sysBll.Delete(SystemId);               
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                
                BindGrid();
            }
        }

        protected void dgvWarehouse_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    int SystemId;

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
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void dgvWarehouse_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvWarehouse.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }

        protected void btnSearchPharma_Click(object sender, EventArgs e)
        {
            try
            {
                DataView dv = ((DataSet)ViewState["PharmacyResultSet"]).Tables[0].DefaultView;
                dv.RowFilter = "SystemName LIKE '" + txtSearchPharma.Text + "%'";

                DataTable dt = new DataTable();
                dt = dv.ToTable();

                dgvWarehouse.DataSource = dt;
                dgvWarehouse.DataBind();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}


 