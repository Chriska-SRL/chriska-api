using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsClient;
using BusinessLogic.DTOs.DTOsDelivery;
using BusinessLogic.DTOs.DTOsOrder;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsReceipt;
using BusinessLogic.DTOs.DTOsRole;
using BusinessLogic.DTOs.DTOsSubCategory;
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

        // Roles
        public void AddRole(AddRoleRequest role) => Roles.AddRole(role);
        public void UpdateRole(UpdateRoleRequest role) => Roles.UpdateRole(role);
        public void DeleteRole(DeleteRoleRequest role) => Roles.DeleteRole(role);
        public RoleResponse GetRoleById(int id) => Roles.GetRoleById(id);
        public List<RoleResponse> GetAllRoles() => Roles.GetAllRoles();

        // Zones
        public void AddZone(AddZoneRequest zone) => Zones.AddZone(zone);
        public void UpdateZone(UpdateZoneRequest zone) => Zones.UpdateZone(zone);
        public void DeleteZone(DeleteZoneRequest zone) => Zones.DeleteZone(zone);
        public ZoneResponse GetZoneById(int id) => Zones.GetZoneById(id);
        public List<ZoneResponse> GetAllZones() => Zones.GetAllZones();

        // Categories
        public void AddCategory(AddCategoryRequest category) => Categories.AddCategory(category);
        public void UpdateCategory(UpdateCategoryRequest category) => Categories.UpdateCategory(category);
        public void DeleteCategory(DeleteCategoryRequest request) => Categories.DeleteCategory(request);
        public List<CategoryResponse> GetAllCategory() => Categories.GetAllCategory();
        public CategoryResponse GetCategoryById(int id) => Categories.GetCategoryById(id);

        // SubCategories
        public void AddSubCategory(AddSubCategoryRequest subCategory) => Categories.AddSubCategory(subCategory);
        public void UpdateSubCategory(UpdateSubCategoryRequest subCategory) => Categories.UpdateSubCategory(subCategory);
        public void DeleteSubCategory(DeleteSubCategoryRequest request) => Categories.DeleteSubCategory(request);
        public SubCategoryResponse GetSubCategoryById(int id) => Categories.GetSubCategoryById(id);
        public List<SubCategoryResponse> GetAllSubCategories() => Categories.GetAllSubCategories();

        // Clientes
        public void AddClient(AddClientRequest request) => Clients.AddClient(request);
        public void UpdateClient(UpdateClientRequest request) => Clients.UpdateClient(request);
        public void DeleteClient(DeleteClientRequest request) => Clients.DeleteClient(request);
        public List<ClientResponse> GetAllClients() => Clients.GetAllClients();
        public ClientResponse GetClientById(int id) => Clients.GetClientById(id);
        public void AddReceipt(AddReceiptRequest request) => Clients.AddReceipt(request);

        // Deliveries
        public void AddDelivery(AddDeliveryRequest request) => Deliveries.AddDelivery(request);
        public void UpdateDelivery(UpdateDeliveryRequest request) => Deliveries.UpdateDelivery(request);
        public void DeleteDelivery(DeleteDeliveryRequest request) => Deliveries.DeleteDelivery(request);

        // Orders
        public void AddOrder(AddOrderRequest request) => Orders.AddOrder(request);
        public void UpdateOrder(UpdateOrderRequest request) => Orders.UpdateOrder(request);
        public void DeleteOrder(DeleteOrderRequest request) => Orders.DeleteOrder(request);
        public OrderResponse GetOrderById(int id) => Orders.GetOrderById(id);
        public List<OrderResponse> GetAllOrders() => Orders.GetAllOrders();
        public void AddOrderItem(OrderItem item) => Orders.AddOrderItem(item);
        public OrderItemResponse GetItemOrderById(int id) => Orders.GetItemOrderById(id);
    }
}
}
