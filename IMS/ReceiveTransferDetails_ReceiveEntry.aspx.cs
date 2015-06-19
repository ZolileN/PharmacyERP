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
    public partial class ReceiveTransferDetails_ReceiveEntry : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                lblTotalTransferQty.Text = Session["TransferedQty"].ToString();
                BindGrid();
            }
        }

        private void BindGrid()
        {
            int TransferDetID = 0;
            DataSet ds = new DataSet();
            int.TryParse(Session["TransferDetailID"].ToString(), out TransferDetID); 
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("sp_getTransferDetailsReceiveEntry_TransferDetailID", connection);
            command.Parameters.AddWithValue("@p_TransferDetailID", TransferDetID);

            command.CommandType = CommandType.StoredProcedure;
            command.ExecuteNonQuery();
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(ds);

            dgvReceiveTransferDetailsReceive.DataSource = ds;
            dgvReceiveTransferDetailsReceive.DataBind();

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            //Update the values in tblTransferDetails_Receive 
            for(int i=0;i<dgvReceiveTransferDetailsReceive.Rows.Count; i++)
            {
                int entryID,TransferedQty = 0;
                Label lblentryID = (Label)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("lblentryID");
                TextBox txtTransferedQty = (TextBox)dgvReceiveTransferDetailsReceive.Rows[i].FindControl("txtSendQty");
                int.TryParse(lblentryID.Text.ToString(),out entryID);
                int.TryParse(txtTransferedQty.Text.ToString(), out TransferedQty);

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_UpdateTransferReceiveEntry", connection);
                command.Parameters.AddWithValue("@p_entryID", entryID);
                command.Parameters.AddWithValue("@p_TransferQty", TransferedQty);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                Response.Redirect("ReceiveTransferOrder.aspx");
            }
             
        }
    }
}