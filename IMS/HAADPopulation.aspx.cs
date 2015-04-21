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
using System.Data.OleDb;
using System.IO;

namespace IMS
{
    public partial class HAADPopulation : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
               
            }
        }
        public  void HaadLIST(DataTable dt)
        {
            //StreamReader srW = new StreamReader(@"G:\HaadDrug.csv");
           // String[] Values = new String[10];
           // while (srW.ReadLine() != null)
           // {
            int x = 0;
            string errorMessage = "";
            DataTable dtGreenRains = new DataTable();
            DataTable HaadList = new DataTable();
            //try
            //{
            //    connection.Open();
            //    String QUery = "Select ItemCode From tbl_ProductMaster";
            //    SqlCommand command = new SqlCommand(QUery, connection);
                
            //    SqlDataAdapter dA = new SqlDataAdapter(command);
            //    dA.Fill(dtGreenRains);
            //}
            //catch(Exception ex)
            //{

            //}
            //finally
            //{
            //    connection.Close();
            //}
            //List<int> deleteList = new List<int>(); 
            ////dt.Columns.Add("isDeleted", typeof(String));
            //HaadList = dt;
            // for (int j = 0; j < dtGreenRains.Rows.Count; j++)
            //{
            //    for (int k = 0; k < dt.Rows.Count; k++)
            //    {
            //        if (dtGreenRains.Rows[j][0].ToString().Equals(dt.Rows[k][0].ToString()))
            //        {
            //            //DataRow dr  = dt.Rows[]
            //            deleteList.Add(k);
            //            break;
            //        }
            //    }
            //}

            // foreach (int key in deleteList)
            // {
            //     HaadList.Rows[key].Delete();
            //     HaadList.AcceptChanges();
            // }

               
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    try
                    {
                        String Status = dt.Rows[i]["Status"].ToString();

                        if (Status.ToLower().Equals("active"))
                        {
                            String GreenRain = dt.Rows[i]["Greenrain Code"].ToString();
                            String ProductName = dt.Rows[i]["Package Name"].ToString();
                            String GenericName = dt.Rows[i]["Generic Name"].ToString();
                            String Strength = dt.Rows[i]["Strength"].ToString();
                            String Form = dt.Rows[i]["Dosage Form"].ToString();
                            String PackSize = dt.Rows[i]["Package Size"].ToString();
                            String UnitSalePrice = dt.Rows[i]["Package Price to Public"].ToString();
                            String UnitCostPrice = dt.Rows[i]["Package Price to Pharmacy"].ToString();
                            String Manufacturer = dt.Rows[i][15].ToString();
                            String BarCodeSerial = "";

                            #region Populating BarCode Serial
                            if(connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            SqlCommand command2 = new SqlCommand("Select Count(*) From tbl_ProductMasterHaadList Where DrugType = 'MEDICINE(HAAD)' AND ItemCode IS NOT NULL", connection);
                            DataSet ds = new DataSet();
                            SqlDataAdapter sA = new SqlDataAdapter(command2);
                            sA.Fill(ds);

                            if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(7))
                            {
                                BarCodeSerial = "1" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(6))
                            {
                                BarCodeSerial = "10" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(5))
                            {
                                BarCodeSerial = "100" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(4))
                            {
                                BarCodeSerial = "1000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(3))
                            {
                                BarCodeSerial = "10000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }

                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(2))
                            {
                                BarCodeSerial = "100000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }

                            else if (ds.Tables[0].Rows[0][0].ToString().Length.Equals(1))
                            {
                                BarCodeSerial = "1000000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            else if (ds.Tables[0].Rows[0][0].ToString().Length < 1)
                            {
                                BarCodeSerial = "1000000" + (Int32.Parse(ds.Tables[0].Rows[0][0].ToString()) + 1).ToString();
                            }
                            connection.Close();
                            #endregion

                            #region Creation Product


                            if (connection.State == ConnectionState.Open)
                            {
                                connection.Close();
                            }
                            connection.Open();
                            SqlCommand command = new SqlCommand("sp_InsertProductHaad", connection);
                            command.CommandType = CommandType.StoredProcedure;
                            command.Parameters.AddWithValue("@p_BarCodeSerial", BarCodeSerial.ToString());
                            command.Parameters.AddWithValue("@p_ProductCode", GreenRain.ToString());
                            command.Parameters.AddWithValue("@p_ProductName", ProductName.ToString());
                            command.Parameters.AddWithValue("@p_Description", ProductName.ToString());
                            command.Parameters.AddWithValue("@p_BrandName", GenericName.ToString());
                            command.Parameters.AddWithValue("@p_ProductType", "MEDICINE(HAAD)");
                            int res1, res4;
                            float res2, res3, res5;

                            command.Parameters.AddWithValue("@p_SubCategoryID", 0);

                            if (float.TryParse(UnitCostPrice.ToString(), out res2))
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", res2);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_UnitCost", 0);
                            }

                            if (float.TryParse(UnitSalePrice.ToString(), out res3))
                            {
                                command.Parameters.AddWithValue("@p_SP", res3);
                            }
                            else
                            {
                                command.Parameters.AddWithValue("@p_SP", 0);
                            }

                            command.Parameters.AddWithValue("@p_MaxiMumDiscount", 0);

                            command.Parameters.AddWithValue("@p_AWT", 0);



                            command.Parameters.AddWithValue("@p_form", Form.ToString());
                            command.Parameters.AddWithValue("@p_strength", Strength.ToString());
                            command.Parameters.AddWithValue("@p_packtype", "0");
                            command.Parameters.AddWithValue("@p_packsize", PackSize.ToString());

                            command.Parameters.AddWithValue("@p_shelf", "0");
                            command.Parameters.AddWithValue("@p_rack", "0");
                            command.Parameters.AddWithValue("@p_bin", "0");

                            x = command.ExecuteNonQuery();
                            #endregion
                        }
                    }
                    catch (Exception ex)
                    {
                        //errorMessage = ex.Message;
                    }
                    finally
                    {
                        //connection.Close();
                    }

                    //if (x == 1)
                    //{
                    //    WebMessageBoxUtil.Show("Record Inserted Successfully");
                    //}
                    //else
                    //{
                    //    WebMessageBoxUtil.Show(errorMessage);
                    //}

                }

            //}

            
        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            //HaadLIST();
            string FilePath = "";
            string[] a = new string[1];
            string fileName = "";
            string FullName = "";

            DataTable dt = null;
            DataSet ds = null;
            if (HaadFileImport.FileName.Length > 0)
            {
                a = HaadFileImport.FileName.Split('.');
                //fileName = Convert.ToString(System.DateTime.Now.Ticks) + "." + a.GetValue(1).ToString();
                //FilePath = Server.MapPath(@"~\APIExcelSheet");
                FilePath = System.IO.Path.GetDirectoryName(HaadFileImport.FileName);
               // HaadFileImport.SaveAs(FilePath + @"\" + fileName);

                FullName = FilePath + @"\" + fileName;
                
                // Database Saved Code
                string connString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties='Excel 12.0;HDR=yes'", @"G:\HaadList.xlsx");
                string sql = "SELECT * from [Sheet1$]";
                dt = new DataTable();



                using (OleDbConnection conn = new OleDbConnection(connString))
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(sql, conn))
                    {
                        using (OleDbDataReader rdr = cmd.ExecuteReader())
                        {
                            dt.Load(rdr);
                            //return dt;
                        }
                    }
                }
            }
            HaadLIST(dt);
            WebMessageBoxUtil.Show("Total Rows Read: " + dt.Rows.Count);
            //gvAPI.DataSource = dt;
            //gvAPI.DataBind();
            
        }
    }
}