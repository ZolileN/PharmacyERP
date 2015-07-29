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
    public partial class AddEditSubCategory : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
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

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }
        protected void btnCancel_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageSubCategory.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageSubCategory.aspx");
        }

        protected void btnSaveSubCategory_Click(object sender, EventArgs e)
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
            Response.Redirect("ManageSubCategory.aspx");
            
        }

        protected void ddDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateddCategory();
        }

    }
}