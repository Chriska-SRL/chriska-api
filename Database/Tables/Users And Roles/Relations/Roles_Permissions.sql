CREATE TABLE [dbo].[Roles_Permissions]
(
    [RoleId] INT NOT NULL,
    [PermissionId] INT NOT NULL,
    CONSTRAINT PK_Roles_Permissions PRIMARY KEY (RoleId, PermissionId),
    CONSTRAINT FK_Roles_Permissions_Roles FOREIGN KEY (RoleId) REFERENCES Roles(Id)
);
