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

namespace IMS
{
    public partial class OrderSalesManual : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public DataSet ProductSet;
        public  DataSet systemSet;
        public  bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                txtIvnoice.Text = "SO-" + DateTime.Now.TimeOfDay.Hours + "_" + DateTime.Now.TimeOfDay.Minutes;
                txtIvnoice.Enabled = false;
                if (Session["OrderSalesDetail"] != null && Session["OrderSalesDetail"].Equals(true))
                {
                    FirstOrder = true;
                    systemSet = new DataSet();
                    ProductSet = new DataSet();
                    LoadData();
                    btnAccept.Visible = true;
                    btnDecline.Visible = true;
                    #region Populating System Types
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select * From tbl_System WHERE System_RoleID =2;", connection); // needs to be completed
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
                            StockAt.SelectedValue = Session["SystemID"].ToString();
                            StockAt.Enabled = false;
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

                    if (Session["Invoice"] != null)
                    {
                        txtIvnoice.Text = Session["Invoice"].ToString();
                        txtIvnoice.Enabled = false;
                    }
                    BindGrid();
                }
                else
                {
                    FirstOrder = false;
                    Session["OrderSalesDetail"] = false;
                    Session["ExistingOrder"] = false;
                    systemSet = new DataSet();
                    ProductSet = new DataSet();
                    #region Populating System Types
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Select * From tbl_System WHERE System_RoleID =2;", connection); // needs to be completed
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

                    }
                    finally
                    {
                        connection.Close();
                    }
                    #endregion
                    LoadData();
                }
            }
        }
        private void LoadData()
        {

            #region Populating Product List

            //try
            //{
            //    connection.Open();
            //    SqlCommand command = new SqlCommand("Select  Top 200 Product_Name,ProductID From tbl_ProductMaster Where tbl_ProductMaster.Product_Id_Org LIKE '444%' AND Status = 1", connection);
            //    DataSet ds = new DataSet();
            //    SqlDataAdapter sA = new SqlDataAdapter(command);
            //    sA.Fill(ds);
            //    ProductSet = ds;
            //    SelectProduct.DataSource = ds.Tables[0];
            //    SelectProduct.DataTextField = "Product_Name";
            //    SelectProduct.DataValueField = "ProductID";
            //    SelectProduct.DataBind();
            //    if (SelectProduct != null)
            //    {
            //        SelectProduct.Items.Insert(0, "Select Product");
            //        SelectProduct.SelectedIndex = 0;
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

        private void UpdateStockMinus(int orderDetailID, int quantity)
        {
            try
            {
                DataSet stockDet;
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetStockBy_OrderDetID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                Dictionary<int, int> stockSet = new Dictionary<int, int>();

                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (quantity > 0)
                    {
                        if (exQuan >= quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), quantity);
                            break;
                        }
                        else if (exQuan < quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            quantity = quantity - exQuan;
                        }
                    }
                }


                foreach (int id in stockSet.Keys)
                {
                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", id);
                    command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    command.Parameters.AddWithValue("@p_Action", "Minus");
                    command.ExecuteNonQuery();

                    #region Generation of Packing List
                    //command = new SqlCommand("sp_PackingListGeneration", connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //command.Parameters.AddWithValue("@p_StockID", id);
                    //command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    //command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                    //command.ExecuteNonQuery();
                    #endregion
                }

            }
            catch (Exception exp) { }
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
                DataSet stockDet;
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetStockBy_OrderDetID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                Dictionary<int, int> stockSet = new Dictionary<int, int>();

                foreach (DataRow row in stockDet.Tables[0].Rows)
                {
                    int exQuan = int.Parse(row["Quantity"].ToString());
                    if (quantity > 0)
                    {
                        if (exQuan >= quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), quantity);
                            break;
                        }
                        else if (exQuan < quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            quantity = quantity - exQuan;
                        }
                    }
                }


                foreach (int id in stockSet.Keys)
                {
                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", id);
                    command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    command.Parameters.AddWithValue("@p_Action", "Plus");
                    command.ExecuteNonQuery();

                    #region Deletetion of Packing List
                    /*command = new SqlCommand("sp_PackingListGeneration", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", id);
                    command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                    command.ExecuteNonQuery();*/
                    #endregion
                }

            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
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

            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
        protected void btnAccept_Click(object sender, EventArgs e)
        {
            //for (int i = 0; i < ProductSet.Tables[0].Rows.Count; i++)
            //{
            //    int  OderDetailID = Convert.ToInt32(ProductSet.Tables[0].Rows[i]["OrderDetailID"].ToString());
            //    int  Quantity = Convert.ToInt32(ProductSet.Tables[0].Rows[i]["Qauntity"].ToString());

            //    Session["RequestedNO"] = Convert.ToInt32(ProductSet.Tables[0].Rows[i]["OrderID"].ToString());

            //    UpdateStockMinus(OderDetailID, Quantity);
            //}
            //
            DataSet dsProducts = (DataSet)Session["dsProducts"];

            Session["RequestedNO"] = Convert.ToInt32(dsProducts.Tables[0].Rows[0]["OrderID"].ToString());
           // Session["RequestedNO"] = Convert.ToInt32(ProductSet.Tables[0].Rows[0]["OrderID"].ToString());
            if (Session["ExistingOrder"].Equals(true)) { Session["RequestedFromID"] = Session["SystemID"]; }
            Response.Redirect("ViewPackingList_SO.aspx", false);
            //Response.Redirect("PO_GENERATE.aspx", false);
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            try
            {
                if (Session["OrderNumber"] != null)
                {
                    int orderID = int.Parse(Session["OrderNumber"].ToString());
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
                Session["OrderNumber"] = null;
                Session["OrderSalesDetail"] = "false";
                txtProduct.Text = "";
                SelectProduct.Visible = false;
                StockAt.Enabled = true;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataBind();
                SelectQuantity.Text = "";
                SelectProduct.SelectedIndex = -1;
                StockAt.SelectedIndex = -1;
                btnAccept.Visible = false;
                btnDecline.Visible = false;
                FirstOrder = false;
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
                if (e.CommandName.Equals("UpdateStock"))
                {
                    int quan = int.Parse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity")).Text);
                    int availablestock = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblAvStock")).Text);
                    int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo")).Text);
                    if (quan <= availablestock)
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_UpdateSODetailsQuantity", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                        command.Parameters.AddWithValue("@p_Qauntity", quan);

                        command.ExecuteNonQuery();
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("Entered Amount Exceed Available Stock, cannot be updated");
                    }
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    long orderDetID = long.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("OrderDetailNo")).Text);
                    connection.Open();

                    if (Session["OrderSalesDetail"] != null && Session["OrderSalesDetail"].Equals(true))
                    {
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
                    }

                    SqlCommand command = new SqlCommand("sp_DeleteSO_ID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);

                    command.ExecuteNonQuery();
                }
                else if (e.CommandName.Equals("Details"))
                {
                    int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("OrderDetailNo")).Text);
                    int ProductID = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblProductID")).Text);
                    int quan = int.Parse(((Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblQuantity")).Text);
                    Session["OderDetailID"] = orderDetID;
                    Session["ProductID"] = ProductID;
                    Session["TotalQuantity"] = quan;
                    Session["SelectedIndex"] = StockAt.SelectedIndex;
                    Session["Invoice"] = txtIvnoice.Text;
                    Response.Redirect("OrderSalesManual_Details.aspx");
                }
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
            }
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("OrderDetailNo")).Text);
                connection.Open();
                SqlCommand command = new SqlCommand("sp_DeleteSO_ID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);


                command.ExecuteNonQuery();
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
                StockDisplayGrid.EditIndex = -1;
                BindGrid();
            }
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void btnCreateOrder_Click(object sender, EventArgs e)
        {
         
            if (txtProduct.Text.Length < 3)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Please enter at least three words to search product')", true);
                return; 
            }
            btnAccept.Visible = true;
            btnDecline.Visible = true;
            int RemainingStock = 0;
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_getStock_Quantity", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_ProductID", Convert.ToInt32(SelectProduct.SelectedValue.ToString()));
                DataSet QuantitySet = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(QuantitySet);
                RemainingStock = Convert.ToInt32(QuantitySet.Tables[0].Rows[0][0].ToString());
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }

            if (Convert.ToInt32(SelectQuantity.Text) <= RemainingStock)
            {
                if (FirstOrder.Equals(false))
                {
                    #region Creating Order

                    int pRequestFrom = 0;
                    int pRequestTo = 0;
                    String OrderMode = "";
                    int OrderType = 3;//incase of vendor this should be 3

                    OrderMode = "Sales";

                    String Invoice = txtIvnoice.Text;
                    String Vendor = "True";


                    try
                    {
                        connection.Open();
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

                        command.Parameters.AddWithValue("@p_OrderType", OrderType);
                        command.Parameters.AddWithValue("@p_Invoice", Invoice);
                        command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                        command.Parameters.AddWithValue("@p_Vendor", Vendor);
                        command.Parameters.AddWithValue("@p_orderStatus", "Pending");
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
                        connection.Close();
                    }
                    #endregion

                    #region Linking to Order Detail table

                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                        command.CommandType = CommandType.StoredProcedure;


                        int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                        OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

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
                        else
                        {
                            command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@p_status", "Pending");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Outside Store");
                        DataSet LinkResult = new DataSet();
                        SqlDataAdapter sA = new SqlDataAdapter(command);
                        sA.Fill(LinkResult);
                        if (LinkResult != null && LinkResult.Tables[0] != null)
                        {
                            Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
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

                    FirstOrder = true;
                }
                else
                {
                    #region Product Existing in the Current Order
                    DataSet ds = new DataSet();
                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_GetOrderbyOutside", connection);
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
                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                            command.CommandType = CommandType.StoredProcedure;

                            int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                            OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

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
                            else
                            {
                                command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                            }


                            command.Parameters.AddWithValue("@p_status", "Pending");
                            command.Parameters.AddWithValue("@p_comments", "Generated to Outside Store");

                            DataSet LinkResult = new DataSet();
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(LinkResult);
                            if (LinkResult != null && LinkResult.Tables[0] != null)
                            {
                                Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
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
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Remaining Stock is less then the entered quantity, please enter less quantity to proceed')", true);
            }

            // needs to check this //
            #region Populate Product Info
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_FillSO_Details", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber,DetailID = 0;
                if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                    //command.ExecuteNonQuery();
                }
                if (int.TryParse(Session["DetailID"].ToString(), out DetailID))
                {
                    command.Parameters.AddWithValue("@p_OrderDetailID", DetailID);
                   
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

            BindGrid();
            txtProduct.Text = "";
            SelectProduct.Visible = false;
            StockAt.Enabled = false;
            SelectQuantity.Text = "";
            
            SelectProduct.SelectedIndex = -1;
        }


        private void BindGrid()
        {
            DataSet ds = new DataSet();

            #region Display Products
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetSODetails_ID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;
                

                if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }

                Session["dsProducts"] = ds;

                

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
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            //txtVendor.Text = "";
            //RequestTo.Visible = false;
            //RequestTo.Enabled = true;
            //StockDisplayGrid.DataSource = null;
            //StockDisplayGrid.DataBind();
            //SelectQuantity.Text = "";
            //SelectProduct.SelectedIndex = -1;
            //RequestTo.SelectedIndex = -1;
            //btnAccept.Visible = false;
            //btnDecline.Visible = false;
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            
            //must be checked for sessions
            if (Session["OrderSalesDetail"].Equals(true) && Session["OrderSalesDetail"].ToString() != null && Session["OrderSalesDetail"] != null)
            {
                Session["OrderNumber"] = "";
                Session["OrderSalesDetail"] = false;
                Response.Redirect("ViewSalesOrders.aspx", false); //must be rechecked
            }
            else
            {
                try
                {
                    if (Session["OrderNumber"] != null)
                    {
                        int orderID = int.Parse(Session["OrderNumber"].ToString());
                        connection.Open();

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
                            command2.Parameters.AddWithValue("@p_Action", "Plus");
                            command2.ExecuteNonQuery();
                        }

                        command = new SqlCommand("sp_DeleteSO", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderID", orderID);
                        command.ExecuteNonQuery();
                    }
                    Session["OrderNumber"] = null;
                    Session["FromViewPlacedOrders"] = "false";
                    txtProduct.Text = "";
                    SelectProduct.Visible = false;
                    StockAt.Enabled = true;
                    StockDisplayGrid.DataSource = null;
                    StockDisplayGrid.DataBind();
                    SelectQuantity.Text = "";
                    SelectProduct.SelectedIndex = -1;
                    StockAt.SelectedIndex = -1;
                    btnAccept.Visible = false;
                    btnDecline.Visible = false;
                    FirstOrder = false;
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
                Session["OrderNumber"] = "";
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
                connection.Open();

                Text = Text + "%";
                SqlCommand command = new SqlCommand("SELECT Distinct tbl_ProductMaster.* From tbl_ProductMaster INNER JOIN tblStock_Detail ON tbl_ProductMaster.ProductID = tblStock_Detail.ProductID Where tbl_ProductMaster.Product_Name LIKE '" + Text + "'", connection);
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
                //ds.Tables[0].Columns.Add("ProductInfo", typeof(string), "Product_Name+ ' '+itemStrength+' '+itemPackSize+' '+itemForm");

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
                    btnDetail.Enabled = false;
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

        
    }
}