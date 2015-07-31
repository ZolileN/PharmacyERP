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
	public partial class Print_Request_WH : System.Web.UI.Page
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

                if (Request.QueryString["Id"] != null) 
                {
                    BindGrid();
                }

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
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("WH_TransferRequestGrid");
            Response.Redirect("SelectWarehouse.aspx", false);
        }

        protected void BindGrid()
        {
            try
            {
                DataTable dtGridSource = new DataTable();
                dtGridSource.Columns.Add("ProductID");
                dtGridSource.Columns.Add("SystemID");
                dtGridSource.Columns.Add("Product_Name");
                dtGridSource.Columns.Add("RequestedFrom");
                dtGridSource.Columns.Add("RequestedTo");
                dtGridSource.Columns.Add("RequestedQty");

                int transferID =  int.Parse(Request.QueryString["Id"].ToString());
                DataSet ds = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystems_ByTransferID", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_transferID", transferID);

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);


                lblFROMSystemName.Text = ds.Tables[0].Rows[0]["FROMName"].ToString();
                lblFROMSystemAddress.Text = ds.Tables[0].Rows[0]["FROMAdress"].ToString();
                lblFROMSystemPhone.Text = ds.Tables[0].Rows[0]["FROMPhone"].ToString();
                lblFROMSystemEmail.Text = ds.Tables[0].Rows[0]["FROMFax"].ToString();
                lblToSystemName.Text = ds.Tables[0].Rows[0]["ToName"].ToString();
                lblToSystemAddress.Text = ds.Tables[0].Rows[0]["ToAddress"].ToString();
                lblToSystemPhone.Text = ds.Tables[0].Rows[0]["ToPhone"].ToString();
                lblToSystemEmail.Text = ds.Tables[0].Rows[0]["ToFax"].ToString();


                DataTable dsDistinct = (DataTable)Session["WH_TransferRequestGrid"];

                dtGridSource.AcceptChanges();
                dgvTransferDisplay.DataSource = dsDistinct;
                dgvTransferDisplay.DataBind();

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
	}
}