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
using log4net;
using IMS.Util;


namespace IMS
{
    public partial class RegisterUsers : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private string USERID = string.Empty;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack)
            {

                if (Session["ur_RoleName"].ToString().Equals("Salesman"))
                {
                    TitleSlman.Visible = true;
                    Titleuser.Visible = false;
                }
                else 
                {
                    TitleSlman.Visible = false;
                    Titleuser.Visible = true;
                }
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
                        if (Session["ur_RoleName"]!=null && Session["ur_RoleName"].ToString().Equals("Salesman"))
                        {
                            ddlURole.SelectedIndex = 3;
                            ddlURole.Enabled = false;
                        }
                        else
                        {
                            ddlURole.SelectedIndex = 0;
                        }
                        
                    }


                }
                catch (Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
                #endregion

                #region Populating System Drop Down DropDown
                try
                {
                    SqlCommand command = new SqlCommand();
                    connection.Open();
                    if (Session["ur_RoleName"] != null && Session["ur_RoleName"].ToString().Equals("Salesman"))
                    {
                        command = new SqlCommand("SELECT *  FROM [dbo].[tbl_System] where System_RoleID IN (1,3)", connection);
                    }
                    else
                    {
                        command = new SqlCommand("SELECT *  FROM [dbo].[tbl_System] where System_RoleID IN (1,2,3,10007)", connection);
                    }
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
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    connection.Close();
                }
                #endregion

                USERID = Request.QueryString["ID"];
                // Start here
                if (!string.IsNullOrEmpty(USERID))
                {
                    UpdateSaleman(USERID);
                    if (!Session["ur_RoleName"].ToString().Equals("Salesman")) 
                    {
                        btnAssociatedStore.Visible = false;
                    }
                }
                else
                {
                    btnSave.Visible = false;
                    btnAssociatedStore.Visible = false;
                    EmployeeID.Text = String.Empty;
                    userPwd.Text = String.Empty;
                }

                // End Here
            }
            expHandler.CheckForErrorMessage(Session);
        }

        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            // Void Page_Load(System.Object, System.EventArgs)
            // Handle specific exception.
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            // Clear the error from the server.
            Server.ClearError();
        }
        private void UpdateSaleman(string var)
        {
            try
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
                EmployeeID.Text = ds2.Tables[0].Rows[0]["U_EmpID"].ToString();
                //uPwd.Text = ds2.Tables[0].Rows[0]["U_Password"].ToString();
                string PwdValue = ds2.Tables[0].Rows[0]["U_Password"].ToString();
                userPwd.TextMode = TextBoxMode.SingleLine;
                userPwd.Text = PwdValue;
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
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
           
            if (Session["ur_RoleName"].ToString().Equals("Salesman"))
            {
                Session.Remove("ur_RoleName");
                Response.Redirect("SalemanMangment.aspx", false);
            }
            else
            {
                Session.Remove("ur_RoleName");
                Response.Redirect("UserManagment.aspx", false);
            }
        }

        protected void btnAddEmployee_Click(object sender, EventArgs e)
        {
            if (ddlSysID.SelectedIndex == 0) 
            {

                WebMessageBoxUtil.Show("Select System");
                return;
            }
            if (ddlURole.SelectedIndex == 0)
            {

                WebMessageBoxUtil.Show("Select User Role");
                return;
            }
            int x = 0;
            String Errormessage = "";
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_AddNewUser", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_EmpID", EmployeeID.Text);
                command.Parameters.AddWithValue("@p_password", userPwd.Text);
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }


            if (x == 2)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID =  " + EmployeeID.Text + " saved Successfully')", true);
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

                if (Session["ur_RoleName"].ToString().Equals("Salesman"))
                {
                    Response.Redirect("SalemanMangment.aspx", false);
                }
                else
                {
                    Response.Redirect("UserManagment.aspx", false);
                }
            }
            else if (x == -2)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID = " + EmployeeID.Text + " already eist.')", true);
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

                command.Parameters.AddWithValue("@p_EmpID", EmployeeID.Text);
                command.Parameters.AddWithValue("@p_password", userPwd.Text);
                command.Parameters.AddWithValue("@p_UserRoleID", int.Parse(ddlURole.SelectedValue.ToString()));
                if (ddlSysID.SelectedIndex > 0)
                {
                    command.Parameters.AddWithValue("@p_SystemID", int.Parse(ddlSysID.SelectedValue.ToString()));
                }
                else 
                {
                    command.Parameters.AddWithValue("@p_SystemID", DBNull.Value);
                }
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                connection.Close();
            }


            if (x == 1)
            {
                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Salesman with ID " + EmployeeID.Text + " updated successfully.')", true);
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
            if (Session["ur_RoleName"].ToString().Equals("Salesman"))
            {
                Response.Redirect("SalemanMangment.aspx",false);
            }
            else 
            {
                Response.Redirect("UserManagment.aspx", false);
            }
        }

        protected void btnAssociatedStore_Click(object sender, EventArgs e)
        {
            try
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
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }
    }
}