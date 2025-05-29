CREATE TABLE [dbo].[Shelves]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(20) NOT NULL UNIQUE, 
    [Description] NCHAR(50) NOT NULL, 
    [WarehouseId] INT NOT NULL, 
    CONSTRAINT [FK_Shelve_Warehouse] FOREIGN KEY ([WarehouseId]) REFERENCES [Warehouses]([Id])
)
