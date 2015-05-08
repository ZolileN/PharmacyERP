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

namespace IMS
{
    public partial class ViewInventory : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

                #region Populating Product Type DropDown
                ProductType.Items.Add("Select Product Type");
                ProductType.Items.Add("Medicine(HAAD)");
                ProductType.Items.Add("Medicine(Non HAAD)");
                ProductType.Items.Add("Non Medicine");
                #endregion

                #region Populating Product Department DropDown
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tblDepartment", connection);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductDept.DataSource = ds.Tables[0];
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

                }
                finally
                {
                    connection.Close();
                }
                #endregion

                #region Populating Category Dropdown
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tblCategory", connection);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductCat.DataSource = ds.Tables[0];
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

                }
                finally
                {
                    connection.Close();
                }
                #endregion

                #region Populating SubCategory Dropdown
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tblSub_Category", connection);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductSubCat.DataSource = ds.Tables[0];
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

                }
                finally
                {
                    connection.Close();
                }
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

                BindGridbyFilters();
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
                        StockDisplayGrid.DataSource = ds;
                        StockDisplayGrid.DataBind();
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
            }
            finally
            {
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
                    
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_ViewInventory_byFilters", connection);
                    #region with parameter approach
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_SysID", id);

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


                    if (ProductType.SelectedIndex <= 0)
                    {
                        command.Parameters.AddWithValue("@p_ProdType", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_ProdType", ProductType.SelectedItem.ToString());
                    }

                    if (string.IsNullOrEmpty(txtSearch.Text))
                    {
                        command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_ProdID", txtSearch.Text);
                    }

                    //if (ProductList.SelectedIndex <= 0)
                    //{
                    //    command.Parameters.AddWithValue("@p_ProdID", DBNull.Value);
                    //}
                    //else
                    //{
                    //    command.Parameters.AddWithValue("@p_ProdID", Convert.ToInt32(ProductList.SelectedValue.ToString()));
                    //}
                    #endregion

                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    SA.Fill(ds);
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

      

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Print"))
                {
                   // Image BarcodeImage=new Image();
                   // //int 
                   
                   // Label Barcode = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("BarCode");
                   // Label Quantity = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblQuantity");
                   // Label ProductName = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ProductName");
                   // long ProductBarCode = long.Parse(e.CommandArgument.ToString());
                   //// int PrintQuantity = Int32.Parse(Quantity.Text.ToString());

                   // #region barcode creation
                   // #region previous approach
                   // //System.Drawing.Image barcodeImage = null;
                   // //BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                   // //b.IncludeLabel = true;
                   // //barcodeImage = b.Encode(BarcodeLib.TYPE.EAN13, ProductBarCode.ToString(), System.Drawing.ColorTranslator.FromHtml("#" + "000000"), System.Drawing.ColorTranslator.FromHtml("#" + "FFFFFF"), 300, 150);
                   // ////string strImageURL = "generateBarCode.aspx?d=" + ProductBarCode + "&h=" + 300 + "&w=" + 150;
                   // ////BarcodeImage.ImageUrl = strImageURL;
                   // ////BarcodeImage.Width = 300;
                   // ////BarcodeImage.Height = 150;

                   // //BarcodeUtility bUtil = new BarcodeUtility();
                   // //bUtil.Print(barcodeImage); 
                   // #endregion
                   // ucPrint.ProductName = ProductName.Text;
                   // ucPrint.CompanyName = "Pharmacy";
                   // ucPrint.PrintCount = int.Parse(Quantity.Text);
                   // ucPrint.BarcodeValue = Barcode.Text;
                    
                   // mpeEditProduct.Show();
                   // #endregion
                    
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Convert.ToInt32(Session["UserSys"]).Equals(1))
            {
                Response.Redirect("ManageStocks.aspx", false);
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
        {
            txtSearch.Text = "";
            ddlProductOrderType.SelectedIndex = -1;
            ProductSubCat.SelectedIndex = -1;
            ProductCat.SelectedIndex = -1;
            ProductDept.SelectedIndex = -1;
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
               
                StockDisplayGrid.RenderControl(hw);
                
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
                //StockDisplayGrid.AllowPaging = true;
                //StockDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                // MAINDIV.Visible = false;
                // TotalCostDiv.Visible = false;
               // btnEmail.Visible = true;
                //btnFax.Visible = true;
                //btnPrint.Enabled = false;
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
           // Session["Search_ProdId"] = ProductList.SelectedValue.ToString();
            Session["Search_ProdId"] = txtSearch.Text.ToString();


            #endregion

            Response.Redirect("Inventory_Print.aspx");


        }

        protected void btnSearchProduct_Click1(object sender, ImageClickEventArgs e)
        {
            String Text = txtSearch.Text + '%';
            Session["Text"] = Text;
            ProductsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }
    }
}