CREATE TABLE [dbo].[Zones]
(
    -- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [Name] NVARCHAR(50) NULL UNIQUE,
    [Description] NVARCHAR(100) NULL,
    [DeliveryDays] NVARCHAR(MAX) NULL, -- JSON: [1,3,5]
    [RequestDays] NVARCHAR(MAX) NULL,  -- JSON: [2,4,6]
    [ImageUrl] NVARCHAR(255) NOT NULL DEFAULT '',

    -- Auditoría
    [CreatedAt] DATETIME2 NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] INT NULL,
    [UpdatedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] INT NULL,
    [DeletedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"

    -- Soft delete
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    -- Foreign keys
    CONSTRAINT [FK_Zones_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Zones_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Zones_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
)
