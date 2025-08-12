CREATE TABLE [dbo].[DiscountClients] (
    [DiscountId] INT NOT NULL,
    [ClientId]   INT NOT NULL,
    CONSTRAINT [PK_DiscountClients] PRIMARY KEY CLUSTERED ([DiscountId], [ClientId]),
    CONSTRAINT [FK_DiscountClients_Discount] FOREIGN KEY ([DiscountId]) REFERENCES [dbo].[Discounts]([Id]),
    CONSTRAINT [FK_DiscountClients_Client]   FOREIGN KEY ([ClientId])   REFERENCES [dbo].[Clients]([Id])
);