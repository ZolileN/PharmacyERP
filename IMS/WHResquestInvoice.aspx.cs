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
using System.IO;
using System.Text;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;
using System.Net.Mime;
using System.Net;
using IMSCommon;
using log4net;
using IMS.Util;


namespace IMS
{
    public partial class WHResquestInvoice : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
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
                    if (Request.QueryString["Id"] != null)
                    {
                        //Session["FirstOrderSO"] = false;
                        int req_ID = int.Parse(Request.QueryString["Id"].ToString());
                        LoadData(req_ID.ToString());
                        #region RequestTo&FROM Population
                        if (Session["WH_SoID"] != null)
                        {
                            SaleOrder.Text = Session["WH_SoID"].ToString();
                        }
                        DataSet dsTo = GetSystems(Convert.ToInt32(Session["WH_RequestedFromID"].ToString()));
                        DataSet dsFROM = GetSystems(Convert.ToInt32(Session["UserSys"].ToString()));
                        SendDate.Text = System.DateTime.Now.ToShortDateString();
                        //From.Text = dsFROM.Tables[0].Rows[0]["SystemName"].ToString();
                        // FromAddress.Text = dsFROM.Tables[0].Rows[0]["SystemAddress"].ToString();
                        To.Text = dsTo.Tables[0].Rows[0]["SystemName"].ToString();
                        ToAddress.Text = dsTo.Tables[0].Rows[0]["SystemAddress"].ToString();
                        #endregion
                    }

                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
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
        public DataSet GetSystems(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", ID);



                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;

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
            return ds;
        }
        public void LoadData(String OrderID)
        {
            #region Display Requests
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetGenTODetails_OrdID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", OrderID);

                DataSet ds = new DataSet();

                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();
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
            #endregion
        }
        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                // Retrieve the underlying data item. In this example
                // the underlying data item is a DataRowView object.
                DataRowView rowView = (DataRowView)e.Row.DataItem;

                // e.Row.Cells["OrderDetailID"].Text
                // Retrieve the key value for the current row. Here it is an int.
                //   int myDataKey = rowView["OrderDetailID"];

                Label OrderDetailID = (Label)e.Row.FindControl("OrderDetailID");
                GridView Details = (GridView)e.Row.FindControl("StockDetailDisplayGrid");

                #region Display Requests
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                   
                    SqlCommand command = new SqlCommand("sp_getTransferDetailsReceiveEntry_TransferDetailID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_TransferDetailID", Convert.ToInt32(OrderDetailID.Text));

                    DataSet ds = new DataSet();

                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);
                    ProductSet = ds;
                    Details.DataSource = null;
                    Details.DataSource = ds.Tables[0];
                    Details.DataBind();
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
                    {
                        connection.Close();
                    }
                }
            }
                #endregion
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("WH_RequestedFromID");
           
            Response.Redirect("RespondStoreRequest.aspx",false);
            //Response.Redirect("ManageOrders.aspx", false);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
                /*connection.Open();
                SqlCommand command = new SqlCommand("sp_GetSaleOrderDetailList", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["WH_TransferNO"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);

                MyExcel.FILE_PATH = Server.MapPath(@"~\SaleOrderFormat\").ToString();
                string convertedFilePath = MyExcel.WriteExcelWithSalesOrderInfo(SaleOrder.Text, SendDate.Text, (Environment.NewLine + To.Text + Environment.NewLine + ToAddress.Text), ds, Server.MapPath(@"~\SaleOrderFormat\"));

                Response.AppendHeader("content-disposition", "attachment; filename=" + convertedFilePath);
                Response.ContentType = "Application/msexcel";
                Response.WriteFile(convertedFilePath);
                Response.End();*/

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_getTransferDetailsReceiveEntry_TransferDetailID", connection);
                command.CommandType = CommandType.StoredProcedure;
                if (Request.QueryString["Id"] != null)
                {
                    int req_ID = int.Parse(Request.QueryString["Id"].ToString());
                    command.Parameters.AddWithValue("@p_OrderID", req_ID);
                    DataSet ds = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(ds);

                    MyExcel.FILE_PATH = Server.MapPath(@"~\SaleOrderFormat\").ToString();

                    // Excel.Workbook myWorkBook;
                    String fileName = "";
                    fileName = MyExcel.WriteExcelWithSalesOrderInfo(SaleOrder.Text, SendDate.Text, (Environment.NewLine + To.Text + Environment.NewLine + ToAddress.Text), ds, Server.MapPath(@"~\SaleOrderFormat\"));
                    string[] files = fileName.Split(';');
                    Response.Clear();
                    Response.Buffer = true;
                    Response.AppendHeader("content-disposition", "attachment; filename=" + files[1]);
                    Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";

                    Response.Charset = "UTF-8";

                    String Path1 = Path.Combine(files[0], files[1]);
                    string url = @"~/SaleOrderFormat/" + files[1];


                    Response.Redirect(url);
                    Response.End();
                }
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

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            Session["OrderNo_Invoice"] = Session["WH_TransferNO"].ToString();
            Response.Redirect("SalesOrderInvoice.aspx",false);
        }

    }
}