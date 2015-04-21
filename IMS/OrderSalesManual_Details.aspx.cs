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

namespace IMS
{
    public partial class OrderSalesManual_Details : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        public static bool TotalExceeded;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                lblTotalQuantity.Text = Session["TotalQuantity"].ToString();
                TotalExceeded = false;
                BindGrid();
            }    
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
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                int Total = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    Total += Convert.ToInt32(ds.Tables[0].Rows[i]["SentQuantity"].ToString()) + Convert.ToInt32(ds.Tables[0].Rows[i]["BonusQuantity"].ToString());
                }
                if(Total >= Convert.ToInt32(lblTotalQuantity.Text.ToString()))
                {
                    TotalExceeded = true;
                }
                else
                {
                    TotalExceeded = false;
                }
                    if (ds != null || ds.Tables[0] != null)
                    {
                        ProductSet = ds;
                        StockDisplayGrid.DataSource = ds.Tables[0];
                        StockDisplayGrid.DataBind();
                    }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        protected void btnAcceptStock_Click(object sender, EventArgs e)
        {
            int TotalQuantity = 0;
            for (int i = 0; i < ProductSet.Tables[0].Rows.Count; i++)
            {
                try
                {
                    int StockID = int.Parse(ProductSet.Tables[0].Rows[i]["StockID"].ToString());
                    int quantity = int.Parse(ProductSet.Tables[0].Rows[i]["SentQuantity"].ToString()) + int.Parse(ProductSet.Tables[0].Rows[i]["BonusQuantity"].ToString());
                    int OrderDetailID = int.Parse(Session["OderDetailID"].ToString());
                    TotalQuantity += quantity;
                    connection.Open();
                    SqlCommand command = new SqlCommand();
                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", StockID);
                    command.Parameters.AddWithValue("@p_quantity", quantity);
                    command.Parameters.AddWithValue("@p_Action", "Minus");
                    command.ExecuteNonQuery();

                    #region Generation of Packing List
                    command = new SqlCommand("sp_PackingListGeneration", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", StockID);
                    command.Parameters.AddWithValue("@p_quantity", quantity);
                    command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                    command.ExecuteNonQuery();
                    #endregion
                }
                catch(Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            }

            try
            {
                int OrderDetailID = int.Parse(Session["OderDetailID"].ToString());
                connection.Open();
                SqlCommand command = new SqlCommand();
                command = new SqlCommand("sp_UpdateOrdDetailQuantity", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_quantity", TotalQuantity);
                command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);

                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            Session["OrderSalesDetail"] = true;
            Response.Redirect("OrderSalesManual.aspx");
        }
        protected void btnDeclineStock_Click(object sender, EventArgs e)
        {
                try
                {
                    int productID = int.Parse(Session["ProductID"].ToString());
                    int OrderDetailID = int.Parse(Session["OderDetailID"].ToString());

                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_DeleteSaleOrderDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                    command.Parameters.AddWithValue("@p_ProductID", productID);
                  
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            Session["OrderSalesDetail"] = true;
            Response.Redirect("OrderSalesManual.aspx");
        }
        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateStock")
            {
                    int sendquantity = int.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity")).Text);
                    int bonusquantity = int.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtBonus")).Text);
                    int availablequantity = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblAvStock")).Text);

                    if ((sendquantity + bonusquantity) <= availablequantity && (sendquantity + bonusquantity) <= Convert.ToInt32(Session["TotalQuantity"].ToString()))
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


                        }
                        catch (Exception ex)
                        {

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
                  

                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_EntryDeleteSaleOrderDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductID);
                    command.Parameters.AddWithValue("@p_StockID", stockID);
                  
                    command.ExecuteNonQuery();

                }
                catch (Exception ex)
                {

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
                int AvailableStock = Convert.ToInt32(((Label)e.Row.FindControl("lblAvStock")).Text.ToString());
                Button EditButton = (Button)e.Row.FindControl("btnEdit");
                Button RefreshButton = (Button)e.Row.FindControl("btnRefresh");
 
              
                if(AvailableStock.Equals(0))
                {
                    EditButton.Enabled = false;
                    RefreshButton.Enabled = false;
                }
                else
                {
                    EditButton.Enabled = true;
                    RefreshButton.Enabled = true;
                }

                if (TotalExceeded.Equals(true))
                {
                    EditButton.Enabled = false;
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