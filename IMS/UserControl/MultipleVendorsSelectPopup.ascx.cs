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

namespace IMS.UserControl
{

    public partial class MultipleVendorsSelectPopup : System.Web.UI.UserControl
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindGrid();

                }
                catch (Exception exp) { }
            }
        }

        public void PopulateGrid()
        {
            if (Session["txtVendor"] != null)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        String Query = "Select * FROM tblVendor Where  SupName Like '" + Session["txtVendor"].ToString() + "'";

                        connection.Open();
                        SqlCommand command = new SqlCommand(Query, connection);
                        SqlDataAdapter SA = new SqlDataAdapter(command);
                        ProductSet = null;
                        SA.Fill(ds);

                        gdvVendor.DataSource = ds;
                        gdvVendor.DataBind();
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
        }
        private void BindGrid()
        {
            ds = VendorBLL.GetAllVendors(connection);
            ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();

        }

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvVendor.PageIndex = e.NewPageIndex;
            if (Session["txtVendor"] != null)
            {
                PopulateGrid();
            }
            else
            {
                BindGrid();
            }
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
            mpe.Show();

        }

        protected void gdvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            VendorBLL _vendorBll = new VendorBLL();
            if (e.CommandName.Equals("CheckChanged"))
            {
                Label ID = (Label)gdvVendor.Rows[0].FindControl("lblSupID");
                int SupID = int.Parse(ID.Text);
                if (SupID > 0)
                {

                }

            }
        }

        protected void gdvVendor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //gdvVendor.EditIndex = -1;
            //BindGrid();
        }

        protected void gdvVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label ID = (Label)gdvVendor.Rows[e.RowIndex].FindControl("lblSupID");
                int id = int.Parse(ID.Text);
                Vendor vendor = new Vendor();//= empid.Text;
                vendor.supp_ID = id;
                ds = VendorBLL.GetDistinct(connection, vendor);

                Session["VendorName"] = ds.Tables[0].Rows[0]["SupName"];
                Session["VendorId"] = ds.Tables[0].Rows[0]["SuppID"];

                Control ctl = this.Parent;
                TextBox ltMetaTags = null;
                ltMetaTags = (TextBox)ctl.FindControl("txtVendor");
                if (ltMetaTags != null)
                {
                    ltMetaTags.Text = ds.Tables[0].Rows[0]["SupName"].ToString();
                }
            }
            catch (Exception exp) { }

        }

        protected void SelectMultipleVendor_Click(object sender, EventArgs e)
        {
            Control ctl = this.Parent;
            TextBox ltMetaTags = null;
           
            Label lblVendorIds = (Label)ctl.FindControl("lblVendorIds");

            ltMetaTags = (TextBox)ctl.FindControl("txtVendor");

            GridViewRow rows = gdvVendor.SelectedRow;
            foreach (GridViewRow row in gdvVendor.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        lblVendorIds.Text = "," + row.Cells[8].Text;
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);
                        }
                    }
                }
            }
        }
    }
}