using AjaxControlToolkit;
using IMS_WHReports.Util;
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
               // command.ExecuteNonQuery();

                DataSet dsResults = new DataSet();
                SqlDataAdapter da = new SqlDataAdapter(command);
                da.Fill(dsResults);
                if (dsResults.Tables[0].Rows.Count > 0 && dsResults.Tables[1].Rows.Count > 0)
                {
                    gdvPendingSOs.DataSource = dsResults.Tables[0];
                    gdvPendingSOs.DataBind();
                    //Session["OrderNumberSO"] = dsResults.Tables[0].Rows[0]["OrderID"].ToString(); ---- This needs to be set in the Checked Selection.
                    //Session["dsProdcts"] = dsResults.Tables[1];
                    Session["dsSalesOrders"] = dsResults.Tables[1];
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
            try
            {
                int OrderID = int.Parse(((Label)gdvPendingSOs.Rows[e.RowIndex].FindControl("lblOrderID")).Text.ToString());
                if(connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_DeleteNonGeneratedSaleOrder", connection);
                command.Parameters.AddWithValue("@p_OrderID", OrderID);
                command.CommandType = CommandType.StoredProcedure;
                command.ExecuteNonQuery();
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
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
                        Session["RequestedFromID"] = ((Label)row.FindControl("lblOrderTo")).Text;

                        int SelectedOrdID = Convert.ToInt32(((Label)row.FindControl("lblOrderID")).Text.ToString());

                        DataTable dt = (DataTable) Session["dsSalesOrders"];

                        if (SelectedOrdID != 0)
                        {
                            DataView dv = dt.DefaultView;
                            dv.RowFilter = "OrderID = " + SelectedOrdID;

                            dt = dv.ToTable();
                        }

                        Session["dsSalesOrders"] = dt;
                        DataSet RsltTable = new DataSet();
                        RsltTable.Tables.Add(dt);
                        Session["dsProdcts"] = RsltTable;

                        Session["OrderNumberSO"] = SelectedOrdID;
                        Session["FirstOrderSO"] = true;
                        Control ctl = this.Parent;
                        TextBox txtDiscountPercentage = null;
                        Button btnGenerateSO;
                        GridView gvStockDisplayGrid = (GridView)ctl.FindControl("StockDisplayGrid");

                        DropDownList ddlStore = (DropDownList)ctl.FindControl("StockAt");
                        DropDownList ddlSalesMan = (DropDownList)ctl.FindControl("ddlSalesman");

                        txtDiscountPercentage = (TextBox)ctl.FindControl("SelectDiscount");
                        btnGenerateSO = (Button)ctl.FindControl("btnAccept");
                         
                        if (txtDiscountPercentage != null)
                        {
                            //txtDiscountPercentage.Text = row.Cells[10].Text;
                            Label lblDicount = (Label)row.FindControl("lblDiscount");

                            //txtDiscountPercentage.Text = lblDicount.Text;
                            if(btnGenerateSO != null)
                            {
                                btnGenerateSO.Visible = true;
                            }
                        }

                        if(ddlStore!=null)
                        {
                            foreach (ListItem Items in ddlStore.Items)
                            {
                                if (Items.Text.Equals(dt.Rows[0]["ToPlace"].ToString()))
                                {
                                    ddlStore.SelectedIndex = ddlStore.Items.IndexOf(Items);
                                    Session["RequestedFromID"] = dt.Rows[0]["ToPlace"].ToString();

                                    break;
                                }
                            }
                           // ddlStore.Enabled = false;
                        }

                        if(ddlSalesMan!=null)
                        {
                            foreach (ListItem Items in ddlSalesMan.Items)
                            {
                                if (Items.Text.Equals(dt.Rows[0]["SalesMan"].ToString()))
                                {
                                    ddlSalesMan.SelectedIndex = ddlSalesMan.Items.IndexOf(Items);
                                    Session["RequestedSalesMan"] = dt.Rows[0]["SalesMan"].ToString();

                                    break;
                                }
                            }
                            //ddlSalesMan.SelectedIndex = Convert.ToInt32(dt.Rows[0]["SalesManID"].ToString());
                           // ddlSalesMan.Enabled = false;
                        }
                        if (gvStockDisplayGrid != null)
                        {
                            DataSet ds = new DataSet();
                            DataTable dtSaleOrders = (DataTable)Session["dsSalesOrders"];
                            //ds.Tables.Add(dtSaleOrders);
                            if (dtSaleOrders != null && dtSaleOrders.Rows.Count > 0)
                            {
                                gvStockDisplayGrid.DataSource = dtSaleOrders;
                                gvStockDisplayGrid.DataBind();
                            }
                        }

                    }
                }
                
            }

            Session.Remove("dsSalesOrders");
        }
    }
}