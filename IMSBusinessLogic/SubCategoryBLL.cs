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
    public class SubCategoryBLL
    {
        public SubCategoryBLL() { }

        public static DataSet GetAllSubCategories()
        {

            DataSet resultSet = new DataSet();
            try
            {
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                resultSet = objSubCategoryDAL.Select();
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetAllSubCategory", connection);
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
        public static DataSet GetDistinct()
        {
            DataSet resultSet = new DataSet();
            try
            {
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                resultSet = objSubCategoryDAL.SelectDistinctCategory();

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

        public DataSet GetCategoriesDropDown(int? DepartmentID)
        {
            DataSet resultSet = new DataSet();
            try
            { 
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                resultSet = objSubCategoryDAL.GetCategoriesdd(DepartmentID);
                 
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
        

    
        public static DataSet GetSubCategoriesBasic()
        {

            DataSet resultSet = new DataSet();
            try
            { 
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                resultSet = objSubCategoryDAL.SelectSubCategoriesBasic();

                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_GetSubCatBasic", connection);
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
         
        public void Update(SubCategory subCategory )
        {
            try
            {
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                objSubCategoryDAL.Update(subCategory.SubCategoryID, subCategory.Name, subCategory.CategoryName);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_UpdateSelectedSubCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);
                //command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                //command.Parameters.AddWithValue("@p_categoryName", subCategory.CategoryName);
                //command.Parameters.AddWithValue("@p_DepartmentName", subCategory.DepartmentName);

                //command.ExecuteNonQuery();
                ////ScriptManager.RegisterClientScriptBlock(this, this.GetType(), "alertMessage", "alert('Product SuccessFully Updated.')", true);
                WebMessageBoxUtil.Show("SubCategory Successfully Updated ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                
            }
        }

        public void Delete(SubCategory subCategory)
        {
            try
            {
                 SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                 objSubCategoryDAL.Delete(subCategory.SubCategoryID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_DeleteSubCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);

                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Deleted ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
            }
        }

      
        public void AddNew(SubCategory subCategory)
        {
            try
            {
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                objSubCategoryDAL.Add(subCategory.Name, subCategory.CategoryID);
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //}
                //SqlCommand command = new SqlCommand("Sp_AddNewSubCategorys", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                //command.Parameters.AddWithValue("@p_categoryId", subCategory.CategoryID);

                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Added ");
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
               
            }
        }

        public void UpdateSubCat(SubCategory subCategory)
        {
            try
            {
                SubCategoryDAL objSubCategoryDAL = new SubCategoryDAL();
                objSubCategoryDAL.Update(subCategory.SubCategoryID, subCategory.Name, subCategory.CategoryName);
                
                //if (connection.State == ConnectionState.Closed)
                //{
                //    connection.Open();
                //} 
                //SqlCommand command = new SqlCommand("Sp_UpdateSubCategory", connection);
                //command.CommandType = CommandType.StoredProcedure;
                //command.Parameters.AddWithValue("@p_Id", subCategory.SubCategoryID);
                //command.Parameters.AddWithValue("@p_Name", subCategory.Name);
                //command.Parameters.AddWithValue("@p_CategoryID", subCategory.CategoryID);
                
                //command.ExecuteNonQuery();
                WebMessageBoxUtil.Show("SubCategory Successfully Updated ");
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
