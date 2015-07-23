using AjaxControlToolkit;
using CrystalDecisions.CrystalReports.Engine;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
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
        protected void Page_Load(object sender, EventArgs e)
        {
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
                throw;
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