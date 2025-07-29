CREATE TABLE [dbo].[Roles]
(
    -- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [Name] NVARCHAR(50) NOT NULL UNIQUE,
    [Description] NVARCHAR(255) NOT NULL,

    -- Campos de auditoría
    [CreatedAt] DATETIME2 NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedLocation] NVARCHAR(50) NULL, -- Coordenadas GPS: "lat,long"
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] INT NULL,
    [UpdatedLocation] NVARCHAR(50) NULL,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] INT NULL,
    [DeletedLocation] NVARCHAR(50) NULL,

    -- Soft delete
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    -- Foreign keys de auditoría
    CONSTRAINT FK_Roles_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Roles_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Roles_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES Users(Id)
);
