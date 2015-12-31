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
using IMSBusinessLogic;
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;

namespace IMS
{
    public partial class rpt_InventorySummaryReport : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        ReportBLL reportbll = new ReportBLL();
        public ReportDocument myReportDocument = new ReportDocument();

        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {

                   

                   

                    #region Populating Product Department DropDown
                    try
                    {
                        ProductDept.DataSource = DepartmentBLL.GetAllDepartment(); ;
                        ProductDept.DataTextField = "Name";
                        ProductDept.DataValueField = "DepId";
                        ProductDept.DataBind();
                        //if (ProductDept != null)
                        //{
                        //    ProductDept.Items.Insert(0, "Select Department");
                        //    ProductDept.SelectedIndex = 0;
                        //}
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
                    #endregion

                  

                    

                  

                    

                    
                   
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
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
        
        

        
        

      

     

        

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("Inventory_StoreID");
            if (Convert.ToInt32(Session["UserSys"]).Equals(1))
            {
                Response.Redirect("WarehouseMain.aspx", false);
            }
            else
            {
                Response.Redirect("StoreMain.aspx", false);
            }
        }

        
       

       

        protected void btnShowREPORT_Click(object sender, EventArgs e) {

            int DepartmentID = int.Parse(ProductDept.SelectedValue);
            





            DataSet ds = reportbll.rpt_InventorySummaryReport(DepartmentID);

            if (ds.Tables[0].Rows.Count > 0)
            {
                myReportDocument.Load(Server.MapPath("~/InventorySummaryReport.rpt"));
                App_Code.Barcode dsReport = new App_Code.Barcode();
                try { dsReport.Tables["sp_rptInventoySummaryReport"].Merge(ds.Tables[0]); }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.ToString());
                }

                myReportDocument.SetDataSource(dsReport.Tables[0]);



                myReportDocument.SetParameterValue("Department", ProductDept.SelectedItem.Text);







                Session["ReportDocument"] = myReportDocument;
                Session["ReportPrinting_Redirection"] = "rpt_InventorySummaryReport.aspx";

                Response.Redirect("CrystalReportViewer.aspx");
            }
            else {
                WebMessageBoxUtil.Show("There is no data against these filters");
            }

            

            

        }

       

       

      

        
        protected void btnPrint_Click(object sender, EventArgs e)
        {

            #region Setting  parameters
            Session["Search_DepID"] = ProductDept.SelectedValue.ToString();
            
            
           
            Session["Search_ProdId"] = "";
            
           

            #endregion

            Response.Redirect("Inventory_Print.aspx",false);


        }

        protected void btnSearchProduct_Click1(object sender, ImageClickEventArgs e)
        {
         
        }

    

        
    }
}