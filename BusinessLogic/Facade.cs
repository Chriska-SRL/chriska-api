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
            ZonesSubSystem zones)
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
        }
    }
}
