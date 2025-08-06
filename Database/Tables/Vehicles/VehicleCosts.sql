CREATE TABLE [dbo].[VehicleCosts]
(
    -- Clave primaria
    [Id] INT NOT NULL IDENTITY PRIMARY KEY,

    -- Campos de la entidad
    [VehicleId] INT NOT NULL,
    [Type] NVARCHAR(50) NOT NULL, 
    [Amount] MONEY NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    [Date] DATE NOT NULL DEFAULT GETDATE(),

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
    CONSTRAINT [FK_Costs_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),
    CONSTRAINT [FK_Costs_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Costs_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Costs_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [CHK_Costs_Amount] CHECK ([Amount] > 0)
)
