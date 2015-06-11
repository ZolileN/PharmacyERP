using IMSCommon.Util;
using iTextSharp.text;
using iTextSharp.text.html.simpleparser;
using iTextSharp.text.pdf;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace IMS
{
    public partial class ReplenishMovement : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!IsPostBack)
            {
                try
                {
                    LoadData();
                    ViewState["VendorID"] = 0;
                    DisplayMainGrid((DataTable)Session["DataTableView"]);
                  
                }
                catch(Exception ex)
                {
                    WebMessageBoxUtil.Show(ex.Message);
                }
            }
        }
        public void DisplayMainGrid(DataTable dt)
        {
            DataTable displayTable = new DataTable();
            displayTable.Clear();
            displayTable.Columns.Add("VendorID", typeof(int));
            displayTable.Columns.Add("VendorName", typeof(String));

            for(int i=0;i<dt.Rows.Count;i++)
            {
                int VendorID = Convert.ToInt32(dt.Rows[i]["VendorID"].ToString());
                String VendorName = dt.Rows[i]["VendorName"].ToString();
                displayTable.Rows.Add(VendorID, VendorName);
                displayTable.AcceptChanges();
            }

            DataView dv = displayTable.DefaultView;
            displayTable = null;
            displayTable = dv.ToTable(true, "VendorID", "VendorName");

            gvVendorNames.DataSource = displayTable;
            gvVendorNames.DataBind();
        }

        public void LoadData()
        {
            int SystemID = 0;
            int.TryParse(Session["UserSys"].ToString(), out SystemID);

            int VendorID = 0;
            int.TryParse(Session["ReplenishVendorID"].ToString(), out VendorID);

            try
            {
                connection.Open();
                SqlCommand cmd = new SqlCommand("sp_ReplenishProductSet_ByDate", connection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@p_SystemID", SystemID);
                DataSet ds = new DataSet();
                SqlDataAdapter dA = new SqlDataAdapter(cmd);
                dA.Fill(ds);

                DataTable dt = new DataTable();
                DataView dv = new DataView();
                if(VendorID >0)
                {
                    dv = ds.Tables[0].DefaultView;
                    dv.RowFilter = "VendorID = '"+VendorID+"'";
                    dt = dv.ToTable();
                }
                else
                {
                    dt = ds.Tables[0];
                }
                Session["DataTableView"] = dt;
                Session["DataTableUpdate"] = ds.Tables[1];

            }
            catch (Exception ex)
            {
                //ex Message should be displayed
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        protected void gvVendorNames_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    int VendorID = 0;
                    Label hfVenID = (Label)e.Row.FindControl("hdnVendorID");
                    int.TryParse(hfVenID.Text.ToString(), out VendorID);

                    if (ViewState["VendorID"].Equals(VendorID))
                    {

                    }
                    else
                    {

                        GridView gvProductList = (GridView)e.Row.FindControl("gvVendorProducts");

                        DataTable dt = (DataTable)Session["DataTableView"];
                        DataView dv = dt.DefaultView;
                        dv.RowFilter = "VendorID = '" + VendorID + "'";

                        dt = null;
                        dt = dv.ToTable();

                        gvProductList.DataSource = dt;
                        gvProductList.DataBind();

                        ViewState["VendorID"] = VendorID;
                    }

                }
            }
            catch(Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
        }
        protected void gvVendorNames_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvVendorNames_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvVendorNames.PageIndex = e.NewPageIndex;
            DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        protected void gvVendorProducts_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        protected void gvVendorProducts_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            try
            {
                if (e.Row.RowType == DataControlRowType.DataRow)
                {
                    //GridView gvProductList = (GridView)gvVendorNames.FindControl("gvVendorProducts");

                    Label lblProductID = (Label)e.Row.FindControl("lblProductID");
                    DropDownList ddlVendorList = (DropDownList)e.Row.FindControl("ddlPreviousVendors");

                    int ProductID = 0;
                    int.TryParse(lblProductID.Text.ToString(), out ProductID);

                    connection.Open();
                    SqlCommand cmd = new SqlCommand("sp_GetProductVendors_Replenishment", connection);
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@productID", ProductID);

                    DataSet ds = new DataSet();
                    SqlDataAdapter dA = new SqlDataAdapter(cmd);
                    dA.Fill(ds);
                    ddlVendorList.DataSource = ds.Tables[0];
                    ddlVendorList.DataValueField = "SuppID";
                    ddlVendorList.DataTextField = "SupName";
                    ddlVendorList.DataBind();
                    if (ddlVendorList != null)
                    {
                        ddlVendorList.Items.Insert(0, "Select Vendor");
                        ddlVendorList.SelectedIndex = 0;
                    }

                }
            }
            catch(Exception ex)
            {
                WebMessageBoxUtil.Show(ex.Message);
            }
            finally
            {
                if (connection.State == ConnectionState.Open) { connection.Close(); }
            }
        }

        protected void gvVendorProducts_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            

            if(e.CommandName=="NewVendor")
            {

            }

            else if (e.CommandName == "UpdateStock")
            {
                Label lblProductID = (Label)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("lblProductID");
                Label lblVendorID = (Label)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("lblVendorID");
                int ProductID = 0;
                int MainVendorID = 0;
                int UpdatedQuantity = 0;
                int.TryParse(lblProductID.Text.ToString(), out ProductID);
                int.TryParse(lblVendorID.Text.ToString(), out MainVendorID);

                TextBox txtQuantity = (TextBox)((GridView)sender).Rows[((GridView)sender).EditIndex].FindControl("txtQuantity");

                int.TryParse(txtQuantity.Text.ToString(), out UpdatedQuantity);

                DataTable dtChanged = (DataTable)Session["DataTableView"];
                if(UpdatedQuantity >0)
                {
                    for (int i = 0; i < dtChanged.Rows.Count; i++)
                    {
                        int Product = 0;
                        int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                        int Vendor = 0;
                        int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                        if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                        {
                            dtChanged.Rows[i]["QtySold"] = UpdatedQuantity;
                            dtChanged.AcceptChanges();
                        }
                    }
                }

                Session["DataTableView"] = dtChanged;

                ((GridView)sender).EditIndex = -1;
                DisplayMainGrid((DataTable)Session["DataTableView"]);
            }

            else if (e.CommandName == "Delete")
            {
                String lblProductID = e.CommandArgument.ToString().Split(',')[0];
                String lblVendorID = e.CommandArgument.ToString().Split(',')[1];
                int ProductID = 0;
                int MainVendorID = 0;
                ArrayList list = new ArrayList();

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
                        list.Add(i);
                    }
                }

                foreach(int i in list)
                {
                    dtChanged.Rows[i].Delete();
                    dtChanged.AcceptChanges();
                }

                Session["DataTableView"] = dtChanged;
                DisplayMainGrid((DataTable)Session["DataTableView"]);
            }
        }

        protected void gvVendorProducts_RowEditing(object sender, GridViewEditEventArgs e)
        {
            
            ((GridView)sender).EditIndex = e.NewEditIndex;
            //DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        protected void gvVendorProducts_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {

        }

        protected void gvVendorProducts_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            ((GridView)sender).EditIndex = -1;
            DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        protected void gvVendorProducts_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {

        }

        protected void ddlPreviousVendors_SelectedIndexChanged(object sender, EventArgs e)
        {

            Label lblProduct = (Label) ((GridView)((DropDownList)sender).Parent.Parent.Parent.Parent).Rows[0].FindControl("lblProductID");
            Label lblVendor = (Label) ((GridView)((DropDownList)sender).Parent.Parent.Parent.Parent).Rows[0].FindControl("lblVendorID");

            int ProductID = 0;
            int MainVendorID = 0;

            int.TryParse(lblProduct.Text.ToString(), out ProductID);
            int.TryParse(lblVendor.Text.ToString(), out MainVendorID);

            int SelectedVendorID = 0;
            int.TryParse(((DropDownList)sender).SelectedValue.ToString(), out SelectedVendorID);

            String SelectedVendorName = ((DropDownList)sender).SelectedItem.ToString();

            DataTable dtChanged = (DataTable)Session["DataTableView"];

            for (int i = 0; i < dtChanged.Rows.Count; i++)
            {
                int Product = 0;
                int.TryParse(dtChanged.Rows[i]["ProductID"].ToString(), out Product);

                int Vendor = 0;
                int.TryParse(dtChanged.Rows[i]["VendorID"].ToString(), out Vendor);

                if (Product.Equals(ProductID) && Vendor.Equals(MainVendorID))
                {
                    dtChanged.Rows[i]["VendorID"] = SelectedVendorID;
                    dtChanged.Rows[i]["VendorName"] = SelectedVendorName;
                    dtChanged.AcceptChanges();
                }
            }

            Session["DataTableView"] = dtChanged;
            DisplayMainGrid((DataTable)Session["DataTableView"]);
        }

        
    }
}