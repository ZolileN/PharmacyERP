using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class Prod2Store : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        
        public static bool FirstOrder;
        string storeName = "";
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["StoreName"] != null)
                {
                    lblStore.Text = Session["StoreName"].ToString();
                    storeName = Session["StoreName"].ToString();
                    lblStoreId.Text = Session["StoreID"].ToString();
                }
              
                BindGrid();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("StoreName");
            Session.Remove("StoreID");
            Response.Redirect("ProductStoreSelect.aspx",false);
        }

        private void BindGrid()
        {
            System.Data.DataTable dt = new System.Data.DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                if (connection.State == ConnectionState.Closed) 
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("Sp_GetProductStoreMapping", connection);
                command.CommandType = CommandType.StoredProcedure;
                int StoreNumber = 0;


                int.TryParse(Session["StoreID"].ToString(), out StoreNumber);
               
               command.Parameters.AddWithValue("@p_StoreID", StoreNumber);
               


                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                StockDisplayGrid.DataSource = ds;
                StockDisplayGrid.DataBind();
                StockDisplayGrid.Visible = true;

            }
            catch (Exception ex)
            {
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

       
      
        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
           
            #region deleting Product 
            try
            {
                Label mappingID = (Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("mappingID");
                Label productID = (Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("prodID");
                
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("sp_GetOrderbyVendor", connection);
                command.CommandType = CommandType.StoredProcedure;
                int StoreNumber = 0;
                int mapID = 0;
                int prodID = 0;

                int.TryParse(Session["StoreID"].ToString(), out StoreNumber);
                int.TryParse(mappingID.Text, out mapID);
                int.TryParse(productID.Text, out prodID);
                command.Parameters.AddWithValue("@p_StoreID", StoreNumber);
                command.Parameters.AddWithValue("@p_MappingID", mapID);
                command.Parameters.AddWithValue("@p_productID", prodID);
               int x= command.ExecuteNonQuery();
               if (x == 1)
               {
                   WebMessageBoxUtil.Show("Successfully deleted");
               }
               else 
               {
                   WebMessageBoxUtil.Show("Mapping Cannot be deleted stock exists with this product");
               }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
                BindGrid();
            }
            #endregion
        }

       

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            String Text = txtSearch.Text + '%';
            Session["Text"] = Text;
            ProductsPopupGrid.SelectAll = true;
            ProductsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {

        }
    }
}