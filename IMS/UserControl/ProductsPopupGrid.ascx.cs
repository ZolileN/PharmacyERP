using AjaxControlToolkit;
using IMS.Util;
using log4net;
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
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();

        public  bool isOpenedFromCreateTransferReq ;

        public bool IsOpenedFromCreateTransferReq
        {
            set { isOpenedFromCreateTransferReq = value; }
            get { return isOpenedFromCreateTransferReq; }
        }
        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    if (isOpenedFromCreateTransferReq)
                    {

                    }
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

        public void PopulateGrid()
        {
            if (Session["Text"].ToString() != "%")
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        DataSet ds2 = new DataSet();
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("dbo.Sp_GetStockByName", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_prodName", Session["Text"].ToString());
                        command.Parameters.AddWithValue("@p_SysID", id);
                        if (!Session["UserRole"].ToString().Equals("Store"))
                        {
                            command.Parameters.AddWithValue("@p_isStore", false);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_isStore", true);
                        }
                        SqlDataAdapter SA = new SqlDataAdapter(command);

                        SA.Fill(ds2);
                        Session["dsProducts_ProdPopUp"] = ds2;

                        StockDisplayGrid.DataSource = ds2;
                        StockDisplayGrid.DataBind();
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
                #endregion
            }
            
        }


        public void PopulateStoreUserGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                int id;
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    DataSet ds2 = new DataSet();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetStockByName_OtherStoreStock", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_prodName", Session["Text"].ToString());
                    command.Parameters.AddWithValue("@p_SysID", id);
                    command.Parameters.AddWithValue("@p_isStore", false);

                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    SA.Fill(ds2);
                    Session["dsProducts_ProdPopUp"] = ds2;

                    StockDisplayGrid.DataSource = ds2;
                    StockDisplayGrid.DataBind();
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
            #endregion
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
                     
                    DataSet ds2 = new DataSet();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetStockByName", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                    command.Parameters.AddWithValue("@p_SysID", id);
                    if (!Session["UserRole"].ToString().Equals("Store"))
                    {
                        command.Parameters.AddWithValue("@p_isStore", false);
                    }
                    else 
                    {
                        command.Parameters.AddWithValue("@p_isStore", true);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    SA.Fill(ds2);
                    Session["dsProducts_ProdPopUp"] = ds2;

                    StockDisplayGrid.DataSource = ds2;
                    StockDisplayGrid.DataBind();
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
            #endregion
        }
        #region GridView Functions & Events
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;

            if (Session["IsOpenedFromCreateTransferReq"] != null && (bool)Session["IsOpenedFromCreateTransferReq"])
            {
                PopulateStoreUserGrid();

            }
            else
            {
                if (Session["Text"].ToString() != "%")
                {
                    PopulateGrid();
                }
                else
                {
                    BindGrid();
                }
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
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
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
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }
                    #endregion

                    BindGrid();
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
                        GridView gvStockDisplayGrid = (GridView)ctl.FindControl("dgvStockDisplayGrid");

                        ltMetaTags = (TextBox)ctl.FindControl("txtSearch");
                         
                        Label lbProdId = (Label)ctl.FindControl("lblProductId");
                        if (lbProdId != null)
                        {
                            lbProdId.Text = Server.HtmlDecode(row.Cells[5].Text);
                        }

                        //DataSet dsProducts_ProdPopUp = new DataSet();
                        //dsProducts_ProdPopUp.Tables.Add((DataTable)Session["dsProdcts"]);

                        DataSet dsProducts_ProdPopUp = (DataSet)Session["dsProdcts"];
                        
                        if (dsProducts_ProdPopUp != null && dsProducts_ProdPopUp.Tables.Count > 0 && dsProducts_ProdPopUp.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drs = dsProducts_ProdPopUp.Tables[0].Select("ProductID = '" + lbProdId.Text + "'");
                            if (drs.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product already added to the Sales Order.')", true);
                                return;
                            }
                        }
                       
                        DataSet dsProducts_POPopUp = new DataSet();
                        dsProducts_POPopUp = (DataSet)Session["dsProducts_MP"];
                        
                       //dsProducts_POPopUp.Tables.Add((DataTable)Session["dsProducts_MP"]);
                        
                        if (dsProducts_POPopUp != null && dsProducts_POPopUp.Tables.Count > 0 && dsProducts_POPopUp.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drs = dsProducts_POPopUp.Tables[0].Select("ProductID = '" + lbProdId.Text + "'");
                            if (drs.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product already added to the Purchase Order.')", true);
                                return;
                            }
                        }
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);

                        }
                        if (gvStockDisplayGrid!=null)
                        {
                            gvStockDisplayGrid.DataSource = null;
                            gvStockDisplayGrid.DataBind();
                        }
                         
                    }
                }
                Session.Remove("Text");
            }

        }
    }
}