using CrystalDecisions.CrystalReports.Engine;
using IMSBusinessLogic;
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
        ReportBLL reportbll = new ReportBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
                LoadCatgeory();
                if (drpCat.SelectedIndex != -1)
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
               

                DataSet ds = reportbll.rpt_HaadNonHaadMedicinesList(Int32.Parse(DrpSubCat.SelectedValue));
                
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
           
        }
        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            if(DrpSubCat.SelectedIndex!=-1)
            LoadReportData();

        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }
    }
}