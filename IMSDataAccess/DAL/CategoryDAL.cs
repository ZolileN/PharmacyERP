using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class CategoryDAL : DataAccessbase
    {
        public CategoryDAL()
        {
           
        }

        public DataSet Select(int? categoryID)
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetCategories.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_catID", categoryID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public void Add(string categoryName, int DepartmentID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddNewCategory.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@p_Name", categoryName), 
                                            new SqlParameter("@p_DepartmentID", DepartmentID),  
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Update(int CategoryID, string CategoryName, int DepartmentID)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateSelectedCategory.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@p_Id", CategoryID), 
                                            new SqlParameter("@p_Name", CategoryName), 
                                            new SqlParameter("@p_DepartmentId", DepartmentID),  
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Delete(int CategoryID)
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteCategory.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Id", CategoryID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

    }
}
