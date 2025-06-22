-- Deshabilitar restricciones de claves foráneas
EXEC sp_msforeachtable 'ALTER TABLE ? NOCHECK CONSTRAINT ALL';

-- Borrar datos respetando dependencias (orden manual)
DELETE FROM [dbo].[Roles_Permissions];
DELETE FROM [dbo].[Users];
DELETE FROM [dbo].[VehicleCosts];
DELETE FROM [dbo].[ProductsStock];
DELETE FROM [dbo].[Invoices_Products];
DELETE FROM [dbo].[Orders_Products];
DELETE FROM [dbo].[Purchases_Products];
DELETE FROM [dbo].[Deliveries];
DELETE FROM [dbo].[ReturnRequests];
DELETE FROM [dbo].[Receipts];
DELETE FROM [dbo].[CreditNotes];
DELETE FROM [dbo].[Invoices];
DELETE FROM [dbo].[Payments];
DELETE FROM [dbo].[OrderRequests];
DELETE FROM [dbo].[Orders];
DELETE FROM [dbo].[Purchases];
DELETE FROM [dbo].[StockMovements];
DELETE FROM [dbo].[Shelves];
DELETE FROM [dbo].[Clients];
DELETE FROM [dbo].[Suppliers];
DELETE FROM [dbo].[Products];
DELETE FROM [dbo].[SubCategories];
DELETE FROM [dbo].[Categories];
DELETE FROM [dbo].[Vehicles];
DELETE FROM [dbo].[Warehouses];
DELETE FROM [dbo].[Zones];
DELETE FROM [dbo].[Roles];
DELETE FROM [dbo].[Permissions];

-- Resetear IDENTITY
EXEC sp_msforeachtable '
IF OBJECTPROPERTY(OBJECT_ID(''?''), ''TableHasIdentity'') = 1
    DBCC CHECKIDENT (''?'', RESEED, 0);
';

-- Rehabilitar restricciones
EXEC sp_msforeachtable 'ALTER TABLE ? WITH CHECK CHECK CONSTRAINT ALL';
