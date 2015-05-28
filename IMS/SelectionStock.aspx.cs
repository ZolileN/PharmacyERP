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
using System.Globalization;

namespace IMS
{
    public partial class SelectionStock : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                ProductSet = new DataSet();
                #region Populating Product List
                try
                {
                    #region previous code
                    // connection.Open();
                    //SqlCommand command = new SqlCommand("Select DISTINCT tbl_ProductMaster.* From tbl_ProductMaster INNER JOIN tblStock_Detail ON tbl_ProductMaster.ProductID = tblStock_Detail.ProductID Where  tbl_ProductMaster.Status = 1", connection);
                    //DataSet ds = new DataSet();
                    //SqlDataAdapter sA = new SqlDataAdapter(command);
                    //sA.Fill(ds);
                    //Session["dsProducts_SelectST"] = ds;

                    //ProductSet = ds;
                    //ds.Tables[0].Columns.Add("ProductInfo", typeof(string), "Product_Name+ ' '+itemStrength+' '+itemPackSize+' '+itemForm");
                    //SelectProduct.DataSource = ds.Tables[0];
                    //SelectProduct.DataTextField = "ProductInfo";
                    //SelectProduct.DataValueField = "ProductID";
                    //SelectProduct.DataBind();
                    //if (SelectProduct != null)
                    //{
                    //    SelectProduct.Items.Insert(0, "Select Product");
                    //    SelectProduct.SelectedIndex = 0;
                    //} 
                    #endregion
                    if (Request.QueryString["Id"] != null)
                    {
                        BindGrid();
                    }
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
        }

        private void BindGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                DataView dv = new DataView();
                //  DataSet dsProducts = (DataSet)Session["dsProducts_SelectST"];
                
                //dv = dsProducts.Tables[0].DefaultView;
                //dv.RowFilter = "ProductID = '" + SelectProduct.SelectedValue.ToString() + "'";
                //dt = dv.ToTable();
                int stock_ID = int.Parse(Request.QueryString["Id"].ToString());
               
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetStock", connection);

                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_StockID", stock_ID);
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(ds);
            
                StockDisplayGrid.DataSource = ds;
                StockDisplayGrid.EditIndex = 0;
                StockDisplayGrid.DataBind();

            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
            #endregion
        }
        protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

            BindGrid();
            
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Delete")) 
                {
                    try
                    {
                        //Label Barcode = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("BarCode");
                        //DataView dv = ProductSet.Tables[0].DefaultView;
                        //dv.RowFilter = "BarCode = '" + long.Parse(Barcode.Text.ToString()) + "'";
                        //DataTable dt = dv.ToTable();
                        //int ProductID = Int32.Parse(dt.Rows[0]["ProductID"].ToString());

                       // Label _StockID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStockID");
                        //int stockID = int.Parse(_StockID.Text);
                        int stockID = int.Parse(e.CommandArgument.ToString());
                        String Query = "Delete From tblStock_Detail Where StockID = '" + stockID + "'";
                        connection.Open();
                        SqlCommand command = new SqlCommand(Query, connection);
                        command.ExecuteNonQuery();
                        WebMessageBoxUtil.Show("Stock Successfully Deleted ");
                    }
                    catch (Exception exp)
                    {
                    }
                    finally
                    {

                        BindGrid();
                       // StockDisplayGrid.EditIndex = -1;
                    }
                }
                if (e.CommandName.Equals("UpdateStock"))
                {
                    int quantity = 0;
                    float cp = 0;
                    float sp = 0;
                    Label Barcode = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("BarCode");
                    TextBox Quantity = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity");
                    TextBox UnitCostPrice = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtUnitCostPrice");
                    TextBox UnitSalePrice = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtUnitSalePrice");
                   
                    Label _StockID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStockID");
                    Label _barcode = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("BarCode");
                    string batch = ((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtBatch")).Text;
                    String prodID = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblProdID")).Text;
                    string lblexpiry = ((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtExpDate")).Text;
                    string lblExpOrg = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblExpOrg")).Text;
                    string BarCodeSerial = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblBarSerial")).Text;
                        
                    #region Parsing fields
                    int stockID = int.Parse(_StockID.Text);
                    int QuanOrg = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblQuantityOrg")).Text);
                    int prodNo = int.Parse(prodID);
                    DateTime expiryOrg = new DateTime();
                    if (!string.IsNullOrEmpty(lblExpOrg))
                    {
                        DateTime.TryParse(lblExpOrg, out expiryOrg);
                    }
                    
                    DateTime expiryDate = new DateTime();
                    DateTime.TryParse(lblexpiry, out expiryDate);

                    if (int.TryParse(Quantity.Text, out quantity))
                    { }
                    else 
                    {
                        WebMessageBoxUtil.Show("Invalid value for quantity");
                        return;
                    }
                    if (float.TryParse(UnitCostPrice.Text, out cp)) { }
                    else 
                    {
                        WebMessageBoxUtil.Show("Invalid value for Cost Price");
                        return;
                    }

                    if (float.TryParse(UnitSalePrice.Text, out sp)) { }
                    else
                    {
                        WebMessageBoxUtil.Show("Invalid value for Sale Price");
                        return;
                    }
                    #endregion


                    #region value checks
                    if (quantity < 0) 
                    {
                        WebMessageBoxUtil.Show("Invalid value for quantity");
                        return;
                    }

                    #endregion
                    #region Barcode generation
                   long BarCodeNumber = 0;
                    if (Barcode.Text.Equals("") || (!lblexpiry.Equals(lblExpOrg)))
                    {
                        #region Barcode Generation

            

                        DateTime dateValue = expiryDate;


                        String mm;//= dateValue.Month.ToString();
                        if (dateValue.Month < 10)
                        {
                            mm = dateValue.Month.ToString().PadLeft(2, '0');

                        }
                        else
                        {
                            mm = dateValue.Month.ToString();
                        }
                        String yy = dateValue.ToString("yy", DateTimeFormatInfo.InvariantInfo);
                        string p1 = BarCodeSerial + mm + yy;

                        if (long.TryParse(p1, out BarCodeNumber))
                        {
                        }
                        else
                        {
                            //post error message 
                        }
                        #endregion


                    } 
                    #endregion

                    #region query execution

                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_UpdateStock", connection);
                    command.CommandType = CommandType.StoredProcedure;
                   
                    command.Parameters.AddWithValue("@p_ReceivedQuantity", quantity);
                    command.Parameters.AddWithValue("@p_ReceivedQuantityOrg", QuanOrg);
                    if (Session["Inventory_StoreID"] == null)
                    {
                        command.Parameters.AddWithValue("@p_StoreID", int.Parse(Session["UserSys"].ToString()));
                    }
                    else 
                    {
                        command.Parameters.AddWithValue("@p_StoreID", int.Parse(Session["Inventory_StoreID"].ToString()));
                    }
                    command.Parameters.AddWithValue("@p_ProductID", prodNo);
                    command.Parameters.AddWithValue("@p_stockID", stockID);
                    if (!_barcode.Text.Equals("0") && BarCodeNumber != 0)
                    {
                        command.Parameters.AddWithValue("@p_BarCode", BarCodeNumber);
                    }
                    else if (!_barcode.Text.Equals("0") && BarCodeNumber == 0)
                    {
                        command.Parameters.AddWithValue("@p_BarCode", long.Parse(_barcode.Text));
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_BarCode", BarCodeNumber);
                    }
                   
                    command.Parameters.AddWithValue("@p_DiscountPercentage", 0);
                    command.Parameters.AddWithValue("@p_Bonus", 0);
                    command.Parameters.AddWithValue("@p_bonusOriginal", 0);// total bonus added
                    command.Parameters.AddWithValue("@p_BatchNumber", batch);
                    if (string.IsNullOrEmpty(lblexpiry))
                    {
                        command.Parameters.AddWithValue("@p_Expiry", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_Expiry", expiryDate);
                    }
                    command.Parameters.AddWithValue("@p_Cost", cp);
                    command.Parameters.AddWithValue("@p_Sales", sp);
                   
                    command.Parameters.AddWithValue("@p_isPO", "False");
                    if (!(string.IsNullOrEmpty(lblExpOrg) || string.IsNullOrWhiteSpace(lblExpOrg)))
                    {
                        command.Parameters.AddWithValue("@p_expiryOriginal", expiryOrg);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_expiryOriginal", DBNull.Value);
                    }
                   
                    command.ExecuteNonQuery(); 

                    #endregion
                                       
                    WebMessageBoxUtil.Show("Stock Successfully Updated ");
                }
            }
            catch (Exception exp)
            {
            }
            finally
            {
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
               
            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                //Label Barcode = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("BarCode");
                //DataView dv = ProductSet.Tables[0].DefaultView;
                //dv.RowFilter = "BarCode = '" + long.Parse(Barcode.Text.ToString()) + "'";
                //DataTable dt = dv.ToTable();
                ////int ProductID = Int32.Parse(dt.Rows[0]["ProductID"].ToString());

                //Label _StockID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStockID");
                //int stockID = int.Parse(_StockID.Text);
                //String Query = "Delete From tblStock_Detail Where StockID = '" + stockID + "'";
                //connection.Open();
                //SqlCommand command = new SqlCommand(Query, connection);
                //command.ExecuteNonQuery();
                //WebMessageBoxUtil.Show("Stock Successfully Deleted ");
            }
            catch (Exception exp)
            {
            }
            finally
            {

                BindGrid();
               // StockDisplayGrid.EditIndex = -1;
            }
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (StockDisplayGrid.Rows.Count > 0)
            {
                String prodID = ((Label)StockDisplayGrid.Rows[0].FindControl("lblProdID")).Text;
                int prod_ID = int.Parse(prodID);
                Response.Redirect("ViewInventory.aspx?Id=" + prod_ID, false);
            }
            else
            {
                Response.Redirect("ViewInventory.aspx", false);
            }
        }
    }
}