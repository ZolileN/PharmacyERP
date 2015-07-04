using IMS.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class rpt_ItemPurchase_Selection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                String Parameter = "";
                if (Request.QueryString["Param"] != null)
                {
                    Parameter = Request.QueryString["Param"].ToString();
                    if (Parameter.Equals("Date"))
                    {
                        Session["DisplayPurchase"] = "rpt_ItemPurchaseDisplay_byDate.aspx";
                    }
                    else if (Parameter.Equals("Pharmacy"))
                    {
                        Session["DisplayPurchase"] = "rpt_ItemPurchaseDisplay_byPharmacy.aspx";
                    }
                    else if (Parameter.Equals("Price"))
                    {
                        Session["DisplayPurchase"] = "rpt_ItemPurchaseDisplay_byPrice.aspx";
                    }
                    else if (Parameter.Equals("Expiry"))
                    {
                        Session["DisplayPurchase"] = "rpt_ItemPurchaseDisplay_byExpiry.aspx";
                    }
                }

                if (Session["rptItemPurchaseDateF"] != null && Session["rptItemPurchaseDateF"].ToString() != "")
                {
                    txtDateFrom.Text = Session["rptItemPurchaseDateF"].ToString();
                }
                if (Session["rptItemPurchaseDateT"] != null && Session["rptItemPurchaseDateT"].ToString() != "")
                {
                    txtDateTO.Text = Session["rptItemPurchaseDateT"].ToString();
                }

                Session["rptProductID"] = null;

                Session["rptSubCategoryID"] = null;

                Session["rptCategoryID"] = null;

                Session["rptDepartmentID"] = null;

                Session["rptCustomerID"] = null;

                Session["rptBarterCustomers"] = null;

                Session["rptItemPurhcaseDateF"] = null;

                Session["rptItemPurchaseDateT"] = null;

                Session["SP_Purchase"] = "YES";


                ddlBarterCustomer.Items.Clear();
                ddlBarterCustomer.Items.Add("Select Option");
                ddlBarterCustomer.Items.Add("Include");
                ddlBarterCustomer.Items.Add("Exclude");
              

            }
        }

        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            if (txtProduct.Text != "")
            {
                Session["rptProductID"] = Session["rptProductID"];
                Session["selectionProduct"] = txtProduct.Text;

            }
            else
            {
                Session["rptProductID"] = "";
                Session["selectionProduct"] = "All";
            }

            if (txtSubcategory.Text != "")
            {
                Session["rptSubCategoryID"] = Session["rptSubCategoryID"];
                Session["selectionSubCategory"] = txtSubcategory.Text;
            }
            else
            {
                Session["rptSubCategoryID"] = "";
                Session["selectionSubCategory"] = "All";
            }

            if (txtCategory.Text != "")
            {
                Session["rptCategoryID"] = Session["rptCategoryID"];
                Session["selectionCategory"] = txtCategory.Text;
            }
            else
            {
                Session["rptCategoryID"] = "";
                Session["selectionCategory"] = "All";
            }

            if (txtDepartment.Text != "")
            {
                Session["rptDepartmentID"] = Session["rptDepartmentID"];
                Session["selectionDepartment"] = txtDepartment.Text;
            }
            else
            {
                Session["rptDepartmentID"] = "";
                Session["selectionDepartment"] = "All";
            }

            if (txtCustomers.Text != "")
            {
                Session["rptCustomerID"] = Session["rptCustomerID"];
                Session["selectionCustomers"] = txtCustomers.Text;
            }
            else
            {
                Session["rptCustomerID"] = "";
                Session["selectionCustomers"] = "All";
            }

            if (txtDateFrom.Text != "")
            {
                Session["rptItemPurchaseDateF"] = txtDateFrom.Text.ToString();
            }
            else
            {
                Session["rptItemPurchaseDateF"] = "";
            }

            if (txtDateTO.Text != "")
            {
                Session["rptItemPurchaseDateT"] = txtDateTO.Text.ToString();
            }
            else
            {
                Session["rptItemPurchaseDateT"] = "";
            }

            if (txtExpiry.Text != "")
            {
                Session["rptItemPurchaseExpiry"] = txtExpiry.Text.ToString();
            }
            else
            {
                Session["rptItemPurchaseExpiry"] = "";
            }

            if (ddlBarterCustomer.SelectedItem.ToString().Equals("Exclude"))
            {
                Session["rptBarterCustomers"] = "Exclude";
            }
            else
            {
                Session["rptBarterCustomers"] = "Include";
            }

            /*if (ddlInternalCustomer.SelectedItem.ToString().Equals("Exclude"))
            {
                Session["rptInternalCustomers"] = "Exclude";
            }
            else
            {
                Session["rptInternalCustomers"] = "Include";
            }*/


          //Session["ItemPurchaseReportDetail"] = dt;
            //Page.ClientScript.RegisterStartupScript(this.GetType(), "OpenWindow", "window.open('ItemPurchase.aspx','_newtab');", true);
           Response.Redirect(Session["DisplayPurchase"].ToString());
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {

            Session["rptProductID"] = "";

            Session["rptSubCategoryID"] = "";

            Session["rptCategoryID"] = "";

            Session["rptDepartmentID"] = "";

            Session["rptCustomerID"] = "";

            Session["rptBarterCustomers"] = "";

            Session["rptItemPurchaseDateF"] = "";

            Session["rptItemPurchaseDateT"] = "";


            Response.Redirect("WarehouseMain.aspx");
        }

        protected void btnSearchCustomers_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchItem_RPT"] = txtCustomers.Text.ToString();
                CustomerPopupGrid.LoadData();
                mpeCustomersDiv.Show();
            }
            catch (Exception ex)
            {
                //show message
            }
            finally
            {

            }
        }

        protected void ddlBarterCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSeachDepartment_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchItemDept_RPT"] = txtDepartment.Text.ToString();
                DepartmentPopupGrid.LoadData();
                mpeDepartmentDiv.Show();
            }
            catch (Exception ex)
            {
                //show message
            }
            finally
            {

            }
        }

        protected void btnSearchCategory_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchItemCat_RPT"] = txtCategory.Text.ToString();
                CategoryPopupGrid.LoadData();
                mpeCategoryDiv.Show();
            }
            catch (Exception ex)
            {
                //show message
            }
            finally
            {

            }
        }

        protected void btnSearchSubcat_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchItemSubCat_RPT"] = txtSubcategory.Text.ToString();
                SubCategoryPopupGrid.LoadData();
                mpeSubCategoryDiv.Show();
            }
            catch (Exception ex)
            {
                //show message
            }
            finally
            {

            }
        }

        protected void btnSearchProduct_Click(object sender, EventArgs e)
        {
            try
            {
                Session["SearchItemProduct_RPT"] = txtProduct.Text.ToString();
                ProductPopupGrid.LoadData();
                mpeProductDiv.Show();
            }
            catch (Exception ex)
            {
                //show message
            }
            finally
            {

            }
        }

        protected void ddlInternalCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearchSalesMan_Click(object sender, EventArgs e)
        {

        }
    }
}