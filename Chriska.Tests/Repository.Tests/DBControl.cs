using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System;
using System.IO;

namespace Chriska.Tests.Repository.Tests
{
    internal class DBControl
    {
        protected static string connectionString = LoadConnectionString();

        private static string LoadConnectionString()
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory()) // Asegura contexto correcto
                .AddJsonFile("appsettings.Development.json", optional: false)
                .Build();

            return config.GetConnectionString("TestDatabase");
        }

        public static void ClearDatabase()
        {
            using var connection = new SqlConnection(connectionString);
            connection.Open();

            using var command = new SqlCommand(@"
        EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

        DELETE FROM Roles_Permissions;
        DELETE FROM Orders_Products;
        DELETE FROM Purchases_Products;
        DELETE FROM Invoices_Products;
        DELETE FROM ProductsStock;
        DELETE FROM StockMovements;
        DELETE FROM CreditNotes;
        DELETE FROM Receipts;
        DELETE FROM Payments;
        DELETE FROM Deliverys;
        DELETE FROM ReturnRequests;
        DELETE FROM Orders;
        DELETE FROM Purchases;
        DELETE FROM Shelves;
        DELETE FROM Vehicles;
        DELETE FROM Users;
        DELETE FROM Clients;
        DELETE FROM Suppliers;
        DELETE FROM SubCategories;
        DELETE FROM Categories;
        DELETE FROM Zones;
        DELETE FROM Warehouses;
        DELETE FROM Products;
        DELETE FROM Costs;
        DELETE FROM Roles;
        DELETE FROM Permissions;

        DBCC CHECKIDENT ('Roles', RESEED, 0);
        DBCC CHECKIDENT ('Users', RESEED, 0);
        DBCC CHECKIDENT ('Suppliers', RESEED, 0);
        DBCC CHECKIDENT ('Categories', RESEED, 0);
        DBCC CHECKIDENT ('SubCategories', RESEED, 0);
        DBCC CHECKIDENT ('Products', RESEED, 0);

        EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';

        INSERT INTO Permissions (Id, Name) VALUES 
        (1, 'CREATE_ROLES'), (2, 'DELETE_ROLES'), (3, 'EDIT_ROLES'), (4, 'VIEW_ROLES'),
        (5, 'CREATE_USERS'), (6, 'DELETE_USERS'), (7, 'EDIT_USERS'), (8, 'VIEW_USERS'),
        (9, 'CREATE_CATEGORIES'), (10, 'DELETE_CATEGORIES'), (11, 'EDIT_CATEGORIES'), (12, 'VIEW_CATEGORIES'),
        (13, 'CREATE_PRODUCTS'), (14, 'DELETE_PRODUCTS'), (15, 'EDIT_PRODUCTS'), (16, 'VIEW_PRODUCTS'),
        (17, 'CREATE_WAREHOUSES'), (18, 'DELETE_WAREHOUSES'), (19, 'EDIT_WAREHOUSES'), (20, 'VIEW_WAREHOUSES'),
        (21, 'CREATE_ZONES'), (22, 'DELETE_ZONES'), (23, 'EDIT_ZONES'), (24, 'VIEW_ZONES'),
        (25, 'CREATE_CLIENTS'), (26, 'DELETE_CLIENTS'), (27, 'EDIT_CLIENTS'), (28, 'VIEW_CLIENTS'),
        (29, 'CREATE_SUPPLIERS'), (30, 'DELETE_SUPPLIERS'), (31, 'EDIT_SUPPLIERS'), (32, 'VIEW_SUPPLIERS'),
        (33, 'CREATE_PURCHASES'), (34, 'DELETE_PURCHASES'), (35, 'EDIT_PURCHASES'), (36, 'VIEW_PURCHASES'),
        (37, 'CREATE_SALES'), (38, 'DELETE_SALES'), (39, 'EDIT_SALES'), (40, 'VIEW_SALES'),
        (41, 'CREATE_ORDERS'), (42, 'DELETE_ORDERS'), (43, 'EDIT_ORDERS'), (44, 'VIEW_ORDERS'),
        (45, 'CREATE_DELIVERIES'), (46, 'DELETE_DELIVERIES'), (47, 'EDIT_DELIVERIES'), (48, 'VIEW_DELIVERIES'),
        (49, 'CREATE_STOCK_MOVEMENTS'), (50, 'DELETE_STOCK_MOVEMENTS'), (51, 'EDIT_STOCK_MOVEMENTS'), (52, 'VIEW_STOCK_MOVEMENTS'),
        (53, 'CREATE_VEHICLES'), (54, 'DELETE_VEHICLES'), (55, 'EDIT_VEHICLES'), (56, 'VIEW_VEHICLES'),
        (57, 'CREATE_PAYMENTS'), (58, 'DELETE_PAYMENTS'), (59, 'EDIT_PAYMENTS'), (60, 'VIEW_PAYMENTS'),
        (61, 'CREATE_COLLECTIONS'), (62, 'DELETE_COLLECTIONS'), (63, 'EDIT_COLLECTIONS'), (64, 'VIEW_COLLECTIONS');
    ", connection);

            command.ExecuteNonQuery();
        }

    }
}
