using AjaxControlToolkit;
using IMS_WHReports.Util;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS_WHReports.UserControl
{

    public partial class MultipleVendorsSelectPopup : System.Web.UI.UserControl
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        bool selectAll = false;
        public int Storeid;
        bool selectSearch = false;

        public bool StoreAssociation
        {
           set 
           {
               ViewState["selectSearch"] = value;
              
              
           }
        }
        public bool SelectAll
        {
            // get { return selectAll; }
            set { selectAll = value; }
        }

        
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

            if (!IsPostBack)
            {
                try
                {
                   // BindGrid();
                    if (selectAll)
                    {
                       // ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }
                    if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true) 
                    {
                        lblSelectVendor.Visible = true;
                        btnSearchStore.Visible = true;
                        txtSearch.Visible = true;
                    }
                }
                catch (Exception ex) 
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                }
            }
            if (IsPostBack)
            {
                if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                {
                     //((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                }
                if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true)
                {
                    lblSelectVendor.Visible = true;
                    btnSearchStore.Visible = true;
                    txtSearch.Visible = true;
                }
            }
            expHandler.CheckForErrorMessage(Session);
        }

        private void Page_Error(object sender, EventArgs e)
        {
            Exception exc = Server.GetLastError();
            // Void Page_Load(System.Object, System.EventArgs)
            // Handle specific exception.
            if (exc is HttpUnhandledException || exc.TargetSite.Name.ToLower().Contains("page_load"))
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.Remote, Session, Server, Response, log, exc);
            }
            else
            {
                expHandler.GenerateExpResponse(pageURL, RedirectionStrategy.local, Session, Server, Response, log, exc);
            }
            // Clear the error from the server.
            Server.ClearError();
        }

        public void PopulateforAssociation() 
        {
            ViewState["checkAllState"] = true;
            ViewState["first"] = true;
            lblSelectVendor.Visible = true;
            btnSearchStore.Visible = true;
            txtSearch.Visible = true;
            gdvVendor.DataSource = null;
            gdvVendor.DataBind();
        }
        public void PopulateGrid()
        {
            if (Session["txtVendor"].ToString() != "%")
            {
                ViewState["first"] = false;
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        //String Query = "Select * FROM tblVendor Where  SupName Like '" + Session["txtVendor"].ToString() + "'";

                        if (connection.State == ConnectionState.Closed)
                            connection.Open();
                        SqlCommand command = new SqlCommand("sp_GetVendor_byNameParam", connection);
                        command.Parameters.AddWithValue("@p_VendName", Session["txtVendor"].ToString());
                        SqlDataAdapter SA = new SqlDataAdapter(command);
                        command.CommandType = CommandType.StoredProcedure;
                        ProductSet = null;
                        SA.Fill(ds);
                        PopulatePreviousState();

                        if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                        {
                            // ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                            ViewState["checkAllState"] = true;
                        }

                        gdvVendor.DataSource = ds;
                        gdvVendor.DataBind();
                    }

                }
                catch (Exception ex)
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
                }
                finally
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();

                }
                #endregion
            }
            else
            {
                BindGrid();
            }
        }
        private void BindGrid()
        {
            try
            {
                ViewState["first"] = false;
                ds = VendorBLL.GetAllVendors();
                ProductSet = ds;
                gdvVendor.DataSource = null;
                gdvVendor.DataSource = ds;
                gdvVendor.DataBind();


                PopulatePreviousState();

                if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                {
                    ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                    ViewState["checkAllState"] = true;
                }
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                if (e.Row.RowType == DataControlRowType.DataRow && CheckBoxArray != null)
                {
                    if (((bool)ViewState["SelectAllChecked"]) == true)
                    {
                        //CheckBox chkAll = (CheckBox)gdvVendor.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                        //chkAll.Checked = true;

                        CheckBox chk = (CheckBox)e.Row.FindControl("chkCtrl");
                        chk.Checked = true;
                    }
                    else
                    {
                        CheckBox chk = (CheckBox)e.Row.FindControl("chkCtrl");
                        chk.Checked = false;
                    }
                }
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            if ((bool)ViewState["first"] == true)
            {
                CheckBox chkAll = (CheckBox)gdvVendor.HeaderRow.Cells[0].FindControl("chkboxSelectAll");

                if (chkAll.Checked)
                {
                    selectAllProducts();
                    ViewState["first"] = false;
                }
                else
                {
                    ViewState["first"] = false;
                }
            }

            SaveCurrentState();
            gdvVendor.PageIndex = e.NewPageIndex;

            if (gdvVendor.PageIndex == 0)
            {
                if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
                {
                   
                    ViewState["first"] = true;
                }
            }

            if (Session["txtVendor"] != null)
            {
                PopulateGrid();
            }
            else if (ViewState["selectSearch"] != null && ((bool)ViewState["selectSearch"]) == true)
            {
                BindAssociatedVendorGrid();
            }
            else
            {
                BindGrid();
            }
            ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
            mpe.Show();

        }

        protected void gdvVendor_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            VendorBLL _vendorBll = new VendorBLL();
            if (e.CommandName.Equals("CheckChanged"))
            {
                Label ID = (Label)gdvVendor.Rows[0].FindControl("lblSupID");
                int SupID = int.Parse(ID.Text);
                if (SupID > 0)
                {

                }

            }
        }

        protected void gdvVendor_RowEditing(object sender, GridViewEditEventArgs e)
        {
            //gdvVendor.EditIndex = -1;
            //BindGrid();
        }

        protected void gdvVendor_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                Label ID = (Label)gdvVendor.Rows[e.RowIndex].FindControl("lblSupID");
                int id = int.Parse(ID.Text);
                Vendor vendor = new Vendor();//= empid.Text;
                vendor.supp_ID = id;
                ds = VendorBLL.GetDistinct(vendor);

                Session["VendorName"] = ds.Tables[0].Rows[0]["SupName"];
                Session["VendorId"] = ds.Tables[0].Rows[0]["SuppID"];

                Control ctl = this.Parent;
                TextBox ltMetaTags = null;
                ltMetaTags = (TextBox)ctl.FindControl("txtVendor");
                if (ltMetaTags != null)
                {
                    ltMetaTags.Text = ds.Tables[0].Rows[0]["SupName"].ToString();
                }
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }

        }

        private void SaveCurrentState()
        {

            ArrayList CheckBoxArray;
            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
            {
                 ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                ViewState["checkAllState"] = true;

                if (ViewState["CheckBoxArray"] != null)
                {
                    CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                }
                else
                {
                    CheckBoxArray = new ArrayList();
                }


                int CheckBoxIndex;
                bool CheckAllWasChecked = false;
                CheckBox chkAll = (CheckBox)gdvVendor.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                string checkAllIndex = "chkAll-" + gdvVendor.PageIndex;
                if (chkAll.Checked)
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                    ViewState["SelectAllChecked"] = true;
                }
                else
                {
                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBoxArray.Remove(checkAllIndex);
                        CheckAllWasChecked = true;
                    }
                    ViewState["SelectAllChecked"] = false;
                }
                for (int i = 0; i < gdvVendor.Rows.Count; i++)
                {
                    if (gdvVendor.Rows[i].RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chk = (CheckBox)gdvVendor.Rows[i].Cells[0].FindControl("chkCtrl");
                        Label lblSupID = (Label)gdvVendor.Rows[i].FindControl("lblSupID");

                        CheckBoxIndex = gdvVendor.PageSize * gdvVendor.PageIndex + (i + 1);
                        if (chk.Checked)
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) == -1 && !CheckAllWasChecked)
                            {
                                string ProdID = "ID " + CheckBoxIndex + "-" + lblSupID.Text;
                                CheckBoxArray.Add(CheckBoxIndex);
                                CheckBoxArray.Add(ProdID);
                            }
                        }
                        else
                        {
                            if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1 || CheckAllWasChecked)
                            {
                                string ProdID = "ID " + CheckBoxIndex + "-" + lblSupID.Text;
                                CheckBoxArray.Remove(CheckBoxIndex);
                                CheckBoxArray.Remove(ProdID);
                            }
                        }
                    }
                }
                ViewState["CheckBoxArray"] = CheckBoxArray;
            }
        }

        public void selectAllProducts()
        {
            DataSet productSet = (DataSet)Session["dsVendors"];
            ArrayList CheckBoxArray;
            if (ViewState["CheckBoxArray"] != null)
            {
                CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
            }
            else
            {
                CheckBoxArray = new ArrayList();
            }

            int checkBoxIndex = -1;
            int pageIndex = 0;
            int paginationCount = productSet.Tables[0].Rows.Count / 10; // 1 page  contains 10 rows, hence divided by 10

            // adding page index for the first time.
            string checkAllIndex;//= "chkAll-" + pageIndex;
            //if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
            //{
            //    CheckBoxArray.Add(checkAllIndex);
            //}

            for (int i = 0; i < productSet.Tables[0].Rows.Count; i++)
            {
                if (i % 10 == 0)
                {

                    checkAllIndex = "chkAll-" + pageIndex;
                    if (CheckBoxArray.IndexOf(checkAllIndex) == -1)
                    {
                        CheckBoxArray.Add(checkAllIndex);
                    }
                    pageIndex++;
                }


                // the checkbox index must range between 0-9
                if (i > 9)
                {
                    checkBoxIndex = ((i % 10));
                }
                else
                {
                    checkBoxIndex++;
                }

                string ProdID = "ID " + checkBoxIndex + "-" + productSet.Tables[0].Rows[i]["SuppID"].ToString();

                if (!CheckBoxArray.Contains(ProdID))
                {
                    CheckBoxArray.Add(checkBoxIndex);
                    CheckBoxArray.Add(ProdID);
                }

            }

            ViewState["CheckBoxArray"] = CheckBoxArray;

        }

        private void PopulatePreviousState()
        {
            if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
            {
                if (ViewState["CheckBoxArray"] != null)
                {
                    ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                    string checkAllIndex = "chkAll-" + gdvVendor.PageIndex;

                    if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                    {
                        CheckBox chkAll = (CheckBox)gdvVendor.HeaderRow.Cells[0].FindControl("chkboxSelectAll");
                        chkAll.Checked = true;
                    }
                    for (int i = 0; i < gdvVendor.Rows.Count; i++)
                    {

                        if (gdvVendor.Rows[i].RowType == DataControlRowType.DataRow)
                        {
                            if (CheckBoxArray.IndexOf(checkAllIndex) != -1)
                            {
                                CheckBox chk = (CheckBox)gdvVendor.Rows[i].Cells[0].FindControl("chkCtrl");
                                chk.Checked = true;

                            }
                            else
                            {
                                int CheckBoxIndex = gdvVendor.PageSize * (gdvVendor.PageIndex) + (i + 1);
                                if (CheckBoxArray.IndexOf(CheckBoxIndex) != -1)
                                {
                                    CheckBox chk = (CheckBox)gdvVendor.Rows[i].Cells[0].FindControl("chkCtrl");
                                    chk.Checked = true;

                                }
                            }
                        }
                    }
                }
            }
        }

        protected void SelectMultipleVendor_Click(object sender, EventArgs e)
        {
            try
            {
                if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true))
                {
                    if ((bool)ViewState["first"] == true)
                    {

                        CheckBox chkAll = (CheckBox)gdvVendor.HeaderRow.Cells[0].FindControl("chkboxSelectAll");

                        if (chkAll.Checked)
                        {
                            selectAllProducts();
                            ViewState["first"] = false;
                        }
                        else
                        {
                            ViewState["first"] = false;
                        }
                    }

                    SaveCurrentState();
                    if (ViewState["CheckBoxArray"] != null)
                    {
                        int StoreId = Convert.ToInt32(Session["SystemId"].ToString());
                        Control ctl = this.Parent;
                        Label lbstoreId = (Label)ctl.FindControl("lblStoreId");
                        ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];
                        
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        try
                        {
                            foreach (var item in CheckBoxArray)
                            {
                                if (item.ToString().Contains("ID"))
                                {
                                    string[] key = item.ToString().Split('-');
                                    try
                                    {

                                        SqlCommand command = new SqlCommand("sp_AddVendorsToStore", connection);
                                        command.CommandType = CommandType.StoredProcedure;

                                        command.Parameters.AddWithValue("@VendorId", int.Parse(key[1]));
                                        command.Parameters.AddWithValue("@StoreId", StoreId);

                                        command.ExecuteNonQuery();


                                    }
                                    catch (Exception ex)
                                    {
                                        string message = expHandler.GenerateLogString(ex);
                                        log.Error(message);
                                    }


                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();
                            throw ex;
                        }
                        finally
                        {
                            if (connection.State == ConnectionState.Open)
                                connection.Close();

                        }

                        WebMessageBoxUtil.Show("Vendors have been successfully associated ");
                        GridView gvParentVendors = (GridView)ctl.FindControl("dgvVendors");
                        DataSet resultSet = new DataSet();

                        SqlCommand comm = new SqlCommand("sp_GetStoredVendors", connection);
                        comm.CommandType = CommandType.StoredProcedure;
                        comm.Parameters.AddWithValue("@StoreId", StoreId);
                        SqlDataAdapter SA = new SqlDataAdapter(comm);
                        SA.Fill(resultSet);

                        gvParentVendors.DataSource = resultSet;
                        gvParentVendors.DataBind();

                        if (connection.State == ConnectionState.Open)
                            connection.Close();
                        gvParentVendors.Visible = true;
                    }
                }
                

                    //    Control ctl2 = this.Parent;
                    //    TextBox ltMetaTags = null;

                    //    Label lblVendorIds = (Label)ctl2.FindControl("lblVendorIds");

                    //    ltMetaTags = (TextBox)ctl2.FindControl("txtVendor");

                    //    GridViewRow rows = gdvVendor.SelectedRow;
                    //    foreach (GridViewRow row in gdvVendor.Rows)
                    //    {
                    //        if (row.RowType == DataControlRowType.DataRow)
                    //        {
                    //            CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    //            if (chkRow.Checked)
                    //            {
                    //                lblVendorIds.Text = lblVendorIds.Text + row.Cells[8].Text + ",";

                    //            }
                    //        }
                    //    }
                    //    if (Session["SystemId"] != null)
                    //    {
                    //        int StoreID = Convert.ToInt32(Session["SystemId"].ToString());
                    //        string[] strVendorIds = lblVendorIds.Text.Split(',');
                    //        foreach (string strVenId in strVendorIds)
                    //        {
                    //            int VendorId = strVenId != "" ? int.Parse(strVenId) : 0;
                    //            if (VendorId > 0)
                    //            {
                    //                connection.Open();
                    //                SqlCommand command = new SqlCommand("sp_AddVendorsToStore", connection);
                    //                command.Parameters.AddWithValue("@VendorId", VendorId);
                    //                command.Parameters.AddWithValue("@StoreId", StoreID);

                    //                command.CommandType = CommandType.StoredProcedure;
                    //                command.ExecuteNonQuery();
                    //                connection.Close();
                    //            }
                    //        }

                    //    }
                    //}
                    //catch (Exception ex)
                    //{

                    //}
                    //finally
                    //{
                    //    connection.Close();

                    //}
                }
          
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }

        public void BindAssociatedVendorGrid()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtSearch.Value))
                {
                    if (connection.State == ConnectionState.Closed)
                        connection.Open();
                    DataSet resultSet = new DataSet();

                    SqlCommand comm = new SqlCommand("Sp_GetStoreVendorsByName", connection);
                    comm.CommandType = CommandType.StoredProcedure;
                    if (String.IsNullOrEmpty(txtSearch.Value))
                    {
                        comm.Parameters.AddWithValue("@p_storeName", DBNull.Value);
                    }
                    else 
                    {
                        comm.Parameters.AddWithValue("@p_storeName", txtSearch.Value);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(comm);
                    SA.Fill(resultSet);

                    gdvVendor.DataSource = resultSet;
                    gdvVendor.DataBind();
                    Session["dsVendors"] = resultSet;
                    PopulatePreviousState();

                    if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                    {
                        ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }
                }
            }
            catch(Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
             
        }

        protected void btnSearchStore_Click(object sender, EventArgs e)
        {
            try
            {
                BindAssociatedVendorGrid();
                ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
                mpe.Show();
            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
        }
    }
}