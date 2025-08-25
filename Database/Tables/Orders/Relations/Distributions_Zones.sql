CREATE TABLE [dbo].[Distributions_Zones]
(
	[DistributionId] INT NOT NULL , 
    [ZoneId] INT NOT NULL, 
    CONSTRAINT [PK_Distributions_Zones] PRIMARY KEY ([DistributionId],[ZoneId]), 
    CONSTRAINT [FK_Distributions_Zones_Distributions] FOREIGN KEY ([DistributionId]) REFERENCES [Distributions]([Id]),
    CONSTRAINT [FK_Distributions_Zones_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id]) 
)
