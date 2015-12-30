using IMS.Util;
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
    public partial class ReceivedTransferOrders : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet dsStatic = new DataSet();
        public static DataTable dtStatic = new DataTable();
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    LoadRepeater();
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        private void LoadRepeater()
        {
            try
            {
                DataSet ds = new DataSet();
                int Userid;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetReceivedTransferOrders", connection);
                int.TryParse(Session["UserSys"].ToString(), out Userid);
                command.Parameters.AddWithValue("@p_LoggedinUserID", Userid);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                DataTable dsDistinct = ds.Tables[0];
                dtStatic = ds.Tables[0];

                DataTable distinctRequests = dsDistinct.DefaultView.ToTable(true, "TransferID");

                repReceivedTransferOrders.DataSource = distinctRequests;
                repReceivedTransferOrders.DataBind();
            }
            catch
            {
                connection.Close();
            }
        }

        protected void repReceivedTransferOrders_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            try
            {
                GridView dgvReceiveTransfer = (GridView)e.Item.FindControl("dgvReceivedTransferOrders");
                
                Literal litReqNo = (Literal)e.Item.FindControl("litReqNo");

                DataSet ds = new DataSet();
                int Userid;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int TransferID = Convert.ToInt32(((DataRowView)e.Item.DataItem).Row[0].ToString());

                SqlCommand command = new SqlCommand("sp_GetReceivedTransferDetails_TransferID", connection);
                int.TryParse(Session["UserSys"].ToString(), out Userid);
                command.Parameters.AddWithValue("@UserID", Userid);
                command.Parameters.AddWithValue("@TransferID", TransferID);

                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(ds);
                dsStatic = ds;

                int totalRows = ds.Tables[0].Rows.Count;
            
                dgvReceiveTransfer.DataSource = ds;
                dgvReceiveTransfer.DataBind();

                litReqNo.Text = ds.Tables[0].Rows[0]["TransferID"].ToString();
               
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}