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
