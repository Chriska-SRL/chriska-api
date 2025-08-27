CREATE TABLE [dbo].[ReturnRequests_Products] (
    [Quantity]       DECIMAL             NOT NULL,
    [UnitPrice]      DECIMAL (18, 2) NOT NULL,
    [Discount]       DECIMAL (5, 2)  NOT NULL,
    [Weight]         INT             NULL,
    [ReturnRequestId] INT             NOT NULL,
    [ProductId]      INT             NOT NULL,
    CONSTRAINT [PK_ReturnRequests_Products] PRIMARY KEY CLUSTERED ([ReturnRequestId] ASC, [ProductId] ASC),
    CONSTRAINT [FK_ReturnRequests_Products_ReturnRequests] FOREIGN KEY ([ReturnRequestId]) REFERENCES [dbo].[ReturnRequests] ([Id]),
    CONSTRAINT [FK_ReturnRequests_Products_Products] FOREIGN KEY ([ProductId]) REFERENCES [dbo].[Products] ([Id])
);

