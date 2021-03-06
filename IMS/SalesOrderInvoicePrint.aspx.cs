﻿using System;
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
    public partial class SalesOrderInvoicePrint : System.Web.UI.Page
    {

        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet InvoiceSet;

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                if (!IsPostBack)
                {
                    InvoiceDate.Text = Session["PrintInvoiceDate"].ToString();
                    DueDate.Text = Session["PrintDueDate"].ToString();
                    SalesMan.Text = Session["SalesMan"].ToString();

                    if (Session["PrintCheck"].ToString().Equals("Actual"))
                    {
                        Invoice.Text = Session["PrintInvoiceNumber"].ToString();
                        lblTotalBonusAmount.Visible = false;
                        //Label9.Visible = false;
                    }
                    else if (Session["PrintCheck"].ToString().Equals("Bonus"))
                    {
                        Invoice.Text = "B" + Session["PrintInvoiceNumber"].ToString();
                        lblTotalSentAmount.Visible = false;
                        //Label8.Visible = false;
                    }
                    DataSet dsTo = GetSystems(Convert.ToInt32(Session["RequestedFromID"].ToString()));
                    To.Text = dsTo.Tables[0].Rows[0]["SystemName"].ToString();
                    ToAddress.Text = dsTo.Tables[0].Rows[0]["SystemAddress"].ToString();
                    LoadData();
                }
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
        public void LoadData()
        {
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_GetSaleOrderDetailList_Bonus", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderID", Convert.ToInt32(Session["OrderNo_Invoice"].ToString()));
                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                InvoiceSet = ds;
                StockDisplayGrid.DataSource = null;
                StockDisplayGrid.DataSource = ds.Tables[0];
                StockDisplayGrid.DataBind();

                double BonusAmount = 0;
                double ActualAmount = 0;
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    BonusAmount += Convert.ToInt32(ds.Tables[0].Rows[i]["AmountBonus"]);
                    ActualAmount += Convert.ToInt32(ds.Tables[0].Rows[i]["Amount"]);
                }

                lblTotalBonusAmount.Text = BonusAmount.ToString();
                lblTotalSentAmount.Text = ActualAmount.ToString();
            }
            catch (Exception ex)
            {
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

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

            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
            return ds;
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SalesOrderInvoice.aspx",false);
        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (Session["PrintCheck"].ToString().Equals("Actual"))
                {
                    e.Row.Cells[7].Visible = false;
                    e.Row.Cells[8].Visible = false;
                }
                else if (Session["PrintCheck"].ToString().Equals("Bonus"))
                {
                    e.Row.Cells[5].Visible = false;
                    e.Row.Cells[6].Visible = false;
                }
            }
            catch (Exception ex)
            {
                
                //throw;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }
    }
}