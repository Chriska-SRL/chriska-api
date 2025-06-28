CREATE TABLE [dbo].[Orders]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATETIME NOT NULL, 
    [Creates] INT NOT NULL, 
    [Status] NCHAR(20) NOT NULL, 
    [DeliveryId] INT NULL, 
    [InvoiceId] INT NULL,
    [UserPrepareId] INT NULL, 
    [UserDeliverId] INT NULL, 
    CONSTRAINT [FK_Orders_Orders] FOREIGN KEY ([DeliveryId]) REFERENCES [Deliveries]([Id]),
    CONSTRAINT [FK_Orders_Invoice] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices]([Id]),
    CONSTRAINT [FK_Orders_Users_Prepare] FOREIGN KEY ([UserPrepareId]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Orders_Users_Deliver] FOREIGN KEY ([UserDeliverId]) REFERENCES [Users]([Id])
)
