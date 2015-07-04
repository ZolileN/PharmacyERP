using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class SelectStores : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtStore.Text + '%';
            Session["txtStore"] = Text;
            StoresPopupGrid.PopulateGrid();
            StoresPopupGrid.SessionIDTag = "SystemId";
            StoresPopupGrid.SessionNameTag = "Storename";
            StoresPopupGrid.SourcePage = "SelectStores.aspx";
            StoresPopupGrid.DestinationPage = "AddVendorsToStore.aspx";
            mpeCongratsMessageDiv.Show();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {
            string Storename = txtStore.Text;
            Session["Storename"] = Storename;
            Session["SystemId"] = lblStoreId.Text;             
            Response.Redirect("AddVendorsToStore.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("Storename");
            Session.Remove("SystemId");
            Response.Redirect("WarehouseMain.aspx",false);
        }
    }
}