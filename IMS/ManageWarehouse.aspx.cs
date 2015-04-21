using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ManageWarehouse : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddWH_Click(object sender, EventArgs e)
        {
            Session["Action"] = "Add";
            Session["SysToAdd"] = RoleNames.warehouse;
            Response.Redirect("AddSystem.aspx", false);
        }

        protected void btnViewWareHouse_Click(object sender, EventArgs e)
        {
            Session["Action"] = "Select";
            Session["SysToAdd"] = RoleNames.warehouse;
            ucSel.SetValue = true;
            mpeEditProduct.Show();
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("HeadOfficeMain.aspx", false);
        }

        protected void btnSubmit_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }

        protected void btnEdit_Click(object sender, EventArgs e)
        {
            Session["Action"] = "Edit";
            Session["SysToAdd"] = RoleNames.warehouse;
            Response.Redirect("AddSystem.aspx", false);
        }
    }
}