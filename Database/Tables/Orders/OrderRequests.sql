CREATE TABLE [dbo].[OrderRequests]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIME NOT NULL, 
    [Observations] NVARCHAR(255) NOT NULL, 
    [Status] NVARCHAR(50) NOT NULL,
    [ClientId] INT NOT NULL, 
    [OrderId] INT NULL,
    [UserId] INT NOT NULL, 
    CONSTRAINT [FK_OrderRequests_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]),
    CONSTRAINT [FK_OrderRequests_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]),
    CONSTRAINT [FK_OrderRequests_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id])
)
