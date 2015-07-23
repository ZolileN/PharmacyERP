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
                    #region Getting User Roles
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("Sp_GetUserRoles", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        DataSet ds = new DataSet();
                        SqlDataAdapter SA = new SqlDataAdapter(command);
                    
                        SA.Fill(ds);
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
                //String Query = "SELECT UserID,U_RolesID, U_EmpID,Address,Contact,U_RolesID,user_RoleName From [tbl_UserRoles] inner join tbl_Users on tbl_Users.U_RolesID=[tbl_UserRoles].user_RoleID";
                // String Query = "SELECT * From tbl_Users";
               
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_userID", DBNull.Value);
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
            //int i;
            //GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
            //bool id = int.TryParse(SalemanDisplayGrid.Rows[e.RowIndex].ToString(), out i);
            ////int userid = int.Parse(SalemanDisplayGrid.SelectedRow.Cells[0].Text);
            //Label label = (Label)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("lblUserID");
            //TextBox ItemName = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Name");
            //TextBox ItemAddress = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Address");
            //TextBox ItemContact = (TextBox)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("Phone");
            //// TextBox ItemUserRole = (TextBox)SalemanDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("ddlUserRole");
            //DropDownList ddluserRole = (DropDownList)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("ddlUserRole");

            //SalemanDisplayGrid.EditIndex = -1;

            //connection.Open();

            //SqlCommand cmd = new SqlCommand("update tbl_Users set [U_EmpID]='" + ItemName.Text + "',address='" + ItemAddress.Text + "',[Contact]='" + ItemContact.Text + "'where [UserID]='" + label.Text + "'", connection);
            //cmd.ExecuteNonQuery();

            //connection.Close();

            //BindGrid();



        }

        protected void SalemanDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
                Label label = (Label)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("lblUserID");

                SalemanDisplayGrid.EditIndex = -1;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                SqlCommand command = new SqlCommand("Sp_DeleteUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_userID", long.Parse(label.Text));
                command.ExecuteNonQuery();
               
                BindGrid();

            }
            catch (Exception exp) { }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                {
                    connection.Close();
                }
            }
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