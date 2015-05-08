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
    public partial class ManageProducts : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                string searchVal = txtSearch.Text;
                if (!string.IsNullOrEmpty(searchVal))
                {
                    //do here
                }
                BindGrid();
            }
        }

        private void BindGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                int id;
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    String Query = "Select * FROM tbl_ProductMaster Where Status = 1";

                    connection.Open();
                    SqlCommand command = new SqlCommand(Query, connection);
                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    ProductSet = null;
                    SA.Fill(ds);
                    Session["dsProducts"] = ds;
                    ProductSet = ds;
                    StockDisplayGrid.DataSource = ds;
                    StockDisplayGrid.DataBind();
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
        protected void btnManageProducts_Click(object sender, EventArgs e)
        {

        }

        protected void btnAddProduct_Click(object sender, EventArgs e)
        {
            Session["MODE"] = "ADD";
            Response.Redirect("AddProduct.aspx");
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            Session["Linkto"] = "DELETE";
            Response.Redirect("Edit_DeleteProduct.aspx");
        }

        protected void btnEditProduct_Click(object sender, EventArgs e)
        {
            Session["Linkto"] = "EDIT";
            Response.Redirect("Edit_DeleteProduct.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageInventory.aspx");
        }

        protected void btnStocks_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStocks.aspx");
        }

        #region GridView Functions & Events
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
                if (e.CommandName.Equals("Edit"))
                {
                    #region Updating Product
                    Label ItemNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UPC");
                    Label ItemName = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ProductName");
                    Label ItemType = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Type");
                    Label GreenRainCode = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("GreenRain");
                    Label UnitSale = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblUnitSalePrice");
                    Label UnitCost = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UnitCost");


                    

                    Session["PageMasterProduct"] = "true";
                    Session["MODE"] = "EDIT";
                   
                    Session["MS_ItemNo"] = ItemNo.Text.ToString();
                    Session["MS_ItemName"] = ItemName.Text.ToString();
                    Session["MS_ItemType"] = ItemType.Text.ToString();

                    DataSet dsProducts = (DataSet)Session["dsProducts"];
                    DataView dv = dsProducts.Tables[0].DefaultView;
                   // DataView dv = ProductSet.Tables[0].DefaultView;
                    dv.RowFilter = "Product_Id_Org = '"+ ItemNo.Text + "'";
                    DataTable dt = dv.ToTable();
                    Session["MS_Manufacterer"] = "";
                    Session["MS_Category"] = "";
                    Session["MS_Description"] = dt.Rows[0]["Description"].ToString();
                    Session["MS_GenericName"] = dt.Rows[0]["GName"].ToString();
                    Session["MS_Control"] = dt.Rows[0]["Control"].ToString();
                    Session["MS_BinNumber"] = dt.Rows[0]["binNumber"].ToString();
                    Session["MS_GreenRainCode"] = GreenRainCode.Text.ToString();
                    Session["MS_BrandName"] = dt.Rows[0]["Brand_Name"].ToString();
                    Session["MS_MaxiMumDiscount"] = dt.Rows[0]["MaxiMumDiscount"].ToString();
                    Session["MS_LineID"] = dt.Rows[0]["LineID"].ToString();
                    Session["MS_UnitSale"] = UnitSale.Text.ToString();
                    Session["MS_UnitCost"] = UnitCost.Text.ToString();
                    Session["MS_itemAWT"] = dt.Rows[0]["itemAWT"].ToString();
                    Session["MS_itemForm"] = dt.Rows[0]["itemForm"].ToString();
                    Session["MS_itemStrength"] = dt.Rows[0]["itemStrength"].ToString();
                    Session["MS_itemPackType"] = dt.Rows[0]["itemPackType"].ToString();
                    Session["MS_itemPackSize"] = dt.Rows[0]["itemPackSize"].ToString();
                    Session["MS_ProductID"] = dt.Rows[0]["ProductID"].ToString();
                    Session["MS_ProductOrderType"] = dt.Rows[0]["productOrderType"].ToString();
                    Session["MS_Bonus12"] = dt.Rows[0]["Bonus12Quantity"].ToString();
                    Session["MS_Bonus25"] = dt.Rows[0]["Bonus25Quantity"].ToString();
                    Session["MS_Bonus50"] = dt.Rows[0]["Bonus50Quantity"].ToString();
                    Response.Redirect("Addproduct.aspx");
                    #endregion

                }
                else if (e.CommandName.Equals("Delete"))
                {

                    #region Delete product
                    try
                    {
                            Label ItemNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UPC");
                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_DeleteProduct", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            
                            int res6 =0;

                            DataSet dsProducts = (DataSet)Session["dsProducts"];
                            DataView dv = dsProducts.Tables[0].DefaultView;
                            //DataView dv = ProductSet.Tables[0].DefaultView;
                            dv.RowFilter = "Product_Id_Org = '" + ItemNo.Text + "'";
                            DataTable dt = dv.ToTable();
                            Session["MS_ProductID"] = dt.Rows[0]["ProductID"].ToString();
                        
                            if (int.TryParse(Session["MS_ProductID"].ToString(), out res6))
                            {
                                command.Parameters.AddWithValue("@p_ProductID", res6);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_ProductID", 0);
                            }
                            command.ExecuteNonQuery();
                    
                    }
                    catch (Exception ex)
                    {
                    }
                    finally
                    {
                        connection.Close();
                    }
                    #endregion

                    BindGrid();
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGrid();
        }

        #endregion

        //protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        connection.Open();

        //        String Text = SelectProduct.SelectedItem.ToString() + "%";
        //        SqlCommand command = new SqlCommand("SELECT * From tbl_ProductMaster Where tbl_ProductMaster.Product_Name LIKE '" + Text + "' AND Status = 1", connection);
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter sA = new SqlDataAdapter(command);
        //        sA.Fill(ds);
        //        if (SelectProduct.DataSource != null)
        //        {
        //            SelectProduct.DataSource = null;
        //        }

        //        ProductSet = null;
        //        ProductSet = ds;

        //        StockDisplayGrid.DataSource = ds;
        //        StockDisplayGrid.DataBind();
        //    }
        //    catch(Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();

        //    }
        //}

        //protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        //{
        //    connection.Open();

        //    String Text = txtSearch.Value.ToString() + "%";
        //    SqlCommand command = new SqlCommand("SELECT * From tbl_ProductMaster Where tbl_ProductMaster.Product_Name LIKE '" + Text + "' AND Status = 1", connection);
        //    DataSet ds = new DataSet();
        //    SqlDataAdapter sA = new SqlDataAdapter(command);
        //    sA.Fill(ds);
        //    if (SelectProduct.DataSource != null)
        //    {
        //        SelectProduct.DataSource = null;
        //    }

        //    ProductSet = null;
        //    ProductSet = ds;

        //    StockDisplayGrid.DataSource = ds;
        //    StockDisplayGrid.DataBind();
        //    //if (txtProduct.Text.Length >= 3)
        //    //{
        //    //    PopulateDropDown(txtProduct.Text);
        //    //    SelectProduct.Visible = true;
        //    //}
        //}

        //public void PopulateDropDown(String Text)
        //{
        //    #region Populating Product Name Dropdown

        //    try
        //    {
        //        connection.Open();

        //        Text = Text + "%";
        //        SqlCommand command = new SqlCommand("SELECT * From tbl_ProductMaster Where tbl_ProductMaster.Product_Name LIKE '" + Text + "' AND Status = 1", connection);
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter sA = new SqlDataAdapter(command);
        //        sA.Fill(ds);
        //        if (SelectProduct.DataSource != null)
        //        {
        //            SelectProduct.DataSource = null;
        //        }

        //        ProductSet = null;
        //        ProductSet = ds;
                
        //        StockDisplayGrid.DataSource = ds;
        //        StockDisplayGrid.DataBind();
                
        //        SelectProduct.DataSource = ds.Tables[0];
        //        SelectProduct.DataTextField = "Product_Name";
        //        SelectProduct.DataValueField = "ProductID";
        //        SelectProduct.DataBind();
        //        if (SelectProduct != null)
        //        {
        //            SelectProduct.Items.Insert(0, "Select Product");
        //            SelectProduct.SelectedIndex = 0;
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();
        //    }
        //    #endregion
        //}

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }

        

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            String Text = txtSearch.Text + '%';
            Session["Text"] = Text;
            ProductsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }
    }
}