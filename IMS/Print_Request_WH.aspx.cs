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
	public partial class Print_Request_WH : System.Web.UI.Page
	{
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
       
        protected void Page_Load(object sender, EventArgs e)
        {

            if (!IsPostBack)
            {

                if (Request.QueryString["Id"] != null) 
                {
                    BindGrid();
                }

            }

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("WH_TransferRequestGrid");
            Response.Redirect("SelectWarehouse.aspx", false);
        }

        protected void BindGrid()
        {
            try
            {
                DataTable dtGridSource = new DataTable();
                dtGridSource.Columns.Add("ProductID");
                dtGridSource.Columns.Add("SystemID");
                dtGridSource.Columns.Add("Product_Name");
                dtGridSource.Columns.Add("RequestedFrom");
                dtGridSource.Columns.Add("RequestedTo");
                dtGridSource.Columns.Add("RequestedQty");

                int transferID =  int.Parse(Request.QueryString["Id"].ToString());
                DataSet ds = new DataSet();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystems_ByTransferID", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_transferID", transferID);

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);


                lblFROMSystemName.Text = ds.Tables[0].Rows[0]["FROMName"].ToString();
                lblFROMSystemAddress.Text = ds.Tables[0].Rows[0]["FROMAdress"].ToString();
                lblFROMSystemPhone.Text = ds.Tables[0].Rows[0]["FROMPhone"].ToString();
                lblFROMSystemEmail.Text = ds.Tables[0].Rows[0]["FROMFax"].ToString();
                lblToSystemName.Text = ds.Tables[0].Rows[0]["ToName"].ToString();
                lblToSystemAddress.Text = ds.Tables[0].Rows[0]["ToAddress"].ToString();
                lblToSystemPhone.Text = ds.Tables[0].Rows[0]["ToPhone"].ToString();
                lblToSystemEmail.Text = ds.Tables[0].Rows[0]["ToFax"].ToString();


                DataTable dsDistinct = (DataTable)Session["WH_TransferRequestGrid"];

                dtGridSource.AcceptChanges();
                dgvTransferDisplay.DataSource = dsDistinct;
                dgvTransferDisplay.DataBind();

            }
            catch
            {

            }
            finally
            {
                connection.Close();
            }
        }
	}
}