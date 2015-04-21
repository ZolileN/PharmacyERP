using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.OleDb;
using Excel = Microsoft.Office.Interop.Excel;
using System.ComponentModel;
using System.Data;
using System.IO;


namespace IMS
{
    public static class MyExcel
    {
        public  static string FILE_PATH = "";
        private static Excel.Workbook MyBook = null;
        private static Excel.Application MyApp = null;
        private static Excel.Worksheet MySheet = null;
        private static int lastRow = 0;

        static MyExcel()
        {
            InitializeExcel();
        }
        public static void InitializeExcel()
        {
            MyApp = new Excel.Application();
            MyApp.Visible = false;            
            lastRow = 22;// MySheet.Cells.SpecialCells(Excel.XlCellType.xlCellTypeLastCell).Row;
        }
        #region Unused Code
        //public static BindingList<Employee> ReadMyExcel()
        //{
        //    EmpList.Clear();
        //    for (int index = 2; index <= lastRow; index++)
        //    {
        //        System.Array MyValues = (System.Array)MySheet.get_Range("A" + index.ToString(), "D" + index.ToString()).Cells.Value;
        //        EmpList.Add(new Employee
        //        {
        //            Name = MyValues.GetValue(1, 1).ToString(),
        //            Employee_ID = MyValues.GetValue(1, 2).ToString(),
        //            Email_ID = MyValues.GetValue(1, 3).ToString(),
        //            Number = MyValues.GetValue(1, 4).ToString()
        //        });
        //    }
        //    return EmpList;
        //}
        //public static void WriteToExcel(Employee emp)
        //{
        //    try
        //    {
        //        lastRow += 1;
        //        MySheet.Cells[lastRow, 1] = emp.Name;
        //        MySheet.Cells[lastRow, 2] = emp.Employee_ID;
        //        MySheet.Cells[lastRow, 3] = emp.Email_ID;
        //        MySheet.Cells[lastRow, 4] = emp.Number;
        //        EmpList.Add(emp);
        //        MyBook.Save();
        //    }
        //    catch (Exception ex)
        //    { }

        //}

        //public static List<Employee> FilterEmpList(string searchValue, string searchExpr)
        //{
        //    List<Employee> FilteredList = new List<Employee>();
        //    switch (searchValue.ToUpper())
        //    {
        //        case "NAME":
        //            FilteredList = EmpList.ToList().FindAll(emp => emp.Name.ToLower().Contains(searchExpr));
        //            break;
        //        case "MOBILE_NO":
        //            FilteredList = EmpList.ToList().FindAll(emp => emp.Number.ToLower().Contains(searchExpr));
        //            break;
        //        case "EMPLOYEE_ID":
        //            FilteredList = EmpList.ToList().FindAll(emp => emp.Employee_ID.ToLower().Contains(searchExpr));
        //            break;
        //        case "EMAIL_ID":
        //            FilteredList = EmpList.ToList().FindAll(emp => emp.Email_ID.ToLower().Contains(searchExpr));
        //            break;
        //        default:
        //            break;
        //    }
        //    return FilteredList;
        //}
        #endregion
        public static void CloseExcel()
        {
            MyBook.Saved = true;
            MyApp.Quit();
        }

        public static string WriteExcelWithSalesOrderInfo(string salesOrderNo, string salesOrderDate, string salesOrderBillTo, DataSet dataset, String FilePath)
        {
            string filePath = FILE_PATH + "SalesOrder_" + salesOrderNo + ".xlsx";
            string fileName = "SalesOrder_" + DateTime.Now.ToString("dd_MM_yyyy_hh_mm_ss") + ".xlsx";
            MyApp = new Excel.Application();
            MyApp.Visible = false;
            lastRow = 22;
            MyBook = MyApp.Workbooks.Open(FILE_PATH + "SalesOrder.xlsx");
            MySheet = (Excel.Worksheet)MyBook.Sheets[1]; // Explict cast is not required here
            try
            {
                MySheet.Cells[15, 2] = salesOrderNo;
                MySheet.Cells[15, 5] = salesOrderDate;
                MySheet.Cells[17, 1] = "Bill To : " + salesOrderBillTo;
                int SerialNo = 1;
                foreach (DataRow item in dataset.Tables[0].Rows)
                {
                    MySheet.Cells[lastRow, 1] = SerialNo;//SerialNo
                    SerialNo++;

                    DateTime dtTIme = DateTime.MinValue;

                    DateTime.TryParse(item["ExpiryDate"].ToString(), out dtTIme);
                    
                    MySheet.Cells[lastRow, 2] = item["Description"].ToString();//ITEM DESCRIPTION
                    MySheet.Cells[lastRow, 3] = dtTIme.ToShortDateString();//Expiry Date
                    MySheet.Cells[lastRow, 4] = item["BatchNumber"].ToString();//batch No
                    MySheet.Cells[lastRow, 5] = item["SendQuantity"].ToString();//Qty
                    MySheet.Cells[lastRow, 6] = item["SalePrice"].ToString();//Net price
                    MySheet.Cells[lastRow, 7] = item["DiscountPercentage"].ToString();//Discount
                    MySheet.Cells[lastRow, 8] = item["Amount"].ToString();//Amount
                    lastRow += 1;
                }
                MyBook.SaveAs(filePath);
                CloseExcel();
            }
            catch (Exception ex)
            { //throw ex;
            }

            return FILE_PATH + ";" + fileName; 
        }
    }
}