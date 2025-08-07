CREATE TABLE [dbo].[DiscountProducts] (
    [DiscountId] INT NOT NULL,
    [ProductId]  INT NOT NULL,
    CONSTRAINT [PK_DiscountProducts] PRIMARY KEY CLUSTERED ([DiscountId], [ProductId]),
    CONSTRAINT [FK_DiscountProducts_Discount] FOREIGN KEY ([DiscountId]) REFERENCES [dbo].[Discounts]([Id]),
    CONSTRAINT [FK_DiscountProducts_Product]  FOREIGN KEY ([ProductId])  REFERENCES [dbo].[Products]([Id])
);