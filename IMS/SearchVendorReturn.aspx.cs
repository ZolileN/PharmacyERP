using IMSBusinessLogic;
using IMSCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class SearchVendorReturn : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }
        public void loadVendors()
        {
            Vendor vendor = new Vendor();
            vendor.SupName = txtVendorName.Text;
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
            ds = VendorBLL.GetDistinctByNane(vendor, SysID, isStore);
            //ProductSet = ds;
            gdvVendor.DataSource = null;
            gdvVendor.DataSource = ds;
            gdvVendor.DataBind();
        }
        protected void btnSearchProduct_Click(object sender, ImageClickEventArgs e)
        {
            loadVendors();
            
        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            
            loadVendors();
            gdvVendor.PageIndex = e.NewPageIndex;
            gdvVendor.DataBind();
        }

        public void UncheckOther(int rowindex)
        {
            foreach (GridViewRow row in gdvVendor.Rows)
            {
                CheckBox ChkBoxRows = (CheckBox)row.FindControl("chkRow");

                if (row.RowIndex==rowindex)
                {
                    if (ChkBoxRows.Checked)
                    {
                        ChkBoxRows.Checked = true;
                    }
                    else
                    {
                        ChkBoxRows.Checked = false;
                    }
                   
                }
                else
                {
                    ChkBoxRows.Checked = false;
                }

                    
            }
        
        }
        protected void chkRow_CheckedChanged(object sender, EventArgs e)
        {

            hdfSupId.Value = "-1";

            CheckBox chk = (CheckBox)sender;
            GridViewRow gr = (GridViewRow)chk.Parent.Parent;
            
            UncheckOther(gr.RowIndex);
            
            hdfSupId.Value = gdvVendor.DataKeys[gr.RowIndex].Value.ToString()+">"+gdvVendor.Rows[gr.RowIndex].Cells[2].Text;
           

         
            

        }

        protected void btnSelect_Click(object sender, EventArgs e)
        {
            if (hdfSupId.Value.ToString() != "-1")
            {
                Response.Redirect("SearchProductReturn.aspx?id=" + hdfSupId.Value.ToString());
            }
        }
    }
}