using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace IMSDataAccess
{
    public class StoredProcedure
    {
        public enum Insert
        {
            Sp_AddNewDepartment,
            Sp_AddNewSubCategory,
            Sp_AddNewCategory,
            Sp_AddNewProductMaster,
            Sp_AddNewProduct_Detail,
            Sp_AddNewVendor,
            Sp_AddNewSystemRole,
            Sp_AddNewUser_Role,
            Sp_AddNewStock,
            Sp_AddOrderDetails,
            Sp_PalceeNewOrder,
            Sp_AddNewSystem,
            sp_AddNewUser,
            Sp_AddSalesmanSystems,
            Sp_AddNewSubCategorys,
            Sp_GetVendorById,
            Sp_GetVendorByName


        }
        public enum Select
        {
            Sp_GetDepartmentList,
            Sp_GetSubCategoryList,
            Sp_GetCategoryList,
            Sp_GetProductList,
            Sp_GetProductMaster,
            Sp_GetProductMasterById,
            Sp_GetProduct_Detail,
            Sp_GetProduct_DetailById,
            Sp_GetPro_DetailByDId,
            Sp_GetVendor,
            Sp_GetVendorById,
            Sp_GetSystemRoles,
            Sp_GetSystem_RoleById,
            Sp_GetUser_Roles,
            Sp_GetUser_RolesById,
            Sp_GetStockDetails,
            Sp_GetStockDetailByParameters,
            Sp_GetDepartmentById,
            Sp_GetCategoryById,
            Sp_GetSubCategoryById,
            Sp_GetAllDepPerCategoryName,
            Sp_GetDistinctCategories,
            Sp_GetProductExpiryDetails,
            Sp_GetOrderDetails,
            Sp_GetPendingOrderDetails,
            Sp_GetAvailableProduct,
            sp_ManageWarehouse_GetWarehouse,
            sp_GetStores_Pharmacy,
            Sp_GetSystem_ByID,
            sp_GetSystemRoles,
            Sp_GetSystem_ByRoles,
            Sp_GetUserRoles,
            Sp_GetUsers,
            Sp_getWH_HeadOffice,
            Sp_getAllSystems,
            Sp_SearchByUser_ID,
            Sp_GetUnAssociatedStores,
            Sp_GetAssociatedStores,
            Sp_GetCategories,
            Sp_GetAllSubCategory,
            Sp_GetSubCatBasic,
            sp_GetNewNotificationTransferRequest,
            sp_GetAllPendingTransferRequests,
            sp_rptInventoryListDetailsReport,
            sp_rptInventorySummaryReport,
            sp_rptInventoryAdjustmentReport,
            sp_rptInventoryReportByVendorID
            SP_Get_HAAD_Medicine_By_Sub_Category

            


        }
        public enum Delete
        {
            Sp_DeleteDepartment,
            Sp_DeleteSubCategory,
            Sp_DeleteCategory,
            Sp_DeleteProductMasterById,
            Sp_DeleteProduct_DetailById,
            Sp_DeleteVendor,
            Sp_DeleteSystem_RoleById,
            Sp_DeleteUser_RoleById,
            Sp_DeleteOrderDetailByOrderDetailId,
            Sp_DeleteOrderDetailByOrderId,
            Sp_DeleteSystem,
            Sp_DeleteUsers,
            Sp_DeleteSalesmanSystem
        }
        public enum Update
        {
            Sp_UpdateSelectedDepartment,
            Sp_UpdateSelectedSubCategory,
            Sp_UpdateSelectedCategory,
            Sp_UpdateProductMasterById,
            Sp_UpdateProduct_DetailById,
            Sp_UpdateVendor,
            Sp_UpdateOrderDetail,
            Sp_AddRecivedOrderDetails,
            Sp_UpdateSystems,
            Sp_UpdateNewUser,
            Sp_UpdateSubCategory,
            sp_SetSeen

        }
    }
}
