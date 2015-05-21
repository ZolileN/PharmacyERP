using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class RecieveSOFullEdit : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            #region Display Products
            try
            {
                connection.Open();
                 
                SqlCommand command = new SqlCommand("Sp_GetSODetail_RecByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                DataSet ds = new DataSet();

                if (int.TryParse(Session["OrderDetNo"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@OrderDetID", OrderNumber);
                }

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                Session["dsMajorSOReceive"] = ds;
                dgvReceiveSOGrid.DataSource = null;
                dgvReceiveSOGrid.DataSource = ds.Tables[0];
                dgvReceiveSOGrid.DataBind();

                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddDamagedAction")).SelectedValue = Session["ddDamagedAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddExpiredAction")).SelectedValue = Session["ddExpiredAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddNotAcceptedAction")).SelectedValue = Session["ddNotAcceptedAction"].ToString();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }

        protected void dgvReceiveSOGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                int RowIndex = gvr.RowIndex;

                int stockid, productId, saleOrderMasterId, saleOrderDetailId = 0;
                string status, storedAt, barcode, batchNo = "";
                DateTime dateCreated, dateUpdated, expiryDate = DateTime.Today;

                double PercentDiscount, UnitCostPrice, uSalePrice;
                int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty, RejectedQty = 0;
                string ProductDescription, ExpiryDate, BatchNo = "";

                DataSet dsMajorSO = (DataSet)Session["dsMajorSOReceive"];
                DataTable dtMajorSO = dsMajorSO.Tables[0];
                if (dtMajorSO != null)
                {
                  
                    status = dtMajorSO.Rows[RowIndex]["Status"].ToString();
                    storedAt = dtMajorSO.Rows[RowIndex]["StoredAt"].ToString();
                    barcode = dtMajorSO.Rows[RowIndex]["BarCode"].ToString();
                    batchNo = dtMajorSO.Rows[RowIndex]["BatchNumber"].ToString();
                    dateCreated = DateTime.Today;
                    dateUpdated = DateTime.Now;
                    Double.TryParse(dtMajorSO.Rows[RowIndex]["DiscountPercentage"].ToString(), out PercentDiscount);
                }

                if (e.CommandName.Equals("UpdateStock"))
                {
                    if(connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    OrderDetId = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("OrderDetID")).Text);
                    AvailableQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblAvailableStock")).Text);
                    SentQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("SendQuantityVal")).Text);

                    ProductDescription = ((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ProductDescription")).Text;
                    ExpiryDate = ((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblExpiryDate")).Text;
                    BatchNo = ((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblBatchNumber")).Text;


                    BonusQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("BonusQuantityVal")).Text);
                    DelieveredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DelieveredQtyVal")).Text);
                    DelieveredBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("delBonusQtyVal")).Text);
                    DamagedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DamagedQuantityVal")).Text);
                    ExpiredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtExpiredQuantity")).Text);
                    RejectedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtReturnedQuantity")).Text);
                    PercentDiscount = Double.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtDiscountPercentage")).Text);

                    DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

                    if (SentQty > (DelieveredQty + ExpiredQty + RejectedQty + DamagedQty))
                    {
                        WebMessageBoxUtil.Show("Mismatch in received and accepted quantity. Kindly correctly fill expired,defected or returned quantities");
                        return;
                    }
                    if(BonusQty < DelieveredBonusQty)
                    {
                        WebMessageBoxUtil.Show("Delivered quantity cannot be greater than Bonus quantity");
                        return;
                    }
                    var ddDamagedAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedAction")).SelectedValue;
                    Session["ddDamagedAction"] = ddDamagedAction;
                    var ddExpiredAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredAction")).SelectedValue;
                    Session["ddExpiredAction"] = ddExpiredAction;
                    var ddNotAcceptedAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedAction")).SelectedValue;
                    Session["ddNotAcceptedAction"] = ddNotAcceptedAction;

                    int addtoStockQty = 0;
                    if (ddDamagedAction == "2")
                    {
                        addtoStockQty = addtoStockQty + DamagedQty;
                    }
                    if (ddExpiredAction == "2")
                    {
                        addtoStockQty = addtoStockQty + ExpiredQty;
                    }
                    if (ddNotAcceptedAction == "2")
                    {
                        addtoStockQty = addtoStockQty + RejectedQty;
                    }
                    if (addtoStockQty > 0)
                    {
                        //add value to stock for this expiry
                        SqlCommand command = new SqlCommand("sp_updatetblStockDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@quantity", addtoStockQty);
                        command.Parameters.AddWithValue("@expirydate", expiryDate);
                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Added");

                    }
                    if (DamagedQty > 0) 
                    {
                        //insert values in the newly created table
                        SqlCommand comm = new SqlCommand("sp_Insert_tblSOReceiveStockDetail_entry", connection);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@stockId", int.TryParse(dtMajorSO.Rows[RowIndex]["StockID"].ToString(), out stockid));
                        comm.Parameters.AddWithValue("@productID",  int.TryParse(dtMajorSO.Rows[RowIndex]["ProductID"].ToString(), out productId));
                        comm.Parameters.AddWithValue("@quantity", DamagedQty);
                        comm.Parameters.AddWithValue("@status", ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedAction")).SelectedValue);
                        comm.Parameters.AddWithValue("@dateCreated", DateTime.Today);
                        comm.Parameters.AddWithValue("@dateUpdated", DateTime.Now);
                        comm.Parameters.AddWithValue("@storedAt", dtMajorSO.Rows[RowIndex]["StoredAt"].ToString());
                        comm.Parameters.AddWithValue("@barCode", dtMajorSO.Rows[RowIndex]["BarCode"].ToString());
                        comm.Parameters.AddWithValue("@expiryDate", DateTime.Parse(ExpiryDate));
                        comm.Parameters.AddWithValue("@batchNumber", dtMajorSO.Rows[RowIndex]["BatchNumber"].ToString());
                        comm.Parameters.AddWithValue("@uCostPrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitCost"].ToString(), out UnitCostPrice));
                        comm.Parameters.AddWithValue("@uSalePrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitSale"].ToString(), out uSalePrice));
                        comm.Parameters.AddWithValue("@saleOrderMasterID", int.TryParse(dtMajorSO.Rows[RowIndex]["OrdermasterID"].ToString(), out saleOrderMasterId));
                        comm.Parameters.AddWithValue("@saleOrderDetailID", OrderDetId);
                        comm.Parameters.AddWithValue("@discountPercentage", PercentDiscount);
                        comm.ExecuteNonQuery();
                    
                    }
                    if(ExpiredQty > 0)
                    {
                        SqlCommand comm2 = new SqlCommand("sp_Insert_tblSOReceiveStockDetail_entry", connection);
                        comm2.CommandType = CommandType.StoredProcedure;
                        comm2.Parameters.AddWithValue("@stockId", int.TryParse(dtMajorSO.Rows[RowIndex]["StockID"].ToString(), out stockid));
                        comm2.Parameters.AddWithValue("@productID", int.TryParse(dtMajorSO.Rows[RowIndex]["ProductID"].ToString(), out productId));
                        comm2.Parameters.AddWithValue("@quantity", ExpiredQty);
                        comm2.Parameters.AddWithValue("@status", ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredAction")).SelectedValue);
                        comm2.Parameters.AddWithValue("@dateCreated", DateTime.Today);
                        comm2.Parameters.AddWithValue("@dateUpdated", DateTime.Now);
                        comm2.Parameters.AddWithValue("@storedAt", dtMajorSO.Rows[RowIndex]["StoredAt"].ToString());
                        comm2.Parameters.AddWithValue("@barCode", dtMajorSO.Rows[RowIndex]["BarCode"].ToString());
                        comm2.Parameters.AddWithValue("@expiryDate", DateTime.Parse(ExpiryDate));
                        comm2.Parameters.AddWithValue("@batchNumber", dtMajorSO.Rows[RowIndex]["BatchNumber"].ToString());
                        comm2.Parameters.AddWithValue("@uCostPrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitCost"].ToString(), out UnitCostPrice));
                        comm2.Parameters.AddWithValue("@uSalePrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitSale"].ToString(), out uSalePrice));
                        comm2.Parameters.AddWithValue("@saleOrderMasterID", int.TryParse(dtMajorSO.Rows[RowIndex]["OrdermasterID"].ToString(), out saleOrderMasterId));
                        comm2.Parameters.AddWithValue("@saleOrderDetailID", OrderDetId);
                        comm2.Parameters.AddWithValue("@discountPercentage", PercentDiscount);
                        comm2.ExecuteNonQuery();
                    }
                    if (RejectedQty > 0)
                    {
                        SqlCommand comm3 = new SqlCommand("sp_Insert_tblSOReceiveStockDetail_entry", connection);
                        comm3.CommandType = CommandType.StoredProcedure;
                        comm3.Parameters.AddWithValue("@stockId", int.TryParse(dtMajorSO.Rows[RowIndex]["StockID"].ToString(), out stockid));
                        comm3.Parameters.AddWithValue("@productID", int.TryParse(dtMajorSO.Rows[RowIndex]["ProductID"].ToString(), out productId));
                        comm3.Parameters.AddWithValue("@quantity", RejectedQty);
                        comm3.Parameters.AddWithValue("@status", ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedAction")).SelectedValue);
                        comm3.Parameters.AddWithValue("@dateCreated", DateTime.Today);
                        comm3.Parameters.AddWithValue("@dateUpdated", DateTime.Now);
                        comm3.Parameters.AddWithValue("@storedAt", dtMajorSO.Rows[RowIndex]["StoredAt"].ToString());
                        comm3.Parameters.AddWithValue("@barCode", dtMajorSO.Rows[RowIndex]["BarCode"].ToString());
                        comm3.Parameters.AddWithValue("@expiryDate", DateTime.Parse(ExpiryDate));
                        comm3.Parameters.AddWithValue("@batchNumber", dtMajorSO.Rows[RowIndex]["BatchNumber"].ToString());
                        comm3.Parameters.AddWithValue("@uCostPrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitCost"].ToString(), out UnitCostPrice));
                        comm3.Parameters.AddWithValue("@uSalePrice", Double.TryParse(dtMajorSO.Rows[RowIndex]["UnitSale"].ToString(), out uSalePrice));
                        comm3.Parameters.AddWithValue("@saleOrderMasterID", int.TryParse(dtMajorSO.Rows[RowIndex]["OrdermasterID"].ToString(), out saleOrderMasterId));
                        comm3.Parameters.AddWithValue("@saleOrderDetailID", OrderDetId);
                        comm3.Parameters.AddWithValue("@discountPercentage", PercentDiscount);
                        comm3.ExecuteNonQuery();
                    }
                     
                    SqlCommand command2 = new SqlCommand("sp_UpdatetblSaleOrderDetail_Receive_entry", connection);
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.AddWithValue("@delieveredQty", DelieveredQty);
                    command2.Parameters.AddWithValue("@defectedQty", DamagedQty);
                    command2.Parameters.AddWithValue("@expiredQty", ExpiredQty);
                    command2.Parameters.AddWithValue("@returnedQty", RejectedQty);
                    command2.Parameters.AddWithValue("@expiredDate", DateTime.Parse(ExpiryDate));
                    command2.Parameters.AddWithValue("@OrderDetId", OrderDetId);
                    command2.ExecuteNonQuery();
                }
                Response.Redirect("RecieveSOFull.aspx");
            }
            catch(Exception ex)
            {
                connection.Close();
            }
            finally
            {
                connection.Close();
            }
        }


        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            //First clear all the sessions if created in this page
            Response.Redirect("ReceiveSalesOrder.aspx");
        }


        protected Boolean IsStatusNotComplete(String status)
        {
            if (status.Equals("Complete") || status.Equals("Initiated"))
            {
                return false;
            }
            else
                return true;
        }

        protected void dgvReceiveSOGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvReceiveSOGrid.EditIndex = e.NewEditIndex;
            //BindGrid();
            int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty, RejectedQty = 0;
             
            int RowIndex = dgvReceiveSOGrid.EditIndex;

            TextBox txtDelieveredQty = (TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DelieveredQtyVal");
            TextBox txtRejected = (TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtReturnedQuantity");
            txtRejected.TextChanged += txtReturnedQuantity_TextChanged;

            DelieveredQty = int.Parse(txtDelieveredQty.Text);

            OrderDetId = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("OrderDetID")).Text);
            AvailableQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblAvailableStock")).Text);
            SentQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("SendQuantityVal")).Text);

            BonusQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("BonusQuantityVal")).Text);
            DelieveredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DelieveredQtyVal")).Text);
            DelieveredBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("delBonusQtyVal")).Text);
            DamagedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DamagedQuantityVal")).Text);
            ExpiredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtExpiredQuantity")).Text);
            RejectedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtReturnedQuantity")).Text);
            
            DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

            txtDelieveredQty.Text = DelieveredQty.ToString();
        }

        private int CalculateDelieveredQty(int SentQty, int BonusQty, int DelieveredBonusQty, int DamagedQty, int ExpiredQty, int RejectedQty)
        {
            int DelieveredQty;
            DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

            return DelieveredQty;
        }
         

        protected void dgvReceiveSOGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                TableCell statusCell = e.Row.Cells[2];
                if (statusCell.Text == "A")
                {
                    statusCell.Text = "Absent";
                }
                if (statusCell.Text == "P")
                {
                    statusCell.Text = "Present";
                }
            }
        }

        protected void txtReturnedQuantity_TextChanged(object sender, EventArgs e)
        {
            int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty, RejectedQty = 0;
            TextBox txtDelieveredQty = (TextBox)dgvReceiveSOGrid.Rows[0].FindControl("DelieveredQtyVal");

            OrderDetId = int.Parse(((Label)dgvReceiveSOGrid.Rows[0].FindControl("OrderDetID")).Text);
            AvailableQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[0].FindControl("lblAvailableStock")).Text);
            SentQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[0].FindControl("SendQuantityVal")).Text);

            BonusQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[0].FindControl("BonusQuantityVal")).Text);
            DelieveredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[0].FindControl("DelieveredQtyVal")).Text);
            DelieveredBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[0].FindControl("delBonusQtyVal")).Text);
            DamagedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[0].FindControl("DamagedQuantityVal")).Text);
            ExpiredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[0].FindControl("txtExpiredQuantity")).Text);
            RejectedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[0].FindControl("txtReturnedQuantity")).Text);

            DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

            txtDelieveredQty.Text = DelieveredQty.ToString();
        }

    }
}