using IMS.UserControl;
using AjaxControlToolkit;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class rpt_ItemSold_Selection : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
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
                        Session["Display"] = "rpt_ItemSoldDisplay_byDate.aspx";
                    }
                    else if (Parameter.Equals("Pharmacy"))
                    {
                        Session["Display"] = "rpt_ItemSoldDisplay_byPharmacy.aspx";
                    }
                    else if (Parameter.Equals("SalesMan"))
                    {
                        Session["Display"] = "rpt_ItemSoldDisplay_bySalesMan.aspx";
                    }
                }

                if (Session["rptItemSoldDateFrom"] != null && Session["rptItemSoldDateFrom"].ToString() != "")
                {
                    txtDateFrom.Text = Session["rptItemSoldDateFrom"].ToString();
                }
                if (Session["rptItemSoldDateTo"] != null && Session["rptItemSoldDateTo"].ToString() != "")
                {
                    txtDateTO.Text = Session["rptItemSoldDateTo"].ToString();
                }

                Session["rptProductID"] = null;

                Session["rptSubCategoryID"] = null;

                Session["rptCategoryID"] = null;

                Session["rptDepartmentID"] = null;

                Session["rptCustomerID"] = null;

                Session["rptSalesDateFrom"] = null;

                Session["rptSalesDateTo"] = null;

                Session["SP_Purchase"] = "NO";

                ddlInternalCustomer.Items.Clear();
                ddlInternalCustomer.Items.Add("Select Option");
                ddlInternalCustomer.Items.Add("Include");
                ddlInternalCustomer.Items.Add("Exclude");

            }
        }

        public void LoadSalesMan ()
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("sp_rptItemSold_getSalesMan", connection);
                command.CommandType = CommandType.StoredProcedure;
                DataSet ds = new DataSet();
                SqlDataAdapter sdA = new SqlDataAdapter(command);
                sdA.Fill(ds);

                ddlSalesMan.DataValueField = "SalesManID";
                ddlSalesMan.DataTextField = "SalesMan";
                ddlSalesMan.DataSource = ds.Tables[0];
                ddlSalesMan.DataBind();
                if (ddlSalesMan != null)
                {
                    ddlSalesMan.Items.Insert(0, "Select SalesMan");
                    ddlSalesMan.SelectedIndex = 0;
                }
               // sp_rptItemSold_getSalesMan
            }
            catch(Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                connection.Close();
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
                Session["rptItemSoldDateFrom"] = txtDateFrom.Text.ToString();
            }
            else
            {
                Session["rptItemSoldDateFrom"] = "";
            }

            if (txtDateTO.Text != "")
            {
                Session["rptItemSoldDateTo"] = txtDateTO.Text.ToString();
            }
            else
            {
                Session["rptItemSoldDateTo"] = "";
            }

            if (ddlInternalCustomer.SelectedItem.ToString().Equals("Exclude"))
            {
                Session["rptInternalCustomers"] = "Exclude";
            }
            else
            {
                Session["rptInternalCustomers"] = "Include";
            }

            if (ddlSalesMan.SelectedIndex>0)
            {
                Session["rptSalesManID"] = ddlSalesMan.SelectedValue.ToString();
                Session["rptSalesMan"] = ddlSalesMan.SelectedItem.ToString();
            }
            else
            {
                Session["rptSalesManID"] = 0;
                Session["rptSalesMan"] = "All";            
            }

            Response.Redirect(Session["Display"].ToString());
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {

            Session["rptProductID"] = "";

            Session["rptSubCategoryID"] = "";

            Session["rptCategoryID"] = "";

            Session["rptDepartmentID"] = "";

            Session["rptCustomerID"] = "";

            Session["rptItemSoldDateFrom"] = "";

            Session["rptItemSoldDateTo"] = "";


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