using AjaxControlToolkit;
using IMS.Util;
using IMSCommon.Util;
using log4net;
using System;
using System.Collections;
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
    public partial class Product_Search_Popup : System.Web.UI.UserControl
    {
        string text = "";
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        bool selectAll=false;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();

        public bool SelectAll
        {
           // get { return selectAll; }
            set { selectAll = value; }
        }

        public bool StoreAssociation
        {
            set
            {
                ViewState["selectSearch"] = value;
                if (value == false) 
                {
                    lblSelectVendor.Visible = false;
                    btnSearchStore.Visible = false;
                    txtSearch.Visible = false;
                }
            }
        }

        public void PopulateforAssociation()
        {
            ViewState["checkAllState"] = true;
            ViewState["first"] = true;
            lblSelectVendor.Visible = true;
            btnSearchStore.Visible = true;
            txtSearch.Visible = true;
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
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
                    Control ctl = this.Parent;
                    TextBox txtsearch = null;
                    txtsearch = (TextBox)ctl.FindControl("txtSearch");
                    if (txtsearch.Text != null)
                    {
                        text = txtsearch.Text;
                    }


                    BindGrid();

                    if (selectAll)
                    {
                        ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }
                    if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true)
                    {
                        lblSelectVendor.Visible = true;
                        btnSearchStore.Visible = true;
                        txtSearch.Visible = true;
                    }
                }
                if (IsPostBack)
                {
                    if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                    {
                        if (!(ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true))
                        {
                            ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        }
                    }
                    if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true)
                    {
                        lblSelectVendor.Visible = true;
                        btnSearchStore.Visible = true;
                        txtSearch.Visible = true;
                    }
                }
                expHandler.CheckForErrorMessage(Session);
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
            if (Session["Text"] != null)
            {
                ViewState["first"] = false;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                       
                            // String Query = "Select * FROM tbl_ProductMaster Where Status = 1 and Product_Name Like '" + Session["Text"].ToString() + "'";
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("dbo.Sp_GetProductByName", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            if (Session["Text"] != null)
                            {
                                command.Parameters.AddWithValue("@p_prodName", Session["Text"].ToString());
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                            }
                            SqlDataAdapter SA = new SqlDataAdapter(command);


                            SA.Fill(ds);
                            Session["dsProducts_PopUp"] = ds;
                            ProductSet = ds;
                            StockDisplayGrid.DataSource = ds;
                            StockDisplayGrid.DataBind();
                            PopulatePreviousState();

                            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                            {
                                ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                                ViewState["checkAllState"] = true;
                            }


                        
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

        public void BindGrid()
        {
            //SaveCurrentState();
            ViewState["first"] = false;
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                int id;
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetProductByName", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (Session["Text"] != null)
                    {
                        command.Parameters.AddWithValue("@p_prodName", Session["Text"].ToString());
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(command);


                    SA.Fill(ds);
                    Session["dsProducts_PopUp"] = ds;
                    ProductSet = ds;
                    StockDisplayGrid.DataSource = ds;
                    StockDisplayGrid.DataBind();
                    PopulatePreviousState();

                    if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                    {
                        ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }

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

        public void selectAllProducts()
        {
            ViewState["first"] = false;
            DataSet productSet = (DataSet)Session["dsProducts_PopUp"];
            ArrayList CheckBoxArray;
            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }
            
            int checkBoxIndex = -1;
            int pageIndex = 0;
            int paginationCount = productSet.Tables[0].Rows.Count/10; // 1 page  contains 10 rows, hence divided by 10
          
            // adding page index for the first time.
            string checkAllIndex;//= "chkAll-" + pageIndex;
            //if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
            //{
            //    CheckBoxArray.Add(checkAllIndex);
            //}
            
            for (int i = 0; i < productSet.Tables[0].Rows.Count; i++)
            {
                if (i % 10 == 0)
                {
                    
                    checkAllIndex = "chkAll-" + pageIndex;
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                    pageIndex++;
                }
                             

                // the checkbox index must range between 0-9
                if (i > 9)
                {
                    checkBoxIndex = ((i % 10));
                }
                else 
                {
                    checkBoxIndex++;
                }

                string ProdID = "ID " + checkBoxIndex + "-" + productSet.Tables[0].Rows[i]["ProductID"].ToString();
                
                if (!CheckBoxArray.Contains(ProdID))
                {
                    CheckBoxArray.Add(checkBoxIndex);
                    CheckBoxArray.Add(ProdID);
                }
               
            }

            ViewState["CheckBoxArray"] = CheckBoxArray;

        }
       

        private void SaveCurrentState() 
        {

            ArrayList CheckBoxArray;
            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
            {
                ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                ViewState["checkAllState"] = true;

                if (ViewState["CheckBoxArray"] != null)
                {
                    CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                }
                else
                {
                    CheckBoxArray = new ArrayList();
                }

                
                int CheckBoxIndex;
                bool CheckAllWasChecked = false;
                CheckBox chkAll = (CheckBox)StockDisplayGrid.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                string checkAllIndex = "chkAll-" + StockDisplayGrid.PageIndex;
                if (chkAll.Checked)
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                }
                else
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBoxArray.Remove(checkAllIndex);
                        CheckAllWasChecked = true;
                    }
                }
                for (int i = 0; i < StockDisplayGrid.Rows.Count; i++)
                {
                    if (StockDisplayGrid.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (CheckBox)StockDisplayGrid.Rows[i].Cells[0].FindControl("chkCtrl");
                        Label lblProd = (Label)StockDisplayGrid.Rows[i].FindControl("lblProductID");
                       
                        CheckBoxIndex = StockDisplayGrid.PageSize * StockDisplayGrid.PageIndex + (i + 1);
                        if (chk.Checked)
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                            {
                                string ProdID = "ID " + CheckBoxIndex + "-" + lblProd.Text;
                                CheckBoxArray.Add(CheckBoxIndex);
                                CheckBoxArray.Add(ProdID);
                            }
                        }
                        else
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                            {
                                string ProdID = "ID " + CheckBoxIndex + "-" + lblProd.Text;
                                CheckBoxArray.Remove(CheckBoxIndex);
                                CheckBoxArray.Remove(ProdID);
                            }
                        }
                    }
                }
                ViewState["CheckBoxArray"] = CheckBoxArray;
            }
        }

        private void PopulatePreviousState() 
        {
            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
            {
                if (ViewState["CheckBoxArray"] != null)
                {
                    ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                    string checkAllIndex = "chkAll-" + StockDisplayGrid.PageIndex;

                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBox chkAll = (CheckBox)StockDisplayGrid.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                        chkAll.Checked = true;
                    }
                    for (int i = 0; i < StockDisplayGrid.Rows.Count; i++)
                    {

                        if (StockDisplayGrid.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                            {
                                CheckBox chk = (CheckBox)StockDisplayGrid.Rows[i].Cells[0].FindControl("chkCtrl");
                                chk.Checked = true;
                                
                            }
                            else
                            {
                                int CheckBoxIndex = StockDisplayGrid.PageSize * (StockDisplayGrid.PageIndex) + (i + 1);
                                if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                                {
                                    CheckBox chk = (CheckBox)StockDisplayGrid.Rows[i].Cells[0].FindControl("chkCtrl");
                                    chk.Checked = true;
                                   
                                }
                            }
                        }
                    }
                }
            }
        }
        
        #region GridView Functions & Events
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if ((bool)ViewState["first"] == true)
            {
                CheckBox chkAll = (CheckBox)StockDisplayGrid.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                if (chkAll.Checked)
                {
                    selectAllProducts();
                    ViewState["first"] = false;
                }
                else 
                {
                    ViewState["first"] = false;
                }
            }
            //else
            //{
            //    SaveCurrentState();
            //}
            SaveCurrentState(); 
            StockDisplayGrid.PageIndex = e.NewPageIndex;

            //in-case its the first page set viewstate["first"] =true, so the user can avail the select all option
            if (StockDisplayGrid.PageIndex == 0) 
            {
                if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
                {
                    ViewState["first"] = true;
                }
            }
            
            if (Session["Text"] != null)
            {
                PopulateGrid();
            }
            else if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true)
            {
                BindAssociatedVendorGrid();
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

                    DataSet dsProducts = (DataSet)Session["dsProducts_PopUp"];
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
                    Response.Redirect("Addproduct.aspx",false);
                    #endregion

                }
                else if (e.CommandName.Equals("Delete"))
                {

                    #region Delete product
                    try
                    {
                        Label ItemNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UPC");
                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand command = new SqlCommand("sp_DeleteProduct", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        int res6 = 0;

                        DataSet dsProducts = (DataSet)Session["dsProducts_PopUp"];
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
            
            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
            {
               // SaveCurrentState();
                if ((bool)ViewState["first"] == true)
                {
                    CheckBox chkAll = (CheckBox)StockDisplayGrid.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                    if (chkAll.Checked)
                    {
                        selectAllProducts();
                        ViewState["first"] = false;
                    }
                    else
                    {
                        ViewState["first"] = false;
                    }
                }
                //else
                //{
                //    SaveCurrentState();
                //}
                SaveCurrentState();
                if (ViewState["CheckBoxArray"] != null)
                {
                    Control ctl = this.Parent;
                    Label lbstoreId = (Label)ctl.FindControl("lblStoreId");
                    ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    try
                    {
                        foreach (var item in CheckBoxArray)
                        {
                            if (item.ToString().Contains("ID"))
                            {
                                string[] key = item.ToString().Split('-');
                                try
                                {
                                    SqlCommand command = new SqlCommand("Sp_AddNewStoreProduct", connection);
                                    command.CommandType = CommandType.StoredProcedure;
                                    command.Parameters.AddWithValue("@p_ProductID", int.Parse(key[1]));
                                    command.Parameters.AddWithValue("@p_StoreID", int.Parse(lbstoreId.Text));
                                    command.ExecuteNonQuery();

                                }
                                catch (Exception ex)
                                {
                                    string message = expHandler.GenerateLogString(ex);
                                    log.Error(message);
                                }


                            }
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
                    ViewState.Remove("CheckBoxArray"); 
                    WebMessageBoxUtil.Show("Products have been successfully associated ");

                    GridView gvParent = (GridView)ctl.FindControl("StockDisplayGrid");

                    DataTable dt = new DataTable();
                    DataSet ds = new DataSet();
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("Sp_GetProductStoreMapping", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        int StoreNumber = 0;


                        int.TryParse(lbstoreId.Text, out StoreNumber);

                        command.Parameters.AddWithValue("@p_StoreID", StoreNumber);

                        SqlDataAdapter sA = new SqlDataAdapter(command);
                        sA.Fill(ds);
                        gvParent.DataSource = ds;
                        gvParent.DataBind();
                        gvParent.Visible = true;
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
                        {
                            connection.Close();
                        }
                    }
                    Session.Remove("Text");
                }
            }
            else
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

                            ltMetaTags = (TextBox)ctl.FindControl("txtSearch");
                            if (ltMetaTags != null)
                            {
                                ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);
                                string description = Server.HtmlDecode(row.Cells[3].Text);
                                GridView gvParent = (GridView)ctl.FindControl("StockDisplayGrid");

                                DataTable dt = new DataTable();
                                DataSet ds = new DataSet();
                                #region Getting Product Details
                                try
                                {
                                    int id;
                                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                                    {
                                        //String Query = "Select * FROM tbl_ProductMaster Where Status = 1 and Product_Name Like '" + ltMetaTags.Text + "'";

                                        
                                        if (connection.State == ConnectionState.Closed)
                                        {
                                            connection.Open();
                                        }
                                        SqlCommand command = new SqlCommand("dbo.Sp_GetProductByName", connection);
                                        command.CommandType = CommandType.StoredProcedure;
                                        if (Session["Text"] != null)
                                        {
                                            //if (!string.IsNullOrEmpty(description))
                                            //{
                                            //    command.Parameters.AddWithValue("@p_prodName", description);
                                            //}
                                            //else
                                            //{
                                                command.Parameters.AddWithValue("@p_prodName", ltMetaTags.Text);
                                           // }
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                                        }
                                        SqlDataAdapter SA = new SqlDataAdapter(command);


                                        SA.Fill(ds);
                                        Session["dsProducts_PopUp"] = ds;
                                        ProductSet = ds;
                                        gvParent.DataSource = ds;
                                        gvParent.DataBind();
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
                    }
                    
                }
                Session.Remove("Text");
            }

        }

        protected void btnSearchStore_Click(object sender, EventArgs e)
        {
            try
            {
                BindAssociatedVendorGrid();
                ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
                mpe.Show();
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

        public void BindAssociatedVendorGrid()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtSearch.Value))
                {
                    if(connection.State==ConnectionState.Closed)
                    connection.Open();
                    DataSet resultSet = new DataSet();

                    SqlCommand comm = new SqlCommand("Sp_GetStoreProductsByName", connection);
                    comm.CommandType = CommandType.StoredProcedure;
                    if (String.IsNullOrEmpty(txtSearch.Value))
                    {
                        comm.Parameters.AddWithValue("@p_storeName", DBNull.Value);
                    }
                    else
                    {
                        comm.Parameters.AddWithValue("@p_storeName", txtSearch.Value);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(comm);
                    SA.Fill(resultSet);

                    StockDisplayGrid.DataSource = resultSet;
                    StockDisplayGrid.DataBind();
                    PopulatePreviousState();

                    if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                    {
                        ((CheckBox)StockDisplayGrid.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
             
            }

        }
    }
}