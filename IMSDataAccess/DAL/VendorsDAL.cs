using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess.DAL
{
    public class VendorsDAL : DataAccessbase
    {
        public VendorsDAL()
        {

        }
        public DataSet Select()
        {
            DataSet ds;
            StoredProcedureName = StoredProcedure.Select.Sp_GetVendor.ToString();

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            ds = dbHelper.Run(base.ConnectionString);
            return ds;
        }

        public DataSet SelectDistinct(int SupplierID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_GetVendorById.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Supp_ID", SupplierID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            return dbHelper.Run(base.ConnectionString, parameters);
        }
        public DataSet SelectDistinctByName(string SupplierName,bool isStore, int SysID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_GetVendorByName.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Supp_Name", SupplierName), 
                                             new SqlParameter("@p_isStore", isStore), 
                                             new SqlParameter("@p_SysID", SysID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
           return dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Add(string supplierName, string address, string city, string state, string country, string pinCode, string Phone, string Fax, string Mobile, string Pager, string Email, string ConPerson, string Discount, string Credit, string BarterExchangeID, int LineID)
        {
            StoredProcedureName = StoredProcedure.Insert.Sp_AddNewVendor.ToString();

            SqlParameter[] parameters = {   new SqlParameter("@p_SupName", supplierName), 
                                            new SqlParameter("@p_Address", address),  
                                            new SqlParameter("@p_City", city), 
                                            new SqlParameter("@p_State", state),  
                                            new SqlParameter("@p_Country", country), 
                                            new SqlParameter("@p_Pincode", pinCode), 
                                            new SqlParameter("@p_Phone", Phone),  
                                            new SqlParameter("@p_Fax", Fax), 
                                            new SqlParameter("@p_Mobile", Mobile),  
                                            new SqlParameter("@p_Pager", Pager), 
                                            new SqlParameter("@p_Email", Email),  
                                            new SqlParameter("@p_ConPerson", ConPerson), 
                                            new SqlParameter("@p_Discount", Discount), 
                                            new SqlParameter("@p_Credit", Credit),  
                                            new SqlParameter("@p_BarterExchangeID", BarterExchangeID),  
                                            new SqlParameter("@p_LineID", LineID),  
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }
        public void Update(int SuppID, string supplierName, string address, string city, string state, string country, string pinCode, string Phone, string Fax, string Mobile, string Pager, string Email, string ConPerson, string Discount, string Credit, string BarterExchangeID, int LineID)
        {
            StoredProcedureName = StoredProcedure.Update.Sp_UpdateVendor.ToString();
            SqlParameter[] parameters = {   new SqlParameter("@p_Supp_ID", SuppID), 
                                            new SqlParameter("@p_SupName", supplierName), 
                                            new SqlParameter("@p_Address", address),  
                                            new SqlParameter("@p_City", city), 
                                            new SqlParameter("@p_State", state),  
                                            new SqlParameter("@p_Country", country), 
                                            new SqlParameter("@p_Pincode", pinCode), 
                                            new SqlParameter("@p_Phone", Phone),  
                                            new SqlParameter("@p_Fax", Fax), 
                                            new SqlParameter("@p_Mobile", Mobile),  
                                            new SqlParameter("@p_Pager", Pager), 
                                            new SqlParameter("@p_Email", Email),  
                                            new SqlParameter("@p_ConPerson", ConPerson), 
                                            new SqlParameter("@p_Discount", Discount), 
                                            new SqlParameter("@p_Credit", Credit),  
                                            new SqlParameter("@p_BarterExchangeID", BarterExchangeID),  
                                            new SqlParameter("@p_LineID", LineID),    
                                        };
            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }

        public void Delete(int SuppID)
        {
            StoredProcedureName = StoredProcedure.Delete.Sp_DeleteVendor.ToString();

            SqlParameter[] parameters = {   
                                            new SqlParameter("@p_Supp_ID", SuppID), 
                                        };

            DataBaseHelper dbHelper = new DataBaseHelper(StoredProcedureName);
            dbHelper.Run(base.ConnectionString, parameters);
        }
    }
}
