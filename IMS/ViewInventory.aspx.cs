﻿using System;
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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using IMSBusinessLogic;
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class ViewInventory : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
       
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {

                    Session.Remove("dsProdcts");
                    Session.Remove("dsProducts_MP");

                    #region remove print sessions

                    Session.Remove("Search_DepID");
                    Session.Remove("Search_CatID");
                    Session.Remove("Search_SubCatID");
                    Session.Remove("Search_ProdIdOrg");
                    Session.Remove("Search_ProdType");
                    Session.Remove("Search_ProdId");
                    Session.Remove("Search_ProdName");
                    Session.Remove("Search_Active");

                    #endregion

                    #region Populating Product Type DropDown
                    ProductType.Items.Add("Select Product Type");
                    ProductType.Items.Add("Medicine(HAAD)");
                    ProductType.Items.Add("Medicine(Non HAAD)");
                    ProductType.Items.Add("Non Medicine");

                    ProductType.SelectedIndex = 0;
                    #endregion

                    #region Populating Product Department DropDown
                    try
                    {
                        ProductDept.DataSource = DepartmentBLL.GetAllDepartment(); ;
                        ProductDept.DataTextField = "Name";
                        ProductDept.DataValueField = "DepId";
                        ProductDept.DataBind();
                        if (ProductDept != null)
                        {
                            ProductDept.Items.Insert(0, "Select Department");
                            ProductDept.SelectedIndex = 0;
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

                    #region Populating Category Dropdown
                    try
                    {
                        //connection.Open();
                        //SqlCommand command = new SqlCommand("Select * From tblCategory", connection);
                        //DataSet ds = new DataSet();
                        //SqlDataAdapter sA = new SqlDataAdapter(command);
                        //sA.Fill(ds);
                        ProductCat.DataSource = InventoryBLL.GetCategoryBasic(connection);
                        ProductCat.DataTextField = "Name";
                        ProductCat.DataValueField = "CategoryID";
                        ProductCat.DataBind();
                        if (ProductCat != null)
                        {
                            ProductCat.Items.Insert(0, "Select Category");
                            ProductCat.SelectedIndex = 0;
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

                    #region Populating SubCategory Dropdown
                    try
                    {
                        //connection.Open();
                        //SqlCommand command = new SqlCommand("Select * From tblSub_Category", connection);
                        //DataSet ds = new DataSet();
                        //SqlDataAdapter sA = new SqlDataAdapter(command);
                        //sA.Fill(ds);
                        int? CatID = null;
                        ProductSubCat.DataSource = SubCategoryBLL.GetSubCategoriesBasic(CatID);
                        ProductSubCat.DataTextField = "Name";
                        ProductSubCat.DataValueField = "Sub_CatID";
                        ProductSubCat.DataBind();

                        if (ProductSubCat != null)
                        {
                            ProductSubCat.Items.Insert(0, "Select Sub Category");
                            ProductSubCat.SelectedIndex = 0;
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

                    #region populate active dropdown
                    ddlActive.Items.Add("Select Option");
                    ddlActive.Items.Add("All Active");
                    ddlActive.Items.Add("All in-Active");
                    ddlActive.Items.Add("All Active & Non-Zero Stock");
                    ddlActive.SelectedIndex = 0;
                    #endregion

                    #region ProductOrderType dropdown population
                    ddlProductOrderType.DataSource = IMSGlobal.GetOrdersType();
                    ddlProductOrderType.DataTextField = "Name";
                    ddlProductOrderType.DataValueField = "OrderTypeId";
                    ddlProductOrderType.DataBind();

                    if (ddlProductOrderType != null)
                    {
                        ddlProductOrderType.Items.Insert(0, "Select Product Order Type");
                        ddlProductOrderType.SelectedIndex = 0;
                    }
                    #endregion

                    #region Populating Stores
                    try
                    {
                        DataSet dsS = new DataSet();
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();

                        }
                        SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@p_SystemID", DBNull.Value);
                        SqlDataAdapter sA = new SqlDataAdapter(command);
                        sA.Fill(dsS);
                        ddlStockAt.DataSource = dsS.Tables[0];
                        ddlStockAt.DataTextField = "SystemName";
                        ddlStockAt.DataValueField = "SystemID";
                        ddlStockAt.DataBind();
                        if (ddlStockAt != null)
                        {
                            ddlStockAt.Items.Insert(0, "Select Store");
                            ddlStockAt.SelectedIndex = 0;
                        }
                        //Set index of system role incase the system role is not warehouse
                        if (!Session["UserRole"].ToString().Equals("WareHouse"))
                        {
                            foreach (System.Web.UI.WebControls.ListItem Items in ddlStockAt.Items)
                            {
                                if (Items.Value.Equals(Session["UserSys"].ToString()))
                                {
                                    ddlStockAt.SelectedIndex = ddlStockAt.Items.IndexOf(Items);
                                    ddlStockAt.Enabled = false;
                                    break;
                                }
                            }
                        }
                        else
                        {

                            if (Session["Inventory_StoreID"] != null)
                            {
                                foreach (System.Web.UI.WebControls.ListItem Items in ddlStockAt.Items)
                                {
                                    if (Items.Value.Equals(Session["Inventory_StoreID"].ToString()))
                                    {
                                        ddlStockAt.SelectedIndex = ddlStockAt.Items.IndexOf(Items);
                                        Session.Remove("Inventory_StoreID");
                                        break;
                                    }
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
                        {
                            connection.Close();
                        }
                    }
                    #endregion
                    if (Request.QueryString["Id"] != null)
                    {
                        BindByProductID();
                    }
                    else
                    {
                        BindGridbyFilters();
                    }
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
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
        private void BindByProductID()
        {

            int prod_ID = int.Parse(Request.QueryString["Id"].ToString());

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
                    SqlCommand command = new SqlCommand("sp_ViewInventory_byFilters", connection);
                    #region with parameter approach
                    command.CommandType = CommandType.StoredProcedure;

                    if (ddlStockAt.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_SysID", id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_SysID", Convert.ToInt32(ddlStockAt.SelectedValue.ToString()));
                    }


                    command.Parameters.AddWithValue("@p_DeptID", DBNull.Value);

                    command.Parameters.AddWithValue("@p_CatID", DBNull.Value);

                    command.Parameters.AddWithValue("@p_SubCatID", DBNull.Value);

                    command.Parameters.AddWithValue("@p_productOrderType", DBNull.Value);

                    command.Parameters.AddWithValue("@p_ProdType", DBNull.Value);
                    command.Parameters.AddWithValue("@p_isActive", DBNull.Value);
                    command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                    command.Parameters.AddWithValue("@p_isPrint", 0);
                    command.Parameters.AddWithValue("@p_ProdID", prod_ID);
                                          
                   
                    #endregion

                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    SA.Fill(ds);
                    dgvStockDisplayGrid.DataSource = ds;
                    dgvStockDisplayGrid.DataBind();
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
                    #region original query
                    //   String Query = "Select tblStock_Detail.ProductID AS ProductID ,tbl_ProductMaster.Product_Name AS ProductName, tblStock_Detail.BarCode AS BarCode, tblStock_Detail.Quantity AS Qauntity, tblStock_Detail.ExpiryDate As Expiry,tbl_ProductMaster.Product_Name AS ProductName,"+
                    //" tbl_ProductMaster.itemPackSize as PackageSize, tbl_ProductMaster.itemStrength as strength, tbl_ProductMaster.itemForm as dosageForm, FORMAT(tblStock_Detail.UCostPrice, 'N2') AS CostPrice, FORMAT(tblStock_Detail.USalePrice, 'N2') AS SalePrice, tbl_System.SystemName AS Location" +
                    //" From  tblStock_Detail INNER JOIN tbl_ProductMaster ON tblStock_Detail.ProductID = tbl_ProductMaster.ProductID INNER JOIN tbl_System ON tblStock_Detail.StoredAt = tbl_System.SystemID AND tblStock_Detail.StoredAt = '" + id.ToString() + "'";

                    #endregion


                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_ViewInventory_byFilters", connection);
                   
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_SysID", id);
                    command.Parameters.AddWithValue("@p_DeptID", DBNull.Value);
                    command.Parameters.AddWithValue("@p_CatID", DBNull.Value);
                    command.Parameters.AddWithValue("@p_SubCatID", DBNull.Value);
                    command.Parameters.AddWithValue("@p_productOrderType", DBNull.Value);
                    command.Parameters.AddWithValue("@p_ProdType", DBNull.Value);
                    command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    SA.Fill(ds);
                    if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                    {
                        dgvStockDisplayGrid.DataSource = ds;
                        dgvStockDisplayGrid.DataBind();
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("No stock available to show");
                        NoProductMessage.Visible = true;
                        ProductCat.Enabled = false;
                        ProductDept.Enabled = false;
                        ProductSubCat.Enabled = false;
                        ProductType.Enabled = false;
                        //SelectProduct.Enabled = false;
                        btnSearch.Enabled = false;
                        btnRefresh.Enabled = false;
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

        public void BindGridbyFilters()
        {
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
                    SqlCommand command = new SqlCommand("sp_ViewInventory_byFilters", connection);
                    command.CommandTimeout = 0;
                    #region with parameter approach
                    command.CommandType = CommandType.StoredProcedure;
                    if (ddlStockAt.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_SysID", id);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_SysID", Convert.ToInt32(ddlStockAt.SelectedValue.ToString()));
                    }
                    if (ProductDept.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_DeptID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_DeptID", Convert.ToInt32(ProductDept.SelectedValue.ToString()));
                    }

                    if (ProductCat.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_CatID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_CatID", Convert.ToInt32(ProductCat.SelectedValue.ToString()));
                    }

                    if (ProductSubCat.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_SubCatID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_SubCatID", Convert.ToInt32(ProductSubCat.SelectedValue.ToString()));
                    }

                    if (ddlProductOrderType.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_productOrderType", DBNull.Value);
                    } 
                    else
                    {
                        command.Parameters.AddWithValue("@p_productOrderType", Convert.ToInt32(ddlProductOrderType.SelectedValue.ToString()));
                    }
                    if (ddlActive.SelectedIndex > 0)
                    {
                        switch (ddlActive.SelectedIndex)
                        {
                            case 0:
                                command.Parameters.AddWithValue("@p_isActive", DBNull.Value);

                                break;
                            case 1:
                                command.Parameters.AddWithValue("@p_isActive", 1);
                                break;
                            case 2:
                                command.Parameters.AddWithValue("@p_isActive", 0);
                                break;


                            case 3:
                                command.Parameters.AddWithValue("@p_isActive", 3);
                                break;
                        }
                       
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_isActive", DBNull.Value);
                    }

                    if (ProductType.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_ProdType", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_ProdType", ProductType.SelectedItem.ToString());
                    }

                    //if (string.IsNullOrEmpty(txtSearch.Text))
                    //{
                    //    command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
                    //}
                    //else
                    //{
                    //    command.Parameters.AddWithValue("@p_ProdID", txtSearch.Text);
                    //}
                    //org value lblProductId.Text
                    command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
                   

                    if (String.IsNullOrEmpty(txtSearch.Value))
                    {
                        command.Parameters.AddWithValue("@p_prodName", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_prodName", txtSearch.Value.ToString());
                    }
                    command.Parameters.AddWithValue("@p_isPrint", 0);
                    #endregion

                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    SA.Fill(ds);
                    dgvStockDisplayGrid.DataSource = ds;
                    dgvStockDisplayGrid.DataBind();
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

      

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvStockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGridbyFilters();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("AddVal"))
                {
                    Label prodNum = (Label)dgvStockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblprodID");
                    if (ddlStockAt.SelectedIndex > 0)
                    {
                        Session["Inventory_StoreID"] = Convert.ToInt32(ddlStockAt.SelectedValue.ToString());
                    }
                    Response.Redirect("AddStock.aspx?Id=" + prodNum.Text, false);
                }
                if (e.CommandName.Equals("Edit"))
                {
                    Label stockNum = (Label)dgvStockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblstockid");
                    if (ddlStockAt.SelectedIndex > 0)
                    {
                        Session["Inventory_StoreID"] = Convert.ToInt32(ddlStockAt.SelectedValue.ToString());
                    }
                    Response.Redirect("SelectionStock.aspx?Id=" + stockNum.Text, false);
                }
                if (e.CommandName.Equals("Delete"))
                {
                    Label stockNum = (Label)dgvStockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblstockid");

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("Sp_DeleteStock", connection);

                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_stockID", int.Parse(stockNum.Text));

                    command.ExecuteNonQuery();
                    WebMessageBoxUtil.Show("Stock Successfully Deleted ");
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
                {
                    connection.Close();
                }
                if (Request.QueryString["Id"] != null)
                {
                    BindByProductID();
                }
                else
                {
                    BindGridbyFilters();
                }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("Inventory_StoreID");
            if (Convert.ToInt32(Session["UserSys"]).Equals(1))
            {
                Response.Redirect("WarehouseMain.aspx", false);
            }
            else
            {
                Response.Redirect("StoreMain.aspx", false);
            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                //Label Label1 = (Label)e.Row.FindControl("Label1");

                //Label dosage = (Label)e.Row.FindControl("dosage2");
                //Label Label2 = (Label)e.Row.FindControl("Label2");

                //Label packSize = (Label)e.Row.FindControl("packSize2");
                //Label Label3 = (Label)e.Row.FindControl("Label3");

                //if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                //{
                //    ProductStrength.Visible = false;
                //    Label1.Visible = false;
                //}
                //else
                //{
                //    ProductStrength.Visible = true;
                //    Label1.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(dosage.Text))
                //{
                //    dosage.Visible = false;
                //    Label2.Visible = false;
                //}
                //else
                //{
                //    dosage.Visible = true;
                //    Label2.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(packSize.Text))
                //{
                //    packSize.Visible = false;
                //    Label3.Visible = false;
                //}
                //else
                //{
                //    packSize.Visible = true;
                //    Label3.Visible = true;
                //}
            }
        }

        protected void ProductCat_SelectedIndexChanged(object sender, EventArgs e)
        {

           BindGridbyFilters();
        }

        protected void ProductDept_SelectedIndexChanged(object sender, EventArgs e)
        {

           BindGridbyFilters();
        }

        protected void ProductSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
           BindGridbyFilters();
            
        }

        protected void ProductName_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void ProductType_SelectedIndexChanged(object sender, EventArgs e)
        {
           BindGridbyFilters();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindGridbyFilters();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {//txtSearch.Text=""; original value
            txtSearch.Value = "";
            ddlProductOrderType.SelectedIndex = -1;
            ProductSubCat.SelectedIndex = -1;
            ProductCat.SelectedIndex = -1;
            ProductDept.SelectedIndex = -1;
            ddlActive.SelectedIndex = -1;
            ProductType.SelectedIndex = -1;
            BindGridbyFilters();
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            //if (SelectProduct.Text.Length >= 3)
            //{
            //    PopulateDropDown(SelectProduct.Text);
            //    ProductList.Visible = true;
            //}
        }

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
        //        if (ProductList.DataSource != null)
        //        {
        //            ProductList.DataSource = null;
        //        }

        //        ProductSet = null;
        //        ProductSet = ds;
        //        //ds.Tables[0].Columns.Add("ProductInfo", typeof(string), "Product_Name+ ' '+itemStrength+' '+itemPackSize+' '+itemForm");
        //        ProductList.DataSource = ds.Tables[0];
        //        ProductList.DataTextField = "Description";
        //        ProductList.DataValueField = "ProductID";
        //        ProductList.DataBind();
        //        if (ProductList != null)
        //        {
        //            ProductList.Items.Insert(0, "Select Product");
        //            ProductList.SelectedIndex = 0;
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

        protected void ProductList_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridbyFilters();
        }

        private void ExportGridToPDF()
        {
            try
            {
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=PO_" + Session["OrderNumber"].ToString() + ".pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                sw.WriteLine("");
                HtmlTextWriter hw = new HtmlTextWriter(sw);
               
                dgvStockDisplayGrid.RenderControl(hw);
                
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                String FilePath = Server.MapPath(@"~\PurchaseOrders");
                String FileName = "Inventory"+ ".pdf";
                FileStream fs = new FileStream(FilePath + @"\" + FileName, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
                writer.Close();
                pdfDoc.Close();
                fs.Close();
                //Response.Write(pdfDoc);

                //Response.Flush();
                //Response.SuppressContent = true; 
                //Response.End();
                //Response.Redirect("PO_GENEREATE.aspx");
                //dgvStockDisplayGrid.AllowPaging = true;
                //dgvStockDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                // MAINDIV.Visible = false;
                // TotalCostDiv.Visible = false;
               // btnEmail.Visible = true;
                //btnFax.Visible = true;
                //btnPrint.Enabled = false;
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

        }
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            #region Setting  parameters
            Session["Search_DepID"] = ProductDept.SelectedValue.ToString();
            Session["Search_CatID"] = ProductCat.SelectedValue.ToString();
            Session["Search_SubCatID"] = ProductSubCat.SelectedValue.ToString();
            Session["Search_ProdIdOrg"] = ddlProductOrderType.SelectedValue.ToString();
            Session["Search_ProdType"] = ProductType.SelectedItem.ToString();
            Session["Search_ProdId"] = "";
            Session["Search_ProdName"] = txtSearch.Value.ToString();
            Session["Search_Active"] = ddlActive.SelectedValue.ToString();

            #endregion

            Response.Redirect("Inventory_Print.aspx",false);


        }

        protected void btnSearchProduct_Click1(object sender, ImageClickEventArgs e)
        {
           // String Text = txtSearch.Text + '%';
           // Session["Text"] = Text;
           // ProductsPopupGrid.PopulateGrid();
            //mpeCongratsMessageDiv.Show();
        }

        protected void dgvStockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvStockDisplayGrid.EditIndex = e.NewEditIndex;
        }

        protected void dgvStockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            
        }

        protected void ddlProductOrderType_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridbyFilters();
        }

        protected void ddlActive_SelectedIndexChanged(object sender, EventArgs e)
        {
            BindGridbyFilters();
        }
    }
}