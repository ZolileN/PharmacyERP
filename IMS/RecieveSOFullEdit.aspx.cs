using IMS.Util;
using IMSCommon.Util;
using log4net;
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
            #region Display Products
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                 
                SqlCommand command = new SqlCommand("Sp_GetSODetail_RecByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                DataSet ds = new DataSet();

                if (int.TryParse(Session["OrderDetailsNo"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@OrderDetID", OrderNumber);
                }

                

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                Session["dsMajorSOReceive"] = ds;
                dgvReceiveSOGrid.DataSource = null;
                dgvReceiveSOGrid.DataSource = ds.Tables[0];
                dgvReceiveSOGrid.DataBind();
                int i = 0;
                if (ds.Tables[0].Rows[0]["DefectedStatus"].ToString()=="2")
                {
                    Session["Damaged"] = ds.Tables[0].Rows[0]["DamagedQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["DamagedQuantity"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ExpiredStatus"].ToString() == "2")
                {
                    Session["Expired"] = ds.Tables[0].Rows[0]["ExpiredQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["ExpiredQuantity"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReturnedStatus"].ToString() == "2")
                {
                    Session["Returned"] = ds.Tables[0].Rows[0]["ReturnedQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["ReturnedQuantity"].ToString());
                }

                if (ds.Tables[0].Rows[0]["DefectedBonusStatus"].ToString() == "2")
                {
                    Session["DamagedBonus"] = ds.Tables[0].Rows[0]["DefectedBonusQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["DefectedBonusQuantity"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ExpiredBonusStatus"].ToString() == "2")
                {
                    Session["ExpiredBonus"] = ds.Tables[0].Rows[0]["ExpiredBonusQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["ExpiredBonusQuantity"].ToString());
                }
                if (ds.Tables[0].Rows[0]["ReturnedBonusStatus"].ToString() == "2")
                {
                    Session["ReturnedBonus"] = ds.Tables[0].Rows[0]["ReturnedBonusQuantity"].ToString();
                    i += Convert.ToInt32(ds.Tables[0].Rows[0]["ReturnedBonusQuantity"].ToString());
                }

                

                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddDamagedAction")).SelectedValue = ds.Tables[0].Rows[0]["DefectedStatus"].ToString();// Session["ddDamagedAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddExpiredAction")).SelectedValue = ds.Tables[0].Rows[0]["ExpiredStatus"].ToString();//Session["ddExpiredAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddNotAcceptedAction")).SelectedValue = ds.Tables[0].Rows[0]["ReturnedStatus"].ToString();//Session["ddNotAcceptedAction"].ToString();

                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddDamagedBonusAction")).SelectedValue = ds.Tables[0].Rows[0]["DefectedBonusStatus"].ToString();// Session["ddDamagedAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddExpiredBonusAction")).SelectedValue = ds.Tables[0].Rows[0]["ExpiredBonusStatus"].ToString();//Session["ddExpiredAction"].ToString();
                ((DropDownList)dgvReceiveSOGrid.Rows[0].FindControl("ddNotAcceptedBonusAction")).SelectedValue = ds.Tables[0].Rows[0]["ReturnedBonusStatus"].ToString();//Session["ddNotAcceptedAction"].ToString();

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
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
                int DamagedBonus = 0, ExpiredBonus = 0, ReturnedBonus = 0;
                string status, storedAt, barcode, batchNo = "";
                DateTime dateCreated, dateUpdated, expiryDate = DateTime.Today;
                double PercentDiscount, UnitCostPrice, uSalePrice;
                string ProductID = "";
                int OrderDetId, AvailableQty, SentQty, BonusQty, DelieveredQty, DelieveredBonusQty, DamagedQty, ExpiredQty,
                    RejectedQty, DamagedBonusQty, ExpiredBonusQty, RejectedBonusQty = 0;
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


                if (Session["DamagedBonus"] != null)
                {
                    int.TryParse(Session["DamagedBonus"].ToString(), out DamagedBonus);
                }
                if (Session["ExpiredBonus"] != null)
                {
                    int.TryParse(Session["ExpiredBonus"].ToString(), out ExpiredBonus);
                }
                if (Session["ReturnedBonus"] != null)
                {
                    int.TryParse(Session["ReturnedBonus"].ToString(), out ReturnedBonus);
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
                    DateTime Expiry;

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

                    DamagedBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("DamagedBonusQuantityVal")).Text);
                    ExpiredBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtExpiredBonusQuantity")).Text);
                    RejectedBonusQty = int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtReturnedBonusQuantity")).Text);

                    PercentDiscount = Double.Parse(((Literal)dgvReceiveSOGrid.Rows[RowIndex].FindControl("txtDiscountPercentage")).Text);

                    //DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);
                    DelieveredQty = SentQty - (DamagedQty + ExpiredQty + RejectedQty);

                    if (SentQty < ( ExpiredQty +  RejectedQty + DamagedQty))
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


                    var ddDamagedBonusAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedBonusAction")).SelectedValue;
                    var ddExpiredBonusAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredBonusAction")).SelectedValue;
                    var ddNotAcceptedBonusAction = ((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedBonusAction")).SelectedValue;

                    int OldStockSum = Damaged + Expired + Returned + DamagedBonus + ExpiredBonus + ReturnedBonus;

                    //Now call the Update Stock Store Procedure and send the OldStockSum as negative value to it

                    if (OldStockSum > 0)
                    {
                        //add value to stock for this expiry
                        SqlCommand command = new SqlCommand("sp_updatetblStockDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@quantity", -OldStockSum);
                        command.Parameters.AddWithValue("@ProductID", int.Parse(ProductID));
                        if (DateTime.TryParse(ExpiryDate, out Expiry))
                        {
                            command.Parameters.AddWithValue("@expirydate", Expiry);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@expirydate", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@p_SysID", int.Parse(Session["UserSys"].ToString()));
                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Added");

                    }
                    Session.Remove("Damaged");
                    Session.Remove("Expired");
                    Session.Remove("Returned");
                    Session.Remove("DamagedBonus");
                    Session.Remove("ExpiredBonus");
                    Session.Remove("ReturnedBonus");
                    
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

                    if (ddDamagedBonusAction == "2")
                    {
                        addtoStockQty = addtoStockQty + DamagedBonusQty;
                    }
                    if (ddExpiredBonusAction == "2")
                    {
                        addtoStockQty = addtoStockQty + ExpiredBonusQty;
                    }
                    if (ddNotAcceptedBonusAction == "2")
                    {
                        addtoStockQty = addtoStockQty + RejectedBonusQty;
                    }

                    
                    if (addtoStockQty > 0)
                    {
                        //add value to stock for this expiry
                        SqlCommand command = new SqlCommand("sp_updatetblStockDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@quantity", addtoStockQty);
                        command.Parameters.AddWithValue("@ProductID", int.Parse(ProductID));

                        if (DateTime.TryParse(ExpiryDate, out Expiry))
                        {
                            command.Parameters.AddWithValue("@expirydate", Expiry);

                        }
                        else
                        {
                            command.Parameters.AddWithValue("@expirydate", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@p_SysID", int.Parse(Session["UserSys"].ToString()));

                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Added");

                    } 
                    SqlCommand command2 = new SqlCommand("sp_UpdatetblSaleOrderDetail_Receive_entry", connection);
                    command2.CommandType = CommandType.StoredProcedure;
                    command2.Parameters.AddWithValue("@bonusQty", BonusQty);
                    command2.Parameters.AddWithValue("@SentQty", SentQty);
                    command2.Parameters.AddWithValue("@delieveredQty", DelieveredQty);
                    command2.Parameters.AddWithValue("@delieveredBonusQty", DelieveredBonusQty);

                    //----------Damaged Quantity and its Status
                    command2.Parameters.AddWithValue("@defectedQty", DamagedQty);
                    command2.Parameters.AddWithValue("@defectedStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedAction")).SelectedValue));

                    //----------Expired Quantity and its Status
                    command2.Parameters.AddWithValue("@expiredQty", ExpiredQty);
                    command2.Parameters.AddWithValue("@expiredStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredAction")).SelectedValue));

                    //----------Returned Quantity and its Status
                    command2.Parameters.AddWithValue("@returnedQty", RejectedQty);
                    command2.Parameters.AddWithValue("@returnedStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedAction")).SelectedValue));

                    //----------Defected Bonus Quantity and its Status
                    command2.Parameters.AddWithValue("@defectedBonusQty", DamagedBonusQty);
                    command2.Parameters.AddWithValue("@defectedBonusStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddDamagedBonusAction")).SelectedValue));

                    //----------Expired Bonus Quantity and its Status
                    command2.Parameters.AddWithValue("@expiredBonusQty", ExpiredBonusQty);
                    command2.Parameters.AddWithValue("@expiredBonusStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddExpiredBonusAction")).SelectedValue));

                    //----------Returned Bonus Quantity and its Status
                    command2.Parameters.AddWithValue("@returnedBonusQty", RejectedBonusQty);
                    command2.Parameters.AddWithValue("@returnedBonusStatus", int.Parse(((DropDownList)dgvReceiveSOGrid.Rows[RowIndex].FindControl("ddNotAcceptedBonusAction")).SelectedValue));


                    if (DateTime.TryParse(ExpiryDate, out Expiry))
                    {
                        command2.Parameters.AddWithValue("@expiredDate", Expiry);

                    }
                    else
                    {
                        command2.Parameters.AddWithValue("@expiredDate", DBNull.Value);
                    }
                     
                    command2.Parameters.AddWithValue("@OrderDetId", OrderDetId);
                    command2.ExecuteNonQuery();

                    //Update Stock Receiving for particular store   

                    SqlCommand command3 = new SqlCommand("Sp_UpdateStockonReceiving", connection);
                    command3.CommandType = CommandType.StoredProcedure;

                    command3.Parameters.AddWithValue("@p_ReceivedQuantity", DelieveredQty);
                    command3.Parameters.AddWithValue("@p_StoreID", int.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("StoreId")).Text));
                    command3.Parameters.AddWithValue("@p_ProductID", int.Parse(ProductID));
                    command3.Parameters.AddWithValue("@p_BarCode", long.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("BarCode")).Text));
                    command3.Parameters.AddWithValue("@p_Bonus", int.Parse(((TextBox)dgvReceiveSOGrid.Rows[RowIndex].FindControl("delBonusQtyVal")).Text));

                    if (DateTime.TryParse(ExpiryDate, out Expiry))
                    {
                        command3.Parameters.AddWithValue("@p_Expiry", Expiry);

                    }
                    else
                    {
                        command3.Parameters.AddWithValue("@p_Expiry", DBNull.Value);
                    }
                     
                    command3.Parameters.AddWithValue("@p_Cost", double.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("UnitCost")).Text));
                    command3.Parameters.AddWithValue("@p_Sales", double.Parse(((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("UnitSale")).Text));     
                    command3.Parameters.AddWithValue("@p_isPO", "TRUE");
                    command3.Parameters.AddWithValue("@p_BatchNumber", ((Label)dgvReceiveSOGrid.Rows[RowIndex].FindControl("BatchNumber")).Text);

                    command3.ExecuteNonQuery();
                }
                Response.Redirect("RecieveSOFull.aspx");
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }


        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            //First clear all the sessions if created in this page
            Response.Redirect("RecieveSOFull.aspx",false);
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
            try
            {
                dgvReceiveSOGrid.EditIndex = e.NewEditIndex;
                BindGrid();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private int CalculateDelieveredQty(int SentQty, int BonusQty, int DelieveredBonusQty, int DamagedQty, int ExpiredQty, int RejectedQty)
        {
            int DelieveredQty;
            DelieveredQty = SentQty + (BonusQty - DelieveredBonusQty) - (DamagedQty + ExpiredQty + RejectedQty);

            return DelieveredQty;
        }
         

        protected void dgvReceiveSOGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    TextBox txtDelBonus = (TextBox)e.Row.FindControl("delBonusQtyVal");

                    TextBox txtDamagedQuantity = (TextBox)e.Row.FindControl("DamagedQuantityVal");
                    TextBox txtExpiredQuantity = (TextBox)e.Row.FindControl("txtExpiredQuantity");
                    TextBox txtReturnedQuantity = (TextBox)e.Row.FindControl("txtReturnedQuantity");


                    TextBox txtDamagedBonusQuantity = (TextBox)e.Row.FindControl("DamagedBonusQuantityVal");
                    TextBox txtExpiredBonusQuantity = (TextBox)e.Row.FindControl("txtExpiredBonusQuantity");
                    TextBox txtReturnedBonusQuantity = (TextBox)e.Row.FindControl("txtReturnedBonusQuantity");

                    DataRowView drv = (DataRowView)e.Row.DataItem;
                    int id = (int)e.Row.RowIndex;


                    //txtDelBonus.Attributes.Add("onchange", "SetDelieveredQty(" + id + ");return false;");

                    txtDamagedQuantity.Attributes.Add("onchange", "SetDelieveredQty(" + id + ");return false;");
                    txtExpiredQuantity.Attributes.Add("onchange", "SetDelieveredQty(" + id + ");return false;");
                    txtReturnedQuantity.Attributes.Add("onchange", "SetDelieveredQty(" + id + ");return false;");


                    txtDamagedBonusQuantity.Attributes.Add("onchange", "SetDelieveredBonusQty(" + id + ");return false;");
                    txtExpiredBonusQuantity.Attributes.Add("onchange", "SetDelieveredBonusQty(" + id + ");return false;");
                    txtReturnedBonusQuantity.Attributes.Add("onchange", "SetDelieveredBonusQty(" + id + ");return false;");
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            
        }

        protected void txtReturnedQuantity_TextChanged(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

    }
}