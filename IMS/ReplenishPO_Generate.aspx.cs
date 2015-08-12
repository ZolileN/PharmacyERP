using IMS.Util;
using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace IMS
{
    public partial class ReplenishPO_Generate : System.Web.UI.Page
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
                    LoadData();
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
        public void LoadData()
        {
            try
            {
                int OrderID =0;
                int.TryParse(Session["RelenishOrderNo"].ToString(), out OrderID); //need to check there

                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("Sp_GetPODetails_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", OrderID);
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                gvReplenismentPO.DataSource = ds;
                gvReplenismentPO.DataBind();

                lblOrderFrom.Text = ds.Tables[0].Rows[0]["FromPlace"].ToString();
                lblOrderFromAddress.Text = ds.Tables[0].Rows[0]["FromPlaceAddress"].ToString();
                lblOrderFromPhone.Text = ds.Tables[0].Rows[0]["FromPlacePhone"].ToString();

                lblOrderTo.Text = ds.Tables[0].Rows[0]["ToPlace"].ToString();
                lblOrderToAddress.Text = ds.Tables[0].Rows[0]["ToPlaceAddress"].ToString();
                lblOrderToPhone.Text = ds.Tables[0].Rows[0]["ToPlacePhone"].ToString();
                lblOrderToEmail.Text = ds.Tables[0].Rows[0]["ToPlaceEmail"].ToString();
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReplenishMovement.aspx",false);
        }

        protected void gvReplenismentPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}