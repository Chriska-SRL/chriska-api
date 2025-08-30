using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class PurchaseRepository : Repository<Purchase, ProductDocument.UpdatableData>, IPurchaseRepository
    {
        public PurchaseRepository(string connectionString, AuditLogger auditLogger) : base(connectionString, auditLogger) { }

        #region Query (base)

        private readonly string baseQuery = @"
            SELECT
                -- Purchase
                p.*,

                -- Supplier (prefijo Supplier)
                s.Id              AS SupplierId,
                s.Name            AS SupplierName,
                s.RUT             AS SupplierRUT,
                s.RazonSocial     AS SupplierRazonSocial,
                s.Address         AS SupplierAddress,
                s.Location        AS SupplierLocation,
                s.Phone           AS SupplierPhone,
                s.ContactName     AS SupplierContactName,
                s.Email           AS SupplierEmail,
                s.Observations    AS SupplierObservations,

                -- User (prefijo User)
                u.Id              AS UserId,
                u.Name            AS UserName,
                u.Username        AS UserUsername,
                u.Password        AS UserPassword,
                u.IsEnabled       AS UserIsEnabled,
                u.NeedsPasswordChange AS UserNeedsPasswordChange,

                -- Role (prefijo UserRole)
                r.Id              AS UserRoleId,
                r.Name            AS UserRoleName,
                r.Description     AS UserRoleDescription,

                -- Items (prefijo PP_)
                pp.PurchaseId     AS PP_PurchaseId,
                pp.Quantity       AS PP_Quantity,
                pp.UnitPrice      AS PP_UnitPrice,
                pp.Discount       AS PP_Discount,
                pp.Weight         AS PP_Weight,

                -- Product (prefijo Product)
                pr.Id             AS ProductId,
                pr.Name           AS ProductName,
                pr.Description    AS ProductDescription,
                pr.InternalCode   AS ProductInternalCode,
                pr.Barcode        AS ProductBarcode,
                pr.UnitType       AS ProductUnitType,
                pr.Price          AS ProductPrice,
                pr.TemperatureCondition AS ProductTemperatureCondition,
                pr.EstimatedWeight AS ProductEstimatedWeight,
                pr.Stock          AS ProductStock,
                pr.AvailableStock AS ProductAvailableStock,
                pr.Observations   AS ProductObservations,
                pr.ImageUrl       AS ProductImageUrl,
                pr.SubCategoryId  AS SubCategoryId,
                pr.BrandId        AS BrandId,
                pr.ShelveId       AS ProductShelveId

            FROM Purchases p
            LEFT JOIN Suppliers s      ON s.Id = p.SupplierId
            LEFT JOIN Users u          ON u.Id = p.CreatedBy
            LEFT JOIN Roles r          ON r.Id = u.RoleId
            LEFT JOIN Purchases_Products pp ON pp.PurchaseId = p.Id
            LEFT JOIN Products pr      ON pr.Id = pp.ProductId
        ";

        #endregion

        #region Add

        public async Task<Purchase> AddAsync(Purchase purchase)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Purchases (Date, Observations, SupplierId, InvoiceNumber, CreatedAt, CreatedBy, CreatedLocation, IsDeleted) " +
                "OUTPUT INSERTED.Id VALUES (@Date, @Observations, @SupplierId, @InvoiceNumber, @CreatedAt, @CreatedBy, @CreatedLocation, 0)",
                purchase,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Date", purchase.Date);
                    cmd.Parameters.AddWithValue("@Observations", (object?)purchase.Observations ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SupplierId", purchase.Supplier?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", (object?)purchase.InvoiceNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedAt", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@CreatedBy", purchase.User?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@CreatedLocation", (object?)purchase.AuditInfo?.CreatedLocation?.ToString() ?? DBNull.Value);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            await AddProductItems(newId, purchase.ProductItems);

            purchase.Id = newId;
            return purchase;
        }

        #endregion

        #region Update

        public async Task<Purchase> UpdateAsync(Purchase purchase)
        {
            await DeleteProductItems(purchase.Id);
            await AddProductItems(purchase.Id, purchase.ProductItems);

            int rows = await ExecuteWriteWithAuditAsync(
                @"UPDATE Purchases SET 
                        Observations = @Observations,
                        SupplierId = @SupplierId,
                        InvoiceNumber = @InvoiceNumber,
                        UpdatedAt = @UpdatedAt,
                        UpdatedBy = @UpdatedBy,
                        UpdatedLocation = @UpdatedLocation
                  WHERE Id = @Id",
                purchase,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", purchase.Id);
                    cmd.Parameters.AddWithValue("@Observations", (object?)purchase.Observations ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@SupplierId", purchase.Supplier?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@InvoiceNumber", (object?)purchase.InvoiceNumber ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedAt", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@UpdatedBy", purchase.User?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@UpdatedLocation", (object?)purchase.AuditInfo?.UpdatedLocation?.ToString() ?? DBNull.Value);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la compra con Id {purchase.Id}");

            return purchase;
        }

        #endregion

        #region Delete

        public async Task<Purchase> DeleteAsync(Purchase purchase)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Purchases SET IsDeleted = 1, DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation WHERE Id = @Id",
                purchase,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", purchase.Id);
                    cmd.Parameters.AddWithValue("@DeletedAt", DateTime.UtcNow);
                    cmd.Parameters.AddWithValue("@DeletedBy", purchase.User?.Id ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@DeletedLocation", (object?)purchase.AuditInfo?.DeletedLocation?.ToString() ?? DBNull.Value);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la compra con Id {purchase.Id}");

            return purchase;
        }

        #endregion

        #region GetAll

        public async Task<List<Purchase>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Date", "Observations", "Status", "SupplierId", "InvoiceNumber" };
            var dict = new Dictionary<int, Purchase>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery,
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int id = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(id, out var purchase))
                        {
                            purchase = PurchaseMapper.FromReader(reader);
                            dict.Add(id, purchase);
                        }

                        var item = ProductItemMapper.FromReader(reader, "PP_");
                        if (item is not null) purchase!.ProductItems.Add(item);
                    }
                    return dict.Values.ToList();
                },
                options: options,
                tableAlias: "p",
                allowedFilterColumns: allowedFilters
            );
        }

        #endregion

        #region GetById

        public async Task<Purchase?> GetByIdAsync(int id)
        {
            var dict = new Dictionary<int, Purchase>();

            return await ExecuteReadAsync(
                baseQuery: baseQuery + " WHERE p.Id = @Id",
                map: reader =>
                {
                    while (reader.Read())
                    {
                        int purId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!dict.TryGetValue(purId, out var purchase))
                        {
                            purchase = PurchaseMapper.FromReader(reader);
                            dict.Add(purId, purchase);
                        }

                        var item = ProductItemMapper.FromReader(reader, "PP_");
                        if (item is not null) purchase!.ProductItems.Add(item);
                    }

                    return dict.Values.FirstOrDefault();
                },
                options: new QueryOptions(),
                tableAlias: "p",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Id", id)
            );
        }

        #endregion

        #region Private helpers (items)

        private async Task AddProductItems(int purchaseId, List<ProductItem> productItems)
        {
            if (productItems == null || !productItems.Any())
                return;

            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            var values = new List<string>();
            var parameters = new List<SqlParameter>();
            int i = 0;

            foreach (var item in productItems)
            {
                values.Add($"(@PurchaseId, @ProductId{i}, @Quantity{i}, @UnitPrice{i}, @Discount{i}, @Weight{i})");
                parameters.Add(new SqlParameter($"@ProductId{i}", item.Product.Id));
                parameters.Add(new SqlParameter($"@Quantity{i}", item.Quantity));
                parameters.Add(new SqlParameter($"@UnitPrice{i}", item.UnitPrice));
                parameters.Add(new SqlParameter($"@Discount{i}", item.Discount));
                parameters.Add(new SqlParameter($"@Weight{i}", (object?)item.Weight ?? DBNull.Value));
                i++;
            }

            string sql = $@"INSERT INTO Purchases_Products (PurchaseId, ProductId, Quantity, UnitPrice, Discount, Weight)
                            VALUES {string.Join(", ", values)}";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);
            foreach (var p in parameters) cmd.Parameters.Add(p);

            await cmd.ExecuteNonQueryAsync();
        }

        private async Task DeleteProductItems(int purchaseId)
        {
            using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            const string sql = @"DELETE FROM Purchases_Products WHERE PurchaseId = @PurchaseId";

            using var cmd = new SqlCommand(sql, connection);
            cmd.Parameters.AddWithValue("@PurchaseId", purchaseId);

            await cmd.ExecuteNonQueryAsync();
        }

        #endregion
    }
}