using AjaxControlToolkit;
using IMSBusinessLogic;
using IMSCommon;
using IMSCommon.Util;
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

namespace IMS.UserControl
{

    public partial class MultipleVendorsSelectPopup : System.Web.UI.UserControl
    {
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
                catch (Exception exp) { }
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
        }

        public void PopulateforAssociation() 
        {
            ViewState["checkAllState"] = true; 
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
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        String Query = "Select * FROM tblVendor Where  SupName Like '" + Session["txtVendor"].ToString() + "'";

                        connection.Open();
                        SqlCommand command = new SqlCommand(Query, connection);
                        SqlDataAdapter SA = new SqlDataAdapter(command);
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
                }
                finally
                {
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
            ds = VendorBLL.GetAllVendors(connection);
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

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
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

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            SaveCurrentState();
            gdvVendor.PageIndex = e.NewPageIndex;
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
                ds = VendorBLL.GetDistinct(connection, vendor);

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
            catch (Exception exp) { }

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
                    SaveCurrentState();
                    if (ViewState["CheckBoxArray"] != null)
                    {
                        int StoreId = Convert.ToInt32(Session["SystemId"].ToString());
                        Control ctl = this.Parent;
                        Label lbstoreId = (Label)ctl.FindControl("lblStoreId");
                        ArrayList CheckBoxArray = (ArrayList)ViewState["CheckBoxArray"];

                        foreach (var item in CheckBoxArray)
                        {
                            if (item.ToString().Contains("ID"))
                            {
                                string[] key = item.ToString().Split('-');
                                try
                                {
                                    if (connection.State == ConnectionState.Closed)
                                    {
                                        connection.Open();
                                    }
                                    SqlCommand command = new SqlCommand("sp_AddVendorsToStore", connection);
                                    command.CommandType = CommandType.StoredProcedure;

                                    command.Parameters.AddWithValue("@VendorId", int.Parse(key[1]));
                                    command.Parameters.AddWithValue("@StoreId", StoreId);

                                    command.ExecuteNonQuery();
                                    connection.Close();

                                }
                                catch (Exception ex)
                                {
                                    connection.Close();
                                }

                            }
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

            }
            finally
            {
                connection.Close();

            }
        }

        public void BindAssociatedVendorGrid()
        {
            try
            {
                if (!String.IsNullOrEmpty(txtSearch.Value))
                {
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

                    PopulatePreviousState();

                    if ((ViewState["checkAllState"] != null && ((bool)ViewState["checkAllState"]) == true) || selectAll == true)
                    {
                        ((CheckBox)gdvVendor.HeaderRow.FindControl("chkboxSelectAll")).Enabled = true;
                        ViewState["checkAllState"] = true;
                    }
                }
            }
            catch
            {

            }
            finally
            {
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
            catch (Exception exp) { }
        }
    }
}