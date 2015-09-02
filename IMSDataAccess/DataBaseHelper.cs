using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;

namespace IMSDataAccess
{
    public class DataBaseHelper:DataAccessbase
    {
       
        public DataBaseHelper(string storedProcedure) 
        {
            StoredProcedureName = storedProcedure;
        }

        public int Run(SqlTransaction MySqlTransaction, string connectionString, SqlParameter[] parameters)
        {
            int mRet;
            mRet = SqlHelper.ExecuteNonQuery(MySqlTransaction, CommandType.StoredProcedure, StoredProcedureName, parameters);
            return mRet;
        }

        public DataSet Run(string connectionString, SqlParameter[] parameters) 
        {
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(connectionString, StoredProcedureName, parameters);
            return ds;
        }

        public DataSet Run(string connectionString)
        {
            DataSet ds;
            ds = SqlHelper.ExecuteDataset(connectionString, CommandType.StoredProcedure, StoredProcedureName);
            return ds;
        }
    }
}
