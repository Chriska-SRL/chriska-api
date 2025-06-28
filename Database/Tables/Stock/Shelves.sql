CREATE TABLE [dbo].[Shelves]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL UNIQUE, 
    [Description] NVARCHAR(255) NOT NULL, 
    [WarehouseId] INT NOT NULL, 
    CONSTRAINT [FK_Shelve_Warehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id])
)
