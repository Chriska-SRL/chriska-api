﻿CREATE TABLE [dbo].[Products]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL UNIQUE,
    [BarCode] NCHAR(13) NULL,
    [UnitType] NCHAR(10) NOT NULL,
    [Price] MONEY NOT NULL, 
    [Description] NVARCHAR(255) NOT NULL, 
    [TemperatureCondition] NVARCHAR(10) NOT NULL, 
    [Stock] INT NOT NULL, 
    [Image] NVARCHAR(255) NOT NULL, 
    [Observations] NVARCHAR(255) NOT NULL, 
    [SubCategoryId] INT NOT NULL, 
    [BrandId] INT NOT NULL, 
    CONSTRAINT [FK_Products_SubCategories] FOREIGN KEY ([SubCategoryId]) REFERENCES [SubCategories]([Id]),
    CONSTRAINT [CHK_Product_UnitType] CHECK (UnitType IN ('Kilo', 'Unit')),
    CONSTRAINT [CHK_Product_TempCondition] CHECK (TemperatureCondition IN ('Cold', 'Frozen', 'Ambient')),
    CONSTRAINT [CHK_Product_Price] CHECK (Price > 0),
    CONSTRAINT [CHK_Product_Stock] CHECK (Stock >= 0),
    CONSTRAINT [CHK_Product_BarcodeFormat] CHECK ([BarCode] IS NULL OR [BarCode] LIKE '[0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9][0-9]'), 
    CONSTRAINT [FK_Products_Brands] FOREIGN KEY ([BrandId]) REFERENCES [Brands]([Id]) 
)
