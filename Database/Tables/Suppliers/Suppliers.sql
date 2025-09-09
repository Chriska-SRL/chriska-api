CREATE TABLE [dbo].[Suppliers]
(
    -- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    -- Campos de la entidad
    [Name] NVARCHAR(50) NOT NULL, 
    [RazonSocial] NVARCHAR(50) NOT NULL, 
    [RUT] NCHAR(12) NOT NULL UNIQUE, 
    [ContactName] NVARCHAR(50) NOT NULL, 
    [Phone] NVARCHAR(15) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [Location] NVARCHAR(100)NOT NULL , 
    [Observations] NVARCHAR(255) NOT NULL,

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
    CONSTRAINT [FK_Suppliers_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Suppliers_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Suppliers_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id])
)
