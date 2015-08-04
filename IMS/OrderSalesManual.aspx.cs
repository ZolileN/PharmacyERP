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
    public partial class OrderSalesManual : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public DataSet ProductSet;
        public  DataSet systemSet;
        public static bool FirstOrder;
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

                    Session.Remove("dsProdcts");
                    Session.Remove("dsProducts_MP");
                    btnCreateOrder.Attributes.Add("OnClientClick", "if(ValidateForm()) {return false; }");

                    txtIvnoice.Text = "SO-" + DateTime.Now.TimeOfDay.Hours + "_" + DateTime.Now.TimeOfDay.Minutes;
                    txtIvnoice.Enabled = false;
                    if (Session["OrderNumberSO"] != null && Session["OrderSalesDetail"] != null && Session["OrderSalesDetail"].Equals(true) && Session["ViewSalesOrders"] != null)
                    {
                        if (Session["ViewSalesOrders"] != null && Session["ViewSalesOrders"].Equals(true))
                        {
                            btnAccept.Text = "RE-GENERATE ORDER";
                            Session["ViewSalesOrders"] = false;
                        }
                        Session["ViewSalesOrders"] = null;
                        Session["FirstOrderSO"] = true;
                        systemSet = new DataSet();
                        ProductSet = new DataSet();
                        LoadData();
                        btnAccept.Visible = true;

                        btnDecline.Visible = true;
                        #region Populating System Types
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("sp_GetAllSystem", connection);
                            command.CommandType = CommandType.StoredProcedure;

                            DataSet ds = new DataSet();
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(ds);
                            StockAt.DataSource = ds.Tables[0];
                            StockAt.DataTextField = "SystemName";
                            StockAt.DataValueField = "SystemID";
                            StockAt.DataBind();
                            if (StockAt != null)
                            {
                                StockAt.Items.Insert(0, "Select System");
                                if (Session["SelectedIndexValue"] != null)
                                {
                                    // set index based on value
                                    foreach (ListItem Items in StockAt.Items)
                                    {
                                        if (Items.Text.Equals(Session["SelectedIndexValue"].ToString()))
                                        {
                                            StockAt.SelectedIndex = StockAt.Items.IndexOf(Items);
                                            break;
                                        }
                                    }
                                    StockAt.Enabled = false;
                                }

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

                        if (Session["Invoice"] != null)
                        {
                            txtIvnoice.Text = Session["Invoice"].ToString();
                            txtIvnoice.Enabled = false;
                        }
                        BindGrid();
                    }
                    else
                    {
                        Session.Remove("SalesManID");
                        Session["OrderNumberSO"] = "";
                        Session["FirstOrderSO"] = false;
                        Session["OrderSalesDetail"] = false;
                        Session["ExistingOrder"] = false;
                        systemSet = new DataSet();
                        ProductSet = new DataSet();
                        #region Populating System Types
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("sp_GetAllSystem", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            DataSet ds = new DataSet();
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(ds);
                            StockAt.DataSource = ds.Tables[0];
                            StockAt.DataTextField = "SystemName";
                            StockAt.DataValueField = "SystemID";
                            StockAt.DataBind();
                            if (StockAt != null)
                            {
                                StockAt.Items.Insert(0, "Select System");
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
                        LoadData();
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
        protected void Page_Unload(object sender, EventArgs e)
        {
            #region For Unknown Crash Logic

            if (!IsPostBack)
            {
                //Session.Remove("OrderSalesDetail");
                //Session.Remove("FirstOrderSO");
                //Session.Remove("ExistingOrder");
                //Session.Remove("ViewSalesOrders");
            }
            #endregion
        }
        private void LoadData()
        {
           

             #region Populate salesman dropdown

            try
            {
               

                DataSet dsSalesman = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_getSalesman_CurrentSystem", connection);
                command.Parameters.AddWithValue("@p_SystemID", int.Parse(Session["UserSys"].ToString()));

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(dsSalesman);

                ddlSalesman.DataSource = dsSalesman.Tables[0];
                ddlSalesman.DataTextField = "Name";
                ddlSalesman.DataValueField = "UserID";
                ddlSalesman.DataBind();

                if (ddlSalesman != null)
                {
                    ddlSalesman.Items.Insert(0, "Select System");
                    if (Session["SalesManID"] != null)
                    {
                        // set index based on value
                        foreach (ListItem Items in ddlSalesman.Items)
                        {
                            if (Items.Text.Equals(Session["SalesManID"].ToString()))
                            {
                                ddlSalesman.SelectedIndex = ddlSalesman.Items.IndexOf(Items);
                                break;
                            }
                        }
                        ddlSalesman.Enabled = false;
                    }

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

        private void UpdateStockMinus(int orderDetailID, int quantity, int ProductID, float Discount, int Bonus, int Sent)
        {
            try
            {
                DataSet stockDet;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region Query stock
                SqlCommand command = new SqlCommand("Sp_GetStockBy_SaleOrderDetID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

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
                        command = new SqlCommand("sp_EntrySaleOrderDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                        command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);
                        command.Parameters.AddWithValue("@p_SendQuantity", stockSet[id]);
                        command.Parameters.AddWithValue("@p_BonusQuantity", 0);
                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);
                        command.Parameters.AddWithValue("@p_Discount", Discount);
                        command.ExecuteNonQuery();

                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                        command.Parameters.AddWithValue("@p_Action", "Minus");
                        command.ExecuteNonQuery();
                    }

                } 
                #endregion

                #region re-query stock
                command = new SqlCommand("Sp_GetStockBy_SaleOrderDetID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                ds = new DataSet();
                dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds; 
                #endregion

                #region set bonus values
                Dictionary<int, int> stockSetBonus = new Dictionary<int, int>();

                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (Bonus > 0 && exQuan > 0)
                    {
                        if (exQuan >= Bonus)
                        {
                            stockSetBonus.Add(int.Parse(row["StockID"].ToString()), Bonus);
                            break;
                        }
                        else if (exQuan < Bonus)
                        {
                            stockSetBonus.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            Bonus = Bonus - exQuan;
                        }
                    }
                } 
                #endregion

                #region Update stock for bonus values
                foreach (int id in stockSetBonus.Keys)
                {
                    DataView dv = stockDet.Tables[0].DefaultView;
                    dv.RowFilter = "StockID = " + id;
                    DataTable dt = dv.ToTable();

                    if (dt != null && dt.Rows.Count > 0)
                    {
                        command = new SqlCommand("sp_EntrySaleOrderDetails", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                        command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);

                        if (stockSet.ContainsKey(id))
                        {
                            command.Parameters.AddWithValue("@p_SendQuantity", stockSet[id]);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_SendQuantity", 0);
                        }


                        command.Parameters.AddWithValue("@p_BonusQuantity", stockSetBonus[id]);
                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);
                        command.Parameters.AddWithValue("@p_Discount", Discount);
                        command.ExecuteNonQuery();

                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSetBonus[id]);
                        command.Parameters.AddWithValue("@p_Action", "Minus");
                        command.ExecuteNonQuery();
                    } 
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
                connection.Close();
                //Response.Redirect("Warehouse_StoreRequests.aspx");
            }
        }

        private void UpdateStockPlus(int orderDetailID, int quantity)
        {
            try
            {
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_GetStockID_OrderDetails", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_orderDetID", orderDetailID);
                    DataSet StockDs = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(StockDs);

                    for (int i = 0; i < StockDs.Tables[0].Rows.Count;i++ )
                    {
                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", StockDs.Tables[0].Rows[i]["StockID"]);
                        command.Parameters.AddWithValue("@p_quantity", StockDs.Tables[0].Rows[i]["Quantity"]);
                        command.Parameters.AddWithValue("@p_Action", "Add");
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
                if (connection.State == ConnectionState.Open) { connection.Close(); }
                //Response.Redirect("Warehouse_StoreRequests.aspx");
            }
        }

        public DataSet GetSystems(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", ID);



                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;

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
            return ds;
        }

        public void InsertionMapping(int PO_ID, int PO_DetID, int PO_DetEntryID, int SO_ID, int SO_DetID, int SO_DetEntryID, int ProductID, int VendorID, int QS, int BQS,
                                     DateTime PO_RecieveDate, DateTime SO_CreationDate, DateTime ExpiryDate, Decimal UCP, Decimal USP)

        {
            try
            {
                if (connection.State.Equals(ConnectionState.Closed)) { connection.Open(); }

                SqlCommand command = new SqlCommand("sp_Insert_SOPO_MapTable", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_POID", PO_ID);
                command.Parameters.AddWithValue("@p_PODetID", PO_DetID);
                command.Parameters.AddWithValue("@p_PODetEntryID", PO_DetEntryID);
                command.Parameters.AddWithValue("@p_SOID", SO_ID);
                command.Parameters.AddWithValue("@p_SODetID", SO_DetID);
                command.Parameters.AddWithValue("@p_SODetEntryID", SO_DetEntryID);
                command.Parameters.AddWithValue("@p_ProductID", ProductID);
                command.Parameters.AddWithValue("@p_Vendor", VendorID);
                command.Parameters.AddWithValue("@p_QauntitySold", QS);
                command.Parameters.AddWithValue("@p_BonusQuantitySold", BQS);
                command.Parameters.AddWithValue("@p_PORecieveDate", PO_RecieveDate);
                command.Parameters.AddWithValue("@p_SOCreationDate", SO_CreationDate);
                command.Parameters.AddWithValue("@p_Expiry", ExpiryDate);
                command.Parameters.AddWithValue("@p_UnitCP", UCP);
                command.Parameters.AddWithValue("@p_UnitSP", USP);

                command.ExecuteNonQuery();

            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
            }
        }
        
        public void SO_PO_Mapping(int OrderID)
        {
            #region Mapping of PO - SO

            DataSet SaleOrderFullSet = new DataSet();
            DataSet PurchasOrderFullSet = new DataSet();
            int SaleOrderID = OrderID;
            try
            {
                #region Get Full Current Saled Order DataSet
                if (connection.State.Equals(ConnectionState.Closed)) { connection.Open(); }
                SqlCommand command = new SqlCommand("sp_Mapping_getSaleOrderDetails_byID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SaleOrderID", SaleOrderID);

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                sdA.Fill(SaleOrderFullSet);
                if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
                #endregion

                for (int i = 0; i < SaleOrderFullSet.Tables[0].Rows.Count; i++)
                {

                    #region Get Full Current Purchase Order DataSet for a product and Filtering it on Expiry Dates

                    if (connection.State.Equals(ConnectionState.Closed)) { connection.Open(); }
                    command = new SqlCommand("sp_Mapping_getPurchaseOrderDetails_byProductID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["ProductID"].ToString()));

                    sdA = new SqlDataAdapter(command);
                    sdA.Fill(PurchasOrderFullSet);
                    if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
                    DataTable dtFiltered = new DataTable();

                    if (PurchasOrderFullSet != null && PurchasOrderFullSet.Tables[0].Rows.Count != 0)
                    {
                        DateTime Expiry = Convert.ToDateTime(SaleOrderFullSet.Tables[0].Rows[i]["ExpiryDate"]);
                        DataView dv = PurchasOrderFullSet.Tables[0].DefaultView;

                        dv.RowFilter = "ExpiryDate = " + Expiry;

                        dtFiltered = dv.ToTable();
                    }

                    #endregion

                    #region Preparing SO PO Mapping Values
                    int PO_ID, PO_DetID, PO_DetEntryID, SO_ID, SO_DetID, SO_DetEntryID, ProdID, QS, BQS, VendID;
                    DateTime ExpiryDate, PO_RecieveDate, SO_CreationDate;
                    Decimal UCP, USP;

                    PO_ID = PO_DetID = PO_DetEntryID = SO_ID = SO_DetID = SO_DetEntryID = ProdID = QS = BQS = VendID = 0;
                    UCP = USP = 0;

                    int SendQuantity = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["SendQuantity"].ToString());
                    int BonusSendQuantity = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["BonusQuantity"].ToString());

                    for (int j = 0; j < dtFiltered.Rows.Count; j++)
                    {
                        int RecievedQuantity = Convert.ToInt32(dtFiltered.Rows[j]["ReceivedQuantity"].ToString());
                        int BonusQuantity = Convert.ToInt32(dtFiltered.Rows[j]["BonusQuantity"].ToString());

                        int PO_QuantitySold = Convert.ToInt32(dtFiltered.Rows[j]["QuantitySold"].ToString());
                        int PO_BonusQuantitySold = Convert.ToInt32(dtFiltered.Rows[j]["BonusQuantitySold"].ToString());


                        PO_ID = Convert.ToInt32(dtFiltered.Rows[j]["OrderID"].ToString());
                        PO_DetID = Convert.ToInt32(dtFiltered.Rows[j]["orderDetailID"].ToString());
                        PO_DetEntryID = Convert.ToInt32(dtFiltered.Rows[j]["entryID"].ToString());

                        SO_ID = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["OrderID"].ToString());
                        SO_DetID = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["orderDetailID"].ToString());
                        SO_DetEntryID = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["entryID"].ToString());

                        ProdID = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["ProductID"].ToString());
                        VendID = Convert.ToInt32(dtFiltered.Rows[j]["VendorID"].ToString());

                        UCP = Convert.ToInt32(dtFiltered.Rows[j]["CostPrice"].ToString());
                        USP = Convert.ToInt32(SaleOrderFullSet.Tables[0].Rows[i]["CostPrice"].ToString());

                        ExpiryDate = Convert.ToDateTime(SaleOrderFullSet.Tables[0].Rows[i]["ExpiryDate"]);
                        PO_RecieveDate = Convert.ToDateTime(dtFiltered.Rows[j]["ReceivedDate"].ToString());
                        SO_CreationDate = Convert.ToDateTime(SaleOrderFullSet.Tables[0].Rows[i]["OrderDate"]);

                        if (SendQuantity > 0)
                        {
                            if (PO_QuantitySold + SendQuantity <= RecievedQuantity)
                            {
                                //Mapping Insertion
                                SendQuantity = 0;
                                QS = SendQuantity;

                                InsertionMapping(PO_ID, PO_DetID, PO_DetEntryID, SO_ID, SO_DetID, SO_DetEntryID, ProdID, VendID, QS, BQS, PO_RecieveDate, SO_CreationDate,
                                                 ExpiryDate, UCP, USP);
                            }
                            else
                            {
                                int Remaining = RecievedQuantity - PO_QuantitySold;
                                SendQuantity = SendQuantity - Remaining;
                                QS = Remaining;
                                InsertionMapping(PO_ID, PO_DetID, PO_DetEntryID, SO_ID, SO_DetID, SO_DetEntryID, ProdID, VendID, QS, BQS, PO_RecieveDate, SO_CreationDate,
                                                ExpiryDate, UCP, USP);
                                //Mapping Insertion
                            }
                        }

                        if (BonusSendQuantity > 0)
                        {
                            if (PO_BonusQuantitySold + BonusSendQuantity <= BonusQuantity)
                            {
                                //Mapping Insertion
                                BonusSendQuantity = 0;
                                BQS = BonusSendQuantity;
                                InsertionMapping(PO_ID, PO_DetID, PO_DetEntryID, SO_ID, SO_DetID, SO_DetEntryID, ProdID, VendID, QS, BQS, PO_RecieveDate, SO_CreationDate,
                                                ExpiryDate, UCP, USP);
                            }
                            else
                            {
                                int Remaining = BonusQuantity - PO_BonusQuantitySold;
                                BonusSendQuantity = BonusSendQuantity - Remaining;
                                BQS = Remaining;
                                InsertionMapping(PO_ID, PO_DetID, PO_DetEntryID, SO_ID, SO_DetID, SO_DetEntryID, ProdID, VendID, QS, BQS, PO_RecieveDate, SO_CreationDate,
                                                ExpiryDate, UCP, USP);
                                //Mapping Insertion
                            }
                        }

                        if (BonusSendQuantity <= 0 && SendQuantity <= 0)
                        {
                            break;
                        }

                    }
                    #endregion

                }


            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State.Equals(ConnectionState.Open)) { connection.Close(); }
            }
            #endregion
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            DataSet dsProducts = (DataSet)Session["dsProdcts"];

            Session["RequestedNO"] = Convert.ToInt32(dsProducts.Tables[0].Rows[0]["OrderID"].ToString());
            bool status=checkOrderStatus(Convert.ToInt32(dsProducts.Tables[0].Rows[0]["OrderID"].ToString()));

            //SO_PO_Mapping(Convert.ToInt32(dsProducts.Tables[0].Rows[0]["OrderID"].ToString()));
            if (status)
            {
                Session["FirstOrderSO"] = false;
                Session["OrderSalesDetail"] = false;
                Session["SelectedIndexValue"] = StockAt.SelectedItem;
                if (Session["ExistingOrder"].Equals(true)) { Session["RequestedFromID"] = Session["SystemID"]; }

                Response.Redirect("ViewPackingList_SO.aspx", false);
            }

           
        }

        /// <summary>
        /// method checks the status of all entries against an order detail.
        /// this is a check added against the problem in which all auto selection of products assigns different value from the one ordered.
        /// </summary>
        /// <param name="orderID"></param>
        private bool checkOrderStatus(int orderID) 
        {
            bool status=true;
            try
            {
                DataSet ds = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("SP_GetSO_InfoForVerification", connection);
                command.CommandType = CommandType.StoredProcedure;
           
                command.Parameters.AddWithValue("@p_OrderID", orderID);
               

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                //table[0] contains order detail
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ordQty = int.Parse(ds.Tables[0].Rows[i]["SendQuantity"].ToString());
                    int orgOrderDetID = int.Parse(ds.Tables[0].Rows[i]["orderDetailID"].ToString());
                    int ordBonQty = int.Parse(ds.Tables[0].Rows[i]["BonusQuantity"].ToString());
                    String prodName = ds.Tables[0].Rows[i]["Description"].ToString();
                    // table[1] contains sum of order details receive
                    for (int j = 0; j < ds.Tables[1].Rows.Count; j++) 
                    {
                          int detOrderDetID = int.Parse(ds.Tables[1].Rows[j]["orderDetailID"].ToString());
                          if (orgOrderDetID == detOrderDetID) 
                          {
                              int totBonQty = int.Parse(ds.Tables[1].Rows[j]["totalBonusQty"].ToString());
                              int totSendQty = int.Parse(ds.Tables[1].Rows[j]["totalSendQty"].ToString());
                              if (totBonQty == ordBonQty && totSendQty == ordQty) { }
                              else 
                              {
                                  WebMessageBoxUtil.Show("Quantity mis-match for "+prodName+". Kindly Verify!!");
                                  status= false;
                              }
                          }
                    }
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
            return status;
        }
        protected void btnDecline_Click(object sender, EventArgs e)
        {
            try
            {
                #region Order Deletion 
                if (Session["OrderNumberSO"] != null)
                {
                    int orderID = int.Parse(Session["OrderNumberSO"].ToString());
                    connection.Open();

                    SqlCommand command = new SqlCommand("sp_GetOrderDetailRecieve", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);

                    for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
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
                }
                #endregion

                Session["OrderNumberSO"] = null;
                Session["OrderSalesDetail"] = false;
                Session["FirstOrderSO"] = false;
                Session["ExistingOrder"] = false;
                Session["SelectedIndexValue"] = null;
                Session.Remove("dsProdcts");
                Session.Remove("dsProducts_MP");
                txtSearch.Text = "";
                txtProduct.Text = "";
                SelectProduct.Visible = false;
                StockAt.Enabled = true;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataBind();
                SelectQuantity.Text = "";
                SelectProduct.SelectedIndex = -1;
                StockAt.SelectedIndex = -1;
                btnAccept.Text = "GENERATE ORDER";
                btnAccept.Visible = false;
                btnDecline.Visible = false;
                
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
            }
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "Edit")
                {
                    int sendquantity =0;
                    int bonusquantity=0;
                    
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblQuantity")).Text,out sendquantity);
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblBonus")).Text,out bonusquantity);

                    Session["PreviousValueMain"] = sendquantity + bonusquantity;  // must be checked
                }

                else if (e.CommandName.Equals("UpdateStock"))
                {
                    //lblProductID
                    int quan = 0; 
                    int.TryParse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity")).Text,out quan);
                    int bonus = 0; 
                    int.TryParse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtBonus")).Text,out bonus);
                    int availablestock = 0; 
                    int.TryParse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblAvStock")).Text,out availablestock);
                    int orderDetID = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo")).Text,out orderDetID);
                    int ProductID = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblProductID")).Text,out ProductID);
                    int TotalQuantity = quan + bonus;
                    if (quan + bonus > 0)
                    {
                        if (TotalQuantity <= (availablestock + Convert.ToInt32(Session["PreviousValueMain"].ToString())))
                        {
                            UpdateStockPlus(orderDetID, Convert.ToInt32(Session["PreviousValueMain"].ToString()));
                            connection.Open();

                            #region remove entry from sales order receiving
                            //need to test this
                            SqlCommand command = new SqlCommand();
                            command = new SqlCommand("sp_DeleteSaleOrderDetails", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                            command.Parameters.AddWithValue("@p_ProductID", ProductID);
                            command.ExecuteNonQuery();
                            #endregion

                            #region Update entry in sales order detail
                            command = new SqlCommand();
                            command = new SqlCommand("Sp_UpdateSODetails", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                            command.Parameters.AddWithValue("@p_OrderQuantity", quan);
                            command.Parameters.AddWithValue("@p_BonusQuantity", bonus);
                            command.ExecuteNonQuery();
                            #endregion


                            #region SystemGenerated Selection of Stock

                            #region Updating SelectedQuantity
                            float Discount = 0;
                            float.TryParse(SelectDiscount.Text.ToString(), out Discount); // Needs to check
                            int Total = quan + bonus;

                            UpdateStockMinus(orderDetID, Total, ProductID, Discount, bonus, quan);

                            #endregion

                            #endregion
                        }
                        else
                        {
                            WebMessageBoxUtil.Show("Entered Amount Exceed Available Stock [ " + availablestock.ToString() + " ]");
                        }
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("Ordered quantity and bonus quantity both cannot be 0");
                    }

                }
                else if (e.CommandName.Equals("Delete"))
                {
                    long orderDetID = long.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("OrderDetailNo")).Text);
                    connection.Open();

                    
                        SqlCommand command3 = new SqlCommand("sp_getOrderDetailRecieve_ID", connection);
                        command3.CommandType = CommandType.StoredProcedure;
                        command3.Parameters.AddWithValue("@p_OrderDetID", orderDetID);
                        DataSet ds = new DataSet();
                        SqlDataAdapter sA = new SqlDataAdapter(command3);
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
                    

                    SqlCommand command = new SqlCommand("sp_DeleteSO_ID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);

                    command.ExecuteNonQuery();
                    if (StockDisplayGrid.Rows.Count == 1)
                    {
                        btnAccept.Visible = false;
                        btnDecline.Visible = false;
                    }
                }
                else if (e.CommandName.Equals("Details"))
                {
                    int orderDetID = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("OrderDetailNo")).Text,out orderDetID);
                    int ProductID = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblProductID")).Text,out ProductID);
                    int quan = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblQuantity")).Text,out quan);
                    int bonus = 0;
                    int.TryParse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblBonus")).Text,out bonus);
                    Session["OderDetailID"] = orderDetID;
                    Session["ProductID"] = ProductID;
                    Session["TotalQuantity"] = quan + bonus;
                    Session["SO_BQuan"] = bonus;
                    Session["SO_Quan"] = quan;
                    Session["SelectedIndexValue"] = StockAt.SelectedItem;
                    Session["Invoice"] = txtIvnoice.Text;
                    Response.Redirect("OrderSalesManual_Details.aspx",false);
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
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
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

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {

           
            int quan, bonQuan;
            quan = bonQuan = 0;
            int.TryParse(SelectQuantity.Text, out quan);
            int.TryParse(SelectBonus.Text, out bonQuan);
            int orderDetID = 0;
            if (bonQuan + quan > 0)
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
                    command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(lblProductId.Text));
                    command.Parameters.AddWithValue("@p_SysID", Convert.ToInt32(Session["UserSys"].ToString()));

                   // command.Parameters.AddWithValue("@p_ProductID",  txtSearch.Text);
                    DataSet QuantitySet = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(QuantitySet);
                    RemainingStock = Convert.ToInt32(QuantitySet.Tables[0].Rows[0][0].ToString());

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

                if ((quan + bonQuan) <= RemainingStock)
                {
                    if (Session["FirstOrderSO"].Equals(false))
                    {
                        #region Creating Order

                        int pRequestFrom = 0;
                        int pRequestTo = 0;
                        String OrderMode = "";
                        int OrderType = 3;//incase of vendor this should be 3

                        OrderMode = "Sales";

                        String Invoice = txtIvnoice.Text;
                        String Vendor = "True";
                        int Salesman = 0;
                        
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("sp_CreateSaleOrder", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            //sets vendor
                            if (int.TryParse(StockAt.SelectedValue.ToString(), out pRequestTo))
                            {
                                command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                            }
                            //sets warehouse/store
                            if (int.TryParse(Session["UserSys"].ToString(), out pRequestFrom))
                            {
                                command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                            }
                            int userID = Convert.ToInt32(Session["UserID"].ToString());

                            command.Parameters.AddWithValue("@p_OrderType", OrderType);
                            command.Parameters.AddWithValue("@p_Invoice", Invoice);
                            command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                            command.Parameters.AddWithValue("@p_Vendor", Vendor);
                            command.Parameters.AddWithValue("@p_userID", userID);

                            if(int.TryParse(ddlSalesman.SelectedValue.ToString(), out Salesman))
                            {
                                command.Parameters.AddWithValue("@p_Salesman", Salesman);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_Salesman", DBNull.Value);
                            }
                            command.Parameters.AddWithValue("@p_orderStatus", "Pending");
                            DataTable dt = new DataTable();
                            SqlDataAdapter dA = new SqlDataAdapter(command);
                            dA.Fill(dt);
                            if (dt.Rows.Count != 0)
                            {
                                Session["OrderNumberSO"] = dt.Rows[0][0].ToString();
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

                        #region Linking to Order Detail table
                        
                        try
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                            command.CommandType = CommandType.StoredProcedure;


                            int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                            OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

                            if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                            {
                                command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                            }
                            command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(lblProductId.Text));
                            //if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNumber))
                            //{
                            //    command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                            //}
                            if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                            {
                                command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                            }

                            if (int.TryParse(SelectBonus.Text.ToString(), out BonusOrdered))
                            {
                                command.Parameters.AddWithValue("@p_BonusQuantity", BonusOrdered);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_BonusQuantity", DBNull.Value);
                            }

                            command.Parameters.AddWithValue("@p_status", "Pending");
                            command.Parameters.AddWithValue("@p_comments", "Generated to Outside Store");
                            DataSet LinkResult = new DataSet();
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(LinkResult);
                            if (LinkResult != null && LinkResult.Tables[0] != null)
                            {
                                int.TryParse(LinkResult.Tables[0].Rows[0][0].ToString(), out orderDetID);
                                //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
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

                        #region SystemGenerated Selection of Stock

                        #region Updating SelectedQuantity

                        int Product, SendQuantity, BonusQuantity;
                        Product = SendQuantity = BonusQuantity = 0;
                        float Discount = 0;

                        int.TryParse(lblProductId.Text, out Product);
                        int.TryParse(SelectQuantity.Text.ToString(), out SendQuantity);
                        int.TryParse(SelectBonus.Text.ToString(), out BonusQuantity);

                        float.TryParse(SelectDiscount.Text.ToString(), out Discount);
                        int Total = SendQuantity + BonusQuantity;

                        UpdateStockMinus(orderDetID, Total, Product, Discount, BonusQuantity, SendQuantity);

                        #endregion

                        #endregion

                        Session["FirstOrderSO"] = true;
                    }
                    else
                    {
                       // Session["DetailID"] = "0";// declaring session as  0;
                        #region Product Existing in the Current Order
                        DataSet ds = new DataSet();
                        try
                        {
                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Open();
                            }
                            SqlCommand command = new SqlCommand("sp_GetOrderbyOutside", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            int OrderNumber = 0;


                            if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                            {
                                command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
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
                            connection.Close();
                        }

                        int ProductNO = 0;
                        bool ProductPresent = false;
                        if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNO))
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
                                SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                                command.CommandType = CommandType.StoredProcedure;

                                int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                                OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

                                if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                                {
                                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                                }
                               // command.Parameters.AddWithValue("@p_ProductID", txtSearch.Text);
                                command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(lblProductId.Text));
                                //if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNumber))
                                //{
                                //    command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                                //}
                                if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                                }
                                if (int.TryParse(SelectBonus.Text.ToString(), out BonusOrdered))
                                {
                                    command.Parameters.AddWithValue("@p_BonusQuantity", BonusOrdered);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@p_BonusQuantity", DBNull.Value);
                                }

                                command.Parameters.AddWithValue("@p_status", "Pending");
                                command.Parameters.AddWithValue("@p_comments", "Generated to Outside Store");

                                DataSet LinkResult = new DataSet();
                                SqlDataAdapter sA = new SqlDataAdapter(command);
                                sA.Fill(LinkResult);
                                if (LinkResult != null && LinkResult.Tables[0] != null)
                                {
                                   //org Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                                    int.TryParse(LinkResult.Tables[0].Rows[0][0].ToString(),out orderDetID);
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

                            #region SystemGenerated Selection of Stock

                            #region Updating SelectedQuantity

                            int Product, SendQuantity, BonusQuantity;
                            Product = SendQuantity = BonusQuantity = 0;
                            float Discount = 0;
                            int.TryParse(lblProductId.Text, out Product);
                            int.TryParse(SelectQuantity.Text.ToString(), out SendQuantity);
                            int.TryParse(SelectBonus.Text.ToString(), out BonusQuantity);

                            float.TryParse(SelectDiscount.Text.ToString(), out Discount);
                            int Total = SendQuantity + BonusQuantity;

                            UpdateStockMinus(orderDetID, Total, Product, Discount, BonusQuantity, SendQuantity);

                            #endregion

                            #endregion
                        }
                        else
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record can not be inserted, because it is already present')", true);
                        }
                    }


                    // needs to check this //
                    #region Populate Product Info
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("Sp_FillSO_Details", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        int OrderNumber, DetailID = 0;
                        if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                            //command.ExecuteNonQuery();
                        }
                        //Session["DetailID"] was used here
                        if (orderDetID !=0)
                        {
                            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);

                        }
                        command.ExecuteNonQuery();


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
                    btnAccept.Visible = true;
                    btnDecline.Visible = true;

                }
                else
                {
                    WebMessageBoxUtil.Show("Available Stock ('" + RemainingStock.ToString() + "') is less than the entered quantity [BONUS + SENT]");
                    //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Remaining Stock is less then the entered quantity, please enter less quantity to proceed')", true);
                }
                BindGrid();
                txtSearch.Text = "";
                txtProduct.Text = "";
                SelectProduct.Visible = false;
                SelectBonus.Text = "";
                SelectDiscount.Text = "";
                StockAt.Enabled = false;
                ddlSalesman.Enabled = false;
                SelectQuantity.Text = "";

                SelectProduct.SelectedIndex = -1;
            }
            else 
            {
                WebMessageBoxUtil.Show("Both ordered and bonus quanities cannot be 0");
            }
        }

        private void BindGrid()
        {
            DataSet ds = new DataSet();
            #region Display Products
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetSODetails_ID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                

                if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }
                 
                if (int.TryParse(Session["UserSys"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_SysID", OrderNumber);
                }

                

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                Session["dsProdcts"] = ds;
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                ddlSalesman.DataValueField = ds.Tables[0].Rows[0]["SalesManID"].ToString();

                 

                if(StockDisplayGrid.DataSource == null)
                {
                    Session["FirstOrderSO"] = false;
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
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            
            //must be checked for sessions
            if (Session["OrderSalesDetail"] != null && Session["OrderSalesDetail"].Equals(true)  && Session["OrderSalesDetail"].ToString() != null)
            {
                Session.Remove("SalesManID");
                Session["OrderNumberSO"] = "";
                Session["OrderSalesDetail"] = false;
                Session["SelectedIndexValue"] = null;
                btnAccept.Text = "GENERATE ORDER";
                Response.Redirect("WarehouseMain.aspx", false); //must be rechecked
            }
            else
            {
                try
                {
                    if (Session["OrderNumberSO"] != null)
                    {
                        int orderID = int.Parse(Session["OrderNumberSO"].ToString());
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }

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
                    }
                    Session["OrderNumberSO"] = null;
                    Session["SelectedIndexValue"] = null;
                    btnAccept.Text = "GENERATE ORDER";
                    txtSearch.Text = "";
                    txtProduct.Text = "";
                    SelectProduct.Visible = false;
                    StockAt.Enabled = true;
                    ddlSalesman.Enabled = true;
                    StockDisplayGrid.DataSource = null;
                    StockDisplayGrid.DataBind();
                    SelectQuantity.Text = "";
                    SelectProduct.SelectedIndex = -1;
                    StockAt.SelectedIndex = -1;
                    btnAccept.Visible = false;
                    btnDecline.Visible = false;
                    Session["FirstOrderSO"] = false;
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
                }
                Session["OrderNumberSO"] = "";
                Session["OrderSalesDetail"] = false;
                Response.Redirect("WarehouseMain.aspx", false);
            }
        }

        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (txtProduct.Text.Length >= 3)
            {
                PopulateDropDown(txtProduct.Text);
                SelectProduct.Visible = true;
            }
        }
        //Sp_FillPO_Details
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
                SqlCommand command = new SqlCommand("sp_getStockProductDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SearchText", Text);
                command.Parameters.AddWithValue("@p_SoredAt", Convert.ToInt32(Session["UserSys"].ToString()));
                
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if(ds.Tables[0].Rows.Count.Equals(0))
                {
                    WebMessageBoxUtil.Show("There is no stock for the selected product");
                }
                if (SelectProduct.DataSource != null)
                {
                    SelectProduct.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                
                SelectProduct.DataSource = ds.Tables[0];
                SelectProduct.DataTextField = "Description";
                SelectProduct.DataValueField = "ProductID";
                SelectProduct.DataBind();
                if (SelectProduct != null)
                {
                    SelectProduct.Items.Insert(0, "Select Product");
                    SelectProduct.SelectedIndex = 0;
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

        protected void SelectProduct_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void RequestTo_SelectedIndexChanged(object sender, EventArgs e)
        {
            //RequestTo.Enabled = false;
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Status = (Label)e.Row.FindControl("lblStatus");
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                Button btnDetail = (Button)e.Row.FindControl("btnDetails");
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

                if(Session["ExistingOrder"].Equals(true))
                {
                   // btnDetail.Enabled = false;
                }
                else
                {
                    btnDetail.Enabled = true;
                }
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

                Label OrderDetailID = (Label)e.Row.FindControl("OrderDetailNo");
                GridView Details = (GridView)e.Row.FindControl("StockDetailDisplayGrid");

                #region Display Requests
                try
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_getSaleOrderDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetID", Convert.ToInt32(OrderDetailID.Text));

                    DataSet ds = new DataSet();

                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductSet = ds;
                    Details.DataSource = null;
                    Details.DataSource = ds.Tables[0];
                    Details.DataBind();
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

           
        }

        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex > -1)
            {
                Session["RequestedFromID"] = StockAt.SelectedValue;
            }
        }

        protected void btnPacking_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewPackingList_SO.aspx",false);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            Response.Redirect("SO_GENERATE.aspx",false);
        }

        protected void btnSearchProduct_Click1(object sender, ImageClickEventArgs e)
        {
            String Text = txtSearch.Text + '%';
            Session["Text"] = Text;
            ProductsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void ddlSalesman_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            #region Re-Populating System Types
            try
            {
                int SalesmanId = 0;
                int.TryParse(ddlSalesman.SelectedValue.ToString(), out SalesmanId);
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_getSalesmanPharmacies", connection);
                command.Parameters.AddWithValue("@p_SalesmanId", SalesmanId);
                command.CommandType = CommandType.StoredProcedure;
                

                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                StockAt.DataSource = ds.Tables[0];
                StockAt.DataTextField = "SystemName";
                StockAt.DataValueField = "SystemID";
                StockAt.DataBind();
                if (StockAt != null)
                {
                    StockAt.Items.Insert(0, "Select System");
                    if (Session["SelectedIndexValue"] != null)
                    {
                        // set index based on value
                        foreach (ListItem Items in StockAt.Items)
                        {
                            if (Items.Text.Equals(Session["SelectedIndexValue"].ToString()))
                            {
                                StockAt.SelectedIndex = StockAt.Items.IndexOf(Items);
                                break;
                            }
                        }
                    }

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

        
    }
}