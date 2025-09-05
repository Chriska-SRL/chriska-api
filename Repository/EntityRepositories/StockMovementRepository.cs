using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using BusinessLogic.Common;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class StockMovementRepository : Repository<StockMovement, StockMovement.UpdatableData>, IStockMovementRepository
    {
        public StockMovementRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        public async Task<StockMovement> AddAsync(StockMovement stockMovement)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                @"INSERT INTO StockMovements (ProductId, Quantity, Type, RasonType, Date, Reason)
                  VALUES (@ProductId, @Quantity, @Type, @RasonType, @Date, @Reason);
                  SELECT SCOPE_IDENTITY();",
                stockMovement,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@ProductId", stockMovement.Product.Id);
                    cmd.Parameters.AddWithValue("@Quantity", stockMovement.Quantity);
                    cmd.Parameters.AddWithValue("@Type", stockMovement.Type.ToString());
                    cmd.Parameters.AddWithValue("@RasonType", stockMovement.RasonType.ToString());
                    cmd.Parameters.AddWithValue("@Date", stockMovement.Date);
                    cmd.Parameters.AddWithValue("@Reason", stockMovement.Reason);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            stockMovement.Id = newId;
            return stockMovement;
        }

        public Task<StockMovement> DeleteAsync(StockMovement entity)
        {
            //no se puede eliminar un movimiento de stock
            throw new NotImplementedException();
        }

        public async Task<List<StockMovement>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "ProductId", "CreatedBy", "Type", "RasonType" }; 

            return await ExecuteReadAsync(
                baseQuery: @"SELECT sm.*,  
                                s.Name AS ShelveName, s.Description AS ShelveDescription, s.WarehouseId,
                                w.Name AS WarehouseName, w.Description AS WarehouseDescription,
                                u.Name AS UserName, u.Username AS UserUsername, u.IsEnabled AS UserIsEnabled, u.NeedsPasswordChange AS UserNeedsPasswordChange, U.RoleId AS RoleId,
                                r.Name AS RoleName, r.Description AS RoleDescription,   
                                p.Name AS ProductName, p.Description AS ProductDescription, p.InternalCode AS ProductInternalCode, p.Barcode AS ProductBarcode, p.UnitType AS ProductUnitType,
                                p.Price AS ProductPrice, p.TemperatureCondition AS ProductTemperatureCondition, p.EstimatedWeight AS ProductEstimatedWeight, p.Stock AS ProductStock,
                                p.AvailableStock AS ProductAvailableStock, p.Observations AS ProductObservations, p.ImageUrl AS ProductImageUrl, p.SubCategoryId AS ProductSubCategoryId,
                                p.BrandId, p.SubCategoryId, p.ShelveId,
                                b.Name AS BrandName, b.Description AS BrandDescription, 
                                sb.Name AS SubCategoryName, sb.Description AS SubCategoryDescription, sb.CategoryId,
                                c.Name AS CategoryName, c.Description AS CategoryDescription,
                                s.Name AS ShelveName, s.Description AS ShelveDescription,
                                w.Name AS WarehouseName, w.Description AS WarehouseDescription,
                                u.Id As UserId, u.Name AS UserName, u.Username AS UserUsername, u.IsEnabled AS UserIsEnabled, u.NeedsPasswordChange AS UserNeedsPasswordChange
                              FROM StockMovements sm
                              INNER JOIN Products p ON p.Id = sm.ProductId    
                              INNER JOIN Shelves s ON s.Id = p.ShelveId
                              INNER JOIN Warehouses w ON w.Id = s.WarehouseId
                              INNER JOIN Users u ON u.Id = sm.CreatedBy
                              INNER JOIN Roles r ON r.Id = u.RoleId
                              INNER JOIN Brands b ON b.Id = p.BrandId
                              INNER JOIN SubCategories sb ON sb.Id = p.SubCategoryId
                              INNER JOIN Categories c ON c.Id = sb.CategoryId",
                map: reader =>
                {
                    var list = new List<StockMovement>();
                    while (reader.Read())
                    {
                        list.Add(StockMovementMapper.FromReader(reader));
                    }
                    return list;
                },
                options: options,
                tableAlias: "sm",
                allowedFilterColumns: allowedFilters
            );
        }

        public async Task<StockMovement?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"SELECT sm.*,  
                                s.Name AS ShelveName, s.Description AS ShelveDescription, s.WarehouseId,
                                w.Name AS WarehouseName, w.Description AS WarehouseDescription,
                                u.Name AS UserName, u.Username AS UserUsername, u.IsEnabled AS UserIsEnabled, u.NeedsPasswordChange AS UserNeedsPasswordChange, U.RoleId AS RoleId,
                                r.Name AS RoleName, r.Description AS RoleDescription,   
                                p.Name AS ProductName, p.Description AS ProductDescription, p.InternalCode AS ProductInternalCode, p.Barcode AS ProductBarcode, p.UnitType AS ProductUnitType,
                                p.Price AS ProductPrice, p.TemperatureCondition AS ProductTemperatureCondition, p.EstimatedWeight AS ProductEstimatedWeight, p.Stock AS ProductStock,
                                p.AvailableStock AS ProductAvailableStock, p.Observations AS ProductObservations, p.ImageUrl AS ProductImageUrl, p.SubCategoryId AS ProductSubCategoryId,
                                p.BrandId, p.SubCategoryId, p.ShelveId,
                                b.Name AS BrandName, b.Description AS BrandDescription, 
                                sb.Name AS SubCategoryName, sb.Description AS SubCategoryDescription, sb.CategoryId,
                                c.Name AS CategoryName, c.Description AS CategoryDescription,
                                s.Name AS ShelveName, s.Description AS ShelveDescription,
                                w.Name AS WarehouseName, w.Description AS WarehouseDescription,
                                u.Id As UserId, u.Name AS UserName, u.Username AS UserUsername, u.IsEnabled AS UserIsEnabled, u.NeedsPasswordChange AS UserNeedsPasswordChange
                              FROM StockMovements sm
                              INNER JOIN Products p ON p.Id = sm.ProductId    
                              INNER JOIN Shelves s ON s.Id = p.ShelveId
                              INNER JOIN Warehouses w ON w.Id = s.WarehouseId
                              INNER JOIN Users u ON u.Id = sm.CreatedBy
                              INNER JOIN Roles r ON r.Id = u.RoleId
                              INNER JOIN Brands b ON b.Id = p.BrandId
                              INNER JOIN SubCategories sb ON sb.Id = p.SubCategoryId
                              INNER JOIN Categories c ON c.Id = sb.CategoryId
                              WHERE sm.Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return StockMovementMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                tableAlias: "sm",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        public Task<StockMovement> UpdateAsync(StockMovement entity)
        {
            //no se puede actualizar un movimiento de stock
            throw new NotImplementedException();
        }
    }
}
