CREATE TABLE [dbo].[Vehicles]
(
    -- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [Plate] CHAR(7) NOT NULL UNIQUE,
    [CrateCapacity] INT NOT NULL,
    [Brand] NVARCHAR(50) NOT NULL,
    [Model] NVARCHAR(50) NOT NULL,

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

    -- Restricciones
    CONSTRAINT [CHK_Vehicles_CrateCapacity] CHECK ([CrateCapacity] > 0),
    CONSTRAINT [CHK_Vehicles_PlateFormat] CHECK ([Plate] LIKE '[A-Z][A-Z][A-Z][0-9][0-9][0-9][0-9]'),

    -- Foreign keys
    CONSTRAINT [FK_Vehicles_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Vehicles_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Vehicles_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
)
