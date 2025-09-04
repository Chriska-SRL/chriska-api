CREATE TABLE [dbo].[Purchases_Products]
(
	[PurchaseId] INT NOT NULL,
    [ProductId] INT NOT NULL, 
	[Quantity] DECIMAL(18,2) NOT NULL, 
    [UnitPrice] DECIMAL(18,2) NOT NULL, 
    [Discount] DECIMAL(5,2) NOT NULL DEFAULT 0,
    [Weight] INT NULL,

    CONSTRAINT [PK_Purchases_Products] PRIMARY KEY ([PurchaseId], [ProductId]),
    CONSTRAINT [FK_Purchases_Products_Purchases] FOREIGN KEY ([PurchaseId]) REFERENCES [Purchases]([Id]),
    CONSTRAINT [FK_Purchases_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id])
)
