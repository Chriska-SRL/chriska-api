CREATE TABLE [dbo].[SupplierReceipts]
(
	-- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    -- Campos de la entidad
    [Date] DATETIME NOT NULL, 
    [Amount] DECIMAL(18,2) NULL,
    [Notes] NVARCHAR(255) NOT NULL,
    [PaymentMethod] NVARCHAR(50) NULL,
    [SupplierId] INT NOT NULL,

    --Restricciones de la entidad
    CONSTRAINT [FK_SupplierReceipts_SupplierId] FOREIGN KEY ([SupplierId]) REFERENCES [Suppliers]([Id]),

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

    --Restricciones Audit
    CONSTRAINT [FK_SupplierReceipts_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_SupplierReceipts_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_SupplierReceipts_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
