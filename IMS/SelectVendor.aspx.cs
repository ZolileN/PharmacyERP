using AjaxControlToolkit;
using IMSBusinessLogic;
using IMSCommon;
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
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    btnContinue.Visible = false;
                    Session.Remove("dsProdcts");
                    Session.Remove("dsProducts_MP");
                    BindGrid();

                }
                catch (Exception exp) { }
            }
        }
        private void BindGrid()
        {
            ds = VendorBLL.GetAllVendors(connection);
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
            VendorBLL _vendorBll = new VendorBLL();

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
            Response.Redirect("ManualPurchase.aspx?" + Vendorname);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtVendor.Text + '%';
            Session["txtVendor"] = Text;
            VendorsPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }
    }
}