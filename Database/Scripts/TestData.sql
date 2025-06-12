/*
Plantilla de script posterior a la implementación							
--------------------------------------------------------------------------------------
 Este archivo contiene instrucciones de SQL que se anexarán al script de compilación.		
 Use la sintaxis de SQLCMD para incluir un archivo en el script posterior a la implementación.			
 Ejemplo:      :r .\miArchivo.sql								
 Use la sintaxis de SQLCMD para hacer referencia a una variable en el script posterior a la implementación.		
 Ejemplo:      :setvar TableName miTabla							
               SELECT * FROM [$(TableName)]					
--------------------------------------------------------------------------------------
*/
-- Desactivar restricciones de clave foránea
EXEC sp_msforeachtable "ALTER TABLE ? NOCHECK CONSTRAINT ALL";

-- Borrar datos de todas las tablas (en orden lógico: relaciones primero)
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

-- Reiniciar IDENTITY (autoincremento) de tablas
DBCC CHECKIDENT ('Roles', RESEED, 0);
DBCC CHECKIDENT ('Users', RESEED, 0);
DBCC CHECKIDENT ('Suppliers', RESEED, 0);
DBCC CHECKIDENT ('Categories', RESEED, 0);
DBCC CHECKIDENT ('SubCategories', RESEED, 0);
DBCC CHECKIDENT ('Products', RESEED, 0);

-- Reactivar restricciones de clave foránea
EXEC sp_msforeachtable "ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL";

INSERT INTO Permissions (Id, Name) VALUES (1, 'CREATE_ROLES');
INSERT INTO Permissions (Id, Name) VALUES (2, 'DELETE_ROLES');
INSERT INTO Permissions (Id, Name) VALUES (3, 'EDIT_ROLES');
INSERT INTO Permissions (Id, Name) VALUES (4, 'VIEW_ROLES');

INSERT INTO Permissions (Id, Name) VALUES (5, 'CREATE_USERS');
INSERT INTO Permissions (Id, Name) VALUES (6, 'DELETE_USERS');
INSERT INTO Permissions (Id, Name) VALUES (7, 'EDIT_USERS');
INSERT INTO Permissions (Id, Name) VALUES (8, 'VIEW_USERS');

INSERT INTO Permissions (Id, Name) VALUES (9, 'CREATE_CATEGORIES');
INSERT INTO Permissions (Id, Name) VALUES (10, 'DELETE_CATEGORIES');
INSERT INTO Permissions (Id, Name) VALUES (11, 'EDIT_CATEGORIES');
INSERT INTO Permissions (Id, Name) VALUES (12, 'VIEW_CATEGORIES');

INSERT INTO Permissions (Id, Name) VALUES (13, 'CREATE_PRODUCTS');
INSERT INTO Permissions (Id, Name) VALUES (14, 'DELETE_PRODUCTS');
INSERT INTO Permissions (Id, Name) VALUES (15, 'EDIT_PRODUCTS');
INSERT INTO Permissions (Id, Name) VALUES (16, 'VIEW_PRODUCTS');

INSERT INTO Permissions (Id, Name) VALUES (17, 'CREATE_WAREHOUSES');
INSERT INTO Permissions (Id, Name) VALUES (18, 'DELETE_WAREHOUSES');
INSERT INTO Permissions (Id, Name) VALUES (19, 'EDIT_WAREHOUSES');
INSERT INTO Permissions (Id, Name) VALUES (20, 'VIEW_WAREHOUSES');

INSERT INTO Permissions (Id, Name) VALUES (21, 'CREATE_ZONES');
INSERT INTO Permissions (Id, Name) VALUES (22, 'DELETE_ZONES');
INSERT INTO Permissions (Id, Name) VALUES (23, 'EDIT_ZONES');
INSERT INTO Permissions (Id, Name) VALUES (24, 'VIEW_ZONES');

INSERT INTO Permissions (Id, Name) VALUES (25, 'CREATE_CLIENTS');
INSERT INTO Permissions (Id, Name) VALUES (26, 'DELETE_CLIENTS');
INSERT INTO Permissions (Id, Name) VALUES (27, 'EDIT_CLIENTS');
INSERT INTO Permissions (Id, Name) VALUES (28, 'VIEW_CLIENTS');

INSERT INTO Permissions (Id, Name) VALUES (29, 'CREATE_SUPPLIERS');
INSERT INTO Permissions (Id, Name) VALUES (30, 'DELETE_SUPPLIERS');
INSERT INTO Permissions (Id, Name) VALUES (31, 'EDIT_SUPPLIERS');
INSERT INTO Permissions (Id, Name) VALUES (32, 'VIEW_SUPPLIERS');

INSERT INTO Permissions (Id, Name) VALUES (33, 'CREATE_PURCHASES');
INSERT INTO Permissions (Id, Name) VALUES (34, 'DELETE_PURCHASES');
INSERT INTO Permissions (Id, Name) VALUES (35, 'EDIT_PURCHASES');
INSERT INTO Permissions (Id, Name) VALUES (36, 'VIEW_PURCHASES');

INSERT INTO Permissions (Id, Name) VALUES (37, 'CREATE_SALES');
INSERT INTO Permissions (Id, Name) VALUES (38, 'DELETE_SALES');
INSERT INTO Permissions (Id, Name) VALUES (39, 'EDIT_SALES');
INSERT INTO Permissions (Id, Name) VALUES (40, 'VIEW_SALES');

INSERT INTO Permissions (Id, Name) VALUES (41, 'CREATE_ORDERS');
INSERT INTO Permissions (Id, Name) VALUES (42, 'DELETE_ORDERS');
INSERT INTO Permissions (Id, Name) VALUES (43, 'EDIT_ORDERS');
INSERT INTO Permissions (Id, Name) VALUES (44, 'VIEW_ORDERS');

INSERT INTO Permissions (Id, Name) VALUES (45, 'CREATE_DELIVERIES');
INSERT INTO Permissions (Id, Name) VALUES (46, 'DELETE_DELIVERIES');
INSERT INTO Permissions (Id, Name) VALUES (47, 'EDIT_DELIVERIES');
INSERT INTO Permissions (Id, Name) VALUES (48, 'VIEW_DELIVERIES');

INSERT INTO Permissions (Id, Name) VALUES (49, 'CREATE_STOCK_MOVEMENTS');
INSERT INTO Permissions (Id, Name) VALUES (50, 'DELETE_STOCK_MOVEMENTS');
INSERT INTO Permissions (Id, Name) VALUES (51, 'EDIT_STOCK_MOVEMENTS');
INSERT INTO Permissions (Id, Name) VALUES (52, 'VIEW_STOCK_MOVEMENTS');

INSERT INTO Permissions (Id, Name) VALUES (53, 'CREATE_VEHICLES');
INSERT INTO Permissions (Id, Name) VALUES (54, 'DELETE_VEHICLES');
INSERT INTO Permissions (Id, Name) VALUES (55, 'EDIT_VEHICLES');
INSERT INTO Permissions (Id, Name) VALUES (56, 'VIEW_VEHICLES');

INSERT INTO Permissions (Id, Name) VALUES (57, 'CREATE_PAYMENTS');
INSERT INTO Permissions (Id, Name) VALUES (58, 'DELETE_PAYMENTS');
INSERT INTO Permissions (Id, Name) VALUES (59, 'EDIT_PAYMENTS');
INSERT INTO Permissions (Id, Name) VALUES (60, 'VIEW_PAYMENTS');

INSERT INTO Permissions (Id, Name) VALUES (61, 'CREATE_COLLECTIONS');
INSERT INTO Permissions (Id, Name) VALUES (62, 'DELETE_COLLECTIONS');
INSERT INTO Permissions (Id, Name) VALUES (63, 'EDIT_COLLECTIONS');
INSERT INTO Permissions (Id, Name) VALUES (64, 'VIEW_COLLECTIONS');

-- Rol con todos los permisos
INSERT INTO Roles (Name, Description) VALUES ('Administrador', 'Acceso total al sistema');

-- Asignar todos los permisos al rol Administrador
INSERT INTO Roles_Permissions (RoleId, PermissionId)
SELECT 1, Id FROM Permissions;

-- Rol con permisos limitados
INSERT INTO Roles (Name, Description) VALUES ('Ventas', 'Acceso solo al módulo de ventas');

-- Asignar permisos específicos al rol Ventas
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (2, 37); -- CREATE_SALES
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (2, 40); -- VIEW_SALES
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (2, 41); -- CREATE_ORDERS
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (2, 44); -- VIEW_ORDERS

-- Rol para operaciones de depósito
INSERT INTO Roles ( Name, Description) VALUES ( 'Deposito', 'Acceso a gestión de stock y almacenes');

-- Asignar permisos relacionados al depósito
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (3, 17); -- CREATE_WAREHOUSES
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (3, 20); -- VIEW_WAREHOUSES
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (3, 49); -- CREATE_STOCK_MOVEMENTS
INSERT INTO Roles_Permissions (RoleId, PermissionId) VALUES (3, 52); -- VIEW_STOCK_MOVEMENTS

-- Usuario admin (rol 1: Administrador)
INSERT INTO Users ( Name, Username, Password, IsEnabled, RoleId)
VALUES ( 'Administrador', 'admin', 'admin', 'T', 1);

-- Usuario de ventas
INSERT INTO Users ( Name, Username, Password, IsEnabled, RoleId)
VALUES ( 'Empleado Ventas', 'ventas1', 'ventas123', 'T', 2);

-- Usuario de depósito
INSERT INTO Users ( Name, Username, Password, IsEnabled, RoleId)
VALUES ( 'Empleado Depósito', 'deposito1', 'deposito123', 'T', 3);

-- Usuario de prueba inhabilitado
INSERT INTO Users ( Name, Username, Password, IsEnabled, RoleId)
VALUES ( 'Usuario Inactivo', 'inactivo', 'inactivo123', 'F', 2);

-- Segundo usuario admin
INSERT INTO Users ( Name, Username, Password, IsEnabled, RoleId)
VALUES ( 'Supervisor', 'supervisor', 'super123', 'T', 1);

-- Insertar 2 categorías
INSERT INTO Categories (Name, Description) VALUES ('Lácteos', 'Productos derivados de la leche');
INSERT INTO Categories (Name, Description) VALUES ('Chacinados', 'Productos cárnicos procesados');

-- Insertar 4 subcategorías
INSERT INTO SubCategories (Name, Description, CategoryId) VALUES ('Leche', 'Leche entera, descremada y saborizada', 1);
INSERT INTO SubCategories (Name, Description, CategoryId) VALUES ('Quesos', 'Quesos blandos, duros y rallados', 1);
INSERT INTO SubCategories (Name, Description, CategoryId) VALUES ('Embutidos cocidos', 'Mortadelas, salchichas y fiambres cocidos', 2);
INSERT INTO SubCategories (Name, Description, CategoryId) VALUES ('Embutidos curados', 'Salames, longanizas y chorizos secos', 2);

-- Leche (Unidad)
INSERT INTO Products 
(Name, BarCode, UnitType, Price, Description, TemperatureCondition, Stock, Image, Observations, SubCategoryId)
VALUES 
-- Leche (SubCategoryId = 1)
('Leche entera 1L',        '7790001000010', 'Unit', 380, 'Leche entera pasteurizada',     'Cold', 200, '', '', 1),
('Leche descremada 1L',    '7790001000027', 'Unit', 390, 'Leche descremada pasteurizada', 'Cold', 180, '', '', 1),
('Leche chocolatada 1L',   '7790001000034', 'Unit', 420, 'Leche sabor chocolate',          'Cold', 150, '', '', 1),

-- Quesos (SubCategoryId = 2)
('Queso cremoso',          '7790002000018', 'Kilo', 1800, 'Queso fresco tipo cremoso',     'Cold', 80,  '', '', 2),
('Queso rallado',          '7790002000025', 'Unit', 950,  'Queso reggianito rallado 100g', 'Cold', 120, '', '', 2),
('Queso pategrás',         '7790002000032', 'Kilo', 2400, 'Queso semiduro tipo pategrás',  'Cold', 60,  '', '', 2),

-- Embutidos cocidos (SubCategoryId = 3)
('Mortadela',              '7790003000016', 'Kilo', 2300, 'Mortadela con pistacho',        'Cold', 55,  '', '', 3),
('Salchichas (6u)',        '7790003000023', 'Unit', 850,  'Salchichas tipo viena',         'Cold', 90,  '', '', 3),
('Fiambre de cerdo',       '7790003000030', 'Kilo', 2100, 'Fiambre cocido de cerdo',       'Cold', 70,  '', '', 3),

-- Embutidos curados (SubCategoryId = 4)
('Salame tandilero',       '7790004000014', 'Kilo', 3100, 'Salame curado artesanal',       'Ambient', 45, '', '', 4),
('Longaniza 250g',         '7790004000021', 'Unit', 1200, 'Longaniza tipo calabresa',      'Ambient', 65, '', '', 4),
('Chorizo seco',           '7790004000038', 'Kilo', 2900, 'Chorizo estacionado',           'Ambient', 50, '', '', 4),
('Fuet catalán',           '7790004000045', 'Unit', 1500, 'Fuet curado estilo catalán',    'Ambient', 30, '', '', 4);

