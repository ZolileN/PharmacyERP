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

namespace IMS.UserControl
{
    public partial class uc_Select_System : System.Web.UI.UserControl
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public bool SetValue 
        {
            set 
            {
                bindValues();
            }
        }
        private void bindValues()
        {
            if (Session["SysToAdd"] != null)
            {
                try
                {
                    //sys ddl action is set true;
                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_GetSystems_ByRoles", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_RoleName", Session["SysToAdd"].ToString());
                    DataSet ds = new DataSet();

                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    SysDDL.DataSource = null;
                    SysDDL.DataSource = ds.Tables[0];
                    SysDDL.DataBind();

                    SysDDL.DataTextField = "SystemName";
                    SysDDL.DataValueField = "SystemID";
                    SysDDL.DataBind();
                    //
                    if (SysDDL != null)
                    {
                        SysDDL.Items.Insert(0, "Select System");
                        SysDDL.SelectedIndex = 0;
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

        protected void btnSelSystem_Click(object sender, EventArgs e)
        {
            Session["UserSys"] = SysDDL.SelectedValue;
            if (Session["SysToAdd"].Equals(RoleNames.warehouse))
            {
                Response.Redirect("WarehouseMain.aspx", false);
            }
            else
            {
                Response.Redirect("StoreMain.aspx", false);
            }
            
        }
    }
}