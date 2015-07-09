using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class IMSLogin : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnLogin_Click(object sender, EventArgs e)
        {
            try
            {
                DropDownList userList = new DropDownList();
                connection.Open();
                SqlCommand command = new SqlCommand("Select * From tbl_Users INNER JOIN tbl_System ON tbl_Users.SystemID = tbl_System.SystemID " +
                    "INNER JOIN tbl_SystemRoles ON tbl_System.System_RoleID = tbl_SystemRoles.RoleID", connection);
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
                        
                        switch(dt.Rows[0]["RoleName"].ToString())
                        {
                            case "WareHouse":
                                Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                Session["isHeadOffice"] = false;
                                Session["UserRole"] = "WareHouse";
                                Response.Redirect("WarehouseMain.aspx",false);
                                break;
                            case "Store":
                                Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                Session["UserSys"] = dt.Rows[0]["SystemID"].ToString();
                                Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                Session["isHeadOffice"] = false;
                                Session["UserRole"] = "Store";
                                Response.Redirect("StoreMain.aspx",false);
                                break;
                            case "HeadOffice":
                                Session["UserName"] = dt.Rows[0]["SystemName"].ToString();
                                Session["isHeadOffice"] = true;
                                Session["UserEmail"] = dt.Rows[0]["U_Email"].ToString();
                                Session["UserRole"] = "HeadOffice";
                                Response.Redirect("HeadOfficeMain.aspx", false);
                                break;
                               
                        }
                    }
                }
                else
                {

                    WebMessageBoxUtil.Show("Invalid username or password.");
                    return;
                }
                
            }
            catch (Exception exp)
            {
            }
            finally 
            {
                connection.Close();
            }
        }
    }
}