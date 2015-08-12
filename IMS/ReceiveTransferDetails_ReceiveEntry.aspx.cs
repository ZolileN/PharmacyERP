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

        public static DataTable dtReceiveEnty;

        public int Sent;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                dtReceiveEnty = new DataTable();
                lblTotalTransferQty.Text = Session["TransferedQty"].ToString();
                Sent = Convert.ToInt32(lblTotalTransferQty.Text.ToString());
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

            SqlCommand command = new SqlCommand("sp_getRequestedTransferProductStockDetails_TransferDetailID", connection);
            command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetID);

            command.Parameters.AddWithValue("@p_StoreID", Userid);
            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);
            
            dtReceiveEnty = ds.Tables[0];

            SetdtReceiveEnty();

            dgvReceiveTransferDetailsReceive.DataSource = dtReceiveEnty;
            dgvReceiveTransferDetailsReceive.DataBind();

            //dgvReceiveTransferDetailsReceive.DataSource = ds;
            //dgvReceiveTransferDetailsReceive.DataBind();
             
        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            for (int i = 0; i < dgvReceiveTransferDetailsReceive.Rows.Count; i++)
            {
                TextBox txtSentQuantity = (TextBox)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("txtSendQty");
                txtSentQuantity.Attributes.Add("onchange", "Validate(" + i + ");return false;");
            }
            for (int i = 0; i < dgvReceiveTransferDetailsReceive.Rows.Count; i++)
            {
                DateTime RequestDate, Expiry;
                int TransferedQty, TransferedBonusQty, TransferDetID, StockId, ProdctID,  ReqQty = 0, AvailableQty ;
                decimal CP, SP;
                double Barcode;


                // Label lblentryID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblentryID");
                Label lblRequestedBonusQty = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblRequestedBonusQty");
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
                Label lblAvailableStock = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblAvailableStock");
                TextBox txtTransferedQty = (TextBox)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("txtSendQty");
                TextBox txtTransferedBonusQty = (TextBox)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("txtBonusQty");

                double.TryParse(lblBarCode.Text.ToString(), out Barcode);
                int.TryParse(lblAvailableStock.Text.ToString(), out AvailableQty);
                int.TryParse(lblRequestedQty.Text.ToString(), out ReqQty);
                
                int.TryParse(txtTransferedQty.Text.ToString(), out TransferedQty);
                int.TryParse(txtTransferedBonusQty.Text.ToString(), out TransferedBonusQty);

                int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetID);
                int.TryParse(lblProductID.Text.ToString(), out ProdctID);
                int.TryParse(lblStockID.Text.ToString(), out StockId);
                DateTime.TryParse(lblRequestedDate.Text.ToString(), out RequestDate);
                DateTime.TryParse(lblExpiryDate.Text.ToString(), out Expiry);
                Decimal.TryParse(lblCP.Text.ToString(), out CP);
                Decimal.TryParse(lblSP.Text.ToString(), out SP);
                string BatchNo = lblBatchNumber.Text.ToString();
                int userID = Convert.ToInt32(Session["UserID"].ToString());

                if (TransferedQty > 0)
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_UpdateTransferReceiveEntry", connection);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetID);
                    command.Parameters.AddWithValue("@p_RequestDate", RequestDate);
                    command.Parameters.AddWithValue("@p_ReqQty", ReqQty);
                    command.Parameters.AddWithValue("@p_TransferQty", TransferedQty);
                    command.Parameters.AddWithValue("@p_TransferBonusQty", TransferedBonusQty);
                    command.Parameters.AddWithValue("@p_Barcode", Barcode);
                    command.Parameters.AddWithValue("@p_ProdctID", ProdctID);
                    command.Parameters.AddWithValue("@p_StockId", StockId);
                    command.Parameters.AddWithValue("@p_Expiry", Expiry);
                    command.Parameters.AddWithValue("@p_CP", CP);
                    command.Parameters.AddWithValue("@p_SP", SP);
                    command.Parameters.AddWithValue("@p_BatchNo", BatchNo);
                    command.Parameters.AddWithValue("@p_TransferToUserID", userID);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    //Update Stock
                    UpdateStockMinus(TransferDetID, ProdctID, AvailableQty, TransferedQty, Expiry, StockId, CP, SP, BatchNo, ReqQty, Barcode, TransferedBonusQty);

                    if (!Session["UserRole"].ToString().Equals("WareHouse"))
                    {
                        Response.Redirect("ReceiveTransferOrder.aspx");
                    }
                    else
                    {
                        Response.Redirect("RespondStoreRequest.aspx", false);
                    }
                   
                }
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
            catch (Exception exp)
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

        private void UpdateStockMinus(int TransferDetailID, int ProductID, int quantity, int Sent, DateTime Expiry, int StockID, decimal CP, decimal SP, string BatchNO, int ReqQty, double Barcode, int TransferedBonusQty)
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                Sent = Sent + TransferedBonusQty;

                SqlCommand command;
                command = new SqlCommand("Sp_UpdateStockBy_ExpiryStockID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_StockID", StockID);
                command.Parameters.AddWithValue("@p_quantity", Sent);
                command.Parameters.AddWithValue("@p_Action", "Minus");
                command.Parameters.AddWithValue("@p_Expiry", Expiry);
                command.ExecuteNonQuery();
            }
            catch (Exception exp) { }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            if (!Session["UserRole"].ToString().Equals("WareHouse"))
            {
                Response.Redirect("StoreMain.aspx", false);
            }
            else 
            {
                Response.Redirect("WarehouseMain.aspx", false);
            }
        }

        protected void dgvReceiveTransferDetailsReceive_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtSentQuantity = (TextBox)e.Row.FindControl("txtSendQty");

                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    int id = (int)e.Row.RowIndex;


                    txtSentQuantity.Attributes.Add("onchange", "Validate(" + id + ");return false;");

                }
            }
            catch (Exception ex)
            {
                
                //throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }

        private void SetdtReceiveEnty()
        {
            DataTable dtTemp = dtReceiveEnty.Copy();
            for (int i = 0; i < dtReceiveEnty.Rows.Count; i++)
            {
                if (Sent > 0)
                {
                     
                    int TransferDetId = Convert.ToInt32(dtReceiveEnty.Rows[i]["TransferDetailID"].ToString());
                    int Quantity = Convert.ToInt32(dtReceiveEnty.Rows[i]["AvailableQty"].ToString());
                    int ProductId = Convert.ToInt32(dtReceiveEnty.Rows[i]["ProductID"].ToString());

                    if (Quantity < Convert.ToInt32(dtTemp.Rows[i]["RequestedQty"]))
                    {
                        dtReceiveEnty.Rows[i]["SentQty"] = Quantity;
                        dtTemp.Rows[i + 1]["RequestedQty"] = Convert.ToInt32(dtTemp.Rows[i]["RequestedQty"]) - Quantity;
                        dtReceiveEnty.AcceptChanges();
                        Sent = Sent - Quantity;
                    }
                    else
                    {
                        dtReceiveEnty.Rows[i]["SentQty"] = Sent;
                        dtReceiveEnty.AcceptChanges();
                        Sent = Sent - Convert.ToInt32(dtReceiveEnty.Rows[i]["RequestedQty"].ToString());
                    }

                }
            }

            #region Bonus
            for (int i = 0; i < dtReceiveEnty.Rows.Count; i++)
            {
                if (Sent > 0)
                {

                    int TransferDetId = Convert.ToInt32(dtReceiveEnty.Rows[i]["TransferDetailID"].ToString());
                    int Quantity = Convert.ToInt32(dtReceiveEnty.Rows[i]["AvailableQty"].ToString());
                    int ProductId = Convert.ToInt32(dtReceiveEnty.Rows[i]["ProductID"].ToString());

                    if (Quantity < Convert.ToInt32(dtTemp.Rows[i]["RequestedQty"]))
                    {
                        dtReceiveEnty.Rows[i]["BonusQty"] = Quantity;
                        dtTemp.Rows[i + 1]["RequestedQty"] = Convert.ToInt32(dtTemp.Rows[i]["RequestedQty"]) - Quantity;
                        dtReceiveEnty.AcceptChanges();
                        Sent = Sent - Quantity;
                    }
                    else
                    {
                        dtReceiveEnty.Rows[i]["BonusQty"] = Sent;
                        dtReceiveEnty.AcceptChanges();
                        Sent = Sent - Convert.ToInt32(dtReceiveEnty.Rows[i]["RequestedQty"].ToString());
                    }

                }
            }
            #endregion
        }
    }
}