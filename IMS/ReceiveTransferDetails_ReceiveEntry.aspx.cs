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
    public partial class ReceiveTransferDetails_ReceiveEntry : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                lblTotalTransferQty.Text = Session["TransferedQty"].ToString();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            int TransferDetID, Userid = 0;
            int.TryParse(Session["UserSys"].ToString(), out Userid);

            DataSet ds = new DataSet();
            int.TryParse(Session["TransferDetailID"].ToString(), out TransferDetID); 
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }

            //SqlCommand command = new SqlCommand("sp_getTransferDetailsReceiveEntry_TransferDetailID", connection);  
            SqlCommand command = new SqlCommand("sp_getRequestedTransferProductStockDetails_TransferDetailID", connection);
            command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetID);

            command.Parameters.AddWithValue("@p_StoreID", Userid);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);

            dgvReceiveTransferDetailsReceive.DataSource = ds;
            dgvReceiveTransferDetailsReceive.DataBind();
             Label lblProductID = (Label)dgvReceiveTransferDetailsReceive.Rows[0].FindControl("lblProductID");
             TextBox txtSendQty = (TextBox)dgvReceiveTransferDetailsReceive.Rows[0].FindControl("txtSendQty");

            // UpdateStockPlus(TransferDetID, Convert.ToInt32(lblProductID.Text.ToString()), Convert.ToInt32(txtSendQty.Text.ToString()));

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {  
            for(int i=0;i<dgvReceiveTransferDetailsReceive.Rows.Count; i++)
            {
                DateTime RequestDate, Expiry;
                int  TransferedQty, TransferDetID, StockId, ProdctID, Barcode, ReqQty = 0;
                decimal CP, SP;
                

               // Label lblentryID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblentryID");
                Label lblTransferDetailsID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblTransferDetailsID");
                Label lblProductID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblProductID");

                Label lblBarCode = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblBarCode");
                Label lblRequestedDate = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblRequestedDate");
                Label lblRequestedQty = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblRequestedQty");
                Label lblSP = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblSP");
                Label lblCP = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblCP");
                Label lblExpiryDate = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblExpiryDate");
                Label lblBatchNumber = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblBatchNumber");
                Label lblStockID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblStockID");
                TextBox txtTransferedQty = (TextBox)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("txtSendQty");

                int.TryParse(lblBarCode.Text.ToString(), out Barcode);
                int.TryParse(lblRequestedQty.Text.ToString(), out ReqQty);
                int.TryParse(txtTransferedQty.Text.ToString(), out TransferedQty);
                int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetID);
                int.TryParse(lblProductID.Text.ToString(), out ProdctID);
                int.TryParse(lblStockID.Text.ToString(), out StockId); 
                DateTime.TryParse(lblRequestedDate.Text.ToString(), out RequestDate);
                DateTime.TryParse(lblExpiryDate.Text.ToString(), out Expiry);
                Decimal.TryParse(lblCP.Text.ToString(), out CP);
                Decimal.TryParse(lblSP.Text.ToString(),out SP);
                string BatchNo = lblBatchNumber.Text.ToString();


                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_UpdateTransferReceiveEntry", connection);
                command.Parameters.AddWithValue("@p_TransferDetID", TransferDetID);
                command.Parameters.AddWithValue("@p_RequestDate", RequestDate);
                command.Parameters.AddWithValue("@p_ReqQty", ReqQty);
                command.Parameters.AddWithValue("@p_TransferQty", TransferedQty);
                command.Parameters.AddWithValue("@p_Barcode", Barcode);
                command.Parameters.AddWithValue("@p_ProdctID", ProdctID);
                command.Parameters.AddWithValue("@p_StockId", StockId);
                command.Parameters.AddWithValue("@p_Expiry", Expiry);
                command.Parameters.AddWithValue("@p_CP", CP);
                command.Parameters.AddWithValue("@p_SP", SP);
                command.Parameters.AddWithValue("@p_BatchNo", BatchNo);
                 
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                //Update Stock
                UpdateStockMinus(TransferDetID, ProdctID, 0, TransferedQty);

                Response.Redirect("ReceiveTransferOrder.aspx");
            }
             
        }

        private void UpdateStockPlus(int TransferDetailID, int ProductID, int Quantity)
        {
            try
            {
                DataSet stockDet;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region Query stock
                SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                #endregion

                #region Set value for ordered quantity
                Dictionary<int, int> stockSet = new Dictionary<int, int>();
                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (Quantity > 0 && exQuan > 0)
                    {
                        if (exQuan >= Quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), Quantity);
                            break;
                        }
                        else if (exQuan < Quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            Quantity = Quantity - exQuan;
                        }
                    }
                }


                #endregion

                #region update stock

                foreach (int id in stockSet.Keys)
                {
                    DataView dv = stockDet.Tables[0].DefaultView;
                    dv.RowFilter = "StockID = " + id;
                    DataTable dt = dv.ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                        command.Parameters.AddWithValue("@p_Action", "Add");
                        command.ExecuteNonQuery();
                    }

                }
                #endregion


            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
            }
        }

        private void UpdateStockMinus(int TransferDetailID, int ProductID, int quantity, int Sent)
        {
            try
            {
                DataSet stockDet;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region Query stock
                SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                #endregion

                #region Set value for ordered quantity
                Dictionary<int, int> stockSet = new Dictionary<int, int>();
                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (Sent > 0 && exQuan > 0)
                    {
                        if (exQuan >= Sent)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), Sent);
                            break;
                        }
                        else if (exQuan < Sent)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            Sent = Sent - exQuan;
                        }
                    }
                }


                #endregion

                #region update stock

                foreach (int id in stockSet.Keys)
                {
                    DataView dv = stockDet.Tables[0].DefaultView;
                    dv.RowFilter = "StockID = " + id;
                    DataTable dt = dv.ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        //command = new SqlCommand("sp_EntryTransferDetails_Receive", connection);
                        //command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetailID);
                        //command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        //command.Parameters.AddWithValue("@p_StockID", id);
                        //command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        //command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        //command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        //command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);
                        //command.Parameters.AddWithValue("@p_TransferredQty", stockSet[id]);

                        //command.Parameters.AddWithValue("@p_RequestedQty", dt.Rows[0]["RequestedQty"]);

                        //command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);

                        //command.ExecuteNonQuery();

                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                        command.Parameters.AddWithValue("@p_Action", "Minus");
                        command.ExecuteNonQuery();
                    }

                }
                #endregion


            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
            }
        }
    }
}