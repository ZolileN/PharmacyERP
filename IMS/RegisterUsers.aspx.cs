﻿using System;
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


namespace IMS
{
    public partial class RegisterUsers : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private string USERID = string.Empty;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Populating User Role Drop Down DropDown
                try
                {
                    if (connection.State == ConnectionState.Open)
                    { connection.Close();
                    }
                    connection.Open();
                    SqlCommand command = new SqlCommand("Select * From tbl_UserRoles", connection);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ddlURole.DataSource = ds.Tables[0];
                    ddlURole.DataTextField = "user_RoleName";
                    ddlURole.DataValueField = "user_RoleID";
                    ddlURole.DataBind();
                    if (ddlURole != null)
                    {
                        ddlURole.Items.Insert(0, "Select User Role");
                        ddlURole.SelectedIndex = 3;
                        ddlURole.Enabled = false;
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

                #region Populating System Drop Down DropDown
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("SELECT *  FROM [dbo].[tbl_System] where System_RoleID=1 or System_RoleID=3", connection);
                    DataSet ds1 = new DataSet();
                    SqlDataAdapter sA1 = new SqlDataAdapter(command);
                    sA1.Fill(ds1);
                    ddlSysID.DataSource = ds1.Tables[0];
                    ddlSysID.DataTextField = "SystemName";
                    ddlSysID.DataValueField = "SystemID";
                    ddlSysID.DataBind();
                    if (ddlSysID != null)
                    {
                        ddlSysID.Items.Insert(0, "Select System ");
                        ddlSysID.SelectedIndex = 0;
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

                USERID = Request.QueryString["ID"];
                // Start here
                if (!string.IsNullOrEmpty(USERID))
                { UpdateSaleman(USERID); }
                else
                {
                    btnSave.Visible = false;
                    btnAssociatedStore.Visible = false;
                }

                // End Here
            }
            
        }

        private void UpdateSaleman(string var)
        {
            int x = 0;

            btnAssociatedStore.Visible = true;

            //Start here SP 
            SqlCommand command = new SqlCommand("Sp_SearchByUser_ID", connection);
            command.CommandType = CommandType.StoredProcedure;

            DataSet ds2 = new DataSet();

            command.Parameters.AddWithValue("@p_UserID", var);

            SqlDataAdapter sA = new SqlDataAdapter(command);
            sA.Fill(ds2);
            EmpID.Text = ds2.Tables[0].Rows[0]["U_EmpID"].ToString();
            //uPwd.Text = ds2.Tables[0].Rows[0]["U_Password"].ToString();
            string PwdValue = ds2.Tables[0].Rows[0]["U_Password"].ToString();
            uPwd.TextMode = TextBoxMode.SingleLine;
            uPwd.Text = PwdValue;
            //uPwd.TextMode = TextBoxMode.Password;
            ddlURole.SelectedValue = ds2.Tables[0].Rows[0]["U_RolesID"].ToString();
            ddlURole.Enabled = false;
            fName.Text = ds2.Tables[0].Rows[0]["U_FirstName"].ToString();
            lstName.Text = ds2.Tables[0].Rows[0]["U_LastName"].ToString();
            Address.Text = ds2.Tables[0].Rows[0]["Address"].ToString();
            ContactNo.Text = ds2.Tables[0].Rows[0]["Contact"].ToString();
            btnAddEmployee.Visible = false;
            //End Here SP
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("salemanMangment.aspx", false);
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            int x = 0;
            String Errormessage = "";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_AddNewUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_EmpID", EmpID.Text);
                command.Parameters.AddWithValue("@p_password", uPwd.Text);
                command.Parameters.AddWithValue("@p_UserRoleID", ddlURole.SelectedValue.ToString());
                command.Parameters.AddWithValue("@p_SystemID", ddlSysID.SelectedValue.ToString());
                command.Parameters.AddWithValue("@p_FirstName", fName.Text);
                command.Parameters.AddWithValue("@p_LastName", lstName.Text);
                command.Parameters.AddWithValue("@p_Contact", ContactNo.Text);
                command.Parameters.AddWithValue("@p_Address", Address.Text);
                command.Parameters.AddWithValue("@ReturnOut", SqlDbType.Int).Direction = ParameterDirection.Output; ;
                command.Parameters.AddWithValue("@p_Name", "");
                command.Parameters.AddWithValue("@p_DisplayName", "");
                command.Parameters.AddWithValue("@p_email", "");
                x = command.ExecuteNonQuery();
                x = Convert.ToInt32(command.Parameters["@ReturnOut"].Value);
            }
            catch (Exception ex)
            {
                Errormessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }


            if (x == 2)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID =  " + EmpID.Text + " saved Successfully')", true);
                //EmpID.Text = "";
                //uPwd.Text = "";
                //ddlURole.SelectedIndex = 3;
                //ddlSysID.SelectedIndex = -1;
                //fName.Text = "";
                //lstName.Text = "";
                //Address.Text = "";
                //ContactNo.Text = "";
                btnAssociatedStore.Visible = true;
                btnAddEmployee.Visible = false;
                btnSave.Visible = true;
            }
            else if (x == -2)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID = " + EmpID.Text + " already eist.')", true);
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(" + Errormessage + ")", true);
            }

        }

        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            // btnAddEmployee_Click(null, null);
            string var = Request.QueryString["ID"];
            int x = 0;
            String Errormessage = "";

            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateNewUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_UserID", var);

                command.Parameters.AddWithValue("@p_EmpID", EmpID.Text);
                command.Parameters.AddWithValue("@p_password", uPwd.Text);
                command.Parameters.AddWithValue("@p_UserRoleID", ddlURole.SelectedValue.ToString());
                command.Parameters.AddWithValue("@p_SystemID", ddlSysID.SelectedValue.ToString());
                command.Parameters.AddWithValue("@p_FirstName", fName.Text);
                command.Parameters.AddWithValue("@p_LastName", lstName.Text);
                command.Parameters.AddWithValue("@p_Contact", ContactNo.Text);
                command.Parameters.AddWithValue("@p_Address", Address.Text);

                // command.Parameters.AddWithValue("@p_Name", "");
                //command.Parameters.AddWithValue("@p_DisplayName", "");
                // command.Parameters.AddWithValue("@p_email", "");

                x = command.ExecuteNonQuery();



            }
            catch (Exception ex)
            {
                Errormessage = ex.Message;
            }
            finally
            {
                connection.Close();
            }


            if (x == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID " + EmpID.Text + " updated successfully.')", true);
                //string NewID = EmpID.Text;
                //uPwd.Text = "";
                //ddlURole.SelectedIndex = -1;
                //ddlSysID.SelectedIndex = -1;
                //fName.Text = "";
                //lstName.Text = "";
                //Address.Text = "";
                //ContactNo.Text = "";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(''" + Errormessage + "'')", true);
            }
        }

        protected void btnAssociatedStore_Click(object sender, EventArgs e)
        {
            string USERID = Request.QueryString["ID"];
            connection.Open();
            SqlCommand commandUserID = new SqlCommand("SELECT MAX(UserID) from [tbl_Users]", connection);

            long id = Convert.ToInt64(commandUserID.ExecuteScalar());
            if (string.IsNullOrWhiteSpace(USERID))
            {

                Response.Redirect("UserStoreManagment.aspx?ID=" + id);
            }
            else
            {
                Response.Redirect("UserStoreManagment.aspx?ID=" + USERID);
            }
            connection.Close();
        }
    }
}