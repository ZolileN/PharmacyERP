using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
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
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                String Parameter = "";
                if (Request.QueryString["Param"] != null)
                {
                    Parameter = Request.QueryString["Param"].ToString();
                    if(Parameter.Equals("Movement"))
                    {
                        Session["parameter"] = "Movement";
                    }
                    else if (Parameter.Equals("Calculation"))
                    {
                        Session["parameter"] = "Calculation";
                    }
                }

                if (Parameter.Equals("Calculation") || (Session["parameter"]!=null && Session["parameter"].ToString().Equals("Calculation")))
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
        }

        public void LoadVendors()
        {
            int SystemID =0;
            int.TryParse(Session["UserSys"].ToString(), out SystemID);
            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_ReplenishVendors_bySales",connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@SystemID", SystemID);
                DataSet  ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(ds);
                ddlVendorNames.DataSource = ds.Tables[0];
                ddlVendorNames.DataValueField = "VendorID";
                ddlVendorNames.DataTextField = "VendorName";
                ddlVendorNames.DataBind();

                ddlVendorNames.Items.Add("All Vendors");

                ddlVendorNames.SelectedIndex = ddlVendorNames.Items.IndexOf(ddlVendorNames.Items.FindByValue("All Vendors"));

            }
            catch(Exception ex)
            {
              //ex Message should be displayed
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
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
}