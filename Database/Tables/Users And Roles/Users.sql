CREATE TABLE [dbo].[Users]
(
	[Id] INT NOT NULL PRIMARY KEY IDENTITY, 
    [Name] NVARCHAR(50) NOT NULL, 
    [Username] NVARCHAR(50) NOT NULL UNIQUE,
    [Password] NVARCHAR(100) NOT NULL, 
    [IsEnabled] NCHAR(1) NOT NULL DEFAULT 'T', 
    [needsPasswordChange] NCHAR(1) NOT NULL DEFAULT 'T', 
    [RoleId] INT NOT NULL,
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]),
    CONSTRAINT [CHK_User_IsEnabled] CHECK ([IsEnabled] IN ('T', 'F')),
    CONSTRAINT [CHK_User_needsPasswordChange] CHECK ([needsPasswordChange] IN ('T', 'F'))
)
