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
    public partial class ManageDepartment : System.Web.UI.Page
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
                        btnAddDepartment.Enabled = false;
                    }
                    BindGrid(false);
                    //BindDropSearch();
                }
                catch (Exception ex) 
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
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
        protected void DepDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            DepDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid(false);
        }

        protected void DepDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            DepDisplayGrid.EditIndex = -1;
            BindGrid(false);
        }

        protected void DepDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                DepartmentBLL depManager = new DepartmentBLL();
                Label ID = (Label)DepDisplayGrid.Rows[e.RowIndex].FindControl("lblDep_ID");
                int selectedId = int.Parse(ID.Text);
                Department depToDelete = new Department();//= empid.Text;
                depToDelete.DepartmentID = selectedId;
                depManager.Delete(depToDelete,connection);


            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                DepDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }
        }

        //protected void DepDisplayGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        //{

        //}

        protected void DepDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                //if (e.CommandName.Equals("Add"))
                //{
                //    DepartmentBLL depManager = new DepartmentBLL();
                //    TextBox txtname = (TextBox)DepDisplayGrid.FooterRow.FindControl("txtAddname");
                //    TextBox txtCode = (TextBox)DepDisplayGrid.FooterRow.FindControl("txtAddCode");

                //    Department depToAdd = new Department();
                //    depToAdd.Name = txtname.Text;
                //    depToAdd.Code = txtCode.Text;

                //    depManager.Add(depToAdd,connection);


                //}
                //else if (e.CommandName.Equals("UpdateDep"))
                //{

                //    DepartmentBLL depManager = new DepartmentBLL();
                //    GridViewRow row = DepDisplayGrid.Rows[DepDisplayGrid.EditIndex];
                //    Label id = (Label)row.FindControl("lblDep_Id");
                //    //TextBox name = (TextBox)DepDisplayGrid.Rows[DepDisplayGrid.EditIndex].Cells[0].FindControl("txtname");
                //    //TextBox code = (TextBox)DepDisplayGrid.Rows[DepDisplayGrid.EditIndex].FindControl("txtCode");

                //    Session["dname"] = DepDisplayGrid.Rows[DepDisplayGrid.EditIndex].Cells[0].FindControl("txtname");
                //    Session["dcode"] = DepDisplayGrid.Rows[DepDisplayGrid.EditIndex].Cells[0].FindControl("txtCode");
                //    Session["dId"] = id.Text;
                //    Response.Redirect("AddDepartment.aspx");
                //   // int selectedId = int.Parse(id.Text);
                //    //Department depToUpdate = new Department();//= empid.Text;
                //    //depToUpdate.DepartmentID = selectedId;
                //    //depToUpdate.Name = name.Text;
                //    //depToUpdate.Code = code.Text;
                //    //depManager.Update(depToUpdate,connection);
                //}
            }
            catch (Exception exp) { }
            finally
            {
                DepDisplayGrid.EditIndex = -1;
                BindGrid(false);
            }

        }

        protected void DepDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            DepDisplayGrid.EditIndex = e.NewEditIndex;
             
            GridViewRow row = DepDisplayGrid.Rows[DepDisplayGrid.EditIndex];
            Label id = (Label)row.FindControl("lblDep_Id");
            Label name = (Label)row.FindControl("lblDep_Name");
            Label code = (Label)row.FindControl("lblDep_Code");

            Session["dname"] = name.Text;
            Session["dcode"] = code.Text;
            Session["dId"] = id.Text;
            Response.Redirect("AddDepartment.aspx",false);

            // BindGrid();
        }
        private void BindGrid(bool isSearch)
        {
            try
            {
                ds = DepartmentBLL.GetAllDepartment(connection);
                DepDisplayGrid.DataSource = ds;
                DepDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        protected void btnAddDepartment_Click(object sender, EventArgs e)
        {
            Session["dId"] = "0";
            Response.Redirect("AddDepartment.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }

        protected void DepDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
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
            }
        }
    }
}