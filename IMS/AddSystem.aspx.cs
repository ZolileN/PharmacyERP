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
    public partial class AddSystem : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["Action"].Equals("Edit"))
                {
                    bindValues();
                    btnAddSystem.Visible = false;
                    btnCancelSystem.Visible = true;
                    btnEditSystem.Visible = true;
                    btnDeleteSystem.Visible = true;
                    selSys.Visible = true;
                    SysDDL.Visible = true;

                    if (Session["SysToAdd"].Equals(RoleNames.store))
                    {
                        regTitleSt.Visible = false;
                        regTitleWH.Visible = false;
                        EditTitleWH.Visible = false;
                        EditTitleSt.Visible = true;
                    }
                    else
                    {
                        regTitleSt.Visible = false;
                        regTitleWH.Visible = false;
                        EditTitleWH.Visible = true;
                        EditTitleSt.Visible = false;
                    }

                }
                else 
                {
                    if (Session["SysToAdd"].Equals(RoleNames.store))
                    {
                        regTitleSt.Visible = true;
                        regTitleWH.Visible = false;
                        EditTitleWH.Visible = false;
                        EditTitleSt.Visible = false;
                    }
                    
                }
                if(Session["SysToAdd"].Equals(RoleNames.store))
                {
                    lblPhar.Visible = true;
                    pharmacyID.Visible = true;
                    
                }
            }
        }

        private void bindValues()
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

        protected void btnAddSystem_Click(object sender, EventArgs e)
        {
            try
            {  
                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_AddNewSystem", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_Name", sysName.Text.ToString());
                    command.Parameters.AddWithValue("@p_Description", sysDesc.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemRoleName", Session["SysToAdd"].ToString());
                    command.Parameters.AddWithValue("@p_SystemAddress", sysAddress.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemPhone", sysPhone.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemFax", sysFax.Text.ToString());
                    command.Parameters.AddWithValue("@p_PharmacyID", pharmacyID.Text.ToString());
                    command.ExecuteNonQuery();
                    WebMessageBoxUtil.Show("System successfully added");
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
                btnCancelSystem_Click(sender, e);
            }
        }

        protected void btnDeleteSystem_Click(object sender, EventArgs e)
        {
             int val;
             if (int.TryParse(sysID.Text, out val))
             {
                 try
                 {
                     connection.Open();
                     SqlCommand command = new SqlCommand("Sp_DeleteSystem", connection);
                     command.CommandType = CommandType.StoredProcedure;

                     command.Parameters.AddWithValue("@p_SystemID", val);


                     command.ExecuteNonQuery();
                     WebMessageBoxUtil.Show("System successfully deleted");
                 }
                 catch (Exception exp) { }
                 finally
                 {
                     connection.Close();
                     btnCancelSystem_Click(sender, e);
                     bindValues();
                 }
             }
        }

        protected void btnCancelSystem_Click(object sender, EventArgs e)
        {
            SysDDL.SelectedIndex = -1;
            sysDesc.Text = String.Empty;
            sysFax.Text = String.Empty;
            sysID.Text = String.Empty;
            sysName.Text = String.Empty;
            sysPhone.Text = String.Empty;
            sysAddress.Text = string.Empty;
            pharmacyID.Text = string.Empty;
            btnDeleteSystem.Enabled = false;
            btnEditSystem.Enabled = false;
            
        }

      

        protected void btnEditSystem_Click(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(sysID.Text, out val))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_UpdateSystems", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_Name", sysName.Text.ToString());
                    command.Parameters.AddWithValue("@p_Description", sysDesc.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemID", val);
                    command.Parameters.AddWithValue("@p_SystemAddress", sysAddress.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemPhone", sysPhone.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemFax", sysFax.Text.ToString());
                    command.Parameters.AddWithValue("@p_PharmacyID", pharmacyID.Text.ToString());
                    command.ExecuteNonQuery();
                    WebMessageBoxUtil.Show("System successfully updated");
                }
                catch (Exception exp) { }
                finally
                {
                    connection.Close();
                    btnCancelSystem_Click(sender, e);
                }
                
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            if (Session["SysToAdd"].Equals(RoleNames.warehouse))
            {
                Response.Redirect("ManageWarehouse.aspx", false);
            }
            else 
            {
                Response.Redirect("ManageStore.aspx", false);
            }
        }


        protected void SysDDL_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                int val;
                if (int.TryParse(SysDDL.SelectedValue, out val))
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_SystemID", val);
                    DataSet ds = new DataSet();

                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    if (ds.Tables[0].Rows[0]["SystemName"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemName"].Equals(""))
                    {
                        sysName.Text = ds.Tables[0].Rows[0]["SystemName"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["Description"] != DBNull.Value || !ds.Tables[0].Rows[0]["Description"].Equals(""))
                    {
                        sysDesc.Text = ds.Tables[0].Rows[0]["Description"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["SystemAddress"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemAddress"].Equals(""))
                    {
                        sysAddress.Text = ds.Tables[0].Rows[0]["SystemAddress"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["SystemPhone"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemPhone"].Equals(""))
                    {
                        sysPhone.Text = ds.Tables[0].Rows[0]["SystemPhone"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["SystemFax"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemFax"].Equals(""))
                    {
                        sysFax.Text = ds.Tables[0].Rows[0]["SystemFax"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["System_PharmacyID"] != DBNull.Value || !ds.Tables[0].Rows[0]["System_PharmacyID"].Equals(""))
                    {
                        pharmacyID.Text = ds.Tables[0].Rows[0]["System_PharmacyID"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["SystemID"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemID"].Equals(""))
                    {
                        sysID.Text = ds.Tables[0].Rows[0]["SystemID"].ToString();
                    }

                    btnDeleteSystem.Enabled = true;
                    btnEditSystem.Enabled = true;
                }
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
            }
        }
    }
}