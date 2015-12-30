using IMSBusinessLogic;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class Returns_Generate : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {

                PO_Number.Text = Page.Request.Params[0].ToString();
                DataSet dsMasterReturn = ProductReturnBLL.GetItemReturns(long.Parse(PO_Number.Text));
                if(dsMasterReturn.Tables[0].Rows.Count>0)
                {
                    PO_Date.Text = dsMasterReturn.Tables[0].Rows[0]["ReturnDate"].ToString();
                    PO_ToName.Text = dsMasterReturn.Tables[0].Rows[0]["SupName"].ToString();
                    PO_ToPhone.Text = dsMasterReturn.Tables[0].Rows[0]["Phone"].ToString();
                    PO_ToEmail.Text = dsMasterReturn.Tables[0].Rows[0]["Email"].ToString();
                    PO_ToAddress.Text = dsMasterReturn.Tables[0].Rows[0]["Address"].ToString();
                    lblVendorType.Text = dsMasterReturn.Tables[0].Rows[0]["ReturnMode"].ToString();
                    DataSet dsDetailsReturn = ProductReturnBLL.GetItemReturnsDetails(long.Parse(PO_Number.Text));
                     if(dsDetailsReturn.Tables[0].Rows.Count>0)
                     {

                         gvReturns.DataSource = dsDetailsReturn;
                         gvReturns.DataBind();

                     }
                }
            }

        }

        protected void btnFax_Click(object sender, EventArgs e)
        {
            Response.Redirect("WarehouseMain.aspx", false);
        }
    }
}