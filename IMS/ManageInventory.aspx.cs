using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ManageInventory : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnManageProducts_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageProducts.aspx",false);
        }

        protected void btnManageSubCategories_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageSubCategory.aspx", false);
        }

        protected void btnManageCategories_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageCategory.aspx", false);
        }

        protected void btnManageDepartments_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageDepartment.aspx", false);
        }

        protected void btnCheckInventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("ViewInventory.aspx",false);
        }

        protected void btnStocks_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageStocks.aspx",false);
        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx",false);
        }
    }
}