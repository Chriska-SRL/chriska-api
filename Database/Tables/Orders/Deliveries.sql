CREATE TABLE [dbo].[Deliveries]
(
	[Id] INT NOT NULL PRIMARY KEY, 
    [Date] DATE NOT NULL, 
    [Observations] NCHAR(10) NULL, 
    [VehicleId] INT NOT NULL,
    CONSTRAINT [FK_Deliveries_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),
)
