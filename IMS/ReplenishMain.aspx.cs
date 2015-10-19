using IMS.Util;
using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;


namespace IMS
{
    public partial class ReplenishMain : System.Web.UI.Page
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
                    String Parameter = "";
                    txtToDate.Attributes.Add("onchange", "changeValues();return false;");

                    if (Request.QueryString["Param"] != null)
                    {
                        Parameter = Request.QueryString["Param"].ToString();
                        if (Parameter.Equals("Movement"))
                        {
                            Session["parameter"] = "Movement";
                        }
                        else if (Parameter.Equals("Calculation"))
                        {
                            Session["parameter"] = "Calculation";
                        }
                    }

                    if (Parameter.Equals("Calculation") || (Session["parameter"] != null && Session["parameter"].ToString().Equals("Calculation")))
                    {
                        Session["parameter"] = "Calculation";

                        lblSaleDates.Visible = true;
                        lblFromDate.Visible = true;
                        lblToDate.Visible = true;
                        lblReplenishDays.Visible = true;
                        lblReplenishHeader.Text = "Replenish ( Calculation )";
                        txtFromDate.Visible = true;
                        txtReplenishDays.Visible = true;
                        txtToDate.Visible = true;



                    }
                    else
                    {
                        Session["parameter"] = "Movement";
                        lblReplenishHeader.Text = "Replenish ( Movement )";
                        lblSaleDates.Visible = false;
                        lblFromDate.Visible = false;
                        lblToDate.Visible = false;
                        lblReplenishDays.Visible = false;

                        txtFromDate.Visible = false;
                        txtReplenishDays.Visible = false;
                        txtToDate.Visible = false;
                    }
                    LoadVendors();

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
        public void LoadVendors()
        {
            int SystemID =0;
            int.TryParse(Session["UserSys"].ToString(), out SystemID);
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand cmd = new SqlCommand("sp_ReplenishVendors_bySales",connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SystemID", SystemID);
                DataSet  ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(ds);

                DataTable dtVendors = ds.Tables[0];

                for (int i = 0; i < ds.Tables[1].Rows.Count;i++)
                {
                    DataRow dtRow = dtVendors.NewRow();
                    dtRow["OrderRequestedFor"] = Convert.ToInt32(ds.Tables[1].Rows[i]["OrderRequestBy"].ToString());
                    dtRow["SupName"] = ds.Tables[1].Rows[i]["SystemName"].ToString();

                    dtVendors.Rows.Add(dtRow);
                    dtVendors.AcceptChanges();
                }

                    ddlVendorNames.DataSource = ds.Tables[0];
                    ddlVendorNames.DataValueField = "OrderRequestedFor";
                    ddlVendorNames.DataTextField = "SupName";
                    ddlVendorNames.DataBind();

                    ddlVendorNames.Items.Add("All Vendors");

                   ddlVendorNames.SelectedIndex = ddlVendorNames.Items.IndexOf(ddlVendorNames.Items.FindByValue("All Vendors"));

            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
              //ex Message should be displayed
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }
        protected void btnBack_Click(object sender, EventArgs e)
        {

            Response.Redirect("StoreMain.aspx");
        }

        protected void ddlVendorNames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnGenerate_Click(object sender, EventArgs e)
        {
            try
            {
                if (txtFromDate.Text.Equals(txtToDate.Text) && Session["parameter"].Equals("Calculation"))
                {
                    WebMessageBoxUtil.Show("From & Two Dates cannot be equal, please change it");
                }
                else
                {
                    Session["FromSalesDate"] = txtFromDate.Text;
                    Session["ToSalesDate"] = txtToDate.Text;
                    Session["ReplenishDays"] = txtReplenishDays.Text;

                    if (ddlVendorNames.SelectedItem.ToString().Equals("All Vendors"))
                    {
                        Session["ReplenishVendorID"] = -1;
                        Response.Redirect("ReplenishMovement.aspx");
                    }
                    else
                    {
                        Session["ReplenishVendorID"] = ddlVendorNames.SelectedValue;
                        Response.Redirect("ReplenishMovement.aspx");
                    }
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
    }
}