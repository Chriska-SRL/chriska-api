CREATE TABLE [dbo].[Deliveries]
(
	-- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY, 

    -- Campos de la entidad
    [Date] DATETIME NOT NULL, 
    [Observations] NVARCHAR(255) NULL,

    [Status] NVARCHAR(20) NOT NULL, 
    [ConfirmedDate] DATETIME NULL,
    [Crates] INT  NOT NULL DEFAULT 0,

    [ClientId] INT NOT NULL,
    [OrderId] INT NOT NULL,

    -- Restricciones
    CONSTRAINT [FK_Deliveries_Orders] FOREIGN KEY ([OrderId]) REFERENCES [Orders]([Id]),
    CONSTRAINT [FK_Deliveries_Clients] FOREIGN KEY ([ClientId]) REFERENCES [Clients]([Id]),

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
    CONSTRAINT [FK_Deliveries_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Deliveries_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Deliveries_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
