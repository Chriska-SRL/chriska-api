using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.DTOs;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsDiscount;
using BusinessLogic.DTOs.DTOsDistribution;
using BusinessLogic.DTOs.DTOsDocumentClient;
using BusinessLogic.DTOs.DTOsImage;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderRequest;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs.DTOsReturnRequest;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.DTOs.DTOsSubCategory;
using BusinessLogic.DTOs.DTOsSupplier;
using BusinessLogic.DTOs.DTOsUser;
using BusinessLogic.DTOs.DTOsVehicle;
using BusinessLogic.DTOs.DTOsWarehouse;
using BusinessLogic.DTOs.DTOsZone;
using BusinessLogic.SubSystem;

namespace BusinessLogic
{
    public class Facade
    {
        private readonly AuthSubSystem Auth;
        private readonly CategoriesSubSystem Categories;
        private readonly ClientsSubSystem Clients;
        private readonly ProductsSubSystem Products;
        private readonly StockSubSystem Stock;
        private readonly SuppliersSubSystem Suppliers;
        private readonly UserSubSystem Users;
        private readonly WarehousesSubSystem Warehouses;
        private readonly ZonesSubSystem Zones;
        private readonly RolesSubSystem Roles;
        private readonly VehicleSubSystem Vehicles;
        private readonly BrandSubSystem Brand;
        private readonly DiscountsSubSystem Discounts;
        private readonly OrderRequestSubSystem OrderRequests;
        private readonly ReturnRequestSubSystem ReturnRequest;
        private readonly OrderSubSystem Orders;
        private readonly DeliveriesSubSystem Deliveries;
        private readonly DistributionSubSystem Distributions;
        private readonly ReceiptSubSystem Receipt;

        public Facade(
            AuthSubSystem auth,
            CategoriesSubSystem categories,
            ClientsSubSystem clients,
            ProductsSubSystem products,
            StockSubSystem stock,
            SuppliersSubSystem suppliers,
            UserSubSystem users,
            WarehousesSubSystem warehouses,
            ZonesSubSystem zones,
            RolesSubSystem roles,
            VehicleSubSystem vehicles,
            BrandSubSystem brand,
            DiscountsSubSystem discounts,
            OrderSubSystem orders,
            OrderRequestSubSystem orderRequests,
            ReturnRequestSubSystem returnRequest,
            DeliveriesSubSystem deliveries,
            DistributionSubSystem distributions,
            ReceiptSubSystem receipt)
        {
            Auth = auth;
            Categories = categories;
            Clients = clients;
            Products = products;
            Stock = stock;
            Suppliers = suppliers;
            Users = users;
            Warehouses = warehouses;
            Zones = zones;
            Roles = roles;
            Vehicles = vehicles;
            Brand = brand;
            Discounts = discounts;
            OrderRequests = orderRequests;
            ReturnRequest = returnRequest;
            Orders = orders;
            Deliveries = deliveries;
            Distributions = distributions;
            Receipt = receipt;
        }

        // --- Auth ---
        public Task<UserResponse?> AuthenticateAsync(string username, string password)
            => Auth.AuthenticateAsync(username, password);

        // --- Roles ---
        public async Task<RoleResponse> AddRoleAsync(AddRoleRequest request) => await Roles.AddRoleAsync(request);
        public async Task<RoleResponse> UpdateRoleAsync(UpdateRoleRequest request) => await Roles.UpdateRoleAsync(request);
        public async Task<RoleResponse> DeleteRoleAsync(DeleteRequest request) => await Roles.DeleteRoleAsync(request);
        public async Task<RoleResponse> GetRoleByIdAsync(int id) => await Roles.GetRoleByIdAsync(id);
        public async Task<List<RoleResponse>> GetAllRolesAsync(QueryOptions options) => await Roles.GetAllRolesAsync(options);

        // --- Clients ---
        public async Task<ClientResponse> AddClientAsync(AddClientRequest request) => await Clients.AddClientAsync(request);
        public async Task<ClientResponse> UpdateClientAsync(UpdateClientRequest request) => await Clients.UpdateClientAsync(request);
        public async Task DeleteClientAsync(DeleteRequest request) => await Clients.DeleteClientAsync(request);
        public async Task<ClientResponse> GetClientByIdAsync(int id) => await Clients.GetClientByIdAsync(id);
        public async Task<List<ClientResponse>> GetAllClientsAsync(QueryOptions options) => await Clients.GetAllClientsAsync(options);

        // --- Categories ---
        public async Task<CategoryResponse> AddCategoryAsync(AddCategoryRequest request) => await Categories.AddCategoryAsync(request);
        public async Task<CategoryResponse> UpdateCategoryAsync(UpdateCategoryRequest request) => await Categories.UpdateCategoryAsync(request);
        public async Task DeleteCategoryAsync(DeleteRequest request) => await Categories.DeleteCategoryAsync(request);
        public async Task<CategoryResponse> GetCategoryByIdAsync(int id) => await Categories.GetCategoryByIdAsync(id);
        public async Task<List<CategoryResponse>> GetAllCategoriesAsync(QueryOptions options) => await Categories.GetAllCategoriesAsync(options);

        // --- SubCategories ---
        public async Task<SubCategoryResponse> AddSubCategoryAsync(AddSubCategoryRequest request) => await Categories.AddSubCategoryAsync(request);
        public async Task<SubCategoryResponse> UpdateSubCategoryAsync(UpdateSubCategoryRequest request) => await Categories.UpdateSubCategoryAsync(request);
        public async Task DeleteSubCategoryAsync(DeleteRequest request) => await Categories.DeleteSubCategoryAsync(request);
        public async Task<SubCategoryResponse> GetSubCategoryByIdAsync(int id) => await Categories.GetSubCategoryByIdAsync(id);
        public async Task<List<SubCategoryResponse>> GetAllSubCategoriesAsync(QueryOptions options) => await Categories.GetAllSubCategoriesAsync(options);

        // --- Brands ---
        public async Task<BrandResponse> AddBrandAsync(AddBrandRequest request) => await Brand.AddBrandAsync(request);
        public async Task<BrandResponse> UpdateBrandAsync(UpdateBrandRequest request) => await Brand.UpdateBrandAsync(request);
        public async Task<BrandResponse> DeleteBrandAsync(DeleteRequest request) => await Brand.DeleteBrandAsync(request);
        public async Task<BrandResponse> GetBrandByIdAsync(int id) => await Brand.GetBrandByIdAsync(id);
        public async Task<List<BrandResponse>> GetAllBrandsAsync(QueryOptions options) => await Brand.GetAllBrandsAsync(options);

        // --- Products ---
        public async Task<ProductResponse> AddProductAsync(ProductAddRequest request) => await Products.AddProductAsync(request);
        public async Task<ProductResponse> UpdateProductAsync(ProductUpdateRequest request) => await Products.UpdateProductAsync(request);
        public async Task DeleteProductAsync(DeleteRequest request) => await Products.DeleteProductAsync(request);
        public async Task<ProductResponse> GetProductByIdAsync(int id) => await Products.GetProductByIdAsync(id);
        public async Task<List<ProductResponse>> GetAllProductsAsync(QueryOptions options) => await Products.GetAllProductsAsync(options);
        public async Task<string> UploadProductImageAsync(AddImageRequest request) => await Products.UploadProductImageAsync(request);
        public async Task DeleteProductImageAsync(int id) => await Products.DeleteProductImageAsync(id);
        public async Task<ProductResponse> GetProductByIdWithDiscountsAsync(int id) => await Products.GetProductByIdWithDiscountsAsync(id);

        // --- Stock Movements ---
        public async Task<StockMovementResponse> AddStockMovementAsync(AddStockMovementRequest request) => await Stock.AddStockMovementAsync(request);
        public async Task<StockMovementResponse> GetStockMovementByIdAsync(int id) => await Stock.GetStockMovementByIdAsync(id);
        public async Task<List<StockMovementResponse>> GetAllStockMovementsAsync(QueryOptions options) => await Stock.GetAllStockMovementsAsync(options);

        // --- Suppliers ---
        public async Task<SupplierResponse> AddSupplierAsync(AddSupplierRequest request) => await Suppliers.AddSupplierAsync(request);
        public async Task<SupplierResponse> UpdateSupplierAsync(UpdateSupplierRequest request) => await Suppliers.UpdateSupplierAsync(request);
        public async Task DeleteSupplierAsync(DeleteRequest request) => await Suppliers.DeleteSupplierAsync(request);
        public async Task<SupplierResponse> GetSupplierByIdAsync(int id) => await Suppliers.GetSupplierByIdAsync(id);
        public async Task<List<SupplierResponse>> GetAllSuppliersAsync(QueryOptions options) => await Suppliers.GetAllSuppliersAsync(options);

        // --- Users ---
        public async Task<UserResponse> AddUserAsync(AddUserRequest request) => await Users.AddUserAsync(request);
        public async Task<UserResponse> UpdateUserAsync(UpdateUserRequest request) => await Users.UpdateUserAsync(request);
        public async Task DeleteUserAsync(DeleteRequest request) => await Users.DeleteUserAsync(request);
        public async Task<UserResponse> GetUserByIdAsync(int id) => await Users.GetUserByIdAsync(id);
        public async Task<List<UserResponse>> GetAllUsersAsync(QueryOptions options) => await Users.GetAllUsersAsync(options);
        public async Task<string> ResetPasswordAsync(ResetPasswordRequest request) => await Users.ResetPasswordAsync(request);

        // --- Warehouses ---
        public async Task<WarehouseResponse> AddWarehouseAsync(AddWarehouseRequest request) => await Warehouses.AddWarehouseAsync(request);
        public async Task<WarehouseResponse> UpdateWarehouseAsync(UpdateWarehouseRequest request) => await Warehouses.UpdateWarehouseAsync(request);
        public async Task DeleteWarehouseAsync(DeleteRequest request) => await Warehouses.DeleteWarehouseAsync(request);
        public async Task<WarehouseResponse> GetWarehouseByIdAsync(int id) => await Warehouses.GetWarehouseByIdAsync(id);
        public async Task<List<WarehouseResponse>> GetAllWarehousesAsync(QueryOptions options) => await Warehouses.GetAllWarehousesAsync(options);

        // --- Shelves ---
        public async Task<ShelveResponse> AddShelveAsync(AddShelveRequest request) => await Warehouses.AddShelveAsync(request);
        public async Task<ShelveResponse> UpdateShelveAsync(UpdateShelveRequest request) => await Warehouses.UpdateShelveAsync(request);
        public async Task DeleteShelveAsync(DeleteRequest request) => await Warehouses.DeleteShelveAsync(request);
        public async Task<ShelveResponse> GetShelveByIdAsync(int id) => await Warehouses.GetShelveByIdAsync(id);
        public async Task<List<ShelveResponse>> GetAllShelvesAsync(QueryOptions options) => await Warehouses.GetAllShelvesAsync(options);

        // --- Zones ---
        public async Task<ZoneResponse> AddZoneAsync(AddZoneRequest request) => await Zones.AddZoneAsync(request);
        public async Task<ZoneResponse> UpdateZoneAsync(UpdateZoneRequest request) => await Zones.UpdateZoneAsync(request);
        public async Task DeleteZoneAsync(DeleteRequest request) => await Zones.DeleteZoneAsync(request);
        public async Task<ZoneResponse> GetZoneByIdAsync(int id) => await Zones.GetZoneByIdAsync(id);
        public async Task<List<ZoneResponse>> GetAllZonesAsync(QueryOptions options) => await Zones.GetAllZonesAsync(options);
        public async Task<string> UploadZoneImageAsync(AddImageRequest request) => await Zones.UploadZoneImageAsync(request);
        public async Task DeleteZoneImageAsync(int id) => await Zones.DeleteZoneImageAsync(id);

        // --- Vehicles ---
        public async Task<VehicleResponse> AddVehicleAsync(AddVehicleRequest request) => await Vehicles.AddVehicleAsync(request);
        public async Task<VehicleResponse> UpdateVehicleAsync(UpdateVehicleRequest request) => await Vehicles.UpdateVehicleAsync(request);
        public async Task DeleteVehicleAsync(DeleteRequest request) => await Vehicles.DeleteVehicleAsync(request);
        public async Task<VehicleResponse> GetVehicleByIdAsync(int id) => await Vehicles.GetVehicleByIdAsync(id);
        public async Task<List<VehicleResponse>> GetAllVehiclesAsync(QueryOptions options) => await Vehicles.GetAllVehiclesAsync(options);

        // --- Vehicle Costs ---
        public async Task<VehicleCostResponse> AddVehicleCostAsync(AddVehicleCostRequest request) => await Vehicles.AddVehicleCostAsync(request);
        public async Task<VehicleCostResponse> UpdateVehicleCostAsync(UpdateVehicleCostRequest request) => await Vehicles.UpdateVehicleCostAsync(request);
        public async Task DeleteVehicleCostAsync(DeleteRequest request) => await Vehicles.DeleteVehicleCostAsync(request);
        public async Task<VehicleCostResponse> GetVehicleCostByIdAsync(int id) => await Vehicles.GetVehicleCostByIdAsync(id);
        public async Task<List<VehicleCostResponse>> GetAllCosts(QueryOptions query) => await Vehicles.GetAllCosts(query);

        // --- Discounts ---
        public async Task<DiscountResponse?> AddDiscountAsync(DiscountAddRequest request) => await Discounts.AddDiscountAsync(request);
        public async Task<DiscountResponse?> UpdateDiscountAsync(DiscountUpdateRequest request) => await Discounts.UpdateDiscountAsync(request);
        public async Task DeleteDiscountAsync(DeleteRequest request) => await Discounts.DeleteDiscountAsync(request);
        public async Task<DiscountResponse?> GetDiscountByIdAsync(int id) => await Discounts.GetDiscountByIdAsync(id);
        public async Task<List<DiscountResponse>?> GetAllDiscountsAsync(QueryOptions query) => await Discounts.GetAllDiscountsAsync(query);
        public async Task<DiscountResponse?> GetBestDiscountAsync(int productId, int clientId) => await Discounts.GetBestDiscountAsync(productId, clientId);

        // --- OrderRequests ---
        public async Task<OrderRequestResponse?> AddOrderRequestAsync(OrderRequestAddRequest request) => await OrderRequests.AddOrderRequestAsync(request);
        public async Task<OrderRequestResponse?> UpdateOrderRequestAsync(OrderRequestUpdateRequest request) => await OrderRequests.UpdateOrderRequestAsync(request);
        public async Task DeleteOrderRequestAsync(DeleteRequest request) => await OrderRequests.DeleteOrderRequestAsync(request);
        public async Task<OrderRequestResponse?> GetOrderRequestByIdAsync(int id) => await OrderRequests.GetOrderRequestByIdAsync(id);
        public async Task<List<OrderRequestResponse>?> GetAllOrderRequestsAsync(QueryOptions query) => await OrderRequests.GetAllOrderRequestsAsync(query);
        public async Task<OrderRequestResponse?> ChangeStatusOrderRequestAsync(int id, DocumentClientChangeStatusRequest request) => await OrderRequests.ChangeStatusOrderRequestAsync(id, request);


        // --- ReturnRequests ---
        public async Task<ReturnRequestResponse?> AddReturnRequestAsync(ReturnRequestAddRequest request) => await ReturnRequest.AddReturnRequestAsync(request);
        public async Task<ReturnRequestResponse?> UpdateReturnRequestAsync(ReturnRequestUpdateRequest request) => await ReturnRequest.UpdateReturnRequestAsync(request);
        public async Task DeleteReturnRequestAsync(DeleteRequest request) => await ReturnRequest.DeleteReturnRequestAsync(request);
        public async Task<ReturnRequestResponse?> GetReturnRequestByIdAsync(int id) => await ReturnRequest.GetReturnRequestByIdAsync(id);
        public async Task<List<ReturnRequestResponse>?> GetAllReturnRequestsAsync(QueryOptions query) => await ReturnRequest.GetAllReturnRequestsAsync(query);
        public async Task<ReturnRequestResponse?> ChangeStatusReturnRequestAsync(int id, DocumentClientChangeStatusRequest request) => await ReturnRequest.ChangeStatusReturnRequestAsync(id, request);
        

        // --- Orders ---
        public async Task<OrderResponse?> UpdateOrderAsync(OrderUpdateRequest request) => await Orders.UpdateOrderAsync(request);
        public async Task<OrderResponse?> GetOrderByIdAsync(int id) => await Orders.GetOrderByIdAsync(id);
        public async Task<List<OrderResponse?>> GetAllOrdersAsync(QueryOptions query) => await Orders.GetAllOrdersAsync(query);
        public async Task<OrderResponse?> ChangeStatusOrderAsync(int id, DocumentClientChangeStatusRequest request) => await Orders.ChangeStatusOrderAsync(id, request);

        //Deliveries
        public async Task<DeliveryResponse?> UpdateDeliveryAsync(DeliveryUpdateRequest request) => await Deliveries.UpdateDeliveryAsync(request);
        public async Task<DeliveryResponse?> GetDeliveryByIdAsync(int id) => await Deliveries.GetDeliveryByIdAsync(id);
        public async Task<List<DeliveryResponse?>> GetAllDeliveriesAsync(QueryOptions query) => await Deliveries.GetAllDeliveriesAsync(query);
        public async Task<DeliveryResponse?> ChangeStatusDeliveryAsync(int id, DocumentClientChangeStatusRequest request)  => await Deliveries.ChangeStatusDeliveryAsync(id, request);

        // --- Distribution ---
        public async Task<DistributionResponse?> AddDistributionAsync(DistributionAddRequest request) => await Distributions.AddDistributionAsync(request);
        public async Task<DistributionResponse?> UpdateDistributionAsync(DistributionUpdateRequest request) => await Distributions.UpdateDistributionAsync(request);
        public async Task DeleteDistributionAsync(DeleteRequest request) => await Distributions.DeleteDistributionAsync(request);
        public async Task<DistributionResponse?> GetDistributionByIdAsync(int id) => await Distributions.GetDistributionByIdAsync(id);
        public async Task<List<DistributionResponse>?> GetAllDistributionsAsync(QueryOptions query) => await Distributions.GetAllDistributionsAsync(query);
        public async Task<List<DeliveryResponse?>> GetConfirmedDeliveriesByClientIdAsync(int ClientId,QueryOptions query) => await Deliveries.GetConfirmedDeliveriesByClientIdAsync(ClientId,query);

        // --- Receipts ---
        public async Task<ReceiptResponse> AddReceiptAsync(ReceiptAddRequest request) => await Receipt.AddReceiptAsync(request);

        public async Task<ReceiptResponse> UpdateReceiptAsync(ReceiptUpdateRequest request) => await Receipt.UpdateReceiptAsync(request);

        public async Task<ReceiptResponse> DeleteReceiptAsync(DeleteRequest request) => await Receipt.DeleteReceiptAsync(request);

        public async Task<ReceiptResponse> GetReceiptByIdAsync(int id) => await Receipt.GetReceiptByIdAsync(id);

        public async Task<List<ReceiptResponse>> GetAllReceiptsAsync(QueryOptions options) => await Receipt.GetAllReceiptsAsync(options);

    }
}
