
using IMS.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class PharmacyNameResult : System.Web.UI.Page
    {
        
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        string clientName;
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            clientName = Request["search"].ToString();
            Getresult();
            
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

        private void Getresult()
        {
            try
            {
                DataTable dt = new DataTable();
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                if (clientName.Length >= 2)
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_GetSystem_byName", connection))
                    {

                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_storeName", this.clientName);
                        //SqlDataReader dr = cmd.ExecuteReader();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StringBuilder sb = new StringBuilder();

                        if (dt.Rows.Count > 1)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sb.Append(dt.Rows[i].ItemArray[0].ToString() + "~");   //Create Con "|" + dt.Rows[i].ItemArray[1].ToString() +

                            }
                        }

                        Response.Write(sb.ToString());
                    }


                }

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }
    
    }
}