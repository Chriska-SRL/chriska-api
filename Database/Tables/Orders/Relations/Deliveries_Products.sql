CREATE TABLE [dbo].[Deliveries_Products]
(
    [Quantity] INT NOT NULL,
    [UnitPrice] DECIMAL(18, 2) NOT NULL,
    [Discount] DECIMAL(5, 2) NOT NULL,
    [Weight] INT NULL,

	[DeliveryId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    CONSTRAINT [PK_Deliveries_Products] PRIMARY KEY ([DeliveryId],[ProductId]), 
    CONSTRAINT [FK_Deliveries_Products_Deliveries] FOREIGN KEY ([DeliveryId]) REFERENCES [Deliveries]([Id]),
    CONSTRAINT [FK_Deliveries_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]) 
)
