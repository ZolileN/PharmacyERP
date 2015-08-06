using IMS.Util;
using IMSBusinessLogic;
using IMSCommon;
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

namespace IMS
{
    public partial class ManageVendor : System.Web.UI.Page
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindGrid();

                }
                catch (Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
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
        private void BindGrid()
        {
            try
            {
                ds = VendorBLL.GetAllVendors(connection);

                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        private void BindGridDistinct(int ID)
        {
            try
            {
                Vendor vendor = new Vendor();
                vendor.supp_ID = ID;
                ds = VendorBLL.GetDistinct(connection, vendor);

                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
          
        }

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            try
            {
                gdvVendor.PageIndex = e.NewPageIndex;

                Vendor vendor = new Vendor();
                vendor.SupName = SelectProduct.Text;
                int SysID = Convert.ToInt32(Session["UserSys"].ToString());
                bool isStore;

                if (!Session["UserRole"].ToString().Equals("Store"))
                {
                    isStore = false;
                }
                else
                {
                    isStore = true;
                }
                ds = VendorBLL.GetDistinctByNane(connection, vendor, SysID, isStore);
                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
                //BindGrid();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        protected void gdvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

        protected void gdvVendor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //gdvVendor.EditIndex = e.NewEditIndex;
            Label SuppID = (Label)gdvVendor.Rows[e.NewEditIndex].FindControl("lblSupID");
            Response.Redirect("AddEditVendor.aspx?Id="+SuppID.Text, false);
            //BindGrid();
        }

        protected void gdvVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                VendorBLL _vendorBll = new VendorBLL();
                Label ID = (Label)gdvVendor.Rows[e.RowIndex].FindControl("lblSupID");
                int id = int.Parse(ID.Text);
                Vendor vendor = new Vendor();//= empid.Text;
                vendor.supp_ID = id;
                _vendorBll.Delete(vendor, connection);


            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                gdvVendor.EditIndex = -1;
                BindGrid();
            }
        }

        protected void btnAddVendor_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddEditVendor.aspx", false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }



        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            if (SelectProduct.Text.Length >= 3)
            {
                PopulateDropDown(SelectProduct.Text);
                StockAt.Visible = true;
            }
        }

        public void PopulateDropDown(String Text)
        {
            #region Populating Product Name Dropdown

            try
            {
                connection.Open();

                //Text = Text + "%";
                SqlCommand command = new SqlCommand("sp_GetVendor_byNameParam", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_VendName", Text);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                if (StockAt.DataSource != null)
                {
                    StockAt.DataSource = null;
                }

                ProductSet = null;
                ProductSet = ds;
                StockAt.DataSource = ds.Tables[0];
                StockAt.DataTextField = "SupName";
                StockAt.DataValueField = "SuppID";
                StockAt.DataBind();
                if (StockAt != null)
                {
                    StockAt.Items.Insert(0, "Select Vendor");
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
        }
        protected void StockAt_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (StockAt.SelectedIndex == -1)
            {
                BindGrid();
            }
            else
            {
                BindGridDistinct(Convert.ToInt32(StockAt.SelectedValue.ToString()));
            }
        }

        protected void SelectProduct_TextChanged(object sender, EventArgs e)
        {

        }

        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            BindGrid();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            Vendor vendor = new Vendor();
            vendor.SupName = SelectProduct.Text;
            int SysID = Convert.ToInt32(Session["UserSys"].ToString());
            bool isStore;
           
            if (!Session["UserRole"].ToString().Equals("Store"))
            {
                isStore = false;
            }
            else
            {
                isStore = true;
            }
            ds = VendorBLL.GetDistinctByNane(connection, vendor, SysID, isStore);
            //ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();
        }
    }
}