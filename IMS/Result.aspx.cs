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
    public partial class Result : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
       
        string clientName;
        protected void Page_Load(object sender, EventArgs e)
        {
            clientName = Request["search"].ToString();
            Getresult();

        }

         


        private void Getresult()
        {
            DataTable dt = new DataTable();
            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            if (clientName.Length >= 2)
            {
                using (SqlCommand cmd = new SqlCommand("Sp_GetProductByName", connection))
                {

                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@p_prodName", clientName);
                    //SqlDataReader dr = cmd.ExecuteReader();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    StringBuilder sb = new StringBuilder();

                    if (dt.Rows.Count > 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            string val=Server.HtmlDecode(dt.Rows[i].ItemArray[0].ToString());
                            sb.Append(val + "~");   //Create Con "|" + dt.Rows[i].ItemArray[1].ToString() +

                        }
                    }

                    Response.Write(Server.HtmlDecode(sb.ToString()));
                }
                if (dt.Rows.Count <= 0)
                {
                    using (SqlCommand cmd = new SqlCommand("Sp_GetProductByName", connection))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("@p_prodName", clientName);
                       
                        //SqlDataReader dr = cmd.ExecuteReader();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StringBuilder sb = new StringBuilder();

                        if (dt.Rows.Count > 1)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                string val = Server.HtmlDecode(dt.Rows[i].ItemArray[0].ToString());
                                sb.Append(val + "~");   //Create Con "|" + dt.Rows[i].ItemArray[1].ToString() +

                            }
                        }

                        Response.Write(Server.HtmlDecode(sb.ToString()));
                    }
                }
                
            }
            
             
        }
    }
}