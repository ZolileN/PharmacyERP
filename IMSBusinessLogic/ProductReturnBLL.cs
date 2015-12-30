using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
   public class ProductReturnBLL
    {
        public static DataSet GetStockByName(string ProductName, int SysID, bool isStore)
        {
            DataSet resultSet = new DataSet();
            try
            {
                ProductReturnDLL objProductDAL = new ProductReturnDLL();

                resultSet = objProductDAL.GetStockByName(ProductName, SysID,isStore);
              
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {

            }
            return resultSet;
        }


        public static DataSet GetStockBatches(long ProductID, int SysID)
        {
            DataSet resultSet = new DataSet();
            try
            {
                ProductReturnDLL objProductDAL = new ProductReturnDLL();

                resultSet = objProductDAL.GetStockBatches(ProductID, SysID);
              
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {

            }
            return resultSet;
        }

        public  static DataSet GenerateItemReturn(long VendorID, long PharmaIDStoreID, Int32 UserID, string ReturnMode, DataTable TableVar)
        {
            DataSet resultSet = new DataSet();
            try
            {
                ProductReturnDLL objProductDAL = new ProductReturnDLL();
                resultSet = objProductDAL.GenerateItemReturn(VendorID, PharmaIDStoreID, UserID, ReturnMode, TableVar);


               
               // WebMessageBoxUtil.Show("Vendor Successfully Updated ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
            return resultSet;
        }
        public static DataSet GetItemReturns(long ReturnID)
        {
            DataSet resultSet = new DataSet();
            try
            {
                ProductReturnDLL objProductDAL = new ProductReturnDLL();

                resultSet = objProductDAL.GetItemReturns(ReturnID);
              
            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {

            }
            return resultSet;
        }
        public static DataSet GetItemReturnsDetails(long ReturnID)
        {
            DataSet resultSet = new DataSet();
            try
            {
                ProductReturnDLL objProductDAL = new ProductReturnDLL();

                resultSet = objProductDAL.GetItemReturnsDetails(ReturnID);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {

            }
            return resultSet;
        }
    }
}
