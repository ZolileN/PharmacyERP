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
            Sp_PalceeNewOrder
            
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
            Sp_GetAvailableProduct

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
            Sp_DeleteOrderDetailByOrderId
        }
        public enum Update
        {
            Sp_UpdateSelectedDepartment,
            Sp_UpdateSelectedSubCategory,
            Sp_UpdateSelectedCategory,
            Sp_UpdateProductMasterById,
            Sp_UpdateProduct_DetailById,
            Sp_UpdateVendor ,
            Sp_UpdateOrderDetail,
            Sp_AddRecivedOrderDetails
        }
    }
}
