using AjaxControlToolkit;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS.UserControl
{
    public partial class ProductsPopupGrid : System.Web.UI.UserControl
    {
        string text = "";
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public void PopulateGrid()
        {
            if (Session["Text"] != null)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        String Query = "Select * FROM tbl_ProductMaster Where Status = 1 and Product_Name Like '" + Session["Text"].ToString() + "'";

                        connection.Open();
                        SqlCommand command = new SqlCommand(Query, connection);
                        SqlDataAdapter SA = new SqlDataAdapter(command);
                        ProductSet = null;
                        SA.Fill(ds);
                        Session["dsProducts_ProdPopUp"] = ds;
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
        }

        public void BindGrid()
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
                    Session["dsProducts_ProdPopUp"] = ds;
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
        #region GridView Functions & Events
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            if (Session["Text"] != null)
            {
                PopulateGrid();
            }
            else
            {
                BindGrid();
            }
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
            mpe.Show();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            if (Session["Text"] != null)
            {
                PopulateGrid();
            }
            else
            {
                BindGrid();
            }
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

                    DataSet dsProducts = (DataSet)Session["dsProducts_ProdPopUp"];
                    DataView dv = dsProducts.Tables[0].DefaultView;
                    // DataView dv = ProductSet.Tables[0].DefaultView;
                    dv.RowFilter = "Product_Id_Org = '" + ItemNo.Text + "'";
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

                        int res6 = 0;

                        DataSet dsProducts = (DataSet)Session["dsProducts_ProdPopUp"];
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
            catch (Exception ex)
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

        protected void SelectProduct_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in StockDisplayGrid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Control ctl = this.Parent;
                        TextBox ltMetaTags = null;
                        GridView gvStockDisplayGrid = (GridView)ctl.FindControl("StockDisplayGrid");

                        ltMetaTags = (TextBox)ctl.FindControl("txtSearch");
                         
                        Label lbProdId = (Label)ctl.FindControl("lblProductId");
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);
                             
                        }
                        if(lbProdId !=null)
                        {
                            lbProdId.Text = Server.HtmlDecode(row.Cells[7].Text);
                        }
                        gvStockDisplayGrid.DataSource = null;
                        gvStockDisplayGrid.DataBind();
                    }
                }
                Session.Remove("Text");
            }

        }
    }
}