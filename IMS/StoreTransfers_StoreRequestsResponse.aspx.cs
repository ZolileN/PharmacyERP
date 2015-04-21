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

namespace IMS
{
    public partial class StoreTransfers_StoreRequestsResponse : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                LoadData();
                ProductSet = new DataSet();
            }
        }

        public void LoadData()
        {
            RequestFrom.Text = Session["RequestedFrom"].ToString();
            RequestDate.Text = Session["RequestedDate"].ToString();

            #region Getting Products against an OrderID
            int RequestNumber = 0;
            if (int.TryParse(Session["RequestedNO"].ToString(), out RequestNumber))
            {
                try
                {
                    int SysID = 0;
                    if (int.TryParse(Session["UserSys"].ToString(), out SysID))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_GetOrderDetailsbyID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderID", RequestNumber);
                        command.Parameters.AddWithValue("@p_SysID", SysID);

                        DataSet ds = new DataSet();
                        SqlDataAdapter dA = new SqlDataAdapter(command);
                        dA.Fill(ds);
                        ProductSet = ds;

                        for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                        {
                            for (int j = 0; j < ds.Tables[1].Rows.Count; j++)
                            {
                                if (ds.Tables[0].Rows[i]["ProductID"].Equals(ds.Tables[1].Rows[j]["ProductID"]))
                                {
                                    ds.Tables[0].Rows[i]["Stock"] = ds.Tables[1].Rows[j]["Inventory"];
                                    break;
                                }
                            }
                        }

                        StockDisplayGrid.DataSource = null;
                        StockDisplayGrid.DataSource = ds.Tables[0];
                        StockDisplayGrid.DataBind();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                //send to Main Request Screen of warehouse
            }
            #endregion

        }

        public DataTable LoadData_Populate()
        {
            DataTable dt = new DataTable();
            #region Getting Products against an OrderID
            int RequestNumber = 0;
            if (int.TryParse(Session["RequestedNO"].ToString(), out RequestNumber))
            {
                try
                {
                    int SysID = 0;
                    if (int.TryParse(Session["UserSys"].ToString(), out SysID))
                    {
                        DataSet ds1 = new DataSet();
                        connection.Open();
                        SqlCommand command = new SqlCommand("sp_GetOrderDetailsbyID", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderID", RequestNumber);
                        command.Parameters.AddWithValue("@p_SysID", SysID);


                        SqlDataAdapter dA = new SqlDataAdapter(command);
                        dA.Fill(ds1);

                        DataView dv = ds1.Tables[0].DefaultView;
                        //dv.RowFilter = "Stock <> 0";
                        dt = dv.ToTable();
                    }
                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
            }
            else
            {
                //send to Main Request Screen of warehouse
            }
            #endregion
            return dt;

        }
        
        protected void btnDecline_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreTransfers_StoreRequests.aspx");
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateStock"))
                {
                    #region Updating SendQuantity in tblOrderDetails
                    Label OderDetailID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo");
                    TextBox Quantity = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity");

                    int RequestNumber = 0;
                    if (int.TryParse(OderDetailID.Text.ToString(), out RequestNumber))
                    {
                        try
                        {
                            int SndQuantity = 0;
                            if (int.TryParse(Quantity.Text.ToString(), out SndQuantity))
                            {
                                connection.Open();
                                SqlCommand command = new SqlCommand("sp_UpdateSendQuantity", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_OrderDetailID", RequestNumber);
                                command.Parameters.AddWithValue("@p_SndQuantity", SndQuantity);
                                command.ExecuteNonQuery();
                            }
                        }
                        catch (Exception ex)
                        {

                        }
                        finally
                        {
                            connection.Close();
                        }
                    }
                    else
                    {
                        //send to Main Request Screen of warehouse
                    }
                    #endregion

                    btnCancel.Enabled = false;
                }
            }
            catch (Exception ex)
            {
            }
            finally
            {
                StockDisplayGrid.EditIndex = -1;
                LoadData();
            }
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            /*if(e.Row.Cells[2].Text.Equals("") || e.Row.Cells[2].Text.Equals("0") || e.Row.Cells[2].Text.Equals(null))
            {
                e.Row.Enabled = false;
            }*/
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            LoadData();
        }

        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnAccept_Click1(object sender, EventArgs e)
        {
            #region Accepting Order
            try
            {
                DataTable temptbl = LoadData_Populate();

                for (int i = 0; i < temptbl.Rows.Count; i++)
                {
                    int OrderDetailID = 0;
                    if (int.TryParse(temptbl.Rows[i]["OrderDetailID"].ToString(), out OrderDetailID))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Sp_Response_Warehouse_Store", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                        command.ExecuteNonQuery();

                        #region Updating Stocks
                        SqlCommand command2 = new SqlCommand("sp_UpdateStock_ProductID", connection);
                        command2.CommandType = CommandType.StoredProcedure;
                        command2.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                        int RequestNumber = 0;
                        if (int.TryParse(Session["RequestedNO"].ToString(), out RequestNumber))
                        {
                            command2.Parameters.AddWithValue("@p_OrderID", RequestNumber);
                        }
                        int SysID = 0;
                        if (int.TryParse(Session["UserSys"].ToString(), out SysID))
                        {
                            command.Parameters.AddWithValue("@p_SysID", SysID);
                        }
                        command2.ExecuteNonQuery();
                        #endregion

                        connection.Close();
                    }
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion

            Response.Redirect("StoreTransfers_StoreRequests.aspx");
        }
    }
}