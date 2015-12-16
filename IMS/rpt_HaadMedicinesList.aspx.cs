using CrystalDecisions.CrystalReports.Engine;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class rpt_HaadMedicinesList : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                LoadCatgeory();
                LoadSubCatgeory();
            }
        }

        
        public void LoadCatgeory()
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("SP_GetCategoryDeptWise", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@DeptName", "HAAD Medicine");

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sdA.Fill(ds);


                drpCat.DataSource = ds;
                drpCat.DataTextField = "Name";
                drpCat.DataValueField = "CategoryID";
                drpCat.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        public void LoadSubCatgeory()
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("SP_GetSubCatByCatWise", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CatgId", drpCat.SelectedValue);

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sdA.Fill(ds);


                DrpSubCat.DataSource = ds;
                DrpSubCat.DataTextField = "Name";
                DrpSubCat.DataValueField = "Sub_CatID";
                DrpSubCat.DataBind();

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

      

        protected void drpCat_SelectedIndexChanged(object sender, EventArgs e)
        {
           
            LoadSubCatgeory();
        }
        public void LoadReportData()
        {
            try
            {
                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("SP_Get_HAAD_Medicine_By_Sub_Category", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Sub_CategoryID", DrpSubCat.SelectedValue);

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sdA.Fill(ds);


                ReportDocument myReportDocument = new ReportDocument();

                myReportDocument.Load(Server.MapPath("~/HaadMedicinesList.rpt"));

                myReportDocument.SetDataSource(ds.Tables[0]);

                myReportDocument.SetParameterValue("rptName", "HAAD LIST REPORT");

              


                Session["ReportDocument"] = myReportDocument;
                Session["ReportPrinting_Redirection"] = "rpt_HaadMedicinesList.aspx";

                Response.Redirect("CrystalReportViewer.aspx");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }
        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            LoadReportData();

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }
    }
}