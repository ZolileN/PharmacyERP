using IMSCommon.Util;
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
    public partial class RespondStoreRequest : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                LoadRepeater();
            }
        }

        private void LoadRepeater()
        {
            try
            {
                DataSet ds = new DataSet();
                int Userid;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_getTransferDetails_UserId", connection);
                int.TryParse(Session["UserSys"].ToString(), out Userid);
                command.Parameters.AddWithValue("@UserID", Userid);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                DataTable dsDistinct = ds.Tables[0];
                DataTable distinctRequests = dsDistinct.DefaultView.ToTable(true, "TransferTo");
                //dsDistinct.DefaultView.RowFilter(); 
                repReceiveTransfer.DataSource = distinctRequests;
                repReceiveTransfer.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReceiveStoreRequest.aspx",false);
        }

        protected void repReceiveTransfer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                GridView dgvReceiveTransfer = (GridView)e.Item.FindControl("dgvReceiveTransfer");
                Literal litStoreName = (Literal)e.Item.FindControl("litStoreName");

                DataSet ds = new DataSet();
                int transID,storedAt;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
               
                SqlCommand command = new SqlCommand("Sp_GetTransferDetails_byParameters", connection);
                command.CommandType = CommandType.StoredProcedure;
                int.TryParse(Session["WH_RequestedNO"].ToString(), out transID);
                command.Parameters.AddWithValue("@p_TransferID", transID);
                int.TryParse(Session["UserSys"].ToString(), out storedAt);
                command.Parameters.AddWithValue("@p_storedAt", storedAt);

             
            
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
               
                dgvReceiveTransfer.DataSource = ds;
                dgvReceiveTransfer.DataBind();

                litStoreName.Text = ds.Tables[0].Rows[0]["RequestedBy"].ToString();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        protected void repReceiveTransfer_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AcceptTransferOrder")
                {
                    GridView gvReceiveTransfer = (GridView)repReceiveTransfer.Items[0].FindControl("dgvReceiveTransfer");
                    for (int i = 0; i < gvReceiveTransfer.Rows.Count; i++)
                    {
                        int TransferNo, TransferDetailNo, RequestedQty, TransferedQty, AvailableQty, ProductId;
                        int LogedInStoreID;


                        int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);

                        Label lblTransferNo = (Label)gvReceiveTransfer.Rows[i].FindControl("lblRequestNo");
                        Label lblTransferDetailsID = (Label)gvReceiveTransfer.Rows[i].FindControl("lblTransferDetailsID");
                        Label lblRequestedQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblRequestedQty");
                        Label lblAvailableQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblAvailableQty");
                        Label lblSentQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblSentQty");
                        Label lblProductID = (Label)gvReceiveTransfer.Rows[i].FindControl("lblProductID");

                        int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                        int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                        int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);

                        int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                        int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                        int.TryParse(lblProductID.Text.ToString(), out ProductId);
                        if (AvailableQty > 0)
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }

                            SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_AcceptAll", connection);
                            command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                            command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                            command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                            command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                            command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                            command.Parameters.AddWithValue("@p_Status", "Accepted");
                            command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                            command.Parameters.AddWithValue("@p_ProductID", ProductId);

                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();

                            Button btnAccept = (Button)gvReceiveTransfer.Rows[i].FindControl("btnAccept");
                            btnAccept.Visible = false;
                            Button btnDeny = (Button)gvReceiveTransfer.Rows[i].FindControl("btnDeny");
                            btnDeny.Visible = false;
                            Button btnEdit = (Button)gvReceiveTransfer.Rows[i].FindControl("btnEdit");
                            btnEdit.Visible = false;
                            Button btnStaticAccepted = (Button)gvReceiveTransfer.Rows[i].FindControl("btnStaticAccepted");
                            btnStaticAccepted.Visible = true;
                            if (RequestedQty != TransferedQty)
                            {
                                if (TransferedQty == 0)
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty);
                                }
                                else
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);
                                }
                            }
                        }
                    }
                }
                if (e.CommandName == "GenTransferOrder")
                {
                    DataSet dsResults = new DataSet();

                    GridView gvReceiveTransfer = (GridView)repReceiveTransfer.Items[0].FindControl("dgvReceiveTransfer");
                    int TransferNo, TransferDetailNo, RequestedQty, TransferedQty, AvailableQty, ProductId;
                    int LogedInStoreID;


                    int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);

                    Label lblTransferNo = (Label)gvReceiveTransfer.Rows[0].FindControl("lblRequestNo");
                    Label lblTransferDetailsID = (Label)gvReceiveTransfer.Rows[0].FindControl("lblTransferDetailsID");
                    Label lblRequestedQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblRequestedQty");
                    Label lblAvailableQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblAvailableQty");
                    Label lblSentQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblSentQty");
                    Label lblProductID = (Label)gvReceiveTransfer.Rows[0].FindControl("lblProductID");

                    int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                    int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                    int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);

                    int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                    int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                    int.TryParse(lblProductID.Text.ToString(), out ProductId);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_AcceptAll", connection);
                    command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                    command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                    command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                    command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                    command.Parameters.AddWithValue("@p_Status", "Accepted");
                    command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductId);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dsResults);

                    Session["TransferRequestGrid"] = dsResults.Tables[0];

                    Response.Redirect("GenerateAcceptedTransferOrder.aspx", false);
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

        }

        private bool UpdateStockMinus(int TransferDetailID, int ProductID, int quantity, int Sent)
        {
            bool isSuccesFull = false;
            try
            {
                int x = 0;
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
                        command = new SqlCommand("sp_EntryTransferDetails_Receive", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetailID);
                        command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);
                        command.Parameters.AddWithValue("@p_TransferredQty", stockSet[id]);

                        command.Parameters.AddWithValue("@p_RequestedQty", dt.Rows[0]["RequestedQty"]);

                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);

                        x = command.ExecuteNonQuery();

                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                        command.Parameters.AddWithValue("@p_Action", "Minus");
                        command.ExecuteNonQuery();
                    }

                }
                #endregion

                if (x == 1) 
                {
                    isSuccesFull = true;
                }
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
            }
            return isSuccesFull;
        }
       

        protected void dgvReceiveTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int TransferNo, TransferDetailNo, RequestedQty, TransferedQty, ReceivedQty, AvailableQty, ProductId;
                int LogedInStoreID;

                int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);
                GridView dgvReceiveTransfer = (GridView)sender;
                Label lblTransferNo = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblRequestNo");
                Label lblTransferDetailsID = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblTransferDetailsID");
                Label lblRequestedQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblRequestedQty");
                Label lblAvailableQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblAvailableQty");
                Label lblSentQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSentQty");
                Label lblProductID = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblProductID");

                int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);

                int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                int.TryParse(lblProductID.Text.ToString(), out ProductId);
                if (e.CommandName == "Edit")
                {
                    Session["TransferDetailID"] = TransferDetailNo;
                    Session["TransferedQty"] = RequestedQty;
                    Response.Redirect("ReceiveTransferDetails_ReceiveEntry.aspx");
                }
                if (e.CommandName == "AcceptProductTransfer")
                {
                    if (AvailableQty > 0)
                    {
                        bool isSuccessful = false;
                        //update transfer entry first
                        if (RequestedQty != TransferedQty)
                        {
                            if (TransferedQty == 0)
                            {
                                isSuccessful = UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty);
                            }
                            else
                            {
                                isSuccessful = UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);
                            }
                        }

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }


                        SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_TransferDetialID", connection);
                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                        command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                        command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                        if (isSuccessful)
                            command.Parameters.AddWithValue("@p_TransferedQty", RequestedQty);
                        else
                            command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                        command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                        command.Parameters.AddWithValue("@p_Status", "Accepted");
                        command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                        command.Parameters.AddWithValue("@p_ProductID", ProductId);

                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                        btnAccept.Visible = false;
                        Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                        btnDeny.Visible = false;
                        Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                        btnEdit.Visible = false;
                        Button btnStaticAccepted = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnStaticAccepted");
                        btnStaticAccepted.Visible = true;
                        if (RequestedQty != TransferedQty)
                        {
                            if (TransferedQty == 0)
                            {
                                UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty);
                            }
                            else
                            {
                                UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);
                            }
                        }
                    }
                    else 
                    {
                        WebMessageBoxUtil.Show("No Stock Available");
                    }
                }
                if (e.CommandName == "DenyProductTransfer")
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_DenyTransferRequest_TransferDetialID", connection);
                    command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                    command.Parameters.AddWithValue("@p_RequestedTransferedQty", TransferedQty);
                    command.Parameters.AddWithValue("@p_ReceivedQty", 0);
                    command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                    command.Parameters.AddWithValue("@p_Status", "Denied");
                    command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductId);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();


                    Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                    btnAccept.Visible = false;
                    Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                    btnDeny.Visible = false;
                    Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                    btnEdit.Visible = false;

                    Label lblStaticDeny = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblStaticDeny");
                    lblStaticDeny.Visible = true;

                }

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

        }

        protected Boolean IsStatusComplete(String status)
        {
            if (status.Equals("Accepted") || status.Equals("Denied"))
            {
                return true;
            }
            else
                return false;
        }
        protected Boolean IsStatusAccepted(String status)
        {
            if (status.Equals("Accepted"))
            {
                return true;
            }
            else
                return false;
        } 
        protected Boolean IsStatusDenied(String status)
        {
            if (status.Equals("Denied"))
            {
                return true;
            }
            else
                return false;
        }
        protected void dgvReceiveTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Button abc = (Button)e.Row.FindControl("btnAccept");
            //    if (dsStatic.Tables[0].Rows[e.Row.RowIndex]["TransferStatus"].ToString() == "Accepted")
            //    {
            //        GridView gv = (GridView)sender;
            //        Button btnAccept = (Button)e.Row.FindControl("btnAccept");
            //        btnAccept.Visible = false;
            //        Button btnDeny = (Button)e.Row.FindControl("btnDeny");
            //        btnDeny.Visible = false;
            //        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
            //        btnEdit.Visible = false;
            //        Button btnStaticAccepted = (Button)e.Row.FindControl("btnStaticAccepted");
            //        btnStaticAccepted.Visible = true;
            //    }
            //}


        }

        
    }
}