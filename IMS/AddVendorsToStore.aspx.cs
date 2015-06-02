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
    public partial class AddVendorsToStore : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["Storename"] != null)
                {
                    spnStoreName.InnerHtml = Session["Storename"].ToString();
                     
                }
            }
            
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectStore.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtVendor.Text + '%';
            Session["txtVendor"] = Text;
            MultipleVendorsSelectPopup.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void dgvVendors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Delete"))
            {
                int VendorId = int.Parse(((Label)dgvVendors.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSupID")).Text);
                connection.Open();
                int StoreId = int.Parse(Session["SystemId"].ToString());
                
                SqlCommand command3 = new SqlCommand("sp_DeleteFromtblVendors_Store", connection);
                command3.CommandType = CommandType.StoredProcedure;
                command3.Parameters.AddWithValue("@VendorID", VendorId);
                command3.Parameters.AddWithValue("@StoreID", StoreId);
                command3.ExecuteNonQuery();

                dgvVendors.DataBind();
                connection.Close();
                 
            }
        }

        protected void dgvVendors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void dgvVendors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendors.PageIndex = e.NewPageIndex;

        }
        
        private void BindData()
        {

        }

    }
}