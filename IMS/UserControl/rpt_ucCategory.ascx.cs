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
    public partial class rpt_ucCategory : System.Web.UI.UserControl
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public void LoadData()
        {
            try
            {

                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand();
                if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("YES"))
                {
                    command = new SqlCommand("sp_rptPI_Category", connection);
                }
                else if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("Expiry"))
                {
                    command = new SqlCommand("sp_rpt_StockDetails", connection);
                }
                else
                {
                     command = new SqlCommand("sp_rptSalesCategory", connection);
                }
                command.CommandType = CommandType.StoredProcedure;
                if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("Expiry"))
                {
                }
                else
                {
                    if (Session["SearchItemCat_RPT"] != null && Session["SearchItemCat_RPT"].ToString() != "")
                    {
                        command.Parameters.AddWithValue("@p_Search", Session["SearchItemCat_RPT"].ToString());
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_Search", DBNull.Value);
                    }
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet dsCustomers = new DataSet();
                dA.Fill(dsCustomers);

                gdvDepartment.DataSource = dsCustomers.Tables[2];
                gdvDepartment.DataBind();

            }
            catch (Exception ex)
            {
                //show ex message
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnSelectDepartment_Click(object sender, EventArgs e)
        {
            GridViewRow rows = gdvDepartment.SelectedRow;
            foreach (GridViewRow row in gdvDepartment.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label CustomerName = (Label)row.Cells[0].FindControl("lblCatName");
                        Label CustomerID = (Label)row.Cells[0].FindControl("lbCatID");

                        if (CustomerID.Text.ToString() != "" && CustomerName.Text.ToString() != "")
                        {
                            TextBox mpe = (TextBox)this.Parent.FindControl("txtCategory");
                            mpe.Text = Server.HtmlDecode(CustomerName.Text);
                            Session["rptCategoryID"] = CustomerID.Text.ToString();
                            break;
                        }
                    }
                    else
                    {
                        TextBox mpe = (TextBox)this.Parent.FindControl("txtCategory");
                        mpe.Text = mpe.Text;
                    }
                }
            }
        }

        protected void gdvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvDepartment.PageIndex = e.NewPageIndex;
            LoadData();
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCategoryDiv");
            mpe.Show();
        }

        protected void gdvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvDepartment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}