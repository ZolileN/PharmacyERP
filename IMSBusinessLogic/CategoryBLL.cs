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
    public class CategoryBLL
    {
        public CategoryBLL() { }

        public static DataSet GetAllCategories(SqlConnection connection)
        {

            DataSet resultSet = new DataSet();
            try
            {
                String Query = "SELECT  tblCategory.CategoryID as categoryID,tblCategory.Name as categoryName, tblDepartment.Name as DepartmentName "+
                                "FROM tblCategory INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId ORDER BY categoryID ASC ";

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

        public DataSet GetById(Category val, SqlConnection connection)
        {
            DataSet resultSet = new DataSet();
            try
            {
                String Query = "SELECT  tblCategory.CategoryID as categoryID,tblCategory.Name as categoryName, tblDepartment.Name as DepartmentName"+
" FROM tblCategory INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId "+"Where tblCategory.CategoryID = '"+ val.CategoryID +"' ORDER BY categoryID ASC";

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

        public static DataSet GetDistinct(SqlConnection connection)
        {
            DataSet resultSet = new DataSet();
            try
            {
                String Query = "SELECT DISTINCT tblCategory.Name as categoryName " + 
                    "FROM tblCategory INNER JOIN tblDepartment On tblCategory.DepartmentID = tblDepartment.DepId";

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
        public DataSet GetDepListByCategoryName(Category val, SqlConnection connection)
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

        public void Update(Category category, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateSelectedCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", category.CategoryID);
                command.Parameters.AddWithValue("@p_Name", category.Name);
                command.Parameters.AddWithValue("@p_DepartmentId", category.DepartmentID);


                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Updated ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(Category category, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_DeleteCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", category.CategoryID);

                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Deleted ");
            }
            catch (Exception ex)
            {

            }
            finally
            {
                connection.Close();
            }
        }

        public void Add(Category category, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_AddNewCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Name", category.Name);
                command.Parameters.AddWithValue("@p_DepartmentID", category.DepartmentID);
               

                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Added ");
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
