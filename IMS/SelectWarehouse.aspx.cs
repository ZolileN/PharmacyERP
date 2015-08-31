using IMS.Util;
using log4net;
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
    public partial class SelectWarehouse : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack) 
            {
                #region Populating Warehouse
                try
                {
                    DataSet dsS = new DataSet();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();

                    }
                    SqlCommand command = new SqlCommand("Sp_GetWarehouseSystems_ByRoles", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_RoleName", "Warehouse");
                    command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(dsS);
                    ddlWH.DataSource = dsS.Tables[0];
                    ddlWH.DataTextField = "SystemName";
                    ddlWH.DataValueField = "SystemID";
                    ddlWH.DataBind();
                    if (ddlWH != null)
                    {
                        ddlWH.Items.Insert(0, "Select Store");
                        ddlWH.SelectedIndex = 0;
                    }
                   
                }
                catch (Exception ex)
                {

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                #endregion
            }
            expHandler.CheckForErrorMessage(Session);
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
        protected void btnContinue_Click(object sender, EventArgs e)
        {
            if (ddlWH.SelectedIndex > 0)
            {
                Session["WH_Name"] = ddlWH.SelectedItem.ToString();
                Session["WH_ID"] = ddlWH.SelectedValue;
                Response.Redirect("ItemRequestWH.aspx");
            }
        }
    }
}