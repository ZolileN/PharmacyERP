using System;
using System.Collections.Generic;
using System.Drawing;

using System.Drawing.Printing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace IMSCommon.Util
{
    public class BarcodeUtility
    {
        public BarcodeUtility() { }

        public void Print(PrintDocument printDoc)
        {
            PrintDialog pd1 = new PrintDialog();
            pd1.Document = printDoc;

            if (pd1.ShowDialog() == DialogResult.OK)
            {
                pd1.Document.Print();
            }

            //System.Drawing.Printing.PrintDocument pd1 = new System.Drawing.Printing.PrintDocument();
            //pd1.PrintPage += new System.Drawing.Printing.PrintPageEventHandler(this.pd_PrintPage);
            //pd1.Print();
        }
        public void Print(Image i)
        {
            PrintDialog pdialogue = new PrintDialog();
            if (pdialogue.ShowDialog() == DialogResult.OK)
            {
                PrintDocument pd = new PrintDocument();
                pd.DefaultPageSettings.PrinterSettings.PrinterName = pdialogue.PrinterSettings.PrinterName;
                pd.DefaultPageSettings.Landscape = true; //or false!

                pd.PrintPage += (sender, args) =>
                {
                    // Image i = Image.FromFile(@"C:\...\...\image.jpg");
                    Rectangle m = args.MarginBounds;

                    if ((double)i.Width / (double)i.Height > (double)m.Width / (double)m.Height) // image is wider
                    {
                        m.Height = (int)((double)i.Height / (double)i.Width * (double)m.Width);
                    }
                    else
                    {
                        m.Width = (int)((double)i.Width / (double)i.Height * (double)m.Height);
                    }
                    args.Graphics.DrawImage(i, m);
                };

                pd.Print();
            }
        }
    }
}
