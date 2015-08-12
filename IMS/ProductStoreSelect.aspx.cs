using IMS.Util;
using log4net;
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
    public partial class ProductStoreSelect : System.Web.UI.Page
    {
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack)
            {
                try
                {
                    btnContinue.Visible = false;
                    Session.Remove("dsStoresPopup");
                   // Session.Remove("dsProducts_MP");
                   // BindGrid();

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
        private void BindGrid()
        {
            
            try
            {
                if (connection.State == ConnectionState.Closed) 
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSystem_ByID", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SystemID", DBNull.Value);


                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

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
                {
                    connection.Close();
                }
            }

        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            string storeName = txtStore.Text;
            Session["StoreName"] = storeName;
            Session["StoreID"] = lblStoreId.Text;
            Response.Redirect("Prod2Store.aspx",false);
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtStore.Text + '%';
            Session["txtStore"] = Text;
            StoresPopupGrid.PopulateGrid();
            StoresPopupGrid.SessionIDTag = "StoreID";
            StoresPopupGrid.SessionNameTag = "StoreName";
            StoresPopupGrid.SourcePage = "ProductStoreSelect.aspx";
            StoresPopupGrid.DestinationPage = "Prod2Store.aspx";
            StoresPopupGrid.PopulateGrid();
            mpeCongratsMessageDiv.Show();
            
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session.Remove("StoreName");
            Session.Remove("StoreID");
            Response.Redirect("WarehouseMain.aspx", false);
        }


    }
}