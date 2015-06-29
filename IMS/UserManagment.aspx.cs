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
    public partial class UserManagment : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet UserSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            #region Populating User Role Drop Down DropDown
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tbl_UserRoles", connection);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    //ddlUserRole.DataSource = ds.Tables[0];
                    //ddlUserRole.DataTextField = "user_RoleName";
                    //ddlUserRole.DataValueField = "user_RoleID";
                    //ddlUserRole.DataBind();
                    //if (ddlUserRole != null)
                    //{
                    //    ddlUserRole.Items.Insert(0, "Select User Role");
                    //    ddlUserRole.SelectedIndex = 0;
                    //}

                }
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


        protected void SalemanDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string ID = "";
            SalemanDisplayGrid.EditIndex = e.NewEditIndex;
            Label UserID = (Label)SalemanDisplayGrid.Rows[e.NewEditIndex].FindControl("lblUserID");
            Label roleName = (Label)SalemanDisplayGrid.Rows[e.NewEditIndex].FindControl("lblroleName");
            ID = UserID.Text;
            Session["ur_RoleName"] = roleName.Text;
            Response.Redirect("RegisterUsers.aspx?ID=" + ID);
        }
        private void BindGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting User Details
            try
            {
                string Text = "sale";

                Text = Text + "%";
                String Query = "SELECT UserID,U_RolesID, U_EmpID,Address,Contact,U_RolesID,user_RoleName From [tbl_UserRoles] inner join tbl_Users on tbl_Users.U_RolesID=[tbl_UserRoles].user_RoleID";
                // String Query = "SELECT * From tbl_Users";
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


        protected void SalemanDisplayGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {
            int i;
            GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
            bool id = int.TryParse(SalemanDisplayGrid.Rows[e.RowIndex].ToString(), out i);
            //int userid = int.Parse(SalemanDisplayGrid.SelectedRow.Cells[0].Text);
            Label label = (Label)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("lblUserID");
            TextBox ItemName = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Name");
            TextBox ItemAddress = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Address");
            TextBox ItemContact = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Phone");
            // TextBox ItemUserRole = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
            DropDownList ddluserRole = (DropDownList)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("ddlUserRole");

            SalemanDisplayGrid.EditIndex = -1;

            connection.Open();

            SqlCommand cmd = new SqlCommand("update tbl_Users set [U_EmpID]='" + ItemName.Text + "',address='" + ItemAddress.Text + "',[Contact]='" + ItemContact.Text + "'where [UserID]='" + label.Text + "'", connection);
            cmd.ExecuteNonQuery();

            connection.Close();

            BindGrid();



        }

        protected void SalemanDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
            Label label = (Label)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("lblUserID");

            SalemanDisplayGrid.EditIndex = -1;

            connection.Open();
            SqlCommand cmd = new SqlCommand("delete from tbl_Users where [UserID]='" + label.Text + "'", connection);
            cmd.ExecuteNonQuery();

            connection.Close();

            BindGrid();


        }

        protected void SalemanDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SalemanDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void SalemanDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SalemanDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnAddSalesman_Click(object sender, EventArgs e)
        {
            Session["ur_RoleName"] = "Admin";
            Response.Redirect("RegisterUsers.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }
    }
}