using AjaxControlToolkit;
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
                    String Query = "Select * FROM tbl_System Where Status = 1";

                    connection.Open();
                    SqlCommand command = new SqlCommand(Query, connection);
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

        public void PopulateGrid()
        {
            if (Session["Text"] != null)
            {
                DataTable dt = new DataTable();
                DataSet ds = new DataSet();
                #region Getting Product Details
                try
                {
                    int id;
                    if (int.TryParse(Session["UserSys"].ToString(), out id))
                    {
                        String Query = "Select * FROM tbl_System Where Status = 1 and SystemName Like '" + Session["Text"].ToString() + "'";

                        connection.Open();
                        SqlCommand command = new SqlCommand(Query, connection);
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
            if (Session["Text"] != null)
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
            foreach (GridViewRow row in dgvStoresPopup.Rows)
            {
                if (row.RowType == DataControlRowType.DataRow)
                {
                    CheckBox chkRow = (row.Cells[0].FindControl("chkCtrl") as CheckBox);
                    if (chkRow.Checked)
                    {
                        Control ctl = this.Parent;
                        TextBox ltMetaTags = null;
                        GridView gvStockDisplayGrid = (GridView)ctl.FindControl("dgvStockDisplayGrid");

                        ltMetaTags = (TextBox)ctl.FindControl("txtSearch");

                        Label lbProdId = (Label)ctl.FindControl("lblProductId");
                        if (lbProdId != null)
                        {
                            lbProdId.Text = Server.HtmlDecode(row.Cells[7].Text);
                        }

                        DataSet dsProducts_ProdPopUp = (DataSet)Session["dsProdcts"];
                        if (dsProducts_ProdPopUp != null && dsProducts_ProdPopUp.Tables.Count > 0 && dsProducts_ProdPopUp.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drs = dsProducts_ProdPopUp.Tables[0].Select("ProductID = '" + lbProdId.Text + "'");
                            if (drs.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product already added to the Sales Order.')", true);
                                return;
                            }
                        }
                        DataSet dsProducts_POPopUp = (DataSet)Session["dsProducts_MP"];
                        if (dsProducts_POPopUp != null && dsProducts_POPopUp.Tables.Count > 0 && dsProducts_POPopUp.Tables[0].Rows.Count > 0)
                        {
                            DataRow[] drs = dsProducts_POPopUp.Tables[0].Select("ProductID = '" + lbProdId.Text + "'");
                            if (drs.Length > 0)
                            {
                                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product already added to the Purchase Order.')", true);
                                return;
                            }
                        }
                        if (ltMetaTags != null)
                        {
                            ltMetaTags.Text = Server.HtmlDecode(row.Cells[1].Text);

                        }
                        if (gvStockDisplayGrid != null)
                        {
                            gvStockDisplayGrid.DataSource = null;
                            gvStockDisplayGrid.DataBind();
                        }

                    }
                }
                Session.Remove("Text");
            }
        }
    }
}