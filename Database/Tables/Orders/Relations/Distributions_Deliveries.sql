CREATE TABLE [dbo].[Distributions_Deliveries]
(
    [Position] INT NOT NULL,

	[DistributionId] INT NOT NULL , 
    [DeliveryId] INT NOT NULL, 
    CONSTRAINT [PK_Distributions_Deliveries] PRIMARY KEY ([DistributionId],[DeliveryId]), 
    CONSTRAINT [FK_Distributions_Deliveries_Distributions] FOREIGN KEY ([DistributionId]) REFERENCES [Distributions]([Id]),
    CONSTRAINT [FK_Distributions_Deliveries_Deliveries] FOREIGN KEY ([DeliveryId]) REFERENCES [Deliveries]([Id]) 
)
