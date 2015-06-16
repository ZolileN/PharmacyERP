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
    public partial class ReceiveTransferOrder : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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
                repReceiveTransfer.DataSource = ds;
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
                dgvReceiveTransfer.DataSource = ds;
                dgvReceiveTransfer.DataBind();
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
                if (e.CommandName == "AcceptProductTransfer")
                {
                    int TransferNo, TransferDetailNo, TransferedQty, ReceivedQty, AvailableQty, ProductId;
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
                    int.TryParse(lblRequestedQty.Text.ToString(), out TransferedQty);
                    int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                    int.TryParse(lblSentQty.Text.ToString(), out ReceivedQty);
                    int.TryParse(lblProductID.Text.ToString(), out ProductId);

                    //Now update the Quanity for Received in TransferOrderDetails
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_TransferDetialID", connection);
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

                    Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                    btnAccept.Visible = false;
                    Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                    btnDeny.Visible = false;
                    Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                    btnEdit.Visible = false;
                    Button btnStaticAccepted = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnStaticAccepted");
                    btnStaticAccepted.Visible = true;
                }
                if (e.CommandName == "DenyProductTransfer")
                {
                    int TransferNo, TransferDetailNo, TransferedQty, ReceivedQty, AvailableQty, ProductId;
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
                    int.TryParse(lblRequestedQty.Text.ToString(), out TransferedQty);
                    int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                    int.TryParse(lblSentQty.Text.ToString(), out ReceivedQty);
                    int.TryParse(lblProductID.Text.ToString(), out ProductId);

                    SqlCommand command = new SqlCommand("sp_DenyTransferRequest_TransferDetialID", connection);
                    command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                    command.Parameters.AddWithValue("@p_RequestedTransferedQty", TransferedQty);
                    command.Parameters.AddWithValue("@p_ReceivedQty", ReceivedQty);
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

        //private void UpdateStockMinus(int TransferNo, int TransferedQty , int ProductID, int Sent) 
        //{
        //    #region Query stock
        //    DataSet stockDet = new DataSet();
        //    SqlCommand command = new SqlCommand("Sp_GetStockBy_ProductID", connection);
        //    command.CommandType = CommandType.StoredProcedure;
        //    command.Parameters.AddWithValue("@TransferNo", ProductID);
        //    command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

        //    DataSet ds = new DataSet();
        //    SqlDataAdapter dA = new SqlDataAdapter(command);
        //    dA.Fill(ds);
        //    stockDet = ds;
        //    #endregion
        //}
        
    }
}