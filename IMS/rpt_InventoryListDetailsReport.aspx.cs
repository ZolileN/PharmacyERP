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
using System.IO;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using IMSBusinessLogic;
using log4net;
using IMS.Util;
using IMSBusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;

namespace IMS
{
    public partial class rpt_InventoryListDetailsReport : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        ReportBLL reportbll = new ReportBLL();
        public ReportDocument myReportDocument = new ReportDocument();

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
                        ProductSubCat.DataSource = SubCategoryBLL.GetSubCategoriesBasic();
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

       

        protected void btnRefresh_Click(object sender, EventArgs e)
        {//txtSearch.Text=""; original value
            txtSearch.Value = "";
           
            ProductSubCat.SelectedIndex = -1;
            ProductCat.SelectedIndex = -1;
            ProductDept.SelectedIndex = -1;

            
            
            
        }

        protected void btnShowREPORT_Click(object sender, EventArgs e) {

            int DepartmentID = int.Parse(ProductDept.SelectedValue); ;



            int CategoryID = int.Parse(ProductCat.SelectedValue);
            int subCategoryID = int.Parse(ProductSubCat.SelectedValue);
            string ProductName = txtSearch.Value;

            DataSet ds = reportbll.rpt_InventoryListDetailsReport(DepartmentID, CategoryID, subCategoryID, ProductName);

           

            myReportDocument.Load(Server.MapPath("~/ItemListDetailsReport.rpt"));
            App_Code.Barcode dsReport = new App_Code.Barcode();
            dsReport.Tables[0].Merge((DataTable)ds.Tables[0]);
            myReportDocument.SetDataSource(dsReport.Tables[0]);


            if (ProductName.Equals(""))
            {
                myReportDocument.SetParameterValue("Product", "All");
            }
            else {
                myReportDocument.SetParameterValue("Product", ProductName);
            }
            if (ProductSubCat.SelectedIndex == 0)
            {
                myReportDocument.SetParameterValue("Subcategory", "All");
            }
            else {
                myReportDocument.SetParameterValue("Subcategory", ProductSubCat.SelectedValue);
            }
            if (ProductCat.SelectedIndex == 0)
            {
                myReportDocument.SetParameterValue("Category", "All");
            }
            else {
                myReportDocument.SetParameterValue("Category", ProductCat.SelectedValue);
                
            }

            if (ProductDept.SelectedIndex == 0)
            {
                myReportDocument.SetParameterValue("Department", "All");
            }
            else {
                myReportDocument.SetParameterValue("Department", ProductDept.SelectedValue);
            }

            
            
            
            
            

            Session["ReportDocument"] = myReportDocument;
            Session["ReportPrinting_Redirection"] = "rpt_InventoryListDetailsReport.aspx";

            Response.Redirect("CrystalReportViewer.aspx");

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

      

        
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            #region Setting  parameters
            Session["Search_DepID"] = ProductDept.SelectedValue.ToString();
            Session["Search_CatID"] = ProductCat.SelectedValue.ToString();
            Session["Search_SubCatID"] = ProductSubCat.SelectedValue.ToString();
            
           
            Session["Search_ProdId"] = "";
            Session["Search_ProdName"] = txtSearch.Value.ToString();
           

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

    

        
    }
}