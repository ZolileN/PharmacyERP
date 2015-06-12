﻿using AjaxControlToolkit;
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
    public partial class StoresPopup : System.Web.UI.UserControl
    {
        string text = "";
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        public static DataSet ProductSet;
        

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                BindGrid();
            }
        }

        public void BindGrid()
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
                    SqlCommand command = new SqlCommand("dbo.Sp_GetSystems_ByRoles", connection);
                    command.CommandType = CommandType.StoredProcedure;
                    command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
                    command.Parameters.AddWithValue("@p_RoleName", "Store");
                    SqlDataAdapter SA = new SqlDataAdapter(command);

                    SA.Fill(ds);
                    Session["dsStoresPopup"] = ds;
                    ProductSet = ds;
                    dgvStoresPopup.DataSource = ds;
                    dgvStoresPopup.DataBind();
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

        public void PopulateGrid()
        {
            if (Session["txtStore"] != null)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                       // String Query = "Select * FROM tbl_System Where SystemName Like '" + Session["txtStore"].ToString() + "'";

                        if (connection.State == ConnectionState.Closed)
                        {
                            connection.Open();
                        }
                        SqlCommand command = new SqlCommand("dbo.Sp_GetSystems_ByRoles", connection);
                        command.CommandType = CommandType.StoredProcedure;
                        if (Session["txtStore"] == null)
                        {
                            command.Parameters.AddWithValue("@p_systemName", DBNull.Value);
                        }
                        else
                        {
                            command.Parameters.AddWithValue("@p_systemName", Session["txtStore"].ToString()); 
                        }
                        command.Parameters.AddWithValue("@p_RoleName", "Store");
                        SqlDataAdapter SA = new SqlDataAdapter(command);
                        ProductSet = null;
                        SA.Fill(ds);
                        Session["dsStoresPopup"] = ds;
                        ProductSet = ds;
                        dgvStoresPopup.DataSource = ds;
                        dgvStoresPopup.DataBind();
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
        }

        #region Stores Gird Events
        protected void dgvStoresPopup_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            dgvStoresPopup.PageIndex = e.NewPageIndex;
            if (Session["txtStore"] != null)
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

        protected void dgvStoresPopup_RowCancelingEdit(object sender, GridViewCancelEditEventArgs e)
        {
            dgvStoresPopup.EditIndex = -1;
            if (Session["Text"] != null)
            {
                PopulateGrid();
            }
            else
            {
                BindGrid();
            }
        }

        protected void dgvStoresPopup_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void dgvStoresPopup_RowDataBound(object sender, GridViewRowEventArgs e)
        {

        }

        protected void dgvStoresPopup_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {

        }

        protected void dgvStoresPopup_RowEditing(object sender, GridViewEditEventArgs e)
        {
            dgvStoresPopup.EditIndex = e.NewEditIndex;
            BindGrid();
        }

        protected void dgvStoresPopup_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        #endregion

        protected void SelectStore_Click(object sender, EventArgs e)
        {
            GridViewRow rows = dgvStoresPopup.SelectedRow;
            foreach (GridViewRow row in dgvStoresPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Control ctl = this.Parent;
                        TextBox ltMetaTags = null;
                        Label lblStoreID = null;
                        Button btnContinue = (Button)ctl.FindControl("btnContinue");
                        if (btnContinue != null)
                        {
                            btnContinue.Visible = true;

                        }
                        lblStoreID = (Label)ctl.FindControl("lblStoreId");
 
                        ltMetaTags = (TextBox)ctl.FindControl("txtStore");
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[2].Text);
                        }
                        if(lblStoreID !=null)
                        {

                            lblStoreID.Text = Server.HtmlDecode(row.Cells[7].Text);


                        }
                    }
                }
            }

            Session.Remove("txtStore");
        }

        public void PopulateStoreWithStock()
        {
            DataSet dsResults = new DataSet();
            int ProductID = int.Parse(Session["ProductId"].ToString());
            if(connection.State == ConnectionState.Closed)
            {
                connection.Open();
            }
            SqlCommand command = new SqlCommand("sp_GetStoreByProductID", connection); 
            command.CommandType = CommandType.StoredProcedure;

            command.Parameters.AddWithValue("@ProductID", ProductID);
            SqlDataAdapter da = new SqlDataAdapter(command);
            da.Fill(dsResults);

            dgvStoresPopup.DataSource = dsResults;
            dgvStoresPopup.DataBind();
        }
    }
}