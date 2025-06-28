CREATE TABLE [dbo].[ProductsStock]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [ProductId] INT NOT NULL,
    [ShelveId] INT NOT NULL,
    [Quantity] INT NOT NULL,
    
    CONSTRAINT [UQ_ProductsStock_Shelve_Product] UNIQUE ([ShelveId], [ProductId]),
    CONSTRAINT [FK_ProductsStock_Shelves] FOREIGN KEY ([ShelveId]) REFERENCES [Shelves]([Id]),
    CONSTRAINT [FK_ProductsStock_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [CHK_ProductsStock_Quantity] CHECK ([Quantity] >= 0)
);

