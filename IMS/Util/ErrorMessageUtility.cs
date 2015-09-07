using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace IMS.Util
{
    public class ErrorMessageUtility
    {
        #region generic exception messages
        public const string remoteMessage = "Error Loading the page,kindly try again in a while";
        public const string genericMessage = "We are experiencing some issues, kindly try again in a while";
        public const string constantErrorMessage = "The issue seems to persist, kindly try after little while!!!";
        public const string stockExistMessage = "Cannot delete this store as stock exists in the inventory.";
        #endregion
    }
}