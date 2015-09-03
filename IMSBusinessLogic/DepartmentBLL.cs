using IMSCommon;
using IMSCommon.Util;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using IMSDataAccess;
using IMSDataAccess.DAL;

namespace IMSBusinessLogic
{
    public class DepartmentBLL
    {

        public DepartmentBLL() { }

        public static DataSet GetAllDepartment()
        {

            DataSet resultSet = new DataSet();
            try
            {
                DepartmentDAL objDepartmentDAL = new DepartmentDAL();
                resultSet = objDepartmentDAL.Select();
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

        public void Add(Department dep)
        {
            try
            {
                DepartmentDAL objDepartmentDAL = new DepartmentDAL();
                objDepartmentDAL.Add(dep.Name, dep.Code);

                WebMessageBoxUtil.Show("Department Successfully Added ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        public void Update(Department dep)
        {
            try
            {
                DepartmentDAL objDepartmentDAL = new DepartmentDAL();
                objDepartmentDAL.Update(dep.DepartmentID, dep.Name, dep.Code);

                WebMessageBoxUtil.Show("Department Successfully Updated");
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public void Delete(Department dep)
        {
            try
            {
                DepartmentDAL objDepartmentDAL = new DepartmentDAL();
                objDepartmentDAL.Delete(dep.DepartmentID);

                WebMessageBoxUtil.Show("Department Successfully Deleted ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }


    }
}
