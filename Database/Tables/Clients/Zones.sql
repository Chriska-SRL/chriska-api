CREATE TABLE [dbo].[Zones]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Datos del dominio
    [Name] NVARCHAR(50) NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    [DeliveryDays] NVARCHAR(60) NOT NULL,
    [RequestDays] NVARCHAR(60) NOT NULL,
    [Image] NVARCHAR(255) NOT NULL,

    -- Auditoría de creación
    [CreatedAt] DATETIME NOT NULL DEFAULT GETDATE(),
    [CreatedBy] INT NOT NULL,
    [CreatedLocation] NVARCHAR(50) NULL,

    -- Auditoría de actualización
    [UpdatedAt] DATETIME NULL,
    [UpdatedBy] INT NULL,
    [UpdatedLocation] NVARCHAR(50) NULL,

    -- Auditoría de borrado lógico (soft delete)
    [DeletedAt] DATETIME NULL,
    [DeletedBy] INT NULL,
    [DeletedLocation] NVARCHAR(50) NULL,

    -- Foreign Keys
    CONSTRAINT [FK_Zones_CreatedBy_Users] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Zones_UpdatedBy_Users] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Zones_DeletedBy_Users] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
