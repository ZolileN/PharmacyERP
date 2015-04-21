using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class WarehouseMain : System.Web.UI.Page
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

        protected void btnManageInventory_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageInventory.aspx",false);
        }

        protected void btnManageOrders_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageOrders.aspx");
        }

        protected void btnRegisterUser_Click(object sender, EventArgs e)
        {
           
        }

        protected void tbnStoreRequests_Click(object sender, EventArgs e)
        {
            Response.Redirect("Warehouse_StoreRequests.aspx", false);
        }

        protected void ButtonBack_Click(object sender, EventArgs e)
        {
            if (Session["isHeadOffice"].ToString().ToLower().Equals("true"))
            {
                Response.Redirect("HeadOfficeMain.aspx", false);
            }
            
        }

        protected void btnManageVendor_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageVendor.aspx", false);
        }

        protected void btnHaadList_Click(object sender, EventArgs e)
        {
            Response.Redirect("HAADPopulation.aspx", false);
        }

        protected void btnAdjustData_Click(object sender, EventArgs e)
        {
            Response.Redirect("ImportingData.aspx", false);
        }

        protected void btnPackingList_Click(object sender, EventArgs e)
        {
            Response.Redirect("PackingListGeneration.aspx");
        }

        
    }
}