CREATE TABLE [dbo].[Purchases_Products]
(
	[PurchaseId] INT NOT NULL,
    [ProductId] INT NOT NULL, 
	[Quantity] INT NOT NULL, 
    [UnitPrice] MONEY NOT NULL, 
    CONSTRAINT [PK_Purchases_Products] PRIMARY KEY ([PurchaseId],[ProductId]), 
    CONSTRAINT [FK_Purchases_Products_Purchases] FOREIGN KEY ([PurchaseId]) REFERENCES [Purchases]([Id]),
    CONSTRAINT [FK_Purchases_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [CHK_Purchases_Products_UnitPrice] CHECK (UnitPrice > 0),
    CONSTRAINT [CHK_Purchases_Products_Quantity] CHECK (Quantity > 0)
)
