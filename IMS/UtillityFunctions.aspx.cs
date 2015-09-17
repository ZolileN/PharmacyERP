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
using System.IO;
using log4net;
using IMS.Util;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using VBE = Microsoft.Vbe.Interop.Forms;
using System.ComponentModel;
using IMSCommon.Util;


namespace IMS
{
    public partial class UtillityFunctions : System.Web.UI.Page
    {
        public static string FILE_PATH = "";
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static int lastRow = 0;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

            }
        }

        public void SaleOrderCreation(String InvoiceDate, String SalesRep, String SalesTo, DataTable Products)
        {
            int sOM_ID;
            try
            {
                sOM_ID = CreateSOMaster(InvoiceDate, SalesRep, SalesTo);

                Dictionary<string, QtyDiscountPair> prod_QtyMap = new Dictionary<string, QtyDiscountPair>();
                Dictionary<string, int> prod_DetIDMap = new Dictionary<string, int>();
                Dictionary<string, int> name_IDMap = new Dictionary<string, int>();

                #region map product,qty,discount
                for (int i = 0; i < Products.Rows.Count; i++)
                {
                    string prodName = Products.Rows[i]["ProductName"].ToString();
                    int qty = 0;
                    int.TryParse(Products.Rows[i]["ProductQuantity"].ToString(), out qty);
                    float disc = 0;
                    float.TryParse(Products.Rows[i]["ProductDiscount"].ToString(), out disc);
                    if (!prod_QtyMap.ContainsKey(prodName))
                    {
                        QtyDiscountPair val = new QtyDiscountPair(qty, disc);
                        prod_QtyMap.Add(prodName, val);
                    }
                    else
                    {
                        QtyDiscountPair val = prod_QtyMap[prodName];
                        val.updateQuantity(qty);
                        val.updateDiscount(disc);
                        prod_QtyMap.Add(prodName, val);
                    }
                } 
                #endregion

                #region create so detail 
                foreach (string key in prod_QtyMap.Keys)
                {
                    //passing dictionary as reference to get the product Name-ID map, this is just to save DB calls
                    int soID = CreateSODetail(key, prod_QtyMap[key].Quantity, prod_QtyMap[key].Discount, ref name_IDMap);
                    prod_DetIDMap.Add(key, soID);
                } 
                #endregion

                #region create so entry
                for (int i = 0; i < Products.Rows.Count; i++)
                {
                    int qty = 0;
                    float disc = 0;
                    float ucp = 0;
                    int prodID = 0;
                    int orderDetailID = 0;
                    string prodName = Products.Rows[i]["ProductName"].ToString();
                    int.TryParse(Products.Rows[i]["ProductQuantity"].ToString(), out qty);
                    float.TryParse(Products.Rows[i]["ProductDiscount"].ToString(), out disc);
                    string expiry = Products.Rows[i]["ProductExpiry"].ToString();
                    string batch = Products.Rows[i]["ProductBatch"].ToString();
                    float.TryParse(Products.Rows[i]["ProductUCP"].ToString(), out ucp);

                    if (name_IDMap.ContainsKey(prodName))
                    {
                        prodID = name_IDMap[prodName];
                        if (prod_DetIDMap.ContainsKey(prodName))
                        {
                            orderDetailID = prod_DetIDMap[prodName];
                            CreateSOEntry(prodID, orderDetailID, qty, disc, batch, ucp, expiry);
                        }
                    }

                } 
                #endregion
                
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

        private void CreateSOEntry(int productID,int orderDetailID,int qty,float Discount,string batch,float CP,string expiry) 
        {
            try 
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

             
                SqlCommand command = new SqlCommand("sp_EntrySaleOrderDetails", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_OrderDetailID", orderDetailID);
                command.Parameters.AddWithValue("@p_ProductID",productID);
                command.Parameters.AddWithValue("@p_StockID", DBNull.Value);
                command.Parameters.AddWithValue("@p_CostPrice", CP);
                command.Parameters.AddWithValue("@p_SalePrice", DBNull.Value);
                command.Parameters.AddWithValue("@p_Expiry",expiry);
                command.Parameters.AddWithValue("@p_Batch", batch);
                command.Parameters.AddWithValue("@p_SendQuantity", qty);
                command.Parameters.AddWithValue("@p_BonusQuantity", DBNull.Value);
                command.Parameters.AddWithValue("@p_BarCode", DBNull.Value);
                command.Parameters.AddWithValue("@p_Discount", Discount);
                command.ExecuteNonQuery();
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
        private int CreateSODetail(string prodName, int quantity, float discount,ref Dictionary<string,int> name_IDMap )
        {
            int productID = 0;
            int sOD_ID = 0;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region fetch productID from name

                SqlCommand command = new SqlCommand("Sp_GetProductByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_prodName", prodName);
                DataSet ds = new DataSet();
                SqlDataAdapter sA1 = new SqlDataAdapter(command);
                sA1.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out productID);
                    if (name_IDMap.ContainsKey(prodName))
                    {
                        name_IDMap[prodName] = productID;
                    }
                    else
                    {
                        name_IDMap.Add(prodName, productID);
                    }
                    //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                }

                #endregion

                #region Order detail Creation
                if (productID != 0)
                {
                    command = new SqlCommand("sp_InserOrderDetail_ByOutStore", connection);
                    command.CommandType = CommandType.StoredProcedure;


                    int OrderNumber, BonusOrdered, ProductNumber, Quantity;
                    OrderNumber = BonusOrdered = ProductNumber = Quantity = 0;

                    if (int.TryParse(Session["OrderNumberSO"].ToString(), out OrderNumber))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                    }
                    command.Parameters.AddWithValue("@p_ProductID", productID);


                    command.Parameters.AddWithValue("@p_OrderQuantity", Quantity);



                    command.Parameters.AddWithValue("@p_BonusQuantity", DBNull.Value);


                    command.Parameters.AddWithValue("@p_status", "Pending");
                    command.Parameters.AddWithValue("@p_comments", "Generated to Outside Store");


                    command.Parameters.AddWithValue("@p_Discount", discount);


                    DataSet LinkResult = new DataSet();
                    SqlDataAdapter sA = new SqlDataAdapter(command);
                    sA.Fill(LinkResult);
                    if (LinkResult != null && LinkResult.Tables[0] != null)
                    {
                        int.TryParse(LinkResult.Tables[0].Rows[0][0].ToString(), out sOD_ID);
                        //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                    }

                }
                return sOD_ID;
                #endregion
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
        private int CreateSOMaster(String InvoiceDate, String SalesRep, String SalesTo)
        {

            #region Creating Order
            int sOM_ID = 0;
            int pRequestFrom = 0;
            int pRequestTo = 0;
            String OrderMode = "";
            int OrderType = 3;//incase of vendor this should be 3

            OrderMode = "Sales";

            // String Invoice = txtIvnoice.Text;
            String Vendor = "True";
            int Salesman = 0;

            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }

                #region fetch salesman ID from name

                SqlCommand command = new SqlCommand("Sp_GetUser_ByName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_userName", SalesRep);
                DataSet ds = new DataSet();
                SqlDataAdapter sA1 = new SqlDataAdapter(command);
                sA1.Fill(ds);
                if (ds != null && ds.Tables[0] != null)
                {
                    int.TryParse(ds.Tables[0].Rows[0][0].ToString(), out Salesman);
                    
                    //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                }

                #endregion

                #region fetch system name
                command = new SqlCommand("Sp_GetSystem_byName", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_storeName", SalesTo);
                DataSet ds1 = new DataSet();
                SqlDataAdapter sA12 = new SqlDataAdapter(command);
                sA12.Fill(ds1);
                if (ds != null && ds1.Tables[0] != null)
                {
                    int.TryParse(ds1.Tables[0].Rows[0][4].ToString(), out pRequestTo);

                    //Session["DetailID"] = LinkResult.Tables[0].Rows[0][0];
                }
                #endregion

                command = new SqlCommand("sp_CreateSO_Previous", connection);
                command.CommandType = CommandType.StoredProcedure;
                //sets vendor
                if (pRequestTo > 0)
                {
                    command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                }
                else 
                {
                    command.Parameters.AddWithValue("@p_RequestTO", DBNull.Value);
                }
                //sets warehouse/store
                if (int.TryParse(Session["UserSys"].ToString(), out pRequestFrom))
                {
                    command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                }
                int userID = Convert.ToInt32(Session["UserID"].ToString());

                command.Parameters.AddWithValue("@p_OrderType", OrderType);
                command.Parameters.AddWithValue("@p_Invoice", DBNull.Value);
                command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                command.Parameters.AddWithValue("@p_Vendor", Vendor);
                command.Parameters.AddWithValue("@p_userID", userID);
                command.Parameters.AddWithValue("@p_dateCreated", DateTime.ParseExact(InvoiceDate, "dd/MM/yyyy", null));
                if (Salesman>0)
                {
                    command.Parameters.AddWithValue("@p_Salesman", Salesman);
                }
                else
                {
                    command.Parameters.AddWithValue("@p_Salesman", DBNull.Value);
                }
                command.Parameters.AddWithValue("@p_orderStatus", "Pending");
                command.Parameters.AddWithValue("@p_isCreatedSO", "false");
                DataTable dt = new DataTable();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(dt);
                if (dt.Rows.Count != 0)
                {
                    int.TryParse(dt.Rows[0][0].ToString(), out sOM_ID);
                }
                return sOM_ID;
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
        public void ReadingExcelFile(String file)
        {
            try
            {
                Excel.Range xlrange;
                Excel.Shapes shapes;
                String TextValue = "";
                String InvoiceNumber = "";
                String InvoiceDate = "";
                String SalesRep = "";
                String SalesTo = "";

                MyApp = new Excel.Application();
                MyApp.Visible = false;
                lastRow = 22;// MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
                MyBook = MyApp.Workbooks.Open(file);

                int Sheets = MyBook.Sheets.Count;

                if (Sheets > 1)
                {
                    MySheet = (Excel.Worksheet)MyBook.Sheets[2];

                    shapes = (Excel.Shapes)MySheet.Shapes;
                    xlrange = MySheet.UsedRange;

                    foreach (Excel.Shape shape in shapes)
                    {
                        if (shape.Name == "TextBox 9")
                        {
                            var value = shape.OLEFormat.Object;
                            TextValue = value.Caption.ToString();
                            break;
                        }
                    }

                    String[] Values = TextValue.Split('\n');
                    String[] InvoiceValues = Values[1].Split('.');
                    String[] InvDateValues = Values[3].Split(':');
                    String[] SalesRepValues = Values[7].Split(':');

                    SalesTo = MySheet.Cells[18, 2].Value.ToString();

                    InvoiceNumber = InvoiceValues[1].Trim();
                    InvoiceDate = InvDateValues[1].Trim();
                    SalesRep = SalesRepValues[1].Trim();
                }
                else
                {
                    MySheet = (Excel.Worksheet)MyBook.Sheets[1];
                    xlrange = MySheet.UsedRange;
                    String InvoiceValues = MySheet.Cells[15, 1].Value.ToString();
                    String InvoiceDateValues = MySheet.Cells[15, 5].Value.ToString();
                    String SalesRepValues = MySheet.Cells[16, 5].Value.ToString();
                    String SalesToValues = MySheet.Cells[17, 1].Value.ToString();

                    String[] Invoice = InvoiceValues.Split(':');
                    String[] SalesRepresent = SalesToValues.Split(':');

                    InvoiceNumber = Invoice[1].Trim();
                    InvoiceDate = InvoiceDateValues.Trim();
                    SalesRep = SalesRepValues.Trim();
                    SalesTo = SalesRepresent[1].Trim();
                    SalesTo = SalesTo.Replace(System.Environment.NewLine, string.Empty);

                    String[] SalesOrderTo = SalesTo.Split('\n');
                    SalesTo = SalesOrderTo[0].Trim();
                    // SalesTo = SalesTo.Replace(System.Environment.NewLine, string.Empty);
                }

                List<String> ProductName = new List<string>();
                List<String> ProductExpiry = new List<string>();
                List<String> ProductBatch = new List<string>();
                List<String> ProductQuantity = new List<string>();
                List<String> ProductUCP = new List<string>();
                List<String> ProductDiscount = new List<string>();
                int j = 0;
                for (int i = 2; i <= xlrange.Count + 1; i++)
                {

                    if (MySheet.Cells[lastRow, 2].Value != null && lastRow <= 49)
                    {
                        ProductName.Add(MySheet.Cells[lastRow, 2].Value.ToString());
                        ProductExpiry.Add(MySheet.Cells[lastRow, 3].Value.ToString());
                        ProductBatch.Add(MySheet.Cells[lastRow, 4].Value.ToString());
                        ProductQuantity.Add(MySheet.Cells[lastRow, 5].Value.ToString());
                        ProductUCP.Add(MySheet.Cells[lastRow, 6].Value.ToString());

                        if (MySheet.Cells[lastRow, 7].Value != null)
                        {
                            ProductDiscount.Add(MySheet.Cells[lastRow, 7].Value.ToString());
                        }
                        else
                        {
                            ProductDiscount.Add("");
                        }
                        lastRow++;
                        j++;
                    }
                    else
                    {
                        break;
                    }
                }

                lastRow = 22;

                DataTable dt = new DataTable();
                dt.Columns.Add("ProductName", typeof(String));
                dt.Columns.Add("ProductExpiry", typeof(String));
                dt.Columns.Add("ProductBatch", typeof(String));
                dt.Columns.Add("ProductQuantity", typeof(String));
                dt.Columns.Add("ProductUCP", typeof(String));
                dt.Columns.Add("ProductDiscount", typeof(String));

                for (int i = 0; i < ProductName.Count; i++)
                {
                    DataRow dr = dt.NewRow();
                    dr["ProductName"] = ProductName[i];
                    dr["ProductExpiry"] = ProductExpiry[i];
                    dr["ProductBatch"] = ProductBatch[i];
                    dr["ProductQuantity"] = ProductQuantity[i];
                    dr["ProductUCP"] = ProductUCP[i];
                    dr["ProductDiscount"] = ProductDiscount[i];
                    dt.Rows.Add(dr);
                    dt.AcceptChanges();
                }

                #region Check Invoice Number
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("sp_VerifyBillNo", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Invoice", InvoiceNumber);


                DataSet ds = new DataSet();
                SqlDataAdapter sA = new SqlDataAdapter(command);
                sA.Fill(ds);
                #endregion

                if (ds != null && ds.Tables[0] != null && ds.Tables[0].Rows.Count > 0)
                {
                    SaleOrderCreation(InvoiceDate, SalesRep, SalesTo, dt);
                }

            }
            catch (Exception ex)
            {

            }

        }


        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir, "*.xlsx", SearchOption.AllDirectories))
                {
                    files.Add(f);
                }
                /*foreach (string d in Directory.GetDirectories(sDir))
                {
                    files.AddRange(DirSearch(d));
                }*/
            }
            catch (System.Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }

            return files;
        }
        protected void btnImportManualSO_Click(object sender, EventArgs e)
        {
            List<String> Files = DirSearch(@"D:\MANUALINVOICE\MANUALINVOICE");
            //Files = new List<string>();
            //Files.Add(@"G:\10-05-2015-INV-344.xlsx");
            List<String> ActualInvoices = new List<String>();
            List<String> BonusInvoices = new List<String>();

            foreach (string f in Files)
            {
                String fileName = Path.GetFileName(f);
                if (fileName.Contains('B') || fileName.Contains('b'))
                {
                    BonusInvoices.Add(f);
                }
                else
                {
                    ActualInvoices.Add(f);
                }
            }

            foreach (string f in ActualInvoices)
            {
                ReadingExcelFile(f);
            }
        }

        public void AdjustmentPO_Acceptence(int orderDetID, int recQuan, int bonusQuan, float CP, float SP, int OrderMasteriD, int ProductID, DateTime Expiry)
        {
            #region Query execution

            if (connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("Sp_StockReceiving", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@p_OrderDetailID", orderDetID);
            command.Parameters.AddWithValue("@p_ReceivedQuantity", recQuan);
            command.Parameters.AddWithValue("@p_ExpiredQuantity", DBNull.Value);
            command.Parameters.AddWithValue("@p_DefectedQuantity", DBNull.Value);
            command.Parameters.AddWithValue("@p_ReturnedQuantity", DBNull.Value);

            command.Parameters.AddWithValue("@p_BarCode", DBNull.Value);
            command.Parameters.AddWithValue("@p_DiscountPercentage", DBNull.Value);
            command.Parameters.AddWithValue("@p_Bonus", bonusQuan);
            command.Parameters.AddWithValue("@p_BatchNumber", DBNull.Value);
            command.Parameters.AddWithValue("@p_Expiry", Expiry);
            
            command.Parameters.AddWithValue("@p_Cost", CP);
            command.Parameters.AddWithValue("@p_Sales", SP);

            command.Parameters.AddWithValue("@p_BonusTotal", bonusQuan);// total bonus added
            command.Parameters.AddWithValue("@p_RemainingQuantity", 0);
            command.Parameters.AddWithValue("@p_SystemType", DBNull.Value);
            command.Parameters.AddWithValue("@p_StoreID", Convert.ToInt32(Session["UserSys"].ToString()));
            command.Parameters.AddWithValue("@p_orderMasterID", OrderMasteriD);
            command.Parameters.AddWithValue("@p_isInternal", "TRUE");
            command.Parameters.AddWithValue("@p_isPO", 1);
            command.Parameters.AddWithValue("@p_ProductID", ProductID);

            command.Parameters.AddWithValue("@p_expiryOriginal", Expiry);
            command.Parameters.AddWithValue("@p_comments", "Completed");
            
            command.ExecuteNonQuery();
            #endregion
        }
        protected void AdjumentPO(int ProductID, int Quantity, int Bonus)
        {
            int quan, bQuan;
            quan = Quantity;
            bQuan = Bonus;
            if (quan + bQuan > 0)
            {
                if (Session["FirstOrder"].Equals(false))
                {
                    #region Creating Order

                    int pRequestFrom = 1;
                    int pRequestTo = 333;
                    String OrderMode = "";
                    int OrderType = 3;//incase of vendor this should be 3

                    OrderMode = "Vendor";
                    

                    String Invoice = "";
                    String Vendor = "True";


                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_CreateOrder", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        //sets vendor

                        command.Parameters.AddWithValue("@p_RequestTO", pRequestTo);
                        

                        if (int.TryParse(Session["UserSys"].ToString(), out pRequestFrom))
                        {
                            command.Parameters.AddWithValue("@p_RequestFrom", pRequestFrom);
                        }

                        command.Parameters.AddWithValue("@p_OrderType", OrderType);
                        command.Parameters.AddWithValue("@p_Invoice", Invoice);
                        command.Parameters.AddWithValue("@p_OrderMode", OrderMode);
                        command.Parameters.AddWithValue("@p_Vendor", Vendor);
                        command.Parameters.AddWithValue("@p_orderStatus", "Pending");
                        DataTable dt = new DataTable();
                        SqlDataAdapter dA = new SqlDataAdapter(command);
                        dA.Fill(dt);
                        if (dt.Rows.Count != 0)
                        {
                            Session["OrderNumber"] = dt.Rows[0][0].ToString();
                        }
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                    #endregion

                    #region Linking to Order Detail table

                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        string ProductNumber = "";
                        int OrderNumber, BonusOrdered;
                        OrderNumber = BonusOrdered = 0;

                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        }
                        command.Parameters.AddWithValue("@p_ProductID", ProductID);
                        command.Parameters.AddWithValue("@p_OrderQuantity", quan);
                        command.Parameters.AddWithValue("@p_OrderBonusQuantity", bQuan);
                        command.Parameters.AddWithValue("@p_status", "Pending");
                        command.Parameters.AddWithValue("@p_comments", "Generated to Vendor");
                        command.ExecuteNonQuery();
                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }
                    #endregion

                    Session["FirstOrder"] = true;
                }
                else
                {
                    #region Product Existing in the Current Order
                    DataSet ds = new DataSet();
                    try
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("sp_GetOrderbyVendor", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        int OrderNumber = 0;


                        if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                        {
                            command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                            SqlDataAdapter sA = new SqlDataAdapter(command);
                            sA.Fill(ds);

                            int ProductNO = 0;
                            bool ProductPresent = false;
                            command.Parameters.AddWithValue("@p_ProductID", ProductID);
                            

                            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                            {
                                if (Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"]).Equals(ProductNO))
                                {
                                    ProductPresent = true;
                                    break;
                                }
                            }

                            if (ProductPresent.Equals(false))
                            {
                                #region Linking to Order Detail table

                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    command = new SqlCommand("sp_InserOrderDetail_ByStore", connection);
                                    command.CommandType = CommandType.StoredProcedure;


                                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                                    {
                                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                                    }
                                    command.Parameters.AddWithValue("@p_ProductID", ProductID);
                                    command.Parameters.AddWithValue("@p_OrderQuantity", quan);
                                    command.Parameters.AddWithValue("@p_OrderBonusQuantity", bQuan);
                                    command.Parameters.AddWithValue("@p_status", "Pending");
                                    command.Parameters.AddWithValue("@p_comments", "Generated to Vendor");
                                    command.ExecuteNonQuery();
                                }
                                catch (Exception ex)
                                {

                                }
                                finally
                                {
                                    connection.Close();
                                }
                                #endregion
                            }
                            else
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record can not be inserted, because it is already present')", true);
                            }
                        }

                    }
                    catch (Exception ex)
                    {

                    }
                    finally
                    {
                        connection.Close();
                    }


                    #endregion

                }

                #region Populate Product Info
                try
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("Sp_FillPO_Details", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    int OrderNumber = 0;
                    command.Parameters.AddWithValue("@p_OrderDetailID", DBNull.Value);
                    if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                    {
                        command.Parameters.AddWithValue("@p_OrderID", OrderNumber);
                        command.ExecuteNonQuery();
                    }


                }
                catch (Exception ex)
                {

                }
                finally
                {
                    connection.Close();
                }
                #endregion

            }
            else
            {
                WebMessageBoxUtil.Show("Both quantity and bonus quantity cannot be 0");
                return;
            }
        }
        protected void btnImPortManualPO_Click(object sender, EventArgs e)
        {
           try
           {
               connection.Open();
               SqlCommand command = new SqlCommand("sp_SelectUnmappedSO", connection);
               command.CommandType = CommandType.StoredProcedure;

               DataSet ds = new DataSet();
               SqlDataAdapter sdA = new SqlDataAdapter(command);
               sdA.Fill(ds);

               Session["FirstOrder"] = false;

               for(int i=0;i<ds.Tables[0].Rows.Count;i++)
               {
                   int ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"].ToString());
                   int QuantitySold = Convert.ToInt32(ds.Tables[0].Rows[i]["QuantitySold"].ToString());
                   int BonusQauntityGiven = Convert.ToInt32(ds.Tables[0].Rows[i]["BonusQauntityGiven"].ToString());

                   AdjumentPO(ProductID, QuantitySold, BonusQauntityGiven);
               }
           }
           catch(Exception ex)
           {

           }
        }

        protected void btnAcceptAdjustmentPO_Click(object sender, EventArgs e)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GetAdjustmentPO_Entries", connection);
                command.CommandType = CommandType.StoredProcedure;

                DataSet ds = new DataSet();
                SqlDataAdapter sdA = new SqlDataAdapter(command);
                sdA.Fill(ds);

                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                    int ProductID = Convert.ToInt32(ds.Tables[0].Rows[i]["ProductID"].ToString());
                    int QuantitySold = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderedQuantity"].ToString());
                    int BonusQauntityGiven = Convert.ToInt32(ds.Tables[0].Rows[i]["BonusQuantity"].ToString());

                    float CP = float.Parse(ds.Tables[0].Rows[i]["OrderedQuantity"].ToString());
                    float SP = float.Parse(ds.Tables[0].Rows[i]["BonusQuantity"].ToString());

                    int orderDetailID = Convert.ToInt32(ds.Tables[0].Rows[i]["orderDetailID"].ToString());
                    int orderMasterID = Convert.ToInt32(ds.Tables[0].Rows[i]["OrderID"].ToString());

                    DateTime Expiry = DateTime.Parse(ds.Tables[0].Rows[i]["ExpiryDate"].ToString());

                    AdjustmentPO_Acceptence(orderDetailID, QuantitySold, BonusQauntityGiven, CP, SP, orderMasterID, ProductID, Expiry);
                }
            }
            catch (Exception ex)
            {

            }
        }
    }

    /// <summary>
    /// internal class that holds quantity and discount against a product
    /// </summary>
    internal class QtyDiscountPair
    {
        private int _quantity = 0;
        private float _discount;
        public int Quantity
        {
            get { return _quantity; }
        }


        public float Discount
        {
            get { return _discount; }

        }

        internal QtyDiscountPair(int qty, float disc)
        {
            _quantity = qty;
            _discount = disc;
        }

        internal void updateQuantity(int val)
        {
            _quantity += val;
        }

        internal void updateDiscount(float val)
        {
            _discount = val;
        }
    }
}