CREATE TABLE [dbo].[Products_Suppliers]
(
	ProductId INT NOT NULL,
	SupplierId INT NOT NULL,

	CONSTRAINT [PK_Products_Suppliers] PRIMARY KEY ([ProductId], [SupplierId]),
	CONSTRAINT [FK_Products_Suppliers_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
)
