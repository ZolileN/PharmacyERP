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
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using IMSBusinessLogic;
using log4net;
using IMS.Util;

namespace IMS
{
    public partial class StockCostAdjustment : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        DataTable dt = new DataTable();
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // for Testing - must Remove after linkage
                txtPhysicalQuantity.Attributes.Add("onchange", "changeValues();return false;");
                Session["AdjustmentPorductID"] = 236456;
                Session["AdjustmentStockID"] = 50052;
                // ----------------------------------------    

                if (Session["AdjustmentPorductID"] != null && Session["AdjustmentPorductID"].ToString() != "" &&
                    Session["AdjustmentStockID"]!=null && Session["AdjustmentStockID"].ToString()!="") 
                {
                    try
                    {
                        int ProductID = Convert.ToInt32(Session["AdjustmentPorductID"].ToString());
                        int StockID = Convert.ToInt32(Session["AdjustmentStockID"].ToString());
                        ViewState["productID"] = ProductID;
                        ViewState["stockID"] = StockID;

                        if (Request.QueryString["AdjustmentOperation"] != null && Request.QueryString["AdjustmentOperation"].ToString() != "")
                        {
                            if (Request.QueryString["AdjustmentOperation"].ToString().ToLower().Equals("cost"))
                            {
                                lblHeader.Text = "Stock Price Adjustment";
                                txtPhysicalQuantity.Enabled = false;
                            }
                            else if (Request.QueryString["AdjustmentOperation"].ToString().ToLower().Equals("stock"))
                            {
                                lblHeader.Text = "Stock Quantity Adjustment";
                                txtNewCP.Enabled = false;
                                txtNewSP.Enabled = false;
                            }
                            else if (Request.QueryString["AdjustmentOperation"].ToString().ToLower().Equals("coststock"))
                            {
                                lblHeader.Text = "Stock Quantity & Price Adjustment";
                            }
                        }

                        LoadData(ProductID, StockID);
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    finally
                    {

                    }
                }
            }
        }

        public void LoadData(int ProductID, int StockID)
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("sp_getAdjustmentStock", connection);
                command.CommandType = CommandType.StoredProcedure;

                command.Parameters.AddWithValue("@p_ProductID", ProductID);
                command.Parameters.AddWithValue("@p_StockID", StockID);

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sdA.Fill(ds);

                dt = ds.Tables[0];
                ViewState["LoadedData"] = dt;
                if(dt!=null)
                {
                    txtBarcode.Text = dt.Rows[0]["BarCode"].ToString();
                    txtUPC.Text = dt.Rows[0]["Product_Id_Org"].ToString();
                    txtGreenRain.Text = dt.Rows[0]["ItemCode"].ToString();

                    txtProduct.Text = dt.Rows[0]["prodDesc"].ToString();

                    txtSystemQuantity.Text = dt.Rows[0]["Quantity"].ToString();
                    txtPhysicalQuantity.Text = dt.Rows[0]["Quantity"].ToString();


                    txtExpiry.Text = dt.Rows[0]["ExpiryDate"].ToString();

                    txtPrevCP.Text = dt.Rows[0]["UnitCost"].ToString();
                    txtNewCP.Text = dt.Rows[0]["UnitCost"].ToString();

                    txtPrevSP.Text = dt.Rows[0]["SP"].ToString();
                    txtNewSP.Text = dt.Rows[0]["SP"].ToString();
                }
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                #region Values to be Saved

                int ProductID = Convert.ToInt32(ViewState["productID"].ToString());
                int StockID = Convert.ToInt32(ViewState["stockID"].ToString());
                
                int CurrentStock = Convert.ToInt32(txtSystemQuantity.Text.ToString());
                int NewStock = Convert.ToInt32(txtPhysicalQuantity.Text.ToString());
                
                float PrevCP = float.Parse(txtPrevCP.Text.ToString());
                float NewCP = float.Parse(txtNewCP.Text.ToString());
                
                float PrevSP = float.Parse(txtPrevSP.Text.ToString());
                float NewSP = float.Parse(txtNewSP.Text.ToString());

                DateTime Expiry = DateTime.Parse(txtExpiry.Text.ToString());

                int UserID = Convert.ToInt32(Session["UserID"].ToString());
                int SystemID = Convert.ToInt32(Session["UserSys"].ToString());
                #endregion


                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("sp_InsertAdjutmentStock", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_ProductID", ProductID);
                command.Parameters.AddWithValue("@p_StockID", StockID);
                command.Parameters.AddWithValue("@p_CurrentStock", CurrentStock);
                command.Parameters.AddWithValue("@p_newStock", NewStock);
                command.Parameters.AddWithValue("@p_PrevCP", PrevCP);
                command.Parameters.AddWithValue("@p_NewCP", NewCP);
                command.Parameters.AddWithValue("@p_PrevSP", PrevSP);
                command.Parameters.AddWithValue("@p_NewSP", NewSP);
                command.Parameters.AddWithValue("@p_Reason", txtReason.Text.ToString());

                command.Parameters.AddWithValue("@p_UserID", UserID);
                command.Parameters.AddWithValue("@p_SystemID", SystemID);
                command.Parameters.AddWithValue("@p_Expiry", Expiry);

                command.ExecuteNonQuery();

                txtAdjustmentQuantity.Text = (NewStock - CurrentStock).ToString();

                LoadData(ProductID, StockID);
            }
            catch(Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        protected void btnCancel_Click(object sender, EventArgs e)
        {

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {

        }
    }
}