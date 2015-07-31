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

namespace IMS
{
    public partial class ReceiveStoreRequest : System.Web.UI.Page
    {
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
                #region Populating Request From DropDown
                ddlReqFrom.Items.Add("Salesman Requests");
                ddlReqFrom.Items.Add("Store Requests");
                ddlReqFrom.Items.Add("Both");
                if (ddlReqFrom != null)
                {
                    ddlReqFrom.Items.Insert(0, "Select Request From");
                    ddlReqFrom.SelectedIndex = 0;
                }

                #endregion

                #region Populating Request Status DropDown
                ddlReqStatus.Items.Add("Complete");
                ddlReqStatus.Items.Add("Incomplete");
                ddlReqStatus.Items.Add("Both");
                if (ddlReqStatus != null)
                {
                    ddlReqStatus.Items.Insert(0, "Select Request Status");
                    ddlReqStatus.SelectedIndex = 0;
                }

                #endregion
                bindGrid();
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
        protected void btnSelectStore_Click(object sender, EventArgs e)
        {
            String Text = txtStore.Text + '%';
            Session["txtStore"] = Text;
            StoresPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
        }

        protected void btnSelectSaleman_Click(object sender, EventArgs e)
        {
            String Text = txtSlman.Text + '%';
            Session["txtSalesman"]=Text;
            salesPopupGrid.populateGrid();
            mpecMessageDic.Show();
        }

        

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            bindGrid();
        }

        protected void bindGrid()
        {
            #region Display Requests
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetTransferRequests", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_RequestedForID", Session["UserSys"]);
                if (ddlReqFrom.SelectedIndex > 0)
                {
                    command.Parameters.AddWithValue("@p_requestFrom", ddlReqFrom.SelectedValue.ToString());

                }
                else
                {
                    command.Parameters.AddWithValue("@p_requestFrom", DBNull.Value);

                }
                if (ddlReqStatus.SelectedIndex > 0)
                {
                    command.Parameters.AddWithValue("@p_OrderStatus",ddlReqStatus.SelectedValue.ToString());

                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderStatus", DBNull.Value);

                }
                if (!String.IsNullOrEmpty(lblStoreId.Text)) 
                {
                    int storeID=0;

                    if (int.TryParse(lblStoreId.Text, out storeID))
                    {
                        command.Parameters.AddWithValue("@p_storeID", storeID);
                    }
                    else 
                    {
                        command.Parameters.AddWithValue("@p_storeID", DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@p_storeID", DBNull.Value);
                }
                
                if (!String.IsNullOrEmpty(lblSlmanID.Text))
                {
                    int slmanID = 0;

                    if (int.TryParse(lblSlmanID.Text, out slmanID))
                    {
                        command.Parameters.AddWithValue("@p_SalesManID", slmanID);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_SalesManID", DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@p_SalesManID", DBNull.Value);
                }

                if (!String.IsNullOrEmpty(txtOrderNO.Text))
                {
                    int ordNO = 0;

                    if (int.TryParse(txtOrderNO.Text, out ordNO))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", ordNO);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                    }
                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderID", DBNull.Value);
                }

                if (String.IsNullOrWhiteSpace(DateTextBox.Text))
                {
                    command.Parameters.AddWithValue("@p_OrderDate", DBNull.Value);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_OrderDate", Convert.ToDateTime(DateTextBox.Text));
                }
                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally
            {
                connection.Close();
            }
            #endregion
        }
        protected void btnRefresh_Click(object sender, EventArgs e)
        {
            ddlReqFrom.SelectedIndex = 0;
            ddlReqStatus.SelectedIndex = 0;
            txtSlman.Text = string.Empty;
            txtStore.Text = string.Empty;
            txtOrderNO.Text = string.Empty;
            bindGrid();
        }

        
       
        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx",false);
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            bindGrid();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            bindGrid();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("View"))
                {
                    int RowNumber = 0;
                    int Pageindex = Convert.ToInt32(StockDisplayGrid.PageIndex);

                    Label RequestNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedNO");
                    Label RequestFrom = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedFrom");
                    //Label RequestDate = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedDate");
                    //Label RequesteeRole = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblSysRole");
                    //Label RequesteeID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedFromID");
                    Session["WH_RequestedNO"] = RequestNo.Text.ToString();
                    Session["WH_RequestedFrom"] = RequestFrom.Text.ToString();
                    //Session["RequestedDate"] = RequestDate.Text.ToString();
                    //Session["RequestDesRole"] = RequesteeRole.Text.ToString();
                  //  Session["WH_RequestDesID"] = RequesteeID.Text.ToString();
                    Response.Redirect("RespondStoreRequest.aspx",false);
                }
            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            finally
            {

            }
        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            bindGrid();
        }
    }
}