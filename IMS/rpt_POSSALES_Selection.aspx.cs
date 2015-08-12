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
using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;
using System.Drawing.Printing;

namespace IMS
{
    public partial class rpt_POSSALES_Selection : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public ReportDocument myReportDocument = new ReportDocument();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                
                // change for POSSALES Session
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

                Session["SP_Purchase"] = "POS";

                ddlProductType.Items.Clear();
                ddlProductType.Items.Add("Select Option");
                ddlProductType.Items.Add("HAAD Medicines");
                ddlProductType.Items.Add("NON-HAAD Medicines");
                ddlProductType.Items.Add("All Products");

                

            }
        }


        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID ;
            ProdID = DeptID = CatID = SubCatID = CustID = 0;
            String ProductType = "";
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("sp_rpt_POSSales", connection);
                command.CommandType = CommandType.StoredProcedure;

                #region Applying Filters

                if (Session["rptCustomerID"] != null && Session["rptCustomerID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCustomerID"].ToString(), out CustID))
                    {

                    }
                }

                if (Session["rptDepartmentID"] != null && Session["rptDepartmentID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptDepartmentID"].ToString(), out DeptID))
                    {

                    }
                }

                if (Session["rptCategoryID"] != null && Session["rptCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptCategoryID"].ToString(), out CatID))
                    {

                    }
                }

                if (Session["rptSubCategoryID"] != null && Session["rptSubCategoryID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptSubCategoryID"].ToString(), out SubCatID))
                    {

                    }
                }


                if (Session["rptProductID"] != null && Session["rptProductID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptProductID"].ToString(), out ProdID))
                    {

                    }
                }

                ProductType = ddlProductType.SelectedItem.ToString();

                #endregion

                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(command);
                dA.Fill(ds);

                if (Session["rptItemSoldDateFrom"] != null && Session["rptItemSoldDateFrom"].ToString() != "" &&
                    Session["rptItemSoldDateTo"] != null && Session["rptItemSoldDateTo"].ToString() != "")
                {
                    DateTime dtFROM = Convert.ToDateTime(Session["rptItemSoldDateFrom"]);
                    DateTime dtTo = Convert.ToDateTime(Session["rptItemSoldDateTo"]);

                    DataView dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "SaleDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND SaleDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";
                    DataTable dtfilterSet = dv.ToTable();
                    dv = dtfilterSet.DefaultView;


                    if (CustID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "SystemName = '" + CustID + "'";
                        dtfilterSet = dv.ToTable();

                    }

                    if (DeptID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "DeptID = '" + DeptID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (CatID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "CatID = '" + CatID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (SubCatID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "SubCategoryID = '" + SubCatID + "'";
                        dtfilterSet = dv.ToTable();

                    }


                    if (ProdID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "ProductID = '" + ProdID + "'";
                        dtfilterSet = dv.ToTable();

                    }



                    DataTable dtFiltered = dv.ToTable();
                    Session["dtPOSSALES"] = dtFiltered;
                }
                else
                {
                    Session["dtPOSSALES"] = ds.Tables[0];
                }
            }
            catch (Exception ex)
            {

            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }

            DataSet dS = new DataSet();
            dS.Tables.Add((DataTable)Session["dtPOSSALES"]);
            dS.AcceptChanges();

            myReportDocument.Load(Server.MapPath("~/rpt_POSSALES.rpt"));
            App_Code.Barcode dsReport = new App_Code.Barcode();
            dsReport.Tables[0].Merge((DataTable)Session["dtPOSSALES"]);
            myReportDocument.SetDataSource(dsReport.Tables[0]);

            myReportDocument.SetParameterValue("Product", Session["selectionProduct"].ToString());
            myReportDocument.SetParameterValue("Subcategory", Session["selectionSubCategory"].ToString());
            myReportDocument.SetParameterValue("Category", Session["selectionCategory"].ToString());
            myReportDocument.SetParameterValue("Department", Session["selectionDepartment"].ToString());
            myReportDocument.SetParameterValue("Customer", Session["selectionCustomers"].ToString());
            myReportDocument.SetParameterValue("FromDate", Session["rptItemSoldDateFrom"].ToString());
            myReportDocument.SetParameterValue("ToDate", Session["rptItemSoldDateTo"].ToString());
            myReportDocument.SetParameterValue("ProductType", Session["ProductType"].ToString());

            Session["ReportDocument"] = myReportDocument;
            Session["ReportPrinting_Redirection"] = "rpt_ItemSold_Selection.aspx";
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

            if (ddlProductType.SelectedItem.ToString().Contains("Select"))
            {
                Session["ProductType"] = "All";
            }
            else
            {
                Session["ProductType"] = ddlProductType.SelectedItem.ToString();
            }
            LoadData();
            Response.Redirect("CrystalReportViewer.aspx");
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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }

        protected void ddlInternalCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void btnSearchSalesMan_Click(object sender, EventArgs e)
        {

        }

        protected void ddlProductType_SelectedIndexChanged(object sender, EventArgs e)
        {

        }
    }
}