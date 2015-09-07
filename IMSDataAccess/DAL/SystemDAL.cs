using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    /// <summary>
    /// This Utility DAL serves both pharmacy and warehouse screens as both are type system
    /// </summary>
    public class SystemDAL : DataAccessbase
    {

        #region Select
        public DataSet SelectAllWarehouse()
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_ManageWarehouse_GetWarehouse.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectAllPharmacies()
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_GetStores_Pharmacy.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectByID(int systemID)
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetSystem_ByID.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_SystemID", systemID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds= dbHelper.Run(base.ConnectionString, parameters);
            return ds;
           
        }

        public DataSet SelectSystemRoles() 
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.sp_GetSystemRoles.ToString();
                   
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds= dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectSystemsByRoles(string roleName,string systemName) 
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_GetSystem_ByRoles.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_RoleName", roleName), 
                                             new SqlParameter("@p_systemName", systemName)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString, parameters);
            return ds;
        }

        public DataSet SelectAllWH_HO() 
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_getWH_HeadOffice.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectAllSystems()
        {
            DataSet ds;
            String StoredProcedureName = StoredProcedure.Select.Sp_getAllSystems.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }
        #endregion

        #region Insert
        public void Insert(string name, string description, string roleName, string address, string phoneNum, string faxNum, string pharmacyID, string barterId, int roleID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddNewSystem.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Name", name), 
                                            new SqlParameter("@p_Description", description), 
                                            new SqlParameter("@p_SystemRoleName", roleName), 
                                            new SqlParameter("@p_SystemAddress", address), 
                                            new SqlParameter("@p_SystemPhone", phoneNum), 
                                            new SqlParameter("@p_SystemFax", faxNum), 
                                            new SqlParameter("@p_PharmacyID", pharmacyID), 
                                            new SqlParameter("@p_BarterID", barterId), 
                                            new SqlParameter("@p_SystemRolesID", roleID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        #endregion

        #region Delete
        public void Delete(int systemID) 
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteSystem.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_SystemID", systemID)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        #endregion

        #region update
        public void Update(string name, string description, int sysID, string address, string phoneNum, string faxNum, string pharmacyID, string barterId)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateSystems.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Name", name), 
                                            new SqlParameter("@p_Description", description), 
                                            new SqlParameter("@p_SystemID", sysID),
                                            new SqlParameter("@p_SystemAddress", address), 
                                            new SqlParameter("@p_SystemPhone", phoneNum), 
                                            new SqlParameter("@p_SystemFax", faxNum), 
                                            new SqlParameter("@p_PharmacyID", pharmacyID), 
                                            new SqlParameter("@p_BarterID", barterId)
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
          
        }
        #endregion

    }
}
