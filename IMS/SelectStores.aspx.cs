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
            mpeCongratsMessageDiv.Show();
        }

        protected void btnContinue_Click(object sender, EventArgs e)
        {

        }
    }
}