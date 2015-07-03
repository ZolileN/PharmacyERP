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
    public partial class SelectWarehouse : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack) 
            {
                #region Populating Warehouse
                try
                {
                    DataSet dsS = new DataSet();
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();

                    }
                    SqlCommand command = new SqlCommand("Sp_GetSystems_ByRoles", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@p_RoleName", "Warehouse");
                    command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(dsS);
                    ddlWH.DataSource = dsS.Tables[0];
                    ddlWH.DataTextField = "SystemName";
                    ddlWH.DataValueField = "SystemID";
                    ddlWH.DataBind();
                    if (ddlWH != null)
                    {
                        ddlWH.Items.Insert(0, "Select Store");
                        ddlWH.SelectedIndex = 0;
                    }
                   
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                }
                #endregion
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            if (ddlWH.SelectedIndex > 0)
            {
                Session["WH_Name"] = ddlWH.SelectedItem.ToString();
                Session["WH_ID"] = ddlWH.SelectedValue;
                Response.Redirect("ItemRequestWH.aspx");
            }
        }
    }
}