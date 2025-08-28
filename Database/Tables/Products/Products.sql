CREATE TABLE [dbo].[Products]
(
    -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    -- Campos de la entidad
    [Name] NVARCHAR(50) NOT NULL,
    [InternalCode] NCHAR(10) NOT NULL,
    [BarCode] NCHAR(13) NULL,
    [UnitType] NCHAR(10) NOT NULL,
    [Price] MONEY NOT NULL, 
    [Description] NVARCHAR(255) NOT NULL, 
    [TemperatureCondition] NVARCHAR(10) NOT NULL, 
    [EstimatedWeight] INT NULL DEFAULT 0, -- Peso estimado en gramos
    [Stock] DECIMAL NOT NULL, 
    [AvailableStock] DECIMAL NOT NULL, 
    [Observations] NVARCHAR(255) NOT NULL, 
    [SubCategoryId] INT NOT NULL, 
    [BrandId] INT NOT NULL, 
    [ImageUrl] NVARCHAR(255) NOT NULL DEFAULT '',
    [ShelveId] INT NOT NULL, 

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
    CONSTRAINT [FK_Products_SubCategories] FOREIGN KEY ([SubCategoryId]) REFERENCES [SubCategories]([Id]),
    CONSTRAINT [FK_Products_Shelves] FOREIGN KEY ([ShelveId]) REFERENCES [Shelves]([Id]),
    CONSTRAINT [FK_Products_Brands] FOREIGN KEY ([BrandId]) REFERENCES [Brands]([Id]),
    CONSTRAINT [FK_Products_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Products_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Products_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [CHK_Product_Price] CHECK ([Price] > 0),
    CONSTRAINT [CHK_Product_Stock] CHECK ([Stock] >= 0),
    CONSTRAINT [CHK_Product_BarcodeFormat] CHECK ([BarCode] IS NULL OR [BarCode] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)

GO

CREATE TRIGGER [dbo].[Trigger_Products]
    ON [dbo].[Products]
  AFTER INSERT
    AS
    BEGIN
        UPDATE p
        SET InternalCode = CAST(p.SubCategoryId AS NVARCHAR(10)) + RIGHT('0000' + CAST(p.Id AS NVARCHAR(4)), 4)
        FROM Products p
        INNER JOIN inserted i ON p.Id = i.Id
    END