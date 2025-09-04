CREATE TABLE [dbo].[SupplierBalanceItems]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,
	[Date] DATETIME NOT NULL,
	[Amount] DECIMAL(18,2) NOT NULL,
	[Balance] DECIMAL(18,2) NOT NULL,
	[Description] NVARCHAR(255) NOT NULL,
	[DocumentType] NVARCHAR(50) NOT NULL,
	[DocumentId] INT NOT NULL,
	[SupplierId] INT NOT NULL,
)
