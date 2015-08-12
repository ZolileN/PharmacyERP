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
    public partial class ManageProduct : System.Web.UI.Page
    {

        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                #region Populating Product Department DropDown
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("Sp_GetDepartmentList", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductDept.DataSource = ds.Tables[0];
                    ProductDept.DataTextField = "Name";
                    ProductDept.DataValueField = "DepId";
                    ProductDept.DataBind();
                    if (ProductDept != null)
                    {
                        ProductDept.Items.Insert(0, "Select Department");
                        ProductDept.SelectedIndex = 0;
                    }
                }
                catch (Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                   
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                }
                #endregion
            }
        }
    }
}