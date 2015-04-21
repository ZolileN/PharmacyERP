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
    public partial class Edit_DeleteProduct : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if(Session["Linkto"].Equals("EDIT"))
                {
                    btnDeleteProduct.Enabled = false;
                    btnCreateProduct.Enabled = true;
                }
                else if (Session["Linkto"].Equals("DELETE"))
                {
                    btnDeleteProduct.Enabled = true;
                    btnCreateProduct.Enabled = false;
                }
                ProductSet = new DataSet();
                
            }
        }

        protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            #region Populating Product Type DropDown
            ProductType.Items.Add("Select Type");
            ProductType.Items.Add("Medicine (HAAD)");
            ProductType.Items.Add("Medicine (NON HAAD)");
            ProductType.Items.Add("Non Medicine");
            
            #endregion

            #region Getting Product Details
            try
            {
                DataView dv = new DataView();
                dv = ProductSet.Tables[0].DefaultView;
                dv.RowFilter = "ProductID = '"+SelectProduct.SelectedValue.ToString()+"'";
                dt = dv.ToTable();
                String Query = "Select tblSub_Category.Sub_CatID as SubcatID, tblSub_Category.Name AS SUBCAT,tblCategory.Name AS CAT,tblDepartment.Name AS DEPT From tblSub_Category INNER JOIN tblCategory ON tblSub_Category.CategoryID = tblCategory.CategoryID INNER JOIN tblDepartment ON tblCategory.DepartmentID = tblDepartment.DepId Where Sub_CatID = '" + dt.Rows[0]["SubCategoryID"] + "'";
                
                connection.Open();
                SqlCommand command = new SqlCommand(Query,connection);
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(ds);
                if (ds.Tables[0].Rows[0]["SubcatID"] != DBNull.Value || !ds.Tables[0].Rows[0]["SubcatID"].Equals(""))
                {
                    Session["SubCatID"] = ds.Tables[0].Rows[0]["SubcatID"].ToString();
                }
                if (dt.Rows[0]["Product_Id_Org"] != DBNull.Value || !dt.Rows[0]["ItemCode"].Equals(""))
                {
                    BarCodeSerial.Text = dt.Rows[0]["Product_Id_Org"].ToString();
                }
                if (dt.Rows[0]["ItemCode"] != DBNull.Value || !dt.Rows[0]["ItemCode"].Equals(""))
                {
                    GreenRainCode.Text = dt.Rows[0]["ItemCode"].ToString();
                }
                if (dt.Rows[0]["Product_Name"] != DBNull.Value || !dt.Rows[0]["Product_Name"].Equals(""))
                {
                    ProductName.Text = dt.Rows[0]["Product_Name"].ToString();
                }
                if (dt.Rows[0]["Description"] != DBNull.Value || !dt.Rows[0]["Description"].Equals(""))
                {
                    ProdcutDesc.Text = dt.Rows[0]["Description"].ToString();
                }
                if (dt.Rows[0]["Brand_Name"] != DBNull.Value || !dt.Rows[0]["Brand_Name"].Equals(""))
                {
                    ProdcutBrand.Text = dt.Rows[0]["Brand_Name"].ToString();
                }
                if (dt.Rows[0]["DrugType"] != DBNull.Value )
                {
                    if(!dt.Rows[0]["DrugType"].Equals("")){
                    ProductType.SelectedValue = dt.Rows[0]["DrugType"].ToString();
                    }
                }

                //if (ds.Tables[0].Rows[0]["DEPT"] != DBNull.Value || !ds.Tables[0].Rows[0]["DEPT"].Equals(""))
                //{
                //    ProductDept.SelectedValue = ds.Tables[0].Rows[0]["DEPT"].ToString();
                //}

                if (ds.Tables[0].Rows[0]["CAT"] != DBNull.Value || !ds.Tables[0].Rows[0]["CAT"].Equals(""))
                {
                    ProductCat.SelectedValue = ds.Tables[0].Rows[0]["CAT"].ToString();
                }
                if (ds.Tables[0].Rows[0]["SUBCAT"] != DBNull.Value || !ds.Tables[0].Rows[0]["SUBCAT"].Equals(""))
                {
                    ProductSubCat.SelectedValue = ds.Tables[0].Rows[0]["SUBCAT"].ToString();
                }
                if (dt.Rows[0]["UnitCost"] != DBNull.Value || !ds.Tables[0].Rows[0]["UnitCost"].Equals(""))
                {
                    ProductCost.Text = dt.Rows[0]["UnitCost"].ToString();
                }
                if (dt.Rows[0]["SP"] != DBNull.Value || !ds.Tables[0].Rows[0]["SP"].Equals(""))
                {
                    ProductSale.Text = dt.Rows[0]["SP"].ToString();
                }
                if (dt.Rows[0]["MaxiMumDiscount"] != DBNull.Value || !ds.Tables[0].Rows[0]["MaxiMumDiscount"].Equals(""))
                {
                    ProductDiscount.Text = dt.Rows[0]["MaxiMumDiscount"].ToString();
                }
                if (dt.Rows[0]["ProductID"] != DBNull.Value || !ds.Tables[0].Rows[0]["ProductID"].Equals(""))
                {
                    Session["ProductID"] = dt.Rows[0]["ProductID"].ToString();
                }

            }
            catch(Exception ex)
            {
                
            }
            finally
            {
                connection.Close();
            }
            #endregion

            #region Populating Product Department DropDown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * From tblDepartment", connection);
                DataSet dsA = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(dsA);
                ProductDept.DataSource = dsA.Tables[0];
                ProductDept.DataTextField = "Name";
                ProductDept.DataValueField = "DepId";
                          
                
                ProductDept.DataBind();
                if (ds.Tables[0].Rows[0]["DEPT"] != DBNull.Value || !ds.Tables[0].Rows[0]["DEPT"].Equals(""))
                {
                    ProductDept.SelectedIndex = -1; // ProductDept.Items.IndexOf(ProductDept.Items.FindByText(ds.Tables[0].Rows[0]["DEPT"].ToString()));

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

        protected void ProductDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Populating Category Dropdown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * From tblCategory Where DepartmentID = '" + ProductDept.SelectedValue.ToString() + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductCat.DataSource = ds.Tables[0];
                ProductCat.DataTextField = "Name";
                ProductCat.DataValueField = "CategoryID";
                ProductCat.DataBind();
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

        protected void ProductCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Populating SubCategory Dropdown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * From tblSub_Category WHERE CategoryID ='" + ProductCat.SelectedValue.ToString() + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSubCat.DataSource = ds.Tables[0];
                ProductSubCat.DataTextField = "Name";
                ProductSubCat.DataValueField = "Sub_CatID";
                ProductSubCat.DataBind();
                
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

        protected void btnCreateProduct_Click(object sender, EventArgs e)
        {
            if(btnCreateProduct.Text.Equals("EDIT"))
            {
                #region Editing Product(s)
                btnCreateProduct.Text = "UPDATE";
                BarCodeSerial.Enabled = false;
                GreenRainCode.Enabled = true;
                ProductName.Enabled = true;
                ProdcutDesc.Enabled = true;
                ProdcutBrand.Enabled = true;
                ProductType.Enabled = true;
                ProductDept.Enabled = true;
                ProductCat.Enabled = true;
                ProductSubCat.Enabled = true;
                ProductCost.Enabled = true;
                ProductSale.Enabled = true;
                ProductDiscount.Enabled = true;
                #endregion
            }
            else if (btnCreateProduct.Text.Equals("UPDATE"))
            {
                #region Updating Product
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_UpdateProduct", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    int res1, res2,res5;
                    float res3, res4;
                    if (int.TryParse(Session["ProductID"].ToString(), out res1))
                    {
                        command.Parameters.AddWithValue("@p_ProductID", res1);
                    }
                    else 
                    {
                        WebMessageBoxUtil.Show("invalid product Id");
                        return;
                    }
                    
                    if (int.TryParse(Session["SubCatID"].ToString(), out res2))
                    {
                        command.Parameters.AddWithValue("@p_SubCategoryID", res2);
                    }//
                    else
                    {
                        WebMessageBoxUtil.Show("invalid sub category Id");
                        return;
                    }
                    
                    if (float.TryParse(ProductCost.Text.ToString(), out res3))
                    {
                        command.Parameters.AddWithValue("@p_UnitCost", res3);
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("invalid unit cost");
                        return;
                    }

                    if (float.TryParse(ProductSale.Text.ToString(), out res3))
                    {
                        command.Parameters.AddWithValue("@p_SP", res3);
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("invalid sales price");
                        return;
                    }

                    if (int.TryParse(ProductDiscount.Text.ToString(), out res5))
                    {
                        command.Parameters.AddWithValue("@p_MaxiMumDiscount", res5);
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("invalid discount");
                        return;
                    }
                    command.Parameters.AddWithValue("@p_BarCodeSerial", BarCodeSerial.Text.ToString());
                    command.Parameters.AddWithValue("@p_ProductCode", GreenRainCode.Text.ToString());
                    command.Parameters.AddWithValue("@p_ProductName", ProductName.Text.ToString());
                    command.Parameters.AddWithValue("@p_Description", ProdcutDesc.Text.ToString());
                    command.Parameters.AddWithValue("@p_BrandName", ProdcutBrand.Text.ToString());
                    command.Parameters.AddWithValue("@p_ProductType", ProductType.SelectedItem.ToString());
                    

                    command.ExecuteNonQuery();
                    WebMessageBoxUtil.Show("SuccessFully Updated");
                    SelectProduct.SelectedIndex = 0;
                    BarCodeSerial.Text = "";
                    GreenRainCode.Text = "";
                    ProductName.Text = "";
                    ProductDept.SelectedIndex = -1;
                    ProductCat.SelectedIndex = -1;
                    ProductSubCat.SelectedIndex = -1;
                    ProductType.SelectedIndex = 0;
                    ProdcutBrand.Text = "";
                    ProdcutDesc.Text = "";
                    ProductSale.Text = "";
                    ProductCost.Text = "";
                    ProductDiscount.Text = "";
                }
                catch(Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }

                BarCodeSerial.Enabled = false;
                GreenRainCode.Enabled = false;
                ProductName.Enabled = false;
                ProdcutDesc.Enabled = false;
                ProdcutBrand.Enabled = false;
                ProductType.Enabled = false;
                ProductDept.Enabled = false;
                ProductCat.Enabled = false;
                ProductSubCat.Enabled = false;
                ProductCost.Enabled = false;
                ProductSale.Enabled = false;
                ProductDiscount.Enabled = false;
                btnCreateProduct.Text = "EDIT";
                #endregion
            }
        }

        protected void btnCancelProduct_Click(object sender, EventArgs e)
        {
            Response.Redirect("./ManageProducts.aspx");
        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {
            
            //Print Message First
            #region Delete Product
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Update tbl_ProductMaster set Status = 0 where ProductID = '" + Int32.Parse(Session["ProductID"].ToString()) + "'", connection);
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SuccessFully Deleted");
                SelectProduct.SelectedIndex = 0;
                BarCodeSerial.Text = "";
                GreenRainCode.Text = "";
                ProductName.Text = "";
                ProductDept.SelectedIndex = 0;
                ProductCat.SelectedIndex = 0;
                ProductSubCat.SelectedIndex = 0;
                ProductType.SelectedIndex = 0;
                ProdcutBrand.Text = "";
                ProdcutDesc.Text = "";
                ProductSale.Text = "";
                ProductCost.Text = "";
                ProductDiscount.Text = "";
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }

        protected void ProductSubCat_SelectedIndexChanged(object sender, EventArgs e)
        {
            Session["SubCatID"] = ProductSubCat.SelectedValue.ToString();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageProducts.aspx", false);
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (txtProduct.Text.Length >= 3)
            {
                PopulateDropDown(txtProduct.Text);
                SelectProduct.Visible = true;
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("SELECT * From tbl_ProductMaster Where tbl_ProductMaster.Product_Name LIKE '" + Text + "' AND tbl_ProductMaster.Product_Id_Org LIKE '444%' AND Status = 1", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (SelectProduct.DataSource != null)
                {
                    SelectProduct.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                SelectProduct.DataSource = ds.Tables[0];
                SelectProduct.DataTextField = "Product_Name";
                SelectProduct.DataValueField = "ProductID";
                SelectProduct.DataBind();
                if (SelectProduct != null)
                {
                    SelectProduct.Items.Insert(0, "Select Product");
                    SelectProduct.SelectedIndex = 0;
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
}