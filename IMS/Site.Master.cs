using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;

namespace IMS
{
    public partial class SiteMaster : MasterPage
    {
        private const string AntiXsrfTokenKey = "__AntiXsrfToken";
        private const string AntiXsrfUserNameKey = "__AntiXsrfUserName";
        private string _antiXsrfTokenValue;

        protected void Page_Init(object sender, EventArgs e)
        {
            // The code below helps to protect against XSRF attacks
            var requestCookie = Request.Cookies[AntiXsrfTokenKey];
            Guid requestCookieGuidValue;
            if (requestCookie != null && Guid.TryParse(requestCookie.Value, out requestCookieGuidValue))
            {
                // Use the Anti-XSRF token from the cookie
                _antiXsrfTokenValue = requestCookie.Value;
                Page.ViewStateUserKey = _antiXsrfTokenValue;
            }
            else
            {
                // Generate a new Anti-XSRF token and save to the cookie
                _antiXsrfTokenValue = Guid.NewGuid().ToString("N");
                Page.ViewStateUserKey = _antiXsrfTokenValue;

                var responseCookie = new HttpCookie(AntiXsrfTokenKey)
                {
                    HttpOnly = true,
                    Value = _antiXsrfTokenValue
                };
                if (FormsAuthentication.RequireSSL && Request.IsSecureConnection)
                {
                    responseCookie.Secure = true;
                }
                Response.Cookies.Set(responseCookie);
            }

            Page.PreLoad += master_Page_PreLoad;
        }

        protected void master_Page_PreLoad(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                // Set Anti-XSRF token
                ViewState[AntiXsrfTokenKey] = Page.ViewStateUserKey;
                ViewState[AntiXsrfUserNameKey] = Context.User.Identity.Name ?? String.Empty;
            }
            else
            {
                // Validate the Anti-XSRF token
                if ((string)ViewState[AntiXsrfTokenKey] != _antiXsrfTokenValue
                    || (string)ViewState[AntiXsrfUserNameKey] != (Context.User.Identity.Name ?? String.Empty))
                {
                    throw new InvalidOperationException("Validation of Anti-XSRF token failed.");
                }
            }
        }


        void WHReports_Login(object sender, EventArgs e)
        {
            string auth_key = Session["key"].ToString();
            string username = Session["LoginID"].ToString();
            string Url = System.Configuration.ConfigurationManager.AppSettings["SecretLogin"]; 
            string formId = "LoginForm";

            System.Diagnostics.Debug.WriteLine(auth_key + " " +username);


            StringBuilder htmlForm = new StringBuilder();
            htmlForm.AppendLine("<html>");
            htmlForm.AppendLine(String.Format("<body onload='document.forms[\"{0}\"].submit()'>", formId));
            htmlForm.AppendLine(String.Format("<form id='{0}' method='POST' action='{1}'>", formId, Url));
            htmlForm.AppendLine("<input type='hidden' name='UserName' id='UserName' value='"+username+"' />");
            htmlForm.AppendLine("<input type='hidden' name='auth_key' id='auth_key' value='" + auth_key+ "' />");
            htmlForm.AppendLine("</form>");
            htmlForm.AppendLine("</body>");
            htmlForm.AppendLine("</html>");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(htmlForm.ToString());
            HttpContext.Current.Response.End();
        }



        void PharmacyReports_Login(object sender, EventArgs e)
        {
            string auth_key = Session["key"].ToString();
            string username = Session["LoginID"].ToString();
            string Url = System.Configuration.ConfigurationManager.AppSettings["SecretLoginPharmacy"];
            string formId = "LoginForm";

            System.Diagnostics.Debug.WriteLine(auth_key + " " + username);


            StringBuilder htmlForm = new StringBuilder();
            htmlForm.AppendLine("<html>");
            htmlForm.AppendLine(String.Format("<body onload='document.forms[\"{0}\"].submit()'>", formId));
            htmlForm.AppendLine(String.Format("<form id='{0}' method='POST' action='{1}'>", formId, Url));
            htmlForm.AppendLine("<input type='hidden' name='UserName' id='UserName' value='" + username + "' />");
            htmlForm.AppendLine("<input type='hidden' name='auth_key' id='auth_key' value='" + auth_key + "' />");
            htmlForm.AppendLine("</form>");
            htmlForm.AppendLine("</body>");
            htmlForm.AppendLine("</html>");

            HttpContext.Current.Response.Clear();
            HttpContext.Current.Response.Write(htmlForm.ToString());
            HttpContext.Current.Response.End();
        }

        public void logo_click(object sender, EventArgs e) {
            String UserRole = Session["UserRole"].ToString();

            switch (UserRole)
            {
                case "WareHouse":
                    
                    Response.Redirect("WarehouseMain.aspx", false);


                    break;
                case "Store":
                    
                    Response.Redirect("StoreMain.aspx", false);
                    break;
                case "HeadOffice":
                   
                    Response.Redirect("HeadOfficeMain.aspx", false);
                    break;

            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {

            
            if(Session["firstNamelastName"]==null){
                Response.Redirect("IMSLogin.aspx", true);
            }

            logo.ServerClick += new EventHandler(logo_click);

            FirstLast.Text = Session["firstNamelastName"].ToString();
            FirstLast.Visible = true;
            headofficeNavigation.Visible = storeNavigation.Visible = StoreBlock.Visible = SalesmanNavigation.Visible=WarehouseBlock.Visible= false;
            warehouseNavigation.Visible = false;
            if (Session["UserName"] !=null){
                lbllogin.Text = Session["UserName"].ToString();
                lbllogin.Visible = true;
            }



            if (Session["userRole"]!=null && Session["userRole"].ToString() == "WareHouse")
            {

                if (Session["user_RoleName"]!=null && "Salesman".Equals(Session["user_RoleName"].ToString()))
                {
                    SalesmanNavigation.Visible = true;
                   

                }
                else {

                    warehouseNavigation.Visible = true;
                    WarehouseBlock.Visible = true;
                    WHReports.ServerClick += new EventHandler(WHReports_Login);
                    PharmacyReports.ServerClick += new EventHandler(PharmacyReports_Login);
                }

                
              
            }
            else if (Session["userRole"].ToString() == "HeadOffice")
            {

                headofficeNavigation.Visible = true;
             
            }
            else if (Session["userRole"].ToString() == "Store")
            {
                
                self_PharmacyReports.ServerClick += new EventHandler(PharmacyReports_Login);
                StoreBlock.Visible = true;
                storeNavigation.Visible = true;
             
            }




        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            //Session.Abandon();
            Session.Clear();
            Context.GetOwinContext().Authentication.SignOut();
        }

        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/IMSLogin.aspx", false);
        }
    }

}