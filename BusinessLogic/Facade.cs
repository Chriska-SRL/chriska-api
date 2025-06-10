using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsClient;
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
            RolesSubSystem roles)
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
        public void AddClient(AddClientRequest request) => Clients.AddClient(request);
        public void UpdateClient(UpdateClientRequest request) => Clients.UpdateClient(request);
        public void DeleteClient(DeleteClientRequest request) => Clients.DeleteClient(request);
        public List<ClientResponse> GetAllClients() => Clients.GetAllClients();
        public ClientResponse GetClientById(int id) => Clients.GetClientById(id);

        // Receipts
        public void AddReceipt(AddReceiptRequest request) => Clients.AddReceipt(request);

        // Deliveries
        public void AddDelivery(AddDeliveryRequest request) => Deliveries.AddDelivery(request);
        public void UpdateDelivery(UpdateDeliveryRequest request) => Deliveries.UpdateDelivery(request);
        public void DeleteDelivery(DeleteDeliveryRequest request) => Deliveries.DeleteDelivery(request);

        // Vehicles
        public void AddVehicle(AddVehicleRequest vehicle) => Deliveries.AddVehicle(vehicle);

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
        public void AddPurchase(AddPurchaseRequest purchase) => Purchases.AddPurchase(purchase);
        public void UpdatePurchase(UpdatePurchaseRequest purchase) => Purchases.UpdatePurchase(purchase);
        public void DeletePurchase(DeletePurchaseRequest request) => Purchases.DeletePurchase(request);
        public PurchaseResponse GetPurchaseById(int id) => Purchases.GetPurchaseById(id);
        public List<PurchaseResponse> GetAllPurchases() => Purchases.GetAllPurchases();

        // Purchase Items
        public void AddPurchaseItem(AddPurchaseItemRequest item) => Purchases.AddPurchaseItem(item);
        public void UpdatePurchaseItem(UpdatePurchaseItemRequest item) => Purchases.UpdatePurchaseItem(item);
        public void DeletePurchaseItem(DeletePurchaseItemRequest request) => Purchases.DeletePurchaseItem(request);

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
        public void AddStockMovement(AddStockMovementRequest stockMovement) => Stock.AddStockMovement(stockMovement);
        public StockMovementResponse GetStockMovementById(int id) => Stock.GetStockMovementById(id);
        public List<StockMovementResponse> GetAllStockMovements() => Stock.GetAllStockMovements();

        // Suppliers
        public void AddSupplier(AddSupplierRequest supplier) => Suppliers.AddSupplier(supplier);
        public void UpdateSupplier(UpdateSupplierRequest supplier) => Suppliers.UpdateSupplier(supplier);
        public void DeleteSupplier(DeleteSupplierRequest supplier) => Suppliers.DeleteSupplier(supplier);
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
        public void AddWarehouse(AddWarehouseRequest warehouse) => Warehouses.AddWarehouse(warehouse);
        public void UpdateWarehouse(UpdateWarehouseRequest warehouse) => Warehouses.UpdateWarehouse(warehouse);
        public void DeleteWarehouse(DeleteWarehouseRequest warehouse) => Warehouses.DeleteWarehouse(warehouse);
        public WarehouseResponse GetWarehouseById(int id) => Warehouses.GetWarehouseById(id);
        public List<WarehouseResponse> GetAllWarehouses() => Warehouses.GetAllWarehouses();

        // Zones
        public void AddZone(AddZoneRequest zone) => Zones.AddZone(zone);
        public void UpdateZone(UpdateZoneRequest zone) => Zones.UpdateZone(zone);
        public void DeleteZone(DeleteZoneRequest zone) => Zones.DeleteZone(zone);
        public ZoneResponse GetZoneById(int id) => Zones.GetZoneById(id);
        public List<ZoneResponse> GetAllZones() => Zones.GetAllZones();
    }
}