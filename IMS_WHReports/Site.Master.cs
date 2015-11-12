using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
namespace IMS_WHReports
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
        void WH_Login(object sender, EventArgs e)
        {
            string auth_key = Session["key"].ToString();
            string username = Session["LoginID"].ToString();
            string Url = System.Configuration.ConfigurationManager.AppSettings["eInventoryMain"];
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
        protected void Page_Load(object sender, EventArgs e)
        {
            InventoryLogin.ServerClick += new EventHandler(WH_Login);

            headofficeNav.Visible = storeNav.Visible = warehouseNav.Visible = StoreBlock.Visible = false;
            warehouseNav.Visible = true;

            if (Session["firstNamelastName"] != null)
            {
                FirstLast.Text = Session["firstNamelastName"].ToString();
                FirstLast.Visible = true;
            }
            else {
                Response.Redirect("IMSLogin.aspx", false);
            }

            

        }

        protected void Unnamed_LoggingOut(object sender, LoginCancelEventArgs e)
        {
            Context.GetOwinContext().Authentication.SignOut();
        }
        protected void Unnamed_Click(object sender, EventArgs e)
        {
            Session.Abandon();
            Response.Redirect("~/IMSLogin.aspx", false);
        }
    }

}