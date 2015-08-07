using IMS.Util;
using IMSCommon.Util;
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

namespace IMS
{
    public partial class AddSystem : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    if (Session["Action"].Equals("Edit"))
                    {

                        btnAddSystem.Visible = false;
                        btnCancelSystem.Visible = true;
                        btnEditSystem.Visible = true;
                        btnDeleteSystem.Visible = true;
                        lblStoreType.Visible = true;
                        ddlPharmacyType.Visible = true;

                        if (Session["SystemId"] != null)
                        {
                            regTitleSt.Visible = false;
                            regTitleWH.Visible = false;
                            EditTitleWH.Visible = true;
                            EditTitleSt.Visible = false;
                            LoadData();
                        }

                        if (Session["SysToAdd"].Equals(RoleNames.store))
                        {
                            regTitleSt.Visible = false;
                            regTitleWH.Visible = false;
                            EditTitleWH.Visible = false;
                            EditTitleSt.Visible = true;
                            lblStoreType.Visible = true;
                            ddlPharmacyType.Visible = true;
                            FillPharmacyTypes();
                        }
                        else
                        {
                            regTitleSt.Visible = false;
                            regTitleWH.Visible = false;
                            EditTitleWH.Visible = true;
                            EditTitleSt.Visible = false;
                            lblStoreType.Visible = false;
                            ddlPharmacyType.Visible = false;
                        }

                    }
                    else
                    {
                        btnAddSystem.Visible = true;
                        if (Session["SysToAdd"].Equals(RoleNames.store))
                        {
                            regTitleSt.Visible = true;
                            regTitleWH.Visible = false;
                            EditTitleWH.Visible = false;
                            EditTitleSt.Visible = false;
                            lblStoreType.Visible = true;
                            ddlPharmacyType.Visible = true;
                            FillPharmacyTypes();
                        }

                    }

                    if (Session["SysToAdd"].Equals(RoleNames.store))
                    {
                        lblPhar.Visible = true;
                        pharmacyID.Visible = true;
                        lblBarterID.Visible = true;
                        txtBarterValue.Visible = true;

                    }
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        private void LoadData()
        {
            try
            {
                sysID.Text = Session["SystemId"].ToString();
                DataSet dsResults = new DataSet();
                int SystemID = int.Parse(Session["SystemId"].ToString());
                if(connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.Parameters.AddWithValue("@p_SystemID", SystemID);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(dsResults);

               

                sysName.Text = dsResults.Tables[0].Rows[0]["SystemName"].ToString();
                sysDesc.Text = dsResults.Tables[0].Rows[0]["Description"].ToString();
                sysAddress.Text = dsResults.Tables[0].Rows[0]["SystemAddress"].ToString();
                sysPhone.Text = dsResults.Tables[0].Rows[0]["SystemPhone"].ToString();
                sysFax.Text = dsResults.Tables[0].Rows[0]["SystemFax"].ToString();
                txtBarterValue.Text = dsResults.Tables[0].Rows[0]["BarterExchangeID"].ToString();
                pharmacyID.Text = dsResults.Tables[0].Rows[0]["System_PharmacyID"].ToString();
               // txtBarterValue.Text = Session["BarterExchangeID"].ToString();
              //  pharmacyID.Text = Session["PharmacyID"].ToString();

                selSys.Visible = false;
                SysDDL.Visible = false;
                btnAddSystem.Visible = false;
                btnDeleteSystem.Visible = false;
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {

            }
        }

        private void FillPharmacyTypes()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetSystemRoles", connection);
                command.CommandType = CommandType.StoredProcedure;
                 
                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ddlPharmacyType.DataSource = null;
                ddlPharmacyType.DataSource = ds.Tables[0];
                ddlPharmacyType.DataBind(); 
                ddlPharmacyType.DataValueField = "RoleID";
                ddlPharmacyType.DataTextField = "RoleName";
                ddlPharmacyType.DataBind(); 

                if (ddlPharmacyType != null)
                {
                    ddlPharmacyType.Items.Insert(0, "Select Pharmacy Type");
                    //ddlPharmacyType.SelectedIndex = 0;
                    if (Session["SystemRoleID"] != null)
                    {
                        foreach (ListItem Items in ddlPharmacyType.Items)
                        {
                            if (Items.Text.Equals(Session["SystemRoleID"].ToString()))
                            {
                                ddlPharmacyType.SelectedIndex = ddlPharmacyType.Items.IndexOf(Items);
                                break;
                            }
                        }
                        
                    }
                }
            }
            catch (Exception exp)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw exp;
            }
            finally
            {
                connection.Close();
            }
        }
        private void bindValues()
        {
            try
            {
                //sys ddl action is set true;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystems_ByRoles", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_RoleName", Session["SysToAdd"].ToString());
                command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw exp;
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
                int SystemRoles = 0;
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                int.TryParse(ddlPharmacyType.SelectedValue, out SystemRoles);
                 
                SqlCommand command = new SqlCommand("Sp_AddNewSystem", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_Name", sysName.Text.ToString());
                command.Parameters.AddWithValue("@p_Description", sysDesc.Text.ToString());
                command.Parameters.AddWithValue("@p_SystemRoleName", Session["SysToAdd"].ToString());
                command.Parameters.AddWithValue("@p_SystemAddress", sysAddress.Text.ToString());
                command.Parameters.AddWithValue("@p_SystemPhone", sysPhone.Text.ToString());
                command.Parameters.AddWithValue("@p_SystemFax", sysFax.Text.ToString());
                command.Parameters.AddWithValue("@p_PharmacyID", pharmacyID.Text.ToString());
                command.Parameters.AddWithValue("@p_BarterID", txtBarterValue.Text.ToString());
                command.Parameters.AddWithValue("@p_SystemRolesID", SystemRoles);
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("System successfully added");
                if (Session["SysToAdd"].Equals(RoleNames.store))
                {
                    Response.Redirect("PharmacyManagement.aspx");
                }
                else
                {
                    Response.Redirect("WarehouseManagement.aspx");
                }

            }
            catch (Exception exp) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw exp;
            }
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
                 catch (Exception exp) 
                 {
                     if (connection.State == ConnectionState.Open)
                         connection.Close();
                     throw exp;
                 }
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
            txtBarterValue.Text = String.Empty;
            
        }

      

        protected void btnEditSystem_Click(object sender, EventArgs e)
        {
            int val;
            if (int.TryParse(sysID.Text, out val))
            {
                try
                {
                    if (connection.State == ConnectionState.Closed) 
                    {
                        connection.Open();
                    }
                    
                    SqlCommand command = new SqlCommand("Sp_UpdateSystems", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_Name", sysName.Text.ToString());
                    command.Parameters.AddWithValue("@p_Description", sysDesc.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemID", val);
                    command.Parameters.AddWithValue("@p_SystemAddress", sysAddress.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemPhone", sysPhone.Text.ToString());
                    command.Parameters.AddWithValue("@p_SystemFax", sysFax.Text.ToString());
                    command.Parameters.AddWithValue("@p_PharmacyID", pharmacyID.Text.ToString());
                    command.Parameters.AddWithValue("@p_BarterID", txtBarterValue.Text.ToString());

                    command.ExecuteNonQuery();
                    WebMessageBoxUtil.Show("System successfully updated");
                    if (Session["SysToAdd"].Equals(RoleNames.store))
                    {
                        Response.Redirect("PharmacyManagement.aspx");
                    }
                    else
                    {
                        Response.Redirect("WarehouseManagement.aspx");
                    }
                }
                catch (Exception exp) 
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw exp;
                }
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
                Response.Redirect("WarehouseManagement.aspx", false);
            }
            else 
            {
                Response.Redirect("PharmacyManagement.aspx", false);
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
                    if (ds.Tables[0].Rows[0]["BarterExchangeID"] != DBNull.Value || !ds.Tables[0].Rows[0]["BarterExchangeID"].Equals(""))
                    {
                        txtBarterValue.Text = ds.Tables[0].Rows[0]["BarterExchangeID"].ToString();
                    }

                    if (ds.Tables[0].Rows[0]["SystemID"] != DBNull.Value || !ds.Tables[0].Rows[0]["SystemID"].Equals(""))
                    {
                        sysID.Text = ds.Tables[0].Rows[0]["SystemID"].ToString();
                    }

                    btnDeleteSystem.Enabled = true;
                    btnEditSystem.Enabled = true;
                }
            }
            catch (Exception exp) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw exp;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}