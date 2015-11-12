using AjaxControlToolkit;
using IMS_WHReports.Util;
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

namespace IMS_WHReports.UserControl
{
    public partial class rpt_ucSalesProduct : System.Web.UI.UserControl
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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

        public void LoadData()
        {
            try
            {

                if (connection.State == ConnectionState.Closed) { connection.Open(); }

                SqlCommand command = new SqlCommand();
                if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("YES"))
                {
                    command = new SqlCommand("sp_rptPI_Products", connection);
                }
                else if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("Expiry"))
                {
                    command = new SqlCommand("sp_rpt_StockDetails", connection);
                }
                else if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("POS"))
                {
                    command = new SqlCommand("sp_GetPOSSALES_PopUpDetails", connection);
                }
                else
                {
                    command = new SqlCommand("sp_rptSalesProducts", connection);
                }

                command.CommandType = CommandType.StoredProcedure;
                if (Session["SP_Purchase"] != null && Session["SP_Purchase"].ToString().Equals("Expiry"))
                {
                }
                else
                {
                    if (Session["SearchItemProduct_RPT"] != null && Session["SearchItemProduct_RPT"].ToString() != "")
                    {
                        command.Parameters.AddWithValue("@p_Search", Session["SearchItemProduct_RPT"].ToString());
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_Search", DBNull.Value);
                    }
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet dsCustomers = new DataSet();
                dA.Fill(dsCustomers);

                gdvDepartment.DataSource = dsCustomers.Tables[0];
                gdvDepartment.DataBind();

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnSelectDepartment_Click(object sender, EventArgs e)
        {
            GridViewRow rows = gdvDepartment.SelectedRow;
            foreach (GridViewRow row in gdvDepartment.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label CustomerName = (Label)row.Cells[0].FindControl("lblProductName");
                        Label CustomerID = (Label)row.Cells[0].FindControl("lblProductID");

                        if (CustomerID.Text.ToString() != "" && CustomerName.Text.ToString() != "")
                        {
                            TextBox mpe = (TextBox)this.Parent.FindControl("txtProduct");
                            mpe.Text = Server.HtmlDecode(CustomerName.Text);
                            Session["rptProductID"] = CustomerID.Text.ToString();
                            break;
                        }
                    }
                    else
                    {
                        TextBox mpe = (TextBox)this.Parent.FindControl("txtProduct");
                        mpe.Text = mpe.Text;
                    }
                }
            }
        }

        protected void gdvDepartment_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvDepartment_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvDepartment.PageIndex = e.NewPageIndex;
            LoadData();
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeProductDiv");
            mpe.Show();
        }

        protected void gdvDepartment_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvDepartment_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvDepartment_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }
    }
}