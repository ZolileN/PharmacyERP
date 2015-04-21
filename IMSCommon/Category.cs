using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCommon
{
    public class Category
    {
        #region Private fields
        private int _categoryID;
        private string _name;
        private int _departmentID;

        #endregion

        public Category()
        {

        }

        #region properties
        public int DepartmentID
        {
            get { return _departmentID; }
            set { _departmentID = value; }
        }
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }


        public int CategoryID
        {
            get { return _categoryID; }
            set { _categoryID = value; }
        }

        #endregion
    }
}
