CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL UNIQUE, 
    [InternalCode] NCHAR(5) NOT NULL UNIQUE, 
    [BarCode] NCHAR(13) NULL UNIQUE, 
    [UnitType] NCHAR(1) NOT NULL,
    [Price] MONEY NOT NULL, 
    [Description] NVARCHAR(255) NULL, 
    [TemperatureCondition] NVARCHAR(10) NOT NULL, 
    [Stock] INT NOT NULL, 
    [Image] NVARCHAR(255) NULL, 
    [Observations] NVARCHAR(255) NULL, 
    [SubCategoryId] INT NOT NULL, 
    CONSTRAINT [FK_Products_SubCategories] FOREIGN KEY ([SubCategoryId]) REFERENCES [SubCategories]([Id]),
    CONSTRAINT [CHK_Product_UnitType] CHECK (UnitType IN ('K', 'U')),
    CONSTRAINT [CHK_Product_TempCondition] CHECK (TemperatureCondition IN ('Frio', 'Congelado', 'Natural')),
    CONSTRAINT [CHK_Product_Price] CHECK (Price > 0),
    CONSTRAINT [CHK_Product_Stock] CHECK (Stock >= 0),
    CONSTRAINT [CHK_Product_BarcodeFormat] CHECK ([Barcode] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]')
)
