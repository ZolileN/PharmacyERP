using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
  public  class NearestExpiryBLL
    {
        public static DataSet LoadDepartments()
        {
            DataSet resultSet = new DataSet();
            try
            {
                NearestExpiryDLL objProductDAL = new NearestExpiryDLL();

                resultSet = objProductDAL.LoadDepartments();

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


        public static DataSet LoadCategory(Int32 deptid)
        {
            DataSet resultSet = new DataSet();
            try
            {
                NearestExpiryDLL objProductDAL = new NearestExpiryDLL();

                resultSet = objProductDAL.LoadCategory(deptid);

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

        public static DataSet LoadSubCategory(Int32 CatgID)
        {
            DataSet resultSet = new DataSet();
            try
            {
                NearestExpiryDLL objProductDAL = new NearestExpiryDLL();

                resultSet = objProductDAL.LoadSubCategory(CatgID);

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
