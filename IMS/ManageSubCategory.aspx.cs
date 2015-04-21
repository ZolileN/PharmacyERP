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
    public partial class ManageSubCategory : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private DataSet ds;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                try
                {
                    BindGrid(false);
                    BindDropSearch();
                }
                catch (Exception exp) { }
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageInventory.aspx", false);
        }

        protected void SubCategoryDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SubCategoryDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid(false);
        }

        protected void SubCategoryDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SubCategoryDisplayGrid.EditIndex = -1;
            BindGrid(false);
        }

        protected void SubCategoryDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Add"))
                {
                    SubCategoryBLL subCategoryManager = new SubCategoryBLL();
                    TextBox txtname = (TextBox)SubCategoryDisplayGrid.FooterRow.FindControl("txtAddname");
                    string catName = ((DropDownList)(SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddCategoryName"))).SelectedItem.Text;
                    string depName = ((DropDownList)(SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName"))).SelectedItem.Text;
                    SubCategory subCategoryToAdd = new SubCategory();
                    subCategoryToAdd.Name = txtname.Text;
                    subCategoryToAdd.CategoryName = catName;
                    subCategoryToAdd.DepartmentName = depName;
                    subCategoryManager.Add(subCategoryToAdd, connection);

                }
                else if (e.CommandName.Equals("UpdateSubCategory"))
                {
                    SubCategoryBLL subCategoryManager = new SubCategoryBLL();
                    Label id = (Label)SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("lblSubCat_ID");
                    TextBox name = (TextBox)SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("txtname");
                    DropDownList ddlCat = (DropDownList)(SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("ddlCategoryName"));
                    string catName = ddlCat.SelectedItem.Text;

                    DropDownList ddlDep = (DropDownList)(SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("ddlDepName"));
                    string depName = ddlDep.SelectedItem.Text;

                    int selectedId = int.Parse(id.Text);
                    SubCategory subCategoryToUpdate = new SubCategory();//= empid.Text;
                    subCategoryToUpdate.SubCategoryID = selectedId;
                    subCategoryToUpdate.Name = name.Text;
                    subCategoryToUpdate.CategoryName = catName;
                    subCategoryToUpdate.DepartmentName = depName;

                    subCategoryManager.Update(subCategoryToUpdate, connection);

                }
            }
            catch (Exception exp) { }
            finally
            {
                SubCategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void SubCategoryDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                SubCategoryBLL subCategoryManager = new SubCategoryBLL();
                Label ID = (Label)SubCategoryDisplayGrid.Rows[e.RowIndex].FindControl("lblSubCat_ID");
                int selectedId = int.Parse(ID.Text);
                SubCategory subCategoryToDelete = new SubCategory();//= empid.Text;
                subCategoryToDelete.SubCategoryID = selectedId;
                subCategoryManager.Delete(subCategoryToDelete, connection);


            }
            catch (Exception exp) { }
            finally
            {
                SubCategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void SubCategoryDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            SubCategoryDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid(false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid(true);
        }
        private void BindGrid(bool isSearch)
        {
            //if (ddlSubCatName.SelectedIndex != -1 && isSearch)
            //{
            //    SubCategoryBLL sub = new SubCategoryBLL();
            //    SubCategory obj = new SubCategory();

            //    obj.SubCategoryID = int.Parse(ddlSubCatName.SelectedValue);
            //    ds = sub.GetById(obj);

            //}
            //else
            {
                ds = SubCategoryBLL.GetAllSubCategories(connection);

            }
            SubCategoryDisplayGrid.DataSource = ds;
            SubCategoryDisplayGrid.DataBind();

            DropDownList catList = (DropDownList)SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddCategoryName");
            catList.DataSource = CategoryBLL.GetDistinct(connection);
            catList.DataBind();
            catList.DataTextField = "categoryName";
            //catList.DataValueField = "categoryID";
            catList.DataBind();

            DropDownList depList = (DropDownList)SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName");
            string catId = ((DropDownList)(SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddCategoryName"))).SelectedItem.Text;
            Category obj2 = new Category();
            obj2.Name = catId;
            CategoryBLL ins = new CategoryBLL();
            depList.DataSource = ins.GetDepListByCategoryName(obj2,connection);
            depList.DataBind();
            depList.DataTextField = "Name";
            depList.DataValueField = "DepId";
            depList.DataBind();

        }

        protected void SubCategoryDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow && SubCategoryDisplayGrid.EditIndex == e.Row.RowIndex)
            {
                try
                {
                    DropDownList catList = (DropDownList)e.Row.FindControl("ddlCategoryName");
                    catList.DataSource = CategoryBLL.GetDistinct(connection);
                    catList.DataBind();
                    catList.DataTextField = "categoryName";
                    // catList.DataValueField = "categoryID";
                    catList.DataBind();

                    DropDownList depList = (DropDownList)e.Row.FindControl("ddlDepName");
                    string catId = ((DropDownList)(e.Row.FindControl("ddlCategoryName"))).SelectedItem.Text;
                    Category obj2 = new Category();
                    obj2.Name = catId;
                    CategoryBLL ins = new CategoryBLL();
                    depList.DataSource = ins.GetDepListByCategoryName(obj2, connection);
                    depList.DataBind();
                    depList.DataTextField = "Name";
                    depList.DataValueField = "DepId";
                    depList.DataBind();

                }
                catch (Exception exo)
                { }
            }
        }



        private void BindDropSearch()
        {

            //ddlSubCatName.DataSource = SubCategoryBLL.GetAllSubCategories();
            //ddlSubCatName.Items.Insert(0, new ListItem("Select SubCategory", ""));
            //ddlSubCatName.DataTextField = "subCatName";
            //ddlSubCatName.DataValueField = "subCatID";

            //ddlSubCatName.DataBind();
        }

        protected void ddlAddCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            DropDownList depList = (DropDownList)SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName");
            string catId = ((DropDownList)(SubCategoryDisplayGrid.FooterRow.FindControl("ddlAddCategoryName"))).SelectedItem.Text;
            Category obj2 = new Category();
            obj2.Name = catId;
            CategoryBLL ins = new CategoryBLL();
            depList.DataSource = ins.GetDepListByCategoryName(obj2, connection);
            depList.DataBind();
            depList.DataTextField = "Name";
            depList.DataValueField = "DepId";
            depList.DataBind();
        }

        protected void ddlCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {

            DropDownList depList = (DropDownList)SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("ddlDepName");
            string catId = ((DropDownList)(SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex].FindControl("ddlCategoryName"))).SelectedItem.Text;
            Category obj2 = new Category();
            obj2.Name = catId;
            CategoryBLL ins = new CategoryBLL();
            depList.DataSource = ins.GetDepListByCategoryName(obj2, connection);
            depList.DataBind();
            depList.DataTextField = "Name";
            depList.DataValueField = "DepId";
            depList.DataBind();
        }
    }
}