using IMS_PharmacyReports.Util;
//using IMSCommon.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using IMSBusinessLogic;
namespace IMS_PharmacyReports
{
    public partial class IMSLogin : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());

        private ExceptionHandler expHandler= ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
         //   expHandler.CheckForErrorMessage(Session);

            if (!IsPostBack)
            {

                String key = null;
                String UserName = null;

                try
                {
                    key = Request.Form["auth_key"].ToString();
                    UserName = Request.Form["UserName"].ToString();
                }
                catch (NullReferenceException ex)
                {

                }

                if (key != null && UserName != null)
                {

                    Session["key"] = key = Request.Form["auth_key"].ToString();

                    string Password = StringCipher.Decrypt(key, "oouEAoBOOoRQy93PA2BmOQ");

                    Session["LoginID"] = UserName = Request.Form["UserName"].ToString();

                    try
                    {
                        DropDownList userList = new DropDownList();
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_GetAllUsers", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        DataSet ds = new DataSet();
                        SqlDataAdapter sA = new SqlDataAdapter(command);
                        sA.Fill(ds);
                        userList.DataSource = ds.Tables[0];
                        userList.DataTextField = "U_EmpID";
                        userList.DataValueField = "U_Password";
                        userList.DataBind();

                        if (userList.Items.FindByText(UserName) != null)
                        {
                            string orgPass = userList.Items.FindByText(UserName).Value;
                            if (orgPass.ToLower().Equals(Password.ToLower()))
                            {
                                DataTable dt = new DataTable();
                                DataView dv = new DataView();
                                dv = ds.Tables[0].DefaultView;
                                dv.RowFilter = "U_EmpID = '" + UserName + "'";
                                dt = dv.ToTable();

                                Session["firstNamelastName"] = dt.Rows[0]["U_FirstName"].ToString() + " " + dt.Rows[0]["U_LastName"].ToString();

                                switch (dt.Rows[0]["RoleName"].ToString())
                                {
                                    case "WareHouse":
                                        Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                        Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                        Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                                        Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                        Session["isHeadOffice"] = false;
                                        Session["UserRole"] = "WareHouse";
                                        System.Diagnostics.Debug.WriteLine("Logged In");
                                        Response.Redirect("Default.aspx", false);


                                        break;

                                    case "Store":
                                        Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                        Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                        Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                        Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                                        Session["isHeadOffice"] = false;
                                        Session["UserRole"] = "Store";
                                        Session["LoginID"] = UserName;
                                        string StoreKey = StringCipher.Encrypt(Password, "oouEAoBOOoRQy93PA2BmOQ");
                                        Session["key"] = StoreKey;

                                        Response.Redirect("Default.aspx", false);
                                        break;

                                    default:
                                        Response.Redirect("IMSLogin.aspx", false);
                                        break;

                                }
                            }
                        }
                        else
                        {

                            //      WebMessageBoxUtil.Show("Invalid username or password.");
                            return;
                        }

                    }
                    catch (Exception exp)
                    {
                        if (log.IsErrorEnabled)
                        {
                            log.Error(expHandler.GenerateLogString(exp));
                        }
                    }
                    finally
                    {
                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                    }

                }


            }

        }

        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            // Void Page_Load(System.Object, System.EventArgs)
            // Handle specific exception.
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
               // expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
             //   expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            // Clear the error from the server.
            Server.ClearError();
        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownList userList = new DropDownList();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetAllUsers", connection);
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                userList.DataSource = ds.Tables[0];
                userList.DataTextField = "U_EmpID";
                userList.DataValueField = "U_Password";
                userList.DataBind();

                if (userList.Items.FindByText(UserName.Text) != null)
                {
                    string orgPass = userList.Items.FindByText(UserName.Text).Value;
                    if (orgPass.ToLower().Equals(Password.Text.ToLower()))
                    {
                        DataTable dt = new DataTable();
                        DataView dv = new DataView();
                        dv = ds.Tables[0].DefaultView;
                        dv.RowFilter = "U_EmpID = '" + UserName.Text + "'";
                        dt = dv.ToTable();

                        Session["firstNamelastName"] = dt.Rows[0]["U_FirstName"].ToString() +" "+ dt.Rows[0]["U_LastName"].ToString();

                        switch(dt.Rows[0]["RoleName"].ToString())
                        {
                            case "WareHouse":
                                Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                                Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                Session["isHeadOffice"] = false;
                                Session["UserRole"] = "WareHouse";

                                Session["LoginID"] = UserName.Text;

                                string key = StringCipher.Encrypt(Password.Text, "oouEAoBOOoRQy93PA2BmOQ");
                                Session["key"] = key;

                                Response.Redirect("Default.aspx", false);


                                break;

                            case "Store":
                                Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                                Session["isHeadOffice"] = false;
                                Session["UserRole"] = "Store";
                                Session["LoginID"] = UserName.Text;
                                string StrKey = StringCipher.Encrypt(Password.Text, "oouEAoBOOoRQy93PA2BmOQ");
                                Session["key"] = StrKey;

                                Response.Redirect("Default.aspx", false);
                                break;

                            default:


                                Response.Redirect("IMSLogin.aspx", false);
                                break;


                        }
                    }
                }
                else
                {

                  //  WebMessageBoxUtil.Show("Invalid username or password.");
                    return;
                }
                
            }
            catch (Exception exp)
            {
                if (log.IsErrorEnabled)
                {
                    log.Error(expHandler.GenerateLogString(exp));
                }
            }
            finally 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
    }
}