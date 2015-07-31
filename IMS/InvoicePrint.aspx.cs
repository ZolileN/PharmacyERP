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
using System.IO;
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class InvoicePrint : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet InvoiceSet;
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
                InvoiceDate.Text = Session["PrintInvoiceDate"].ToString();
                DueDate.Text = Session["PrintDueDate"].ToString();
                SalesMan.Text = Session["SalesMan"].ToString();

                if (Session["PrintCheck"].ToString().Equals("Actual"))
                {
                    Invoice.Text = Session["PrintInvoiceNumber"].ToString();
                    lblTotalBonusAmount.Visible = false;
                    //Label9.Visible = false;
                }
                else if (Session["PrintCheck"].ToString().Equals("Bonus"))
                {
                    Invoice.Text = "B" + Session["PrintInvoiceNumber"].ToString();
                    lblTotalSentAmount.Visible = false;
                    //Label8.Visible = false;
                }
                DataSet dsTo = GetSystems(Convert.ToInt32(Session["RequestedFromID"].ToString()));
                To.Text = dsTo.Tables[0].Rows[0]["SystemName"].ToString();
                ToAddress.Text = dsTo.Tables[0].Rows[0]["SystemAddress"].ToString();
                LoadData();
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
        public void LoadData()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetSaleOrderDetailList_Bonus", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["OrderNo_Invoice"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                InvoiceSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                double BonusAmount = 0;
                double ActualAmount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BonusAmount += Convert.ToDouble(ds.Tables[0].Rows[i]["AmountBonus"]);
                    ActualAmount += Convert.ToDouble(ds.Tables[0].Rows[i]["Amount"]);
                }

                lblTotalBonusAmount.Text = BonusAmount.ToString();
                lblTotalSentAmount.Text = ActualAmount.ToString();
            }
            catch (Exception ex)
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
        public DataSet GetSystems(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", ID);



                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }
            return ds;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesOrderInvoice.aspx",false);
        }

        
    }
}