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

namespace IMS
{
    public partial class StoreRequests : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                FirstOrder = false;
                systemSet = new DataSet();
                ProductSet = new DataSet();
                LoadData();
                
            }
        }

        private void LoadData()
        {
            #region Getting Systems
            try
            {
                connection.Open();
                DataSet ds = new DataSet();
                SqlCommand command = new SqlCommand("sp_GetSystemsForStore", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_UserSys", Convert.ToInt32(Session["UserSys"].ToString()));
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                RequestTo.DataSource = ds.Tables[0];
                RequestTo.DataTextField = "SystemName";
                RequestTo.DataValueField = "SystemID";
                RequestTo.DataBind();
                if (RequestTo != null)
                {
                    RequestTo.Items.Insert(0, "Select System");
                    RequestTo.SelectedIndex = 0;
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            #endregion
        }
        protected void RequestTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            RequestTo.Enabled = false;
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
          
            //InvNO.Enabled = false;
            btnAccept.Visible = true;
            btnDecline.Visible = true;
            if (FirstOrder.Equals(false))
            {
                #region Creating Order
                //Session["UserSys"] = 2;
                int pRequestFrom = 0;
                int pRequestTo = 0;
                String OrderMode = "";
                int OrderType = 2;

                if (RequestTo.SelectedItem.ToString().Contains("store")) // neeed to check it, because name doesn't always contains Store
                {
                    OrderMode = "Store";
                }
                else
                {
                    OrderMode = "Warehouse";
                }

                String Invoice = "";
                String Vendor = "";

                
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_CreateOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (int.TryParse(RequestTo.SelectedValue.ToString(), out pRequestTo))
                    {
                        command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                    }
                    if (int.TryParse(Session["UserSys"].ToString(), out pRequestFrom))
                    {
                        command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                    }
                    int userID = Convert.ToInt32(Session["UserID"].ToString());

                    command.Parameters.AddWithValue("@p_userID", userID);
                    command.Parameters.AddWithValue("@p_OrderType", OrderType);
                    command.Parameters.AddWithValue("@p_Invoice", Invoice);
                    command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                    command.Parameters.AddWithValue("@p_Vendor", Vendor);
                    command.Parameters.AddWithValue("@p_orderStatus", "Initiated");
                    DataTable dt = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(command);
                    dA.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        Session["OrderNumber"] = dt.Rows[0][0].ToString();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                #endregion

                #region Linking to Order Detail table

                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    int OrderNumber, ProductNumber, Quantity = 0;

                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                    }
                    if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNumber))
                    {
                        command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                    }
                    if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                    {
                        command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                    }

                    if (RequestTo.SelectedItem.ToString().Contains("store")) // neeed to check it, because name doesn't always contains Store
                    {
                        command.Parameters.AddWithValue("@p_status", "Initiated");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Store");
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_status", "Initiated");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Warehouse");
                    }
                    command.ExecuteNonQuery();
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                }
                #endregion
                FirstOrder = true;
            }
            else
            {
                #region Product Existing in the Current Order
                DataSet ds = new DataSet();
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_GetStoreRequest_byOrderID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    int OrderNumber = 0;


                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                    }
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
                
                int ProductNO = 0;
                bool ProductPresent =false;
                if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNO))
                {
                }
                
                for(int i=0;i<ds.Tables[0].Rows.Count;i++)
                {
                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]).Equals(ProductNO))
                    {
                        ProductPresent = true;
                        break;
                    }
                }
                #endregion

                if(ProductPresent.Equals(false))
                {
                    #region Linking to Order Detail table

                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        int OrderNumber, ProductNumber, Quantity = 0;

                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        }
                        if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNumber))
                        {
                            command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                        }
                        if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                        {
                            command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                        }

                        if (RequestTo.SelectedItem.ToString().Contains("store")) // neeed to check it, because name doesn't always contains Store
                        {
                            command.Parameters.AddWithValue("@p_status", "Initiated");
                            command.Parameters.AddWithValue("@p_comments", "Generated to Store");
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_status", "Initiated");
                            command.Parameters.AddWithValue("@p_comments", "Generated to Warehouse");
                        }
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                    #endregion

                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                }
                else
                {
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record can be inserted, because it is already present')", true);
                }
            }
            
            #region Display Products
            BindGrid();
            #endregion
        }

        private void BindGrid() 
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetStoreRequest_byOrderID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                DataSet ds = new DataSet();

                if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                StockDisplayGrid.DataSource = null;
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
        }
        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            //should Delete all the rows, if final accept is not pressed.
            Response.Redirect("StoreRequestsMain.aspx");
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            //need to check this,
            int ErrorNo = 0;
            try
            {
                connection.Open();
                if (e.CommandName == "UpdateStock")
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    String OrderDetailNo = Convert.ToString((Label)StockDisplayGrid.Rows[RowIndex].FindControl("OrderDetailNo"));
                    String OrderQuantity = Convert.ToString((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtQuantity"));
                    SqlCommand command = new SqlCommand("sp_UpdateOrderDetailsQuantity", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", Convert.ToInt32(OrderDetailNo));
                    command.Parameters.AddWithValue("@p_Qauntity", Convert.ToInt32(OrderDetailNo));
                    ErrorNo = command.ExecuteNonQuery();
                }
                else if (e.CommandName == "Delete")
                {
                    GridViewRow gvr = (GridViewRow)(((Button)e.CommandSource).NamingContainer);
                    int RowIndex = gvr.RowIndex;
                    String OrderDetailNo = Convert.ToString((Label)StockDisplayGrid.Rows[RowIndex].FindControl("OrderDetailNo"));
                    String OrderQuantity = Convert.ToString((TextBox)StockDisplayGrid.Rows[RowIndex].FindControl("txtQuantity"));
                    SqlCommand command = new SqlCommand("sp_DeleteOrderDetailsbyID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", Convert.ToInt32(OrderDetailNo));
                    ErrorNo = command.ExecuteNonQuery();
                }
            }
            catch(Exception ex)
            {
               
            }
            finally
            {
                connection.Close();
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                Label Label1 = (Label)e.Row.FindControl("Label1");

                Label dosage = (Label)e.Row.FindControl("dosage2");
                Label Label2 = (Label)e.Row.FindControl("Label2");

                Label packSize = (Label)e.Row.FindControl("packSize2");
                Label Label3 = (Label)e.Row.FindControl("Label3");

                if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                {
                    ProductStrength.Visible = false;
                    Label1.Visible = false;
                }
                else
                {
                    ProductStrength.Visible = true;
                    Label1.Visible = true;
                }

                if (String.IsNullOrWhiteSpace(dosage.Text))
                {
                    dosage.Visible = false;
                    Label2.Visible = false;
                }
                else
                {
                    dosage.Visible = true;
                    Label2.Visible = true;
                }

                if (String.IsNullOrWhiteSpace(packSize.Text))
                {
                    packSize.Visible = false;
                    Label3.Visible = false;
                }
                else
                {
                    packSize.Visible = true;
                    Label3.Visible = true;
                }
            }
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //should clean everything if final accept is not pressed
            //InvNO.Text = "";
            RequestTo.Enabled = true;
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
            SelectQuantity.Text = "";
            SelectProduct.SelectedIndex = -1;
            RequestTo.SelectedIndex = -1;
            btnAccept.Visible = false;
            btnDecline.Visible = false;
        }

        protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                int UserSys = 0;
                SqlCommand command = new SqlCommand();
                if (int.TryParse(Session["UserSys"].ToString(), out UserSys))
                {
                    command = new SqlCommand("sp_GetSystemName_byID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_UserSysID", UserSys);
                }

                DataTable dt = new DataTable();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(dt);

                Session["S_RequestFrom"] = dt.Rows[0][0].ToString();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            Session["S_RequestInvoice"] = "";
            Session["S_RequestTo"] = RequestTo.SelectedItem.ToString();

            Response.Redirect("StoreRequestsView.aspx");

        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            //should clean everything if final accept is not pressed
          //  InvNO.Text = "";
            RequestTo.Enabled = true;
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
            SelectQuantity.Text = "";
            SelectProduct.SelectedIndex = -1;
            RequestTo.SelectedIndex = -1;
            btnAccept.Visible = false;
            btnDecline.Visible = false;
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (ProductList.Text.Length >= 3)
            {
                PopulateDropDown(ProductList.Text);
                SelectProduct.Visible = true;
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                //Text = Text + "%";
                SqlCommand command = new SqlCommand("Sp_GetProductByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_prodName", Text);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (SelectProduct.DataSource != null)
                {
                    SelectProduct.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                //ds.Tables[0].Columns.Add("ProductInfo", typeof(string), "Product_Name+ ' '+itemStrength+' '+itemPackSize+' '+itemForm");
                SelectProduct.DataSource = ds.Tables[0];
                SelectProduct.DataTextField = "Description";
                SelectProduct.DataValueField = "ProductID";
                SelectProduct.DataBind();
                if (ProductList != null)
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