using IMS.Util;
using IMSCommon.Util;
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

namespace IMS
{
    public partial class AddVendorsToStore : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            try
            {
                System.Uri url = Request.Url;
                pageURL = url.AbsolutePath.ToString();
                log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
                if (!IsPostBack)
                {
                    if (Session["Storename"] != null)
                    {
                        spnStoreName.InnerHtml = Session["Storename"].ToString();
                        BindGrid();
                        PopulateStoreDropDown();
                    }
                }
                expHandler.CheckForErrorMessage(Session);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            
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

        private void PopulateStoreDropDown()
        {
            //#region Populating Product Name Dropdown

            //try
            //{
            //    if (connection.State == ConnectionState.Closed)
            //    {
            //        connection.Open();
            //    }

            //    SqlCommand command = new SqlCommand("sp_getOutsideStoresHavingVendors", connection); 
            //    DataSet ds = new DataSet();
            //    SqlDataAdapter sA = new SqlDataAdapter(command);
            //    sA.Fill(ds);
            //    if (ds.Tables[0].Rows.Count.Equals(0))
            //    {
                    
            //    }
            //    if (ddlStoreVendors.DataSource != null)
            //    {
            //        ddlStoreVendors.DataSource = null;
            //    }

            //    ProductSet = null;
            //    ProductSet = ds;

            //    ddlStoreVendors.DataSource = ds.Tables[0];
            //    ddlStoreVendors.DataTextField = "SystemName";
            //    ddlStoreVendors.DataValueField = "SystemID";
            //    ddlStoreVendors.DataBind();
            //    if (ddlStoreVendors != null)
            //    {
            //        ddlStoreVendors.Items.Insert(0, "Select Vendor");
            //        ddlStoreVendors.SelectedIndex = 0;
            //    }
            //}
            //catch (Exception ex)
            //{

            //}
            //finally
            //{
            //    connection.Close();
            //}
            //#endregion
        }

        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Session.Remove("SystemId");
            Session.Remove("Storename");
            Response.Redirect("SelectStores.aspx");
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            String Text = txtVendor.Text + '%';
            Session["txtVendor"] = Text;
            MultipleVendorsSelectPopup.SelectAll = true;
            MultipleVendorsSelectPopup.PopulateGrid();
             
            mpeCongratsMessageDiv.Show();
        }

        protected void dgvVendors_RowCommand(object sender, GridViewCommandEventArgs e)
        {
           
        }

        private void BindGrid()
        {
            try
            {
                DataSet resultSet = new DataSet();
                int StoreId = int.Parse(Session["SystemId"].ToString());
                SqlCommand comm = new SqlCommand("sp_GetStoredVendors", connection);
                comm.CommandType = CommandType.StoredProcedure;
                comm.Parameters.AddWithValue("@StoreId", StoreId);
                SqlDataAdapter SA = new SqlDataAdapter(comm);
                SA.Fill(resultSet);

                dgvVendors.DataSource = resultSet;
                dgvVendors.DataBind();
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
        protected void dgvVendors_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            try
            {
                int VendorId = int.Parse(((Label)dgvVendors.Rows[e.RowIndex].FindControl("lblSupID")).Text);
                int StoreId = int.Parse(((Label)dgvVendors.Rows[e.RowIndex].FindControl("lblStoreID")).Text);
                connection.Open();

                SqlCommand command3 = new SqlCommand("sp_DeleteFromtblVendors_Store", connection);
                command3.CommandType = CommandType.StoredProcedure;
                command3.Parameters.AddWithValue("@VendorID", VendorId);
                command3.Parameters.AddWithValue("@StoreID", StoreId);
                command3.ExecuteNonQuery();

                BindGrid();

                connection.Close();
                dgvVendors.Visible = true;
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

        protected void dgvVendors_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvVendors.PageIndex = e.NewPageIndex;

            BindGrid();

        }

        protected void ddlStoreVendors_SelectedIndexChanged(object sender, EventArgs e)
        {
            
        }

        protected void btnShow_Click(object sender, EventArgs e)
        {

            MultipleVendorsSelectPopup.SelectAll = true;
            MultipleVendorsSelectPopup.StoreAssociation = true;
            MultipleVendorsSelectPopup.PopulateforAssociation();
            mpeCongratsMessageDiv.Show();
            //int StoreId = int.Parse(ddlStoreVendors.SelectedValue.ToString());
            //if (StoreId > 0)
            //{
            //    MultipleVendorsSelectPopup.SelectAll = true;
            //    MultipleVendorsSelectPopup.Storeid = StoreId;
            //    MultipleVendorsSelectPopup.BindAssociatedVendorGrid(StoreId);

            //    mpeCongratsMessageDiv.Show();
            //}
             
        }
         
    }
}