using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class PlaceOrder : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAutoPurchase_Click(object sender, EventArgs e)
        {

        }

        protected void btnManualPurchase_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            Response.Redirect("OrderPurchaseManual.aspx");
        }

        protected void btnEditOrder_Click(object sender, EventArgs e)
        {
            //Response.Redirect("ViewPlacedOrders.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            Response.Redirect("ManageOrders.aspx");
        }
    }
}