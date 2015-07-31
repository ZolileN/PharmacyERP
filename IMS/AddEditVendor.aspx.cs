﻿using IMS.Util;
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

namespace IMS
{
    public partial class AddEditVendor : System.Web.UI.Page
    {
        public static SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString());
        private ILog log;
        private string pageURL;
        private ExceptionHandler expHandler = ExceptionHandler.GetInstance();
        protected void Page_Load(object sender, EventArgs e)
        {
            System.Uri url = Request.Url;
            pageURL = url.AbsolutePath.ToString();
            log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
            if (!IsPostBack)
            {


                if (Request.QueryString["Id"] != null)
                {
                    EditTitleWH.Visible = true;
                    regTitleWH.Visible = false;
                    btnCreateVendor.Visible = false;
                    btnUpdateVendor.Visible = true;
                    LoadData();
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
        private void LoadData()
        {
            try
            {


                DataSet ds = new DataSet();
                Vendor obj = new Vendor();

                obj.supp_ID = int.Parse(Request.QueryString["Id"].ToString());
                ds = VendorBLL.GetDistinct(connection,obj);


                //    ds.Tables[0].Rows[0]["Supp_ID"].ToString();
                //  txtVendorName.Text = ds.Tables[0].Rows[0]["Supp_ID"].ToString();
                txtID.Text = ds.Tables[0].Rows[0]["SuppID"].ToString();
                txtVendorName.Text = ds.Tables[0].Rows[0]["SupName"].ToString();
                txtaddress.Text = ds.Tables[0].Rows[0]["Address"].ToString();
                txtcity.Text = ds.Tables[0].Rows[0]["City"].ToString();
                txtState.Text = ds.Tables[0].Rows[0]["State"].ToString();
                txtCounty.Text = ds.Tables[0].Rows[0]["Country"].ToString();
                txtPincode.Text = ds.Tables[0].Rows[0]["Pincode"].ToString();
                txtphone.Text = ds.Tables[0].Rows[0]["Phone"].ToString();
                txtfax.Text = ds.Tables[0].Rows[0]["Fax"].ToString();
                txtmobile.Text = ds.Tables[0].Rows[0]["Mobile"].ToString();
                txtpager.Text = ds.Tables[0].Rows[0]["Pager"].ToString();
                txtEmail.Text = ds.Tables[0].Rows[0]["Email"].ToString();
                txtConPerson.Text = ds.Tables[0].Rows[0]["ConPerson"].ToString();
                txtDiscount.Text = ds.Tables[0].Rows[0]["Discount"].ToString();
                txtCredit.Text = ds.Tables[0].Rows[0]["Credit"].ToString();
                

            }
            catch (Exception ex)
            {

                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }

        }
        protected void btnGoBack_Click(object sender, EventArgs e)
        {
            Response.Redirect("ManageVendor.aspx",false);
        }

        protected void btnCreateVendor_Click(object sender, EventArgs e)
        {
            try
            {
                Vendor obj = new Vendor();


                obj.SupName = txtVendorName.Text;
                obj.Email = txtEmail.Text;
                obj.city = txtcity.Text;
                obj.State = txtState.Text;
                obj.Country = txtCounty.Text;
                obj.address = txtaddress.Text;
                obj.ConPerson = txtConPerson.Text;
                obj.Credit = txtCredit.Text;
                obj.Discount = txtDiscount.Text;
                obj.Fax = txtfax.Text;
                obj.Phone = txtphone.Text;
                obj.Mobile = txtmobile.Text;
                obj.Pager = txtpager.Text;
                obj.Pincode = txtPincode.Text;
                obj.BarterExchangeID = txtBarderExchangeID.Text;

                obj.DateCreated = DateTime.Now;
                VendorBLL objAdd = new VendorBLL();
                objAdd.Add(obj, connection);

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally 
            {
                Clear();
            }
        }

        protected void btnCancelVendor_Click(object sender, EventArgs e)
        {
            Clear();
           
        }

        private void Clear() 
        {
            txtVendorName.Text = string.Empty;
            txtEmail.Text = string.Empty;
            txtcity.Text = string.Empty;
            txtState.Text = string.Empty;
            txtCounty.Text = string.Empty;
            txtaddress.Text = string.Empty;
            txtConPerson.Text = string.Empty;
            txtCredit.Text = string.Empty;
            txtDiscount.Text = string.Empty;
            txtfax.Text = string.Empty;
            txtphone.Text = string.Empty;
            txtmobile.Text = string.Empty;
            txtpager.Text = string.Empty;
            txtPincode.Text = string.Empty;
        }
        protected void btnUpdateVendor_Click(object sender, EventArgs e)
        {
            try
            {
                Vendor obj = new Vendor();

                obj.supp_ID = int.Parse(txtID.Text);
                obj.SupName = txtVendorName.Text;
                obj.Email = txtEmail.Text;
                obj.city = txtcity.Text;
                obj.State = txtState.Text;
                obj.Country = txtCounty.Text;
                obj.address = txtaddress.Text;
                obj.ConPerson = txtConPerson.Text;
                obj.Credit = txtCredit.Text;
                obj.Discount = txtDiscount.Text;
                obj.Fax = txtfax.Text;
                obj.Phone = txtphone.Text;
                obj.Mobile = txtmobile.Text;
                obj.Pager = txtpager.Text;
                obj.Pincode = txtPincode.Text;
                obj.DateCreated = DateTime.Now;
                VendorBLL objAdd = new VendorBLL();
                objAdd.Update(obj, connection);

            }
            catch (Exception ex)
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();
                throw ex;
            }
            finally
            {
                Clear();
            }
        }
    }
}