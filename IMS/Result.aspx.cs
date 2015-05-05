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
                using (SqlCommand cmd = new SqlCommand("select DISTINCT Product_Name from tbl_ProductMaster where Description LIKE  @Client_Name+'%'", connection))
                {

                    SqlParameter parClientName = new SqlParameter("@Client_Name", SqlDbType.VarChar);
                    parClientName.Value = clientName;
                    cmd.Parameters.Add(parClientName);
                    //SqlDataReader dr = cmd.ExecuteReader();

                    SqlDataAdapter da = new SqlDataAdapter(cmd);
                    da.Fill(dt);
                    StringBuilder sb = new StringBuilder();

                    if (dt.Rows.Count > 1)
                    {
                        for (int i = 0; i < dt.Rows.Count; i++)
                        {
                            sb.Append(dt.Rows[i].ItemArray[0].ToString() + "~");   //Create Con

                        }
                    }

                    Response.Write(sb.ToString());
                }
                if (dt.Rows.Count <= 0)
                {
                    using (SqlCommand cmd = new SqlCommand("select DISTINCT Product_Name from tbl_ProductMaster where Product_Name LIKE  @Client_Name+'%'", connection))
                    {

                        SqlParameter parClientName = new SqlParameter("@Client_Name", SqlDbType.VarChar);
                        parClientName.Value = clientName;
                        cmd.Parameters.Add(parClientName);
                        //SqlDataReader dr = cmd.ExecuteReader();

                        SqlDataAdapter da = new SqlDataAdapter(cmd);
                        da.Fill(dt);
                        StringBuilder sb = new StringBuilder();

                        if (dt.Rows.Count > 1)
                        {
                            for (int i = 0; i < dt.Rows.Count; i++)
                            {
                                sb.Append(dt.Rows[i].ItemArray[0].ToString() + "~");   //Create Con

                            }
                        }

                        Response.Write(sb.ToString());
                    }
                }
                
            }
            
             
        }
    }
}