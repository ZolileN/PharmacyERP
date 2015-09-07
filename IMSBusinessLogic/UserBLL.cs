using IMSDataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class UserBLL
    {
        UserDAL userDAL=null;
        public UserBLL() 
        {
            userDAL = new UserDAL();
        }

        #region Select
        public DataSet SelectUserRoles() 
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = userDAL.SelectUserRoles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet SelectByID(string ID) 
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = userDAL.SelectByID(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
        public DataSet SelectUser(string role)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = userDAL.SelectUser(role);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
        #endregion

        #region delete
        public void Delete(long id)
        {
            try
            {
                userDAL.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region insert
        public void Insert(string empID,string password,string userRoleID,string systemID,string firstName,string lastName,string contact,string address,string name,
                string displayName,string email) 
        {
            try
            {
                userDAL.Insert(empID,password,userRoleID,systemID,firstName,lastName,contact,address,name,displayName,email);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region update
        public void Update(string userID,string empID, string password, int userRoleID, int? systemID, string firstName, string lastName, string contact, string address)
        {
            try
            {
                userDAL.Update(userID,empID, password, userRoleID, systemID, firstName, lastName, contact, address);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}
