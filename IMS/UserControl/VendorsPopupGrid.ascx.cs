using AjaxControlToolkit;
using IMS.Util;
using IMSBusinessLogic;
using IMSCommon;
using log4net;
using System;
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
    public partial class VendorsPopupGrid : System.Web.UI.UserControl
    {
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        DataSet ds;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        bool selectSearch = false;
        string repVenID = string.Empty;

        public string RepVenID
        {
           // get { return repVenID; }
            set { repVenID = value; }
        }
        string repProdID = string.Empty;

        public string RepProdID
        {
           // get { return repProdID; }
            set { repProdID = value; }
        }
        public bool SelectSearch
        {
           // get { return selectSearch; }
            set { selectSearch = value; }
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
                    BindGrid();
                     if ((ViewState["displaySearch"] != null && ((bool)ViewState["displaySearch"]) == true)) 
                     {
                         lblSelectVendor.Visible = true;
                         txtVendor.Visible = true;
                         btnSearch.Visible = true;
                     }
                   
                }
                catch (Exception ex) 
                {
                    if (connection.State == ConnectionState.Open)
                        connection.Close();
                    throw ex;
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
        public void PopulateGrid()
        {
            if (Session["txtVendor"] != null)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("dbo.Sp_GetVendorByName", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        if (Session["txtVendor"] != null)
                        {
                            command.Parameters.AddWithValue("@p_Supp_Name", Session["txtVendor"].ToString());
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_Supp_Name", DBNull.Value);
                        }
                        command.Parameters.AddWithValue("@p_SysID", id);
                        if (!Session["UserRole"].ToString().Equals("Store"))
                        {
                            command.Parameters.AddWithValue("@p_isStore", false);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_isStore", true);
                        }
                        SqlDataAdapter SA = new SqlDataAdapter(command);

                        ProductSet = null;
                        SA.Fill(ds);
                        
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
                    connection.Close();
                }
                #endregion
            }
        }
        public void PopulateWithSearch() 
        {
            ViewState["displaySearch"] = true;
            ViewState["repVenID"]=repVenID;
            ViewState["repProdID"] = repProdID;
            lblSelectVendor.Visible = true;
            txtVendor.Visible = true;
            btnSearch.Visible = true;
            BindGrid();
        }

       
        private void BindGrid()
        {
            try
            {
                int id;
                DataSet ds1 = new DataSet();
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetVendorByName", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (Session["txtVendor"] != null)
                    {
                        command.Parameters.AddWithValue("@p_Supp_Name", Session["txtVendor"].ToString());
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_Supp_Name", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@p_SysID", id);
                    if (!Session["UserRole"].ToString().Equals("Store"))
                    {
                        command.Parameters.AddWithValue("@p_isStore", false);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_isStore", true);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    ProductSet = null;
                    SA.Fill(ds1);

                    ProductSet = ds1;
                    gdvVendor.DataSource = null;
                    gdvVendor.DataSource = ds1;
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
        }

        protected void gdvVendor_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void gdvVendor_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gdvVendor_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gdvVendor.PageIndex = e.NewPageIndex;
            if (Session["txtVendor"] != null)
            {
                PopulateGrid();
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
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
             
        }

        protected void SelectVendor_Click(object sender, EventArgs e)
        {
            try
            {
                GridViewRow rows = gdvVendor.SelectedRow;
                foreach (GridViewRow row in gdvVendor.Rows)
                {
                    if (row.RowType == DataControlRowType.DataRow)
                    {
                        CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                        if (chkRow.Checked)
                        {
                            Control ctl = this.Parent;
                            if ((ViewState["displaySearch"] != null && ((bool)ViewState["displaySearch"]) == true))
                            {

                                // string val=  ;
                                Label id = row.Cells[8].FindControl("lblSupID") as Label;
                                //string val2= id.Text;
                                //    Session["Rep_Params"] = val+"~"+val2;
                                #region change logic

                                String NewVendorName = Server.HtmlDecode(row.Cells[1].Text);
                                String NewVendorID = id.Text;

                                int NewVendor = 0;
                                int.TryParse(NewVendorID, out NewVendor);

                                String lblProductID = ViewState["repProdID"].ToString(); //
                                String lblVendorID = ViewState["repVenID"].ToString();// e.CommandArgument.ToString().Split(',')[1];
                                int ProductID = 0;
                                int MainVendorID = 0;

                                int.TryParse(lblProductID, out ProductID);
                                int.TryParse(lblVendorID, out MainVendorID);


                                DataTable dtChanged = (DataTable)Session["DataTableView"];
                                for (int i = 0; i < dtChanged.Rows.Count; i++)
                                {
                                    int Product = 0;
                                    int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                                    int Vendor = 0;
                                    int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                                    if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                                    {
                                        dtChanged.Rows[i]["VendorID"] = NewVendorID;
                                        dtChanged.Rows[i]["VendorName"] = NewVendorName;
                                        dtChanged.AcceptChanges();
                                    }
                                }
                                Session["DataTableView"] = dtChanged;
                                //calling method
                                ReplenishMovement p = Page as ReplenishMovement;
                                if (p != null)
                                    p.DisplayMainGrid((DataTable)Session["DataTableView"]);

                                ViewState.Remove("repProdID");
                                ViewState.Remove("repVenID");
                                ViewState.Remove("displaySearch");
                                #endregion
                            }
                            else
                            {
                                TextBox ltMetaTags = null;
                                Button btnContinue = (Button)ctl.FindControl("btnContinue");
                                // Label lblVendirId = (Label)
                                btnContinue.Visible = true;
                                ltMetaTags = (TextBox)ctl.FindControl("txtVendor");
                                if (ltMetaTags != null)
                                {
                                    ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);
                                }
                            }
                        }
                    }
                }

                Session.Remove("txtVendor");
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

        protected void chkCtrl_CheckedChanged(object sender, EventArgs e)
        {
            if(sender is CheckBox)
            {

            }
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            try
            {
                DataSet ds = new DataSet();
                int id;
                if (int.TryParse(Session["UserSys"].ToString(), out id))
                {
                    if (connection.State == ConnectionState.Closed)
                    {
                        connection.Open();
                    }
                    SqlCommand command = new SqlCommand("dbo.Sp_GetVendorByName", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    if (txtVendor.Text != null)
                    {
                        command.Parameters.AddWithValue("@p_Supp_Name", txtVendor.Text);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_Supp_Name", DBNull.Value);
                    }
                    command.Parameters.AddWithValue("@p_SysID", id);
                    if (!Session["UserRole"].ToString().Equals("Store"))
                    {
                        command.Parameters.AddWithValue("@p_isStore", false);
                    }
                    else
                    {
                        command.Parameters.AddWithValue("@p_isStore", true);
                    }
                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    ProductSet = null;
                    SA.Fill(ds);

                    ProductSet = ds;
                    gdvVendor.DataSource = null;
                    gdvVendor.DataSource = ds;
                    gdvVendor.DataBind();

                    ModalPopupExtender mpe = (ModalPopupExtender)this.Parent.FindControl("mpeCongratsMessageDiv");
                    mpe.Show();
                }
            }
            catch (Exception ex) 
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
        }

      
        
    }
}