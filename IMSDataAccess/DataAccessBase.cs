using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace IMSDataAccess
{
    public class DataAccessbase
    {
        private string _storedProcedureName;
        protected string StoredProcedureName
        {
            get
            {
                return _storedProcedureName;
            }
            set
            {
                _storedProcedureName = value;
            }
        }
        protected string ConnectionString
        {
            get
            {
                return ConfigurationManager.ConnectionStrings["IMSConnectionString"].ToString();
            }
        }
    }
}
