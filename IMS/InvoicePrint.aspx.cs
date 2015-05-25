using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using System.IO;

namespace IMS
{
    public partial class InvoicePrint : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet InvoiceSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                InvoiceDate.Text = Session["PrintInvoiceDate"].ToString();
                DueDate.Text = Session["PrintDueDate"].ToString();
                SalesMan.Text = Session["SalesMan"].ToString();

                if (Session["PrintCheck"].ToString().Equals("Actual"))
                {
                    Invoice.Text = Session["PrintInvoiceNumber"].ToString();
                    lblTotalBonusAmount.Visible = false;
                    //Label9.Visible = false;
                }
                else if (Session["PrintCheck"].ToString().Equals("Bonus"))
                {
                    Invoice.Text = "B" + Session["PrintInvoiceNumber"].ToString();
                    lblTotalSentAmount.Visible = false;
                    //Label8.Visible = false;
                }
                DataSet dsTo = GetSystems(Convert.ToInt32(Session["RequestedFromID"].ToString()));
                To.Text = dsTo.Tables[0].Rows[0]["SystemName"].ToString();
                ToAddress.Text = dsTo.Tables[0].Rows[0]["SystemAddress"].ToString();
                LoadData();
            }

        }

        public void LoadData()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetSaleOrderDetailList_Bonus", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["OrderNo_Invoice"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                InvoiceSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                double BonusAmount = 0;
                double ActualAmount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BonusAmount += Convert.ToDouble(ds.Tables[0].Rows[i]["AmountBonus"]);
                    ActualAmount += Convert.ToDouble(ds.Tables[0].Rows[i]["Amount"]);
                }

                lblTotalBonusAmount.Text = BonusAmount.ToString();
                lblTotalSentAmount.Text = ActualAmount.ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }

        }
        public DataSet GetSystems(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", ID);



                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return ds;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesOrderInvoice.aspx");
        }

        
    }
}