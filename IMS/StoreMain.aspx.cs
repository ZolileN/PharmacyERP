using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class StoreMain : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (Session["isHeadOffice"].ToString().ToLower().Equals("true"))
                {
                    ButtonBack.Visible = true;
                }
            }
        }

        protected void btnViewInventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewInventory.aspx");
        }

        protected void btnStoreTransfers_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreTransfers_StoreRequests.aspx");
        }

        protected void btnStoreRecievings_Click(object sender, EventArgs e)
        {
             Response.Redirect("ReceiveStock.aspx");
        }

        protected void btnStoreRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("StoreRequestsMain.aspx");
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            if (Session["isHeadOffice"].ToString().ToLower().Equals("true"))
            {
                Response.Redirect("HeadOfficeMain.aspx", false);
            }

        }
    }
}