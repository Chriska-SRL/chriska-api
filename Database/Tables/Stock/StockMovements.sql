CREATE TABLE [dbo].[StockMovements]
(
	  -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    [Quantity] DECIMAL (18, 2) NOT NULL, 
    [Type]  NVARCHAR(50) NOT NULL, 
    [Reason] NVARCHAR(255) NOT NULL, 
    [Date] DATETIME NOT NULL, 
    [ProductId] INT NOT NULL,

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
    CONSTRAINT [FK_StockMovements_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_StockMovements_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_StockMovements_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_StockMovements_Products] FOREIGN KEY ([ProductId]) REFERENCES [Products]([Id]),
)
