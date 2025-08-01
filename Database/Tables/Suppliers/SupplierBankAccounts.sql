CREATE TABLE [dbo].[SupplierBankAccounts]
(
	 -- Clave primaria de la entidad
    [Id] INT NOT NULL PRIMARY KEY IDENTITY,

    -- Campos de la entidad
    [BankName] NVARCHAR(50) NOT NULL, 
    [AccountName] NVARCHAR(50) NOT NULL, 
    [AccountNumber] NVARCHAR(50) NOT NULL,
    [SupplierId] INT NOT NULL, 

    CONSTRAINT FK_SupplierBankAccounts_SupplierId FOREIGN KEY (SupplierId) REFERENCES Suppliers(Id)
)
