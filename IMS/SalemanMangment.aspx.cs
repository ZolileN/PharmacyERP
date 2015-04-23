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
                    ddlUserRole.DataSource = ds.Tables[0];
                    ddlUserRole.DataTextField = "user_RoleName";
                    ddlUserRole.DataValueField = "user_RoleID";
                    ddlUserRole.DataBind();
                    if (ddlUserRole != null)
                    {
                        ddlUserRole.Items.Insert(0, "Select User Role");
                        ddlUserRole.SelectedIndex = 0;
                    }

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
                Text = Text + "%";

                string TextAddress = UserAddress.Text;
                TextAddress = TextAddress + "%";

                string TextContact = UserContact.Text;
                TextContact = TextContact + "%";
                string UserRole = ddlUserRole.SelectedItem.ToString();
                UserRole = UserRole + "%";
                SqlCommand command;
                if (Text.Length > 2 && !string.IsNullOrWhiteSpace(Text))
                {
                    command = new SqlCommand("SELECT * From tbl_Users Where [tbl_Users].U_EmpID LIKE '" + Text + "'", connection);
                }
                else if (TextAddress.Length > 2 && !string.IsNullOrWhiteSpace(TextAddress))
                {
                    command = new SqlCommand("SELECT * From tbl_Users Where [tbl_Users].Address LIKE '" + TextAddress + "'", connection);
                }
                else if (UserRole.Length > 2 && !string.IsNullOrWhiteSpace(UserRole))
                {
                    command = new SqlCommand("SELECT UserID,U_RolesID, U_EmpID,Address,Contact,U_RolesID user_RoleName From [tbl_UserRoles] inner join tbl_Users on tbl_Users.U_RolesID=[tbl_UserRoles].user_RoleID where [tbl_UserRoles].user_RoleName like'" + UserRole + "'", connection);
                }
                else
                {
                    command = new SqlCommand("SELECT * From tbl_Users Where [tbl_Users].Contact LIKE '" + TextContact + "'", connection);
                }
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
            string DI = "";
            SalemanDisplayGrid.EditIndex = e.NewEditIndex;
            Label UserID = (Label)SalemanDisplayGrid.Rows[e.NewEditIndex].FindControl("lblUserID");
            //Label UserID = (Label)SalemanDisplayGrid.Rows[0].FindControl("lblUserID");

            DI = UserID.Text;

            Response.Redirect("RegisterUsers.aspx?ID=" + DI);
            // BindGrid();
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
                String Query = "SELECT UserID,U_RolesID, U_EmpID,Address,Contact,U_RolesID user_RoleName From [tbl_UserRoles] inner join tbl_Users on tbl_Users.U_RolesID=[tbl_UserRoles].user_RoleID where [tbl_UserRoles].user_RoleName like'" + Text + "'";
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
        //protected void SalemanDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        //{
        //    try
        //    {
        //        //if (e.CommandName.Equals("Edit"))
        //        //{
        //        //   // Label UserID = (Label)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblUserID");
        //        //   // TextBox ItemName = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Name") ;
        //        //   // TextBox ItemAddress = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Address");
        //        //   // TextBox ItemContact = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("Phone");
        //        //   //// TextBox ItemUserRole = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
        //        //   // DropDownList ddluserRole = (DropDownList)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");

        //        //}
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {

        //    }
        //}

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


    }
}