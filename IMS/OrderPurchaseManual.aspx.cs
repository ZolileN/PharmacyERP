﻿using System;
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
using System.Web.Services;
using System.Collections;
using IMSCommon.Util;

namespace IMS
{
    public partial class OrderPurchaseManual : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            
            if (!IsPostBack)
            {
                
                if ((Session["OrderNumber"] != null && Session["FromViewPlacedOrders"] != null)) //|| Session["isGenOption"] != null)
                {
                    Session["FirstOrder"] = true;
                   
                    systemSet = new DataSet();
                    ProductSet = new DataSet();
                    LoadData();
                    BindGrid();
                    if (StockDisplayGrid.DataSource != null)
                    {
                        btnAccept.Visible = true;
                        btnAccept.Text = "RE-GENERATE ORDER";
                        btnDecline.Visible = true;
                    }
                    Session["FromViewPlacedOrders"] = null;
                    //Session["isGenOption"] = null;
                }
                else
                {
                    Session["OrderNumber"] = "";
                    Session["FromViewPlacedOrders"] = null;
                    Session["FirstOrder"] = false;
                    //Session["isGenOption"] = null;
                   
                    systemSet = new DataSet();
                    ProductSet = new DataSet();
                    LoadData();
                }
                if (Session["VendorId"] != null)
                {
                    CmbVendors.SelectedValue = Session["VendorId"].ToString();
                }

            }
             
            
            
        }
        private void LoadData()
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

                //CmbVendors.DataSource = ds.Tables[0];
                //CmbVendors.DataTextField = "SupName";
                //CmbVendors.DataValueField = "SuppID";
                //CmbVendors.DataBind();
                //if (CmbVendors != null)
                //{
                //    CmbVendors.Items.Insert(0, "Select Vendor");
                //    CmbVendors.SelectedIndex = 0;
                //}
                
                //RequestTo.DataSource = ds.Tables[0];
                //RequestTo.DataTextField = "SupName";
                //RequestTo.DataValueField = "SuppID";
                //RequestTo.DataBind();
                //if (RequestTo != null)
                //{
                //    RequestTo.Items.Insert(0, "Select Vendor");
                //    RequestTo.SelectedIndex = 0;
                //}
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion

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


        
        
        protected void btnAccept_Click(object sender, EventArgs e)
        {

           
            Response.Redirect("PO_GENERATE.aspx", false);
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet delDs = new DataSet();
                if (Session["OrderNumber"] != null)
                {
                    int orderID = int.Parse(Session["OrderNumber"].ToString());
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    //fetch order detail entries
                    SqlCommand command = new SqlCommand("Sp_GetPOReceiveEntry_byOMID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(delDs);
                    for (int i = 0; i < delDs.Tables[0].Rows.Count; i++)
                    {

                       //remove entries and remove values from stock
                        command = new SqlCommand("Sp_RemovePOEntries", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@p_OrderDetailID", delDs.Tables[0].Rows[i]["orderDetailID"].ToString());
                        command.Parameters.AddWithValue("@p_expiryOriginal", delDs.Tables[0].Rows[i]["ExpiryDate"].ToString());
                        command.Parameters.AddWithValue("@p_entryID", delDs.Tables[0].Rows[i]["entryID"].ToString());
                        command.Parameters.AddWithValue("@p_ProductID", delDs.Tables[0].Rows[i]["ProductID"].ToString());
                        command.Parameters.AddWithValue("@p_StoreID", delDs.Tables[0].Rows[i]["OrderRequestBy"].ToString());

                        command.ExecuteNonQuery();
                    }
                    //delete order master and order detail
                    command = new SqlCommand("sp_DeleteOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);
                    command.ExecuteNonQuery();
                }
                Session["OrderNumber"] = null;
                Session["FromViewPlacedOrders"] = null;
                txtVendor.Text = "";
                txtProduct.Text = "";
                txtVendor.Enabled = true;
                SelectProduct.Visible = false;
                CmbVendors.Enabled = true;

                RequestTo.Visible = false;
                RequestTo.Enabled = true;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataBind();
                SelectQuantity.Text = "";
                SelectProduct.SelectedIndex = -1;
                RequestTo.SelectedIndex = -1;
                btnAccept.Visible = false;
                btnDecline.Visible = false;
                Session["FirstOrder"] = false;
               // Session["isGenOption"] = null;
                lblttlcst.Visible = false;
                lblTotalCostALL.Visible = false;
                WebMessageBoxUtil.Show("Order and stock has been successfully removed");
            }
            catch(Exception ex)
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
                    int quan, bonusquantity,quanOrg;
                    quan = bonusquantity = quanOrg = 0;
                    int.TryParse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity")).Text,out quan);
                    int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo")).Text);
                    int.TryParse(((TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtBonusQuantity")).Text, out bonusquantity);
                    int.TryParse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblQuantityOrg")).Text, out quanOrg);
                    string status = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStatus")).Text;
                    if (quan + bonusquantity > 0)
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_UpdateOrderDetailsQuantity", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                        command.Parameters.AddWithValue("@p_Qauntity", quan);
                        command.Parameters.AddWithValue("@p_bonusQauntity", bonusquantity);

                        command.ExecuteNonQuery();

                        if (!status.Equals("Pending"))
                        {
                            command = new SqlCommand("Sp_UpdateOrderStatus", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
                            command.Parameters.AddWithValue("@p_Status", "Partial");
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        WebMessageBoxUtil.Show("Both ordered quantities cannot be 0");
                    }
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int orderDetID = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo")).Text);
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_DeleteOrderDetailsbyID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);

                    command.ExecuteNonQuery();
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
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_DeleteOrderDetailsbyID", connection);
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
            int quan, bQuan;
            quan = bQuan = 0;
            int.TryParse(SelectQuantity.Text.ToString(), out quan);
            int.TryParse(SelectPrice.Text.ToString(), out bQuan);
            if (quan + bQuan > 0)
            {
                btnAccept.Visible = true;
                btnDecline.Visible = true;
                if (Session["FirstOrder"].Equals(false))
                {
                    #region Creating Order

                    int pRequestFrom = 0;
                    int pRequestTo = 0;
                    String OrderMode = "";
                    int OrderType = 3;//incase of vendor this should be 3

                    if (CmbVendors.SelectedItem.ToString().Contains("store")) // neeed to check it, because name doesn't always contains Store
                    {
                        OrderMode = "Store";
                    }
                    else if (CmbVendors.SelectedItem.ToString().Contains("warehouse"))
                    {
                        OrderMode = "Warehouse";
                    }
                    else
                    {
                        OrderMode = "Vendor";
                    }

                    String Invoice = "";
                    String Vendor = "True";


                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_CreateOrder", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        //sets vendor

                        if (int.TryParse(CmbVendors.SelectedValue.ToString(), out pRequestTo))
                        {
                            command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                        }

                        //if (!string.IsNullOrEmpty(Session["VendorName"].ToString()))
                        //{
                        //    if (int.TryParse(Session["VendorId"].ToString(), out pRequestTo))
                        //    {
                        //        command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                        //    }
                        //}
                        //else
                        //{
                        //    if (int.TryParse(CmbVendors.SelectedValue.ToString(), out pRequestTo))
                        //    {
                        //        command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                        //    }
                        //}

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
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        string ProductNumber = "";
                        int OrderNumber, BonusOrdered, Quantity;
                        OrderNumber = BonusOrdered = Quantity = 0;

                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        }
                        command.Parameters.AddWithValue("@p_ProductID", txtSearch.Value.ToString());
                        if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                        {
                            command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                        }
                        if (int.TryParse(SelectPrice.Text.ToString(), out BonusOrdered))
                        {
                            command.Parameters.AddWithValue("@p_OrderBonusQuantity", BonusOrdered);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_OrderBonusQuantity", DBNull.Value);
                        }

                        command.Parameters.AddWithValue("@p_status", "Pending");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Vendor");

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

                    Session["FirstOrder"] = true;
                }
                else
                {
                    #region Product Existing in the Current Order
                    DataSet ds = new DataSet();
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_GetOrderbyVendor", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        int OrderNumber = 0;


                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(ds);

                            int ProductNO = 0;
                            bool ProductPresent = false;
                            command.Parameters.AddWithValue("@p_ProductID", txtSearch.Value.ToString());
                            //if (int.TryParse(SelectProduct.SelectedValue.ToString(), out ProductNO))
                            //{
                            //}

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]).Equals(ProductNO))
                                {
                                    ProductPresent = true;
                                    break;
                                }
                            }

                            if (ProductPresent.Equals(false))
                            {
                                #region Linking to Order Detail table

                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    int BonusOrdered, ProductNumber, Quantity;
                                    OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

                                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                                    {
                                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                                    }

                                    command.Parameters.AddWithValue("@p_ProductID", txtSearch.Value.ToString());

                                    if (int.TryParse(SelectQuantity.Text.ToString(), out Quantity))
                                    {
                                        command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@p_OrderQuantity", DBNull.Value);
                                    }
                                    if (int.TryParse(SelectPrice.Text.ToString(), out BonusOrdered))
                                    {
                                        command.Parameters.AddWithValue("@p_OrderBonusQuantity", BonusOrdered);
                                    }
                                    else
                                    {
                                        command.Parameters.AddWithValue("@p_OrderBonusQuantity", DBNull.Value);
                                    }

                                    command.Parameters.AddWithValue("@p_status", "Pending");
                                    command.Parameters.AddWithValue("@p_comments", "Generated to Vendor");

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
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record can not be inserted, because it is already present')", true);
                            }
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
                #region Populate Product Info
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("Sp_FillPO_Details", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    int OrderNumber = 0;
                    command.Parameters.AddWithValue("@p_OrderDetailID", DBNull.Value);
                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        command.ExecuteNonQuery();
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
                BindGrid();
                txtVendor.Text = "";
                txtProduct.Text = "";
                SelectProduct.Visible = false;
                txtSearch.Value = "";
                // RequestTo.Visible = false;
                SelectQuantity.Text = "";
                SelectPrice.Text = "";
                txtVendor.Enabled = false;
                SelectProduct.SelectedIndex = -1;
                //RequestTo.SelectedIndex = -1;
            }
            else
            {
                WebMessageBoxUtil.Show("Both quantity and bonus quantity cannot be 0");
                return;
            }
        }


        private void BindGrid() 
        {
            #region Display Products
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetOrderbyVendor", connection);
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

                float TCost = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    float Cost = 0;
                    if (float.TryParse(ds.Tables[0].Rows[i]["totalDiscountPrice"].ToString(), out Cost))
                    {
                        TCost += Cost;
                    }
                }
                lblttlcst.Visible = true;
                lblTotalCostALL.Visible = true;
                lblTotalCostALL.Text = TCost.ToString();
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
            txtVendor.Text = "";
            CmbVendors.Enabled = true;
            CmbVendors.SelectedIndex = -1;
            RequestTo.Visible = false;
            RequestTo.Enabled = true;
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
            SelectQuantity.Text = "";
            SelectProduct.SelectedIndex = -1;
            RequestTo.SelectedIndex = -1;
            btnAccept.Visible = false;
            btnDecline.Visible = false;
            lblttlcst.Visible = false;
            lblTotalCostALL.Visible = false;
        }

        protected void btnCancelOrder_Click(object sender, EventArgs e)
        {
            lblttlcst.Visible = false;
            lblTotalCostALL.Visible = false;
            //must be checked for sessions
            if (Session["FromViewPlacedOrders"] !=null && Session["FromViewPlacedOrders"].ToString().Equals("true") && Session["FromViewPlacedOrders"].ToString() != null)
            {
                Session["OrderNumber"] = "";
                Session["FromViewPlacedOrders"] = null;
                Session["FirstOrder"] = false;
               // Session["isGenOption"] = null;
                Response.Redirect("ViewPlacedOrders.aspx", false);
            }
            else
            {
                
                if (Session["OrderNumber"] != null)
                {
                    //order number is "" after coming from page unload()
                    if (!Session["OrderNumber"].ToString().Equals(""))
                    {
                        int orderID = int.Parse(Session["OrderNumber"].ToString());
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
                Session["OrderNumber"] = "";
                Session["FromViewPlacedOrders"] = null;
                Session["FirstOrder"] = false;
                Response.Redirect("PlaceOrder.aspx",false);
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
               // ds.Tables[0].Columns.Add("ProductInfo", typeof(string), "Product_Name+ ' '+itemStrength+' '+itemPackSize+' '+itemForm");
                
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
            RequestTo.Enabled = false;
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Status = (Label)e.Row.FindControl("lblStatus");
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

                //Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                //Label Label1 = (Label)e.Row.FindControl("Label1");

                //Label dosage = (Label)e.Row.FindControl("dosage2");
                //Label Label2 = (Label)e.Row.FindControl("Label2");

                //Label packSize = (Label)e.Row.FindControl("packSize2");
                //Label Label3 = (Label)e.Row.FindControl("Label3");

                //if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                //{
                //    ProductStrength.Visible = false;
                //    Label1.Visible = false;
                //}
                //else
                //{
                //    ProductStrength.Visible = true;
                //    Label1.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(dosage.Text))
                //{
                //    dosage.Visible = false;
                //    Label2.Visible = false;
                //}
                //else
                //{
                //    dosage.Visible = true;
                //    Label2.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(packSize.Text))
                //{
                //    packSize.Visible = false;
                //    Label3.Visible = false;
                //}
                //else
                //{
                //    packSize.Visible = true;
                //    Label3.Visible = true;
                //}
            }
        }

        protected void btnSearchVendor_Click(object sender, ImageClickEventArgs e)
        {
            if (txtVendor.Text.Length >= 3)
            {
                PopulateDropDownVendor(txtVendor.Text);
                RequestTo.Visible = false;
            }
        }

        public void PopulateDropDownVendor(String Text)
        {
            #region Populating Vendor Name Dropdown

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
                if (RequestTo.DataSource != null)
                {
                    RequestTo.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                RequestTo.DataSource = ds.Tables[0];
                RequestTo.DataTextField = "SupName";
                RequestTo.DataValueField = "SuppID";
                RequestTo.DataBind();
                if (RequestTo != null)
                {
                    RequestTo.Items.Insert(0, "Select Vendor");
                    RequestTo.SelectedIndex = 0;
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
         

        protected void CmbVendors_SelectedIndexChanged(object sender, EventArgs e)
        {
            CmbVendors.Enabled = false;
        }

          
    }
}