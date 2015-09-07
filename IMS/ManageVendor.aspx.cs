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
        public static DataSet ProductSet;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack)
            {
                try
                {
                    BindGrid();

                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally 
                {
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
                ds = VendorBLL.GetAllVendors();

                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
            }
            catch (Exception ex) 
            {
                throw ex;
            }
            finally 
            {
            }
        }

        private void BindGridDistinct(int ID)
        {
            try
            {
                Vendor vendor = new Vendor();
                vendor.supp_ID = ID;
                ds = VendorBLL.GetDistinct(vendor);

                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
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
                ds = VendorBLL.GetDistinctByNane(vendor, SysID, isStore);
                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();
                //BindGrid();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally 
            {
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
                _vendorBll.Delete(vendor);


            }
            catch (Exception ex)
            {
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
                
            }
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
            ds = VendorBLL.GetDistinctByNane(vendor, SysID, isStore);
            //ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();
        }
    }
}