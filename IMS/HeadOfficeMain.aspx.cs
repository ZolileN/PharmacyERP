using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
	public partial class HeadOfficeMain : System.Web.UI.Page
	{
		protected void Page_Load(object sender, EventArgs e)
		{

		}

        protected void btnMngStore_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStore.aspx", false);
        }

        protected void btnMngWareHouse_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageWarehouse.aspx", false);
        }

        protected void btnCmpnyInfo_Click(object sender, EventArgs e)
        {
            Response.Redirect("CompanySettings.aspx", false);
        }

        protected void btnViewReport_Click(object sender, EventArgs e)
        {
            Response.Redirect("Reports.aspx", false);
        }

        protected void BtnRegisterUser_Click(object sender, EventArgs e)
        {
            Response.Redirect("RegisterUsers.aspx", false);
        }
	}
}