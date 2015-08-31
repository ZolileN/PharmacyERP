using AjaxControlToolkit;
using IMS.Util;
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

namespace IMS.UserControl
{
    public partial class uc_Order_Display : System.Web.UI.UserControl
    {
     public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();

        
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (IsPostBack)
                {
                    //Response.Redirect("ManualPurchase.aspx", false);

                }
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                   
                    LoadData();
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
        public void LoadData()
        {
            #region Display Requests
            try
            {

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetPO_nonGen", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_RequestedByID", Session["UserSys"]);
                command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
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
        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            LoadData();
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

            try
            {
                if (e.CommandName.Equals("Edit"))
                {
                    int RowNumber = 0;
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);
                    Label RequestNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedNO");
                    Label RequestFrom = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedFrom");
                    //session is setting
                    Session["OrderNumber"] = RequestNo.Text.ToString();
                    Session["Vendorname"] = RequestFrom.Text.ToString();
                    Session["FromViewPlacedOrders"] = "true";
                    Response.Redirect("ManualPurchase.aspx", false);
                }
                else if (e.CommandName.Equals("Delete"))
                {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label RequestNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedNO");

                    int orderID = int.Parse(RequestNo.Text.ToString());
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
                    connection.Close();
            }
        }
        public void populateGrid() 
        {
            LoadData();
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            LoadData();
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeOrdersPopupDiv");
            mpe.Show();
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
           
        }

        protected void btnIgnore_Click(object sender, EventArgs e)
        {

            if (Session["Vendorname_PO"] != null)
            {
                Session["Vendorname"] = Session["Vendorname_PO"];
                Session.Remove("Vendorname_PO");
                Response.Redirect("ManualPurchase.aspx?" + Session["Vendorname"].ToString());
            }
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label RequestNo = (Label)StockDisplayGrid.Rows[e.RowIndex].FindControl("RequestedNO");

                    int orderID = int.Parse(RequestNo.Text.ToString());
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("sp_DeleteOrder", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderID", orderID);

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

                LoadData();
                ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeOrdersPopupDiv");
                mpe.Show();

            }
        }

        
    }
}