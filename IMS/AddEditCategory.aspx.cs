using IMSBusinessLogic;
using IMSCommon;
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
        
        protected void Page_Load(object sender, EventArgs e)
        {
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
                catch
                {

                }
                
            }
        }

        private void PopulateDepartmentsDD()
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

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        { 
            Response.Redirect("ManageCategory.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageCategory.aspx");
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

                Response.Redirect("ManageCategory.aspx");
            }
            catch(Exception ex)
            {

            }
            
        }
    }
}