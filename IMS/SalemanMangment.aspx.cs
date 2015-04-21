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
using IMSCommon.Util;

namespace IMS
{
    public partial class SalemanMangment : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet UserSet;
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddNew_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegisterUsers.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();

                string Text = UserName.Text;
                // String Text = SelectProduct.SelectedItem.ToString() + "%";
                SqlCommand command = new SqlCommand("SELECT * From tbl_Users Where [tbl_Users].U_EmpID LIKE '" + Text + "'", connection);
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                //if (SelectProduct.DataSource != null)
                //{
                //    SelectProduct.DataSource = null;
                //}

                UserSet = null;
                UserSet = ds;

                SalemanDisplayGrid.DataSource = ds;
                SalemanDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();

            }
        }

        protected void btnClear_Click(object sender, EventArgs e)
        {
            UserName.Text = "";
            UserAddress.Text = "";
            UserContact.Text = "";
            ddlUserRole.SelectedIndex = 0;
        }
        protected void SalemanDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SalemanDisplayGrid.EditIndex = e.NewEditIndex;
         BindGrid();
        }
        private void BindGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting Product Details
            try
            {
                string Text = UserName.Text;
                
                    String Query = "SELECT * From tbl_Users Where [tbl_Users].U_EmpID LIKE '" + Text + "'";

                    connection.Open();
                    SqlCommand command = new SqlCommand(Query, connection);
                    SqlDataAdapter SA = new SqlDataAdapter(command);
                    UserSet = null;
                    SA.Fill(ds);
                    UserSet = ds;
                    SalemanDisplayGrid.DataSource = ds;
                    SalemanDisplayGrid.DataBind();
                

            }
            catch (Exception ex)
            {
            }
            finally
            {
                connection.Close();
            }
            #endregion
        }
        protected void SalemanDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Edit"))
                {
                    TextBox ItemName = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Name") ;
                    TextBox ItemAddress = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Address");
                    TextBox ItemContact = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Phone");
                   // TextBox ItemUserRole = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
                    DropDownList ddluserRole = (DropDownList)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
                  
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {

            }
        }

        protected void SalemanDisplayGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int i;
            GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
           bool id =int.TryParse( SalemanDisplayGrid.Rows[e.RowIndex].ToString(), out i);
           int userid = int.Parse(SalemanDisplayGrid.SelectedRow.Cells[0].Text);
           TextBox ItemName = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Name");
           TextBox ItemAddress = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Address");
           TextBox ItemContact = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Phone");
           // TextBox ItemUserRole = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
           DropDownList ddluserRole = (DropDownList)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("ddlUserRole");

           SalemanDisplayGrid.EditIndex = -1;

           connection.Open();

           SqlCommand cmd = new SqlCommand("update detail set name='" + ItemName.Text + "',address='" + ItemAddress.Text + "',country='" + ItemContact.Text + "'where id='" + id + "'", connection);
           cmd.ExecuteNonQuery();

           connection.Close();

           BindGrid();



        }

        

    }
}