using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ManageOrders : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnPlaceOrder_Click(object sender, EventArgs e)
        {
            Session["OrderNumber"] = null;
            Session["FromViewPlacedOrders"] = "false";
            Response.Redirect("PlaceOrder.aspx");
        }

        protected void btnRecievOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("RecieveOrder.aspx");
        }

        protected void btnViewOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewPlacedOrders.aspx");
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx");
        }

        protected void tbnSalesOrder_Click(object sender, EventArgs e)
        {
            Response.Redirect("OrderSalesManual.aspx");
        }
    }
}