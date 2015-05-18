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
    public partial class RecieveSOFull : System.Web.UI.Page
    {

        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public DataSet ProductSet;
        public DataSet systemSet;
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                BindGrid();
            }
        }

        private void BindGrid()
        {
            DataSet ds = new DataSet();
            #region Display Products
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetSODetails_ID", connection);
                command.CommandType = CommandType.StoredProcedure;
                int OrderNumber = 0;


                if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                }





                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                Session["dsProdcts"] = ds;
                ProductSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                if (StockDisplayGrid.DataSource == null)
                {
                    Session["FirstOrderSO"] = false;
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

        }
        protected void btnAcceptAll_Click(object sender, EventArgs e)
        {

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            //First clear all the sessions if created in this page
            Response.Redirect("ReceiveSalesOrder.aspx");
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Label Status = (Label)e.Row.FindControl("lblStatus");
                Button btnEdit = (Button)e.Row.FindControl("btnEdit");
                Button btnDetail = (Button)e.Row.FindControl("btnDetails");
                Button btnDelete = (Button)e.Row.FindControl("btnDelete");

                if (Status.Text.Equals("Complete") || Status.Text.Equals("Partial"))
                {
                    btnDelete.Enabled = false;
                }
                else
                {
                    btnEdit.Enabled = true;
                    btnDelete.Enabled = true;
                }

                if (Session["ExistingOrder"].Equals(true))
                {
                    // btnDetail.Enabled = false;
                }
                else
                {
                    btnDetail.Enabled = true;
                }
                Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                Label Label1 = (Label)e.Row.FindControl("Label1");

                Label dosage = (Label)e.Row.FindControl("dosage2");
                Label Label2 = (Label)e.Row.FindControl("Label2");

                Label packSize = (Label)e.Row.FindControl("packSize2");
                Label Label3 = (Label)e.Row.FindControl("Label3");

                if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                {
                    ProductStrength.Visible = false;
                    Label1.Visible = false;
                }
                else
                {
                    ProductStrength.Visible = true;
                    Label1.Visible = true;
                }

                if (String.IsNullOrWhiteSpace(dosage.Text))
                {
                    dosage.Visible = false;
                    Label2.Visible = false;
                }
                else
                {
                    dosage.Visible = true;
                    Label2.Visible = true;
                }

                if (String.IsNullOrWhiteSpace(packSize.Text))
                {
                    packSize.Visible = false;
                    Label3.Visible = false;
                }
                else
                {
                    packSize.Visible = true;
                    Label3.Visible = true;
                }

                Label OrderDetailID = (Label)e.Row.FindControl("OrderDetailNo");
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

                #endregion

            }


        }

        protected void StockDisplayGrid_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName.Equals("Edit"))
            {
                Label OrderDetNo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderDetailNo");
                Label ProductID = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("lblProductID");
                Label RequestedTo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("RequestedTo");
                Label OrderTo = (Label)StockDisplayGrid.Rows[Convert.ToInt32(e.CommandArgument)].FindControl("OrderTo");
                //session is setting
                Session["OrderDetNo"] = OrderDetNo.Text.ToString();
                Session["ProductID"] = ProductID.Text;
                Session["RequestedTo"] = RequestedTo.Text;

                Response.Redirect("RecieveSOFullEdit.aspx");
            }
                 
        }
    }
}