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
    public class CategoryBLL
    {
        public CategoryBLL() { }

        public static DataSet GetAllCategories()
        {

            DataSet resultSet = new DataSet();
            try
            {
                CategoryDAL objCategoryDAL = new CategoryDAL();
                resultSet = objCategoryDAL.Select(null);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetCategories", connection);
                //command.CommandType = CommandType.StoredProcedure;
                ////  String Query = "SELECT  tblCategory.CategoryID as categoryID,tblCategory.Name as categoryName, tblDepartment.Name as DepartmentName "+
                ////                "FROM tblCategory INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId ORDER BY categoryID ASC ";
                //command.Parameters.AddWithValue("@p_catID ", DBNull.Value);
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

        public DataSet GetById(Category val )
        {
            DataSet resultSet = new DataSet();
            try
            {
                CategoryDAL objCategoryDAL = new CategoryDAL();
                resultSet = objCategoryDAL.Select(val.CategoryID);
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

        public void Add(Category category )
        {
            try
            {
                CategoryDAL objCategoryDAL = new CategoryDAL();
                objCategoryDAL.Add(category.Name, category.DepartmentID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_AddNewCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Name", category.Name);
                //command.Parameters.AddWithValue("@p_DepartmentID", category.DepartmentID);


                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Added ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
              
            }
        }

        public void Update(Category category )
        {
            try
            {
                CategoryDAL objCategoryDAL = new CategoryDAL();
                objCategoryDAL.Update(category.CategoryID, category.Name, category.DepartmentID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_UpdateSelectedCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Id", category.CategoryID);
                //command.Parameters.AddWithValue("@p_Name", category.Name);
                //command.Parameters.AddWithValue("@p_DepartmentId", category.DepartmentID);


                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Updated ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public void Delete(Category category)
        {
            try
            {
                CategoryDAL objCategoryDAL = new CategoryDAL();
                objCategoryDAL.Delete(category.CategoryID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_DeleteCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Id", category.CategoryID);

                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("Category Successfully Deleted ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {

            }
        }

        
    }
}
