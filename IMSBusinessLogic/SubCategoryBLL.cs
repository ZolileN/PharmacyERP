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
    public class SubCategoryBLL
    {
        public SubCategoryBLL() { }

        public static DataSet GetAllSubCategories(SqlConnection connection)
        {

            DataSet resultSet = new DataSet();
            try
            {
                //String Query = "SELECT tblSub_Category.Sub_CatID as subCatID, tblSub_Category.Name as subCatName,tblCategory.Name as categoryName,tblDepartment.Name as DepartmentName "+
                //                "FROM tblSub_Category INNER JOIN tblCategory on tblCategory.CategoryID=tblSub_Category.CategoryID "+
                //                " INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId ORDER BY subCatID ASC";

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetAllSubCategory", connection);
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

        public static DataSet GetSubCategoriesBasic(SqlConnection connection)
        {

            DataSet resultSet = new DataSet();
            try
            {
                //String Query = "SELECT tblSub_Category.Sub_CatID as subCatID, tblSub_Category.Name as subCatName,tblCategory.Name as categoryName,tblDepartment.Name as DepartmentName "+
                //                "FROM tblSub_Category INNER JOIN tblCategory on tblCategory.CategoryID=tblSub_Category.CategoryID "+
                //                " INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId ORDER BY subCatID ASC";

                if (connection.State == ConnectionState.Closed)
                {
                    connection.Open();
                }
                SqlCommand command = new SqlCommand("Sp_GetSubCatBasic", connection);
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
        //public DataSet GetById(SubCategory val, SqlConnection connection)
        //{
        //    DataSet resultSet = new DataSet();
        //    try
        //    {
        //        String Query = "SELECT tblSub_Category.Sub_CatID as subCatID, tblSub_Category.Name as subCatName,tblCategory.Name as categoryName,tblDepartment.Name as DepartmentName" +
        //                        " FROM tblSub_Category INNER JOIN tblCategory on tblCategory.CategoryID=tblSub_Category.CategoryID INNER JOIN tblDepartment On tblCategory.DepartmentID=tblDepartment.DepId "
        //                        + "Where tblSub_Category.Sub_CatID = '" + val.SubCategoryID + "' ORDER BY subCatID ASC";


        //        connection.Open();
        //        SqlCommand command = new SqlCommand(Query, connection);
        //        SqlDataAdapter SA = new SqlDataAdapter(command);
        //        SA.Fill(resultSet);

        //    }
        //    catch (Exception exp)
        //    {

        //    }
        //    finally
        //    {
        //        connection.Close();

        //    }
        //    return resultSet;
        //}

        public void Update(SubCategory subCategory, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateSelectedSubCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);
                command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                command.Parameters.AddWithValue("@p_categoryName", subCategory.CategoryName);
                command.Parameters.AddWithValue("@p_DepartmentName", subCategory.DepartmentName);

                command.ExecuteNonQuery();
                //ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product SuccessFully Updated.')", true);
                WebMessageBoxUtil.Show("SubCategory Successfully Updated ");
            }
            catch (Exception exp)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void Delete(SubCategory subCategory, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_DeleteSubCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);

                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Deleted ");
            }
            catch (Exception exp)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void Add(SubCategory subCategory, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_AddNewSubCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                command.Parameters.AddWithValue("@p_categoryName", subCategory.CategoryName);
                command.Parameters.AddWithValue("@p_DepartmentName", subCategory.DepartmentName);
               
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Added ");
            }
            catch (Exception exp)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void AddNew(SubCategory subCategory, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_AddNewSubCategorys", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                command.Parameters.AddWithValue("@p_categoryId", subCategory.CategoryID);
                 
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Added ");
            }
            catch (Exception exp)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }

        public void UpdateSubCat(SubCategory subCategory, SqlConnection connection)
        {
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("Sp_UpdateSubCategory", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);
                command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                command.Parameters.AddWithValue("@p_CategoryID", subCategory.CategoryID);
                
                command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Updated ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
