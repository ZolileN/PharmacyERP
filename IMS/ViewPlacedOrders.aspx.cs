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
using log4net;
using IMS.Util;


namespace IMS
{
    
    public partial class ViewPlacedOrders : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    #region Getting Vendors
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        DataSet ds = new DataSet();
                        SqlCommand command = new SqlCommand("Sp_GetVendor", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        SqlDataAdapter dA = new SqlDataAdapter(command);
                        dA.Fill(ds);

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
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        throw ex;
                    }
                    finally
                    {
                        connection.Close();
                    }



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
        private void LoadParameterized(string par, int id) 
        {
            #region Display Orders
            try
            {
                  int idSys;
                  if (int.TryParse(Session["UserSys"].ToString(), out idSys))
                  {
                      if(connection.State == ConnectionState.Closed)
                      {
                          connection.Open();
                      }
                      SqlCommand command = new SqlCommand("sp_GetPendingOrders_ByVendorID", connection);
                      command.CommandType = CommandType.StoredProcedure;
                      command.Parameters.AddWithValue("@p_storedAt", idSys);
                      switch (id)
                      {
                          case 1:
                              if (String.IsNullOrWhiteSpace(par))
                              {
                                  command.Parameters.AddWithValue("@p_VendID", DBNull.Value);
                              }
                              else
                              {
                                  command.Parameters.AddWithValue("@p_VendID", par);
                              }
                              command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                              command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                              command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);
                              break;
                          case 2:
                              if (OrderStatus.SelectedIndex <= 0)
                              {
                                  command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);
                              }
                              else
                              {
                                  command.Parameters.AddWithValue("@p_OrderStatus", OrderStatus.SelectedValue.ToString());
                              }
                              command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                              command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                              command.Parameters.AddWithValue("@p_VendID", DBNull.Value);
                              break;
                          case 3:
                              if (String.IsNullOrWhiteSpace(par))
                              {
                                  command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                              }
                              else
                              {
                                  command.Parameters.AddWithValue("@p_OrderDate", Convert.ToDateTime(par));
                              }
                              command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                              command.Parameters.AddWithValue("@p_VendID", DBNull.Value);
                              command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);
                              break;
                          case 4:
                              if (String.IsNullOrWhiteSpace(par))
                              {
                                  command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                              }
                              else
                              {
                                  command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(par));
                              }
                              command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                              command.Parameters.AddWithValue("@p_VendID", DBNull.Value);
                              command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);
                              break;
                      }

                      DataSet ds = new DataSet();

                      SqlDataAdapter sA = new SqlDataAdapter(command);
                      sA.Fill(ds);
                      //ProductSet = ds;
                      //StockDisplayGrid.DataSource = null;
                      StockDisplayGrid.DataSource = ds;
                      StockDisplayGrid.DataBind();
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
                connection.Close();
            }
            #endregion
        }
        
        public void LoadData(String VendorID)
        {
            #region Display Orders
            try
            {
                int idSys;
                if (int.TryParse(Session["UserSys"].ToString(), out idSys))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_GetPendingOrders_ByVendorID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_storedAt", idSys);
                                        
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
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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
                int rowIndex=-1;
                if (Session["SelecetedRowIndex"] != null) 
                {
                    rowIndex = int.Parse(Session["SelecetedRowIndex"].ToString());
                    
                }
                if (e.CommandName.Equals("ReGen"))
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);
                    Session["Vendorname"] = StockAt.SelectedItem.Text;
                    Label OrderNo = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderNO");
                        //session is setting
                        Session["OrderNumber"] = OrderNo.Text.ToString();
                        Session["FromViewPlacedOrders"] = "true";
                        Response.Redirect("ManualPurchase.aspx", false);
                       // Response.Redirect("OrderPurchaseManual.aspx", false);
                    
                }
                else if (e.CommandName.Equals("UpdateRec")) 
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label OrderNo = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderNO");
                    Session["RequestedNO"] = OrderNo.Text.ToString();
                    Session["RequestDesRole"] = "Vendor";
                    Response.Redirect("AcceptPurchaseOrder.aspx");
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
                    SqlCommand command = new SqlCommand("sp_DeleteOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);

                    command.ExecuteNonQuery();
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
            try
            {
                StockDisplayGrid.EditIndex = e.NewEditIndex;
                //DataTable filterSet = StockDisplayGrid.DataSource as DataTable;
                //DataView dataView = filterSet.DefaultView;
                //Label OrderNo = (Label)StockDisplayGrid.Rows[e.NewEditIndex].FindControl("OrderNO");
                ////session is setting
                //Session["SelecetedRowIndex"] = e.NewEditIndex;
                //dataView.RowFilter = "OrderID = " + OrderNo.Text.ToString();
                //Session["SelecetedRowIndex"] = e.NewEditIndex;
                //StockDisplayGrid.EditIndex = 0;
                //LoadParameterized(OrderNo.Text.ToString(),4);
                
                //StockDisplayGrid.DataSource = dataView;
                //StockDisplayGrid.DataBind();
                //StockDisplayGrid.EditIndex = 0;
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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

        protected Boolean IsStatusComplete(String status)
        {
            if (status.Equals("Complete") || status.Equals("Partial"))
            {
                return true;
            }
            else
                return false;
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            if (Convert.ToInt32(Session["UserSys"]).Equals(1))
            {
                Response.Redirect("WarehouseMain.aspx", false);
            }
            else
            {
                Response.Redirect("StoreMain.aspx", false);
            }
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
           // StockAt.Visible = false;
             StockAt.SelectedIndex = 0;
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
                SqlCommand command = new SqlCommand("sp_GetVendor_byNameParam", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_VendName", Text);
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
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