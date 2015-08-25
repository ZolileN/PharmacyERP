using AjaxControlToolkit;
using IMS.Util;
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
    public partial class uc_PendingSalesOrderPopUp : System.Web.UI.UserControl
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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
                    BindGrid();
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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

        public void BindGrid()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetUserPendingSaleOrders", connection);
                command.Parameters.AddWithValue("@p_LoggedinnUserId", int.Parse(Session["UserSys"].ToString()));
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();

                DataSet dsResults = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dsResults);
                if (dsResults.Tables[0].Rows.Count > 0)
                {
                    gdvPendingSOs.DataSource = dsResults;
                    gdvPendingSOs.DataBind();
                    Session["OrderNumberSO"] = dsResults.Tables[0].Rows[0]["OrderID"].ToString();
                    Session["dsProdcts"] = dsResults;
                    Session["dsSalesOrders"] = dsResults;
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        protected void gdvPendingSOs_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvPendingSOs_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvPendingSOs.PageIndex = e.NewPageIndex;
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeNonGeneratedSOsPopup");
            mpe.Show();
        }

        protected void gdvPendingSOs_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void gdvPendingSOs_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void gdvPendingSOs_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void SelectSalesOrder_Click(object sender, EventArgs e)
        {
            foreach (GridViewRow row in gdvPendingSOs.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Session["RequestedFromID"] = ((Label)row.FindControl("lblOrderRequestedFor")).Text;
                        Control ctl = this.Parent;
                        TextBox txtDiscountPercentage = null;
                        Button btnGenerateSO;
                        GridView gvStockDisplayGrid = (GridView)ctl.FindControl("StockDisplayGrid");

                        txtDiscountPercentage = (TextBox)ctl.FindControl("SelectDiscount");
                        btnGenerateSO = (Button)ctl.FindControl("btnAccept");
                         
                        if (txtDiscountPercentage != null)
                        {
                            //txtDiscountPercentage.Text = row.Cells[10].Text;
                            Label lblDicount = (Label)row.FindControl("lblDiscount");

                            txtDiscountPercentage.Text = lblDicount.Text;
                            if(btnGenerateSO != null)
                            {
                                btnGenerateSO.Visible = true;
                            }
                        }
                        if (gvStockDisplayGrid != null)
                        {
                            DataSet ds = (DataSet)Session["dsSalesOrders"];
                            if (ds.Tables[0].Rows.Count > 0)
                            {
                                gvStockDisplayGrid.DataSource = ds;
                                gvStockDisplayGrid.DataBind();
                            }
                        }

                    }
                }
                Session.Remove("dsSalesOrders");
            }
        }
    }
}