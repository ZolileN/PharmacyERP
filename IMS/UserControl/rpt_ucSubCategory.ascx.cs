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
    public partial class rpt_ucSubCategory : System.Web.UI.UserControl
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
                SqlCommand command = new SqlCommand("sp_rptSalesSubCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (Session["SearchItemSubCat_RPT"] != null && Session["SearchItemSubCat_RPT"].ToString() != "")
                {
                    command.Parameters.AddWithValue("@p_Search", Session["SearchItemSubCat_RPT"].ToString());
                }
                else
                {
                    command.Parameters.AddWithValue("@p_Search", DBNull.Value);
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet dsCustomers = new DataSet();
                dA.Fill(dsCustomers);

                gdvDepartment.DataSource = dsCustomers.Tables[0];
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
                        Label CustomerName = (Label)row.Cells[0].FindControl("lblSubCatName");
                        Label CustomerID = (Label)row.Cells[0].FindControl("lbSubCatID");

                        if (CustomerID.Text.ToString() != "" && CustomerName.Text.ToString() != "")
                        {
                            TextBox mpe = (TextBox)this.Parent.FindControl("txtSubcategory");
                            mpe.Text = Server.HtmlDecode(CustomerName.Text);
                            Session["rptSubCategoryID"] = CustomerID.Text.ToString();
                            break;
                        }
                    }
                    else
                    {
                        TextBox mpe = (TextBox)this.Parent.FindControl("txtSubcategory");
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
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeSubCategoryDiv");
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