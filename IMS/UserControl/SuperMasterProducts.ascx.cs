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


namespace IMS.UserControl
{
    public partial class SuperMasterProducts : System.Web.UI.UserControl
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["ProductMasterSearch"]!=null && Session["ProductMasterSearch"].ToString() != null && Session["ProductMasterSearch"].ToString() != "")
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

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }
    }
}