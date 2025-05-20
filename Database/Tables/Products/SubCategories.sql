CREATE TABLE [dbo].[SubCategories]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL UNIQUE, 
    [CategoryId] INT NOT NULL, 
    CONSTRAINT [FK_SubCategories_Categories] FOREIGN KEY ([CategoryId]) REFERENCES [Categories]([Id])
)
