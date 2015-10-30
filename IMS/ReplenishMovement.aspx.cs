using IMS.Util;
using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ReplenishMovement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if(!IsPostBack)
            {
                try
                {
                    if ((Session["FromSalesDate"] != null && Session["FromSalesDate"].ToString() != "") &&
                        (Session["ToSalesDate"] != null && Session["ToSalesDate"].ToString() != "") &&
                        (Session["ReplenishDays"] != null && Session["ReplenishDays"].ToString() != ""))
                    {
                        DateTime FromDate = Convert.ToDateTime(Session["FromSalesDate"].ToString());
                        DateTime ToDate = Convert.ToDateTime(Session["ToSalesDate"].ToString());

                        TimeSpan diff = ToDate - FromDate;
                        double TotalDays = diff.TotalDays;
                        int Days = Convert.ToInt32(TotalDays);

                        int ReplenishDays = 0;
                        int.TryParse((Session["ReplenishDays"].ToString()), out ReplenishDays);

                         //Session["DataTableView"] = null;
                         //Session["DataTableUpdate"] = null;

                        LoadData_SalesDate(Days, ReplenishDays, FromDate,ToDate);
                        ViewState["VendorID"] = -1;
                        DisplayMainGrid((DataTable)Session["DataTableView"]);
                    }
                    else
                    {
                        LoadData();
                        ViewState["VendorID"] = -1;
                        DisplayMainGrid((DataTable)Session["DataTableView"]);
                    }

                    if (Session["parameter"] != null && Session["parameter"].ToString().Equals("Calculation"))
                    {
                        lblReplenishHeader.Text = "Replenish ( Calculation )";
                    }
                    else
                    {
                        lblReplenishHeader.Text = "Replenish ( Movement )";
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
            }
            expHandler.CheckForErrorMessage(Session);
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
        public void DisplayMainGrid(DataTable dt)
        {
            try
            {
                DataTable displayTable = new DataTable();
                displayTable.Clear();
                displayTable.Columns.Add("VendorID", typeof(int));
                displayTable.Columns.Add("VendorName", typeof(String));
                displayTable.Columns.Add("isWareHouse", typeof(int));

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    int VendorID = Convert.ToInt32(dt.Rows[i]["VendorID"].ToString());
                    String VendorName = dt.Rows[i]["VendorName"].ToString();
                    String isWareHouse = dt.Rows[i]["isWareHouse"].ToString();
                    displayTable.Rows.Add(VendorID, VendorName, isWareHouse);
                    displayTable.AcceptChanges();
                }

                DataView dv = displayTable.DefaultView;
                displayTable = null;
                displayTable = dv.ToTable(true, "VendorID", "VendorName", "isWareHouse");
                gvVendorNames.DataSource = null;
                gvVendorNames.DataSource = displayTable;
                gvVendorNames.DataBind();
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
        }

        public void LoadData()
        {
            int SystemID = 0;
            int.TryParse(Session["UserSys"].ToString(), out SystemID);

            int VendorID = 0;
            int.TryParse(Session["ReplenishVendorID"].ToString(), out VendorID);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_ReplenishProductSet_ByDate", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_SystemID", SystemID);
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(ds);

                DataTable dt = new DataTable();
                DataView dv = new DataView();
                if(VendorID >0)
                {
                    dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "VendorID = '"+VendorID+"'";
                    dt = dv.ToTable();
                }
                else
                {
                    dt = ds.Tables[0];
                }
                Session["DataTableView"] = dt;
                Session["DataTableUpdate"] = ds.Tables[1];

            }
            catch (Exception ex)
            {
                //ex Message should be displayed
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        public void LoadData_SalesDate(int DateDifference, int ReplenishDays, DateTime FromDate, DateTime ToDate)
        {
            int SystemID = 0;
            int.TryParse(Session["UserSys"].ToString(), out SystemID);

            int VendorID = 0;
            int.TryParse(Session["ReplenishVendorID"].ToString(), out VendorID);

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                //sp_ReplenishProducts_Calculation
                SqlCommand cmd = new SqlCommand("Sp_ReplenishProducts_Calculation_WHFlag", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_SystemID", SystemID);
                cmd.Parameters.AddWithValue("@p_DaysDifference", DateDifference);
                cmd.Parameters.AddWithValue("@p_ReplenishDays", ReplenishDays);
                cmd.Parameters.AddWithValue("@p_FromDate", FromDate);
                cmd.Parameters.AddWithValue("@p_ToDate", ToDate);
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(ds);

                DataTable dt = new DataTable();
                DataView dv = new DataView();

                // ------ Vendor ID should be checked and manipulated properly ------//
                if (VendorID > 0)
                {
                    dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "VendorID = '" + VendorID + "'";
                    dt = dv.ToTable();
                }
                else 
                {
                    dt = ds.Tables[0];
                }
                Session["DataTableView"] = dt;
                Session["DataTableUpdate"] = ds.Tables[1];

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        protected void gvVendorNames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int VendorID = 0;
                    Label hfVenID = (Label)e.Row.FindControl("hdnVendorID");
                    Label hdnVendorName = (Label)e.Row.FindControl("hdnVendorName");
                    int.TryParse(hfVenID.Text.ToString(), out VendorID);

                    String SelectVendorName = hdnVendorName.Text.ToString();

                    //if (ViewState["VendorID"].Equals(VendorID))
                    //{
                        
                    //}
                    //else
                    //{

                        GridView gvProductList = (GridView)e.Row.FindControl("gvVendorProducts");

                        DataTable dt = (DataTable)Session["DataTableView"];
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "VendorID = '" + VendorID + "' AND VendorName = '" + SelectVendorName + "'";

                        dt = null;
                        dt = dv.ToTable();

                        gvProductList.DataSource = dt;
                        gvProductList.DataBind();

                        //ViewState["VendorID"] = VendorID;
                    //}

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
        }
        protected void gvVendorNames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvVendorNames_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendorNames.PageIndex = e.NewPageIndex;
            DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        protected void gvVendorProducts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvVendorProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //GridView gvProductList = (GridView)gvVendorNames.FindControl("gvVendorProducts");

                    Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                    DropDownList ddlVendorList = (DropDownList)e.Row.FindControl("ddlPreviousVendors");

                    int ProductID = 0;
                    int.TryParse(lblProductID.Text.ToString(), out ProductID);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand cmd = new SqlCommand("sp_GetProductVendors_Replenishment", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productID", ProductID);

                    DataSet ds = new DataSet();
                    SqlDataAdapter dA = new SqlDataAdapter(cmd);
                    dA.Fill(ds);
                    ddlVendorList.DataSource = ds.Tables[0];
                    ddlVendorList.DataValueField = "SuppID";
                    ddlVendorList.DataTextField = "SupName";
                    ddlVendorList.DataBind();
                    if (ddlVendorList != null)
                    {
                        ddlVendorList.Items.Insert(0, "Select Vendor");
                        ddlVendorList.SelectedIndex = 0;
                    }

                }
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        protected void gvVendorProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "NewVendor")
                {
                    // String Text = txtVendor.Text + '%';
                    VendorsPopupGrid.SelectSearch = true;
                    VendorsPopupGrid.RepProdID = e.CommandArgument.ToString().Split(',')[0];
                    VendorsPopupGrid.RepVenID = e.CommandArgument.ToString().Split(',')[1];
                    VendorsPopupGrid.PopulateWithSearch();
                    mpeCongratsMessageDiv.Show();

                }

                else if (e.CommandName == "UpdateStock")
                {
                    Label lblProductID = (Label)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("lblProductID");
                    Label lblVendorID = (Label)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("lblVendorID");
                    int ProductID = 0;
                    int MainVendorID = 0;
                    int UpdatedQuantity = 0;
                    int.TryParse(lblProductID.Text.ToString(), out ProductID);
                    int.TryParse(lblVendorID.Text.ToString(), out MainVendorID);

                    TextBox txtQuantity = (TextBox)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("txtQuantity");

                    int.TryParse(txtQuantity.Text.ToString(), out UpdatedQuantity);

                    DataTable dtChanged = (DataTable)Session["DataTableView"];
                    if (UpdatedQuantity > 0)
                    {
                        for (int i = 0; i < dtChanged.Rows.Count; i++)
                        {
                            int Product = 0;
                            int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                            int Vendor = 0;
                            int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                            if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                            {
                                dtChanged.Rows[i]["QtySold"] = UpdatedQuantity;
                                dtChanged.AcceptChanges();
                            }
                        }
                    }

                    Session["DataTableView"] = dtChanged;

                    ((GridView)sender).EditIndex = -1;
                    DisplayMainGrid((DataTable)Session["DataTableView"]);
                }

                else if (e.CommandName == "Delete")
                {
                    String lblProductID = e.CommandArgument.ToString().Split(',')[0];
                    String lblVendorID = e.CommandArgument.ToString().Split(',')[1];
                    int ProductID = 0;
                    int MainVendorID = 0;
                    ArrayList list = new ArrayList();

                    int.TryParse(lblProductID, out ProductID);
                    int.TryParse(lblVendorID, out MainVendorID);

                    DataTable dtChanged = (DataTable)Session["DataTableView"];

                    for (int i = 0; i < dtChanged.Rows.Count; i++)
                    {
                        int Product = 0;
                        int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                        int Vendor = 0;
                        int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                        if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                        {
                            list.Add(i);
                        }
                    }

                    foreach (int i in list)
                    {
                        dtChanged.Rows[i].Delete();
                        dtChanged.AcceptChanges();
                    }

                    Session["DataTableView"] = dtChanged;
                    DisplayMainGrid((DataTable)Session["DataTableView"]);
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
        }

        protected void gvVendorProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {

            
            GridView grid = ((GridView)(sender));

            

            grid.EditIndex = e.NewEditIndex;
           
            
            //DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        protected void gvVendorProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvVendorProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ((GridView)sender).EditIndex = -1;
            DisplayMainGrid((DataTable)Session["DataTableView"]);

        }

        protected void gvVendorProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReplenishMain.aspx");
        }

        protected void ddlPreviousVendors_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                GridViewRow gvr = (GridViewRow)(((Control)sender).NamingContainer);
                Label lblProduct = (Label)((GridView)((DropDownList)sender).Parent.Parent.Parent.Parent).Rows[gvr.RowIndex].FindControl("lblProductID");
                Label lblVendor = (Label)((GridView)((DropDownList)sender).Parent.Parent.Parent.Parent).Rows[gvr.RowIndex].FindControl("lblVendorID");

                int ProductID = 0;
                int MainVendorID = 0;

                int.TryParse(lblProduct.Text.ToString(), out ProductID);
                int.TryParse(lblVendor.Text.ToString(), out MainVendorID);

                int SelectedVendorID = 0;
                int.TryParse(((DropDownList)sender).SelectedValue.ToString(), out SelectedVendorID);

                String SelectedVendorName = ((DropDownList)sender).SelectedItem.ToString();

                DataTable dtChanged = (DataTable)Session["DataTableView"];

                for (int i = 0; i < dtChanged.Rows.Count; i++)
                {
                    int Product = 0;
                    int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                    int Vendor = 0;
                    int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                    if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                    {
                        dtChanged.Rows[i]["VendorID"] = SelectedVendorID;
                        dtChanged.Rows[i]["VendorName"] = SelectedVendorName;
                        dtChanged.AcceptChanges();
                    }
                }

                Session["DataTableView"] = dtChanged;
                DisplayMainGrid((DataTable)Session["DataTableView"]);
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
        }


        protected void gvVendorNames_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            bool NextPage = false;

            int OrderNumber = 0;
            if (e.CommandName == "CreatePO")
            {
                try
                {
                    String Vendor = e.CommandArgument.ToString();
                    int VendorID = 0;
                   // int.TryParse(Vendor, out VendorID);

                    

                    int.TryParse(((Label)gvVendorNames.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("hdnVendorID")).Text, out VendorID);

                    if (VendorID > 0)
                    {
                        int SystemID = 0;
                        int.TryParse(Session["UserSys"].ToString(), out SystemID);

                        
                        int isWareHouse = 0;
                        int.TryParse(((Label)gvVendorNames.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("isWareHouse")).Text, out isWareHouse);

                        GridView gridview = (GridView)gvVendorNames.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("gvVendorProducts");

                        String SelectVendorName = ((Label)gvVendorNames.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("hdnVendorName")).Text;

                        DataView dv = ((DataTable)Session["DataTableView"]).DefaultView;
                     //   dv.RowFilter = "VendorID = '" + VendorID + "'";
                        dv.RowFilter = "VendorID = '" + VendorID + "' AND VendorName = '" + SelectVendorName + "'";

                        

                        DataTable dtChanged = dv.ToTable();
                        dtChanged.Columns.Add("StoreID", typeof(int));

                        for (int i = 0; i < dtChanged.Rows.Count; i++)
                        {
                            dtChanged.Rows[i]["StoreID"] = SystemID;
                        }
                        SqlCommand command = new SqlCommand();

                        if (VendorID == 1 && isWareHouse==1)
                        {
                            #region  Creating Transfer Order

                            int userID = Convert.ToInt32(Session["UserID"].ToString());

                            int p_TransferBy = 0;
                            int p_TransferTo = 0;
                            try
                            {
                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.Open();
                                }

                                command = new SqlCommand("sp_CreateTransferOrder", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                //Select Vendor
                                if (int.TryParse(dtChanged.Rows[0]["VendorID"].ToString(), out p_TransferTo))
                                {
                                    command.Parameters.AddWithValue("@p_TransferTo", p_TransferTo);
                                }
                                //Select From
                                if (int.TryParse(dtChanged.Rows[0]["StoreID"].ToString(), out p_TransferBy))
                                {
                                    command.Parameters.AddWithValue("@p_TransferBy", p_TransferBy);
                                }

                                command.Parameters.AddWithValue("@p_TransferStatus", "Initiated");
                                command.Parameters.AddWithValue("@p_TransferByUserID", userID);
                                DataTable dt = new DataTable();
                                SqlDataAdapter dA = new SqlDataAdapter(command);
                                dA.Fill(dt);
                                if (dt.Rows.Count != 0)
                                {
                                    Session["TransferNo"] = dt.Rows[0][0].ToString();
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
                        else
                        {
                            #region Create Order

                            if (connection.State == ConnectionState.Closed) { connection.Open(); }
                            command = new SqlCommand("sp_CreateOrder", connection);
                            command.CommandType = CommandType.StoredProcedure;

                            int pRequestTo = 0;
                            int pRequestFrom = 0;
                            String VendorCheck = "True";
                            if (int.TryParse(dtChanged.Rows[0]["VendorID"].ToString(), out pRequestTo))
                            {
                                command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                            }

                            if (int.TryParse(dtChanged.Rows[0]["StoreID"].ToString(), out pRequestFrom))
                            {
                                command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                            }

                            int userID = Convert.ToInt32(Session["UserID"].ToString());

                            command.Parameters.AddWithValue("@p_userID", userID);

                            command.Parameters.AddWithValue("@p_OrderType", 3);
                            command.Parameters.AddWithValue("@p_Invoice", "");
                            command.Parameters.AddWithValue("@p_OrderMode", "Vendor");
                            command.Parameters.AddWithValue("@p_Vendor", VendorCheck);
                            command.Parameters.AddWithValue("@p_orderStatus", "Pending");
                            command.Parameters.AddWithValue("@p_isPOGen", true);
                            DataTable dt = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(command);
                            dA.Fill(dt);
                            if (dt.Rows.Count != 0)
                            {
                                int.TryParse(dt.Rows[0][0].ToString(), out OrderNumber);
                            }
                            #endregion
                        }
                            

                        for (int i = 0; i < dtChanged.Rows.Count; i++)
                        {
                            if (VendorID == 1 && isWareHouse==1)
                            {
                                #region Linking to Transfer Detail table

                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    command = new SqlCommand("sp_InsertIntoTransferOrderDetails", connection);
                                    command.CommandType = CommandType.StoredProcedure;


                                    int TransferNo, BonusOrdered, ProductNumber, Quantity;
                                    TransferNo = BonusOrdered = ProductNumber = Quantity = 0;

                                    if (int.TryParse(Session["TransferNo"].ToString(), out TransferNo))
                                    {
                                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                                    }
                                    command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(dtChanged.Rows[i]["ProductID"].ToString()));

                                    if (int.TryParse(dtChanged.Rows[i]["QtySold"].ToString(), out Quantity))
                                    {
                                        command.Parameters.AddWithValue("@p_RequestedQty", Quantity);
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@p_RequestedQty", DBNull.Value);
                                    }

                                    command.Parameters.AddWithValue("@p_TransferStatus", "Initiated");

                                    DataSet LinkResult = new DataSet();
                                    SqlDataAdapter sA = new SqlDataAdapter(command);
                                    sA.Fill(LinkResult);
                                    if (LinkResult != null && LinkResult.Tables[0] != null)
                                    {
                                        //Session["TransferDetailID"] = LinkResult.Tables[0].Rows[0][0];
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
                            else
                            {
                                #region Linking to Order Detail Table
                                command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                int ProductNumber = 0;
                                int BonusOrdered, Quantity;
                                BonusOrdered = Quantity = 0;

                                command.Parameters.AddWithValue("@p_OrderID", OrderNumber);

                                int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out ProductNumber);
                                command.Parameters.AddWithValue("@p_ProductID", ProductNumber);

                                if (int.TryParse(dtChanged.Rows[i]["QtySold"].ToString(), out Quantity))
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                                }

                                command.Parameters.AddWithValue("@p_OrderBonusQuantity", DBNull.Value);
                                command.Parameters.AddWithValue("@p_status", "Pending");
                                command.Parameters.AddWithValue("@p_comments", "Replenishment - Generated to Vendor");

                                command.ExecuteNonQuery();
                                #endregion
                            }
                        }

                        #region Updating Replenishment Columns in tablSales
                        DataTable dtUpdateSales = (DataTable)Session["DataTableUpdate"];

                        if (VendorID == 1 && isWareHouse==1)
                        {
                            OrderNumber = Convert.ToInt32(Session["TransferNo"].ToString());
                            command = new SqlCommand("sp_getTransferDetails_TransferID", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_TransferID", OrderNumber);
                        }
                        else
                        {
                            command = new SqlCommand("Sp_GetPODetails_ByID", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        }
                        DataSet ds = new DataSet();
                        SqlDataAdapter sA1 = new SqlDataAdapter(command);
                        sA1.Fill(ds);
                        DataTable dtOrdDetails = ds.Tables[0];

                        for (int i = 0; i < dtOrdDetails.Rows.Count;i++ )
                        {
                            int ProductID =0;
                            int.TryParse(dtOrdDetails.Rows[i]["ProductID"].ToString(), out ProductID);
                            DataView dvSales = dtUpdateSales.DefaultView;
                            dvSales.RowFilter = "ProductID = '" + ProductID + "'";
                            DataTable dtUpdated = dvSales.ToTable();

                            if(dtUpdated!=null && dtUpdated.Rows.Count>0)
                            {
                                for(int j=0;j<dtUpdated.Rows.Count;j++)
                                {
                                    if (connection.State == ConnectionState.Closed) { connection.Open(); }

                                    command = new SqlCommand("sp_ReplenishUpdateSales", connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.AddWithValue("@SaleID", Convert.ToInt32(dtUpdated.Rows[j]["SaleID"].ToString()));
                                    command.Parameters.AddWithValue("@ProductID", ProductID);
                                    command.Parameters.AddWithValue("@OrderID", OrderNumber);
                                    if (VendorID == 1 && isWareHouse == 1)
                                    {
                                        command.Parameters.AddWithValue("@OrderDetailID", Convert.ToInt32(dtOrdDetails.Rows[i]["TransferDetailID"].ToString()));
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@OrderDetailID", Convert.ToInt32(dtOrdDetails.Rows[i]["OrderDetailID"].ToString()));
                                    }
                                    command.Parameters.AddWithValue("@Vendor", VendorID);
                                    command.ExecuteNonQuery();
                                }
                            }
                        }
                        #endregion

                        NextPage = true;

                       
                        WebMessageBoxUtil.Show("Replenishment Order has been generated");
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("Please Specify a Vendor First");
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
                    if (connection.State == ConnectionState.Open) { connection.Close(); }
                    if ((Session["FromSalesDate"] != null && Session["FromSalesDate"].ToString() != "") &&
                        (Session["ToSalesDate"] != null && Session["ToSalesDate"].ToString() != "") &&
                        (Session["ReplenishDays"] != null && Session["ReplenishDays"].ToString() != ""))
                    {
                        DateTime FromDate = Convert.ToDateTime(Session["FromSalesDate"].ToString());
                        DateTime ToDate = Convert.ToDateTime(Session["ToSalesDate"].ToString());

                        TimeSpan diff = ToDate - FromDate;
                        double TotalDays = diff.TotalDays;
                        int Days = Convert.ToInt32(TotalDays);

                        int ReplenishDays = 0;
                        int.TryParse((Session[ReplenishDays].ToString()), out ReplenishDays);

                        LoadData_SalesDate(Days, ReplenishDays, FromDate, ToDate);
                    }
                    else
                    {
                        LoadData();
                    }
                    DisplayMainGrid((DataTable)Session["DataTableView"]);
                    if (NextPage.Equals(true))
                    {
                        Session["RelenishOrderNo"] = OrderNumber;
                        Response.Redirect("ReplenishPO_Generate.aspx");
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("Cannot Create PO at this moment, please check all values are correct; if problem persists, contanct your System Admin");
                    }
                }
            }
        }

        protected void btnAddNewVendor_Click(object sender, EventArgs e)
        {
            //Session["txtVendor"] = "";
            //VendorsPopupGrid.PopulateGrid();
            //mpeCongratsMessageDiv.Show();
        }

        
    }
}