using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class UserDAL: DataAccessbase
    {
        public UserDAL() { }

        #region Select
        public DataSet SelectUserRoles() 
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetUserRoles.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectUser(string roleName)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetUsers.ToString();
            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_roleName", roleName)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public DataSet SelectByID(string userID) 
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_SearchByUser_ID.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }
        #endregion

        #region delete
        public void Delete(long systemID)
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteUsers.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_userID", systemID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }
        #endregion

        #region Update
        public void Update(string userID, string empID, string password, int userRoleID, int? systemID, string firstName, string lastName, string contact, string address)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateNewUser.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_UserID", userID),
                                            new SqlParameter("@p_EmpID", empID), 
                                            new SqlParameter("@p_password", password), 
                                            new SqlParameter("@p_UserRoleID", userRoleID), 
                                            new SqlParameter("@p_SystemID", systemID), 
                                            new SqlParameter("@p_FirstName", firstName), 
                                            new SqlParameter("@p_LastName", lastName), 
                                            new SqlParameter("@p_Contact", contact), 
                                            new SqlParameter("@p_Address", address)
                                            
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);

        }
        #endregion

        #region insert
        public void Insert(string empID,string password,string userRoleID,string systemID,string firstName,string lastName,string contact,string address,string name,
                string displayName,string email) 
        {
            StoredProcedureName = StoredProcedure.Insert.sp_AddNewUser.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_EmpID", empID), 
                                            new SqlParameter("@p_password", password), 
                                            new SqlParameter("@p_UserRoleID", userRoleID), 
                                            new SqlParameter("@p_SystemID", systemID), 
                                            new SqlParameter("@p_FirstName", firstName), 
                                            new SqlParameter("@p_LastName", lastName), 
                                            new SqlParameter("@p_Contact", contact), 
                                            new SqlParameter("@p_Address", address), 
                                                                
                                            new SqlParameter("@p_Name", name), 
                                            new SqlParameter("@p_DisplayName", displayName), 
                                            new SqlParameter("@p_email", email)
                                           // new SqlParameter("@ReturnOut",SqlDbType.Int)//.Direction =ParameterDirection.Output
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
            
        }
        #endregion
    }
}
