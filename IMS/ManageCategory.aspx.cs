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
    public partial class ManageCategory : System.Web.UI.Page
    {
        private DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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

                        categoryManager.Add(categoryToAdd,connection);
                        
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
                        categoryManager.Update(categoryToUpdate, connection);
                    }
                    else
                    {
                        //  WebMessageBoxUtil.Show("Invalid input in Department field ");
                    }
                }
            }
            catch (Exception exp) { }
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
                categoryManager.Delete(categoryToDelete, connection);

            }
            catch (Exception exp) { }
            finally
            {
                CategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void CategoryDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            CategoryDisplayGrid.EditIndex = e.NewEditIndex;
            BindGrid(false);
        }

        private void BindGrid(bool isSearch)
        {
            //if (ddlCatName.SelectedIndex != -1 && isSearch)
            //{
            //    Category obj = new Category();
            //    obj.CategoryID = int.Parse(ddlCatName.SelectedValue);
            //    CategoryBLL ins = new CategoryBLL();
            //    ds = ins.GetById(obj);
            //}
            //else
            {
                ds = CategoryBLL.GetAllCategories(connection);
            }
            CategoryDisplayGrid.DataSource = ds;
            CategoryDisplayGrid.DataBind();

            DropDownList depList = (DropDownList)CategoryDisplayGrid.FooterRow.FindControl("ddlAddDepName");
            depList.DataSource = DepartmentBLL.GetAllDepartment(connection);
            depList.DataBind();
            depList.DataTextField = "Name";
            depList.DataValueField = "DepId";
            depList.DataBind();
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
                if ((e.Row.RowState & DataControlRowState.Edit) > 0)
                {
                    try
                    {
                        DropDownList depList = (DropDownList)e.Row.FindControl("ddlDepName");
                        depList.DataSource = DepartmentBLL.GetAllDepartment(connection);
                        depList.DataBind();
                        depList.DataTextField = "Name";
                        depList.DataValueField = "DepId";
                        depList.DataBind();
                        depList.Items.FindByValue((e.Row.FindControl("lblDep_Id") as Label).Text).Selected = true;

                        DataRowView dr = e.Row.DataItem as DataRowView;
                        depList.SelectedValue = (string)e.Row.DataItem; // you can use e.Row.DataItem to get the value
                    }
                    catch (Exception exo)
                    { }
                }
            }


        }

        protected void CategoryDisplayGrid_Sorting(object sender, GridViewSortEventArgs e)
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


            ds = CategoryBLL.GetAllCategories(connection);
            CategoryDisplayGrid.DataSource = ds;
            sortedView = new DataView(ds.Tables[0]);
            sortedView.Sort = e.SortExpression + " " + sortingDirection;
            Session["SortedView"] = sortedView;
            CategoryDisplayGrid.DataSource = sortedView;
            CategoryDisplayGrid.DataBind();
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
    }
}