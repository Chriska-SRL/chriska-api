CREATE TABLE [dbo].[VehicleCosts]
(
    [VehicleId] INT NOT NULL, 
    [Id] INT NOT NULL, 
    [Type] INT NOT NULL,
    [Amount] MONEY NOT NULL, 
    [Description] NVARCHAR(255) NOT NULL, 
    CONSTRAINT [PK_Costs] PRIMARY KEY ([VehicleId], [Id]),
    CONSTRAINT [FK_Costs_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),
    CONSTRAINT [CHK_Costs_Amount] CHECK ([Amount] > 0)
);
