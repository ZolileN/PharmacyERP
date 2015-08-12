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
    public partial class SalesOrderInvoice : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet InvoiceSet;
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
                    if (Session["PrintInvoiceNumber"] != null && Session["PrintInvoiceNumber"].ToString() != "")
                    {
                        txtIvnoice.Text = Session["PrintInvoiceNumber"].ToString();
                    }
                    if (Session["PrintInvoiceDate"] != null && Session["PrintInvoiceDate"].ToString() != "")
                    {
                        DateTextBox.Text = Session["PrintInvoiceDate"].ToString();
                    }
                    if (Session["PrintDueDate"] != null && Session["PrintDueDate"].ToString() != "")
                    {
                        DateTextBox2.Text = Session["PrintDueDate"].ToString();
                    }

                    LoadData();
                    ddlSalesMan.Items.Add("Select-SalesMan");
                    ddlSalesMan.Items.Add("Mohanad Karim");
                    ddlSalesMan.Items.Add("Mohamed Irshad");
                    ddlSalesMan.Items.Add("Shareef");
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
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetInvoiceNumber", connection);
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                int NewInvoiceNumber = Convert.ToInt32(ds.Tables[0].Rows[0][0].ToString()) +1;
                txtIvnoice.Text = NewInvoiceNumber.ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
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
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_UpdateInvoiceNumber", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InvoiceNumber", Convert.ToInt32(txtIvnoice.Text.ToString()));
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                for (int i = 0; i < InvoiceSet.Tables[0].Rows.Count; i++)
                {
                    SqlCommand command = new SqlCommand("sp_InsertInvoiceTemp", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@SOrderID", Convert.ToInt32(Session["OrderNo_Invoice"].ToString()));

                    command.Parameters.AddWithValue("@InvID", Convert.ToInt32(txtIvnoice.Text.ToString()));
                    command.Parameters.AddWithValue("@InvDate", DateTextBox.Text.ToString());
                    command.Parameters.AddWithValue("@DueDate", DateTextBox2.Text.ToString());

                    command.Parameters.AddWithValue("@P_ID", Convert.ToInt32(InvoiceSet.Tables[0].Rows[i]["ProductID"].ToString()));
                    command.Parameters.AddWithValue("@ExpDate", InvoiceSet.Tables[0].Rows[i]["ExpiryDate"].ToString());
                    command.Parameters.AddWithValue("@BatchNumber", InvoiceSet.Tables[0].Rows[i]["BatchNumber"].ToString());
                    command.Parameters.AddWithValue("@Discount", InvoiceSet.Tables[0].Rows[i]["DiscountPercentage"].ToString());
                    command.Parameters.AddWithValue("@CostPrice", InvoiceSet.Tables[0].Rows[i]["CostPrice"].ToString());
                    command.Parameters.AddWithValue("@SentQuantity", InvoiceSet.Tables[0].Rows[i]["SendQuantity"].ToString());
                    command.Parameters.AddWithValue("@SentAmount", InvoiceSet.Tables[0].Rows[i]["Amount"].ToString());
                    command.Parameters.AddWithValue("@BonusQuantity", InvoiceSet.Tables[0].Rows[i]["BonusQuantity"].ToString());
                    command.Parameters.AddWithValue("@BonusAmount", InvoiceSet.Tables[0].Rows[i]["AmountBonus"].ToString());

                    command.ExecuteNonQuery();
                }
                
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void btnPrintActual_Click(object sender, EventArgs e)
        {
            Session["PrintCheck"] = "Actual";
            
            Session["PrintInvoiceNumber"] = txtIvnoice.Text;
            Session["PrintInvoiceDate"] = DateTextBox.Text;
            Session["PrintDueDate"] = DateTextBox2.Text;
            if (ddlSalesMan.SelectedItem.ToString().Equals("Select-SalesMan"))
            {
                Session["SalesMan"] = "";
            }
            else
            {
                Session["SalesMan"] = ddlSalesMan.SelectedItem.ToString();
            }
            Response.Redirect("InvoicePrint.aspx");
        }

        protected void btnPrintBonus_Click(object sender, EventArgs e)
        {
            Session["PrintCheck"] = "Bonus";
            Session["PrintInvoiceNumber"] = txtIvnoice.Text;
            Session["PrintInvoiceDate"] = DateTextBox.Text;
            Session["PrintDueDate"] = DateTextBox2.Text;
            if (ddlSalesMan.SelectedItem.ToString().Equals("Select-SalesMan"))
            {
                Session["SalesMan"] = "";
            }
            else
            {
                Session["SalesMan"] = ddlSalesMan.SelectedItem.ToString();
            }
            Response.Redirect("InvoicePrintBonus.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
           
            Response.Redirect("WarehouseMain.aspx");
        }
    }
}