using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class StoreRequestsMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAutoGenerateRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect("AutoRequest_Store.aspx");
        }

        protected void btnManualGenerateRequest_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreRequests.aspx");
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreMain.aspx");
        }
    }
}