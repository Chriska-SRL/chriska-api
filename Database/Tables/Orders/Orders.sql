CREATE TABLE [dbo].[Orders]
(
	-- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY, 

    -- Campos de la entidad
    [Date] DATETIME NOT NULL, 
    [Observations] NVARCHAR(255) NULL,

    [Status] NVARCHAR(20) NOT NULL, 
    [ConfirmedDate] DATETIME NULL,

    [Creates] INT NOT NULL, 

    [DeliveryId] INT NULL,
    [ClientId] INT NOT NULL,
    [OrderRequestId] INT NOT NULL,

    -- Restricciones
    CONSTRAINT [FK_Orders_Deliveries] FOREIGN KEY ([DeliveryId]) REFERENCES [Deliveries]([Id]),
    CONSTRAINT [FK_Orders_OrderRequest] FOREIGN KEY ([OrderRequestId]) REFERENCES [OrderRequests]([Id]),
    CONSTRAINT [FK_Orders_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]),

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
    CONSTRAINT [FK_Orders_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Orders_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Orders_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)

