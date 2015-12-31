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
using System.Globalization;

namespace IMS
{
    public partial class rpt_NearestExpiry : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public ReportDocument myReportDocument = new ReportDocument();

        public void loadDeapartments()
        { 
       
             DataSet ds;
            ds = NearestExpiryBLL.LoadDepartments();
            drpDepartments.DataSource = ds;
            drpDepartments.DataValueField = "DepId";
            drpDepartments.DataTextField = "Name";
            drpDepartments.DataBind();
        }
        public void loadCategory()
        {

            DataSet ds;
            ds = NearestExpiryBLL.LoadCategory(Int32.Parse(drpDepartments.SelectedValue.ToString()));
            drpCatg.DataSource = ds;
            drpCatg.DataValueField = "CategoryID";
            drpCatg.DataTextField = "Name";
            drpCatg.DataBind();
        }

        public void loadSubCategory()
        {

            DataSet ds;
            ds = NearestExpiryBLL.LoadSubCategory(Int32.Parse(drpCatg.SelectedValue.ToString()));
            drpSubCatg.DataSource = ds;
            drpSubCatg.DataValueField = "Sub_CatID";
            drpSubCatg.DataTextField = "Name";
            drpSubCatg.DataBind();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                loadDeapartments();
                loadCategory();
                loadSubCategory();

                //if (Session["rptItemPurchaseDateF"] != null && Session["rptItemPurchaseDateF"].ToString() != "")
                //{
                //    txtDateFrom.Text = Session["rptItemPurchaseDateF"].ToString();
                //}
                //if (Session["rptItemPurchaseDateT"] != null && Session["rptItemPurchaseDateT"].ToString() != "")
                //{
                //    txtDateTO.Text = Session["rptItemPurchaseDateT"].ToString();
                //}

                //Session["rptProductID"] = null;

                //Session["rptSubCategoryID"] = null;

                //Session["rptCategoryID"] = null;

                //Session["rptDepartmentID"] = null;

                //Session["rptItemPurhcaseDateF"] = null;

                //Session["rptItemPurchaseDateT"] = null;

                //Session["SP_Purchase"] = "Expiry";

                txtDateFrom.Text = System.DateTime.Now.ToShortDateString();
                txtDateFrom.Enabled = false;
            }
        }

        //protected void btnCreateReport_Click(object sender, EventArgs e)
        //{
        //    if (txtProduct.Text != "")
        //    {
        //        Session["rptProductID"] = Session["rptProductID"];
        //        Session["selectionProduct"] = txtProduct.Text;

        //    }
        //    else
        //    {
        //        Session["rptProductID"] = "";
        //        Session["selectionProduct"] = "All";
        //    }

        //    if (txtSubcategory.Text != "")
        //    {
        //        Session["rptSubCategoryID"] = Session["rptSubCategoryID"];
        //        Session["selectionSubCategory"] = txtSubcategory.Text;
        //    }
        //    else
        //    {
        //        Session["rptSubCategoryID"] = "";
        //        Session["selectionSubCategory"] = "All";
        //    }

        //    if (txtCategory.Text != "")
        //    {
        //        Session["rptCategoryID"] = Session["rptCategoryID"];
        //        Session["selectionCategory"] = txtCategory.Text;
        //    }
        //    else
        //    {
        //        Session["rptCategoryID"] = "";
        //        Session["selectionCategory"] = "All";
        //    }

        //    if (txtDepartment.Text != "")
        //    {
        //        Session["rptDepartmentID"] = Session["rptDepartmentID"];
        //        Session["selectionDepartment"] = txtDepartment.Text;
        //    }
        //    else
        //    {
        //        Session["rptDepartmentID"] = "";
        //        Session["selectionDepartment"] = "All";
        //    }

              
            
        //    if (txtDateFrom.Text != "")
        //    {
        //        Session["rptItemPurchaseDateF"] = txtDateFrom.Text.ToString();
        //    }
        //    else
        //    {
        //        Session["rptItemPurchaseDateF"] = "";
        //    }

        //    if (txtDateTO.Text != "")
        //    {

        //        Session["rptItemPurchaseDateT"] = txtDateTO.Text.ToString();
        //    }
        //    else
        //    {
        //        DateTime dt = DateTime.Now.Date.AddMonths(6);
        //        //dt = dt.AddMonths(6);
        //        String ToDate = dt.ToShortDateString();
        //        Session["rptItemPurchaseDateT"] = ToDate;
        //    }

        //        Session["rptItemPurchaseExpiry"] = "";
            
        //        Session["rptBarterCustomers"] = "Exlude";

        //        Session["rptCustomerID"] = "";
        //        Session["selectionCustomers"] = "All";
            

        //    LoadData();
        //    Response.Redirect("CrystalReportViewer.aspx");
        //}

        //public void LoadData()
        //{
        //    int ProdID, DeptID, CatID, SubCatID, CustID, SalesID;
        //    ProdID = DeptID = CatID = SubCatID = CustID = SalesID = 0;
        //    String BarterValue = "";
        //    DateTime Expiry = new DateTime();
        //    try
        //    {
        //        connection.Open();

        //        SqlCommand command = new SqlCommand("sp_rptNearestExpiryItems", connection);
        //        command.CommandType = CommandType.StoredProcedure;

        //        #region Applying Filters

        //        BarterValue = Session["rptBarterCustomers"].ToString();

        //        if (Session["rptCustomerID"] != null && Session["rptCustomerID"].ToString() != "")
        //        {
        //            if (int.TryParse(Session["rptCustomerID"].ToString(), out CustID))
        //            {

        //            }
        //        }

        //        if (Session["rptDepartmentID"] != null && Session["rptDepartmentID"].ToString() != "")
        //        {
        //            if (int.TryParse(Session["rptDepartmentID"].ToString(), out DeptID))
        //            {

        //            }
        //        }

        //        if (Session["rptCategoryID"] != null && Session["rptCategoryID"].ToString() != "")
        //        {
        //            if (int.TryParse(Session["rptCategoryID"].ToString(), out CatID))
        //            {

        //            }
        //        }

        //        if (Session["rptSubCategoryID"] != null && Session["rptSubCategoryID"].ToString() != "")
        //        {
        //            if (int.TryParse(Session["rptSubCategoryID"].ToString(), out SubCatID))
        //            {

        //            }
        //        }


        //        if (Session["rptProductID"] != null && Session["rptProductID"].ToString() != "")
        //        {
        //            if (int.TryParse(Session["rptProductID"].ToString(), out ProdID))
        //            {

        //            }
        //        }

        //        if (Session["rptItemPurchaseExpiry"] != null && Session["rptItemPurchaseExpiry"].ToString() != "")
        //        {

        //            Expiry = Convert.ToDateTime(Session["rptItemPurchaseExpiry"].ToString());
        //        }


        //        #endregion

        //        DateTime dtFROM;
        //        DateTime dtTo;
        //        if (Session["rptItemPurchaseDateF"].ToString() != "" && Session["rptItemPurchaseDateT"].ToString() != "")
        //        {
        //            dtFROM = Convert.ToDateTime(Session["rptItemPurchaseDateF"].ToString());
        //            dtTo = Convert.ToDateTime(Session["rptItemPurchaseDateT"].ToString());


        //            command.Parameters.AddWithValue("@p_FromDate", dtFROM);
        //            command.Parameters.AddWithValue("@p_ToDate", dtTo);
        //        }
        //        else
        //        {
        //            command.Parameters.AddWithValue("@p_FromDate", DBNull.Value);
        //            command.Parameters.AddWithValue("@p_ToDate", DBNull.Value);
        //        }
        //        DataSet ds = new DataSet();
        //        SqlDataAdapter dA = new SqlDataAdapter(command);
                
        //        dA.Fill(ds);

        //        if ((Session["rptItemPurchaseDateF"] != null && Session["rptItemPurchaseDateF"].ToString() != "") &&
        //            (Session["rptItemPurchaseDateT"] != null && Session["rptItemPurchaseDateT"].ToString() != ""))
        //        {
        //            //DateTime dtFROM = Convert.ToDateTime(Session["rptItemPurchaseDateF"].ToString());
        //            //DateTime dtTo = Convert.ToDateTime(Session["rptItemPurchaseDateT"].ToString());

        //            DataView dv = ds.Tables[0].DefaultView;
        //            //dv.RowFilter = "OrderDate >= '" + dtFROM.ToString("yyyy-MM-dd") + "' AND OrderDate <= '" + dtTo.ToString("yyyy-MM-dd") + "'";

        //            DataTable dtfilterSet = dv.ToTable();
        //            dv = dtfilterSet.DefaultView;

        //            if (CustID != 0)
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "OrderRequestedFor = '" + CustID + "'";
        //                dtfilterSet = dv.ToTable();

        //            }

        //            if (BarterValue.Equals("Exclude"))
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "BarterExchangeID IS NULL";
        //                dtfilterSet = dv.ToTable();
        //            }


        //            if (DeptID != 0)
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "DeptID = '" + DeptID + "'";
        //                dtfilterSet = dv.ToTable();

        //            }


        //            if (CatID != 0)
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "CatID = '" + CatID + "'";
        //                dtfilterSet = dv.ToTable();

        //            }


        //            if (SubCatID != 0)
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "SubCategoryID = '" + SubCatID + "'";
        //                dtfilterSet = dv.ToTable();

        //            }


        //            if (ProdID != 0)
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "ProductID = '" + ProdID + "'";
        //                dtfilterSet = dv.ToTable();

        //            }


        //            if (Expiry != null && (Session["rptItemPurchaseExpiry"] != null && Session["rptItemPurchaseExpiry"].ToString() != ""))
        //            {
        //                dv = dtfilterSet.DefaultView;
        //                dv.RowFilter = "ExpiryDate = '" + Expiry.ToString("yyyy-MM-dd") + "'";
        //                dtfilterSet = dv.ToTable();
        //            }


        //            DataTable dtFiltered = dtfilterSet;
        //            Session["dtItemPurchased"] = dtFiltered;
        //        }
        //        else
        //        {
        //            Session["dtItemPurchased"] = ds.Tables[0];
        //        }
        //    }
        //    catch (Exception ex)
        //    {

        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();
        //    }

        //    //DataSet dS = new DataSet();
        //    //dS.Tables.Add((DataTable)Session["dtItemPurchased"]);
        //    //dS.AcceptChanges();

        //    myReportDocument.Load(Server.MapPath("~/NearestExpiryItems.rpt"));
        //    App_Code.Barcode dsReport = new App_Code.Barcode();
        //    dsReport.Tables[0].Merge((DataTable)Session["dtItemPurchased"]);
        //    myReportDocument.SetDataSource(dsReport.Tables[0]);

        //    myReportDocument.SetParameterValue("Product", Session["selectionProduct"].ToString());
        //    myReportDocument.SetParameterValue("Subcategory", Session["selectionSubCategory"].ToString());
        //    myReportDocument.SetParameterValue("Category", Session["selectionCategory"].ToString());
        //    myReportDocument.SetParameterValue("Department", Session["selectionDepartment"].ToString());
        //    myReportDocument.SetParameterValue("FromDate", Session["rptItemPurchaseDateF"].ToString());
        //    myReportDocument.SetParameterValue("ToDate", Session["rptItemPurchaseDateT"].ToString());

        //    Session["ReportDocument"] = myReportDocument;
        //    Session["ReportPrinting_Redirection"] = "rpt_NearestExpiry.aspx";

        //}
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

       

        //protected void btnSeachDepartment_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["SearchItemDept_RPT"] = txtDepartment.Text.ToString();
        //        DepartmentPopupGrid.LoadData();
        //       // mpeDepartmentDiv.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        //show message
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();

        //    }
        //}

        //protected void btnSearchCategory_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["SearchItemCat_RPT"] = txtCategory.Text.ToString();
        //        CategoryPopupGrid.LoadData();
        //        //mpeCategoryDiv.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        //show message
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();

        //    }
        //}

        //protected void btnSearchSubcat_Click(object sender, EventArgs e)
        //{
        //    try
        //    {
        //        Session["SearchItemSubCat_RPT"] = txtSubcategory.Text.ToString();
        //        SubCategoryPopupGrid.LoadData();
        //      //  mpeSubCategoryDiv.Show();
        //    }
        //    catch (Exception ex)
        //    {
        //        //show message
        //    }
        //    finally
        //    {
        //        if (connection.State == ConnectionState.Open)
        //            connection.Close();

        //    }
        //}

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

        protected void drpDepartments_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadCategory();
            loadSubCategory();
        }

        protected void drpCatg_SelectedIndexChanged(object sender, EventArgs e)
        {
            loadSubCategory();
        }

        Nullable<long> ProductId;
        Nullable<long> DepartmentId;
        Nullable<long> CatgeoryId;
        Nullable<long> SubCatgeoryId;
        protected void btnCreateReport_Click(object sender, EventArgs e)
        {
           
            string toDate=string.Empty;
            string fromDate = txtDateFrom.Text.ToString();

            int SysID = Convert.ToInt32(Session["UserSys"].ToString());
            if (drpDepartments.SelectedValue != "0") DepartmentId =long.Parse(drpDepartments.SelectedValue.ToString());
            if (drpCatg.SelectedValue != "0") CatgeoryId=long.Parse(drpCatg.SelectedValue.ToString());
            if (drpSubCatg.SelectedValue != "0") SubCatgeoryId = long.Parse(drpSubCatg.SelectedValue.ToString());
            if (txtProduct.Text.Trim() != "") ProductId = long.Parse(Session["rptProductID"].ToString());
            if (txtDateTO.Text.Trim() != "")
            {
               toDate= txtDateTO.Text.ToString();
            }
            else
            {
                DateTime dt = DateTime.Now.Date.AddMonths(6);
                toDate = dt.ToShortDateString();
               
            }




            DataSet ds;
            ds = NearestExpiryBLL.RptGetNearestExpiryItems(txtDateFrom.Text, toDate, SysID, ProductId, DepartmentId, CatgeoryId, SubCatgeoryId);


            myReportDocument.Load(Server.MapPath("~/NearestExpiryItems.rpt"));
            myReportDocument.SetDataSource(ds.Tables[0]);

            myReportDocument.SetParameterValue("ProductName", txtProduct.Text.Trim() == "" ? "All" : txtProduct.Text.Trim());
            myReportDocument.SetParameterValue("SubcatgName", drpSubCatg.SelectedValue.ToString() == "0" ? "All" : drpSubCatg.SelectedItem.ToString());
            myReportDocument.SetParameterValue("CatgName", drpCatg.SelectedValue.ToString() == "0" ? "All" : drpCatg.SelectedItem.ToString());
            myReportDocument.SetParameterValue("DeptName", drpDepartments.SelectedValue.ToString() == "0" ? "All" : drpDepartments.SelectedItem.ToString());
            myReportDocument.SetParameterValue("dtFrom", txtDateFrom.Text);
            myReportDocument.SetParameterValue("dtTo", toDate);
            
            Session["ReportDocument"] = myReportDocument;
            Session["ReportPrinting_Redirection"] = "rpt_NearestExpiry.aspx";
            Response.Redirect("CrystalReportViewer.aspx");

        }
    }
}