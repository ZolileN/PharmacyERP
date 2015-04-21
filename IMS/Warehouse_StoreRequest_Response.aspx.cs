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


namespace IMS
{
    public partial class Warehouse_StoreRequest_Response : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
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
                        
                        for (int i = 0; i < ds.Tables[0].Rows.Count;i++ )
                        {
                            for(int j=0;j<ds.Tables[1].Rows.Count;j++)
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
                catch(Exception ex)
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
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            LoadData();
        }

        protected void StockDisplayGrid_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = -1;
            LoadData();
        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            try
            {
                if (e.CommandName.Equals("UpdateStock"))
                {
                    #region Updating SendQuantity in tblOrderDetails
                    Label OderDetailID = (Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("OrderDetailNo");
                    string status = ((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblStatus")).Text;
                    TextBox Quantity = (TextBox)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("txtQuantity");
                    int remQuan = int.Parse(((Label)StockDisplayGrid.Rows[StockDisplayGrid.EditIndex].FindControl("lblRemainQuan")).Text);
                    int RequestNumber = 0;
                    if (int.TryParse(OderDetailID.Text.ToString(), out RequestNumber))
                    {
                        try
                        {
                            int SndQuantity = 0;
                            if (int.TryParse(Quantity.Text.ToString(), out SndQuantity))
                            {
                                if (SndQuantity > remQuan) 
                                {
                                    WebMessageBoxUtil.Show("Your remaining quantity cannot be larger than " + remQuan);
                                    StockDisplayGrid.EditIndex = -1;
                                    LoadData();
                                    return;
                                }
                                connection.Open();
                                SqlCommand command = new SqlCommand("sp_UpdateSendQuantity", connection);
                                command.CommandType = CommandType.StoredProcedure;
                                command.Parameters.AddWithValue("@p_OrderDetailID", RequestNumber);
                                command.Parameters.AddWithValue("@p_SndQuantity", SndQuantity);
                                if (status.Equals("Partial"))
                                {
                                    command.Parameters.AddWithValue("@p_Status", "Partial");
                                }
                                else 
                                {
                                    command.Parameters.AddWithValue("@p_Status", "Pending");
                                }
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

                    btnDecline.Enabled = false;
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
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Stock = (Label)e.Row.FindControl("Stock");
                Button Action = (Button)e.Row.FindControl("btnEdit");

                if (Stock.Text.Equals("") || Stock.Text.Equals("0") || Stock.Text.Equals(null))
                {
                    Action.Visible = false;
                }
                else
                {
                    Action.Visible = true;
                }

                //Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                //Label Label1 = (Label)e.Row.FindControl("Label1");

                //Label dosage = (Label)e.Row.FindControl("dosage2");
                //Label Label2 = (Label)e.Row.FindControl("Label2");

                //Label packSize = (Label)e.Row.FindControl("packSize2");
                //Label Label3 = (Label)e.Row.FindControl("Label3");

                //if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                //{
                //    ProductStrength.Visible = false;
                //    Label1.Visible = false;
                //}
                //else
                //{
                //    ProductStrength.Visible = true;
                //    Label1.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(dosage.Text))
                //{
                //    dosage.Visible = false;
                //    Label2.Visible = false;
                //}
                //else
                //{
                //    dosage.Visible = true;
                //    Label2.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(packSize.Text))
                //{
                //    packSize.Visible = false;
                //    Label3.Visible = false;
                //}
                //else
                //{
                //    packSize.Visible = true;
                //    Label3.Visible = true;
                //}
            }
            
                
            
        }

        protected void StockDisplayGrid_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void StockDisplayGrid_RowEditing(object sender, GridViewEditEventArgs e)
        {
            StockDisplayGrid.EditIndex = e.NewEditIndex;
            LoadData();
        }

        protected void btnAccept_Click(object sender, EventArgs e)
        {
            #region Accepting Order
            try
            {
                DataTable temptbl = ProductSet.Tables[0];

                for(int i=0;i<temptbl.Rows.Count;i++)
                {
                    int OrderDetailID = 0;
                    if (int.TryParse(temptbl.Rows[i]["OrderDetailID"].ToString(), out OrderDetailID))
                    {
                        connection.Open();
                        SqlCommand command = new SqlCommand("Sp_Response_Warehouse_Store", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                        command.ExecuteNonQuery();
                        connection.Close();
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            #endregion
        }

        protected void btnDecline_Click(object sender, EventArgs e)
        {
            Response.Redirect("Warehouse_StoreRequests.aspx");
        }

        protected void StockDisplayGrid_SelectedIndexChanged(object sender, EventArgs e)
        {
            LoadData();
        }

        protected void btnAccept_Click1(object sender, EventArgs e)
        {
            bool Status = false;
            #region Accepting Order
            try
            {
                DataTable temptbl = LoadData_Populate();
                
                for (int i = 0; i < temptbl.Rows.Count; i++)
                {
                    if (temptbl.Rows[i]["SndQauntity"].ToString().Equals("0") || temptbl.Rows[i]["SndQauntity"].ToString().Equals(null) || temptbl.Rows[i]["SndQauntity"].ToString().Equals(""))
                    {
                    }
                    else
                    {
                        int OrderDetailID = 0;
                        if (int.TryParse(temptbl.Rows[i]["OrderDetailID"].ToString(), out OrderDetailID))
                        {
                            int quan=int.Parse(temptbl.Rows[i]["SndQauntity"].ToString());
                            UpdateStock(OrderDetailID,quan);
                            connection.Open();
                            SqlCommand command = new SqlCommand("Sp_Response_Warehouse_Store", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_OrderDetailID", OrderDetailID);
                            //command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));
                            command.ExecuteNonQuery();
                            //SqlDataAdapter dA = new SqlDataAdapter(command);
                            //DataTable dtValues = new DataTable();
                            //dA.Fill(dtValues);

                           /* for (int x = 0; x < dtValues.Rows.Count; x++)
                            {
                                #region Updating Stocks

                                SqlCommand command2 = new SqlCommand("sp_UpdateStock_ProductID", connection);
                                command2.CommandType = CommandType.StoredProcedure;
                                
                                command2.Parameters.AddWithValue("@p_Barcode", Convert.ToInt32(dtValues.Rows[x]["BarCode"].ToString()));
                                
                                command2.Parameters.AddWithValue("@p_Quantity", Convert.ToInt32(dtValues.Rows[x]["SendQuantity"].ToString()));
                                
                                command.Parameters.AddWithValue("@p_SysID", Convert.ToInt32(Session["UserSys"].ToString()));
                                
                                command2.ExecuteNonQuery();
                                #endregion
                            }*/

                            connection.Close();
                        }
                    }
                }
                Status = true;
            }
            catch (Exception ex)
            {
                Status = false;
            }
            finally
            {
                connection.Close();
                Response.Redirect("Warehouse_StoreRequests.aspx");
            }
            #endregion
        }

        private void UpdateStock(int orderDetailID, int quantity) 
        {
            try
            {
                DataSet stockDet;
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetStockBy_OrderDetID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_StoredAt", int.Parse(Session["UserSys"].ToString()));

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);
                stockDet = ds;
                Dictionary<int, int> stockSet = new Dictionary<int, int>();

                foreach (DataRow row in  stockDet.Tables[0].Rows)
                {
                    int exQuan=int.Parse(row["Quantity"].ToString());
                    if (quantity > 0)
                    {
                        if (exQuan >= quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), quantity);
                            break;
                        }
                        else if (exQuan < quantity)
                        {
                            stockSet.Add(int.Parse(row["StockID"].ToString()), exQuan);
                            quantity = quantity - exQuan;
                        }
                    }
                }


                foreach (int id in stockSet.Keys) 
                {
                    command = new SqlCommand("Sp_UpdateStockBy_StockID", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", id);
                    command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    command.Parameters.AddWithValue("@p_Action", "Minus");
                    command.ExecuteNonQuery();

                    #region Generation of Packing List
                    command = new SqlCommand("sp_PackingListGeneration", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_StockID", id);
                    command.Parameters.AddWithValue("@p_quantity", stockSet[id]);
                    command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                    command.ExecuteNonQuery();
                    #endregion
                }
                
            }
            catch (Exception exp) { }
            finally
            {
                connection.Close();
                //Response.Redirect("Warehouse_StoreRequests.aspx");
            }
        }
    }
}