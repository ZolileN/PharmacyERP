using IMSCommon;
using IMSCommon.Util;
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

        public static DataSet GetAllVendors(SqlConnection connection)
        {

            DataSet resultSet = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetVendor", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(resultSet);

            }
            catch (Exception exp)
            {

            }
            finally
            {
                connection.Close();

            }
            return resultSet;
        }

        public static DataSet GetDistinct(SqlConnection connection,Vendor vendor)
        {
            DataSet resultSet = new DataSet();
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_GetVendorById", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(resultSet);

            }
            catch (Exception exp)
            {

            }
            finally
            {
                connection.Close();

            }
            return resultSet;
        }

        public void Update(Vendor vendor, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateVendor", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);
                command.Parameters.AddWithValue("@p_SupName", vendor.SupName);
                command.Parameters.AddWithValue("@p_Address", vendor.address);
                command.Parameters.AddWithValue("@p_City", vendor.city);
                command.Parameters.AddWithValue("@p_State", vendor.State);
                command.Parameters.AddWithValue("@p_Country", vendor.Country);
                command.Parameters.AddWithValue("@p_Pincode ", vendor.Pincode);
                command.Parameters.AddWithValue("@p_Phone", vendor.Phone);
                command.Parameters.AddWithValue("@p_Fax", vendor.Fax);
                command.Parameters.AddWithValue("@p_Mobile", vendor.Mobile);
                command.Parameters.AddWithValue("@p_Pager", vendor.Pager);
                command.Parameters.AddWithValue("@p_Email", vendor.Email);
                command.Parameters.AddWithValue("@p_ConPerson", vendor.ConPerson);
                command.Parameters.AddWithValue("@p_Discount", vendor.Discount);
                command.Parameters.AddWithValue("@p_Credit", vendor.Credit);
                command.Parameters.AddWithValue("@p_LineID", 1);


                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Updated ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(Vendor vendor, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_DeleteVendor", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Supp_ID", vendor.supp_ID);

                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Deleted ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Add(Vendor vendor, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_AddNewVendor", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_SupName", vendor.SupName);
                command.Parameters.AddWithValue("@p_Address",vendor.address);
                command.Parameters.AddWithValue("@p_City",vendor.city);
                command.Parameters.AddWithValue("@p_State",vendor.State);
                command.Parameters.AddWithValue("@p_Country",vendor.Country);
                command.Parameters.AddWithValue("@p_Pincode ",vendor.Pincode);
                command.Parameters.AddWithValue("@p_Phone",vendor.Phone);
                command.Parameters.AddWithValue("@p_Fax",vendor.Fax);
                command.Parameters.AddWithValue("@p_Mobile",vendor.Mobile);
                command.Parameters.AddWithValue("@p_Pager",vendor.Pager);
                command.Parameters.AddWithValue("@p_Email",vendor.Email);
                command.Parameters.AddWithValue("@p_ConPerson",vendor.ConPerson);
                command.Parameters.AddWithValue("@p_Discount",vendor.Discount);
                command.Parameters.AddWithValue("@p_Credit",vendor.Credit);
                command.Parameters.AddWithValue("@p_LineID",1);

                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Vendor Successfully Added ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }
    }
}
