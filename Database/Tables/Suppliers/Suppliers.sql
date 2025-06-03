CREATE TABLE [dbo].[Suppliers]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [RazonSocial] NVARCHAR(50) NOT NULL, 
    [RUT] NCHAR(12) NOT NULL UNIQUE, 
    [ContactName] NVARCHAR(50) NOT NULL, 
    [Phone] NCHAR(15) NOT NULL, 
    [Email] NVARCHAR(50) NOT NULL, 
    [Address] NVARCHAR(100) NOT NULL, 
    [mapsAddress] NVARCHAR(100) NOT NULL, 
    [Bank] NVARCHAR(50) NOT NULL, 
    [BankAccount] NVARCHAR(255) NOT NULL, 
    [Observations] NVARCHAR(255) NOT NULL,
    CONSTRAINT CHK_RUT_Format CHECK (RUT LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'),
    CONSTRAINT CHK_Email_Format CHECK (Email LIKE '_%@_%._%')
)
