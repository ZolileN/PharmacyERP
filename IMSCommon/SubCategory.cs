using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCommon
{
    public class SubCategory
    {
        #region fields
        private int _subCategoryID;
        private string _name;
        private int _categoryID;
        private string categoryName;
        private string departmentName;



        #endregion

        public SubCategory() { }

        #region properties
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }

        public int SubCategoryID
        {
            get { return _subCategoryID; }
            set { _subCategoryID = value; }
        }


        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        public string CategoryName
        {
            get { return categoryName; }
            set { categoryName = value; }
        }


        public string DepartmentName
        {
            get { return departmentName; }
            set { departmentName = value; }
        }
        #endregion
    }
}
