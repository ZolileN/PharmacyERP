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
                    dgvVendors.Visible = false;
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
                //long VendorId = long.Parse(((Label)dgvVendors.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblSupID")).Text);
                //connection.Open();


                //SqlCommand command3 = new SqlCommand("sp_getOrderDetailRecieve_ID", connection);
                //command3.CommandType = CommandType.StoredProcedure;
                //command3.Parameters.AddWithValue("@p_OrderDetID", VendorId);
                //DataSet ds = new DataSet();
                //SqlDataAdapter sA = new SqlDataAdapter(command3);
                //sA.Fill(ds);

                //for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                //{
                //    int StockID = int.Parse(ds.Tables[0].Rows[i]["StockID"].ToString());
                //    int quantity = int.Parse(ds.Tables[0].Rows[i]["Quantity"].ToString());
                //    SqlCommand command2 = new SqlCommand();
                //    command2 = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                //    command2.CommandType = CommandType.StoredProcedure;
                //    command2.Parameters.AddWithValue("@p_StockID", StockID);
                //    command2.Parameters.AddWithValue("@p_quantity", quantity);
                //    command2.Parameters.AddWithValue("@p_Action", "Add");
                //    command2.ExecuteNonQuery();
                //}


                //SqlCommand command = new SqlCommand("sp_DeleteSO_ID", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_OrderDetailID", VendorId);

                //command.ExecuteNonQuery();
                //if (StockDisplayGrid.Rows.Count == 1)
                //{
                //    btnAccept.Visible = false;
                //    btnDecline.Visible = false;
                //}
            }
        }
    }
}