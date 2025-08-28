CREATE TABLE [dbo].[OrderRequests_Products]
(
	[Quantity] DECIMAL NOT NULL,
    [UnitPrice] DECIMAL(18, 2) NOT NULL,
    [Discount] DECIMAL(5, 2) NOT NULL,
    [Weight] INT NULL,

	[OrderRequestId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    CONSTRAINT [PK_OrderRequests_Products] PRIMARY KEY ([OrderRequestId],[ProductId]), 
    CONSTRAINT [FK_OrderRequests_Products_OrderRequests] FOREIGN KEY ([OrderRequestId]) REFERENCES [OrderRequests]([Id]),
    CONSTRAINT [FK_OrderRequests_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]) 
)
