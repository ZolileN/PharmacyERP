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
    public partial class AddEditCategory : System.Web.UI.Page
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
            if(!IsPostBack)
            {
                PopulateDepartmentsDD();
                try
                {
                    if (Convert.ToInt32(Session["CatId"].ToString()) > 0)
                    {
                        CategoryName.Text = Session["Catname"].ToString();
                        lblHeading.Text = "Edit Category";
                        foreach (ListItem Items in CategoryDepartment.Items)
                        {
                            if (Items.Text.Equals(Session["depId"].ToString()))
                            {
                                CategoryDepartment.SelectedIndex = CategoryDepartment.Items.IndexOf(Items);
                                break;
                            }
                        }
                        // CategoryDepartment.SelectedItem.Text = Session["depId"].ToString();
                        btnSaveCategory.Text = "Update";
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
        private void PopulateDepartmentsDD()
        {
            #region Populating Department DropDown
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
                CategoryDepartment.DataSource = ds.Tables[0];
                CategoryDepartment.DataTextField = "Name";
                CategoryDepartment.DataValueField = "DepId";
                CategoryDepartment.DataBind();
                if (CategoryDepartment != null)
                {
                    CategoryDepartment.Items.Insert(0, "Select Department");
                    CategoryDepartment.SelectedIndex = 0;
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
            #endregion
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        { 
            Response.Redirect("ManageCategory.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageCategory.aspx",false);
        }

        protected void btnSaveCategory_Click(object sender, EventArgs e)
        {
            try
            {
                CategoryBLL categoryManager = new CategoryBLL();

                if (Convert.ToInt32(Session["CatId"].ToString()) > 0)
                {
                    int selectedId = int.Parse(Session["CatId"].ToString());
                    Category categoryToUpdate = new Category();
                    categoryToUpdate.CategoryID = selectedId;
                    categoryToUpdate.Name = CategoryName.Text;
                    categoryToUpdate.DepartmentID = Convert.ToInt32(CategoryDepartment.SelectedValue);
                    categoryManager.Update(categoryToUpdate, connection);

                }
                else
                {

                    Category categoryToAdd = new Category();
                    categoryToAdd.Name = CategoryName.Text;
                    categoryToAdd.DepartmentID = Convert.ToInt32(CategoryDepartment.SelectedValue);

                    categoryManager.Add(categoryToAdd, connection);
                }

                Session.Remove("Catname");
                Session.Remove("depId");
                Session.Remove("CatId");

                Response.Redirect("ManageCategory.aspx", false);
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