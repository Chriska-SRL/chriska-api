CREATE TABLE [dbo].[ProductsStock]
( 
    [ProductId] INT NOT NULL, 
    [ShelveId] INT NOT NULL,
    [Quantity] INT NOT NULL, 
    CONSTRAINT [PK_Shelves_Products] PRIMARY KEY ([ShelveId],[ProductId]), 
    CONSTRAINT [FK_Shelves_Products_Shelves] FOREIGN KEY ([ShelveId]) REFERENCES [Shelves]([Id]),
    CONSTRAINT [FK_Shelves_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [CHK_ProductsStock_Quantity] CHECK ([Quantity] > 0)
)
