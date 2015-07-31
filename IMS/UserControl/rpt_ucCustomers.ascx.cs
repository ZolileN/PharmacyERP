using AjaxControlToolkit;
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

namespace IMS.UserControl
{
    public partial class rpt_ucCustomers : System.Web.UI.UserControl
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if(!IsPostBack)
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
                if (Session["SP_Purchase"]!=null && Session["SP_Purchase"].ToString().Equals("YES"))
                {
                    command = new SqlCommand("sp_rptPI_Customers", connection);
                }
                else
                {
                    command = new SqlCommand("sp_rptSalesCustomers", connection);
                }
                command.CommandType = CommandType.StoredProcedure;
                if (Session["SearchItem_RPT"] != null && Session["SearchItem_RPT"].ToString() != "")
                {
                    command.Parameters.AddWithValue("@p_Search", Session["SearchItem_RPT"].ToString());
                }
                else
                {
                    command.Parameters.AddWithValue("@p_Search", DBNull.Value);
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet dsCustomers = new DataSet();
                dA.Fill(dsCustomers);

                gdvCustomers.DataSource = dsCustomers.Tables[0];
                gdvCustomers.DataBind();

            }
            catch(Exception ex)
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
        protected void gdvCustomers_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvCustomers_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvCustomers.PageIndex = e.NewPageIndex;
            LoadData();
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCustomersDiv");
            mpe.Show();
        }

        protected void gdvCustomers_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvCustomers_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvCustomers_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void btnSelectCustomer_Click(object sender, EventArgs e)
        {
            GridViewRow rows = gdvCustomers.SelectedRow;
            foreach (GridViewRow row in gdvCustomers.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Label CustomerName = (Label)row.Cells[0].FindControl("lblCustomer");
                        Label CustomerID = (Label)row.Cells[0].FindControl("lblSysID");

                        if(CustomerID.Text.ToString()!="" && CustomerName.Text.ToString()!="")
                        {
                            TextBox mpe = (TextBox)this.Parent.FindControl("txtCustomers");
                            mpe.Text = Server.HtmlDecode(CustomerName.Text);
                            Session["rptCustomerID"] = CustomerID.Text.ToString();
                            break;
                        }
                    }
                    else
                    {
                        TextBox mpe = (TextBox)this.Parent.FindControl("txtCustomers");
                        mpe.Text = mpe.Text;
                    }
                }
              }


        }
    }
}