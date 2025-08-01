CREATE TABLE [dbo].[ClientBankAccounts]
(
	 -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [BankName] NVARCHAR(50) NOT NULL, 
    [AccountName] NVARCHAR(50) NOT NULL, 
    [AccountNumber] NVARCHAR(50) NOT NULL, 
    [ClientId] INT NOT NULL,

    CONSTRAINT FK_ClientBankAccounts_ClientId FOREIGN KEY (ClientId) REFERENCES Clients(Id),

)
