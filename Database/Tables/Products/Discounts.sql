CREATE TABLE [dbo].[Discounts]
(
	 -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [Description] NVARCHAR(255) NOT NULL,
    [Percentage] DECIMAL(5, 2) NOT NULL CHECK (Percentage >= 0 AND Percentage <= 100),
    [ExpirationDate] DATETIME2 NOT NULL,
    [Status] NVARCHAR(20) NOT NULL,
    [ProductQuantity] INT NOT NULL CHECK (ProductQuantity >= 0),
    [BrandId] INT NULL, -- Puede ser NULL si el descuento no está asociado a una marca específica
    [SubCategoryId] INT NULL, -- Puede ser NULL si el descuento no está asociado a una subcategoría específica
    [ZoneId] INT NULL, -- Puede ser NULL si el descuento no está asociado a una zona específica

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

    -- Foreign keys
    CONSTRAINT FK_Discounts_Brand FOREIGN KEY (BrandId) REFERENCES Brands(Id),
    CONSTRAINT FK_Discounts_SubCategory FOREIGN KEY (SubCategoryId) REFERENCES SubCategories(Id),
    CONSTRAINT FK_Discounts_Zone FOREIGN KEY (ZoneId) REFERENCES Zones(Id),

    -- Foreign keys de auditoría
    CONSTRAINT FK_Discounts_CreatedBy FOREIGN KEY (CreatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Discounts_UpdatedBy FOREIGN KEY (UpdatedBy) REFERENCES Users(Id),
    CONSTRAINT FK_Discounts_DeletedBy FOREIGN KEY (DeletedBy) REFERENCES Users(Id)
)
