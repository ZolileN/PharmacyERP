using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCommon
{
    public class Department
    {
        #region Private members
        string _name;
        int _departmentID;
        string _code;
        #endregion

        public Department()
        {

        }

        #region Public properties

        public string Code
        {
            get { return _code; }
            set { _code = value; }
        }
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

        #endregion

    }
}
