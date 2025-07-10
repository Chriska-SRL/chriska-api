CREATE TABLE [dbo].[Zones_DeliveryDays]
(
    [ZoneId] INT NOT NULL,
    [Day] INT NOT NULL,
    CONSTRAINT [FK_Zones_DeliveryDays_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id]),
    CONSTRAINT [PK_Zones_DeliveryDays] PRIMARY KEY ([ZoneId], [Day]),
    CONSTRAINT [CHK_DeliveryDay_ValidValue] CHECK ([Day] BETWEEN 0 AND 6)
)