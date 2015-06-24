using AjaxControlToolkit;
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
    public partial class uc_Select_Salesman : System.Web.UI.UserControl
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        public void populateGrid()
        {
            int id;
            DataSet ds1 = new DataSet();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("dbo.Sp_GetUserby_Role", connection);
            command.CommandType = CommandType.StoredProcedure;

            if (Session["txtSalesman"] != null)
            {
                command.Parameters.AddWithValue("@p_userName", Session["txtSalesman"].ToString());
            }
            else
            {
                command.Parameters.AddWithValue("@p_userName", DBNull.Value);
            }

            command.Parameters.AddWithValue("@p_roleName", "Salesman");

            SqlDataAdapter SA = new SqlDataAdapter(command);


            SA.Fill(ds1);


            gdvSalesman.DataSource = null;
            gdvSalesman.DataSource = ds1;
            gdvSalesman.DataBind();

        }
        private void BindGrid()
        {
            int id;
            DataSet ds1 = new DataSet();

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("dbo.Sp_GetUserby_Role", connection);
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@p_userName", DBNull.Value);


            command.Parameters.AddWithValue("@p_roleName", "Salesman");

            SqlDataAdapter SA = new SqlDataAdapter(command);


            SA.Fill(ds1);


            gdvSalesman.DataSource = null;
            gdvSalesman.DataSource = ds1;
            gdvSalesman.DataBind();

        }

        protected void gdvSalesman_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvSalesman_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvSalesman_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void gdvSalesman_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvSalesman.PageIndex = e.NewPageIndex;
            if (Session["txtSalesman"] != null)
            {
                populateGrid();
            }
            else
            {
                BindGrid();
            }
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
            mpe.Show();
        }

        protected void gdvSalesman_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void SelectSalesman_Click(object sender, EventArgs e)
        {
            GridViewRow rows = gdvSalesman.SelectedRow;
            foreach (GridViewRow row in gdvSalesman.Rows)
             {
                 if (row.RowType == DataControlRowType.DataRow)
                 {
                     CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                     Control ctl = this.Parent;
                     TextBox ltMetaTags = null;
                     TextBox ltSalID = null;
                     ltMetaTags = (TextBox)ctl.FindControl("txtSlman");
                     ltSalID = (TextBox)ctl.FindControl("lblSlmanID");

                     if (chkRow.Checked)
                     {
                         Label Name = (Label)row.Cells[0].FindControl("lblName");
                         Label salemanID = (Label)row.Cells[0].FindControl("lblSalemanID");
                         ltMetaTags.Text = Name.Text;
                         ltSalID.Text = salemanID.Text;
                         break;
                     }
                 }
             }
        }
    }
}