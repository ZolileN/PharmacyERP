using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class generateBarCode : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["d"] != null)
            {
                System.Drawing.Image barcodeImage = null;
                try
                {
                    string strData = Request.QueryString["d"];
                    int imageHeight = Convert.ToInt32(Request.QueryString["h"]);
                    int imageWidth = Convert.ToInt32(Request.QueryString["w"]);
                    //string Forecolor = Request.QueryString["fc"];
                    //string Backcolor = Request.QueryString["bc"];
                    //bool bIncludeLabel = Request.QueryString["il"].ToLower().Trim() == "true";
                    string strImageFormat = "jpeg";
                    //string strAlignment = Request.QueryString["align"].ToLower().Trim();


                    if (Request.QueryString["d"] != null)
                    {

                        BarcodeLib.Barcode b = new BarcodeLib.Barcode();
                        b.IncludeLabel = true;
                        barcodeImage = b.Encode(BarcodeLib.TYPE.EAN13, strData, System.Drawing.ColorTranslator.FromHtml("#" + "000000"), System.Drawing.ColorTranslator.FromHtml("#" + "FFFFFF"), 300, 150);


                        Response.ContentType = "image/" + strImageFormat;
                        System.IO.MemoryStream MemStream = new System.IO.MemoryStream();

                        switch (strImageFormat)
                        {
                            case "gif": barcodeImage.Save(MemStream, ImageFormat.Gif); break;
                            case "jpeg": barcodeImage.Save(MemStream, ImageFormat.Jpeg); break;
                            case "png": barcodeImage.Save(MemStream, ImageFormat.Png); break;
                            case "bmp": barcodeImage.Save(MemStream, ImageFormat.Bmp); break;
                            case "tiff": barcodeImage.Save(MemStream, ImageFormat.Tiff); break;
                            default: break;
                        }//switch
                        MemStream.WriteTo(Response.OutputStream);

                    }
                }
                catch (Exception exo) { }
                finally
                {
                    if (barcodeImage != null)
                    {
                        //Clean up / Dispose...
                        barcodeImage.Dispose();
                    }
                }//finally
            }
        }
    }
}