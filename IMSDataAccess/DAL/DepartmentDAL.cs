using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class DepartmentDAL : DataAccessbase
    {
        public DepartmentDAL()
        {
           
        }
        public DataSet Select()
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetDepartmentList.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }
        
        public void Add(string depName, string Code)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddNewDepartment.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@Name", depName), 
                                            new SqlParameter("@Code", Code),  
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Update(int departmentId, string depName, string Code)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateSelectedDepartment.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@p_Id", departmentId), 
                                            new SqlParameter("@p_Name", depName), 
                                            new SqlParameter("@p_Code", Code),  
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Delete(int departmentId)
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteDepartment.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Id", departmentId), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

    }
}
