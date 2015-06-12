using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace IMS
{
    public partial class ReplenishPO_Generate : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                LoadData();
            }
        }

        public void LoadData()
        {
            try
            {
                int OrderID =0;
                int.TryParse(Session["RelenishOrderNo"].ToString(), out OrderID); //need to check there

                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("Sp_GetPODetails_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", OrderID);
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                gvReplenismentPO.DataSource = ds;
                gvReplenismentPO.DataBind();

                lblOrderFrom.Text = ds.Tables[0].Rows[0]["FromPlace"].ToString();
                lblOrderFromAddress.Text = ds.Tables[0].Rows[0]["FromPlaceAddress"].ToString();
                lblOrderFromPhone.Text = ds.Tables[0].Rows[0]["FromPlacePhone"].ToString();

                lblOrderTo.Text = ds.Tables[0].Rows[0]["ToPlace"].ToString();
                lblOrderToAddress.Text = ds.Tables[0].Rows[0]["ToPlaceAddress"].ToString();
                lblOrderToPhone.Text = ds.Tables[0].Rows[0]["ToPlacePhone"].ToString();
                lblOrderToEmail.Text = ds.Tables[0].Rows[0]["ToPlaceEmail"].ToString();
            }
            catch(Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ReplenishMovement.aspx");
        }

        protected void gvReplenismentPO_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }
    }
}