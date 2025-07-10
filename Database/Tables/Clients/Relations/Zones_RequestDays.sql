CREATE TABLE [dbo].[Zones_RequestDays]
(
    [ZoneId] INT NOT NULL,
    [Day] INT NOT NULL,
    CONSTRAINT [FK_Zones_RequestDays_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id]),
    CONSTRAINT [PK_Zones_RequestDays] PRIMARY KEY ([ZoneId], [Day]),
    CONSTRAINT [CHK_RequestDay_ValidValue] CHECK ([Day] BETWEEN 0 AND 6)
)