using IMSDataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class SystemBLL
    {
        private SystemDAL sysDal=null;
        public SystemBLL() 
        {
            sysDal = new SystemDAL();
        }

        #region Select
        public DataSet SelectAllWarehouse()
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectAllWarehouse();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
       
        public DataSet SelectAllSystems()
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectAllSystems();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
        public DataSet SelectAllWH_HO()
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectAllWH_HO();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
        public DataSet SelectAllPharmacies()
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectAllPharmacies();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;

        }
        public DataSet SelectByID(int id)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectByID(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet SelectSystemRoles() 
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectSystemRoles();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }

        public DataSet SelectSystemsByRoles(string roleName, string systemName) 
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = sysDal.SelectSystemsByRoles(roleName,systemName);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
        }
        #endregion

        #region Delete
        public void Delete(int id)
        {
            try
            {
                sysDal.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion

        #region Update
        public void Update(string name, string description, int sysID, string address, string phoneNum, string faxNum, string pharmacyID, string barterId)
        {
            try
            {
                sysDal.Update(name, description, sysID, address, phoneNum, faxNum, pharmacyID, barterId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region Insert

        public void Insert(string name, string description, string roleName, string address, string phoneNum, string faxNum, string pharmacyID, string barterId, int systemRoles) 
        {
            try
            {
                sysDal.Insert(name,description,roleName,address,phoneNum,faxNum,pharmacyID,barterId,systemRoles);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion



    }
}
