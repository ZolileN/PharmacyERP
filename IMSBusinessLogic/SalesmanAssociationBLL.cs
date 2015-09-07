using IMSDataAccess.DAL;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSBusinessLogic
{
    public class SalesmanAssociationBLL
    {
        SalesmanAssociationDAL slmanDAL;
        public SalesmanAssociationBLL() 
        {
            slmanDAL = new SalesmanAssociationDAL();
        }

        #region Select
        public DataSet SelectUnAssociatedStores(long userID) 
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = slmanDAL.SelectUnAssociatedStores(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;
            
        }

        public DataSet SelectAssociatedStores(long userID)
        {
            DataSet dsResults = new DataSet();
            try
            {
                dsResults = slmanDAL.SelectAssociatedStores(userID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return dsResults;

        } 
        #endregion

        #region delete
        public void Delete(long userID) 
        {
            try
            {
                slmanDAL.Delete(userID);
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }
        #endregion

        #region Insert
        public void Insert(long userID, long SystemID)
        {
            try
            {
                slmanDAL.Insert(userID, SystemID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        #endregion
    }
}
