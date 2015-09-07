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
using IMSBusinessLogic;


namespace IMS
{
    public partial class RegisterUsers : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private string USERID = string.Empty;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        private UserBLL userBll = new UserBLL();
        private SystemBLL sysBll = new SystemBLL();

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
                    DataSet ds = new DataSet();
                    ds = userBll.SelectUserRoles();
                    ddlURole.DataSource = ds.Tables[0];
                    ddlURole.DataTextField = "user_RoleName";
                    ddlURole.DataValueField = "user_RoleID";
                    ddlURole.DataBind();
                    if (ddlURole != null)
                    {
                        ddlURole.Items.Insert(0, "Select User Role");
                        if (Session["ur_RoleName"] != null && Session["ur_RoleName"].ToString().Equals("Salesman"))
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

                    throw ex;
                }
                
                #endregion

                
                #region Populating System Drop Down DropDown
                try
                {
              
                    DataSet ds1 = new DataSet();

                    if (Session["ur_RoleName"] != null && Session["ur_RoleName"].ToString().Equals("Salesman"))
                    {
                        ds1 = sysBll.SelectAllWH_HO();
                    }
                    else
                    {
                        ds1 = sysBll.SelectAllSystems();
                    }
                   
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
                    
                    throw ex;
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
                DataSet ds2 = new DataSet();
                ds2 = userBll.SelectByID(var);

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
                userBll.Insert( EmployeeID.Text,userPwd.Text,ddlURole.SelectedValue.ToString(),ddlSysID.SelectedValue.ToString(),fName.Text,lstName.Text
                    ,ContactNo.Text,Address.Text,"","","");

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('User saved Successfully')", true);
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
            catch (Exception ex)
            {
                Errormessage = ex.Message;
             
                throw ex;
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
               
                userBll.Update(var,EmployeeID.Text,userPwd.Text,int.Parse(ddlURole.SelectedValue.ToString()),ddlSysID.SelectedIndex > 0?int.Parse(ddlSysID.SelectedValue.ToString()):(int?)null,
                   fName.Text, lstName.Text,ContactNo.Text,Address.Text);

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('User updated successfully.')", true);
                
                if (Session["ur_RoleName"].ToString().Equals("Salesman"))
                {
                    Response.Redirect("SalemanMangment.aspx", false);
                }
                else
                {
                    Response.Redirect("UserManagment.aspx", false);
                }

            }
            catch (Exception ex)
            {
                Errormessage = ex.Message;
              
                throw ex;
            }
            
        }

        protected void btnAssociatedStore_Click(object sender, EventArgs e)
        {
            try
            {
                string USERID = Request.QueryString["ID"];
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
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

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}