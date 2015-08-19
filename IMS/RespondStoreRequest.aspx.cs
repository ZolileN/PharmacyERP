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
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class RespondStoreRequest : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

                if (!IsPostBack)
                {
                    LoadRepeater();
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
        private void LoadRepeater()
        {
            try
            {
                DataSet ds = new DataSet();
                int Userid;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_getTransferDetails_UserId", connection);
                int.TryParse(Session["UserSys"].ToString(), out Userid);
                command.Parameters.AddWithValue("@UserID", Userid);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                DataTable dsDistinct = ds.Tables[0];
                DataTable distinctRequests = dsDistinct.DefaultView.ToTable(true, "TransferTo");
                //dsDistinct.DefaultView.RowFilter(); 
                repReceiveTransfer.DataSource = distinctRequests;
                repReceiveTransfer.DataBind();
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

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReceiveStoreRequest.aspx",false);
        }

        protected void repReceiveTransfer_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                GridView dgvReceiveTransfer = (GridView)e.Item.FindControl("dgvReceiveTransfer");
                Literal litStoreName = (Literal)e.Item.FindControl("litStoreName");

                DataSet ds = new DataSet();
                int transID,storedAt;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
               
                SqlCommand command = new SqlCommand("Sp_GetTransferDetails_byParameters", connection);
                command.CommandType = CommandType.StoredProcedure;
                int.TryParse(Session["WH_RequestedNO"].ToString(), out transID);
                command.Parameters.AddWithValue("@p_TransferID", transID);
                int.TryParse(Session["UserSys"].ToString(), out storedAt);
                command.Parameters.AddWithValue("@p_storedAt", storedAt);

             
            
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
               
                dgvReceiveTransfer.DataSource = ds;
                dgvReceiveTransfer.DataBind();

                litStoreName.Text = ds.Tables[0].Rows[0]["RequestedBy"].ToString();
                Session["WH_RequestedFromID"] = ds.Tables[0].Rows[0]["RequestedByID"].ToString();
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

        protected void repReceiveTransfer_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            try
            {
                if (e.CommandName == "AcceptTransferOrder")
                {
                    GridView gvReceiveTransfer = (GridView)repReceiveTransfer.Items[0].FindControl("dgvReceiveTransfer");
                    for (int i = 0; i < gvReceiveTransfer.Rows.Count; i++)
                    {
                        int TransferNo, TransferDetailNo, RequestedQty,requestedBonusQty, TransferedQty,transferedBonusQty, AvailableQty, ProductId;
                        int LogedInStoreID;


                        int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);

                        Label lblTransferNo = (Label)gvReceiveTransfer.Rows[i].FindControl("lblRequestNo");
                        Label lblTransferDetailsID = (Label)gvReceiveTransfer.Rows[i].FindControl("lblTransferDetailsID");
                        Label lblRequestedQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblRequestedQty");
                        Label lblRequestedBonQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblReqBonQty");
                        Label lblAvailableQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblAvailableQty");
                        Label lblSentQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblSentQty");
                        Label lblSentBonQty = (Label)gvReceiveTransfer.Rows[i].FindControl("lblSentBonQty");
                        Label lblProductID = (Label)gvReceiveTransfer.Rows[i].FindControl("lblProductID");

                        int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                        int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                        
                        int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);
                        int.TryParse(lblRequestedBonQty.Text.ToString(), out requestedBonusQty);

                        int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                        
                        int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                        int.TryParse(lblSentBonQty.Text.ToString(), out transferedBonusQty);

                        int.TryParse(lblProductID.Text.ToString(), out ProductId);
                        int userID = Convert.ToInt32(Session["UserID"].ToString());

                        if (AvailableQty > 0 && (RequestedQty + requestedBonusQty <= AvailableQty))
                        {
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }

                            SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_AcceptAll", connection);
                            command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                            command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                            command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                            command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                            command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                            command.Parameters.AddWithValue("@p_Status", "Accepted");
                            command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                            command.Parameters.AddWithValue("@p_ProductID", ProductId);
                            command.Parameters.AddWithValue("@p_TransferedBonQty", transferedBonusQty);
                           // command.Parameters.AddWithValue("@p_SystemID", int.Parse(Session["WH_RequestedFromID"].ToString()));
                            command.Parameters.AddWithValue("@p_TransferToUserID", userID);

                            command.CommandType = CommandType.StoredProcedure;
                            command.ExecuteNonQuery();

                            Button btnAccept = (Button)gvReceiveTransfer.Rows[i].FindControl("btnAccept");
                            btnAccept.Visible = false;
                            Button btnDeny = (Button)gvReceiveTransfer.Rows[i].FindControl("btnDeny");
                            btnDeny.Visible = false;
                            Button btnEdit = (Button)gvReceiveTransfer.Rows[i].FindControl("btnEdit");
                            btnEdit.Visible = false;
                            HtmlGenericControl btnStaticAccepted = (HtmlGenericControl)gvReceiveTransfer.Rows[i].FindControl("btnStaticAccepted");
                            btnStaticAccepted.Visible = true;  
                          
                            if (RequestedQty != TransferedQty || requestedBonusQty != transferedBonusQty)
                            {
                                if (TransferedQty == 0)
                                {
                                    if (requestedBonusQty == 0)
                                    {
                                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty, requestedBonusQty);
                                    }
                                    else 
                                    {
                                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty, transferedBonusQty);
                                    }
                                }
                                else
                                {
                                    if (requestedBonusQty == 0)
                                    {
                                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty, requestedBonusQty);
                                    }
                                    else
                                    {
                                        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty, transferedBonusQty);
                                    }
                                }
                            }
                        }
                    }
                }
                if (e.CommandName == "GenTransferOrder")
                {
                    DataSet dsResults = new DataSet();

                    GridView gvReceiveTransfer = (GridView)repReceiveTransfer.Items[0].FindControl("dgvReceiveTransfer");
                    int TransferNo, TransferDetailNo, RequestedQty, TransferedQty, AvailableQty, ProductId;
                    int LogedInStoreID;


                    int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);

                    Label lblTransferNo = (Label)gvReceiveTransfer.Rows[0].FindControl("lblRequestNo");
                    Label lblTransferDetailsID = (Label)gvReceiveTransfer.Rows[0].FindControl("lblTransferDetailsID");
                    Label lblRequestedQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblRequestedQty");
                    Label lblAvailableQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblAvailableQty");
                    Label lblSentQty = (Label)gvReceiveTransfer.Rows[0].FindControl("lblSentQty");
                    Label lblProductID = (Label)gvReceiveTransfer.Rows[0].FindControl("lblProductID");

                    int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                    int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                    int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);

                    int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                    int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                    int.TryParse(lblProductID.Text.ToString(), out ProductId);

                    int salesOrderNo=CreateAssociatedSalesOrder(TransferNo);

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }

                    SqlCommand command = new SqlCommand("Sp_SetSOwithTO", connection);
                    command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    command.Parameters.AddWithValue("@p_SalesOrderID", salesOrderNo);
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    command = new SqlCommand("sp_UpdateTransferOrderDetials_Generate", connection);
                    command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                    command.Parameters.AddWithValue("@p_SystemID", int.Parse(Session["WH_RequestedFromID"].ToString()));
               
                    //command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    //command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                    //command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                    //command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                    //command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                    //command.Parameters.AddWithValue("@p_Status", "Accepted");
                    //command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                    //command.Parameters.AddWithValue("@p_ProductID", ProductId);
                    //command.Parameters.AddWithValue("@p_SystemID", int.Parse(Session["WH_RequestedFromID"].ToString()));
                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();

                    SqlDataAdapter da = new SqlDataAdapter(command);
                    da.Fill(dsResults);

                    Session["TransferRequestGrid"] = dsResults.Tables[0];
                    Session["WH_SoID"] = salesOrderNo;
                    int transID;
                    int.TryParse(Session["WH_RequestedNO"].ToString(), out transID);
                   
                    Response.Redirect("WHResquestInvoice.aspx?Id=" + transID, false);
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
                LoadRepeater();
            }

        }

        private int CreateAssociatedSalesOrder(int transferID)
        {
            int salesOrderID = 0;
            try
            {
                DataSet resSet = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetCompleteTransferInfo", connection);
                command.Parameters.AddWithValue("@p_TransferID", transferID);
                command.CommandType = CommandType.StoredProcedure;


                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(resSet);
                int.TryParse(resSet.Tables[0].Rows[0]["AssociatedSOID"].ToString(), out salesOrderID);
                if (salesOrderID == 0)
                {
                    #region Creating Order

                    int pRequestFrom = 0;
                    int pRequestTo = 0;
                    String OrderMode = "";
                    int OrderType = 3;//incase of vendor this should be 3

                    OrderMode = "Transfer";
                    int userID = Convert.ToInt32(Session["UserID"].ToString());

                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    command = new SqlCommand("sp_CreateSaleOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    //sets vendor
                    if (int.TryParse(resSet.Tables[0].Rows[0]["TransferTo"].ToString(), out pRequestTo))
                    {
                        command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                    }
                    //sets warehouse/store
                    if (int.TryParse(resSet.Tables[0].Rows[0]["TransferBy"].ToString(), out pRequestFrom))
                    {
                        command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                    }

                    command.Parameters.AddWithValue("@p_OrderType", OrderType);
                    command.Parameters.AddWithValue("@p_Invoice", DBNull.Value);
                    command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                    command.Parameters.AddWithValue("@p_Vendor", DBNull.Value);

                    command.Parameters.AddWithValue("@p_Salesman", DBNull.Value);
                    command.Parameters.AddWithValue("@p_userID", userID);
                    command.Parameters.AddWithValue("@p_orderStatus", "Pending");
                    command.Parameters.AddWithValue("@p_isCreatedSO", "true");
                    DataTable dt = new DataTable();
                    SqlDataAdapter dA = new SqlDataAdapter(command);
                    dA.Fill(dt);
                    if (dt.Rows.Count != 0)
                    {
                        salesOrderID = int.Parse(dt.Rows[0][0].ToString());
                    }

                    #endregion
                    if (salesOrderID > 0)
                    {
                        foreach (DataRow _row in resSet.Tables[1].Rows)
                        {
                            int orderDetID = 0;

                            #region Linking to Order Detail table

                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                            command.CommandType = CommandType.StoredProcedure;


                            int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                            OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;
                            if (salesOrderID > 0)
                            {
                                command.Parameters.AddWithValue("@p_OrderID", salesOrderID);

                                command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(_row["ProductID"].ToString()));
                                //if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNumber))
                                //{
                                //    command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                                //}
                                if (int.TryParse(_row["RequestedQty"].ToString(), out Quantity))
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                                }

                                if (int.TryParse(_row["RequestedBonusQty"].ToString(), out BonusOrdered))
                                {
                                    command.Parameters.AddWithValue("@p_BonusQuantity", BonusOrdered);
                                }
                                else
                                {
                                    command.Parameters.AddWithValue("@p_BonusQuantity", DBNull.Value);
                                }

                                command.Parameters.AddWithValue("@p_status", "Pending");
                                command.Parameters.AddWithValue("@p_comments", "Generated to Internal Pharmacy");
                                DataSet LinkResult = new DataSet();
                                SqlDataAdapter sA = new SqlDataAdapter(command);
                                sA.Fill(LinkResult);
                                if (LinkResult != null && LinkResult.Tables[0] != null)
                                {
                                    int.TryParse(LinkResult.Tables[0].Rows[0][0].ToString(), out orderDetID);
                                    //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                                }

                                if (orderDetID > 0)
                                {
                                    #region Populate Product Info

                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    command = new SqlCommand("Sp_FillSO_Details", connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);

                                    command.ExecuteNonQuery();

                                    #endregion
                                }
                            }
                            #endregion

                            DataView dv = resSet.Tables[2].DefaultView;
                            dv.RowFilter = "TransferDetailID = " + _row["TransferDetailID"].ToString();
                            DataTable dtDetail = dv.ToTable();

                            #region adding entry to salesOrder detail
                            foreach (DataRow _entryRow in dtDetail.Rows)
                            {
                                command = new SqlCommand("sp_EntrySaleOrderDetails_SODetails", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                                command.Parameters.AddWithValue("@p_ProductID", int.Parse(_entryRow["ProductID"].ToString()));
                                command.Parameters.AddWithValue("@p_StockID", int.Parse(_entryRow["StockID"].ToString()));
                                command.Parameters.AddWithValue("@p_CostPrice", _entryRow["CostPrice"]);
                                command.Parameters.AddWithValue("@p_SalePrice", _entryRow["SalePrice"]);
                                command.Parameters.AddWithValue("@p_Expiry", _entryRow["ExpiryDate"]);
                                command.Parameters.AddWithValue("@p_Batch", _entryRow["BatchNumber"]);
                                command.Parameters.AddWithValue("@p_SendQuantity", _entryRow["TransferredQty"]);
                                command.Parameters.AddWithValue("@p_BonusQuantity", _entryRow["DelieveredBonusQty"]);
                                command.Parameters.AddWithValue("@p_BarCode", _entryRow["Barcode"]);
                                command.Parameters.AddWithValue("@p_Discount", _entryRow["DiscountPercentage"]);
                                command.ExecuteNonQuery();
                            }
                            #endregion
                        }
                    }
                }
                else
                {
                    #region get Existing SO details
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    command = new SqlCommand("Sp_GetCompleteSalesOrderInfo", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    int systemID = 0;
                    DataSet soEntries = new DataSet(); 
                    command.Parameters.AddWithValue("@p_OrderID", salesOrderID);

                    if (int.TryParse(Session["UserSys"].ToString(), out systemID))
                    {
                        command.Parameters.AddWithValue("@p_SysID", systemID);
                    }
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(soEntries);
                    #endregion

                    foreach (DataRow _row in resSet.Tables[1].Rows)
                    {
                        string prodID = _row["ProductID"].ToString();

                        DataView soEntryDV = soEntries.Tables[1].DefaultView;
                        soEntryDV.RowFilter = "ProductID =" + prodID;
                        DataTable dtExisitingSOEntry = soEntryDV.ToTable();

                        if (dtExisitingSOEntry.Rows.Count == 0)
                        {
                            DataView dv = resSet.Tables[2].DefaultView;
                            dv.RowFilter = "TransferDetailID = " + _row["TransferDetailID"].ToString();
                            DataTable dtDetail = dv.ToTable();

                            DataView soDetailDV = soEntries.Tables[0].DefaultView;
                            soDetailDV.RowFilter = "ProductID =" + prodID;
                            DataTable dtSODetail = soDetailDV.ToTable();

                            foreach (DataRow _entryRow in dtDetail.Rows)
                            {
                                command = new SqlCommand("sp_EntrySaleOrderDetails_SODetails", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_OrderDetailID", int.Parse(dtSODetail.Rows[0]["OrderDetailID"].ToString()));
                                command.Parameters.AddWithValue("@p_ProductID", int.Parse(_entryRow["ProductID"].ToString()));
                                command.Parameters.AddWithValue("@p_StockID", int.Parse(_entryRow["StockID"].ToString()));
                                command.Parameters.AddWithValue("@p_CostPrice", _entryRow["CostPrice"]);
                                command.Parameters.AddWithValue("@p_SalePrice", _entryRow["SalePrice"]);
                                command.Parameters.AddWithValue("@p_Expiry", _entryRow["ExpiryDate"]);
                                command.Parameters.AddWithValue("@p_Batch", _entryRow["BatchNumber"]);
                                command.Parameters.AddWithValue("@p_SendQuantity", _entryRow["TransferredQty"]);
                                command.Parameters.AddWithValue("@p_BonusQuantity", _entryRow["DelieveredBonusQty"]);
                                command.Parameters.AddWithValue("@p_BarCode", _entryRow["Barcode"]);
                                command.Parameters.AddWithValue("@p_Discount", _entryRow["DiscountPercentage"]);
                                command.ExecuteNonQuery();
                            }

                            //removing row filters

                            //soDetailDV = soEntries.Tables[0].DefaultView;
                            //soDetailDV.RowFilter = string.Empty;
                            //dtSODetail = soEntryDV.ToTable();

                            //dv = resSet.Tables[2].DefaultView;
                            //dv.RowFilter = string.Empty;
                            //dtDetail = dv.ToTable();
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return salesOrderID;
        }
        private bool UpdateStockMinus(int TransferDetailID, int ProductID, int quantity, int Sent,int Bonus)
        {
            bool isSuccesFull = false;
            try
            {
                int x = 0;
                DataSet stockDet;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region Query stock
                SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
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
                        command.Parameters.AddWithValue("@p_DeliveredBonQty", DBNull.Value);
                        command.Parameters.AddWithValue("@p_RequestedBonusQty", Bonus);
                        command.Parameters.AddWithValue("@p_RequestedQty", dt.Rows[0]["RequestedQty"]);
                       
                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);

                        x = command.ExecuteNonQuery();

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
                command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_TransferDetail", TransferDetailID);
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
                        command = new SqlCommand("sp_EntryTransferDetails_Receive", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetailID);
                        command.Parameters.AddWithValue("@p_ProductID", dt.Rows[0]["ProductID"]);
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_CostPrice", dt.Rows[0]["UCostPrice"]);
                        command.Parameters.AddWithValue("@p_SalePrice", dt.Rows[0]["USalePrice"]);
                        command.Parameters.AddWithValue("@p_Expiry", dt.Rows[0]["ExpiryDate"]);
                        command.Parameters.AddWithValue("@p_Batch", dt.Rows[0]["BatchNumber"]);

                        if (stockSet.ContainsKey(id))
                        {
                            command.Parameters.AddWithValue("@p_TransferredQty", stockSet[id]);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_TransferredQty", 0);
                        }

                        command.Parameters.AddWithValue("@p_DeliveredBonQty", stockSetBonus[id]);
                        command.Parameters.AddWithValue("@p_RequestedBonusQty", Bonus);
                        command.Parameters.AddWithValue("@p_RequestedQty", dt.Rows[0]["RequestedQty"]);

                        command.Parameters.AddWithValue("@p_BarCode", dt.Rows[0]["BarCode"]);
                        command.ExecuteNonQuery();

                        command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_StockID", id);
                        command.Parameters.AddWithValue("@p_quantity", stockSetBonus[id]);
                        command.Parameters.AddWithValue("@p_Action", "Minus");
                        command.ExecuteNonQuery();
                    }
                }
                #endregion

                if (x == 1)
                {
                    isSuccesFull = true;
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
            return isSuccesFull;
        }
       

        protected void dgvReceiveTransfer_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                int TransferNo, TransferDetailNo, RequestedQty, requestedBonusQty, TransferedQty, transferedBonusQty, ReceivedQty, AvailableQty, ProductId, Discount;
                int LogedInStoreID;
                 int userID = Convert.ToInt32(Session["UserID"].ToString());
                int.TryParse(Session["UserSys"].ToString(), out LogedInStoreID);
                GridView dgvReceiveTransfer = (GridView)sender;
                Label lblTransferNo = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblRequestNo");
                Label lblTransferDetailsID = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblTransferDetailsID");
                Label lblRequestedQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblRequestedQty");
                Label lblAvailableQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblAvailableQty");
                Label lblSentQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSentQty");
                Label lblProductID = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblProductID");
                Label lblRequestedBonQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblReqBonQty");
                Label lblSentBonQty = (Label)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSentBonQty");
                TextBox txtRequestedFrom = (TextBox)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("RequestedFrom");

                int.TryParse(txtRequestedFrom.Text.ToString(), out Discount);
                int.TryParse(lblTransferNo.Text.ToString(), out TransferNo);
                int.TryParse(lblTransferDetailsID.Text.ToString(), out TransferDetailNo);
                int.TryParse(lblRequestedQty.Text.ToString(), out RequestedQty);
                int.TryParse(lblRequestedBonQty.Text.ToString(), out requestedBonusQty);

                int.TryParse(lblAvailableQty.Text.ToString(), out AvailableQty);
                int.TryParse(lblSentQty.Text.ToString(), out TransferedQty);
                int.TryParse(lblSentBonQty.Text.ToString(), out transferedBonusQty);
                int.TryParse(lblProductID.Text.ToString(), out ProductId);
                if (e.CommandName == "Edit")
                {
                    Session["TransferDetailID"] = TransferDetailNo;
                    Session["TransferedQty"] = RequestedQty;
                    Response.Redirect("ReceiveTransferDetails_ReceiveEntry.aspx");
                }
                if (e.CommandName == "AcceptProductTransfer")
                {
                    if (AvailableQty > 0 && (RequestedQty+requestedBonusQty <= AvailableQty)) 
                    {
                        bool isSuccessful = false;
                        //update transfer entry first
                        if (RequestedQty != TransferedQty || requestedBonusQty != transferedBonusQty)
                        {
                            if (TransferedQty == 0)
                            {
                                if (transferedBonusQty == 0)
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty, requestedBonusQty);
                                }
                                else
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty, transferedBonusQty);
                                }
                            }
                            else
                            {
                                if (transferedBonusQty == 0)
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty, requestedBonusQty);
                                }
                                else
                                {
                                    UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty, transferedBonusQty);
                                }
                            }
                        }
                       

                        
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }


                        SqlCommand command = new SqlCommand("sp_UpdateTransferOrderDetials_TransferDetialID", connection);
                        command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                        command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                        command.Parameters.AddWithValue("@p_RequestedQty", RequestedQty);
                        if (isSuccessful)
                        {
                            command.Parameters.AddWithValue("@p_TransferedQty", RequestedQty);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_TransferedQty", TransferedQty);
                        }
                        command.Parameters.AddWithValue("@p_TransferedBonusQty", requestedBonusQty);
                        command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                        command.Parameters.AddWithValue("@p_Status", "Accepted");
                        command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                        command.Parameters.AddWithValue("@p_ProductID", ProductId);
                        command.Parameters.AddWithValue("@p_Discount", Discount);
                        command.Parameters.AddWithValue("@p_TransferToUserID", userID);
                        command.CommandType = CommandType.StoredProcedure;
                        command.ExecuteNonQuery();

                        Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                        btnAccept.Visible = false;
                        Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                        btnDeny.Visible = false;
                        Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                        btnEdit.Visible = false;
                        HtmlGenericControl btnStaticAccepted = (HtmlGenericControl)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnStaticAccepted");
                        btnStaticAccepted.Visible = true;  
                        //if (RequestedQty != TransferedQty)
                        //{
                        //    if (TransferedQty == 0)
                        //    {
                        //        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, RequestedQty);
                        //    }
                        //    else
                        //    {
                        //        UpdateStockMinus(TransferDetailNo, ProductId, AvailableQty, TransferedQty);
                        //    }
                        //}
                    }
                    else 
                    {
                        WebMessageBoxUtil.Show("No Stock Available");
                    }
                }
                if (e.CommandName == "DenyProductTransfer")
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_DenyTransferRequest_TransferDetialID", connection);
                    command.Parameters.AddWithValue("@p_TransferID", TransferNo);
                    command.Parameters.AddWithValue("@p_TransferDetID", TransferDetailNo);
                    command.Parameters.AddWithValue("@p_RequestedTransferedQty", TransferedQty);
                    command.Parameters.AddWithValue("@p_ReceivedQty", 0);
                    command.Parameters.AddWithValue("@p_AvailableQty", AvailableQty);
                    command.Parameters.AddWithValue("@p_Status", "Denied");
                    command.Parameters.AddWithValue("@p_LogedinnStore", LogedInStoreID);
                    command.Parameters.AddWithValue("@p_ProductID", ProductId);

                    command.CommandType = CommandType.StoredProcedure;
                    command.ExecuteNonQuery();


                    Button btnAccept = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnAccept");
                    btnAccept.Visible = false;
                    Button btnDeny = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnDeny");
                    btnDeny.Visible = false;
                    Button btnEdit = (Button)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("btnEdit");
                    btnEdit.Visible = false;

                    HtmlGenericControl lblStaticDeny = (HtmlGenericControl)dgvReceiveTransfer.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblStaticDeny");
                    lblStaticDeny.Visible = true;  

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
                LoadRepeater();
            }

        }

        public Boolean IsStatusComplete(String status)
        {
            if (status.Equals("Accepted") || status.Equals("Denied"))
            {
                return true;
            }
            else
                return false;
        }
        protected Boolean IsStatusAccepted(String status)
        {
            if (status.Equals("Accepted"))
            {
                return true;
            }
            else
                return false;
        } 
        protected Boolean IsStatusDenied(String status)
        {
            if (status.Equals("Denied"))
            {
                return true;
            }
            else
                return false;
        }
        protected void dgvReceiveTransfer_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            //if (e.Row.RowType == DataControlRowType.DataRow)
            //{
            //    Button abc = (Button)e.Row.FindControl("btnAccept");
            //    if (dsStatic.Tables[0].Rows[e.Row.RowIndex]["TransferStatus"].ToString() == "Accepted")
            //    {
            //        GridView gv = (GridView)sender;
            //        Button btnAccept = (Button)e.Row.FindControl("btnAccept");
            //        btnAccept.Visible = false;
            //        Button btnDeny = (Button)e.Row.FindControl("btnDeny");
            //        btnDeny.Visible = false;
            //        Button btnEdit = (Button)e.Row.FindControl("btnEdit");
            //        btnEdit.Visible = false;
            //        Button btnStaticAccepted = (Button)e.Row.FindControl("btnStaticAccepted");
            //        btnStaticAccepted.Visible = true;
            //    }
            //}


        }

        
    }
}