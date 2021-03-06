﻿using IMS.Util;
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
    public partial class ManageCategory : System.Web.UI.Page
    {
        private DataSet ds;
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
                try
                {
                    if (Session["UserRole"].ToString().Equals("Store"))
                    {
                        btnAddCategory.Enabled = false;
                    }
                    BindGrid(false);
                    BindDropSearch();
                }
                catch (Exception ex)
                {
                   
                    throw ex;
                }
                finally 
                {
                   
                }
            }
            expHandler.CheckForErrorMessage(Session);
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageInventory.aspx", false);
        }
        protected void CategoryDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            CategoryDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid(false);
        }

        protected void CategoryDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            CategoryDisplayGrid.EditIndex = -1;
            BindGrid(false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid(true);
        }
        protected void CategoryDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("Add"))
                {
                    CategoryBLL categoryManager = new CategoryBLL();
                    TextBox txtname = (TextBox)CategoryDisplayGrid.FooterRow.FindControl("txtAddname");
                    // TextBox txtDepId = (TextBox)CategoryDisplayGrid.FooterRow.FindControl("txtAddDepID");
                    string depName = (CategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName") as DropDownList).SelectedItem.Value;
                    Category categoryToAdd = new Category();
                    categoryToAdd.Name = txtname.Text;
                    int res;
                    if (int.TryParse(depName, out res))
                    {
                        categoryToAdd.DepartmentID = res;

                        categoryManager.Add(categoryToAdd);
                        
                    }
                    else
                    {
                        //WebMessageBoxUtil.Show("Invalid input in Department field ");
                    }

                }
                else if (e.CommandName.Equals("UpdateCategory"))
                {

                    CategoryBLL categoryManager = new CategoryBLL();
                    Label id = (Label)CategoryDisplayGrid.Rows[CategoryDisplayGrid.EditIndex].FindControl("lblCat_ID");
                    TextBox name = (TextBox)CategoryDisplayGrid.Rows[CategoryDisplayGrid.EditIndex].FindControl("txtname");
                    DropDownList ddlDep = (DropDownList)(CategoryDisplayGrid.Rows[CategoryDisplayGrid.EditIndex].FindControl("ddlDepName"));
                    string depName = ddlDep.SelectedItem.Value;
                    // TextBox departmentId = (TextBox)CategoryDisplayGrid.Rows[e.RowIndex].FindControl("txtDepID");

                    int selectedId = int.Parse(id.Text);
                    Category categoryToUpdate = new Category();//= empid.Text;
                    categoryToUpdate.CategoryID = selectedId;
                    categoryToUpdate.Name = name.Text;
                    int res;
                    if (int.TryParse(depName, out res))
                    {
                        categoryToUpdate.DepartmentID = int.Parse(depName);
                        categoryManager.Update(categoryToUpdate);
                    }
                    else
                    {
                        //  WebMessageBoxUtil.Show("Invalid input in Department field ");
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            finally
            {
               
                CategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void CategoryDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                CategoryBLL categoryManager = new CategoryBLL();
                Label ID = (Label)CategoryDisplayGrid.Rows[e.RowIndex].FindControl("lblCat_ID");
                int selectedId = int.Parse(ID.Text);
                Category categoryToDelete = new Category();//= empid.Text;
                categoryToDelete.CategoryID = selectedId;
                categoryManager.Delete(categoryToDelete);

            }
            catch (Exception ex)
            {
              
                throw ex;
            }
            finally
            {
              
                CategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void CategoryDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CategoryDisplayGrid.EditIndex = e.NewEditIndex;

            GridViewRow row = CategoryDisplayGrid.Rows[CategoryDisplayGrid.EditIndex];
            Label id = (Label)row.FindControl("lblCat_ID");
            Label name = (Label)row.FindControl("lblCat_Name");
            Label depId = (Label)row.FindControl("lblDep_Id");

            Session["Catname"] = name.Text;
            Session["depId"] = depId.Text;
            Session["CatId"] = id.Text;
 
            Response.Redirect("AddEditCategory.aspx",false);

            //BindGrid(false);
        }

        private void BindGrid(bool isSearch)
        {
            try
            {
                ds = CategoryBLL.GetAllCategories();

                CategoryDisplayGrid.DataSource = ds;
                CategoryDisplayGrid.DataBind();

                DropDownList depList = (DropDownList)CategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName");
                depList.DataSource = DepartmentBLL.GetAllDepartment();
                depList.DataBind();
                depList.DataTextField = "Name";
                depList.DataValueField = "DepId";
                depList.DataBind();
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
            finally 
            {
               
            }
        }

        private void BindDropSearch()
        {

            //ddlCatName.DataSource = CategoryBLL.GetAllCategories();
            //ddlCatName.Items.Insert(0, new ListItem("Select Category", ""));
            //ddlCatName.DataTextField = "categoryName";
            //ddlCatName.DataValueField = "categoryId";

            //ddlCatName.DataBind();
        }
        protected void CategoryDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {

            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (Session["UserRole"].ToString().Equals("Store"))
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                        btnEdit.Visible = false;
                        btnDelete.Visible = false;
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                 

                }

                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    try
                    {
                        DropDownList depList = (DropDownList)e.Row.FindControl("ddlDepName");
                        depList.DataSource = DepartmentBLL.GetAllDepartment();
                        depList.DataBind();
                        depList.DataTextField = "Name";
                        depList.DataValueField = "DepId";
                        depList.DataBind();
                        depList.Items.FindByValue((e.Row.FindControl("lblDep_Id") as Label).Text).Selected = true;

                        DataRowView dr = e.Row.DataItem as DataRowView;
                        depList.SelectedValue = (string)e.Row.DataItem; // you can use e.Row.DataItem to get the value
                    }
                    catch (Exception ex)
                    {
                      
                        throw ex;
                    }
                    finally 
                    {
                    }
                }
            }


        }

        protected void CategoryDisplayGrid_Sorting(object sender, GridViewSortEventArgs e)
        {
            try
            {
                DataView sortedView;
                string sortingDirection = string.Empty;
                if (direction == SortDirection.Ascending)
                {
                    direction = SortDirection.Descending;
                    sortingDirection = "Desc";

                }
                else
                {
                    direction = SortDirection.Ascending;
                    sortingDirection = "Asc";

                }


                ds = CategoryBLL.GetAllCategories();
                CategoryDisplayGrid.DataSource = ds;
                sortedView = new DataView(ds.Tables[0]);
                sortedView.Sort = e.SortExpression + " " + sortingDirection;
                Session["SortedView"] = sortedView;
                CategoryDisplayGrid.DataSource = sortedView;
                CategoryDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
            finally 
            {
                
            }
        }

        public SortDirection direction
        {
            get
            {
                if (ViewState["directionState"] == null)
                {
                    ViewState["directionState"] = SortDirection.Ascending;
                }
                return (SortDirection)ViewState["directionState"];
            }
            set
            {
                ViewState["directionState"] = value;
            }
        }

        protected void btnAddCategory_Click(object sender, EventArgs e)
        {
            Session["CatId"] = "0";
            Response.Redirect("AddEditCategory.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }
    }
}