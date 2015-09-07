using IMSCommon;
using IMSCommon.Util;
using IMSDataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class VendorBLL
    {
        public VendorBLL() { }

        public static DataSet GetAllVendors()
        {

            DataSet resultSet = new DataSet();
            try
            {
                VendorsDAL objVendorDAL = new VendorsDAL();
                resultSet = objVendorDAL.Select();
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetVendor", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //SqlDataAdapter SA = new SqlDataAdapter(command);
                //SA.Fill(resultSet);

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

        public static DataSet GetDistinct(Vendor vendor)
        {
            DataSet resultSet = new DataSet();
            try
            {
                VendorsDAL objVendorDAL = new VendorsDAL();
                resultSet = objVendorDAL.SelectDistinct(vendor.supp_ID);

                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetVendorById", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);
                //SqlDataAdapter SA = new SqlDataAdapter(command);
                //SA.Fill(resultSet);

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
        public void Add(Vendor vendor)
        {
            try
            {
                VendorsDAL objVendorDAL = new VendorsDAL();
                objVendorDAL.Add(vendor.SupName, vendor.address, vendor.city, vendor.State, vendor.Country, vendor.Pincode, vendor.Phone,
                                vendor.Fax, vendor.Mobile, vendor.Pager, vendor.Email, vendor.ConPerson, vendor.Discount, vendor.Credit, vendor.BarterExchangeID, 1);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_AddNewVendor", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_SupName", vendor.SupName);
                //command.Parameters.AddWithValue("@p_Address", vendor.address);
                //command.Parameters.AddWithValue("@p_City", vendor.city);
                //command.Parameters.AddWithValue("@p_State", vendor.State);
                //command.Parameters.AddWithValue("@p_Country", vendor.Country);
                //command.Parameters.AddWithValue("@p_Pincode ", vendor.Pincode);
                //command.Parameters.AddWithValue("@p_Phone", vendor.Phone);
                //command.Parameters.AddWithValue("@p_Fax", vendor.Fax);
                //command.Parameters.AddWithValue("@p_Mobile", vendor.Mobile);
                //command.Parameters.AddWithValue("@p_Pager", vendor.Pager);
                //command.Parameters.AddWithValue("@p_Email", vendor.Email);
                //command.Parameters.AddWithValue("@p_ConPerson", vendor.ConPerson);
                //command.Parameters.AddWithValue("@p_Discount", vendor.Discount);
                //command.Parameters.AddWithValue("@p_Credit", vendor.Credit);
                //command.Parameters.AddWithValue("@p_BarterExchangeID", vendor.BarterExchangeID);
                //command.Parameters.AddWithValue("@p_LineID", 1);

                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Added ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void Update(Vendor vendor)
        {
            try
            {
                VendorsDAL objVendorDAL = new VendorsDAL();
                objVendorDAL.Update(vendor.supp_ID, vendor.SupName, vendor.address, vendor.city, vendor.State, vendor.Country, vendor.Pincode, vendor.Phone,
                                vendor.Fax, vendor.Mobile, vendor.Pager, vendor.Email, vendor.ConPerson, vendor.Discount, vendor.Credit, vendor.BarterExchangeID, 1);
              

                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_UpdateVendor", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);
                //command.Parameters.AddWithValue("@p_SupName", vendor.SupName);
                //command.Parameters.AddWithValue("@p_Address", vendor.address);
                //command.Parameters.AddWithValue("@p_City", vendor.city);
                //command.Parameters.AddWithValue("@p_State", vendor.State);
                //command.Parameters.AddWithValue("@p_Country", vendor.Country);
                //command.Parameters.AddWithValue("@p_Pincode ", vendor.Pincode);
                //command.Parameters.AddWithValue("@p_Phone", vendor.Phone);
                //command.Parameters.AddWithValue("@p_Fax", vendor.Fax);
                //command.Parameters.AddWithValue("@p_Mobile", vendor.Mobile);
                //command.Parameters.AddWithValue("@p_Pager", vendor.Pager);
                //command.Parameters.AddWithValue("@p_Email", vendor.Email);
                //command.Parameters.AddWithValue("@p_ConPerson", vendor.ConPerson);
                //command.Parameters.AddWithValue("@p_Discount", vendor.Discount);
                //command.Parameters.AddWithValue("@p_Credit", vendor.Credit);
                //command.Parameters.AddWithValue("@p_LineID", 1);


                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Updated ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

        public void Delete(Vendor vendor)
        {
            try
            {
                VendorsDAL objVendorDAL = new VendorsDAL();
                objVendorDAL.Delete(vendor.supp_ID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_DeleteVendor", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);

                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Deleted ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

       

        public static DataSet GetDistinctByNane(Vendor vendor, int SysID, bool isStore)
        {
            DataSet resultSet = new DataSet();
            try
            {
                 
                VendorsDAL objVendorDAL = new VendorsDAL();
                resultSet = objVendorDAL.SelectDistinctByName(vendor.SupName, isStore, SysID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetVendorByName", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Supp_Name", vendor.SupName);
                //command.Parameters.AddWithValue("@p_isStore", isStore);
                //command.Parameters.AddWithValue("@p_SysID", SysID );

                //SqlDataAdapter SA = new SqlDataAdapter(command);
                //SA.Fill(resultSet);

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
    }
}
