using IMSCommon;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class InventoryBLL
    {
      
        public static DataSet GetCategoryBasic(SqlConnection connection)
        {
            DataSet resultSet = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetCategoryBasic", connection);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(resultSet);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
            return resultSet;
        }

        public static DataSet GetDepartmentCategory(SqlConnection connection,Category obj)
        {
            DataSet resultSet = new DataSet();
            try
            {
                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetCategoryBasic", connection);
                command.Parameters.AddWithValue("@p_DepartmentID", obj.DepartmentID);
                command.CommandType = CommandType.StoredProcedure;
                SqlDataAdapter SA = new SqlDataAdapter(command);
                SA.Fill(resultSet);

            }
            catch (Exception exp)
            {
                throw exp;
            }
            finally
            {
                if (connection.State == ConnectionState.Open)
                    connection.Close();

            }
            return resultSet;
        }

    }
}
