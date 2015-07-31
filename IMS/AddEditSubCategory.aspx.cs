using IMS.Util;
using IMSBusinessLogic;
using IMSCommon;
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
    public partial class AddEditSubCategory : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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
                
                PopulateddDepartment();
                if (Convert.ToInt32(Session["subcatid"].ToString()) > 0)
                {
                    txtSubCategoryName.Text = Session["subcatname"].ToString();
                    ddDepartment.SelectedIndex= 1;
                    
                    foreach (ListItem Items  in ddCategory.Items) 
                    {
                        if (Items.Text.Equals(Session["catname"].ToString())) 
                        {
                           ddCategory.SelectedIndex = ddCategory.Items.IndexOf( Items);
                            break;
                        }
                    }
                   
                    btnSaveSubCategory.Text = "Update";
                }
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
        private void PopulateddDepartment()
        {
            #region Populating Department DropDown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetDepartmentList", connection);
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ddDepartment.DataSource = ds.Tables[0];
                ddDepartment.DataTextField = "Name";
                ddDepartment.DataValueField = "DepId";
                ddDepartment.DataBind();
                if (ddDepartment != null)
                {
                    ddDepartment.Items.Insert(0, "Select Department");
                    ddDepartment.SelectedIndex = 0;
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
        }
    
        private void PopulateddCategory()
        {
            #region Populating Category DropDown
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetCategoryList", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_deptID", Convert.ToInt32(ddDepartment.SelectedValue.ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ddCategory.DataSource = ds.Tables[0];
                ddCategory.DataTextField = "categoryName";
                ddCategory.DataValueField = "categoryID";
                ddCategory.DataBind();
                if (ddCategory != null)
                {
                    ddCategory.Items.Insert(0, "Select Category");
                    ddCategory.SelectedIndex = 0;
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
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageSubCategory.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageSubCategory.aspx",false);
        }

        protected void btnSaveSubCategory_Click(object sender, EventArgs e)
        {
            try
            {
                SubCategoryBLL subCategoryManager = new SubCategoryBLL();
                if (Convert.ToInt32(Session["subcatid"].ToString()) > 0)
                {
                    string catId = ddCategory.SelectedValue;

                    string depName = ddDepartment.SelectedItem.Text;

                    int selectedId = int.Parse(Session["subcatid"].ToString());
                    SubCategory subCategoryToUpdate = new SubCategory();
                    subCategoryToUpdate.SubCategoryID = selectedId;
                    subCategoryToUpdate.Name = txtSubCategoryName.Text;
                    subCategoryToUpdate.CategoryID = int.Parse(catId);

                    subCategoryManager.UpdateSubCat(subCategoryToUpdate, connection);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SubCategory SuccessFully Updated.')", true);

                }
                else
                {
                    SubCategory subCategoryToAdd = new SubCategory();
                    subCategoryToAdd.Name = txtSubCategoryName.Text;
                    subCategoryToAdd.CategoryID = Convert.ToInt32(ddCategory.Text);

                    subCategoryManager.AddNew(subCategoryToAdd, connection);
                }
                Response.Redirect("ManageSubCategory.aspx",false);

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        protected void ddDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateddCategory();
        }

    }
}