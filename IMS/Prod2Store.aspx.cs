using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.Services;
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

                SqlCommand command = new SqlCommand("Sp_DeleteProduct_StoreMapping", connection);
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
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            String Text = txtSearch.Text + '%';
            Session["Text"] = Text;
            ProductsPopupGrid.StoreAssociation = false;
            ProductsPopupGrid.SelectAll = true;
            ProductsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            { 
                if (e.CommandName == "UpdateStock")
                {
                    int ProductID, StoreID = 0;
                    float SP, CP;
                    Label lblProductID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("prodID");
                    int.TryParse(lblProductID.Text.ToString(), out ProductID);
                    int.TryParse(Session["StoreID"].ToString(), out StoreID);
                    TextBox txtUnitCost = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtUnitCost");
                    TextBox txtSP = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtSP");
                    float.TryParse(txtUnitCost.Text.ToString(), out CP);
                    float.TryParse(txtSP.Text.ToString(), out SP);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand command = new SqlCommand("Sp_UpdateProductStoreMappingPrices", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_StoreID", StoreID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductID);
                    command.Parameters.AddWithValue("@p_SP", SP);
                    command.Parameters.AddWithValue("@p_UnitCost", CP);
                    command.ExecuteNonQuery();
                    //BindGrid();
                }
                if (e.CommandName == "Cancel")
                 {

                 }
               
            }
            catch
            {

            }
            finally
            {
                connection.Close();
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
            }
            
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {  
            ProductsPopupGrid.SelectAll = true;
            ProductsPopupGrid.StoreAssociation = true;
            ProductsPopupGrid.PopulateforAssociation();
            mpeCongratsMessageDiv.Show();
            //int StoreId = int.Parse(ddlStoreVendors.SelectedValue.ToString());
            //if (StoreId > 0)
            //{
            //    MultipleVendorsSelectPopup.SelectAll = true;
            //    MultipleVendorsSelectPopup.Storeid = StoreId;
            //    MultipleVendorsSelectPopup.BindAssociatedVendorGrid(StoreId);

            //    mpeCongratsMessageDiv.Show();
            //}

        }

        

         
    }
}