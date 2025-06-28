DROP TRIGGER IF EXISTS trg_PreventInvalidStockMovement;
GO

CREATE TRIGGER trg_PreventInvalidStockMovement
ON StockMovements
INSTEAD OF INSERT
AS
BEGIN
    SET NOCOUNT ON;

    -- Validar stock en Products
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN Products p ON i.ProductId = p.Id
        WHERE i.Type = 'E' AND p.Stock < i.Quantity
    )
    BEGIN
        RAISERROR('Stock insuficiente en producto.', 16, 1);
        RETURN;
    END

    -- Validar stock en ProductsStock
    IF EXISTS (
        SELECT 1
        FROM inserted i
        JOIN ProductsStock ps ON i.ProductId = ps.ProductId AND i.ShelveId = ps.ShelveId
        WHERE i.Type = 'E' AND ps.Quantity < i.Quantity
    )
    BEGIN
        RAISERROR('Stock insuficiente en estantería.', 16, 1);
        RETURN;
    END

    -- Insertar y devolver ID
    DECLARE @output TABLE (Id INT);

    INSERT INTO StockMovements (Quantity, Type, Reason, Date, ProductId, UserId, ShelveId)
    OUTPUT INSERTED.Id INTO @output
    SELECT Quantity, Type, Reason, Date, ProductId, UserId, ShelveId
    FROM inserted;

    -- Actualizar stock en Products
    UPDATE p
    SET p.Stock =
        CASE i.Type
            WHEN 'I' THEN p.Stock + i.Quantity
            WHEN 'E' THEN p.Stock - i.Quantity
        END
    FROM Products p
    INNER JOIN inserted i ON p.Id = i.ProductId;

    -- Actualizar o insertar en ProductsStock
    MERGE ProductsStock AS target
    USING inserted AS i
        ON target.ProductId = i.ProductId AND target.ShelveId = i.ShelveId
    WHEN MATCHED THEN
        UPDATE SET Quantity =
            CASE i.Type
                WHEN 'I' THEN target.Quantity + i.Quantity
                WHEN 'E' THEN target.Quantity - i.Quantity
            END
    WHEN NOT MATCHED BY TARGET AND i.Type = 'I' THEN
        INSERT (ProductId, ShelveId, Quantity)
        VALUES (i.ProductId, i.ShelveId, i.Quantity);

    -- Eliminar si queda stock cero
    DELETE FROM ProductsStock
    WHERE Quantity = 0;

    -- Devolver el ID al cliente
    SELECT Id FROM @output;
END
