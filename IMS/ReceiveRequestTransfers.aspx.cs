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
    public partial class ReceiveRequestTransfers : System.Web.UI.Page
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
                int TransferDetID;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("sp_ReceiveSentTransferOrder_TransferDetID", connection);
                int.TryParse(Session["TransferDetailsID"].ToString(), out TransferDetID);
                command.Parameters.AddWithValue("@p_TransferDetID", TransferDetID);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);

                dgvReceiveOurTransfersEntry.DataSource = ds;
                dgvReceiveOurTransfersEntry.DataBind();
                lblTotalSentQty.Text = ds.Tables[0].Rows[0]["TotalSentQty"].ToString();
            }
            catch(Exception ex)
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

        protected void btnAcceptAll_Click(object sender, EventArgs e)
        {
            try
            {

                for (int i = 0; i < dgvReceiveOurTransfersEntry.Rows.Count; i++)
                {
                    int entryID, ReceivedQty, TransferDetID = 0, ProductID, barcode, DelieveredBonusQty;
                    decimal CP, SP;
                    DateTime Expiry;
                    string BatchNumber;
                    
                    Label lblentryID = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblentryID");
                    int.TryParse(lblentryID.Text.ToString(), out entryID);
                    Label lblDetailsID = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblTransferDetailID");
                    
                    //int.TryParse(lblDetailsID.Text.ToString(), out TransferDetID);
                    int.TryParse(Session["TransferDetailsID"].ToString(), out TransferDetID);
                    Label lblReceivedQty = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblReceivedQty");
                    int.TryParse(lblReceivedQty.Text.ToString(), out ReceivedQty);

                    Label lblCP = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblCP");
                    decimal.TryParse(lblCP.Text.ToString(), out CP);
                    Label lblSP = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblSP");
                    decimal.TryParse(lblSP.Text.ToString(), out SP);
                    Label lblBarcode = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblBarcode");
                    int.TryParse(lblBarcode.Text.ToString(), out barcode);
                    Label lblBatchNumber = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblBatchNumber");
                    BatchNumber = lblBatchNumber.Text.ToString();

                    Label lblProductID = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblProductID");
                    int.TryParse(lblProductID.Text.ToString(), out ProductID);

                    Label lblExpiryDate = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblExpiryDate");
                    DateTime.TryParse(lblExpiryDate.Text.ToString(), out Expiry);

                    Label lblTransferedBonusQty = (Label)dgvReceiveOurTransfersEntry.Rows[i].FindControl("lblTransferedBonusQty");
                    int.TryParse(lblTransferedBonusQty.Text.ToString(), out DelieveredBonusQty);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand command = new SqlCommand("sp_UpdateTransferReceiveEntry_Received", connection);

                    command.Parameters.AddWithValue("@p_entryID", entryID);
                    command.Parameters.AddWithValue("@p_ReceiveQty", ReceivedQty);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetID);
                    command.Parameters.AddWithValue("@p_DelieveredBonusQty", DelieveredBonusQty);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    // Update Stock for this Store (Add Received Stock)
                   // ReceivedQty = ReceivedQty + DelieveredBonusQty; 
                    UpdateStockPlus(TransferDetID, ReceivedQty, ProductID, barcode, Expiry, CP, SP, BatchNumber);
                    Session.Remove("TransferDetailsID");
                    Response.Redirect("SentTransferRequests.aspx",false);

                }
            }
            catch(Exception ex)
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

        private void UpdateStockPlus(int TransferDetailID, int quantity, int ProductID, int BarCode, DateTime Expiry, decimal CP, decimal SP, string BatchNo)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                #region Query stock
                SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));
                DataSet StockDs = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(StockDs);
                 
                #endregion

                if (StockDs.Tables[0].Rows.Count > 0)
                {
                    for (int i = 0; i < StockDs.Tables[0].Rows.Count; i++)
                    {
                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", StockDs.Tables[0].Rows[i]["StockID"]);
                        command.Parameters.AddWithValue("@p_quantity", quantity);
                        command.Parameters.AddWithValue("@p_Action", "Add");
                        command.ExecuteNonQuery();
                    }
                }
                else
                {
                    int id; int x = 0;
                    int.TryParse(Session["UserSys"].ToString(), out id);

                    SqlCommand commnd = new SqlCommand("sp_AddStock", connection);
                    commnd.CommandType = CommandType.StoredProcedure;
                    commnd.Parameters.AddWithValue("@p_ProductID", ProductID);
                    commnd.Parameters.AddWithValue("@p_Quantity", quantity);
                    commnd.Parameters.AddWithValue("@p_Status", "1");
                     commnd.Parameters.AddWithValue("@p_UserRoleID", id);
                     
                    commnd.Parameters.AddWithValue("@p_BarCode", BarCode);
                    commnd.Parameters.AddWithValue("@p_Expiry", Expiry); // Calender Date or DateTime Picker Date
                    commnd.Parameters.AddWithValue("@p_Cost", CP);
                    commnd.Parameters.AddWithValue("@p_Sales", SP);
                    commnd.Parameters.AddWithValue("@p_BatchNumber", BatchNo);
                    x = commnd.ExecuteNonQuery();
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("TransferDetailsID");
            Response.Redirect("SentTransferRequests.aspx",false);
        }
    }
}