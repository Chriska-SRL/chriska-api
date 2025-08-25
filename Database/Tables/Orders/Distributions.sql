CREATE TABLE [dbo].[Distributions]
(
	-- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    -- Campos de la entidad
    [Observations] NVARCHAR(255) NULL,
    [Date] DATETIME NOT NULL,
    [UserId] INT NOT NULL,
    [VehicleId] INT NULL,

    -- Restricciones
    CONSTRAINT [FK_Distributions_Users] FOREIGN KEY ([UserId]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Distributions_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),

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
    CONSTRAINT [FK_Distributions_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Distributions_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Distributions_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
