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
using System.Globalization;
using IMSCommon.Util;
using AjaxControlToolkit;

namespace IMS.UserControl
{
    public partial class ProductSearch : System.Web.UI.UserControl
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
               
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("SELECT ProductID,Product_Name From tbl_ProductMaster Where tbl_ProductMaster.Product_Name LIKE '" + Text + "' AND tbl_ProductMaster.Product_Id_Org LIKE '444%' AND Status = 1", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (SysDDL.DataSource != null)
                {
                    SysDDL.DataSource = null;
                }

                SysDDL.DataSource = ds.Tables[0];
                SysDDL.DataTextField = "Product_Name";
                SysDDL.DataValueField = "ProductID";
                SysDDL.DataBind();
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
        protected void SysDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
         
        }

        protected void btnSelSystem_Click(object sender, EventArgs e)
        {
            if(SysDDL.SelectedIndex > 0)
            {
                TextBox txtProduct = (TextBox)Parent.FindControl("SelectProduct");
                Session["SelectProductID"] = SysDDL.SelectedIndex;
                Session["SelectProduct"] = SysDDL.SelectedItem;
                AddStock p = Page as AddStock;
                if (p != null)
                { p.PopulateSearchProductText(); }
                
            }
        }
    }
}