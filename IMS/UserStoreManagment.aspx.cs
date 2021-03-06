﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.Data.SqlTypes;
using System.Configuration;
using log4net;
using IMS.Util;
using IMSBusinessLogic;

namespace IMS
{
    public partial class UserStoreManagment : System.Web.UI.Page
    {
        private List<string> Pharmacy_List;
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        DataTable dtAllAvailableStore = new DataTable();
        DataTable dtAllAssociatedStore = new DataTable();
        string SystemIDAvailable;
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        private SalesmanAssociationBLL slmanBll = new SalesmanAssociationBLL();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack)
            {
                #region Populating Available Store Drop Down DropDown
                try
                {
                    string ID = Request.QueryString["ID"];
                    
                    #region original logic
                    //if (connection.State == ConnectionState.Closed)
                    //{
                    //    connection.Open();
                    //}

                    //SqlCommand command = new SqlCommand("Sp_GetUnAssociatedStores", connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //SqlDataAdapter sA = new SqlDataAdapter(command);


                    //command.Parameters.AddWithValue("@p_UserID", long.Parse(ID));

                    //sA.Fill(dtAllAvailableStore); 
                    #endregion

                    dtAllAvailableStore = slmanBll.SelectUnAssociatedStores(long.Parse(ID)).Tables[0];
                    DataTable dtTest = new DataTable();
                    dtAllAvailableStore.Columns.Add("Checked", typeof(bool));
                    Session["dtAvailable"] = dtAllAvailableStore;
                    foreach (DataRow dr in dtAllAvailableStore.Rows)
                    {
                        dr["Checked"] = false;
                    }
                    gvAllAvailableStore.DataSource = (DataTable)Session["dtAvailable"];
                    gvAllAvailableStore.DataBind();
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                

                #endregion

                #region Populating Associated Store Drop Down DropDown
                try
                {
                    string ID = Request.QueryString["ID"];
                    #region original logic
                    //if (connection.State == ConnectionState.Closed)
                    //{
                    //    connection.Open();
                    //}

                    //SqlCommand command = new SqlCommand("Sp_GetAssociatedStores", connection);
                    //command.CommandType = CommandType.StoredProcedure;
                    //SqlDataAdapter sA = new SqlDataAdapter(command);


                    //command.Parameters.AddWithValue("@p_UserID", long.Parse(ID));
                    //sA.Fill(dtAllAssociatedStore); 
                    #endregion
                    dtAllAssociatedStore = slmanBll.SelectAssociatedStores(long.Parse(ID)).Tables[0];
                    dtAllAssociatedStore.Columns.Add("Checked", typeof(bool));
                    Session["dtAssociated"] = dtAllAssociatedStore;
                    foreach (DataRow dr in dtAllAssociatedStore.Rows)
                    {
                        dr["Checked"] = false;
                    }
                    gvAssociatesStore.DataSource = (DataTable)Session["dtAssociated"];
                    gvAssociatesStore.DataBind();
                }
                catch (Exception ex)
                {
                  
                    throw ex;
                }
               
                #endregion
                //SelectCheckBox_OnCheckedChanged(null, null);
               
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

        protected void SelectCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            try
            {
                //DataTable dt = new DataTable();
                //dt.Columns.AddRange(new DataColumn[1] { new DataColumn("txtName")});
                Pharmacy_List = new List<string>();
                foreach (GridViewRow row in gvAllAvailableStore.Rows)
                {

                    CheckBox chkRow = (row.Cells[0].FindControl("CCheckbox") as CheckBox);
                    if (chkRow != null && chkRow.Checked)
                    {
                        SystemIDAvailable = (row.Cells[0].FindControl("lblSystemID") as Label).Text;
                        string simple = (row.Cells[1].FindControl("lblNameAvailable") as Label).Text;
                        DataTable dt = (DataTable)Session["dtAvailable"];
                        Pharmacy_List.Add(SystemIDAvailable);
                    }

                }
                AssociateStore(Pharmacy_List);
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

        protected void gvAllAvailableStore_RowCommand(object sender, GridViewCommandEventArgs e)
        {

        }

        protected void btnSwapeOne_Click(object sender, EventArgs e)
        {
            try
            {
                DataTable dtAvailable = (DataTable)Session["dtAvailable"];
                DataTable dtAssociated = (DataTable)Session["dtAssociated"];
                for (int i = dtAssociated.Rows.Count - 1; i >= 0; i--)
                {
                    if (((bool)dtAssociated.Rows[i]["Checked"] == true))
                    {
                        dtAssociated.Rows[i]["Checked"] = false;
                    }
                }
                for (int i = dtAvailable.Rows.Count - 1; i >= 0; i--)
                {
                    if (((bool)dtAvailable.Rows[i]["Checked"] == true))
                    {
                        dtAvailable.Rows[i]["Checked"] = false;
                        dtAssociated.Rows.Add(dtAvailable.Rows[i].ItemArray);
                        dtAvailable.Rows.Remove(dtAvailable.Rows[i]);
                    }
                }
                BindGrid();
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

        protected void AssociateStore(List<string> Pharmacy)
        {
            try
            {
                if (Pharmacy.Count > 0)
                {
                    DataTable dt = (DataTable)Session["dtAvailable"];

                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        dt.Rows[i]["Checked"] = false;
                    }

                    for (int i = 0; i < Pharmacy.Count; i++)
                    {
                        DataRow[] drs = dt.Select("SystemID = " + Pharmacy[i].ToString());

                        if (drs.Length > 0)
                        {
                            drs[0]["Checked"] = true;
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
        }

        private void Deassociated(List<string> Pharmacy)
        {
            if (Pharmacy.Count > 0)
            {
                DataTable dtAssociated = (DataTable)Session["dtAssociated"];
                for (int i = 0; i < dtAssociated.Rows.Count; i++)
                {
                    dtAssociated.Rows[i]["Checked"] = false;
                }
                for (int i = 0; i < Pharmacy.Count; i++)
                {
                    DataRow[] drs = dtAssociated.Select("SystemID = " + Pharmacy[i].ToString());
                    if (drs.Length > 0)
                    {
                        drs[0]["Checked"] = true;
                    }
                }
            }

        }

        protected void btnSwapeAll_Click(object sender, EventArgs e)
        {
            DataTable dtAvailable = (DataTable)Session["dtAvailable"];
            DataTable dtAssociated = (DataTable)Session["dtAssociated"];
            for (int i = dtAvailable.Rows.Count - 1; i >= 0; i--)
            {
                dtAssociated.Rows.Add(dtAvailable.Rows[i].ItemArray);
                dtAvailable.Rows.Remove(dtAvailable.Rows[i]);

            }
            BindGrid();

        }

        protected void btnBackSwapeAll_Click(object sender, EventArgs e)
        {
            DataTable dtAvailable = (DataTable)Session["dtAvailable"];
            DataTable dtAssociated = (DataTable)Session["dtAssociated"];
            for (int i = dtAssociated.Rows.Count - 1; i >= 0; i--)
            {
                dtAvailable.Rows.Add(dtAssociated.Rows[i].ItemArray);
                dtAssociated.Rows.Remove(dtAssociated.Rows[i]);
            }
            BindGrid();

        }

        protected void btnBackSwape_Click(object sender, EventArgs e)
        {
            DataTable dtAvailable = (DataTable)Session["dtAvailable"];
            DataTable dtAssociated = (DataTable)Session["dtAssociated"];
            for(int i = dtAvailable.Rows.Count - 1; i >= 0; i--)
            {
                if (((bool)dtAvailable.Rows[i]["Checked"] == true))
                {
                    dtAvailable.Rows[i]["Checked"] = false;
                }
            }
            for (int i = dtAssociated.Rows.Count - 1; i >= 0; i--)
                if (((bool)dtAssociated.Rows[i]["Checked"] == true))
                {
                    dtAssociated.Rows[i]["Checked"] = false;
                    dtAvailable.Rows.Add(dtAssociated.Rows[i].ItemArray);
                    dtAssociated.Rows.Remove(dtAssociated.Rows[i]);
                }
            BindGrid();
        }
        protected void SelectAssociatedCheckBox_OnCheckedChanged(object sender, EventArgs e)
        {
            //DataTable dtAvailable = (DataTable)Session["dtAvailable"];
            //DataTable dtAssociated = (DataTable)Session["dtAssociated"];
            Pharmacy_List = new List<string>();
            foreach (GridViewRow row in gvAssociatesStore.Rows)
            {

                CheckBox chkRow = (row.Cells[0].FindControl("CCheckboxAssocaited") as CheckBox);
                if (chkRow != null && chkRow.Checked)
                {
                    SystemIDAvailable = (row.Cells[0].FindControl("lblAssociatedSystemID") as Label).Text;
                    string simple = (row.Cells[1].FindControl("lblNameAssociated") as Label).Text;

                    Pharmacy_List.Add(SystemIDAvailable);
                }

            }
            Deassociated(Pharmacy_List);
        }
        private void BindGrid()
        {
            DataTable dtAvailable = (DataTable)Session["dtAvailable"];
            DataTable dtAssociated = (DataTable)Session["dtAssociated"];
            #region Bind Grid
            try
            {
                gvAllAvailableStore.DataSource = dtAvailable;
                gvAllAvailableStore.DataBind();
                gvAssociatesStore.DataSource = dtAssociated;
                gvAssociatesStore.DataBind();


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

        protected void btnSave_Click(object sender, EventArgs e)
        {
            try
            {
                string ID = Request.QueryString["ID"];
                DataTable dtAvailable = (DataTable)Session["dtAvailable"];
                DataTable dtAssociated = (DataTable)Session["dtAssociated"];
                long SystemID = 0;

                #region original logic
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}

                //SqlCommand command = new SqlCommand("Sp_DeleteSalesmanSystem", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_UserID", long.Parse(ID));
                //command.ExecuteNonQuery(); 
               
                
                //foreach (DataRow dr in dtAssociated.Rows)
                //{

                //    SystemID = Convert.ToInt64(dr["SystemID"]);
                //    command = new SqlCommand("Sp_AddSalesmanSystems", connection);
                //    command.CommandType = CommandType.StoredProcedure;
                //    command.Parameters.AddWithValue("@p_UserID", long.Parse(ID));
                //    command.Parameters.AddWithValue("@p_systemID", SystemID);
                //    //SqlCommand commandNew = new SqlCommand("insert into  [dbo].[SalesmanSystems]([UserID],SystemID) values(" + ID + "," +SystemID + ")", connection);
                //    command.ExecuteNonQuery();
                //}
                //connection.Close();
                #endregion

                slmanBll.Delete(long.Parse(ID));
                
                foreach (DataRow dr in dtAssociated.Rows)
                {
                    SystemID = Convert.ToInt64(dr["SystemID"]);
                    slmanBll.Insert(long.Parse(ID), SystemID);
                }

                ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Record Saved Successfully')", true);
            }
            catch (Exception ex)
            {
                throw ex;
            }
           

        }

        protected void btnBack_Click(object sender, EventArgs e)
        {
            string ID = Request.QueryString["ID"];
            Response.Redirect("RegisterUsers.aspx?ID=" + ID,false);
        }

        protected void gvAllAvailableStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAllAvailableStore.PageIndex = e.NewPageIndex;
            BindGrid();
        }

        protected void btnSearchPharma_Click(object sender, EventArgs e)
        {
            DataView dv = ((DataTable)Session["dtAvailable"]).DefaultView;
            dv.RowFilter = "SystemName LIKE '" + txtSearchPharma.Text + "%'";

            DataTable dt = new DataTable();
            dt = dv.ToTable();

            gvAllAvailableStore.DataSource = dt;
            gvAllAvailableStore.DataBind();
        }

        protected void gvAssociatesStore_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            gvAssociatesStore.PageIndex = e.NewPageIndex;
            gvAssociatesStore.DataSource = (DataTable)Session["dtAssociated"];
            gvAssociatesStore.DataBind();
        }
    }
}