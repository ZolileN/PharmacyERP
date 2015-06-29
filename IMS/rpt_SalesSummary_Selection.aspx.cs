using IMS.UserControl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class rpt_SalesSummary_Selection : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                if (Session["rptSalesDateFrom"] != null && Session["rptSalesDateFrom"].ToString() != "")
                {
                    txtDateFrom.Text = Session["rptSalesDateFrom"].ToString();
                }
                if (Session["rptSalesDateTo"] != null && Session["rptSalesDateTo"].ToString() != "")
                {
                    txtDateTO.Text = Session["rptSalesDateTo"].ToString();
                }

                Session["rptProductID"] = null;

                Session["rptSubCategoryID"] = null;

                Session["rptCategoryID"] = null;

                Session["rptDepartmentID"] = null;

                Session["rptCustomerID"] = null;

                Session["rptBarterCustomers"] = null;

                Session["rptSalesDateFrom"] = null;

                Session["rptSalesDateTo"] = null;

                ddlInternalCustomer.Items.Clear();
                ddlInternalCustomer.Items.Add("Select Option");
                ddlInternalCustomer.Items.Add("Include");
                ddlInternalCustomer.Items.Add("Exclude");

                ddlBarterCustomer.Items.Clear();
                ddlBarterCustomer.Items.Add("Select Option");
                ddlBarterCustomer.Items.Add("Include");
                ddlBarterCustomer.Items.Add("Exclude");

                
            }
        }

        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            if (txtProduct.Text != "All" || txtProduct.Text != "")
            {
                Session["rptProductID"] = Session["rptProductID"];
                Session["selectionProduct"] = txtProduct.Text;
                
            }
            else
            {
                Session["rptProductID"] = "";
                Session["selectionProduct"] = "All";
            }

            if (txtSubcategory.Text != "All" || txtSubcategory.Text != "")
            {
                Session["rptSubCategoryID"] = Session["rptSubCategoryID"];
                Session["selectionSubCategory"] = txtSubcategory.Text;
            }
            else
            {
                Session["rptSubCategoryID"] = "";
                Session["selectionSubCategory"] = "All";
            }

            if (txtCategory.Text != "All" || txtCategory.Text != "")
            {
                Session["rptCategoryID"] = Session["rptCategoryID"];
                Session["selectionCategory"] = txtCategory.Text;
            }
            else
            {
                Session["rptCategoryID"] = "";
                Session["selectionCategory"] = "All";
            }

            if (txtDepartment.Text != "All" || txtDepartment.Text != "")
            {
                Session["rptDepartmentID"] = Session["rptDepartmentID"];
                Session["selectionDepartment"] = txtDepartment.Text;
            }
            else
            {
                Session["rptDepartmentID"] = "";
                Session["selectionDepartment"] = "All";
            }

            if (txtCustomers.Text != "All" || txtCustomers.Text != "")
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
                Session["rptSalesDateFrom"] = txtDateFrom.Text.ToString();
            }
            else
            {
                Session["rptSalesDateFrom"] = "";
            }

            if (txtDateTO.Text != "")
            {
                Session["rptSalesDateTo"] = txtDateTO.Text.ToString();
            }
            else
            {
                Session["rptSalesDateTo"] = "";
            }

            if (ddlInternalCustomer.SelectedItem.ToString().Equals("Exclude"))
            {
                Session["rptInternalCustomers"] = "Exclude";
            }
            else
            {
                Session["rptInternalCustomers"] = "Include";
            }

            if (ddlBarterCustomer.SelectedItem.ToString().Equals("Exclude"))
            {
                Session["rptBarterCustomers"] = "Exclude";
            }
            else
            {
                Session["rptBarterCustomers"] = "Include";
            }


            Response.Redirect("rpt_SalesSummaryDisplay.aspx");
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            
                Session["rptProductID"] = "";

                Session["rptSubCategoryID"] = "";
            
                Session["rptCategoryID"] = "";
            
                Session["rptDepartmentID"] = "";
            
                Session["rptCustomerID"] = "";
            
                Session["rptSalesDateFrom"] = "";
            
                Session["rptSalesDateTo"] = "";

                Session["rptBarterCustomers"] = "";

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
            catch(Exception ex)
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
    }
}