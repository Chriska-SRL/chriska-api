CREATE TABLE [dbo].[StockMovements]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Quantity] INT NOT NULL, 
    [Type] NCHAR(1) NOT NULL, 
    [Reason] NVARCHAR(255) NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [ProductId] INT NOT NULL,
    [ShelveId] INT NOT NULL,
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_StockMovements_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
    CONSTRAINT [FK_StockMovements_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT [CHK_StockMovements_Type] CHECK ([Type] IN ('I', 'E')),
    CONSTRAINT [CHK_StockMovements_Quantity] CHECK ([Quantity] > 0),
    CONSTRAINT [FK_StockMovements_Shelves] FOREIGN KEY ([ShelveId]) REFERENCES [Shelves]([Id])
)
