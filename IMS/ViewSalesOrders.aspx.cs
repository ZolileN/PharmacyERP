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
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class ViewSalesOrders : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    Session["ViewSalesOrders"] = false;

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
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            // Void Page_Load(System.Object, System.EventArgs)
            // Handle specific exception.
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            // Clear the error from the server.
            Server.ClearError();
        }
        protected void Page_UnLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                Session["ViewSalesOrders"] = true;
            }
        }
        public void LoadData(String VendorID)
        {
            #region Display Orders
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetPendingSO_byID", connection);
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
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
                    Label SystemID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("SystemID");
                    Label Invoice = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Invoice");
                     Label OrderTo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderTo");
                     Label SalesManID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("SalesManID");
                    //session is setting
                    Session["OrderNumberSO"] = OrderNo.Text.ToString();
                    Session["SelectedIndexValue"] = OrderTo.Text.ToString();
                    Session["OrderSalesDetail"] = true;
                    Session["SystemID"] = SystemID.Text;
                    Session["ExistingOrder"] = true;
                    Session["Invoice"] = Invoice.Text;
                    Session["SalesManID"] = SalesManID.Text.ToString();
                    Response.Redirect("OrderSalesManual.aspx", false);
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label OrderNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderNO");


                    int orderID = int.Parse(OrderNo.Text.ToString());
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    #region Order Deletion


                    SqlCommand command = new SqlCommand("sp_GetOrderDetailRecieve", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        int StockID = int.Parse(ds.Tables[0].Rows[i]["StockID"].ToString());
                        int quantity = int.Parse(ds.Tables[0].Rows[i]["Quantity"].ToString());
                        SqlCommand command2 = new SqlCommand();
                        command2 = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.AddWithValue("@p_StockID", StockID);
                        command2.Parameters.AddWithValue("@p_quantity", quantity);
                        command2.Parameters.AddWithValue("@p_Action", "Add");
                        command2.ExecuteNonQuery();
                    }

                    command = new SqlCommand("sp_DeleteSO", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);
                    command.ExecuteNonQuery();
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Saless order Successfully Deleted.')", true);
                   // WebMessageBoxUtil.Show("");
                    #endregion
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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
            Session["OrderNumberSO"] = null;
          //  Session["ViewSalesOrders"] = "false";
            Response.Redirect("WarehouseMain.aspx");
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
                    if (btnDelete != null)
                    {
                        btnDelete.Enabled = false;
                    }
                }
                else
                {
                    if (btnEdit != null)
                    {
                        btnEdit.Enabled = true;
                    }
                    if (btnDelete != null)
                    {
                        btnDelete.Enabled = true;
                    }
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
            Session["OrderNumberSO"] = null;
           // Session["FromViewPlacedOrders"] = "false";
           // Session["ViewSalesOrders"] = "false";
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
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                //Text = Text + "%";
                //SqlCommand command = new SqlCommand("Select * From tbl_System Where tbl_System.SystemName LIKE '" + Text + "'", connection);
                DataSet ds = new DataSet();
                SqlCommand command = new SqlCommand("Sp_GetAllCategories", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (Text != null)
                {
                    command.Parameters.AddWithValue("@p_storeName ", Text);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_storeName ", DBNull.Value);
                }
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (StockAt.DataSource != null)
                {
                    StockAt.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                StockAt.DataSource = ds.Tables[0];
                StockAt.DataTextField = "SystemName";
                StockAt.DataValueField = "SystemID";
                StockAt.DataBind();
                if (StockAt != null)
                {
                    StockAt.Items.Insert(0, "Select Store");
                    StockAt.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            #endregion
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }
    }
}