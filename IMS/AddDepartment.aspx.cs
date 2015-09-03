
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
    public partial class AddDepartment : System.Web.UI.Page
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
                    if (Convert.ToInt32(Session["dId"].ToString()) > 0)
                    {
                        DepartmentName.Text = Session["dname"].ToString();
                        DepartmentCode.Text = Session["dcode"].ToString();
                        btnSaveDepartment.Text = "Update";
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
        protected void btnSaveDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                DepartmentBLL depManager = new DepartmentBLL();

                if (Convert.ToInt32(Session["dId"].ToString()) > 0)
                {
                    int selectedId = int.Parse(Session["dId"].ToString());
                    Department depToUpdate = new Department();
                    depToUpdate.DepartmentID = selectedId;
                    depToUpdate.Name = DepartmentName.Text;
                    depToUpdate.Code = DepartmentCode.Text;
                    depManager.Update(depToUpdate);
                }
                else
                {
                    Department depToAdd = new Department();
                    depToAdd.Name = DepartmentName.Text;
                    depToAdd.Code = DepartmentCode.Text;

                    depManager.Add(depToAdd);
                }
                Session.Remove("dname");
                Session.Remove("dcode");
                Session.Remove("dId");

                Response.Redirect("ManageDepartment.aspx", false);
            }
            catch (Exception exp)
            {
                
                throw exp;
            }
            finally 
            {
               
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {
            DepartmentName.Text = "";
            DepartmentCode.Text = "";
            Response.Redirect("ManageDepartment.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageDepartment.aspx",false);
        }
    }
}