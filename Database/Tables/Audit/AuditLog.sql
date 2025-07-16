CREATE TABLE [dbo].[AuditLog]
(
    [Id] INT IDENTITY(1,1) PRIMARY KEY,
    [EntityName] NVARCHAR(100) NOT NULL,
    [EntityId] INT NOT NULL,
    [Action] NVARCHAR(10) NOT NULL, -- Insert, Update, Delete
    [ChangedAt] DATETIME2 NOT NULL DEFAULT GETUTCDATE(),
    [ChangedBy] INT NOT NULL,
    [ChangedLocation] NVARCHAR(100) NULL
);
