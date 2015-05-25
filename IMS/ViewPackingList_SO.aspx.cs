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

namespace IMS
{
    public partial class ViewPackingList_SO : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; //This needs to be removed as not used in the entire page
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData(Session["RequestedNO"].ToString());
                #region RequestTo&FROM Population
                SaleOrder.Text = Session["RequestedNO"].ToString();
                DataSet dsTo = GetSystems(Convert.ToInt32(Session["RequestedFromID"].ToString()));
                DataSet dsFROM = GetSystems(Convert.ToInt32(Session["UserSys"].ToString()));
                SendDate.Text = System.DateTime.Now.ToShortDateString();
                //From.Text = dsFROM.Tables[0].Rows[0]["SystemName"].ToString();
               // FromAddress.Text = dsFROM.Tables[0].Rows[0]["SystemAddress"].ToString();
                To.Text = dsTo.Tables[0].Rows[0]["SystemName"].ToString();
                ToAddress.Text = dsTo.Tables[0].Rows[0]["SystemAddress"].ToString();
                #endregion
            }
        }

        public DataSet GetSystems(int ID)
        {
            DataSet ds = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", ID);



                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                ProductSet = ds;

            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            return ds;
        }
        public void LoadData(String OrderID)
        {
            #region Display Requests
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetGenSODetails_OrdID", connection);
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

            }
            finally
            {
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
                    if (connection.State == ConnectionState.Open)
                    {
                        connection.Close();
                    }
                    connection.Open();
                    SqlCommand command = new SqlCommand("sp_getSaleOrderDetail", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_OrderDetID", Convert.ToInt32(OrderDetailID.Text));

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

                }
                finally
                {
                    connection.Close();
                }
            }
                #endregion
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["OrderSalesDetail"] = true;
            Session["ViewSalesOrders"] = false;
            Response.Redirect("OrderSalesManual.aspx");
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
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["RequestedNO"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);

                MyExcel.FILE_PATH = Server.MapPath(@"~\SaleOrderFormat\").ToString();
                string convertedFilePath = MyExcel.WriteExcelWithSalesOrderInfo(SaleOrder.Text, SendDate.Text, (Environment.NewLine + To.Text + Environment.NewLine + ToAddress.Text), ds, Server.MapPath(@"~\SaleOrderFormat\"));

                Response.AppendHeader("content-disposition", "attachment; filename=" + convertedFilePath);
                Response.ContentType = "Application/msexcel";
                Response.WriteFile(convertedFilePath);
                Response.End();*/

                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetSaleOrderDetailList", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["RequestedNO"].ToString()));
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
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        protected void btnInvoice_Click(object sender, EventArgs e)
        {
            Session["OrderNo_Invoice"] = Session["RequestedNO"].ToString();
            Response.Redirect("SalesOrderInvoice.aspx");
        }

    }
}