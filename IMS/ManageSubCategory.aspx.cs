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
    public partial class ManageSubCategory : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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
                        btnAddSubCategory.Enabled = false;
                    }
                    BindGrid(false);
                    BindDropSearch();
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
                SubCategoryDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        protected void SubCategoryDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            try
            {
                SubCategoryDisplayGrid.EditIndex = e.NewEditIndex;

                GridViewRow row = SubCategoryDisplayGrid.Rows[SubCategoryDisplayGrid.EditIndex];
                Label subcatid = (Label)row.FindControl("lblSubCat_ID");
                Label subcatname = (Label)row.FindControl("lblSubCat_Name");
                Label catname = (Label)row.FindControl("lblCat_Id");
                Label lblDepartmentName = (Label)row.FindControl("lblDep_Id");

                Session["DepartmentName"] = lblDepartmentName.Text;
                Session["subcatname"] = subcatname.Text;
                Session["catname"] = catname.Text;
                Session["subcatid"] = subcatid.Text;
                Response.Redirect("AddEditSubCategory.aspx");
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
            

            //BindGrid(false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            BindGrid(true);
        }
        private void BindGrid(bool isSearch)
        {
            try
            {
                ds = SubCategoryBLL.GetAllSubCategories(connection);

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
                depList.DataSource = ins.GetDepListByCategoryName(obj2, connection);
                depList.DataBind();
                depList.DataTextField = "Name";
                depList.DataValueField = "DepId";
                depList.DataBind();
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

        protected void SubCategoryDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                try
                {
                    if (Session["UserRole"].ToString().Equals("Store"))
                    {
                        LinkButton btnEdit = (LinkButton)e.Row.FindControl("btnEdit");
                        LinkButton btnDelete = (LinkButton)e.Row.FindControl("btnDelete");
                        if (btnEdit != null)
                        {
                            btnEdit.Visible = false;
                        }
                        if (btnDelete != null)
                        {
                            btnDelete.Visible = false;
                        }
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                }
            }

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
            try
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

        protected void ddlCategoryName_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
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

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }

        protected void btnAddSubCategory_Click(object sender, EventArgs e)
        {
            Session["subcatid"] = "0";
            Response.Redirect("AddEditSubCategory.aspx",false);
        }

    }
}