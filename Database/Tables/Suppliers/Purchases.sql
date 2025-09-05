CREATE TABLE [dbo].[Purchases]
(
	-- Clave primaria
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,

	-- Campos de la entidad
	[Date] DATETIME NOT NULL,
	[Observations] NVARCHAR(255) NULL,
	[InvoiceNumber] NVARCHAR(30) NULL UNIQUE,
	[SupplierId] INT NOT NULL,
	[Status] NVARCHAR(20) NOT NULL, 

	-- Restricciones
	CONSTRAINT [FK_Purchases_Suppliers] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers]([Id]),

	-- Auditoría
	[CreatedAt] DATETIME2 NOT NULL,
	[CreatedBy] INT NOT NULL,
	[CreatedLocation] NVARCHAR(50) NULL,
	[UpdatedAt] DATETIME2 NULL,
	[UpdatedBy] INT NULL,
	[UpdatedLocation] NVARCHAR(50) NULL,
	[DeletedAt] DATETIME2 NULL,
	[DeletedBy] INT NULL,
	[DeletedLocation] NVARCHAR(50) NULL,

	-- Soft delete
	[IsDeleted] BIT NOT NULL DEFAULT 0,

	-- Restricciones Audit
	CONSTRAINT [FK_Purchases_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_Purchases_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
	CONSTRAINT [FK_Purchases_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)