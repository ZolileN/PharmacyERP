using IMS.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class SelectStores : System.Web.UI.Page
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
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

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                String Text = txtStore.Text + '%';
                Session["txtStore"] = Text;
                StoresPopupGrid.PopulateGrid();
                StoresPopupGrid.SessionIDTag = "SystemId";
                StoresPopupGrid.SessionNameTag = "Storename";
                StoresPopupGrid.SourcePage = "SelectStores.aspx";
                StoresPopupGrid.DestinationPage = "AddVendorsToStore.aspx";
                mpeCongratsMessageDiv.Show();
            }
            catch (Exception ex) 
            {
               
                throw ex;
            }
            finally
            {
                
            }
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            string Storename = txtStore.Text;
            Session["Storename"] = Storename;
            Session["SystemId"] = lblStoreId.Text;             
            Response.Redirect("AddVendorsToStore.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("Storename");
            Session.Remove("SystemId");
            Response.Redirect("WarehouseMain.aspx",false);
        }
    }
}