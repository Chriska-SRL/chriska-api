CREATE TABLE [dbo].[Purchases]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Date] DATETIME NOT NULL, 
    [Status] NCHAR(15) NOT NULL, 
    [SupplierId] INT NOT NULL, 
    [InvoiceNumber] NVARCHAR(30) NULL UNIQUE, 
    CONSTRAINT [FK_Purchases_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers]([Id]),
     CONSTRAINT CHK_Purchases_Status CHECK ([Status] IN ('Sugerido', 'Solicitado', 'Pago'))
)