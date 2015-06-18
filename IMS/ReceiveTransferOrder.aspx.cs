﻿using System;
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
    public partial class ReceiveTransferOrder : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet dsStatic = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
                DataTable distinctRequests = dsDistinct.DefaultView.ToTable(true, "TransferBy");

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

        protected void repReceiveTransfer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                GridView dgvReceiveTransfer = (GridView)e.Item.FindControl("dgvReceiveTransfer");
                Literal litStoreName = (Literal)e.Item.FindControl("litStoreName");

                DataSet ds = new DataSet();
                int Userid;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int TransferBy = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row[0].ToString());

                SqlCommand command = new SqlCommand("sp_getUserTransferDetails_TransferID", connection);
                int.TryParse(Session["UserSys"].ToString(), out Userid);
                command.Parameters.AddWithValue("@UserID", Userid);
                command.Parameters.AddWithValue("@TransferBy", TransferBy);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                dsStatic = ds;
                dgvReceiveTransfer.DataSource = ds;
                dgvReceiveTransfer.DataBind();
               
                litStoreName.Text = ds.Tables[0].Rows[0]["RequestedBy"].ToString();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
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
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_TransferDetialID", connection);
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

                    Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                    btnAccept.Visible = false;
                    Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                    btnDeny.Visible = false;
                    Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                    btnEdit.Visible = false;
                    Button btnStaticAccepted = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnStaticAccepted");
                    btnStaticAccepted.Visible = true;
                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty);
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

        protected void btnAcceptTransferOrder_Click(object sender, EventArgs e)
        {
            
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
                        int TransferNo, TransferDetailNo, TransferedQty, ReceivedQty, AvailableQty, ProductId;
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
                        int.TryParse(lblRequestedQty.Text.ToString(), out TransferedQty);
                        int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                        int.TryParse(lblSentQty.Text.ToString(), out ReceivedQty);
                        int.TryParse(lblProductID.Text.ToString(), out ProductId);

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_AcceptAll", connection);
                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                        command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                        command.Parameters.AddWithValue("@p_RequestedTransferedQty", TransferedQty);
                        command.Parameters.AddWithValue("@p_ReceivedQty", ReceivedQty);
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
                        //Update Stock in Stock Table
                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);

                    }
                }
                if (e.CommandName == "GenTransferOrder")
                {
                    GridView gvReceiveTransfer = (GridView)repReceiveTransfer.Items[0].FindControl("dgvReceiveTransfer");
                    for (int i = 0; i < gvReceiveTransfer.Rows.Count; i++)
                    {
                        int TransferNo, TransferDetailNo, TransferedQty, ReceivedQty, AvailableQty, ProductId;
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
                        int.TryParse(lblRequestedQty.Text.ToString(), out TransferedQty);
                        int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                        int.TryParse(lblSentQty.Text.ToString(), out ReceivedQty);
                        int.TryParse(lblProductID.Text.ToString(), out ProductId);

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

                        SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_AcceptAll", connection);
                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                        command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                        command.Parameters.AddWithValue("@p_RequestedTransferedQty", TransferedQty);
                        command.Parameters.AddWithValue("@p_ReceivedQty", ReceivedQty);
                        command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                        command.Parameters.AddWithValue("@p_Status", "Accepted");
                        command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                        command.Parameters.AddWithValue("@p_ProductID", ProductId);

                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        DataSet dsResults = new DataSet();
                        SqlDataAdapter da = new SqlDataAdapter(command);
                        da.Fill(dsResults);

                        //update Stock
                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);

                        Session["TransferRequestGrid"] = dsResults.Tables[0];

                        Response.Redirect("GenerateAcceptedTransferOrder.aspx", false);
                    }
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
                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);
                        
                        command.ExecuteNonQuery();

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

        protected void dgvReceiveTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            GridView dgvReceiveTransfer = (GridView)e.Row.FindControl("dgvReceiveTransfer");
            if (dsStatic.Tables[0].Rows[0]["TransferStatus"].ToString() == "Accepted")
            {
                //Button btnAccept = (Button)dgvReceiveTransfer.Rows[0].FindControl("btnAccept");
                //btnAccept.Visible = false;
                //Button btnDeny = (Button)dgvReceiveTransfer.Rows[0].FindControl("btnDeny");
                //btnDeny.Visible = false;
                //Button btnEdit = (Button)dgvReceiveTransfer.Rows[0].FindControl("btnEdit");
                //btnEdit.Visible = false;
                //Button btnStaticAccepted = (Button)dgvReceiveTransfer.Rows[0].FindControl("btnStaticAccepted");
                //btnStaticAccepted.Visible = true;
            }

        }
 
    }
}