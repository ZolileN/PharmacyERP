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
using System.IO;

namespace IMS
{
    public partial class ViewPackingList_SO : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
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
            Response.Redirect("ManageOrders.aspx", false);
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {

        }

        protected void btnExport_Click(object sender, EventArgs e)
        {
            try
            {
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
                //Byte[] fileBytes = File.ReadAllBytes(Path.Combine(MyExcel.FILE_PATH, files[1]));

                //vnd.openxmlformats-officedocument.spreadsheetml.sheet
                //application/msexcel

                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AppendHeader("content-disposition", "attachment;filename=" + fileName);
                //Response.TransmitFile(fileName);
                //Response.End();
                //using (MemoryStream MyMemoryStream = new MemoryStream())
                //{
                //    myWorkBook.SaveAs(MyMemoryStream);
                //    //Response.Write(MyMemoryStream);
                //    MyMemoryStream.WriteTo(Response.OutputStream);
                //    //Response.Flush();
                //    //Response.End();
                //}




                //Byte[] fileBytes = File.ReadAllBytes(Path.Combine(MyExcel.FILE_PATH, convertedFilePath));

                //Clear the response               
                //Response.Clear();
                //Response.ClearContent();
                //Response.ClearHeaders();
                //Response.Cookies.Clear();
                ////Add the header & other information      
                //Response.Cache.SetCacheability(HttpCacheability.Private);
                //Response.CacheControl = "private";
                //Response.Charset = System.Text.UTF8Encoding.UTF8.WebName;
                //Response.ContentEncoding = System.Text.UTF8Encoding.UTF8;
                ////Response.AppendHeader("Content-Length", fileBytes.Length.ToString());
                ////Response.AppendHeader("Pragma", "cache");
                ////Response.AppendHeader("Expires", "60");
                ////Response.AppendHeader("Content-Disposition",
                ////"attachment; " +
                ////"filename=\"ExcelReport.xlsx\"; " +
                ////"size=" + fileBytes.Length.ToString() + "; " +
                ////"creation-date=" + DateTime.Now.ToString("R") + "; " +
                ////"modification-date=" + DateTime.Now.ToString("R") + "; " +
                ////"read-date=" + DateTime.Now.ToString("R"));
                //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                ////Write it back to the client    
                //Response.BinaryWrite(fileBytes);
                //Response.End();
                //

                Response.Clear();
                Response.Buffer = true;
                Response.AppendHeader("content-disposition", "attachment; filename=" + files[1]);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                //Response.AddHeader("content-disposition", "attachment;filename=" + files[1]);
                //Response.Charset = "";
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                ////Response.Redirect(fileName, false);
                ////Response.BinaryWrite(fileBytes);
                //Response.Output.Write(files[1]);
                //Response.Flush();
                //Response.End();

                //Response.Clear();
                //Response.Buffer = true;
                Response.Charset = "UTF-8";
                //Response.ContentType = "application/vnd.ms-excel";
                //@"SaleOrderFormat\"+
                String Path1 = Path.Combine(files[0], files[1]);
                string url = @"~/SaleOrderFormat/" + files[1];
                //Response.WriteFile(Path1);

                Response.Redirect(url);
                Response.End();
            }
            catch (Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                connection.Close();
                //Response.End();
            }
        }

    }
}