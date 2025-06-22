CREATE TABLE [dbo].[Vehicles]
(
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,
    [Plate] CHAR(7) NOT NULL UNIQUE,
    [CrateCapacity] INT NOT NULL,
    [Brand] NVARCHAR(50) NOT NULL,
    [Model] NVARCHAR(50) NOT NULL,
    CONSTRAINT [CHK_Vehicles_CrateCapacity] CHECK ([CrateCapacity] > 0),
    CONSTRAINT [CHK_Vehicles_PlateFormat] CHECK ([Plate] LIKE '[A-Z][A-Z][A-Z][0-9][0-9][0-9][0-9]')
);

