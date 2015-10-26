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
    public partial class GenerateTransferRequest : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataTable distinctStores = new DataTable();
        public static DataTable dsDistinct = new DataTable();
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
                    dsDistinct = (DataTable)Session["TransferRequestGrid"];
                    distinctStores = dsDistinct.DefaultView.ToTable(true, "RequestedFrom","SystemID");
                    
                    drpTransferDetailsReport.DataSource = distinctStores;
                    drpTransferDetailsReport.DataBind();

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
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("TransferRequestGrid");
            Response.Redirect("CreateTransferRequest.aspx", false);
        }

        protected void drpTransferDetailsReport_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            //Display Sent/Received Records
            try
            {
                DataTable dtGridSource = new DataTable();
                dtGridSource.Columns.Add("ProductID");
                dtGridSource.Columns.Add("SystemID");
                dtGridSource.Columns.Add("Product_Name");
                dtGridSource.Columns.Add("RequestedFrom");
                dtGridSource.Columns.Add("RequestedTo");
                dtGridSource.Columns.Add("RequestedQty");
                dtGridSource.Columns.Add("BonusQty");
                dtGridSource.Columns.Add("PercentageDiscount");


                int FromID = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row[0].ToString());
                int ToID = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row[1].ToString());
                DataSet ds = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                
                //SqlCommand command = new SqlCommand("sp_GetSystems_ByID", connection);
                SqlCommand command = new SqlCommand("sp_GetSystems_Transfers_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_FromID", FromID);
                command.Parameters.AddWithValue("@p_ToID", ToID);


                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);

                Label lblFROMSystemName = (Label)e.Item.FindControl("lblFROMSystemName");
                Label lblFROMSystemAddress = (Label)e.Item.FindControl("lblFROMSystemAddress");
                Label lblFROMSystemPhone = (Label)e.Item.FindControl("lblFROMSystemPhone");
                Label lblFROMSystemEmail = (Label)e.Item.FindControl("lblFROMSystemEmail");
                Label lblToSystemName = (Label)e.Item.FindControl("lblToSystemName");
                Label lblToSystemAddress = (Label)e.Item.FindControl("lblToSystemAddress");
                Label lblToSystemPhone = (Label)e.Item.FindControl("lblToSystemPhone");
                Label lblToSystemEmail = (Label)e.Item.FindControl("lblToSystemEmail");
                
                lblFROMSystemName.Text = ds.Tables[0].Rows[0]["SystemName"].ToString();
                lblFROMSystemAddress.Text = ds.Tables[0].Rows[0]["SystemAddress"].ToString();
                lblFROMSystemPhone.Text = ds.Tables[0].Rows[0]["SystemPhone"].ToString();
                lblFROMSystemEmail.Text = ds.Tables[0].Rows[0]["SystemFax"].ToString();
                lblToSystemName.Text = ds.Tables[1].Rows[0]["SystemName"].ToString();
                lblToSystemAddress.Text = ds.Tables[1].Rows[0]["SystemAddress"].ToString();
                lblToSystemPhone.Text = ds.Tables[1].Rows[0]["SystemPhone"].ToString();
                lblToSystemEmail.Text = ds.Tables[1].Rows[0]["SystemFax"].ToString();


                GridView dgvTransferDisplay = (GridView)e.Item.FindControl("dgvTransferDisplay");

                DataRow[] drList = dsDistinct.Select("SystemID = '" + ToID + "'");

                //dtGridSource.DefaultView.RowFilter = "SystemID = " + StoreId;
                //dtGridSource.DefaultView.ToTable();
                foreach (DataRow dr in drList)
                {
                    dtGridSource.Rows.Add(dr.ItemArray);
                }
                dtGridSource.AcceptChanges();
                dgvTransferDisplay.DataSource = dtGridSource;
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}