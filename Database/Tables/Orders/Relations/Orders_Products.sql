CREATE TABLE [dbo].[Orders_Products]
(
	[OrderId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    CONSTRAINT [PK_Orders_Products] PRIMARY KEY ([OrderId],[ProductId]), 
    CONSTRAINT [FK_Orders_Products_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]),
    CONSTRAINT [FK_Orders_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]) 
)
