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
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {

                    PopulateddDepartment();

                    PopulateddCategory();
                    if (Convert.ToInt32(Session["subcatid"].ToString()) > 0)
                    {
                        
                        txtSubCategoryName.Text = Session["subcatname"].ToString();
                        
                        //foreach (ListItem Items in ddCategory.Items)
                        //{
                        //    if (Items.Text.Contains(Session["catname"].ToString()))
                        //    {
                        //        ddCategory.SelectedIndex = ddCategory.Items.IndexOf(Items);
                        //        break;
                        //    }
                        //}

                        btnSaveSubCategory.Text = "Update";
                    }
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }
        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            Server.ClearError();
        }
        private void PopulateddDepartment()
        {
            #region Populating Department DropDown
            try
            {
               
                DataSet ds = new DataSet();
                ds = DepartmentBLL.GetAllDepartment();                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                               
                ddDepartment.DataSource = ds.Tables[0];
                
               

                ddDepartment.DataTextField = "Name";
                ddDepartment.DataValueField = "DepId";
                ddDepartment.DataBind();
                if (ddDepartment != null)
                {
                    ddDepartment.Items.Insert(0, "Select Department");

                    if (Session["DepartmentName"] != null)
                    {
                        foreach (ListItem Items in ddDepartment.Items)
                        {
                            if (Items.Text.Contains(Session["DepartmentName"].ToString()))
                            {
                                ddDepartment.SelectedIndex = ddDepartment.Items.IndexOf(Items);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ddDepartment.SelectedIndex = 0;
                    }
                  
                    
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            #endregion
        }
    
        private void PopulateddCategory()
        {
            #region Populating Category DropDown
            try
            {
                SubCategoryBLL objSubCatBAL = new SubCategoryBLL();
                DataSet ds = new DataSet();
                 
                if (ddDepartment.SelectedIndex > 0)
                {
                    ds = objSubCatBAL.GetCategoriesDropDown(Convert.ToInt32(ddDepartment.SelectedValue.ToString()));
                }
                else 
                {
                    ds = objSubCatBAL.GetCategoriesDropDown(null);
                }
                 
                ddCategory.DataSource = ds.Tables[0];
                ddCategory.DataTextField = "categoryName";
                ddCategory.DataValueField = "categoryID";
                ddCategory.DataBind();
                if (ddCategory != null)
                {
                    ddCategory.Items.Insert(0, "Select Category");

                    if (Session["catname"] != null)
                    {
                        foreach (ListItem Items in ddCategory.Items)
                        {
                            if (Items.Text.Contains(Session["catname"].ToString()))
                            {
                                ddCategory.SelectedIndex = ddCategory.Items.IndexOf(Items);
                                break;
                            }
                        }
                    }
                    else
                    {
                        ddCategory.SelectedIndex = 0;
                    }
                     
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
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

                    subCategoryManager.UpdateSubCat(subCategoryToUpdate);
                    ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('SubCategory SuccessFully Updated.')", true);

                }
                else
                {
                    SubCategory subCategoryToAdd = new SubCategory();
                    subCategoryToAdd.Name = txtSubCategoryName.Text;
                    subCategoryToAdd.CategoryID = Convert.ToInt32(ddCategory.SelectedValue);

                    subCategoryManager.AddNew(subCategoryToAdd);
                }
                Response.Redirect("ManageSubCategory.aspx", false);

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        protected void ddDepartment_SelectedIndexChanged(object sender, EventArgs e)
        {
            PopulateddCategory();
        }

    }
}