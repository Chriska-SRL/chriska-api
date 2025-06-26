CREATE TABLE [dbo].[Receipts]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Amount] MONEY NOT NULL, 
    [Notes] NVARCHAR(255) NULL, 
    [Date] DATETIME NOT NULL, 
    [ClientId] INT NOT NULL,
    [PaymentMethod] NCHAR(100) NULL, 
    CONSTRAINT [FK_Receipts_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]),
)
