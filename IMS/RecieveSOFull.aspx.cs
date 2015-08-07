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
    public partial class RecieveSOFull : System.Web.UI.Page
    {

        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public DataSet ProductSet;
        public DataSet systemSet;
        public static bool FirstOrder;
        public GridView innerGrid;
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
                    BindGrid();
                    DisplaySODetailsLabels();
                    //for (int id = 0; id < innerGrid.Rows.Count; id++)
                    //{

                    //    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { setExpiryDates(" + id + ");return false });", true);
                    //}
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


                if (int.TryParse(Session["SalesONumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }
                if (int.TryParse(Session["UserSys"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_SysID", OrderNumber);
                }
                  
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                Session["dsSODetails"] = ds;
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                if (StockDisplayGrid.DataSource == null)
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
        private void DisplaySODetailsLabels()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                
                SqlCommand command = new SqlCommand("sp_getSalesOrderDetailsByOrderID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;

                if (int.TryParse(Session["SalesONumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }

                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);


                lblSOID.Text = ds.Tables[0].Rows[0]["OrderID"].ToString();
                SODate.Text = ds.Tables[0].Rows[0]["OrderDate"].ToString();
                OrderStatus.Text = ds.Tables[0].Rows[0]["Status"].ToString();
                OrderTo.Text = ds.Tables[0].Rows[0]["SystemName"].ToString();


                sendQty.Text = ds.Tables[0].Rows[0]["SentQuantity"].ToString();
                RetQty.Text = ds.Tables[0].Rows[0]["ReturnedQuantity"].ToString();
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
        
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            //First clear all the sessions if created in this page
            Response.Redirect("ReceiveSalesOrder.aspx");
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    Label Status = (Label)e.Row.FindControl("lblStatus");
                    Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                    Button btnDetail = (Button)e.Row.FindControl("btnDetails");
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

                    if (Session["ExistingOrder"].Equals(true))
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
                    innerGrid = Details;
                    innerGrid.RowDataBound += innerGrid_RowDataBound;
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

                        for (int i = 0; i < Details.Rows.Count; i++)
                        {
                            ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { setExpiryDates(" + i + ");return false });", true);
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
            catch (Exception ex) 
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }

        }

        void innerGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowIndex >= 0)
            {
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "script", "$(function () { setExpiryDates(" + e.Row.RowIndex + ");return false });", true);


                Label lblExpiry = (Label)e.Row.FindControl("lblExpiryDate");

                lblExpiry.Attributes.Add("OnDataBinding", "setExpiryDates(" + e.Row.RowIndex + ");return false;");

                 
            }
        }

        protected void btnAcceptAll_Click(object sender, EventArgs e)
        {
            try
            {
                for (int i = 0; i < StockDisplayGrid.Rows.Count; i++)
                {
                    Label OrderDetNo = (Label)StockDisplayGrid.Rows[i].FindControl("OrderDetailNo");
                    GridView Details = (GridView)StockDisplayGrid.Rows[i].FindControl("StockDetailDisplayGrid");

                    if (Details != null)
                    {
                        for (int j = 0; j < Details.Rows.Count; j++)
                        {
                            DateTime dtExpiry;
                            Label lblExpiryDate = (Label)Details.Rows[j].FindControl("lblExpiryDate");
                            if (!string.IsNullOrWhiteSpace(lblExpiryDate.Text))
                            {
                                dtExpiry = DateTime.Parse(lblExpiryDate.Text.ToString());
                            }
                            else
                            {
                                dtExpiry = DateTime.MinValue;
                            }

                            int orderDetailNo = int.Parse(OrderDetNo.Text.ToString());
                            if (connection.State == ConnectionState.Closed)
                            {
                                connection.Open();
                            }
                            SqlCommand command2 = new SqlCommand("sp_UpdatetblSaleOrderDetail_Receive_AcceptAll", connection);
                            command2.CommandType = CommandType.StoredProcedure;
                            if (dtExpiry == DateTime.MinValue)
                                command2.Parameters.AddWithValue("@expiredDate", DBNull.Value);
                            else
                                command2.Parameters.AddWithValue("@expiredDate", dtExpiry);

                            command2.Parameters.AddWithValue("@OrderDetId", orderDetailNo);
                            command2.ExecuteNonQuery();
                        }

                    }
                }
                Response.Redirect("ReceiveSalesOrder.aspx",false);
            }
            catch(Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
           
        }


        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Edit"))
            {
                Label OrderDetNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderDetailNo");
                //Label ProductID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblProductID");
               // Label RequestedTo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedTo");
               // Label OrderTo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderTo");
                //session is setting
                Session["OrderDetailsNo"] = OrderDetNo.Text.ToString();
                //Session["ProductID"] = ProductID.Text;
                //Session["RequestedTo"] = RequestedTo.Text;

                Response.Redirect("RecieveSOFullEdit.aspx",false);
            }
                 
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}