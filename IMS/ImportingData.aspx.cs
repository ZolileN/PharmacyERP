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

namespace IMS
{
    public partial class ImportingData : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet;
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {

            }
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Select * from tblProduct_StockMappingTEMP", connection);
                DataSet MappingTable = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(MappingTable);

                SqlCommand command1 = new SqlCommand("Select tblStock_Detail.StockID, SUBSTRING(CONVERT(varchar(16), tblStock_Detail.BarCode), 1, 8) as ID , tblStock_Detail.BarCode from tblStock_Detail", connection);
                DataSet StockTable = new DataSet();
                SqlDataAdapter dA1 = new SqlDataAdapter(command1);
                dA1.Fill(StockTable);

                for(int i =0; i<MappingTable.Tables[0].Rows.Count;i++)
                {
                    for(int j =0; j<StockTable.Tables[0].Rows.Count;j++)
                    {
                        
                        if (StockTable.Tables[0].Rows[j]["ID"].ToString().Equals(MappingTable.Tables[0].Rows[i]["Product_Id_Org"].ToString()))
                        {
                            String Query = "Update tblStock_Detail SET tblStock_Detail.ProductID ='" + Convert.ToInt32(MappingTable.Tables[0].Rows[i]["ProductID"].ToString()) + "' WHERE BarCode = '" + StockTable.Tables[0].Rows[j]["BarCode"] + "'";
                            SqlCommand command3 = new SqlCommand(Query, connection);
                            command3.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch(Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

    }
}