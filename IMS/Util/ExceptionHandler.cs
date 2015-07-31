using IMSCommon.Util;
using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.SessionState;

namespace IMS.Util
{
    /// <summary>
    ///  Utility responsible for logging and generating appropriate response against an exception.
    ///  This class is singelton in nature. 
    /// </summary>
    public class ExceptionHandler
    {
        #region fields
        private static ExceptionHandler _handler=null;
        private Dictionary<string, string> redirectionLookup;
        #endregion

        #region Constructor
        private ExceptionHandler()
        {
            redirectionLookup = new Dictionary<string, string>() { };
        }

        #endregion

        #region public methods
        public static ExceptionHandler GetInstance() 
        {
            if (_handler == null) 
            {
                _handler = new ExceptionHandler();
            }
            return _handler;
        }

        public void GenerateExpResponse(string sourcePage, RedirectionStrategy redirectionStrategy, HttpSessionState session, HttpServerUtility server, HttpResponse response, ILog log, Exception exp)
        {
            //+ Log error
            string errorMessage = this.GenerateLogString(exp);
            log.Error(errorMessage);
            //-
                     
            //redirect to different page in case of remote as strategy
            if (redirectionStrategy == RedirectionStrategy.Remote)
            {
                SetErrorMessage(session, ErrorMessageUtility.remoteMessage);
                string url=urlLookup(sourcePage);
                if (url != null)
                    server.Transfer(url,false);
                else 
                {
                    if (Convert.ToInt32(session["UserSys"]).Equals(1))
                    {
                        response.Redirect("WarehouseMain.aspx", false);
                    }
                    else
                    {
                        response.Redirect("StoreMain.aspx", false);
                    }
                }
            }
            else 
            {
                SetErrorMessage(session, ErrorMessageUtility.genericMessage);
                server.Transfer(sourcePage, false);
            }
          
        }
        #endregion

        #region helper methods
        public String GenerateLogString(Exception exp) 
        {
            string errorMessage;
            string[] splitString = exp.StackTrace.Split(new string[] { "line" }, StringSplitOptions.None);
            string[] lineSplit = splitString[1].Split(new string[] { "\r\n" }, StringSplitOptions.None);
            errorMessage= exp.GetType().ToString() + " Message : " + exp.Message + " Method : " + exp.TargetSite + " Line # : " + lineSplit[0] ;
            
            return errorMessage;
        }

        private void SetErrorMessage(HttpSessionState session, string message)
        {
            session["Is_Error"] = "True";
            session["Error_Message"] = message;
        }

        private string urlLookup(string sourcePage) 
        {
            string url = null;
            if (redirectionLookup.ContainsKey(sourcePage)) 
            {
                url=redirectionLookup[sourcePage];
            }
            return url;
        }

        internal void CheckForErrorMessage(HttpSessionState session)
        {
            if (session["Is_Error"] != null)
            {
                if (session["Is_Error"].Equals("True"))
                {

                    WebMessageBoxUtil.Show(session["Error_Message"].ToString());
                    session["Is_Error"] = "False";
                    session["Error_Message"] = string.Empty;
                }
            }
        }
        #endregion
    }
}