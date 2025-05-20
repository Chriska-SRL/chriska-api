CREATE TABLE [dbo].[Payments]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Amount] MONEY NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [Notes] NVARCHAR(255) NULL, 
    [SupplierId] INT NULL, 
    CONSTRAINT [FK_Payments_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers]([Id]),
    CONSTRAINT [CHK_Payments_Suppliers_Amount] CHECK (Amount > 0)
)
