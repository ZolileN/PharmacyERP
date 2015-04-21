using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IMSCommon.Util
{
    //private enum ProductOrderType
    //{
    //    StoreDirectOrder,
    //    WarehouseDirectOrder,
    //    WarehouseOrderStore,
    //    WarehouseOrderOthers
    //}
    public static class IMSGlobal
    {
        /// <summary>
        /// returns order types
        /// </summary>
        /// <returns></returns>
        public static DataTable GetOrdersType()
        {
            DataTable orderType = new DataTable();
            orderType.Columns.Add("OrderTypeId", typeof(int));
            orderType.Columns.Add("Name", typeof(string));
            orderType.Rows.Add(1, "Exclusive Store Orders");
            //orderType.Rows.Add(2, "Exclusive WareHouse Orders");
            orderType.Rows.Add(2, "Warehouse Product for all");
            orderType.Rows.Add(3, "Warehouse Product for stores");
            return orderType;
        }
    }
}
