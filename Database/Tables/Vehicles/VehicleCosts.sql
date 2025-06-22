CREATE TABLE [dbo].[VehicleCosts]
(
    [Id] INT NOT NULL IDENTITY PRIMARY KEY,
    [VehicleId] INT NOT NULL,
    [Type] INT NOT NULL,
    [Amount] MONEY NOT NULL,
    [Description] NVARCHAR(255) NOT NULL,
    [Date] DATE NOT NULL DEFAULT GETDATE(), 
    CONSTRAINT [FK_Costs_Vehicles] FOREIGN KEY ([VehicleId]) REFERENCES [Vehicles]([Id]),
    CONSTRAINT [CHK_Costs_Amount] CHECK ([Amount] > 0)
);
