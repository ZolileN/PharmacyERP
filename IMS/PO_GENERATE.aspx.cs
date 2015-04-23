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
using System.Text;
using System.Collections;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.text.html;
using iTextSharp.text.html.simpleparser;
using System.Net.Mail;
using Microsoft.Reporting.WebForms;
using System.Net.Mime;
using System.Net;


namespace IMS
{
    public partial class PO_GENERATE : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        public static DataSet systemSet; 
        public static bool FirstOrder;
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                FirstOrder = false;
                systemSet = new DataSet();
                ProductSet = new DataSet();
                LoadData();
                #region Getting & Populating Values
                PO_Number.Text = Session["OrderNumber"].ToString();
                DataSet dsProducts = (DataSet)Session["dsProducts"];
                PO_Date.Text = ((DateTime)dsProducts.Tables[0].Rows[0]["OrderDate"]).Date.ToString();

                PO_FromName.Text = dsProducts.Tables[0].Rows[0]["SystemName"].ToString();
                PO_FromAddress.Text = dsProducts.Tables[0].Rows[0]["SystemAddress"].ToString();
                PO_FromPhone.Text = "Phone: " + dsProducts.Tables[0].Rows[0]["SystemPhone"].ToString();

                PO_ToName.Text = dsProducts.Tables[0].Rows[0]["SupName"].ToString();
                PO_ToAddress.Text = dsProducts.Tables[0].Rows[0]["VendorAddress"].ToString();
                PO_ToPhone.Text = "Phone:" + dsProducts.Tables[0].Rows[0]["VendorPhone"].ToString();
                PO_ToEmail.Text = "Email:" + dsProducts.Tables[0].Rows[0]["VendorEmail"].ToString();
                float TCost = 0;
                for (int i = 0; i < dsProducts.Tables[0].Rows.Count; i++)
                {
                    float Cost =0;
                    if (float.TryParse(dsProducts.Tables[0].Rows[i]["totalCostPrice"].ToString(), out Cost))
                    {
                        TCost += Cost;
                    }
                }
                lblTotalCostALL.Text = TCost.ToString();
                #endregion

                //ExportGridToPDF();
            }
        }

        private void LoadData()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_GeneratePO", connection);
                command.CommandType = CommandType.StoredProcedure;

                int OrderNumber = 0;
                if (int.TryParse(Session["OrderNumber"].ToString(), out OrderNumber))
                {
                    command.Parameters.AddWithValue("@p_OrderNo", OrderNumber);
                }

                SqlDataAdapter dA = new SqlDataAdapter(command);
                DataSet ds = new DataSet();
                 
                dA.Fill(ds);
                Session["dsProducts"] = ds;

                ProductSet = ds;
                DataSet dsProducts = (DataSet)Session["dsProducts"];
                //dA.Fill(ProductSet);
                StockDisplayGrid.DataSource = dsProducts;
                StockDisplayGrid.DataBind();
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
        protected void StockDisplayGrid_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            StockDisplayGrid.PageIndex = e.NewPageIndex;
            LoadData();
        }

        public static System.Data.DataSet PrepareDSForExport(System.Data.DataSet ds)
        {
            if (ds == null || ds.Tables.Count == 0)
                return ds;

            foreach (DataRow dr in ds.Tables[0].Rows)
            {
                foreach (DataColumn dc in ds.Tables[0].Columns)
                {
                    if (dr[dc.ColumnName].ToString().Replace(",", ".").Trim() == "" && dc.DataType == typeof(DateTime))
                        dr[dc.ColumnName] = DateTime.Parse("1/1/1900");
                    else if (dr[dc.ColumnName].ToString().Replace(",", ".").Trim() == "" && (dc.DataType == typeof(int) || dc.DataType == typeof(long) || dc.DataType == typeof(decimal) || dc.DataType == typeof(float)))
                        dr[dc.ColumnName] = "0";
                    else
                        dr[dc.ColumnName] = dr[dc.ColumnName].ToString().Replace(",", ".").Trim();
                }
            }

            return ds;

        }

        public static void ExportData(System.Data.DataTable dt, string ExportFileName)
        {

            //if (sfd.FilterIndex == 1)
            //{
            //    bool ExcelImport = false;
            //    if (W.Show("Will this file be Imported into MS EXCEL?", "EXCEL", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
            //        ExcelImport = true;

            //    using (StreamWriter sw = new StreamWriter(sfd.FileName))
            //    {
            //        StringBuilder sb = new StringBuilder();
            //        foreach (DataColumn dc in dt.Columns)
            //        {
            //            sb.Append(dc.ColumnName + ",");
            //        }
            //        sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));

            //        foreach (DataRow dr in dt.Rows)
            //        {
            //            sb = new StringBuilder();
            //            string rec = "";
            //            foreach (DataColumn dc in dt.Columns)
            //            {
            //                if (ExcelImport)
            //                    rec = @"=" + "\"" + dr[dc.ColumnName].ToString() + "\"";
            //                else
            //                    rec = dr[dc.ColumnName].ToString();

            //                sb.Append(rec + ",");
            //            }

            //            sw.WriteLine(sb.ToString().Substring(0, sb.ToString().Length - 1));
            //        }
            //    }
            //}
            //else if (sfd.FilterIndex == 2)
            //{
            //Document is inbuilt class, available in iTextSharp
            Document document = new Document(PageSize.A4, 80, 50, 30, 65);
            StringBuilder strData = new StringBuilder(string.Empty);
            try
            {
                StringWriter sw = new StringWriter();
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine(Environment.NewLine);
                sw.WriteLine(Environment.NewLine);
                StreamWriter strWriter = new StreamWriter(ExportFileName, false, Encoding.UTF8);
                //strWriter.Write("<html><head><link href=Style.css rel=stylesheet type=text/css /></head><body>" + htw.InnerWriter.ToString() + "</body></html>");
                #region Write in StreamWriter

                string sError = string.Empty;
                int iNoOfColsinaPage = 7;
                int iRunningColCount = 0;
                strWriter.Write("<HTML><BODY><TABLE>");
                foreach (DataColumn dc in dt.Columns)
                {

                    if (iRunningColCount % iNoOfColsinaPage == 0)
                    {
                        strWriter.Write("<TR>");
                    }
                    strWriter.Write("<TD>&nbsp;" + dc.ColumnName + "&nbsp;</TD>");

                    iRunningColCount++;

                    if (iRunningColCount % iNoOfColsinaPage == 0)
                    {
                        strWriter.Write("</TR>");
                    }
                }

                foreach (DataRow dr in dt.Rows)
                {
                    iRunningColCount = 0;
                    foreach (DataColumn dc in dt.Columns)
                    {
                        if (iRunningColCount % iNoOfColsinaPage == 0)
                        {
                            strWriter.Write("<TR>");
                        }
                        strWriter.Write("<TD>" + dr[dc.ColumnName].ToString() + "&nbsp;</TD>");
                        iRunningColCount++;
                        if (iRunningColCount % iNoOfColsinaPage == 0)
                        {
                            strWriter.Write("</TR>");
                        }
                    }
                }

                strWriter.Write("</TABLE></BODY></HTML>");
                //sb = strWriter.to .Replace("</TD><TR>", "</TD></TR><TR>");

                #endregion
                strWriter.Close();
                strWriter.Dispose();
                iTextSharp.text.html.simpleparser.
                StyleSheet styles = new iTextSharp.text.html.simpleparser.StyleSheet();
                styles.LoadTagStyle("ol", "leading", "16,0");
                List<IElement> objects;
                styles.LoadTagStyle("li", "face", "garamond");
                styles.LoadTagStyle("span", "size", "8px");
                styles.LoadTagStyle("body", "font-family", "times new roman");
                styles.LoadTagStyle("body", "font-size", "10px");
                StreamReader sr = new StreamReader(ExportFileName, Encoding.Default);
                objects = HTMLWorker.ParseToList(sr, styles);
                sr.Close();
                PdfWriter.GetInstance(document, new FileStream(ExportFileName, FileMode.Create));

                document.Add(new Header("stylesheet", "Style.css"));
                document.Open();
                document.NewPage();
                for (int k = 0; k < objects.Count; k++)
                {
                    document.Add((IElement)objects[k]);
                }

            }
            catch (Exception ex)
            {
                //throw ex;
            }
            finally
            {
                document.Close();
            }
        }

        protected void btnPrint_Click(object sender, EventArgs e)
        {
            
            ExportGridToPDF();
            
            Page_Load(sender, e);
            
        }

        protected void btnFax_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderPurchaseManual.aspx",false);
        }

        private void ReadPdfFile(String path)
        {
            WebClient client = new WebClient();
            Byte[] buffer = client.DownloadData(path);

            if (buffer != null)
            {
                Response.ContentType = "application/pdf";
                Response.AddHeader("content-length", buffer.Length.ToString());
                Response.BinaryWrite(buffer);
            }

        }
        protected void btnEmail_Click(object sender, EventArgs e)
        {
            if (PO_ToEmail.Text != null && PO_ToEmail.Text != "")
            {
                ExportGridToPDF();
                DataSet dsProducts = (DataSet)Session["dsProducts"];
                String ToAddress = dsProducts.Tables[0].Rows[0]["VendorEmail"].ToString();
                //String ToAddress = ProductSet.Tables[0].Rows[0]["VendorEmail"].ToString();
                try
                {
                MailMessage msg = new MailMessage();
                msg.From = new MailAddress("alahliyapharmceutical@gmail.com");
                if (ToAddress != null && ToAddress != "")
                {
                    msg.To.Add(new MailAddress(ToAddress));
                }

                msg.Subject = "Purchase Order Generated by " + PO_FromName.Text;
                msg.Body = "The Purchase Order Number [" + PO_Number.Text + "] is generated on " + System.DateTime.Now.ToShortDateString() + " by " + PO_FromName.Text
                          + " to " + PO_ToName.Text + ".";;
               
                    //msg.IsBodyHtml = IsBodyHTML;
                    String FilePath = Server.MapPath(@"~\PurchaseOrders");
                    String FileName = "PO_" + Session["OrderNumber"].ToString() + ".pdf";
                    String FileAttachment = FilePath + @"\" + FileName;
                    if (FileAttachment != null)
                    {
                        Attachment attachment = new Attachment(FileAttachment);

                        msg.Attachments.Add(attachment);
                    }
                    
                msg.Priority = MailPriority.High;
                SmtpClient objSMTP = new SmtpClient();
                objSMTP.Send(msg);
                //WebmessageUtil.Show();
                }
                catch (Exception ex)
                {
                    Response.Write("<script>alert('Proper Credentials are not given, please check it later');</script>");
                }
            }
        }
        

        public override void VerifyRenderingInServerForm(Control control)
        {
            //required to avoid the runtime error "
            //Control 'GridView1' of type 'GridView' must be placed inside a form tag with runat=server."
        }
        private void ExportGridToPDF()
        {
            try
            {
                //Response.ContentType = "application/pdf";
                //Response.AddHeader("content-disposition", "attachment;filename=PO_" + Session["OrderNumber"].ToString() + ".pdf");
                //Response.Cache.SetCacheability(HttpCacheability.NoCache);
                StringWriter sw = new StringWriter();
                sw.WriteLine("");
                HtmlTextWriter hw = new HtmlTextWriter(sw);
                MAINDIV.RenderControl(hw);
                //StockDisplayGrid.RenderControl(hw);
                //TotalCostDiv.RenderControl(hw);
                StringReader sr = new StringReader(sw.ToString());
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 10f, 0f);
                HTMLWorker htmlparser = new HTMLWorker(pdfDoc);
                pdfDoc.Open();

                htmlparser.Parse(sr);
                String FilePath = Server.MapPath(@"~\PurchaseOrders");
                String FileName = "PO_" + Session["OrderNumber"].ToString() + ".pdf";
                FileStream fs = new FileStream(FilePath + @"\" + FileName, FileMode.Create);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, fs);
                writer.Close();
                pdfDoc.Close();
                fs.Close();
                //Response.Write(pdfDoc);
                
                //Response.Flush();
                //Response.SuppressContent = true; 
                //Response.End();
                //Response.Redirect("PO_GENEREATE.aspx");
                //StockDisplayGrid.AllowPaging = true;
                //StockDisplayGrid.DataBind();
            }
            catch(Exception ex)
            {

            }
            finally
            {
               // MAINDIV.Visible = false;
               // TotalCostDiv.Visible = false;
                btnEmail.Visible = true;
                //btnFax.Visible = true;
                //btnPrint.Enabled = false;
            }

        }

        protected void StockDisplayGrid_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                //Label ProductStrength = (Label)e.Row.FindControl("ProductStrength2");
                //Label Label1 = (Label)e.Row.FindControl("Label1");

                //Label dosage = (Label)e.Row.FindControl("dosage2");
                //Label Label2 = (Label)e.Row.FindControl("Label2");

                //Label packSize = (Label)e.Row.FindControl("packSize2");
                //Label Label3 = (Label)e.Row.FindControl("Label3");

                //if (String.IsNullOrWhiteSpace(ProductStrength.Text))
                //{
                //    ProductStrength.Visible = false;
                //    Label1.Visible = false;
                //}
                //else
                //{
                //    ProductStrength.Visible = true;
                //    Label1.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(dosage.Text))
                //{
                //    dosage.Visible = false;
                //    Label2.Visible = false;
                //}
                //else
                //{
                //    dosage.Visible = true;
                //    Label2.Visible = true;
                //}

                //if (String.IsNullOrWhiteSpace(packSize.Text))
                //{
                //    packSize.Visible = false;
                //    Label3.Visible = false;
                //}
                //else
                //{
                //    packSize.Visible = true;
                //    Label3.Visible = true;
                //}
            }
        }  


    }

}