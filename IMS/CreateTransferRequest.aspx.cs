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

namespace IMS.StoreManagement.StoreRequests
{
    public partial class CreateTransferRequest : System.Web.UI.Page
    {
        public DataTable dtTransfer = new DataTable();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataTable dtStatic;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                dtStatic = new DataTable();
                Session["FirstOrderTransfer"] = false;

                dtStatic.Columns.Add("ProductID");
                dtStatic.Columns.Add("SystemID");
                dtStatic.Columns.Add("Product_Name");
                dtStatic.Columns.Add("RequestedFrom");
                dtStatic.Columns.Add("RequestedTo");
                dtStatic.Columns.Add("TransferedQty");

            }
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
            Session["Text"] = Text;
            ProductsPopupGrid.PopulateStoreUserGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void btnSelectStore_Click(object sender, EventArgs e)
        {
            Session["ProductId"] = lblProductId.Text;

            StoresPopupGrid.PopulateStoreWithStock();
            mpeStoresPopupDiv.Show();
        }

        protected void btnAddRequest_Click(object sender, EventArgs e)
        {
          
            dtStatic.Rows.Add(lblProductId.Text, lblStoreId.Text, txtSearch.Text, Session["UserSys"].ToString(), txtStore.Text, txtTransferredQty.Text);
            dgvCreateTransfer.DataSource = dtStatic;
            dgvCreateTransfer.DataBind();
            Session["TransferRequestGrid"] = dtStatic;
            txtSearch.Text = "";
            txtStore.Text = "";
            txtTransferredQty.Text = "";
        }

        protected void btnGenerateRequest_Click(object sender, EventArgs e)
        {
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
                        int.TryParse(drDetails["TransferedQty"].ToString(), out TransferredQty);
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
                                command.Parameters.AddWithValue("@p_SysID", Convert.ToInt32(Session["UserSys"].ToString()));


                                DataSet QuantitySet = new DataSet();
                                SqlDataAdapter sA = new SqlDataAdapter(command);
                                sA.Fill(QuantitySet);
                                RemainingStock = Convert.ToInt32(QuantitySet.Tables[0].Rows[0][0].ToString());

                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                connection.Close();

                            }
                            if (Session["FirstOrderTransfer"].Equals(false))
                            {

                                #region  Creating Transfer Order


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

                                    command.Parameters.AddWithValue("@p_TransferStatus", "Initiate");
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

                                }
                                finally
                                {
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

                                    if (int.TryParse(drDetails["TransferedQty"].ToString(), out Quantity))
                                    {
                                        command.Parameters.AddWithValue("@p_TransferedQty", Quantity);
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@p_TransferedQty", DBNull.Value);
                                    }


                                    command.Parameters.AddWithValue("@p_TransferStatus", "Initiate");

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

                                }
                                finally
                                {
                                    connection.Close();
                                }
                                #endregion


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

                                }
                                finally
                                {
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

                                        if (int.TryParse(drDetails["TransferedQty"].ToString(), out Quantity))
                                        {
                                            command.Parameters.AddWithValue("@p_TransferedQty", Quantity);
                                        }
                                        else
                                        {
                                            command.Parameters.AddWithValue("@p_TransferedQty", DBNull.Value);
                                        }


                                        command.Parameters.AddWithValue("@p_TransferStatus", "Initiate");

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

                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                    #endregion



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
               
               
                //Session["StoreId"] = lblStoreId.Text.ToString();

                //BindGrid();
                //txtSearch.Text = "";
                //txtStore.Text = "";
                //txtTransferredQty.Text = "";
            }


            //DataSet dsTransfer = (DataSet)Session["dsTransfer"];
            //Session["TransferNo"] = dsTransfer.Tables[0].Rows[0]["TransferID"].ToString();
            //Session["FirstOrderTransfer"] = false;

            Response.Redirect("GenerateTransferRequest.aspx", false);
        }
         

        protected void dgvCreateTransfer_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvCreateTransfer.PageIndex = e.NewPageIndex;
            BindGrid();
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
            catch
            {

            }
            finally
            {
                connection.Close();
                BindGrid();
            }
        }
            //    DataSet stockDet;
            //    if (connection.State == ConnectionState.Closed)
            //    {
            //        connection.Open();
            //    }


                
            //    Label lblTransferDetailID = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("TransferDetailID");
            //    int TansferDetailID = 0;
            //    int.TryParse(lblTransferDetailID.Text.ToString(), out TansferDetailID);

            //    Label lblSystemID = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("lblSystemID");
            //    int StoredAt = 0;
            //    int.TryParse(lblSystemID.Text.ToString(), out StoredAt);

            //    Label lblProductID = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("lblProductID");
            //    int ProductID = 0;
            //    int.TryParse(lblProductID.Text.ToString(), out ProductID);

            //    Label lblTransferedQty = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("TransferedQty");
            //    int TransferedQty = 0;
            //    int.TryParse(lblTransferedQty.Text.ToString(), out TransferedQty);


            //    #region Query stock
            //    SqlCommand command = new SqlCommand("sp_GetStockBy_TransferDetail", connection);
            //    command.CommandType = CommandType.StoredProcedure;
            //    command.Parameters.AddWithValue("@p_TransferDetail", TansferDetailID);
            //    command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

            //    DataSet ds = new DataSet();
            //    SqlDataAdapter dA = new SqlDataAdapter(command);
            //    dA.Fill(ds);
            //    stockDet = ds;
            //    #endregion

            //    #region Update Stock (Add Stock back to store)

            //    //command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
            //    //command.CommandType = CommandType.StoredProcedure;
            //    //command.Parameters.AddWithValue("@p_StockID", int.Parse(stockDet.Tables[0].Rows[0]["StockID"].ToString()));
            //    //command.Parameters.AddWithValue("@p_quantity", TransferedQty);
            //    //command.Parameters.AddWithValue("@p_Action", "Add");
            //    //command.ExecuteNonQuery();

            //    #endregion

            //    if (connection.State == ConnectionState.Closed)
            //    {
            //        connection.Open();
            //    }
            //    SqlCommand sqCommmand = new SqlCommand("sp_DeleteFromtblTransferDetails_TransferReceive", connection);
            //    sqCommmand.CommandType = CommandType.StoredProcedure;
            //    sqCommmand.Parameters.AddWithValue("@P_TransferDetailsId", TansferDetailID);
            //    sqCommmand.ExecuteNonQuery();

            //    BindGrid();

      

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
                    //int rowindex = Convert.ToInt32(e.CommandArgument.ToString());
                    //int PrevtransferdQty = 0;
                    //int.TryParse(((Label)dgvCreateTransfer.Rows[rowindex].FindControl("TransferedQty")).Text, out PrevtransferdQty);
                    //Session["PrevtransferdQty"] = PrevtransferdQty;
                }
                else if (e.CommandName == "UpdateStock")
                {
                    int rowindex = dgvCreateTransfer.EditIndex;

                    //Label lblTransferDetailID = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("TransferDetailID");
                    //int TansferDetailID = 0;
                    //int.TryParse(lblTransferDetailID.Text.ToString(), out TansferDetailID);

                    //Label lblProductID = (Label)dgvCreateTransfer.Rows[rowindex].FindControl("lblProductID");
                    //int ProductID = 0;
                    //int.TryParse(lblProductID.Text.ToString(), out ProductID);

                    TextBox tbTransferedQty = (TextBox)dgvCreateTransfer.Rows[rowindex].FindControl("txtTransferQty");
                    int TransferedQty = 0;
                    int.TryParse(tbTransferedQty.Text.ToString(), out TransferedQty);

                    dtStatic.Rows[0]["TransferedQty"] = TransferedQty;
                    dtStatic.AcceptChanges();
                     

                    #region remove entry from sales order receiving
                    //connection.Open();
                    //SqlCommand command = new SqlCommand();
                    //command = new SqlCommand("sp_DeleteTransferDetails", connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@p_TransferDetailID", TansferDetailID);
                    //command.Parameters.AddWithValue("@p_ProductID", ProductID);
                    //command.ExecuteNonQuery();
                    //#endregion

                    //#region Update entry in sales order detail
                    //command = new SqlCommand();
                    //command = new SqlCommand("sp_UpdateTransferDetails", connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@p_TransferDetailID", TansferDetailID);
                    //command.Parameters.AddWithValue("@p_TransferedQuantity", TransferedQty);

                    //command.ExecuteNonQuery();



                    #endregion
                }
            }
            catch
            {

            }
            finally
            {
                //connection.Close();
                dgvCreateTransfer.EditIndex = -1;
                BindGrid();
            }
        }
    }
}