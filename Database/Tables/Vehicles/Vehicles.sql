CREATE TABLE [dbo].[Vehicles]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Plate] NCHAR(7) NOT NULL UNIQUE, 
    [CreateCapacity] INT NOT NULL, 
    [Brand] NVARCHAR(50) NULL, 
    [Model] NVARCHAR(50) NULL,
    CONSTRAINT [CHK_Vehicles_CreateCapacity] CHECK ([CreateCapacity] > 0),
    CONSTRAINT [CHK_Vehicles_PlateFormat] CHECK ([Plate] LIKE '[A-Z][A-Z][A-Z][0-9][0-9][0-9][0-9]')
)   
