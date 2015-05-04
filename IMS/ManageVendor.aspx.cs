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
    public partial class ManageVendor : System.Web.UI.Page
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
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

        private void BindGrid()
        {
            ds = VendorBLL.GetAllVendors(connection);
            //ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();
            //drpVendor.DataSource = ds;
            //drpVendor.Items.Insert(0, new ListItem("Select Product", ""));
            //drpVendor.DataTextField = "SupName";
            //drpVendor.DataValueField = "Supp_ID";

            //drpVendor.DataBind();
        }

        private void BindGridDistinct(int ID)
        {
            Vendor vendor = new Vendor();
            vendor.supp_ID = ID;
            ds = VendorBLL.GetDistinct(connection, vendor);
            //ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();
            //drpVendor.DataSource = ds;
            //drpVendor.Items.Insert(0, new ListItem("Select Product", ""));
            //drpVendor.DataTextField = "SupName";
            //drpVendor.DataValueField = "Supp_ID";

            //drpVendor.DataBind();
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
            BindGrid();
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
            try
            {
                VendorBLL _vendorBll = new VendorBLL();
                Label ID = (Label)gdvVendor.Rows[e.RowIndex].FindControl("lblSupID");
                int id = int.Parse(ID.Text);
                Vendor vendor = new Vendor();//= empid.Text;
                vendor.supp_ID = id;
                _vendorBll.Delete(vendor, connection);


            }
            catch (Exception exp) { }
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

                Text = Text + "%";
                SqlCommand command = new SqlCommand("Select * From tblVendor Where tblVendor.SupName LIKE '" + Text + "'", connection);
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
    }
}