CREATE TABLE Images (
    Id INT IDENTITY(1,1) PRIMARY KEY,      -- ID de la imagen (clave primaria autoincremental)
    EntityType NVARCHAR(255) NOT NULL,      -- Tipo de entidad (ej. "products", "zones", etc.)
    EntityId INT NOT NULL,                  -- ID de la entidad asociada (ej. el ID de un producto o zona)
    FileName NVARCHAR(255) NOT NULL,        -- Nombre del archivo (ej. "image.jpg")
    BlobName NVARCHAR(255) NOT NULL,        -- Nombre del archivo en el almacenamiento en Blob
    ContentType NVARCHAR(100) NOT NULL,     -- Tipo de contenido (ej. "image/jpeg")
    Size BIGINT NOT NULL,                   -- Tamaño del archivo en bytes
    UploadDate DATETIME NOT NULL,           -- Fecha de carga de la imagen
    CreatedAt DATETIME DEFAULT GETDATE(),   -- Fecha de creación de la imagen
    CreatedBy NVARCHAR(255),                -- Quién creó la imagen
    UpdatedAt DATETIME,                     -- Fecha de última actualización
    UpdatedBy NVARCHAR(255),                -- Quién actualizó la imagen
    DeletedAt DATETIME,                     -- Fecha de eliminación (para soft delete)
    DeletedBy NVARCHAR(255)                 -- Quién eliminó la imagen
);
