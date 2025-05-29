CREATE TABLE [dbo].[Costs]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Amount] MONEY NOT NULL, 
    [Description] NVARCHAR(255) NULL, 
    [Type] NCHAR(20) NOT NULL,
    [VehicleId] INT NOT NULL, 
    CONSTRAINT [FK_Costs_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),
    CONSTRAINT [CHK_Costs_Amount] CHECK ([Amount] > 0),
    CONSTRAINT [CHK_Costs_Type] CHECK ([Type] IN (
        'Combustible',
        'Patente',
        'Reparación',
        'Mantenimiento',
        'Seguro',
        'Neumáticos',
        'Lavado',
        'Otro'
    ))
)
