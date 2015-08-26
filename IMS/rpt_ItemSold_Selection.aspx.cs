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
    public partial class rpt_ItemSold_Selection : System.Web.UI.Page
    {
        public DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public ReportDocument myReportDocument = new ReportDocument();
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
                    else if (Parameter.ToLower().Equals("Profitability".ToLower())) 
                    {
                        ViewState["isProfReport"] = true;
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

                Session["rptBarterCustomers"] = null;

                Session["rptSalesDateFrom"] = null;

                Session["rptSalesDateTo"] = null;

                Session["SP_Purchase"] = "NO";

                ddlInternalCustomer.Items.Clear();
                ddlInternalCustomer.Items.Add("Select Option");
                ddlInternalCustomer.Items.Add("Include");
                ddlInternalCustomer.Items.Add("Exclude");

                ddlBarterCustomer.Items.Clear();
                ddlBarterCustomer.Items.Add("Select Option");
                ddlBarterCustomer.Items.Add("Include");
                ddlBarterCustomer.Items.Add("Exclude");

                LoadSalesMan();

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
                if (connection.State == ConnectionState.Open)
                    connection.Close();
            }
        }

        public void LoadData()
        {
            int ProdID, DeptID, CatID, SubCatID, CustID, SalesID;
            ProdID = DeptID = CatID = SubCatID = CustID = SalesID = 0;
            String BarterValue = "";
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand("sp_rpt_ItemSold", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@InternalCustomer", Session["rptInternalCustomers"].ToString());

                #region Applying Filters

                if (Session["rptSalesManID"] != null && Session["rptSalesManID"].ToString() != "")
                {
                    if (int.TryParse(Session["rptSalesManID"].ToString(), out SalesID))
                    {

                    }
                }

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

                BarterValue = Session["rptBarterCustomers"].ToString();
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
                    dv.RowFilter = "OrderDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND OrderDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";
                    DataTable dtfilterSet = dv.ToTable();
                    dv = dtfilterSet.DefaultView;

                    if (SalesID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "SalesMan = '" + SalesID + "'";
                        dtfilterSet = dv.ToTable();
                    }

                    if (CustID != 0)
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "OrderRequestedFor = '" + CustID + "'";
                        dtfilterSet = dv.ToTable();

                    }

                    if (BarterValue.Equals("Exclude"))
                    {
                        dv = dtfilterSet.DefaultView;
                        dv.RowFilter = "BarterExchangeID IS NULL";
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
                    Session["dtItemSoldDate"] = dtFiltered;
                }
                else
                {
                    Session["dtItemSoldDate"] = ds.Tables[0];
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
            dS.Tables.Add((DataTable)Session["dtItemSoldDate"]);
            dS.AcceptChanges();

            myReportDocument.Load(Server.MapPath("~/ItemSaleDetailReport.rpt"));
            App_Code.Barcode dsReport = new App_Code.Barcode();
            dsReport.Tables[0].Merge((DataTable)Session["dtItemSoldDate"]);
            myReportDocument.SetDataSource(dsReport.Tables[0]);

            myReportDocument.SetParameterValue("Product", Session["selectionProduct"].ToString());
            myReportDocument.SetParameterValue("Subcategory", Session["selectionSubCategory"].ToString());
            myReportDocument.SetParameterValue("Category", Session["selectionCategory"].ToString());
            myReportDocument.SetParameterValue("Department", Session["selectionDepartment"].ToString());
            myReportDocument.SetParameterValue("Customer", Session["selectionCustomers"].ToString());
            myReportDocument.SetParameterValue("FromDate", Session["rptItemSoldDateFrom"].ToString());
            myReportDocument.SetParameterValue("ToDate", Session["rptItemSoldDateTo"].ToString());
            myReportDocument.SetParameterValue("BarterCustomer", Session["rptBarterCustomers"].ToString());
            myReportDocument.SetParameterValue("SalesMan", Session["rptSalesMan"].ToString());

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

            LoadData();
            Response.Redirect("CrystalReportViewer.aspx");
            //Response.Redirect(Session["Display"].ToString());
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
    }
}