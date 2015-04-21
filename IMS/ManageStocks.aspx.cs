using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ManageStocks : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnAddStock_Click(object sender, EventArgs e)
        {
            Response.Redirect("AddStock.aspx");
        }


        protected void btnEditStock_Click(object sender, EventArgs e)
        {
            Response.Redirect("SelectionStock.aspx");
        }

        protected void btnCheckInventory_Click(object sender, EventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageInventory.aspx");
        }

        protected void btnViewStocks_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewInventory.aspx");
        }
    }
}