using IMSCommon.Util;
using Microsoft.Reporting.WebForms;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS.UserControl
{
    public partial class uc_printBarcode : System.Web.UI.UserControl
    {
        string barcodeVal;
        int printCount;
        string productName;
        string companyName;

        
        protected void Page_Load(object sender, EventArgs e)
        {
        }

        public string ProductName 
        {
            set
            {
                productName = value;
            }
        }


        public string CompanyName
        {

            set { companyName = value; }
        }
        public int PrintCount
        {
            set
            {
                printCount = value;
            }
        }
        public string BarcodeValue
        {
            set
            {
                barcodeVal = value;
                createBarcode();
            }
        }


        private void createBarcode() 
        {

            Ean13 ean13 = new Ean13();
            if (barcodeVal.Length >= 12)
            {
                ean13.CountryCode = barcodeVal.Substring(0, 3);
                ean13.ManufacturerCode = barcodeVal.Substring(3, 5);
                ean13.ProductCode = barcodeVal.Substring(8, 4);

                Bitmap img = new Bitmap(150, 150);

                //System.Drawing.Graphics g = this.picBarcode.CreateGraphics( );
                System.Drawing.Graphics g = Graphics.FromImage(img);
                g.FillRectangle(new System.Drawing.SolidBrush(System.Drawing.SystemColors.Control),
                    new Rectangle(0, 0, img.Width, img.Height));

                
               
                ean13.DrawEan13Barcode(g, new System.Drawing.Point(25, 5),companyName,productName);
               
                Byte[] bytes = imageToByteArray(img);
                string base64String = Convert.ToBase64String(bytes, 0, bytes.Length);
               
                        // picBarcode.Image = bytes;
                
                ReportViewer1.ProcessingMode = Microsoft.Reporting.WebForms.ProcessingMode.Local;
                ReportViewer1.LocalReport.ReportPath = Server.MapPath("~/Report/barcode_Print.rdlc");

               //create new data set to be added
                DataTable dt = new DataTable();
                dt.Columns.Add("barcodeValue", typeof(String));
                dt.Columns.Add("idColumn", typeof(int));

                for (int i = 0; i < printCount; i++)
                {
                    dt.Rows.Add(base64String,i);
                }

                ReportDataSource dataSource = new ReportDataSource("barcodeDisplay", dt);
                ReportViewer1.LocalReport.DataSources.Clear();
                ReportViewer1.LocalReport.DataSources.Add(dataSource);
               // ReportViewer1.Visible = true;
                ReportViewer1.LocalReport.Refresh();
                g.Dispose();
            }
        }

        public byte[] imageToByteArray(System.Drawing.Image imageIn)
        {

            using (var ms = new MemoryStream())
            {
                imageIn.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
                return ms.ToArray();
            }
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
             Response.Redirect("ViewInventory.aspx", false);
        }
        
    }
}