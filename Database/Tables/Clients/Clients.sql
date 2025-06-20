CREATE TABLE [dbo].[Clients]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [RazonSocial] NVARCHAR(50) NOT NULL, 
    [RUT] NCHAR(20) NOT NULL, 
    [Address] NVARCHAR(255) NOT NULL, 
    [MapsAddress] NVARCHAR(255) NOT NULL, 
    [Phone] NCHAR(12) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [ContactName] NVARCHAR(50) NOT NULL, 
    [Schedule] NVARCHAR(100) NOT NULL, 
    [Bank] NVARCHAR(50) NOT NULL,
    [BankAccount] NVARCHAR(255) NOT NULL, 
    [LoanedCrates] INT NOT NULL DEFAULT 0, 
    [Observations] NVARCHAR(255) NULL, 
    [ZoneId] INT NOT NULL,
    CONSTRAINT [FK_Clients_Zones] FOREIGN KEY ([ZoneId]) REFERENCES [Zones]([Id])
)
