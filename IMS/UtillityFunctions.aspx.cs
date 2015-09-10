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
            if(!IsPostBack)
            {

            }
        }

        public void SaleOrderCreation(String InvoiceDate, String SalesRep, String SalesTo, DataTable Products)
        {
            
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
                    
                    if (MySheet.Cells[lastRow, 2].Value != null && lastRow <=49)
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

               for(int i=0;i<ProductName.Count;i++)
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
            catch(Exception ex)
            {

            }
               
        }

       
        private List<String> DirSearch(string sDir)
        {
            List<String> files = new List<String>();
            try
            {
                foreach (string f in Directory.GetFiles(sDir,"*.xlsx",SearchOption.AllDirectories))
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
            List<String> Files = DirSearch(@"G:\MANUALINVOICE\MANUALINVOICE");
            //Files = new List<string>();
            //Files.Add(@"G:\10-05-2015-INV-344.xlsx");
            List<String> ActualInvoices = new List<String>();
            List<String> BonusInvoices = new List<String>();

            foreach(string f in Files)
            {
               String fileName = Path.GetFileName(f);
               if(fileName.Contains('B') || fileName.Contains('b'))
               {
                   BonusInvoices.Add(f);
               }
               else
               {
                   ActualInvoices.Add(f);
               }
            }

            foreach(string f in ActualInvoices)
            {
                ReadingExcelFile(f);
            }
        }
    }
}