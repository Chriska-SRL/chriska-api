using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Options;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class ProductRepository : Repository<Product, Product.UpdatableData>, IProductRepository
    {
        public ProductRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Product> AddAsync(Product product)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Products (InternalCode, BarCode, Name, UnitType, Price, Description, TemperatureCondition, EstimatedWeight, Stock, AvailableStock, Observations, SubCategoryId, BrandId, ShelveId) " +
                "VALUES (@InternalCode, @BarCode, @Name, @UnitType, @Price, @Description, @TemperatureCondition, @EstimatedWeight, @Stock, @AvailableStock, @Observations, @SubCategoryId, @BrandId, @ShelveId); " +
                "SELECT CAST(SCOPE_IDENTITY() AS INT);",
                product,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@InternalCode", product.InternalCode);
                    cmd.Parameters.AddWithValue("@BarCode", product.Barcode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@UnitType", product.UnitType.ToString());
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@TemperatureCondition", product.TemperatureCondition.ToString());
                    cmd.Parameters.AddWithValue("@EstimatedWeight", product.EstimatedWeight);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AvailableStock", product.AvailableStocks);
                    cmd.Parameters.AddWithValue("@Observations", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                    cmd.Parameters.AddWithValue("@ShelveId", product.Shelve.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            await AddProductSuppliersAsync(newId, product.Suppliers);
            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");
            product.Id = newId;
            product.SetInternalCode();
            return product;
        }

        #endregion

        #region Update

        public async Task<Product> UpdateAsync(Product product)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET InternalCode = @InternalCode, BarCode = @BarCode, Name = @Name, UnitType = @UnitType, Price = @Price, Description = @Description, TemperatureCondition = @TemperatureCondition, EstimatedWeight = @EstimatedWeight, Stock = @Stock, AvailableStock = @AvailableStock, Observations = @Observations, SubCategoryId = @SubCategoryId, BrandId = @BrandId, ShelveId = @ShelveId " +
                "WHERE Id = @Id",
                product,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@InternalCode", product.InternalCode);
                    cmd.Parameters.AddWithValue("@BarCode", product.Barcode ?? (object)DBNull.Value);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@UnitType", product.UnitType.ToString());
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@TemperatureCondition", product.TemperatureCondition.ToString());
                    cmd.Parameters.AddWithValue("@EstimatedWeight", product.EstimatedWeight);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AvailableStock", product.AvailableStocks);
                    cmd.Parameters.AddWithValue("@Observations", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                    cmd.Parameters.AddWithValue("@ShelveId", product.Shelve.Id);
                }
            );

            await DeleteProductSuppliersAsync(product.Id);
            await AddProductSuppliersAsync(product.Id, product.Suppliers);

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el producto con Id {product.Id}");

            return product;
        }

        #endregion

        #region Delete

        public async Task<Product> DeleteAsync(Product product)
        {
            if (product == null)
                throw new ArgumentNullException(nameof(product), "El producto no puede ser nulo.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET IsDeleted = 1 WHERE Id = @Id",
                product,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar el producto con Id {product.Id}");

            return product;
        }


        #endregion

        #region GetAll
        //no trae las cuentas bancarias de los proveedores
        public async Task<List<Product>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Name", "InternalCode", "BarCode", "UnitType", "BrandId", "SubCategoryId", "ShelveId"};
            return await ExecuteReadAsync(
                baseQuery: @"SELECT p.*, 
                                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription, 
                                   b.Id AS BrandId, b.Name AS BrandName, b.description AS BrandDescription,
                                   c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription,
                                   s.Id AS SupplierId, s.Name AS SupplierName, s.RazonSocial AS SupplierRazonSocial, s.Address AS SupplierAddress, 
                                   s.Phone AS SupplierPhone, s.Email AS SupplierEmail, s.ContactName AS SupplierContactName, s.RUT AS SupplierRUT, 
                                   s.Location AS SupplierLocation, s.Observations AS SupplierObservations,
                                   sh.Id AS ShelveId, sh.Name AS ShelveName, sh.Description AS ShelveDescription,
                                   w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription
                            FROM Products p
                            INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                            INNER JOIN Categories c ON sc.CategoryId = c.Id
                            INNER JOIN Brands b ON p.BrandId = b.Id
                            INNER JOIN Shelves sh ON p.ShelveId = sh.Id
                            INNER JOIN Warehouses w ON sh.WarehouseId = w.Id
                            LEFT JOIN Products_Suppliers ps ON p.Id = ps.ProductId
                            LEFT JOIN Suppliers s ON ps.SupplierId = s.Id",
                map: reader =>
                {
                    var products = new Dictionary<int, Product>();

                    while (reader.Read())
                    {
                        var productId = (int)reader["Id"];
                        if (!products.TryGetValue(productId, out var product))
                        {
                            var suppliers = new List<Supplier>();
                            product = ProductMapper.FromReader(reader);
                            products[productId] = product;
                        }

                        var supplierIdObj = reader["SupplierId"];
                        if (supplierIdObj != DBNull.Value)
                        {
                            var supplier = SupplierMapper.FromReaderForProduct(reader);
                            if (!product.Suppliers.Any(s => s.Id == supplier.Id))
                                product.Suppliers.Add(supplier);
                        }
                    }

                    return products.Values.ToList();
                },
                options: options,
                allowedFilterColumns: allowedFilters,
                tableAlias: "p"
            );
        }


        #endregion

        #region GetById
        //no trae las cuentas bancarias de los proveedores
        public async Task<Product?> GetByIdAsync(int id)
        {
           return await GetByFieldAsync("Id", id.ToString());
        }

        private async Task<Product?> GetByFieldAsync(string fieldName, string value)
        {
            return await ExecuteReadAsync(
                baseQuery: $@"SELECT p.*, 
                                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription, 
                                   b.Id AS BrandId, b.Name AS BrandName, b.description AS BrandDescription,
                                   c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription,
                                   s.Id AS SupplierId, s.Name AS SupplierName, s.RazonSocial AS SupplierRazonSocial, s.Address AS SupplierAddress, 
                                   s.Phone AS SupplierPhone, s.Email AS SupplierEmail, s.ContactName AS SupplierContactName, s.RUT AS SupplierRUT, 
                                   s.Location AS SupplierLocation, s.Observations AS SupplierObservations,
                                   sh.Id AS ShelveId, sh.Name AS ShelveName, sh.Description AS ShelveDescription,
                                   w.Id AS WarehouseId, w.Name AS WarehouseName, w.Description AS WarehouseDescription
                            FROM Products p
                            INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                            INNER JOIN Categories c ON sc.CategoryId = c.Id
                            INNER JOIN Brands b ON p.BrandId = b.Id
                            INNER JOIN Shelves sh ON p.ShelveId = sh.Id
                            INNER JOIN Warehouses w ON sh.WarehouseId = w.Id
                            LEFT JOIN Products_Suppliers ps ON p.Id = ps.ProductId
                            LEFT JOIN Suppliers s ON ps.SupplierId = s.Id
                            WHERE p.{fieldName} = @Value",
                map: reader =>
                {
                    Product? product = null;
                    var suppliers = new List<Supplier>();

                    while (reader.Read())
                    {
                        if (product is null)
                        {
                            product = ProductMapper.FromReader(reader);
                        }

                        var supplierIdObj = reader["SupplierId"];
                        if (supplierIdObj != DBNull.Value)
                        {
                            var supplier = SupplierMapper.FromReaderForProduct(reader);
                            if (!suppliers.Any(s => s.Id == supplier.Id))
                                suppliers.Add(supplier);
                        }
                    }
                    if (product != null) product.Suppliers = suppliers;
                    return product;
                },
                options: new QueryOptions(),
                tableAlias: "p",
                configureCommand: cmd => cmd.Parameters.AddWithValue("@Value", value));
        }

        #endregion
        public async Task<string> UpdateImageUrlAsync(Product product, string imageUrl)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET ImageUrl = @ImageUrl WHERE Id = @Id",
                product,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@ImageUrl", imageUrl);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la imagen del producto con Id {product.Id}");

            return imageUrl;
        }

        private async Task AddProductSuppliersAsync(int productId, IEnumerable<Supplier> suppliers)
        {
            if (suppliers == null || !suppliers.Any())
                return;

            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                var values = new List<string>();
                var parameters = new List<SqlParameter>();
                int i = 0;

                foreach (var supplier in suppliers)
                {
                    values.Add($"(@ProductId, @SupplierId{i})");
                    parameters.Add(new SqlParameter($"@SupplierId{i}", supplier.Id));
                    i++;
                }

                string sql = $@"INSERT INTO Products_Suppliers (ProductId, SupplierId)
                        VALUES {string.Join(", ", values)}";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ProductId", productId);
                foreach (var p in parameters)
                    cmd.Parameters.Add(p);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al agregar proveedores del producto.", ex);
            }
        }

        private async Task DeleteProductSuppliersAsync(int productId)
        {
            try
            {
                using var connection = new SqlConnection(_connectionString);
                await connection.OpenAsync();

                const string sql = @"DELETE FROM Products_Suppliers WHERE ProductId = @ProductId";

                using var cmd = new SqlCommand(sql, connection);
                cmd.Parameters.AddWithValue("@ProductId", productId);

                await cmd.ExecuteNonQueryAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("Error al eliminar proveedores del producto.", ex);
            }
        }

        public async Task UpdateStockAsync(int id, int stock, int availableStock)
        {
            int rows = await ExecuteWriteAsync(
                "UPDATE Products " +
                "SET Stock = Stock + @Stock, " +
                "    AvailableStock = AvailableStock + @AvailableStock " +
                "WHERE Id = @Id",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id",id);
                    cmd.Parameters.AddWithValue("@Stock", stock);
                    cmd.Parameters.AddWithValue("@AvailableStock", availableStock);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar el stock del producto con Id {id}");

        }
        public async Task<Product?> GetByIdWithDiscountsAsync(int productId)
        {
            var product = await GetByFieldAsync("Id", productId.ToString());
            if (product is null) return null;

            var discounts = await ExecuteReadAsync(
                baseQuery: @"
                            SELECT
                    d.*,
                    db.Id   AS DBrandId,       db.Name AS DBrandName,       db.Description AS DBrandDescription,
                    dsc.Id  AS DSubCategoryId, dsc.Name AS DSubCategoryName, dsc.Description AS DSubCategoryDescription,
                    dcat.Id AS DCategoryId,    dcat.Name AS DCategoryName,  dcat.Description AS DCategoryDescription,
                    dz.Id   AS DZoneId,        dz.Name AS DZoneName,        dz.Description AS DZoneDescription,
                    dz.ImageUrl AS DZoneImageUrl, dz.DeliveryDays AS DZoneDeliveryDays, dz.RequestDays AS DZoneRequestDays,

                    cl.Id  AS ClientId, cl.Name AS ClientName, cl.RUT AS ClientRUT, cl.RazonSocial AS ClientRazonSocial,
                    cl.Address AS ClientAddress, cl.MapsAddress AS ClientMapsAddress, cl.Schedule AS ClientSchedule,
                    cl.Phone AS ClientPhone, cl.ContactName AS ClientContactName, cl.Email AS ClientEmail,
                    cl.Observations AS ClientObservations, cl.LoanedCrates AS ClientLoanedCrates, cl.Qualification AS ClientQualification
                FROM Discounts d
                LEFT JOIN Brands        db   ON d.BrandId       = db.Id
                LEFT JOIN SubCategories dsc  ON d.SubCategoryId = dsc.Id
                LEFT JOIN Categories    dcat ON dsc.CategoryId  = dcat.Id
                LEFT JOIN Zones         dz   ON d.ZoneId        = dz.Id
                LEFT JOIN DiscountClients dcl ON d.Id = dcl.DiscountId
                LEFT JOIN Clients         cl  ON dcl.ClientId = cl.Id
                WHERE
                      (EXISTS (SELECT 1 FROM DiscountProducts dp WHERE dp.DiscountId = d.Id AND dp.ProductId = @ProductId)  -- por producto
                   OR d.BrandId = @BrandId                                                                                  -- por marca
                   OR d.SubCategoryId = @SubCategoryId                                                                      -- por subcat
                   OR (d.BrandId IS NULL AND d.SubCategoryId IS NULL
                       AND NOT EXISTS (SELECT 1 FROM DiscountProducts dp2 WHERE dp2.DiscountId = d.Id)))                  -- global
                AND d.Status = @Active AND d.ExpirationDate >= SYSUTCDATETIME()
                ",
                               map: r =>
                               {
                                   var dict = new Dictionary<int, Discount>();
                                   while (r.Read())
                                   {
                                       var id = (int)r["Id"]; 
                                       if (!dict.TryGetValue(id, out var d))
                                       {
                                           d = DiscountMapper.FromReader(r);
                                           dict[id] = d;
                                       }
                                       if (r["ClientId"] != DBNull.Value)
                                       {
                                           var client = ClientMapper.FromReader(r, "Client"); 
                                           if (!d.Clients.Any(c => c.Id == client.Id)) d.Clients.Add(client);
                                       }
                                   }
                                   return dict.Values.ToList();
                               },
                    options: new QueryOptions(),
                    tableAlias: "d",
                    configureCommand: cmd =>
                    {
                        cmd.Parameters.AddWithValue("@ProductId", productId);
                        cmd.Parameters.AddWithValue("@BrandId", (object?)product.Brand?.Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@SubCategoryId", (object?)product.SubCategory?.Id ?? DBNull.Value);
                        cmd.Parameters.AddWithValue("@Active", DiscountStatus.Available.ToString()); 
                    });

            product.Discounts = discounts;
            return product;
        }
    }
}
