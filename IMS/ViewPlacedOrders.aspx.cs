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
    public partial class ViewPlacedOrders : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Getting Vendors
                //try
                //{
                //    connection.Open();
                //    DataSet ds = new DataSet();
                //    SqlCommand command = new SqlCommand("Sp_GetVendor", connection);
                //    command.CommandType = CommandType.StoredProcedure;
                //    SqlDataAdapter dA = new SqlDataAdapter(command);
                //    dA.Fill(ds);

                //    StockAt.DataSource = ds.Tables[0];
                //    StockAt.DataTextField = "SupName";
                //    StockAt.DataValueField = "SuppID";
                //    StockAt.DataBind();
                //    if (StockAt != null)
                //    {
                //        StockAt.Items.Insert(0, "Select Vendor");
                //        StockAt.SelectedIndex = 0;
                //    }
                //}
                //catch (Exception ex)
                //{

                //}
                //finally
                //{
                //    connection.Close();
                //}
                #endregion

                #region Populating Order Status DropDown
                OrderStatus.Items.Add("Pending");
                OrderStatus.Items.Add("Partial");
                OrderStatus.Items.Add("Complete");
                if (OrderStatus != null)
                {
                    OrderStatus.Items.Insert(0, "Select Order Status");
                    OrderStatus.SelectedIndex = 0;
                }

                #endregion


                if (StockAt.SelectedIndex <= 0)
                {
                    LoadData("");
                }
                else
                {
                    LoadData(StockAt.SelectedValue);
                }

            }
        }
        public void LoadData(String VendorID)
        {
            #region Display Orders
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetPendingOrders_ByVendorID", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (String.IsNullOrWhiteSpace(VendorID) || StockAt.SelectedIndex <= 0)
                {
                    command.Parameters.AddWithValue("@p_VendID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_VendID", VendorID);
                }

                if (OrderStatus.SelectedIndex <= 0)
                {
                    command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderStatus", OrderStatus.SelectedValue.ToString());
                }

                if (String.IsNullOrWhiteSpace(DateTextBox.Text.ToString()))
                {
                    command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderDate", Convert.ToDateTime(DateTextBox.Text.ToString()));
                }


                if (String.IsNullOrWhiteSpace(txtOrderNO.Text.ToString()))
                {
                    command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(txtOrderNO.Text.ToString()));
                }

                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;
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
            #endregion
        }
        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex == -1)
            {
                LoadData(null);
            }
            else
            {
                LoadData(StockAt.SelectedValue);
                Session["SelectedVendor"] = StockAt.SelectedValue;
                Session["SelectedSysVendor"] = StockAt.SelectedItem;
            }
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex <= 0)
            {
                LoadData("");
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            if (StockAt.SelectedIndex <= 0)
            {
                LoadData("");
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            if (StockAt.SelectedIndex <= 0)
            {
                LoadData("");
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Edit"))
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label OrderNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderNO");
                    //session is setting
                    Session["OrderNumber"] = OrderNo.Text.ToString();
                    Session["FromViewPlacedOrders"] = "true";
                    Response.Redirect("OrderPurchaseManual.aspx",false);
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label OrderNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderNO");

                    int orderID = int.Parse(OrderNo.Text.ToString());
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_DeleteOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);

                    command.ExecuteNonQuery();
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
                if (StockAt.SelectedIndex <= 0)
                {
                    LoadData("");
                }
                else
                {
                    LoadData(StockAt.SelectedValue);
                }
            }
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            if (StockAt.SelectedIndex <= 0)
            {
                LoadData("");
            }
            else
            {
                LoadData(StockAt.SelectedValue);
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            Response.Redirect("ManageOrders.aspx");
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Status = (Label)e.Row.FindControl("OrderStatus");
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                if (Status.Text.Equals("Complete") || Status.Text.Equals("Partial"))
                {
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }
            }
        }

        protected void OrderStatus_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData(StockAt.SelectedValue.ToString());
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            LoadData(StockAt.SelectedValue.ToString());
        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            SelectProduct.Text = "";
            StockAt.Visible = false;
            StockAt.SelectedIndex = -1;
            txtOrderNO.Text = "";
            DateTextBox.Text = "";
            OrderStatus.SelectedIndex = 0;
            LoadData("");
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (SelectProduct.Text.Length >= 3)
            {
                PopulateDropDown(SelectProduct.Text);
                StockAt.Visible = true;
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("Select * From tblVendor Where tblVendor.SupName LIKE '" + Text + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (StockAt.DataSource != null)
                {
                    StockAt.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                StockAt.DataSource = ds.Tables[0];
                StockAt.DataTextField = "SupName";
                StockAt.DataValueField = "SuppID";
                StockAt.DataBind();
                if (StockAt != null)
                {
                    StockAt.Items.Insert(0, "Select Vendor");
                    StockAt.SelectedIndex = 0;
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

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}