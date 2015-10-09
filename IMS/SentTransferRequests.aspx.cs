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
    public partial class SentTransferRequests : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet dsStatic = new DataSet();
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
                 DataSet ds = new DataSet();
                 int Userid;
                 if (connection.State == ConnectionState.Closed)
                 {
                     connection.Open();
                 }

                 SqlCommand command = new SqlCommand("sp_GetOurSentTransferRequests_UserID", connection);
                 int.TryParse(Session["UserSys"].ToString(), out Userid);
                 command.Parameters.AddWithValue("@p_LoggedinUserID", Userid);
                 
                 command.CommandType = CommandType.StoredProcedure;
                 command.ExecuteNonQuery();
                 SqlDataAdapter da = new SqlDataAdapter(command);
                 da.Fill(ds);
                 dsStatic = ds;
                 dgvReceiveOurTransfers.DataSource = ds;
                 dgvReceiveOurTransfers.DataBind();
             }
             catch(Exception ex)
             {
                 if (connection.State == ConnectionState.Open)
                     connection.Close();
                 throw ex;
             }
            finally
             {
                 if (connection.State == ConnectionState.Open)
                     connection.Close();
             }
        }

        protected void btnGenTransferAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateTransferRequest.aspx",false);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreMain.aspx",false);
        }

        
        protected void dgvReceiveOurTransfers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dsStatic.Tables[0].Rows[e.Row.RowIndex]["TransferStatus"].ToString() == "Accepted")
                {
                    Button btnReceive = (Button)e.Row.FindControl("btnReceive");
                    if (btnReceive != null)
                    {
                        btnReceive.Visible = true;
                    }
                }
            }
        }

        protected void dgvReceiveOurTransfers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                 if(e.CommandName == "ReceiveProductTransfer")
                 {
                     Label lblDetailsID = (Label)dgvReceiveOurTransfers.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblTransferDetailID");
                     Session["TransferDetailsID"] = lblDetailsID.Text;

                     Response.Redirect("ReceiveRequestTransfers.aspx", false);
                 }
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}