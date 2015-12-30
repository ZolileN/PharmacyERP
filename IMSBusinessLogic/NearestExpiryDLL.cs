using IMSDataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    class NearestExpiryDLL : DataAccessbase
    {
        public DataSet LoadDepartments()
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetDepartments_NearestExpiry.ToString();
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString,null);
        }

        public DataSet LoadCategory(Int32 deptid)
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetCategory_NearestExpiry.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@DepartmentID", deptid)
                                         
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }

        public DataSet LoadSubCategory(Int32 CatgID)
        {
            StoredProcedureName = StoredProcedure.Select.SP_GetSubCategory_NearestExpiry.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@CatgID", CatgID)
                                         
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }
    }
}
