CREATE TABLE [dbo].[ZoneImages]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [ZoneId] INT NOT NULL,
    [FileName] NVARCHAR(255) NOT NULL,
    [BlobName] NVARCHAR(255) NOT NULL,
    [ContentType] NVARCHAR(100) NOT NULL,
    [Size] BIGINT NOT NULL,
    [UploadDate] DATETIME2 NOT NULL,

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
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    -- Restricciones
    CONSTRAINT [FK_ZoneImages_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id]),
    CONSTRAINT [FK_ZoneImages_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_ZoneImages_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_ZoneImages_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [CHK_ZoneImages_Size] CHECK ([Size] >= 0)
)
