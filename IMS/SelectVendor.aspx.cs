using AjaxControlToolkit;
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
    public partial class SelectVendor : System.Web.UI.Page
    {
        //protected void Page_Load(object sender, EventArgs e)
        //{

        //}

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

                bool display = ordersPopupGrid.populateGrid();

                if (display.Equals(true))
                {
                    mpeOrdersPopupDiv.Show();
                }

                try
                {
                    btnContinue.Visible = false;
                    Session.Remove("dsProdcts");
                    Session.Remove("dsProducts_MP");
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
            
            ds = VendorBLL.GetAllVendors();
            ProductSet = ds;
                 

        }

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
             BindGrid();
            //ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
            //mpe.Show();

        }

        protected void gdvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
        }

        protected void gdvVendor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //gdvVendor.EditIndex = -1;
            //BindGrid();
        }

        protected void gdvVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
             

        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            string Vendorname = txtVendor.Text;
            Session["Vendorname"] = Vendorname;
            bool display = ordersPopupGrid.populateGrid();

            if (display.Equals(false))
            {
                mpeOrdersPopupDiv.Show();
            }

            Response.Redirect("ManualPurchase.aspx?" + Vendorname);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtVendor.Text + '%';
            Session["txtVendor"] = Text;
            VendorsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void btnContinue_Click1(object sender, EventArgs e)
        {
            string Vendorname = txtVendor.Text;
            Session["Vendorname"] = Vendorname;
            

           // Response.Redirect("ManualPurchase.aspx?" + Vendorname);
        }
    }
}