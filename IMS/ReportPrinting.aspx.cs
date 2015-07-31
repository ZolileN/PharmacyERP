using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using IMS.Util;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing.Printing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ReportPrinting : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if(!IsPostBack)
            {
                ReportDocument doc = new ReportDocument();
                doc = (ReportDocument)Session["ReportDocument"];
                doc.ExportToDisk(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, Server.MapPath(@"~\CrystalReports\Report.pdf"));

                hdnResultValue.Value = Server.MapPath(@"~\CrystalReports\Report.pdf");
                //PrintingReport();
                //doc.PrintOptions.PrinterName = hdnResultValue.Value;//GetDefaultPrinter();
                //doc.PrintToPrinter(1, false, 0, 0);
                //CrystalReportViewer ctrl = (CrystalReportViewer)Session["ReportDoc_PrintingControl"];
                //PrintHelper_WebControl.PrintWebControl(ctrl);

                //Page.ClientScript.RegisterStartupScript(this.GetType(), "CallMyFunction", "printPDF('" + hdnResultValue.Value + "')", true);
            }
            expHandler.CheckForErrorMessage(Session);
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
        public void PrintingReport()
        {
            printDiv.InnerText = string.Empty;

            try
            {
                using (StreamReader streamReader = new StreamReader(Server.MapPath(@"~\CrystalReports\Report.pdf")))
                {
                    string lineIn = string.Empty;

                    while ((lineIn = streamReader.ReadLine()) != null)
                    {
                        if (printDiv.InnerText.Length > 0)
                            printDiv.InnerText += "<br>";

                        printDiv.InnerText += lineIn;
                    }
                }
            }
            catch (Exception ex)
            {
               
                throw ex;
            }
        }
        public string GetDefaultPrinter()
        {
            PrinterSettings settings = new PrinterSettings();
            foreach (string printer in PrinterSettings.InstalledPrinters)
            {
                settings.PrinterName = printer;
                if (settings.IsDefaultPrinter)
                    return printer;
            }
            return string.Empty;
        }

        public string GetDefaultprinterName()
        {
            throw new NotImplementedException();
        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session["rptProductID"] = null;

            Session["rptSubCategoryID"] = null;

            Session["rptCategoryID"] = null;

            Session["rptDepartmentID"] = null;

            Session["rptCustomerID"] = null;

            Session["rptBarterCustomers"] = null;

            // Session["rptSalesDateFrom"] = null;

            //  Session["rptSalesDateTo"] = null;

            Response.Redirect(Session["ReportPrinting_Redirection"].ToString());
        }
    }
}