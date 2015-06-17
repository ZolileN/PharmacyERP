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
                Session["rptProductID"] = null;

                Session["rptSubCategoryID"] = null;

                Session["rptCategoryID"] = null;

                Session["rptDepartmentID"] = null;

                Session["rptCustomerID"] = null;

                Session["rptSalesDateFrom"] = null;

                Session["rptSalesDateTo"] = null;
            }
        }

        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
            if(txtProduct.Text!="")
            {
                Session["rptProductID"] = Session["rptProductID"];
                
            }
            else
            {
                Session["rptProductID"] = "";
                
            }

            if (txtSubcategory.Text != "")
            {
                Session["rptSubCategoryID"] = Session["rptSubCategoryID"];
            }
            else
            {
                Session["rptSubCategoryID"] = "";
            }

            if (txtCategory.Text != "")
            {
                Session["rptCategoryID"] = Session["rptCategoryID"];
            }
            else
            {
                Session["rptCategoryID"] = "";
            }

            if (txtDepartment.Text != "")
            {
                Session["rptDepartmentID"] = Session["rptDepartmentID"];
            }
            else
            {
                Session["rptDepartmentID"] = "";
            }

            if (txtCustomers.Text != "")
            {
                Session["rptCustomerID"] = Session["rptCustomerID"];
            }
            else
            {
                Session["rptCustomerID"] = "";
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
    }
}