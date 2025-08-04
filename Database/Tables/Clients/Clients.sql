CREATE TABLE [dbo].[Clients]
(
    -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [Name] NVARCHAR(50) NOT NULL, 
    [RazonSocial] NVARCHAR(50) NOT NULL, 
    [RUT] NCHAR(12) NOT NULL, 
    [Address] NVARCHAR(255) NOT NULL, 
    [MapsAddress] NVARCHAR(255) NOT NULL, 
    [Phone] NVARCHAR(12) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [ContactName] NVARCHAR(50) NOT NULL, 
    [Schedule] NVARCHAR(100) NOT NULL, 
    [LoanedCrates] INT NOT NULL DEFAULT 0, 
    [Observations] NVARCHAR(255) NOT NULL, 
    [Qualification] NVARCHAR(5) NOT NULL DEFAULT '3/5',
    [ZoneId] INT NOT NULL,

    -- Campos de auditoría
    [CreatedAt] DATETIME2 NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] INT NULL,
    [UpdatedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] INT NULL,
    [DeletedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"

    -- Soft delete flag
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    -- Restricciones
    CONSTRAINT [FK_Clients_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id]),
    CONSTRAINT [FK_Clients_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Clients_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Clients_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
