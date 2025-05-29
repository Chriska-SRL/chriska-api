CREATE TABLE [dbo].[Invoices_Products]
(
    [InvoiceId] INT NOT NULL , 
    [ProductId] INT NOT NULL, 
    CONSTRAINT [PK_Invoices_Products] PRIMARY KEY ([InvoiceId],[ProductId]), 
    CONSTRAINT [FK_Roles_Permissions_Invoices] FOREIGN KEY ([InvoiceId]) REFERENCES [Invoices]([Id]),
    CONSTRAINT [FK_Roles_Permissions_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]) 
)
