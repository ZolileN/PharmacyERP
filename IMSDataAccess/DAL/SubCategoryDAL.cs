using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class SubCategoryDAL : DataAccessbase
    {
        public SubCategoryDAL()
        {

        }

        public DataSet Select()
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetAllSubCategory.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectDistinctCategory()
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetDistinctCategories.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }
        public DataSet SelectSubCategoriesBasic(int? CategoryID)
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetSubCatBasic.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@p_CategoryID", CategoryID), 
                                            
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }
        public DataSet GetCategoriesdd(int? DepartmentID)
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetCategoryList.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@p_deptID", DepartmentID), 
                                            
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public void Add(string subCategoryName, int CategoryID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddNewSubCategorys.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@p_Name", subCategoryName), 
                                            new SqlParameter("@p_categoryId", CategoryID),  
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Update(int subCategoryID, string subCategoryName, string CategoryID)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateSubCategory.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@p_Id", subCategoryID), 
                                            new SqlParameter("@p_Name", subCategoryName), 
                                            new SqlParameter("@p_CategoryID", CategoryID),   
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Delete(int SubCategoryID)
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteSubCategory.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Id", SubCategoryID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }
    }
}
