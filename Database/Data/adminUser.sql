-- Paso 1: Desactivar constraints que se cruzan
ALTER TABLE dbo.Users NOCHECK CONSTRAINT FK_Users_CreatedBy;
ALTER TABLE dbo.Users NOCHECK CONSTRAINT FK_Users_Roles;
ALTER TABLE dbo.Roles NOCHECK CONSTRAINT FK_Roles_CreatedBy;

-- Paso 2: Insertar rol Administrador con Id = 1
SET IDENTITY_INSERT dbo.Roles ON;

INSERT INTO dbo.Roles (Id, Name, Description, CreatedAt, CreatedBy, CreatedLocation, IsDeleted)
VALUES (1, 'Administrador', 'Acceso total al sistema', SYSDATETIME(), 1, 'Lat: 0.0, Lng: 0.0', 0);

SET IDENTITY_INSERT dbo.Roles OFF;

-- Paso 3: Insertar usuario admin con Id = 1
SET IDENTITY_INSERT dbo.Users ON;

INSERT INTO dbo.Users (Id, Name, Username, Password, RoleId, CreatedAt, CreatedBy, CreatedLocation, IsDeleted)
VALUES (1, 'Admin', 'admin', 'admin123', 1, SYSDATETIME(), 1, 'Lat: 0.0, Lng: 0.0', 0);

SET IDENTITY_INSERT dbo.Users OFF;

-- Paso 4: Asignar todos los permisos al rol administrador
DECLARE @RoleId INT = 1;
DECLARE @PermissionId INT = 1;

WHILE @PermissionId <= 73
BEGIN
    INSERT INTO dbo.Roles_Permissions (RoleId, PermissionId)
    VALUES (@RoleId, @PermissionId);

    SET @PermissionId += 1;
END

-- Paso 5: Reactivar constraints
ALTER TABLE dbo.Users CHECK CONSTRAINT FK_Users_CreatedBy;
ALTER TABLE dbo.Users CHECK CONSTRAINT FK_Users_Roles;
ALTER TABLE dbo.Roles CHECK CONSTRAINT FK_Roles_CreatedBy;
