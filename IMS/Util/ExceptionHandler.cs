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
        private Dictionary<string, Dictionary<string,string>> redirectionLookup;
        private const string GENERAL = "General";
        private const string PHARMACY = "Store";
        private const string STORE = "WareHouse";
        private const string SALESMAN = "Salesman";
        private const string NORMAL = "Normal";
        private const string HEADOFFICE = "HeadOffice";
        #endregion

        #region Constructor
        private ExceptionHandler()
        {
            redirectionLookup = new Dictionary<string, Dictionary<string,string>>() 
            {
                {"/AcceptOrder.aspx",
                    new Dictionary<String,String>()
                    {
                        {GENERAL,"~/ReceiveStock.aspx"}
                    }
                },
                {"/AcceptPurchaseOrder.aspx",
                    new Dictionary<String,String>()
                    {
                        {GENERAL,"~/ViewPurchaseOrders.aspx"}
                    }
                },
                {"/AddDepartment.aspx",new Dictionary<String,String>()
                    {
                        {GENERAL,"~/ManageDepartment.aspx"}
                    }
                },
                {"/AddEditCategory.aspx",
                    new Dictionary<string,string>
                    {
                        {GENERAL,"~/ManageCategory.aspx"}
                    }
                },
                {"/AddEditSubCategory.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ManageSubCategory.aspx"}
                    }
                },
                {"/AddEditVendor.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ManageVendor.aspx"}
                    }
                },
                {"/AddProduct.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ManageProducts.aspx"}
                    }
                },
                {"/AddStock.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ViewInventory.aspx"}
                    }
                },
                {"/AddSystem.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseManagement.aspx"},
                        {PHARMACY,"PharmacyManagement.aspx"}
                    }
                },
                {"/AddVendorsToStore.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/SelectStores.aspx"}
                    }
                },
                {"/CreateTransferRequest.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/StoreMain.aspx"}
                    }
                },
                {"/DisplayOrderDetailEntries.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/AcceptPurchaseOrder.aspx"}
                    }
                },
                {"/GenerateAcceptedTransferOrder.aspx",
                    new Dictionary<String,String>()
                    {
                        {STORE,"~/ReceiveTransferOrder.aspx"},
                        {PHARMACY,"~/RespondStoreRequest.aspx"}
                    }
                },
                {"/HAADPopulation.aspx",
                    new Dictionary<String,String>()
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/Inventory_Print.aspx",new Dictionary<String,String>()
                    {
                        {GENERAL,"~/ViewInventory.aspx"}
                    }
                },
                {"/InvoicePrint.aspx",
                    new Dictionary<string,string>
                    {
                        {GENERAL,"~/SalesOrderInvoice.aspx"}
                    }
                },
                {"/InvoicePrintBonus.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/SalesOrderInvoice.aspx"}
                    }
                },
                {"/ItemRequestWH.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/StoreMain.aspx"}
                    }
                },
                {"/ManageCategory.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ManageDepartment.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ManageProducts.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseMain.aspx"},
                        {PHARMACY,"StoreMain.aspx"}
                    }
                },
                {"/ManageSubCategory.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ManageVendor.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ManualPurchase.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseMain.aspx"},
                        {PHARMACY,"StoreMain.aspx"}
                    }
                },
                {"/OrderSalesManual.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/OrderSalesManual_Details.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/OrderSalesManual.aspx"}
                    }
                },
                {"/PackingListGeneration.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/PharmacyManagement.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/PharmacyNameResult.aspx",new Dictionary<string,string>
                    {
                         {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/PO_GENERATE.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/Print_Request_WH.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/SelectWarehouse.aspx"}
                    }
                },
                {"/Prod2Store.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"ProductStoreSelect.aspx"} 
                    }
                },
                {"/ProductStoreSelect.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ReceiveRequestTransfers.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/SentTransferRequests.aspx"}
                    }
                },
                {"/ReceiveSalesOrder.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ReceiveStoreRequest.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ReceiveTransferOrder.aspx",new Dictionary<string,string>
                    {
                      
                        {GENERAL,"StoreMain.aspx"}
                    }
                },
                {"/RecieveSOFull.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ReceiveSalesOrder.aspx"}
                    }
                },
                {"/RecieveSOFullEdit.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/RecieveSOFull.aspx"}
                    }
                },
                {"/RegisterUsers.aspx",new Dictionary<string,string>
                    {
                        {SALESMAN,"SalemanMangment.aspx"},
                        {NORMAL,"UserManagment.aspx"}
                    }
                },
                {"/ReplenishMain.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/StoreMain.aspx"}
                    }
                },
                {"/ReplenishMovement.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ReplenishMain.aspx"}
                    }
                },
                {"/ReplenishPO_Generate.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/ReplenishMovement.aspx"}
                    }
                },
                {"/ReportPrinting.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/RespondStoreRequest.aspx",new Dictionary<string,string>
                    {
                         {GENERAL,"~/ReceiveStoreRequest.aspx"}
                    }
                },
                {"/SalemanMangment.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/SalesOrderInvoice.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/SelectionStock.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"ViewInventory.aspx"} 
                    }
                },
                {"/SelectStores.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/SelectVendor.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseMain.aspx"},
                        {PHARMACY,"StoreMain.aspx"}
                    }
                },
                {"/SelectWarehouse.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/StoreMain.aspx"}
                    }
                },
                {"/SentTransferRequests.aspx",new Dictionary<string,string>
                    {
                      
                        {GENERAL,"StoreMain.aspx"}
                    }
                },
                {"/UserManagment.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/UserStoreManagment.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"RegisterUsers.aspx"}
                    }
                },
                {"/ViewInventory.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseMain.aspx"},
                        {PHARMACY,"StoreMain.aspx"}
                    }
                },
                {"/ViewPackingList.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/PackingListGeneration.aspx"}
                    }
                },
                {"/ViewPackingList_SO.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/OrderSalesManual.aspx"}
                    }
                },
                {"/ViewPlacedOrders.aspx",new Dictionary<string,string>
                    {
                        {STORE,"WarehouseMain.aspx"},
                        {PHARMACY,"StoreMain.aspx"}
                    }
                },
                {"/ViewPurchaseOrders.aspx",new Dictionary<string,string>
                    {
                         {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/ViewSalesOrders.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/WarehouseManagement.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"~/WarehouseMain.aspx"}
                    }
                },
                {"/WHResquestInvoice.aspx",new Dictionary<string,string>
                    {
                        {GENERAL,"ReceiveStoreRequest.aspx"} 
                    }
                }
            };
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
                string url=urlLookup(sourcePage,session);
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
            string innerException= exp.InnerException != null ? exp.InnerException.Message : string.Empty;
            errorMessage = exp.GetType().ToString() + " Message : " + exp.Message + "Inner Exception : " + innerException + " Method : " + exp.TargetSite + " Line # : " + lineSplit[0] + " Stack trace # : "+exp.StackTrace;
            
            return errorMessage;
        }

        private void SetErrorMessage(HttpSessionState session, string message)
        {
            session["Is_Error"] = "True";
            session["Error_Message"] = message;
        }

        private string urlLookup(string sourcePage, HttpSessionState session)
        {
            string url = null;
            if (redirectionLookup.ContainsKey(sourcePage))
            {
                Dictionary<string, string> val = redirectionLookup[sourcePage];
                if (val.ContainsKey(GENERAL))
                {
                    url = val[GENERAL];
                }
                else
                {
                    if (val.ContainsKey(STORE) || val.ContainsKey(PHARMACY))
                    {
                        if (Convert.ToInt32(session["UserSys"]).Equals(1))
                        {
                            url = val[STORE];
                        }
                        else
                        {
                            url = val[PHARMACY];
                        }
                    }
                    else
                    {
                        if (session["ur_RoleName"].ToString().Equals("Salesman"))
                        {
                            url = val[SALESMAN];
                        }
                        else
                        {
                            url = val[NORMAL];
                        }
                    }
                }
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