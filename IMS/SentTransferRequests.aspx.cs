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
    public partial class SentTransferRequests : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet dsStatic = new DataSet();
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
             try
             {
                 DataSet ds = new DataSet();
                 int Userid;
                 if (connection.State == ConnectionState.Closed)
                 {
                     connection.Open();
                 }

                 SqlCommand command = new SqlCommand("sp_GetOurSentTransferRequests_UserID", connection);
                 int.TryParse(Session["UserSys"].ToString(), out Userid);
                 command.Parameters.AddWithValue("@p_LoggedinUserID", Userid);
                 
                 command.CommandType = CommandType.StoredProcedure;
                 command.ExecuteNonQuery();
                 SqlDataAdapter da = new SqlDataAdapter(command);
                 da.Fill(ds);
                 dsStatic = ds;
                 dgvReceiveOurTransfers.DataSource = ds;
                 dgvReceiveOurTransfers.DataBind();
             }
             catch
             {

             }
            finally
             {
                 connection.Close();
             }
        }

        protected void btnGenTransferAll_Click(object sender, EventArgs e)
        {
            Response.Redirect("CreateTransferRequest.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreMain.aspx");
        }

        
        protected void dgvReceiveOurTransfers_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                if (dsStatic.Tables[0].Rows[e.Row.RowIndex]["TransferStatus"].ToString() == "Accepted")
                {
                    Button btnReceive = (Button)e.Row.FindControl("btnReceive");
                    btnReceive.Visible = true;
                }
            }
        }

        protected void dgvReceiveOurTransfers_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                 if(e.CommandName == "ReceiveProductTransfer")
                 {
                     Label lblDetailsID = (Label)dgvReceiveOurTransfers.Rows[Convert.ToInt32(e.CommandArgument.ToString())].FindControl("lblTransferDetailID");
                     Session["TransferDetailsID"] = lblDetailsID.Text;

                     Response.Redirect("ReceiveRequestTransfers.aspx");
                 }
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