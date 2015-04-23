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


namespace IMS
{
    public partial class RegisterUsers : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                #region Populating User Role Drop Down DropDown
                try
                {
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
                        ddlURole.SelectedIndex = 0;
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
                    SqlCommand command = new SqlCommand("Select * From tbl_System", connection);
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

                EmpID.Text = "";
                uPwd.Text = "";

                string var = Request.QueryString["ID"];
                // Start here
                if (!string.IsNullOrEmpty(var))
                { UpdateSaleman(var); }
                else
                {
                    btnSave.Visible = false;
                }
                // End Here
            }
        }

        private void UpdateSaleman(string var)
        {
            SqlCommand command1;
            command1 = new SqlCommand("SELECT *  FROM [dbo].[tbl_Users] where UserID=" + var + "", connection);
            DataSet ds2 = new DataSet();
            SqlDataAdapter sA2 = new SqlDataAdapter(command1);
            sA2.Fill(ds2);
            ddlSysID.DataSource = ds2.Tables[0];
            EmpID.Text = ds2.Tables[0].Rows[0]["U_EmpID"].ToString();
            // EmpID.Enabled = false;
            uPwd.Text = ds2.Tables[0].Rows[0]["U_Password"].ToString();
            //uPwd.Enabled = false;
            ddlURole.SelectedValue = ds2.Tables[0].Rows[0]["U_RolesID"].ToString();
            ddlURole.Enabled = false;
            fName.Text = ds2.Tables[0].Rows[0]["U_FirstName"].ToString();
            lstName.Text = ds2.Tables[0].Rows[0]["U_LastName"].ToString();
            Address.Text = ds2.Tables[0].Rows[0]["Address"].ToString();
            ContactNo.Text = ds2.Tables[0].Rows[0]["Contact"].ToString();
            btnAddEmployee.Visible = false;
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

                command.Parameters.AddWithValue("@p_Name", "");
                command.Parameters.AddWithValue("@p_DisplayName", "");
                command.Parameters.AddWithValue("@p_email", "");

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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Inserted Successfully')", true);
                EmpID.Text = "";
                uPwd.Text = "";
                ddlURole.SelectedIndex = -1;
                ddlSysID.SelectedIndex = -1;
                fName.Text = "";
                lstName.Text = "";
                Address.Text = "";
                ContactNo.Text = "";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(''" + Errormessage + "'')", true);
            }
        }

        protected void btnSave_Click(object sender, EventArgs e)
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
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Updated Successfully')", true);
                EmpID.Text = "";
                uPwd.Text = "";
                ddlURole.SelectedIndex = -1;
                ddlSysID.SelectedIndex = -1;
                fName.Text = "";
                lstName.Text = "";
                Address.Text = "";
                ContactNo.Text = "";
            }
            else
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert(''" + Errormessage + "'')", true);
            }
        }
    }
}