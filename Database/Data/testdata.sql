INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (1, 'CREATE_ROLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (2, 'DELETE_ROLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (3, 'EDIT_ROLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (4, 'VIEW_ROLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (5, 'CREATE_USERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (6, 'DELETE_USERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (7, 'EDIT_USERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (8, 'VIEW_USERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (9, 'CREATE_CATEGORIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (10, 'DELETE_CATEGORIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (11, 'EDIT_CATEGORIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (12, 'VIEW_CATEGORIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (13, 'CREATE_PRODUCTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (14, 'DELETE_PRODUCTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (15, 'EDIT_PRODUCTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (16, 'VIEW_PRODUCTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (17, 'CREATE_WAREHOUSES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (18, 'DELETE_WAREHOUSES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (19, 'EDIT_WAREHOUSES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (20, 'VIEW_WAREHOUSES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (21, 'CREATE_ZONES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (22, 'DELETE_ZONES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (23, 'EDIT_ZONES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (24, 'VIEW_ZONES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (25, 'CREATE_CLIENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (26, 'DELETE_CLIENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (27, 'EDIT_CLIENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (28, 'VIEW_CLIENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (29, 'CREATE_SUPPLIERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (30, 'DELETE_SUPPLIERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (31, 'EDIT_SUPPLIERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (32, 'VIEW_SUPPLIERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (33, 'CREATE_PURCHASES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (34, 'DELETE_PURCHASES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (35, 'EDIT_PURCHASES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (36, 'VIEW_PURCHASES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (37, 'CREATE_SALES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (38, 'DELETE_SALES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (39, 'EDIT_SALES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (40, 'VIEW_SALES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (41, 'CREATE_ORDERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (42, 'DELETE_ORDERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (43, 'EDIT_ORDERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (44, 'VIEW_ORDERS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (45, 'CREATE_DELIVERIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (46, 'DELETE_DELIVERIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (47, 'EDIT_DELIVERIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (48, 'VIEW_DELIVERIES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (49, 'CREATE_STOCK_MOVEMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (50, 'DELETE_STOCK_MOVEMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (51, 'EDIT_STOCK_MOVEMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (52, 'VIEW_STOCK_MOVEMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (53, 'CREATE_VEHICLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (54, 'DELETE_VEHICLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (55, 'EDIT_VEHICLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (56, 'VIEW_VEHICLES');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (57, 'CREATE_PAYMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (58, 'DELETE_PAYMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (59, 'EDIT_PAYMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (60, 'VIEW_PAYMENTS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (61, 'CREATE_COLLECTIONS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (62, 'DELETE_COLLECTIONS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (63, 'EDIT_COLLECTIONS');
INSERT INTO [dbo].[Permissions] (Id, Name) VALUES (64, 'VIEW_COLLECTIONS');

INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Administrador General', 'Acceso completo a la configuración y gestión del sistema.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Encargado de Ventas', 'Gestiona clientes, ventas, pedidos y cobranzas.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Encargado de Compras', 'Realiza órdenes de compra y gestiona proveedores.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Encargado de Depósito', 'Controla stock, movimientos y entregas.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Repartidor', 'Visualiza entregas asignadas y reporta estado.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Administrativo', 'Apoyo general en tareas administrativas.');
INSERT INTO [dbo].[Roles] (Name, Description) VALUES ('Contador', 'Accede a reportes contables y controla pagos y cobranzas.');


-- Administrador General (Id = 1) - Todos los permisos
INSERT INTO [dbo].[Roles_Permissions]
SELECT 1, Id FROM [dbo].[Permissions];

-- Encargado de Ventas (Id = 2)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(2, 37), (2, 38), (2, 39), (2, 40), -- Sales
(2, 41), (2, 42), (2, 43), (2, 44), -- Orders
(2, 61), (2, 62), (2, 63), (2, 64), -- Collections
(2, 25), (2, 26), (2, 27), (2, 28); -- Clients

-- Encargado de Compras (Id = 3)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(3, 33), (3, 34), (3, 35), (3, 36), -- Purchases
(3, 29), (3, 30), (3, 31), (3, 32); -- Suppliers

-- Encargado de Depósito (Id = 4)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(4, 45), (4, 46), (4, 47), (4, 48), -- Deliveries
(4, 49), (4, 50), (4, 51), (4, 52), -- Stock Movements
(4, 13), (4, 14), (4, 15), (4, 16); -- Products

-- Repartidor (Id = 5)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(5, 48), -- View Deliveries
(5, 44); -- View Orders

-- Administrativo (Id = 6)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(6, 8), (6, 12), (6, 16), (6, 20), (6, 24), (6, 28), (6, 32), (6, 36), (6, 40), (6, 44), (6, 48), (6, 52), (6, 56), (6, 60), (6, 64); -- Solo vistas

-- Contador (Id = 7)
INSERT INTO [dbo].[Roles_Permissions] (RoleId, PermissionId) VALUES
(7, 57), (7, 58), (7, 59), (7, 60), -- Payments
(7, 61), (7, 62), (7, 63), (7, 64); -- Collections

INSERT INTO [dbo].[Users] (Name, Username, Password, RoleId) VALUES
('Juan Pérez', 'admin', 'admin123', 1),         -- Administrador General
('María López', 'mlopez', 'ventas123', 2),       -- Encargado de Ventas
('Carlos Gómez', 'cgomez', 'compras123', 3),     -- Encargado de Compras
('Laura Fernández', 'lfernandez', 'deposito123', 4), -- Encargado de Depósito
('Pedro Ruiz', 'pruiz', 'reparto123', 5),        -- Repartidor
('Sofía Torres', 'storres', 'admin456', 6),      -- Administrativo
('Diego Martín', 'dmartin', 'contador123', 7);   -- Contador

INSERT INTO [dbo].[Vehicles] (Plate, CrateCapacity, Brand, Model) VALUES
('ABC1234', 100, 'Mercedes-Benz', 'Sprinter 415'),
('DEF5678', 80, 'Iveco', 'Daily 35S14'),
('GHI9012', 120, 'Renault', 'Master L3H2'),
('JKL3456', 90, 'Ford', 'Transit'),
('MNO7890', 110, 'Peugeot', 'Boxer');

-- Supone que los vehículos insertados anteriormente tienen Id del 1 al 5

INSERT INTO [dbo].[VehicleCosts] (VehicleId, Type, Amount, Description, Date) VALUES
(1, 1, 15000.00, 'Cambio de neumáticos', '2025-05-10'),
(1, 2, 4500.00, 'Cambio de aceite y filtros', '2025-06-01'),
(2, 1, 8000.00, 'Reparación de frenos', '2025-04-22'),
(3, 3, 3000.00, 'Lavado integral y mantenimiento', '2025-06-15'),
(4, 1, 12000.00, 'Servicio completo 10.000 km', '2025-03-30'),
(5, 2, 5000.00, 'Alineación y balanceo', '2025-06-10');

INSERT INTO [dbo].[Categories] (Name, Description) VALUES
('Chacinados', 'Productos embutidos como salames, jamones y fiambres.'),
('Lácteos', 'Productos derivados de la leche como quesos y yogures.'),
('Congelados', 'Productos conservados a baja temperatura.'),
('Frescos', 'Productos de corta vida útil que requieren refrigeración.'),
('Otros', 'Productos complementarios fuera de las categorías principales.');

-- Subcategorías de Chacinados (CategoryId = 1)
INSERT INTO [dbo].[SubCategories] (Name, Description, CategoryId) VALUES
('Salames', 'Variedades de salames curados.', 1),
('Jamones', 'Jamones cocidos y crudos.', 1),
('Mortadelas', 'Mortadelas tradicionales y saborizadas.', 1);

-- Subcategorías de Lácteos (CategoryId = 2)
INSERT INTO [dbo].[SubCategories] (Name, Description, CategoryId) VALUES
('Quesos blandos', 'Quesos tipo cremoso, port salut, etc.', 2),
('Quesos duros', 'Quesos tipo reggianito, sardo, etc.', 2),
('Yogures', 'Yogures bebibles y firmes.', 2);

-- Subcategorías de Congelados (CategoryId = 3)
INSERT INTO [dbo].[SubCategories] (Name, Description, CategoryId) VALUES
('Empanadas', 'Empanadas congeladas listas para hornear.', 3),
('Hamburguesas', 'Hamburguesas de carne congeladas.', 3);

-- Subcategorías de Frescos (CategoryId = 4)
INSERT INTO [dbo].[SubCategories] (Name, Description, CategoryId) VALUES
('Leche', 'Leche entera, descremada y deslactosada.', 4),
('Crema', 'Crema de leche y derivados.', 4);

-- Subcategorías de Otros (CategoryId = 5)
INSERT INTO [dbo].[SubCategories] (Name, Description, CategoryId) VALUES
('Panificados', 'Pan, prepizzas y productos de panadería.', 5),
('Bebidas', 'Jugos y bebidas complementarias.', 5);


-- Productos de Chacinados
INSERT INTO [dbo].[Products] (Name, BarCode, UnitType, Price, Description, TemperatureCondition, Stock, Image, Observations, SubCategoryId) VALUES
('Salame Milan', '1234567890123', 'Kilo', 3200.00, 'Salame tipo milan madurado', 'Cold', 50, 'salame_milan.jpg', 'Curado natural', 1),
('Jamón Cocido Premium', '1234567890124', 'Kilo', 4500.00, 'Jamón cocido sin cuero', 'Cold', 30, 'jamon_cocido.jpg', 'Sin fécula agregada', 2),

-- Productos de Lácteos
('Queso Cremoso', '1234567890125', 'Kilo', 2800.00, 'Queso fresco tipo cremoso', 'Cold', 40, 'queso_cremoso.jpg', 'Ideal para pizzas', 4),
('Queso Sardo', '1234567890126', 'Kilo', 3900.00, 'Queso duro estacionado', 'Cold', 20, 'queso_sardo.jpg', 'Rallado fino', 5),
('Yogur Bebible Frutilla', '1234567890127', 'Unit', 250.00, 'Yogur bebible sabor frutilla 1L', 'Cold', 100, 'yogur_frutilla.jpg', 'Con pulpa natural', 6),

-- Congelados
('Empanadas de Carne', '1234567890128', 'Unit', 1500.00, 'Caja de 12 empanadas congeladas', 'Frozen', 25, 'empanadas_carne.jpg', 'Masa casera', 7),
('Hamburguesas Vacuno', '1234567890129', 'Unit', 2200.00, 'Caja con 8 hamburguesas de carne vacuna', 'Frozen', 15, 'hamburguesas.jpg', '80% carne', 8),

-- Frescos
('Leche Entera 1L', '1234567890130', 'Unit', 350.00, 'Leche entera larga vida', 'Ambient', 200, 'leche_entera.jpg', 'Sin conservantes', 9),
('Crema de Leche 200ml', '1234567890131', 'Unit', 600.00, 'Crema para cocinar', 'Cold', 80, 'crema_leche.jpg', 'Alta densidad', 10),

-- Otros
('Prepizza con Salsa', '1234567890132', 'Unit', 700.00, 'Base de pizza precocida con salsa', 'Ambient', 60, 'prepizza.jpg', 'Lista para hornear', 11),
('Jugo Natural Naranja 1L', '1234567890133', 'Unit', 500.00, 'Jugo exprimido sin azúcar', 'Cold', 90, 'jugo_naranja.jpg', '100% exprimido', 12);

INSERT INTO Warehouses (Name, Description, Address) VALUES
('Depósito Central', 'Depósito principal de refrigerados y frescos', 'Av. Industria 1234, Córdoba'),
('Depósito Secundario', 'Depósito para productos congelados y secos', 'Ruta 9 Km 45, Córdoba');

INSERT INTO Shelves (Name, Description, WarehouseId) VALUES
('Estantería Lácteos', 'Refrigeración media para lácteos', 1),
('Estantería Chacinados', 'Refrigeración baja para fiambres', 1),
('Estantería Congelados', 'Zona de frío - congelados', 2),
('Estantería Secos', 'Para productos de ambiente seco', 2);

INSERT INTO ProductsStock (ProductId, ShelveId, Quantity) VALUES
(1, 2, 50),    -- Salame Milan en Estantería Chacinados
(2, 2, 30),    -- Jamón Cocido Premium en Estantería Chacinados
(3, 1, 40),    -- Queso Cremoso en Estantería Lácteos
(4, 1, 20),    -- Queso Sardo en Estantería Lácteos
(5, 1, 100),   -- Yogur Frutilla en Estantería Lácteos
(6, 3, 25),    -- Empanadas de carne en Estantería Congelados
(7, 3, 15),    -- Hamburguesas Vacuno en Estantería Congelados
(8, 4, 200),   -- Leche Entera en Estantería Secos
(9, 1, 80),   -- Crema de Leche en Estantería Lácteos
(10, 4, 60),   -- Prepizza con Salsa en Estantería Secos
(11, 1, 90);   -- Jugo Natural Naranja en Estantería Lácteos


INSERT INTO StockMovements (Quantity, Type, Reason, Date, ProductId, ShelveId, UserId) VALUES
(50, 'I', 'Ingreso inicial de Salame Milan', GETDATE(), 1, 2, 1),
(30, 'I', 'Ingreso inicial de Jamón Cocido Premium', GETDATE(), 2, 2, 1),
(40, 'I', 'Ingreso inicial de Queso Cremoso', GETDATE(), 3, 1, 1),
(20, 'I', 'Ingreso inicial de Queso Sardo', GETDATE(), 4, 1, 1),
(100, 'I', 'Ingreso inicial de Yogur Frutilla', GETDATE(), 5, 1, 1),
(25, 'I', 'Ingreso inicial de Empanadas de carne', GETDATE(), 6, 3, 1),
(15, 'I', 'Ingreso inicial de Hamburguesas Vacuno', GETDATE(), 7, 3, 1),
(200, 'I', 'Ingreso inicial de Leche Entera', GETDATE(), 8, 4, 1),
(80, 'I', 'Ingreso inicial de Crema de Leche', GETDATE(), 9, 1, 1),
(60, 'I', 'Ingreso inicial de Prepizza con Salsa', GETDATE(), 10, 4, 1),
(90, 'I', 'Ingreso inicial de Jugo Natural Naranja', GETDATE(), 11, 1, 1);

