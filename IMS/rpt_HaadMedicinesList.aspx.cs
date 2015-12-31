using CrystalDecisions.CrystalReports.Engine;
using IMSBusinessLogic;
using IMSCommon.Util;
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
                if (ds != null)
                {
                    drpCat.Items.Insert(0, "All");
                    drpCat.SelectedIndex = 0;
                }

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
                int? CategoryId ;
                string selectedSubCategory = drpCat.SelectedValue;
                
                if (selectedSubCategory.Equals("All"))
                    CategoryId = null;
                else
                    CategoryId = Int32.Parse(drpCat.SelectedValue);

                if (connection.State == ConnectionState.Closed) { connection.Open(); }
                SqlCommand command = new SqlCommand("SP_GetSubCatByCatWise", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@CatgId", CategoryId);

                SqlDataAdapter sdA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                sdA.Fill(ds);


                DrpSubCat.DataSource = ds;
                DrpSubCat.DataTextField = "Name";
                DrpSubCat.DataValueField = "Sub_CatID";
                DrpSubCat.DataBind();
                if (ds != null)
                {
                    DrpSubCat.Items.Insert(0, "All");
                    DrpSubCat.SelectedIndex = 0;
                }

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
                int? SubCategory,CatID;
                string selectedSubCategory = DrpSubCat.SelectedValue;
                string selectedCategory = drpCat.SelectedValue;
                if (selectedCategory.Equals("All"))
                    CatID = null;
                else
                    CatID = Int32.Parse(drpCat.SelectedValue);

                if (selectedSubCategory.Equals("All"))
                    SubCategory = null;
                else
                    SubCategory = Int32.Parse(DrpSubCat.SelectedValue);
                DataSet ds = reportbll.rpt_HaadNonHaadMedicinesList(4028, CatID, SubCategory);
                if(ds.Tables[0].Rows.Count > 0)
                {
                    ReportDocument myReportDocument = new ReportDocument();

                    myReportDocument.Load(Server.MapPath("~/HaadMedicinesList.rpt"));

                    myReportDocument.SetDataSource(ds.Tables[0]);

                    myReportDocument.SetParameterValue("rptName", "HAAD LIST REPORT"); 

                    Session["ReportDocument"] = myReportDocument;
                    Session["ReportPrinting_Redirection"] = "rpt_HaadMedicinesList.aspx";

                    Response.Redirect("CrystalReportViewer.aspx");
                }
                else
                    WebMessageBoxUtil.Show("No data found against this Filter");
                
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