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
using IMSCommon.Util;
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class OrderSalesManual_Details : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        public static bool TotalExceeded;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if(!IsPostBack)
            {
                try
                {
                    lblTotalQuantity.Text = Session["TotalQuantity"].ToString();
                    lblQuan.Text = Session["SO_Quan"].ToString();
                    lblBonQuan.Text = Session["SO_BQuan"].ToString();
                    //TotalExceeded = false;
                    BindGrid();
                    Session["TotalExceeded"] = false;
                }
                catch (Exception ex) 
                {

                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
            }
            expHandler.CheckForErrorMessage(Session);
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
        public void BindGrid()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_getProductStock", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_orderDetailID", Convert.ToInt32(Session["OderDetailID"].ToString()));
                command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(Session["ProductID"].ToString()));
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                #region Checking Exceeded Quantity
                int Total = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Total += Convert.ToInt32(ds.Tables[0].Rows[i]["SentQuantity"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[i]["BonusQuantity"].ToString());
                }
                if(Total >= Convert.ToInt32(lblTotalQuantity.Text.ToString()))
                {
                    Session["TotalExceeded"] = true;
                }
                else
                {
                    Session["TotalExceeded"] = false;
                }
                #endregion

                
                    if (ds != null || ds.Tables[0] != null)
                    {
                        ProductSet = ds;
                         
                        StockDisplayGrid.DataSource = ds.Tables[0];
                        StockDisplayGrid.DataBind();
                    }

            }
            catch(Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
        protected void btnAcceptStock_Click(object sender, EventArgs e)
        {
            try
            {
                int TotalQuantity, quan, bQuan;
                TotalQuantity = quan = bQuan = 0;
                int.TryParse(lblQuan.Text, out quan);
                int.TryParse(lblBonQuan.Text, out bQuan);
                int.TryParse(lblTotalQuantity.Text, out TotalQuantity);
                ////DataTable filterSet = this.StockDisplayGrid as DataTable;
                int totVal, totBonVal;
                totVal = totBonVal = 0;
                //DataView dataView =  StockDisplayGrid.DataSource;
                DataSet ds = ProductSet;
                int selectedSum = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int val, bonVal;
                    val = bonVal = 0;
                    //if (Convert.ToInt32(filterSet.Rows[i]["ProductID"]).Equals(ProductNO))
                    //{
                    //    ProductPresent = true;
                    //    break;
                    //}

                    int.TryParse(ds.Tables[0].Rows[i]["SentQuantity"].ToString(), out val);
                    int.TryParse(ds.Tables[0].Rows[i]["BonusQuantity"].ToString(), out bonVal);
                    selectedSum += (val + bonVal);
                    totVal += val;
                    totBonVal += bonVal;
                }

                if (totVal > quan)
                {
                    WebMessageBoxUtil.Show("Selected Quantity can not be larger than " + quan.ToString());
                    return;
                }
                if (totBonVal > bQuan)
                {
                    WebMessageBoxUtil.Show("Selected Bonus Quantity can not be larger than " + bQuan.ToString());
                    return;
                }
                if (selectedSum > TotalQuantity)
                {
                    WebMessageBoxUtil.Show("Selected Quantity can not be larger than " + TotalQuantity.ToString());
                    return;
                }


                Session["OrderSalesDetail"] = true;
                Session["ViewSalesOrders"] = false;
                Response.Redirect("OrderSalesManual.aspx",false);
            }
            catch(Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }
        protected void btnDeclineStock_Click(object sender, EventArgs e)
        {
            #region Canceling Selection of Stocks in OrderDetailEntry
               
            #endregion

            Session["OrderSalesDetail"] = true;
            Response.Redirect("OrderSalesManual.aspx",false);
        }
        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "Edit")
            {
                int sendquantity = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblQuantity")).Text);
                int bonusquantity = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblBonus")).Text);

                Session["PreviousValue"] = sendquantity + bonusquantity;
            }
            else if (e.CommandName == "UpdateStock")
            {
                    int sendquantity = int.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity")).Text);
                    int bonusquantity = int.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtBonus")).Text);
                    int availablequantity = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblAvStock")).Text);
                    int totalquantity = sendquantity + bonusquantity;
                    int p_totalquantity = 0;
                    int.TryParse(Session["PreviousValue"].ToString(), out p_totalquantity);
                    int difference = 0;//totalquantity - p_totalquantity;

                    if(p_totalquantity > totalquantity)
                    {
                        difference = p_totalquantity - totalquantity;
                    }
                    else
                    {
                        difference = totalquantity - p_totalquantity;
                    }
                    if (totalquantity <= availablequantity && totalquantity <= Convert.ToInt32(Session["TotalQuantity"].ToString()) && difference >= 0)
                    {
                        try
                        {

                            int stockID = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStockID")).Text);
                            int ProductID = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblProductID")).Text);

                            float Discount = float.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtDiscount")).Text);

                            float CostPrice = float.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblCostPrice")).Text);
                            float SalePrice = float.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblSalePrice")).Text);
                            long BarCode = long.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblBarCode")).Text);
                            int OrderDetailID = int.Parse(Session["OderDetailID"].ToString());
                            DateTime Expiry = Convert.ToDateTime(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblExpiry")).Text);
                            String Batch = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblBatch")).Text.ToString();

                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_EntrySaleOrderDetails", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                            command.Parameters.AddWithValue("@p_ProductID", ProductID);
                            command.Parameters.AddWithValue("@p_StockID", stockID);
                            command.Parameters.AddWithValue("@p_CostPrice", CostPrice);
                            command.Parameters.AddWithValue("@p_SalePrice", SalePrice);
                            command.Parameters.AddWithValue("@p_Expiry", Expiry);
                            command.Parameters.AddWithValue("@p_Batch", Batch);
                            command.Parameters.AddWithValue("@p_SendQuantity", sendquantity);
                            command.Parameters.AddWithValue("@p_BonusQuantity", bonusquantity);
                            command.Parameters.AddWithValue("@p_BarCode", BarCode);
                            command.Parameters.AddWithValue("@p_Discount", Discount);

                            command.ExecuteNonQuery();


                            #region Updating Stock Plus
                                if (p_totalquantity > 0)
                                {
                                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@p_StockID", stockID);
                                    command.Parameters.AddWithValue("@p_quantity", p_totalquantity);
                                    command.Parameters.AddWithValue("@p_Action", "Add");
                                    command.ExecuteNonQuery();
                                }
                            #endregion

                            #region Updating Stock Minus
                                command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_StockID", stockID);
                                command.Parameters.AddWithValue("@p_quantity", totalquantity);
                                command.Parameters.AddWithValue("@p_Action", "Minus");
                                command.ExecuteNonQuery();
                            #endregion


                        }
                        catch (Exception ex)
                        {

                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            throw ex;
                        }
                        finally
                        {

                            if (connection.State == ConnectionState.Open) { connection.Close(); }
                            StockDisplayGrid.EditIndex = -1;
                            BindGrid();
                        }
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("You have entered an exceeded quantity then the available stock or the total send quantity, please re adjust");
                    }

            }
            else if (e.CommandName == "Refresh")
            {
                try
                {
                    int stockID = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblStockID")).Text);
                    int ProductID = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblProductID")).Text);
                    int OrderDetailID = int.Parse(Session["OderDetailID"].ToString());
                    int sendquantity = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblQuantity")).Text);
                    int bonusquantity = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblBonus")).Text);
                    int totalquantity = sendquantity + bonusquantity;

                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_EntryDeleteSaleOrderDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductID);
                    command.Parameters.AddWithValue("@p_StockID", stockID);
                  
                    command.ExecuteNonQuery();

                    #region Updating Stock
                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", stockID);
                    command.Parameters.AddWithValue("@p_quantity", totalquantity);
                    command.Parameters.AddWithValue("@p_Action", "Add");
                    command.ExecuteNonQuery();
                    #endregion

                }
                catch (Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                    StockDisplayGrid.EditIndex = -1;
                    BindGrid();
                }
            }
        }
        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
           
            BindGrid();
        }
        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            int Total =0;

            if(e.Row.RowType == DataControlRowType.DataRow)
            {
                //Moiz: this code will throw an exception when in edit template will figure it out
               
                try
                {
                    int AvailableStock = Convert.ToInt32(((Label)e.Row.FindControl("lblAvStock")).Text.ToString());
                    int SentQuantity = Convert.ToInt32(((Label)e.Row.FindControl("lblQuantity")).Text.ToString());
                    int BonusQuantity = Convert.ToInt32(((Label)e.Row.FindControl("lblBonus")).Text.ToString());
                    Button EditButton = (Button)e.Row.FindControl("btnEdit");
                    Button RefreshButton = (Button)e.Row.FindControl("btnRefresh");


                    if (AvailableStock.Equals(0) && SentQuantity.Equals(0) && BonusQuantity.Equals(0))
                    {
                        if (EditButton != null)
                        {
                            EditButton.Enabled = false;
                        }
                        if (RefreshButton != null)
                        {
                            RefreshButton.Enabled = true;
                        }
                    }
                    //else if(AvailableStock.Equals(0))
                    //{
                    //    EditButton.Enabled = false;
                    //    RefreshButton.Enabled = false;
                    //}
                    else
                    {
                        if (EditButton != null)
                        {
                            EditButton.Enabled = true;
                        }
                        if (RefreshButton != null)
                        {
                            RefreshButton.Enabled = true;
                        }
                    }

                    if (Session["TotalExceeded"].Equals(true))
                    {
                        if (EditButton != null)
                        {
                            EditButton.Enabled = false;
                        }
                    }
                }
                catch (Exception ex) { //Exception pi gai!!! Jugaar 
                }
            }
        }
        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }
    }
}