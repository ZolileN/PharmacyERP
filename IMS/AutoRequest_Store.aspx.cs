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
    public partial class AutoRequest_Store : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private static DataSet ProductSet;
        public static DataSet systemSet;
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                ProductSet = new DataSet();
            }
        }

        public void GenerateSales(DateTime Salefrom, DateTime Saleto, int SysID)
        {
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_GenerateAutoRequest_Sales", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_SysID", SysID);
                cmd.Parameters.AddWithValue("@p_SaleFROM", Salefrom);
                cmd.Parameters.AddWithValue("@p_SaleTO", Saleto);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(cmd);
                sA.Fill(ds);
                ProductSet = ds;

                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        protected void btnCreateRequest_Click(object sender, EventArgs e)
        {
            int SysID = Int32.Parse(Session["UserSys"].ToString());
            DateTime SalesFrom = Convert.ToDateTime(DateTextBox.Text.ToString());
            DateTime SalesTo = Convert.ToDateTime(DateTextBox2.Text.ToString());

            GenerateSales(SalesFrom,SalesTo,SysID);
            btnAccept.Visible = true;
            btnDecline.Visible = true;
        }

        protected void btnCancelRequest_Click(object sender, EventArgs e)
        {
            if(DateTextBox.Enabled.Equals(false))
            {
                DateTextBox.Enabled = true;
            }
            if (DateTextBox2.Enabled.Equals(false))
            {
                DateTextBox2.Enabled = true;
            }
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
            ProductSet = null;
            btnAccept.Visible = false;
            btnDecline.Visible = false;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreRequestsMain.aspx");
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            if (ProductSet.Equals(null))
            {
                WebMessageBoxUtil.Show("Plese generate Sales by providing Dates, in order to generate requests");
            }
            else
            {
                for (int i = 0; i < ProductSet.Tables[0].Rows.Count; i++)
                {
                    if (i == 0)
                    {
                        #region Creating Order
                        int pRequestFrom = 0;
                        int pRequestTo = 0;
                        String OrderMode = "Warehouse";
                        int OrderType = 2;
                        String Invoice = "";
                        String Vendor = "False";


                        try
                        {
                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_CreateOrder", connection);
                            command.CommandType = CommandType.StoredProcedure;

                            if (int.TryParse(ProductSet.Tables[0].Rows[i]["TOSALES"].ToString(), out pRequestTo))
                            {
                                command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                            }
                            if (int.TryParse(ProductSet.Tables[0].Rows[i]["FROMSALES"].ToString(), out pRequestFrom))
                            {
                                command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                            }

                            command.Parameters.AddWithValue("@p_OrderType", OrderType);
                            command.Parameters.AddWithValue("@p_Invoice", Invoice);
                            command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                            command.Parameters.AddWithValue("@p_Vendor", Vendor);
                            command.Parameters.AddWithValue("@p_orderStatus", "Initiated");

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
                    }

                    #region Linking to Order Detail table

                    try
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                        command.CommandType = CommandType.StoredProcedure;

                        int OrderNumber, ProductNumber, Quantity = 0;

                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        }
                        if (int.TryParse(ProductSet.Tables[0].Rows[i]["ProductID"].ToString(), out ProductNumber))
                        {
                            command.Parameters.AddWithValue("@p_ProductID", ProductNumber);
                        }
                        if (int.TryParse(ProductSet.Tables[0].Rows[i]["SaleQuantity"].ToString(), out Quantity))
                        {
                            command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);
                        }

                        command.Parameters.AddWithValue("@p_status", "Initiated");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Warehouse");

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

                WebMessageBoxUtil.Show("Auto Request of following items has been generated to warehouse");
                Response.Redirect("StoreRequestsMain.aspx");
            }
           

            
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            if (DateTextBox.Enabled.Equals(false))
            {
                DateTextBox.Enabled = true;
            }
            if (DateTextBox2.Enabled.Equals(false))
            {
                DateTextBox2.Enabled = true;
            }
            StockDisplayGrid.DataSource = null;
            StockDisplayGrid.DataBind();
            ProductSet = null;
            btnAccept.Visible = false;
            btnDecline.Visible = false;
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            DataSet dsManipulation = new DataSet();
            dsManipulation = ProductSet;
            try
            {
                if (e.CommandName.Equals("UpdateStock"))
                {
                    Label Barcode = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("BarCode");
                    TextBox Quantity = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity");

                    for (int i = 0; i < dsManipulation.Tables[0].Rows.Count; i++)
                    {
                        if (dsManipulation.Tables[0].Rows[i]["BarCode"].ToString().Equals(Barcode.Text.ToString()))
                        {
                            dsManipulation.Tables[0].Rows[i]["SaleQuantity"] = Convert.ToInt32(Quantity.Text.ToString());
                            break;
                        }
                    }
               }
            }
            catch (Exception exp)
            {
            }
            finally
            {
                StockDisplayGrid.EditIndex = -1;
                ProductSet = null;
                ProductSet = dsManipulation;
                StockDisplayGrid.DataSource = ProductSet.Tables[0];
                StockDisplayGrid.DataBind();
            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            StockDisplayGrid.DataSource = ProductSet.Tables[0];
            StockDisplayGrid.DataBind();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            StockDisplayGrid.DataSource = ProductSet.Tables[0];
            StockDisplayGrid.DataBind();
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            StockDisplayGrid.DataSource = ProductSet.Tables[0];
            StockDisplayGrid.DataBind();
        }

        protected void DateTextBox_TextChanged(object sender, EventArgs e)
        {
            if(DateTextBox.Text.Equals(null))
            {

            }
            else
            {
                DateTextBox.Enabled = false;
            }
        }

        protected void DateTextBox2_TextChanged(object sender, EventArgs e)
        {
            if (DateTextBox2.Text.Equals(null))
            {

            }
            else
            {
                DateTextBox2.Enabled = false;
            }
        }
    }
}