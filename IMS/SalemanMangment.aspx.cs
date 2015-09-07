using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using IMSCommon.Util;
using log4net;
using IMS.Util;
using IMSBusinessLogic;

namespace IMS
{
    public partial class SalemanMangment : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet UserSet;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        private UserBLL userBll = new UserBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            
            try
            {
                if (!IsPostBack)
                {
                    BindGrid();
                }
            }
            catch (Exception ex)
            {
                
                throw ex;
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
      
        
        protected void SalemanDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            string ID= "";
            SalemanDisplayGrid.EditIndex = e.NewEditIndex;
            Label UserID = (Label)SalemanDisplayGrid.Rows[e.NewEditIndex].FindControl("lblUserID");
            Session["ur_RoleName"] = "Salesman";
            ID = UserID.Text;

            Response.Redirect("RegisterUsers.aspx?ID=" + ID);
        }
        private void BindGrid()
        {
            DataTable dt = new DataTable();
            DataSet ds = new DataSet();
            #region Getting User Details
            try
            {
                UserSet = null;
                ds = userBll.SelectUser("Salesman");
                UserSet = ds;
                SalemanDisplayGrid.DataSource = ds;
                SalemanDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
            #endregion
        }
         

        protected void SalemanDisplayGrid_RowUpdating(object sender, GridViewUpdateEventArgs e)
        {

        }

        protected void SalemanDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                GridViewRow row = (GridViewRow)SalemanDisplayGrid.Rows[e.RowIndex];
                Label label = (Label)SalemanDisplayGrid.Rows[e.RowIndex].FindControl("lblUserID");

                SalemanDisplayGrid.EditIndex = -1;
                userBll.Delete(long.Parse(label.Text));
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                BindGrid();

            }

        }

        protected void SalemanDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            SalemanDisplayGrid.EditIndex = -1;
            BindGrid();
        }

        protected void SalemanDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SalemanDisplayGrid.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnAddSalesman_Click(object sender, EventArgs e)
        {
            Session["ur_RoleName"] = "Salesman";
            Response.Redirect("RegisterUsers.aspx",false);
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx",false);
        }


    }
}