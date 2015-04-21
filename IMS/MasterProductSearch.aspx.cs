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
    public partial class MasterProductSearch : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["ProductMasterSearch"] != null && Session["ProductMasterSearch"].ToString() != null && Session["ProductMasterSearch"].ToString() != "")
                {
                    PopulateDropDown(Session["ProductMasterSearch"].ToString());
                }
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("SELECT * From tbl_ProductSuperMaster Where tbl_ProductSuperMaster.DrugName LIKE '" + Text + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (StockDisplayGrid.DataSource != null)
                {
                    StockDisplayGrid.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

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
        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           try
           {
               if (e.CommandName.Equals("Select"))
               {
                   Label ItemNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("DrugID");
                   Label ItemName = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("DrugName");
                   Label ItemType = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("DrugType");
                   Label Manufacterer = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Manufacterer");
                   Label Category = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Category");
                   Label GenericName = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("GenericName");
                   Label Control = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Control");
                   Label Bin_Number = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Bin_Number");
                   Label GreenRainCode = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("GreenRainCode");
                   Label Brand_Name = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Brand_Name");
                   Label MaxiMumDiscount = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("MaxiMumDiscount");
                   Label LineID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("LineID");
                   Label UnitSale = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UnitSale");
                   Label UnitCost = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("UnitCost");
                   Label itemAWT = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("itemAWT");
                   Label itemForm = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("itemForm");
                   Label itemStrength = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("itemStrength");
                   Label itemPackType = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("itemPackType");
                   Label itemPackSize = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("itemPackSize");

                   Session["PageMasterProduct"] = "true";

                   Session["MS_ItemNo"] = ItemNo.Text.ToString();
                   Session["MS_ItemName"] = ItemName.Text.ToString();
                   Session["MS_ItemType"] = ItemType.Text.ToString();
                   Session["MS_Manufacterer"] = Manufacterer.Text.ToString();
                   Session["MS_Category"] = Category.Text.ToString();
                   Session["MS_GenericName"] = GenericName.Text.ToString();
                   Session["MS_Control"] = Control.Text.ToString();
                   Session["MS_BinNumber"] = Bin_Number.Text.ToString();
                   Session["MS_GreenRainCode"] = GreenRainCode.Text.ToString();
                   Session["MS_BrandName"] = Brand_Name.Text.ToString();
                   Session["MS_MaxiMumDiscount"] = MaxiMumDiscount.Text.ToString();
                   Session["MS_LineID"] = LineID.Text.ToString();
                   Session["MS_UnitSale"] = UnitSale.Text.ToString();
                   Session["MS_UnitCost"] = UnitCost.Text.ToString();
                   Session["MS_itemAWT"] = itemAWT.Text.ToString();
                   Session["MS_itemForm"] = itemForm.Text.ToString();
                   Session["MS_itemStrength"] = itemStrength.Text.ToString();
                   Session["MS_itemPackType"] = itemPackType.Text.ToString();
                   Session["MS_itemPackSize"] = itemPackSize.Text.ToString();

                   Response.Redirect("Addproduct.aspx");


               }
           }
            catch(Exception ex)
           {

           }
            finally
           {

           }
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            if (Session["ProductMasterSearch"] != null && Session["ProductMasterSearch"].ToString() != null && Session["ProductMasterSearch"].ToString() != "")
            {
                PopulateDropDown(Session["ProductMasterSearch"].ToString());
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("Addproduct.aspx");
        }
    }
}