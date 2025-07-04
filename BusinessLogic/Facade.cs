﻿using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsCost;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsPayment;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsPurchase;
using BusinessLogic.DTOs.DTOsPurchaseItem;
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
        private readonly DeliveriesSubSystem Deliveries;
        private readonly ClientsSubSystem Clients;
        private readonly OrdersSubSystem Orders;
        private readonly ProductsSubSystem Products;
        private readonly PaymentsSubSystem Payments;
        private readonly StockSubSystem Stock;
        private readonly PurchasesSubSystem Purchases;
        private readonly ReturnsSubSystem Returns;
        private readonly SuppliersSubSystem Suppliers;
        private readonly UserSubSystem Users;
        private readonly WarehousesSubSystem Warehouses;
        private readonly ZonesSubSystem Zones;
        private readonly RolesSubSystem Roles;
        private readonly VehicleSubSystem Vehicles;

        public Facade(
            AuthSubSystem auth,
            CategoriesSubSystem categories,
            DeliveriesSubSystem deliveries,
            ClientsSubSystem clients,
            OrdersSubSystem orders,
            ProductsSubSystem products,
            PaymentsSubSystem payments,
            StockSubSystem stock,
            PurchasesSubSystem purchases,
            ReturnsSubSystem returns,
            SuppliersSubSystem suppliers,
            UserSubSystem users,
            WarehousesSubSystem warehouses,
            ZonesSubSystem zones,
            RolesSubSystem roles,
            VehicleSubSystem vehicles)
        {
            Auth = auth;
            Categories = categories;
            Deliveries = deliveries;
            Clients = clients;
            Orders = orders;
            Products = products;
            Payments = payments;
            Stock = stock;
            Purchases = purchases;
            Returns = returns;
            Suppliers = suppliers;
            Users = users;
            Warehouses = warehouses;
            Zones = zones;
            Roles = roles;
            Vehicles = vehicles;
        }
        // Authentication
        public UserResponse? Authenticate(string username, string password) => Auth.Authenticate(username, password);

        // Categories
        public CategoryResponse AddCategory(AddCategoryRequest category) => Categories.AddCategory(category);
        public CategoryResponse UpdateCategory(UpdateCategoryRequest category) => Categories.UpdateCategory(category);
        public CategoryResponse DeleteCategory(int id) => Categories.DeleteCategory(id);
        public List<CategoryResponse> GetAllCategory() => Categories.GetAllCategory();
        public CategoryResponse GetCategoryById(int id) => Categories.GetCategoryById(id);

        // SubCategories
        public SubCategoryResponse AddSubCategory(AddSubCategoryRequest subCategory) => Categories.AddSubCategory(subCategory);
        public SubCategoryResponse UpdateSubCategory(UpdateSubCategoryRequest subCategory) => Categories.UpdateSubCategory(subCategory);
        public SubCategoryResponse DeleteSubCategory(int id) => Categories.DeleteSubCategory(id);
        public SubCategoryResponse GetSubCategoryById(int id) => Categories.GetSubCategoryById(id);
        public List<SubCategoryResponse> GetAllSubCategories() => Categories.GetAllSubCategories();

        // Clients
        public ClientResponse AddClient(AddClientRequest request) => Clients.AddClient(request);
        public ClientResponse UpdateClient(UpdateClientRequest request) => Clients.UpdateClient(request);
        public ClientResponse DeleteClient(int id) => Clients.DeleteClient(id);
        public List<ClientResponse> GetAllClients() => Clients.GetAllClients();
        public ClientResponse GetClientById(int id) => Clients.GetClientById(id);

        // Receipts
        public ReceiptResponse AddReceipt(AddReceiptRequest request) => Clients.AddReceipt(request);
        public ReceiptResponse UpdateReceipt(UpdateReceiptRequest request) => Clients.UpdateReceipt(request);
        public ReceiptResponse DeleteReceipt(int id) => Clients.DeleteReceipt(id);
        public ReceiptResponse GetReceiptById(int id) => Clients.GetReceiptById(id);
        public List<ReceiptResponse> GetAllReceipts() => Clients.GetAllReceipts();

        // Deliveries
        public void AddDelivery(AddDeliveryRequest request) => Deliveries.AddDelivery(request);
        public void UpdateDelivery(UpdateDeliveryRequest request) => Deliveries.UpdateDelivery(request);
        public void DeleteDelivery(DeleteDeliveryRequest request) => Deliveries.DeleteDelivery(request);

        // Vehicles
        public VehicleResponse AddVehicle(AddVehicleRequest request) => Vehicles.AddVehicle(request);
        public VehicleResponse UpdateVehicle(UpdateVehicleRequest request) => Vehicles.UpdateVehicle(request);
        public VehicleResponse DeleteVehicle(int id) => Vehicles.DeleteVehicle(id);
        public VehicleResponse GetVehicleById(int id) => Vehicles.GetVehicleById(id);
        public VehicleResponse GetVehicleByPlate(string plate) => Vehicles.GetVehicleByPlate(plate);
        public List<VehicleResponse> GetAllVehicles() => Vehicles.GetAllVehicles();

        // VehicleCosts
        public VehicleCostResponse AddVehicleCost(AddVehicleCostRequest request) => Vehicles.AddVehicleCost(request);
        public VehicleCostResponse UpdateVehicleCost(UpdateVehicleCostRequest request) => Vehicles.UpdateVehicleCost(request);
        public VehicleCostResponse DeleteVehicleCost(int costId) => Vehicles.DeleteVehicleCost(costId);
        public List<VehicleCostResponse> GetVehicleCosts(int vehicleId) => Vehicles.GetVehicleCosts(vehicleId);
        public List<VehicleCostResponse> GetVehicleCostsByDateRange(int vehicleId, DateTime from, DateTime to) => Vehicles.GetCostsByDateRange(vehicleId, from, to);
        public VehicleCostResponse GetVehicleCostById(int id) => Vehicles.GetVehicleCostById(id);


        // Orders
        public void AddOrder(AddOrderRequest request) => Orders.AddOrder(request);
        public void UpdateOrder(UpdateOrderRequest request) => Orders.UpdateOrder(request);
        public void DeleteOrder(DeleteOrderRequest request) => Orders.DeleteOrder(request);

        // public OrderResponse GetOrderById(int id) => Orders.GetOrderById(id);
        // public List<OrderResponse> GetAllOrders() => Orders.GetAllOrders();

        // OrderItems
        public void AddOrderItem(AddOrderItemRequest item) => Orders.AddOrderItem(item);
        public OrderItemResponse GetOrderItemById(int id) => Orders.GetItemOrderById(id);

        // Payments
        public void AddPayment(AddPaymentRequest payment) => Payments.AddPayment(payment);
        public void UpdatePayment(UpdatePaymentRequest payment) => Payments.UpdatePayment(payment);
        public void DeletePayment(DeletePaymentRequest request) => Payments.DeletePayment(request);
        public PaymentResponse GetPaymentById(int id) => Payments.GetPaymentById(id);

        // Products
        public ProductResponse AddProduct(AddProductRequest product) => Products.AddProduct(product);
        public ProductResponse UpdateProduct(UpdateProductRequest product) => Products.UpdateProduct(product);
        public ProductResponse DeleteProduct(int id) => Products.DeleteProduct(id);
        public ProductResponse GetProductById(int id) => Products.GetProductById(id);
        public List<ProductResponse> GetAllProducts() => Products.GetAllProducts();

        // Purchases
        public PurchaseResponse AddPurchase(AddPurchaseRequest purchase) => Purchases.AddPurchase(purchase);
        public PurchaseResponse UpdatePurchase(UpdatePurchaseRequest purchase) => Purchases.UpdatePurchase(purchase);
        public PurchaseResponse DeletePurchase(int id) => Purchases.DeletePurchase(id);
        public PurchaseResponse GetPurchaseById(int id) => Purchases.GetPurchaseById(id);
        public List<PurchaseResponse> GetAllPurchases() => Purchases.GetAllPurchases();

        // Purchase Items
        public PurchaseItemResponse AddPurchaseItem(AddPurchaseItemRequest item) => Purchases.AddPurchaseItem(item);
        public PurchaseItemResponse UpdatePurchaseItem(UpdatePurchaseItemRequest item) => Purchases.UpdatePurchaseItem(item);
        public PurchaseItemResponse DeletePurchaseItem(int id) => Purchases.DeletePurchaseItem(id);

        // Returns
        public void AddReturnRequest(AddReturnRequest_Request request) => Returns.AddReturnRequest(request);
        public void UpdateReturnRequest(UpdateReturnRequest_Request request) => Returns.UpdateReturnRequest(request);
        public void DeleteReturnRequest(DeleteReturnRequest_Request request) => Returns.DeleteReturnRequest(request);
        public ReturnRequestResponse GetReturnRequestById(int id) => Returns.GetReturnRequestById(id);

        // Roles
        public RoleResponse AddRole(AddRoleRequest role) => Roles.AddRole(role);
        public RoleResponse UpdateRole(UpdateRoleRequest role) => Roles.UpdateRole(role);
        public RoleResponse DeleteRole(int id) => Roles.DeleteRole(id);
        public RoleResponse GetRoleById(int id) => Roles.GetRoleById(id);
        public List<RoleResponse> GetAllRoles() => Roles.GetAllRoles();

        // Stock 
        public StockMovementResponse AddStockMovement(AddStockMovementRequest stockMovement) => Stock.AddStockMovement(stockMovement);
        public StockMovementResponse GetStockMovementById(int id) => Stock.GetStockMovementById(id);
        public List<StockMovementResponse> GetAllStockMovements(DateTime from, DateTime to) => Stock.GetAllStockMovements(from, to);
        public List<StockMovementResponse> GetAllStockMovementsByShelve(int id, DateTime from, DateTime to) => Stock.GetAllStockMovementsByShelve(id, from, to);
        public List<StockMovementResponse> GetAllStockMovementsByWarehouse(int id, DateTime from, DateTime to) => Stock.GetAllStockMovementsByWarehouse(id, from, to);

        // Suppliers
        public SupplierResponse AddSupplier(AddSupplierRequest supplier) => Suppliers.AddSupplier(supplier);
        public SupplierResponse UpdateSupplier(UpdateSupplierRequest supplier) => Suppliers.UpdateSupplier(supplier);
        public SupplierResponse DeleteSupplier(int id) => Suppliers.DeleteSupplier(id);
        public SupplierResponse GetSupplierById(int id) => Suppliers.GetSupplierById(id);
        public List<SupplierResponse> GetAllSuppliers() => Suppliers.GetAllSupplierResponse();

        // Users
        public UserResponse AddUser(AddUserRequest user) => Users.AddUser(user);
        public UserResponse UpdateUser(UpdateUserRequest user) => Users.UpdateUser(user);
        public UserResponse DeleteUser(int id) => Users.DeleteUser(id);
        public UserResponse GetUserById(int id) => Users.GetUserById(id);
        public List<UserResponse> GetAllUsers() => Users.GetAllUsers();
        public string ResetPassword(int userId, string? newPassword = null) => Users.ResetPassword(userId, newPassword);

        // Warehouses
        public WarehouseResponse AddWarehouse(AddWarehouseRequest warehouse) => Warehouses.AddWarehouse(warehouse);
        public WarehouseResponse UpdateWarehouse(UpdateWarehouseRequest warehouse) => Warehouses.UpdateWarehouse(warehouse);
        public WarehouseResponse DeleteWarehouse(int id) => Warehouses.DeleteWarehouse(id);
        public WarehouseResponse GetWarehouseById(int id) => Warehouses.GetWarehouseById(id);
        public List<WarehouseResponse> GetAllWarehouses() => Warehouses.GetAllWarehouses();

        //Shelves
        public ShelveResponse AddShelve(AddShelveRequest warehouse) => Warehouses.AddShelve(warehouse);
        public ShelveResponse UpdateShelve(UpdateShelveRequest warehouse) => Warehouses.UpdateShelve(warehouse);
        public ShelveResponse DeleteShelve(int id) => Warehouses.DeleteShelve(id);
        public ShelveResponse GetShelveById(int id) => Warehouses.GetShelveById(id);
        public List<ShelveResponse> GetAllShelves() => Warehouses.GetAllShelves();
        // Zones
        public ZoneResponse AddZone(AddZoneRequest zone) => Zones.AddZone(zone);
        public ZoneResponse UpdateZone(UpdateZoneRequest zone) => Zones.UpdateZone(zone);
        public ZoneResponse DeleteZone(int id) => Zones.DeleteZone(id);
        public ZoneResponse GetZoneById(int id) => Zones.GetZoneById(id);
        public List<ZoneResponse> GetAllZones() => Zones.GetAllZones();

        // Brand
        public BrandResponse AddBrand(AddBrandRequest brand) => Products.AddBrand(brand);
        public BrandResponse UpdateBrand(UpdateBrandRequest brand) => Products.UpdateBrand(brand);
        public BrandResponse DeleteBrand(int id) => Products.DeleteBrand(id);
        public BrandResponse GetBrand(int id) => Products.GetBrandById(id);
        public List<BrandResponse> GetAllBrand() => Products.GetAllBrands();
    }
}