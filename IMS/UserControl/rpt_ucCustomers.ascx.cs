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
    public partial class rpt_ucCustomers : System.Web.UI.UserControl
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                
            }
        }

        public void LoadData()
        {
            try
            {

                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("sp_rptSalesCustomers", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (Session["SearchItem_RPT"] != null && Session["SearchItem_RPT"].ToString() != "")
                {
                    command.Parameters.AddWithValue("@p_Search", Session["SearchItem_RPT"].ToString());
                }
                else
                {
                    command.Parameters.AddWithValue("@p_Search", DBNull.Value);
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet dsCustomers = new DataSet();
                dA.Fill(dsCustomers);

                gdvCustomers.DataSource = dsCustomers.Tables[0];
                gdvCustomers.DataBind();

            }
            catch(Exception ex)
            {
               //show ex message
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void gdvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvCustomers.PageIndex = e.NewPageIndex;
            LoadData();
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCustomersDiv");
            mpe.Show();
        }

        protected void gdvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvCustomers_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            GridViewRow rows = gdvCustomers.SelectedRow;
            foreach (GridViewRow row in gdvCustomers.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label CustomerName = (Label)row.Cells[0].FindControl("lblCustomer");
                        Label CustomerID = (Label)row.Cells[0].FindControl("lblSysID");

                        if(CustomerID.Text.ToString()!="" && CustomerName.Text.ToString()!="")
                        {
                            TextBox mpe = (TextBox)this.Parent.FindControl("txtCustomers");
                            mpe.Text = Server.HtmlDecode(CustomerName.Text);
                            Session["rptCustomerID"] = CustomerID.Text.ToString();
                            break;
                        }
                    }
                    else
                    {
                        TextBox mpe = (TextBox)this.Parent.FindControl("txtCustomers");
                        mpe.Text = mpe.Text;
                    }
                }
              }


        }
    }
}