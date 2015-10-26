using IMS.Util;
using IMSCommon.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS.StoreManagement.StoreRequests
{

    // Edited By Shahid 09-10-15
    // Need to Remove Bonus and Discount
    // Making Bonus and Discount fields Visisble=False in aspx and here commenting the original code for future use.
    // In SQL Proceedure, making the Bonus And Discount field null by default and even not removing the Fields there


    public partial class CreateTransferRequest : System.Web.UI.Page
    {
        public DataTable dtTransfer = new DataTable();
        public static DataSet ProductSet;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataTable dtStatic;
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
                    dtStatic = new DataTable();
                    Session["FirstOrderTransfer"] = false;

                    dtStatic.Columns.Add("ProductID");
                    dtStatic.Columns.Add("SystemID");
                    dtStatic.Columns.Add("Product_Name");
                    dtStatic.Columns.Add("RequestedFrom");
                    dtStatic.Columns.Add("RequestedTo");
                    dtStatic.Columns.Add("RequestedQty");

                    DataColumn[] keyColumns = new DataColumn[1];
                    keyColumns[0] = dtStatic.Columns["ProductID"];
                    dtStatic.PrimaryKey = keyColumns;
                   

                    //dtStatic.Columns.Add("BonusQty");
                    //dtStatic.Columns.Add("PercentageDiscount");

                    SelectProduct.Visible = false;
                    SelectStore.Visible = false;

                    step_1();

                    if (dtStatic.Rows.Count == 0) {
                        btnGenerateRequest.Visible = false;
                    }

                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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

        private void BindGrid()
        {
            dgvCreateTransfer.DataSource = dtStatic;
            dgvCreateTransfer.DataBind();


            #region Display Products
            //try
            //{
            //    if (connection.State == ConnectionState.Closed)
            //    {
            //        connection.Open();
            //    }
            //    SqlCommand command = new SqlCommand("sp_GetTransferDetails_TranferID", connection);
            //    command.CommandType = CommandType.StoredProcedure;
            //    int TransferNo = 0;


            //    if (int.TryParse(Session["TransferNo"].ToString(), out TransferNo))
            //    {
            //        command.Parameters.AddWithValue("@TranferID", TransferNo);
            //    }

                
            //    SqlDataAdapter sA = new SqlDataAdapter(command);
            //    sA.Fill(ds);
            //    Session["dsTransfer"] = ds;

            //    dgvCreateTransfer.DataSource = null;
            //    dgvCreateTransfer.DataSource = ds.Tables[0];
            //    dgvCreateTransfer.DataBind();

            //    if (dgvCreateTransfer.DataSource == null)
            //    {
            //        Session["FirstOrderTransfer"] = false;
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

        }


        

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
             
            Session.Remove("TransferNo");
            Session.Remove("FirstOrderTransfer");
            Session.Remove("ProductId");
            Session.Remove("PrevtransferdQty");
            
            Response.Redirect("StoreMain.aspx", false);
        }

        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
            String Text = txtSearch.Text + '%';
           // Session["Text"] = Text;
            Session["IsOpenedFromCreateTransferReq"] = true;
            SelectProduct.Visible = true;
            ViewState["ProductName"] = Text;
            PopulateProductGrid(Text);
           // ProductsPopupGrid.PopulateStoreUserGrid();
            //mpeCongratsMessageDiv.Show();
        }

        protected void btnSelectStore_Click(object sender, EventArgs e)
        {
            //Session["ProductId"] = lblProduct.Text;

            //StoresPopupGrid.PopulateStoreWithStock();
            //mpeStoresPopupDiv.Show();
        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {

            step_default();

            if (dtStatic.Rows.Count >= 0)
            {
                btnGenerateRequest.Visible = true;
            }

            
            

           
          

            //dtStatic.Rows.Add(lblProductId.Text, lblStoreId.Text, txtSearch.Text, Session["UserSys"].ToString(), txtStore.Text, txtTransferredQty.Text, txtBonusQty.Text,txtPercentageDiscount.Text);
            dtStatic.Rows.Add(ViewState["ProductID"].ToString(), ViewState["StoreID"].ToString(), ViewState["Product_Name"].ToString(), Session["UserSys"].ToString(), ViewState["SystemName"].ToString(), txtTransferredQty.Text);
            dgvCreateTransfer.DataSource = dtStatic;
            dgvCreateTransfer.DataBind();
            Session["TransferRequestGrid"] = dtStatic;
            txtSearch.Text = "";
         //   txtStore.Text = "";
            txtTransferredQty.Text = "";
            //txtBonusQty.Text = "";
            //txtPercentageDiscount.Text = "";
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            

        }

        protected void SelectProduct_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in StockDisplayGrid.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                       // Control ctl = this.Parent.Parent;
                        TextBox ltMetaTags = null;
                     //   GridView gvStockDisplayGrid = (GridView)ctl.FindControl("StockDisplayGrid");

                        //ltMetaTags = (TextBox)ctl.FindControl("txtSearch");

                        Label lbProdId = (row.Cells[0].FindControl("lblProductID") as Label);
                        //if (lbProdId != null)
                        //{
                        //    lbProdId.Text = Server.HtmlDecode(row.Cells[4].Text);
                        //}

                        

                        Label ProdName = row.Cells[0].Controls[0].FindControl("ProductName") as Label;

                        

                        //DataSet dsProducts_ProdPopUp = new DataSet();
                        //dsProducts_ProdPopUp.Tables.Add((DataTable)Session["dsProdcts"]);

                        DataTable ProductsList = (DataTable)Session["TransferRequestGrid"];

                        if (ProductsList != null && ProductsList.Rows.Count > 0 && ProductsList.Rows.Count > 0)
                        {
                            

                            DataRow drs = ProductsList.Rows.Find("ProductID = '" + lbProdId.Text + "'");
                            if (drs!=null)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product already added to the Sales Order.')", true);
                                return;
                            }
                        }

                        
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);

                        }

                       // lblProduct.Text = lbProdId.Text;
                        ViewState["Product_Name"] = ProdName.Text;
                           ViewState["ProductID"] = lbProdId.Text;


                        if (StockDisplayGrid != null)
                        {
                            StockDisplayGrid.DataSource = null;
                            StockDisplayGrid.DataBind();
                            PopulateGrid();
                        }

                    }
                }
             
            }

        }

        public void SelectStore_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in StoresPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {



                        Label lblStoreID = (row.Cells[0].FindControl("SystemID") as Label);//row.Cells[0].Controls[0].FindControl("SystemID") as Label;
                        Label lblSystemName = (row.Cells[0].FindControl("SystemName") as Label);
                        //if (lblStoreID != null)
                        //{
                        //    lblStoreID.Text = Server.HtmlDecode(row.Cells[5].Text);
                        //}
                            //lProduct.Text = lblStoreID.Text;



                        if (StoresPopup != null)
                        {
                            StoresPopup.DataSource = null;
                            StoresPopup.DataBind();
                            SelectStore.Visible = false;
                        }
                      

                        step_4();

                        ViewState["StoreID"] = lblStoreID.Text;
                        ViewState["SystemName"] = lblSystemName.Text;

                    }
                }

            }
        }

        public void PopulateGrid()
        {
            //SelectProduct.Visible = false;
            //DataTable dt = new DataTable();
            //DataSet ds = new DataSet();
            //#region Getting Product Details
            //try
            //{
            //    int id;
            //    if (int.TryParse(Session["UserSys"].ToString(), out id))
            //    {
            //        if (connection.State == ConnectionState.Closed)
            //        {
            //            connection.Open();
            //        }
            //        SqlCommand command = new SqlCommand("dbo.Sp_GetSystems_ByRoles", connection);
            //        command.CommandType = CommandType.StoredProcedure;
            //        if (Session["txtStore"] == null)
            //        {
            //            command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
            //        }
            //        else
            //        {
            //            command.Parameters.AddWithValue("@p_systemName", Session["txtStore"].ToString());
            //        }
            //        command.Parameters.AddWithValue("@p_RoleName", "Store");
            //        SqlDataAdapter SA = new SqlDataAdapter(command);
            //        SA.Fill(ds);
            //        StoresPopup.DataSource = ds;
            //        StoresPopup.DataBind();
            //        SelectStore.Visible = true;

            //    }

            //}
            //catch (Exception ex)
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //    throw ex;
            //}
            //finally
            //{
            //    if (connection.State == ConnectionState.Open)
            //        connection.Close();
            //}
            //#endregion


            try
            {

                step_3();

                
                SelectProduct.Visible = false;
                DataSet dsResults = new DataSet();
                int ProductID = int.Parse(ViewState["ProductID"].ToString());
                int LogedInStoreID;
                int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetStoreByProductID", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_LogedInStoreID", LogedInStoreID);
                command.Parameters.AddWithValue("@ProductID", ProductID);
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dsResults);

                StoresPopup.DataSource = dsResults;
                StoresPopup.DataBind();
                SelectStore.Visible = true;
                if (dsResults.Tables[0].Rows.Count == 0) {
                    SelectStore.Visible = false;
                    noPharmacy.Visible = true;
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

        public void PopulateProductGrid(String ProductName = "")
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();

            step_2();


            #region Getting Product Details
            try
            {
                int id;
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    DataSet ds2 = new DataSet();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetStockByName_OtherStoreStock", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_prodName", ProductName);
                    command.Parameters.AddWithValue("@p_SysID", id);
                    command.Parameters.AddWithValue("@p_isStore", false);

                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    SA.Fill(ds2);
                    

                    StockDisplayGrid.DataSource = ds2;
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            #endregion
        }

        protected void btnGenerateRequest_Click(object sender, EventArgs e)
        {
            //Creating a request to other Pharmacy
            DataTable distinctStores = dtStatic.DefaultView.ToTable(true, "SystemID");
            foreach (DataRow dr in distinctStores.Rows)
            {
                DataRow[] drTransferDetails = dtStatic.Select("SystemID =" + dr["SystemID"].ToString());

                foreach (var drDetails in drTransferDetails)
                {
                    if(drDetails!=null)
                    {
                        int TransferredQty;
                        TransferredQty = 0;
                        int.TryParse(drDetails["RequestedQty"].ToString(), out TransferredQty);
                        if (TransferredQty > 0)
                        {
                            int RemainingStock = 0;
                            try
                            {
                                if (connection.State == ConnectionState.Closed)
                                {
                                    connection.Open();
                                }
                                SqlCommand command = new SqlCommand("sp_getStock_Quantity", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(drDetails["ProductID"].ToString()));
                                command.Parameters.AddWithValue("@p_SysID", drDetails["SystemID"].ToString());


                                DataSet QuantitySet = new DataSet();
                                SqlDataAdapter sA = new SqlDataAdapter(command);
                                sA.Fill(QuantitySet);
                                if (QuantitySet != null && QuantitySet.Tables[0].Rows.Count > 0 && QuantitySet.Tables[0].Rows[0][0].ToString()!="")
                                {
                                    RemainingStock = Convert.ToInt32(QuantitySet.Tables[0].Rows[0][0].ToString());
                                }
                                else
                                {
                                    WebMessageBoxUtil.Show("The requested quantity exceeds the total stock limit of the selected pharmacy");
                                    return;
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
                            if (Session["FirstOrderTransfer"].Equals(false))
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

                                    SqlCommand command = new SqlCommand("sp_CreateTransferOrder", connection);
                                    command.CommandType = CommandType.StoredProcedure;
                                    //Select Vendor
                                    if (int.TryParse(dr["SystemID"].ToString(), out p_TransferTo))
                                    {
                                        command.Parameters.AddWithValue("@p_TransferTo", p_TransferTo);
                                    }
                                    //Select From
                                    if (int.TryParse(Session["UserSys"].ToString(), out p_TransferBy))
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

                                #region Linking to Transfer Detail table

                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    SqlCommand command = new SqlCommand("sp_InsertIntoTransferOrderDetails", connection);
                                    command.CommandType = CommandType.StoredProcedure;


                                    int TransferNo, BonusOrdered, ProductNumber, Quantity;
                                    TransferNo = BonusOrdered = ProductNumber = Quantity = 0;

                                    if (int.TryParse(Session["TransferNo"].ToString(), out TransferNo))
                                    {
                                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                                    }
                                    command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(drDetails["ProductID"].ToString()));

                                    if (int.TryParse(drDetails["RequestedQty"].ToString(), out Quantity))
                                    {
                                        command.Parameters.AddWithValue("@p_RequestedQty", Quantity);
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@p_RequestedQty", DBNull.Value);
                                    }
                                    //int BonusQty = 0;
                                    //double DiscountPercentage;
                                    //int.TryParse(drDetails["BonusQty"].ToString(),out BonusQty);
                                    //double.TryParse(drDetails["PercentageDiscount"].ToString(), out DiscountPercentage);

                                    //command.Parameters.AddWithValue("@p_ReqBonusQty", BonusQty);
                                    //command.Parameters.AddWithValue("@p_Discount", DiscountPercentage);

                                    command.Parameters.AddWithValue("@p_TransferStatus", "Initiated");

                                    DataSet LinkResult = new DataSet();
                                    SqlDataAdapter sA = new SqlDataAdapter(command);
                                    sA.Fill(LinkResult);
                                    if (LinkResult != null && LinkResult.Tables[0] != null)
                                    {
                                        Session["TransferDetailID"] = LinkResult.Tables[0].Rows[0][0];
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
                                int TransferDetailID = Convert.ToInt32(Session["TransferDetailID"].ToString());
                                //UpdateStockMinus(TransferDetailID, Convert.ToInt32(drDetails["ProductID"].ToString()), 0, Convert.ToInt32(drDetails["RequestedQty"].ToString()), Convert.ToInt32(dr["SystemID"].ToString()));

                                Session["FirstOrderTransfer"] = true;
                            }
                            else
                            {
                                Session["TransferDetailID"] = "0";

                                #region Check wether Product Existing in the Current Transfer
                                DataSet ds = new DataSet();
                                try
                                {
                                    if (connection.State == ConnectionState.Open)
                                    {
                                        connection.Open();
                                    }
                                    SqlCommand command = new SqlCommand("sp_getTransferDetails_TransferID", connection);
                                    command.CommandType = CommandType.StoredProcedure;
                                    int TransferNo = 0;


                                    if (int.TryParse(Session["TransferNo"].ToString(), out TransferNo))
                                    {
                                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                                    }
                                    SqlDataAdapter sA = new SqlDataAdapter(command);
                                    sA.Fill(ds);
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

                                int ProductNO = 0;
                                bool ProductPresent = false;
                                if (int.TryParse(drDetails["ProductID"].ToString(), out ProductNO))
                                {
                                }

                                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                                {
                                    if (Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]).Equals(ProductNO))
                                    {
                                        ProductPresent = true;
                                        break;
                                    }
                                }
                                #endregion

                                if (ProductPresent.Equals(false))
                                {
                                    #region Linking to Order Detail table

                                    try
                                    {
                                        if (connection.State == ConnectionState.Closed)
                                        {
                                            connection.Open();
                                        }
                                        SqlCommand command = new SqlCommand("sp_InsertIntoTransferOrderDetails", connection);
                                        command.CommandType = CommandType.StoredProcedure;


                                        int TransferNo, BonusOrdered, ProductNumber, Quantity;
                                        TransferNo = BonusOrdered = ProductNumber = Quantity = 0;

                                        if (int.TryParse(Session["TransferNo"].ToString(), out TransferNo))
                                        {
                                            command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                                        }
                                        command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(drDetails["ProductID"].ToString()));

                                        if (int.TryParse(drDetails["RequestedQty"].ToString(), out Quantity))
                                        {
                                            command.Parameters.AddWithValue("@p_RequestedQty", Quantity);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@p_RequestedQty", DBNull.Value);
                                        }

                                        //int BonusQty = 0;
                                        
                                        //double DiscountPercentage;
                                        //int.TryParse(drDetails["BonusQty"].ToString(), out BonusQty);
                                        //double.TryParse(drDetails["PercentageDiscount"].ToString(), out DiscountPercentage);

                                        //command.Parameters.AddWithValue("@p_ReqBonusQty", BonusQty);
                                        //command.Parameters.AddWithValue("@p_Discount", DiscountPercentage);


                                        command.Parameters.AddWithValue("@p_TransferStatus", "Initiated");

                                        DataSet LinkResult = new DataSet();
                                        SqlDataAdapter sA = new SqlDataAdapter(command);
                                        sA.Fill(LinkResult);
                                        if (LinkResult != null && LinkResult.Tables[0] != null)
                                        {
                                            Session["TransferDetailID"] = LinkResult.Tables[0].Rows[0][0];
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
                                    int TransferDetailID = Convert.ToInt32(Session["TransferDetailID"].ToString());
                                    //UpdateStockMinus(TransferDetailID, Convert.ToInt32(drDetails["ProductID"].ToString()), 0, Convert.ToInt32(drDetails["RequestedQty"].ToString()), Convert.ToInt32(dr["SystemID"].ToString()));

                                }
                                else
                                {
                                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record can not be inserted, because it is already present')", true);
                                }
                            }
                            //if (TransferredQty <= RemainingStock)
                            //{
                            //    //if (Session["StoreId"] != null)
                            //    //{
                            //    //    if (lblStoreId.Text.ToString() != Session["StoreId"].ToString())
                            //    //    {
                            //    //        Session["FirstOrderTransfer"] = false;
                            //    //    }
                            //    //    else
                            //    //    {
                            //    //        Session["FirstOrderTransfer"] = true;
                            //    //    }
                            //    //}

                                
                            //}
                            //else
                            //{
                            //    WebMessageBoxUtil.Show("Available Stock ('" + RemainingStock.ToString() + "') is less than the entered quantity [Transferred Qty]");
                            //}
                        }
                    }
                }
                Session["FirstOrderTransfer"] = false;
                
            }
             
            Response.Redirect("GenerateTransferRequest.aspx", false);
        }

        private void UpdateStockMinus(int TransferDetailID, int ProductID, int quantity, int Sent, int StoredAt)
        {
            try
            {
                DataSet stockDet;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region Query stock
                SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", StoredAt);

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                #endregion

                #region Set value for ordered quantity
                Dictionary<int, int> stockSet = new Dictionary<int, int>();
                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (Sent > 0 && exQuan > 0)
                    {
                        if (exQuan >= Sent)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), Sent);
                            break;
                        }
                        else if (exQuan < Sent)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            Sent = Sent - exQuan;
                        }
                    }
                }


                #endregion

                #region update stock

                foreach (int id in stockSet.Keys)
                {
                    DataView dv = stockDet.Tables[0].DefaultView;
                    dv.RowFilter = "StockID = " + id;
                    DataTable dt = dv.ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        command = new SqlCommand("sp_EntryTransferDetails_Receive", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetailID);
                        command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);
                        command.Parameters.AddWithValue("@p_TransferredQty", stockSet[id]);

                        command.Parameters.AddWithValue("@p_RequestedQty", dt.Rows[0]["RequestedQty"]);

                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);

                        command.ExecuteNonQuery();

                        //command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        //command.CommandType = CommandType.StoredProcedure;
                        //command.Parameters.AddWithValue("@p_StockID", id);
                        //command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                        //command.Parameters.AddWithValue("@p_Action", "Minus");
                        //command.ExecuteNonQuery();
                    }

                }
                #endregion


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

        protected void dgvCreateTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCreateTransfer.PageIndex = e.NewPageIndex;
            //PopulateProductGrid(ViewState["ProductName"].ToString());
        }
        protected void dgvCreateTransfer_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int rowindex = e.RowIndex;
                dtStatic.Rows[rowindex].Delete();
                dtStatic.AcceptChanges();
                BindGrid();
            }
            catch(Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                BindGrid();
            }
        }
         

        protected void dgvCreateTransfer_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvCreateTransfer.EditIndex = e.NewEditIndex;
            BindGrid();

        }

        protected void dgvCreateTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                     
                }
                else if (e.CommandName == "UpdateStock")
                {
                    int rowindex = dgvCreateTransfer.EditIndex;
                     
                    
                    TextBox tbTransferedQty = (TextBox)dgvCreateTransfer.Rows[rowindex].FindControl("txtTransferQty");
                    int TransferedQty = 0;
                    int.TryParse(tbTransferedQty.Text.ToString(), out TransferedQty);

                  //  TextBox txtBonusQty = (TextBox)dgvCreateTransfer.Rows[rowindex].FindControl("txtBonusQty");
                   // int BonusQty = 0;
                  //  int.TryParse(txtBonusQty.Text.ToString(), out BonusQty);

                  //  TextBox txtPercentageDiscount = (TextBox)dgvCreateTransfer.Rows[rowindex].FindControl("txtPercentageDiscount");
                 //   float PercentDiscount = 0;
                  //  float.TryParse(txtPercentageDiscount.Text.ToString(), out PercentDiscount);

                    dtStatic.Rows[rowindex]["RequestedQty"] = TransferedQty;
                  //  dtStatic.Rows[rowindex]["BonusQty"] = BonusQty;
                   // dtStatic.Rows[rowindex]["PercentageDiscount"] = PercentDiscount;
                    dtStatic.AcceptChanges();
                     

                    #region remove entry from sales order receiving
                    
                    #endregion
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
                
                dgvCreateTransfer.EditIndex = -1;
                BindGrid();
            }
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            PopulateProductGrid(ViewState["ProductName"].ToString());
        }

        protected void StoresPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StoresPopup.PageIndex = e.NewPageIndex;
            PopulateGrid();
        }

        public void step_1()
        {
            step1.Attributes["class"] += " active";
            StockDisplayGrid.Visible = false;
            StoresPopup.Visible = false;
            lbQty.Visible = false;
            txtTransferredQty.Visible = false;
            btnAddRequest.Visible = false;
            noPharmacy.Visible = false;
        }
        public void step_2()
        {
            step1.Attributes.Add("class", step1.Attributes["class"].ToString().Replace("active", ""));
            step2.Attributes["class"] += " active";

            StoresPopup.Visible = false;
            StockDisplayGrid.Visible = true;

            lblProd.Visible = false;
            txtSearch.Visible = false;
            btnSearchProduct.Visible = false;

            noPharmacy.Visible = false;


        }
        public void step_3()
        {
            step1.Attributes.Add("class", step1.Attributes["class"].ToString().Replace("active", ""));
            step2.Attributes.Add("class", step2.Attributes["class"].ToString().Replace("active", ""));
            step3.Attributes["class"] += " active";
            StoresPopup.Visible = true;
            StockDisplayGrid.Visible = false;

            noPharmacy.Visible = false;
        }
        public void step_4()
        {
            step1.Attributes.Add("class", step1.Attributes["class"].ToString().Replace("active", ""));
            step2.Attributes.Add("class", step2.Attributes["class"].ToString().Replace("active", ""));
            step3.Attributes.Add("class", step3.Attributes["class"].ToString().Replace("active", ""));
            step4.Attributes["class"] += " active";

            StoresPopup.Visible = false;
            StockDisplayGrid.Visible = false;

            lbQty.Visible = true;
            txtTransferredQty.Visible = true;
            btnAddRequest.Visible = true;

            noPharmacy.Visible = false;


        }
        public void step_default()
        {
            step4.Attributes.Add("class", step4.Attributes["class"].ToString().Replace("active", ""));
            step1.Attributes.Add("class", step1.Attributes["class"].ToString() + " active");
            StoresPopup.Visible = false;
            StockDisplayGrid.Visible = false;

            lblProd.Visible = true;
            txtSearch.Visible = true;
            btnSearchProduct.Visible = true;
            lbQty.Visible = false;
            txtTransferredQty.Visible = false;
            btnAddRequest.Visible = false;

            noPharmacy.Visible = false;

        }

        protected void reset_Click(object sender, EventArgs e)
        {
            step2.Attributes.Add("class", step2.Attributes["class"].ToString().Replace("active", ""));
            step3.Attributes.Add("class", step3.Attributes["class"].ToString().Replace("active", ""));
            step4.Attributes.Add("class", step4.Attributes["class"].ToString().Replace("active", ""));
            step1.Attributes.Add("class", step1.Attributes["class"].ToString() + " active");

            lblProd.Visible = true;
            txtSearch.Visible = true;
            btnSearchProduct.Visible = true;

            lbQty.Visible = false;
            txtTransferredQty.Visible = false;
            btnAddRequest.Visible = false;

            StoresPopup.Visible = false;
            StockDisplayGrid.Visible = false;

            noPharmacy.Visible = false;

            SelectProduct.Visible = false;
            SelectStore.Visible = false;

            ViewState["ProductID"] = null;
            ViewState["ProductName"] = null;
            ViewState["StoreID"]=null;
            ViewState["Product_Name"] = null;
            ViewState["SystemName"] = null;

        }



    }

    
}