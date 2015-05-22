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
    public partial class AddProduct : System.Web.UI.Page
    {
        public static SqlConnection connection= new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // need to change now for new fcked up logic 

                if(Session["MODE"].Equals("ADD"))
                {
                    btnCreateProduct.Text = "ADD";

                    #region Populating BarCode Serial
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select Count(*) From tbl_ProductMaster Where Product_Id_Org LIKE '2%';", connection);
                        DataSet ds = new DataSet();
                        SqlDataAdapter sA = new SqlDataAdapter(command);
                        sA.Fill(ds);

                        if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(7))
                        {
                            BarCodeSerial.Text = "2" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(6))
                        {
                            BarCodeSerial.Text = "20" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(5))
                        {
                            BarCodeSerial.Text = "200" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(4))
                        {
                            BarCodeSerial.Text = "2000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(3))
                        {
                            BarCodeSerial.Text = "20000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }

                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(2))
                        {
                            BarCodeSerial.Text = "200000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }

                        else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(1))
                        {
                            BarCodeSerial.Text = "2000000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                        }
                        else if (ds.Tables[0].Rows[0][0].ToString().Length < 1)
                        {
                            BarCodeSerial.Text = "2000000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
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

                    BarCodeSerial.Visible = true;
                }
                else if (Session["MODE"].Equals("EDIT"))
                {
                    btnCreateProduct.Text = "UPDATE";
                    //BarCodeSerial.Visible = false;
                }

                #region Master Search Mechanism
                if (Session["PageMasterProduct"] != null && Session["PageMasterProduct"].ToString() != null && 
                    Session["PageMasterProduct"].ToString() != "" && Session["PageMasterProduct"].ToString()!="false")
                {
                    FromMaster_Load(Session["MS_ItemNo"].ToString(), Session["MS_ItemName"].ToString(), Session["MS_ItemType"].ToString(), Session["MS_Manufacterer"].ToString(),
                                    Session["MS_Category"].ToString(), Session["MS_GenericName"].ToString(), Session["MS_Control"].ToString(), Session["MS_BinNumber"].ToString(),
                                    Session["MS_GreenRainCode"].ToString(), Session["MS_BrandName"].ToString(), Session["MS_MaxiMumDiscount"].ToString(), Session["MS_LineID"].ToString(),
                                    Session["MS_UnitSale"].ToString(), Session["MS_UnitCost"].ToString(), Session["MS_itemAWT"].ToString(), Session["MS_itemForm"].ToString(),
                                    Session["MS_itemStrength"].ToString(), Session["MS_itemPackType"].ToString(), Session["MS_itemPackSize"].ToString(), Session["MS_Description"].ToString()
                                    , Session["MS_Bonus12"].ToString(), Session["MS_Bonus25"].ToString(), Session["MS_Bonus50"].ToString(), Session["MS_Active"].ToString());
                }
                #endregion


                #region Populating Product Type DropDown
                ProductType.Items.Add("Medicine(HAAD)");
                ProductType.Items.Add("Medicine(Non HAAD)");
                ProductType.Items.Add("NonMedicine");

                if (Session["MODE"].Equals("EDIT"))
                {
                    foreach (ListItem Items  in ProductType.Items) 
                    {
                        if (Items.Text.Equals(Session["MS_ItemType"].ToString())) 
                        {
                            ProductType.SelectedIndex = ProductType.Items.IndexOf( Items);
                            break;
                        }
                    }
                    //int selIndex;
                    //int.TryParse(Session["MS_ProductOrderType"].ToString(), out selIndex);
                    //ddlProductOrderType.SelectedIndex = selIndex;
                    // foreach( )
                }
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

                #region Populate Product Order Type

                ddlProductOrderType.DataSource=IMSGlobal.GetOrdersType();
                ddlProductOrderType.DataTextField = "Name";
                ddlProductOrderType.DataValueField = "OrderTypeId";
                ddlProductOrderType.DataBind();
                
                if (ddlProductOrderType !=null)
                {
                    ddlProductOrderType.Items.Insert(0,"Select Product Order Type");
                    ddlProductOrderType.SelectedIndex = 0;
                }
               
                if (Session["MODE"].Equals("EDIT")) 
                {
                    int selIndex; 
                    int.TryParse(Session["MS_ProductOrderType"].ToString(),out selIndex); 
                    ddlProductOrderType.SelectedIndex =selIndex;
                   // foreach( )
                }
                #endregion
            }
        }

       
        public void FromMaster_Load(String ItemNo, String ItemName, String ItemType, String Manufacturer, String Category, String GenericName,
                                    String Control, String BinNumber, String GreenRain, String BrandName, String MaxDiscount, String LineID, 
                                    String UnitSale, String UnitCost, String ItemAwt, String Form, String Strength, String itemPackType,
                                    String itemPackSize, String Description="", string b12 = "", string b25 = "", string b50 = "",string _active="")
        {
            BarCodeSerial.Text = ItemNo;
            GreenRainCode.Text = GreenRain;
            ProductName.Text = ItemName;
            ProdcutBrand.Text = BrandName;
            ProductType.Text = ItemType;
            ProductCost.Text = UnitCost;
            ProductSale.Text = UnitSale;
            WholeSalePrice.Text = ItemAwt;
            ProductDiscount.Text = MaxDiscount;
            ProdcutDesc.Text = Description;
            ItemForm.Text = Form;
            ItemStrength.Text = Strength;
            PackType.Text = itemPackType;
            PackSize.Text = itemPackSize;
            binNumber.Text = BinNumber;
            bonus12.Text = b12;
            bonus25.Text = b25;
            bonus50.Text = b50;
            if (_active.Equals("1"))
            {
                chkActive.Checked = true;
            }
            else
            {
                chkActive.Checked = false;
            }
        }
        protected void btnAddProduct_Click(object sender, EventArgs e)
        {

        }

        protected void btnDeleteProduct_Click(object sender, EventArgs e)
        {

        }

        protected void btnEditProduct_Click(object sender, EventArgs e)
        {

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session["PageMasterProduct"] = "false";
            Response.Redirect("~/ManageProducts.aspx", false);
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {

        }

        protected void AddProduct_Click(object sender, EventArgs e)
        {

        }

        protected void btnCreateProduct_Click(object sender, EventArgs e)
        {
                if (ProductType.SelectedItem.ToString() == "Medicine(HAAD)" && 
                    (GreenRainCode.Text.Equals("") || GreenRainCode.Text.Equals(null)))
                {
                        WebMessageBoxUtil.Show("FOR HADD MEDICINES, PLEASE ENTER THE RESPECTIVE GREENRAIN CODE");
                }
                else
                {
                    if (btnCreateProduct.Text.Equals("ADD"))
                    {
                        int x = 0;

                        #region Creation Product
                        string errorMessage = "";
                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_InsertProduct", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_BarCodeSerial", BarCodeSerial.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductCode", GreenRainCode.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductName", ProductName.Text.ToString());
                            command.Parameters.AddWithValue("@p_Description", ProdcutDesc.Text.ToString());
                            command.Parameters.AddWithValue("@p_BrandName", ProdcutBrand.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductType", ProductType.SelectedItem.ToString());
                            if (ddlProductOrderType.SelectedIndex > 0)
                            {
                                command.Parameters.AddWithValue("@p_productOrderType", int.Parse(ddlProductOrderType.SelectedValue.ToString()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_productOrderType", DBNull.Value);
                            }
                            int res1, res4, res7, res8, res9;
                            float res2, res3, res5;
                            if (int.TryParse(ProductSubCat.SelectedValue.ToString(), out res1))
                            {
                                command.Parameters.AddWithValue("@p_SubCategoryID", res1);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_SubCategoryID", 0);
                            }

                            if (float.TryParse(ProductCost.Text.ToString(), out res2))
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", res2);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", 0);
                            }

                            if (float.TryParse(ProductSale.Text.ToString(), out res3))
                            {
                                command.Parameters.AddWithValue("@p_SP", res3);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_SP", 0);
                            }

                            if (int.TryParse(ProductDiscount.Text.ToString(), out res4))
                            {
                                command.Parameters.AddWithValue("@p_MaxiMumDiscount", res4);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_MaxiMumDiscount", 0);
                            }

                            if (float.TryParse(WholeSalePrice.Text.ToString(), out res5))
                            {
                                command.Parameters.AddWithValue("@p_AWT", res5);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_AWT", 0);
                            }
                            if (int.TryParse(bonus12.Text, out res7))
                            {
                                command.Parameters.AddWithValue("@p_bonus12", res7);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus12", 0);
                            }

                            if (int.TryParse(bonus25.Text, out res8))
                            {
                                command.Parameters.AddWithValue("@p_bonus25", res8);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus25", 0);
                            }

                            if (int.TryParse(bonus50.Text, out res9))
                            {
                                command.Parameters.AddWithValue("@p_bonus50", res9);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus50", 0);
                            }

                            if (chkActive.Checked == true)
                            {
                                command.Parameters.AddWithValue("@p_Active", 1);
                            }
                            else 
                            {
                                command.Parameters.AddWithValue("@p_Active", 0);
                            }
                            command.Parameters.AddWithValue("@p_form", ItemForm.Text.ToString());
                            command.Parameters.AddWithValue("@p_strength", ItemStrength.Text.ToString());
                            command.Parameters.AddWithValue("@p_packtype", PackType.Text.ToString());
                            command.Parameters.AddWithValue("@p_packsize", PackSize.Text.ToString());

                            command.Parameters.AddWithValue("@p_shelf", shelfNumber.Text.ToString());
                            command.Parameters.AddWithValue("@p_rack", rackNumber.Text.ToString());
                            command.Parameters.AddWithValue("@p_bin", binNumber.Text.ToString());

                            x = command.ExecuteNonQuery();
                        }
                        catch (Exception ex)
                        {
                            errorMessage = ex.Message;
                        }
                        finally
                        {
                            connection.Close();
                        }

                        if (x == 1)
                        {
                            WebMessageBoxUtil.Show("Record Inserted Successfully");
                            Session["PageMasterProduct"] = "false";
                            BarCodeSerial.Text = "";
                            GreenRainCode.Text = "";
                            ProductName.Text = "";
                            ProdcutDesc.Text = "";
                            ProdcutBrand.Text = "";
                            ProductType.SelectedIndex = -1;
                            ProductSubCat.SelectedIndex = -1;
                            ProductDept.SelectedIndex = -1;
                            ProductCat.SelectedIndex = -1;
                            ddlProductOrderType.SelectedIndex = 0;
                            ProductCost.Text = "";
                            ProductSale.Text = "";
                            ProductDiscount.Text = "";
                            rackNumber.Text = "";
                            shelfNumber.Text = "";
                            binNumber.Text = "";
                            WholeSalePrice.Text = "";
                            ItemForm.Text = "";
                            ItemStrength.Text = "";
                            PackType.Text = "";
                            PackSize.Text = "";
                        }
                        else
                        {
                            WebMessageBoxUtil.Show(errorMessage);
                        }
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
                            command.Parameters.AddWithValue("@p_BarCodeSerial", BarCodeSerial.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductCode", GreenRainCode.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductName", ProductName.Text.ToString());
                            command.Parameters.AddWithValue("@p_Description", ProdcutDesc.Text.ToString());
                            command.Parameters.AddWithValue("@p_BrandName", ProdcutBrand.Text.ToString());
                            command.Parameters.AddWithValue("@p_ProductType", ProductType.SelectedItem.ToString());
                            if (ddlProductOrderType.SelectedIndex > 0)
                            {
                                command.Parameters.AddWithValue("@p_productOrderType", int.Parse(ddlProductOrderType.SelectedValue.ToString()));
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_productOrderType", DBNull.Value);
                            }
                            int res1, res4,res6,res7,res8,res9;
                            float res2, res3, res5;

                            
                            if (int.TryParse(Session["MS_ProductID"].ToString(), out res6))
                            {
                                command.Parameters.AddWithValue("@p_ProductID", res6);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_ProductID", 0);
                            }

                            if (int.TryParse(bonus12.Text, out res7))
                            {
                                command.Parameters.AddWithValue("@p_bonus12", res7);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus12", 0);
                            }

                            if (int.TryParse(bonus25.Text, out res8))
                            {
                                command.Parameters.AddWithValue("@p_bonus25", res8);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus25", 0);
                            }

                            if (int.TryParse(bonus50.Text, out res9))
                            {
                                command.Parameters.AddWithValue("@p_bonus50", res9);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_bonus50", 0);
                            }

                            if (float.TryParse(ProductCost.Text.ToString(), out res2))
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", res2);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", 0);
                            }

                            if (float.TryParse(ProductSale.Text.ToString(), out res3))
                            {
                                command.Parameters.AddWithValue("@p_SP", res3);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_SP", 0);
                            }

                            if (int.TryParse(ProductDiscount.Text.ToString(), out res4))
                            {
                                command.Parameters.AddWithValue("@p_MaxiMumDiscount", res4);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_MaxiMumDiscount", 0);
                            }

                            if (float.TryParse(WholeSalePrice.Text.ToString(), out res5))
                            {
                                command.Parameters.AddWithValue("@p_AWT", res5);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_AWT", 0);
                            }
                            if (chkActive.Checked == true)
                            {
                                command.Parameters.AddWithValue("@p_Active", 1);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_Active", 0);
                            }
                            
                            command.Parameters.AddWithValue("@p_form", ItemForm.Text.ToString());
                            command.Parameters.AddWithValue("@p_strength", ItemStrength.Text.ToString());
                            command.Parameters.AddWithValue("@p_packtype", PackType.Text.ToString());
                            command.Parameters.AddWithValue("@p_packsize", PackSize.Text.ToString());

                            command.Parameters.AddWithValue("@p_shelf", shelfNumber.Text.ToString());
                            command.Parameters.AddWithValue("@p_rack", rackNumber.Text.ToString());
                            command.Parameters.AddWithValue("@p_bin", binNumber.Text.ToString());


                            int x = command.ExecuteNonQuery();
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product SuccessFully Updated.')", true);
                            if (x > 0)
                            {
                                WebMessageBoxUtil.Show("SuccessFully Updated");
                                Session["PageMasterProduct"] = "false";
                                //SelectProduct.SelectedIndex = 0;
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
                                WholeSalePrice.Text = "";
                                ItemForm.Text = "";
                                ItemStrength.Text = "";
                                PackType.Text = "";
                                PackSize.Text = "";
                                bonus12.Text = "";
                                bonus25.Text = "";
                                bonus50.Text = "";
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            connection.Close();
                        }
                        
                        Response.Redirect("ManageProducts.aspx",false);
                        #endregion
                    }
                }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {

        }

        protected void btnCancelProduct_Click(object sender, EventArgs e)
        {
                Session["PageMasterProduct"] = "false";
                GreenRainCode.Text=string.Empty;
                ProductName.Text=string.Empty;
                ProdcutDesc.Text = string.Empty;
                ProdcutBrand.Text = string.Empty;
                chkActive.Checked = true;
                ProductType.SelectedIndex = -1;
                ProductDept.SelectedIndex=0;
                ddlProductOrderType.SelectedIndex = 0;
                ProductCat.Items.Clear();
                ProductSubCat.Items.Clear();
                ProductCost.Text = string.Empty;
                ProductSale.Text = string.Empty;
                ProductDiscount.Text = string.Empty;
                bonus12.Text = string.Empty;
                bonus25.Text = string.Empty;
                bonus50.Text = string.Empty;
        }

        protected void ProductDept_SelectedIndexChanged(object sender, EventArgs e)
        {
            #region Populating Category Dropdown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * From tblCategory Where DepartmentID = '"+ ProductDept.SelectedValue.ToString() +"'", connection);
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
        }

        protected void btnMasterSearch_Click(object sender, EventArgs e)
        {
            Session["ProductMasterSearch"] = txtProduct.Text.ToString();
            Response.Redirect("MasterProductSearch.aspx");
        }

     

        
       

        
    }
}