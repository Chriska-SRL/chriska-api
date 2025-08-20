-- =============================================
-- Precarga de Productos
-- =============================================

INSERT INTO dbo.Brands(Name)
VALUES ('Coca Cola'), ('Pepsi'), ('Nestlé'), ('Unilever');


INSERT INTO dbo.Categories (Name)
VALUES ('Bebidas'), ('Snacks'), ('Lácteos'), ('Limpieza');


INSERT INTO dbo.SubCategories (Name, CategoryId)
VALUES 
    ('Gaseosas', 1),
    ('Jugos', 1),
    ('Papitas', 2),
    ('Galletitas', 2),
    ('Leche', 3),
    ('Yogures', 3),
    ('Detergentes', 4),
    ('Desinfectantes', 4);

INSERT INTO dbo.Products 
    (Barcode, InternalCode, Name, UnitType, Price, Stock, AvailableStock, Observations, BrandId, SubCategoryId, CreatedAt)
VALUES
    ('7731234567890', 'P001', 'Coca Cola 1.5L', 1, 150.00, 100, 100, 'Botella retornable', 1, 1, GETDATE()),
    ('7731234567891', 'P002', 'Pepsi 1.5L', 1, 140.00, 80, 80, 'Botella no retornable', 2, 1, GETDATE()),
    ('7731234567892', 'P003', 'Jugo de Naranja 1L', 1, 120.00, 50, 50, 'Con pulpa', 3, 2, GETDATE()),
    ('7731234567893', 'P004', 'Papas Lays Clásicas', 2, 90.00, 200, 200, 'Bolsa 200g', 4, 3, GETDATE()),
    ('7731234567894', 'P005', 'Galletitas Oreo', 2, 85.00, 150, 150, 'Paquete 120g', 3, 4, GETDATE()),
    ('7731234567895', 'P006', 'Leche Entera 1L', 1, 70.00, 300, 300, 'UHT', 3, 5, GETDATE()),
    ('7731234567896', 'P007', 'Yogur Frutilla 200ml', 1, 45.00, 250, 250, 'Con trozos de fruta', 3, 6, GETDATE()),
    ('7731234567897', 'P008', 'Detergente Magistral 750ml', 1, 110.00, 120, 120, 'Limón', 4, 7, GETDATE()),
    ('7731234567898', 'P009', 'Desinfectante Lysoform 500ml', 1, 160.00, 90, 90, 'Aerosol antibacterial', 4, 8, GETDATE());
