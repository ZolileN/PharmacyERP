using IMSBusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class SearchProductReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (hdfSupId.Value.ToString() == "-1")
                {
                    hdfSupId.Value = Page.Request.Params[0].Split('>')[0].ToString();
                    lblVendor.Text = Page.Request.Params[0].Split('>')[1].ToString();
                    CreatedataTable();
                    SetDataSourceReturn();
                    Button1.Style.Add("Display", "none");
                }
            }
        }
        public void loadProducts()
        {
            
            int SysID = Convert.ToInt32(Session["UserSys"].ToString());
            bool isStore;

            if (!Session["UserRole"].ToString().Equals("Store"))
            {
                isStore = false;
            }
            else
            {
                isStore = true;
            }
            DataSet ds;
           ds= ProductReturnBLL.GetStockByName(txtProductName.Text, SysID, isStore);


            gdvProducts.DataSource = null;
            gdvProducts.DataSource = ds;
            gdvProducts.DataBind();
            pnlgdvProducts.Visible = true;
        }
        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            loadProducts();
        }
    
        public void loadSubGridBatches(long ProductID)
        {
            int SysID = Convert.ToInt32(Session["UserSys"].ToString());
           
            DataSet ds;
            ds = ProductReturnBLL.GetStockBatches(ProductID, SysID);


            grvBatches.DataSource = null;
            grvBatches.DataSource = ds;
            grvBatches.DataBind();
           
        }

        protected void chkRowBtaches_CheckedChanged(object sender, EventArgs e)
        {
            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;

            TextBox txt = (TextBox)gr.FindControl("txtReturnQty");
            if (chk.Checked)
            {
                
                txt.Enabled = true;

               
            }
            else
            {
                txt.Enabled = false;
                //txt.Text = "";
            }
            mPoupBatch.Show();
        }
     

        protected void gdvProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            loadProducts();
            gdvProducts.PageIndex = e.NewPageIndex;
            gdvProducts.DataBind();
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("SearchVendorReturn.aspx");
        }

        protected void txtReturnQty_TextChanged(object sender, EventArgs e)
        {
            try
            {
                TextBox txt = (TextBox)sender;
                GridViewRow gr = (GridViewRow)txt.Parent.Parent;
                Int32 Qty = Int32.Parse(grvBatches.Rows[gr.RowIndex].Cells[4].Text);
                if (txt.Text != "")
                {
                    if (Int32.Parse(txt.Text) > Qty)
                    {

                        ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Return Qty can not be greater than Qty')", true);
                        txt.Text = "";
                    }

                }
            }
            catch
            {
                
               
            }
           
           
          

            mPoupBatch.Show();
        }
        public void CreatedataTable()
        {
            DataTable dt = new DataTable();
            dt.Columns.Add("StockID");
            dt.Columns.Add("ProductID");
            dt.Columns.Add("Name");
            dt.Columns.Add("BatchNo");
            dt.Columns.Add("Expiry");
            dt.Columns.Add("ReturnQty");
            Session.Add("dtReturn",dt);

        }
        public void SetDataSourceReturn()
        {
           
            grvReturns.DataSource = (DataTable)Session["dtReturn"];
            grvReturns.DataBind();

            if (grvReturns.Rows.Count > 0) panelgrvReturns.Visible = true; else panelgrvReturns.Visible = false;
        }
        public void CheckBatchesChange()
        {
            DataTable dt = (DataTable)Session["dtReturn"];
            foreach (GridViewRow row in grvBatches.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRowBtaches");

                if (ChkBoxRows.Checked)
                {
                  TextBox txt = (TextBox)row.FindControl("txtReturnQty");
                  DataRow[] rows;


                  long stockID =long.Parse(grvBatches.DataKeys[row.RowIndex].Value.ToString());

                  rows = dt.Select("StockID = '" + stockID + "'");

                  if (rows.Count() != 0)
                  {
                      rows[0]["ReturnQty"] = txt.Text;


                      dt.AcceptChanges();
                  }
                  else
                  {
                      if (txt.Text != "" &&  txt.Text!="0")
                          dt.Rows.Add(new object[] { stockID, hdfProductId.Value, hdfProdutName.Value, row.Cells[2].Text.Replace("&nbsp;", ""), row.Cells[3].Text.Replace("&nbsp;", ""), txt.Text });
                  }
                    

                }

            }
           
            Session.Add("dtReturn", dt);
            SetDataSourceReturn();

            mPoupBatch.Hide();
            grvBatches.DataSource = null;
            grvBatches.DataBind();

           
        }

        protected void btnPoupSelect_Click(object sender, EventArgs e)
        {
            CheckBatchesChange();
        }

        protected void grvReturns_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "deleterow")
            {

               
                GridViewRow gvRow = (GridViewRow)(((LinkButton)e.CommandSource).NamingContainer);
                string ID = grvReturns.DataKeys[gvRow.RowIndex].Value.ToString();
                
                long stockId = long.Parse(ID);
                DataTable dt = (DataTable)Session["dtReturn"];
                
                DataRow[] rows;
                rows = dt.Select("StockID = '" + stockId + "'");
                foreach (DataRow row in rows)
                    dt.Rows.Remove(row);

                dt.AcceptChanges();
                Session.Add("dtReturn", dt);
                SetDataSourceReturn();
            }
        }

        protected void grvBatches_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                DataRowView rowView = (DataRowView)e.Row.DataItem;
                long stockId = long.Parse(rowView["stockId"].ToString());


                DataTable dt = (DataTable)Session["dtReturn"];
                DataRow[] rows;
                rows = dt.Select("StockID = '" + stockId + "'");

                if (rows.Count()!=0)
                {
                    TextBox txtreturnQty = (e.Row.FindControl("txtReturnQty") as TextBox);
                    CheckBox chkrow = (e.Row.FindControl("chkRowBtaches") as CheckBox);

                    chkrow.Checked = true;
                    txtreturnQty.Enabled = true;
                    txtreturnQty.Text = rows[0].ItemArray[5].ToString();
                }
               
            }
        }

        protected void btnGenerateReturns_Click(object sender, EventArgs e)
        {
            DataTable dt = (DataTable)Session["dtReturn"];
            if (dt.Rows.Count > 0)
            {
                dt.Columns.Remove("Name");
                dt.Columns.Remove("BatchNo");
                dt.Columns.Remove("Expiry");
               
                dt.AcceptChanges();
               DataSet dsReturnID=
               ProductReturnBLL.GenerateItemReturn(long.Parse(hdfSupId.Value.ToString()), long.Parse(Session["UserSys"].ToString()), Int32.Parse(Session["UserID"].ToString()), Session["UserRole"].ToString(), dt);

                if (dsReturnID.Tables[0].Rows.Count > 0){
                    
                    Response.Redirect("Returns_Generate.aspx?id="+dsReturnID.Tables[0].Rows[0][0].ToString());
                
                }
            }
        }

        protected void btnClose_Click(object sender, EventArgs e)
        {
            grvBatches.DataSource = null;
            grvBatches.DataBind();
            mPoupBatch.Hide();

        }

        protected void lnkView_Click(object sender, EventArgs e)
        {
            LinkButton chk = (LinkButton)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;

            hdfProductId.Value = gdvProducts.DataKeys[gr.RowIndex].Value.ToString();
            hdfProdutName.Value = System.Net.WebUtility.HtmlDecode(gdvProducts.Rows[gr.RowIndex].Cells[3].Text.ToString());

            loadSubGridBatches(long.Parse(hdfProductId.Value.ToString()));
            mPoupBatch.Show();

        }
    }
}