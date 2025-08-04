CREATE TABLE [dbo].[Users]
(
    -- Clave primaria
    [Id] INT NOT NULL PRIMARY KEY IDENTITY, 

    -- Campos de la entidad
    [Name] NVARCHAR(50) NOT NULL, 
    [Username] NVARCHAR(50) NOT NULL,
    [Password] NVARCHAR(100) NOT NULL, 
    [IsEnabled] NCHAR(1) NOT NULL DEFAULT 'T', 
    [NeedsPasswordChange] NCHAR(1) NOT NULL DEFAULT 'T', 
    [RoleId] INT NOT NULL,

    -- Auditoría
    [CreatedAt] DATETIME2 NOT NULL,
    [CreatedBy] INT NOT NULL,
    [CreatedLocation] NVARCHAR(50) NULL,
    [UpdatedAt] DATETIME2 NULL,
    [UpdatedBy] INT NULL,
    [UpdatedLocation] NVARCHAR(50) NULL,
    [DeletedAt] DATETIME2 NULL,
    [DeletedBy] INT NULL,
    [DeletedLocation] NVARCHAR(50) NULL,
    
    -- Soft delete
    [IsDeleted] BIT NOT NULL DEFAULT 0,

    -- Foreign keys
    CONSTRAINT [FK_Users_Roles] FOREIGN KEY ([RoleId]) REFERENCES [Roles]([Id]),
    CONSTRAINT [FK_Users_CreatedBy] FOREIGN KEY ([CreatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Users_UpdatedBy] FOREIGN KEY ([UpdatedBy]) REFERENCES [Users]([Id]),
    CONSTRAINT [FK_Users_DeletedBy] FOREIGN KEY ([DeletedBy]) REFERENCES [Users]([Id]),

    -- Checks
    CONSTRAINT [CHK_User_IsEnabled] CHECK ([IsEnabled] IN ('T', 'F')),
    CONSTRAINT [CHK_User_NeedsPasswordChange] CHECK ([NeedsPasswordChange] IN ('T', 'F'))
)
