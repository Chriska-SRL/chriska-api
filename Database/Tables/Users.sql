CREATE TABLE [dbo].[User]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NCHAR(20) NOT NULL, 
    [Username] NCHAR(20) NOT NULL UNIQUE,
    [Password] NCHAR(20) NOT NULL, 
    [isEnabled] BIT NOT NULL DEFAULT 1, 
    [RoleId] INT NOT NULL, 
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id])
    )
