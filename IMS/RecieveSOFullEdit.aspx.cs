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

                if (ds.Tables[0].Rows[0]["DefectedStatus"].ToString()=="2")
                {
                    Session["Damaged"] = ds.Tables[0].Rows[0]["DamagedQuantity"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ExpiredStatus"].ToString() == "2")
                {
                    Session["Expired"] = ds.Tables[0].Rows[0]["ExpiredQuantity"].ToString();
                }
                if (ds.Tables[0].Rows[0]["ReturnedStatus"].ToString() == "2")
                {
                    Session["Returned"] = ds.Tables[0].Rows[0]["ReturnedQuantity"].ToString();
                }

                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddDamagedAction")).SelectedValue = ds.Tables[0].Rows[0]["DefectedStatus"].ToString();// Session["ddDamagedAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddExpiredAction")).SelectedValue = ds.Tables[0].Rows[0]["ExpiredStatus"].ToString();//Session["ddExpiredAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddNotAcceptedAction")).SelectedValue = ds.Tables[0].Rows[0]["ReturnedStatus"].ToString();//Session["ddNotAcceptedAction"].ToString();
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

                int Damaged = 0, Expired = 0, Returned = 0;
                string status, storedAt, barcode, batchNo = "";
                DateTime dateCreated, dateUpdated, expiryDate = DateTime.Today;
                double PercentDiscount, UnitCostPrice, uSalePrice;
                string ProductID = "";
                int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty, RejectedQty = 0;
                string ProductDescription, ExpiryDate, BatchNo = "";
                if (Session["Damaged"] != null)
                {
                    int.TryParse(Session["Damaged"].ToString(), out Damaged);
                }
                if (Session["Expired"] != null)
                {
                    int.TryParse(Session["Expired"].ToString(), out Expired);
                }
                if (Session["Returned"] != null)
                {
                    int.TryParse(Session["Returned"].ToString(), out Returned);
                }
                
              

                DataSet dsMajorSO = (DataSet)Session["dsMajorSOReceive"];
                DataTable dtMajorSO = dsMajorSO.Tables[0];
                if (dtMajorSO != null)
                {
                    ProductID = dtMajorSO.Rows[RowIndex]["ProductID"].ToString();
                    //status = dtMajorSO.Rows[RowIndex]["Status"].ToString();
                    //storedAt = dtMajorSO.Rows[RowIndex]["StoredAt"].ToString();
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

                    ProductDescription = ((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ProductDescription")).Text;
                    ExpiryDate = ((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblExpiryDate")).Text;
                    BatchNo = ((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblBatchNumber")).Text;

                    OrderDetId = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("OrderDetID")).Text);
                    AvailableQty = int.Parse(((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("lblAvailableStock")).Text);
                    SentQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("SendQuantityVal")).Text);
                    BonusQty = int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("BonusQuantityVal")).Text);
                    DelieveredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DelieveredQtyVal")).Text);
                    DelieveredBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("delBonusQtyVal")).Text);
                    DamagedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DamagedQuantityVal")).Text);
                    ExpiredQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtExpiredQuantity")).Text);
                    RejectedQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtReturnedQuantity")).Text);
                    PercentDiscount = Double.Parse(((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtDiscountPercentage")).Text);

                    DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

                    if (SentQty < (DelieveredQty + ExpiredQty +  RejectedQty + DamagedQty))
                    {
                        WebMessageBoxUtil.Show("Mismatch in received and accepted quantity. Kindly correctly fill expired,defected or returned quantities");
                        return;
                    }
                    if(BonusQty < DelieveredBonusQty)
                    {
                        WebMessageBoxUtil.Show("Delivered Bonus quantity cannot be greater than Bonus quantity");
                        return;
                    }
                    var ddDamagedAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedAction")).SelectedValue; 
                    var ddExpiredAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredAction")).SelectedValue; 
                    var ddNotAcceptedAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedAction")).SelectedValue;

                    int OldStockSum = Damaged + Expired + Returned;

                    //Now call the Update Stock Store Procedure and send the OldStockSum as negative value to it

                    if (OldStockSum > 0)
                    {
                        //add value to stock for this expiry
                        SqlCommand command = new SqlCommand("sp_updatetblStockDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@quantity", -OldStockSum);
                        command.Parameters.AddWithValue("@ProductID", int.Parse(ProductID));
                        command.Parameters.AddWithValue("@expirydate", ExpiryDate);
                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Added");

                    }
                    Session.Remove("Damaged");
                    Session.Remove("Expired");
                    Session.Remove("Returned");
                    
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
                        command.Parameters.AddWithValue("@ProductID", int.Parse(ProductID));
                        command.Parameters.AddWithValue("@expirydate", ExpiryDate);
                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Added");

                    } 
                    SqlCommand command2 = new SqlCommand("sp_UpdatetblSaleOrderDetail_Receive_entry", connection);
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.AddWithValue("@bonusQty", BonusQty);
                    command2.Parameters.AddWithValue("@SentQty", SentQty);
                    command2.Parameters.AddWithValue("@delieveredQty", DelieveredQty);
                    command2.Parameters.AddWithValue("@delieveredBonusQty", DelieveredBonusQty);
                    command2.Parameters.AddWithValue("@defectedQty", DamagedQty);
                    command2.Parameters.AddWithValue("@defectedStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedAction")).SelectedValue));

                    command2.Parameters.AddWithValue("@expiredQty", ExpiredQty);
                    command2.Parameters.AddWithValue("@expiredStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredAction")).SelectedValue));

                    command2.Parameters.AddWithValue("@returnedQty", RejectedQty);
                    command2.Parameters.AddWithValue("@returnedStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedAction")).SelectedValue));

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
            Response.Redirect("RecieveSOFull.aspx");
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
                foreach (TableCell cell in e.Row.Cells)
                {
                     
                    string s = cell.Text;
                    if (cell.Text.Length > 25 )
                        cell.Text = cell.Text.Substring(0, 25) + "<br/>";
                    cell.ToolTip = s;
                }
            }

            
        }

        protected void txtReturnedQuantity_TextChanged(object sender, EventArgs e)
        {
            int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty, RejectedQty = 0;
            TextBox txtDelieveredQty = (TextBox)dgvReceiveSOGrid.Rows[0].FindControl("DelieveredQtyVal");

            OrderDetId = int.Parse(((Label)dgvReceiveSOGrid.Rows[0].FindControl("OrderDetID")).Text);
            AvailableQty = int.Parse(((Literal)dgvReceiveSOGrid.Rows[0].FindControl("lblAvailableStock")).Text);
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