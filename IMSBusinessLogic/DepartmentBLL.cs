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
    public class DepartmentBLL
    {
        
        public DepartmentBLL() { }
        // public static List<Department> GetAllDepartment()
        public static DataSet GetAllDepartment(SqlConnection connection)
        {
            
            DataSet resultSet = new DataSet();
            try
            {
                String Query = "SELECT * FROM tblDepartment";

                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
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

        public DataSet GetById(Department val, SqlConnection connection)
        {
            DataSet resultSet = new DataSet();
            try
            {
                String Query = "SELECT * FROM tblDepartment";

                connection.Open();
                SqlCommand command = new SqlCommand(Query, connection);
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

        public void Update(Department dep, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateSelectedDepartment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", dep.DepartmentID);
                command.Parameters.AddWithValue("@p_Name", dep.Name);
                command.Parameters.AddWithValue("@p_Code", dep.Code);


                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Department Successfully Updated ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
            
        }

        public void Delete(Department dep, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_DeleteDepartment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", dep.DepartmentID);
                
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Department Successfully Deleted ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Add(Department dep, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_AddNewDepartment", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@Name", dep.Name);
                command.Parameters.AddWithValue("@Code", dep.Code);


                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Department Successfully Added ");
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
