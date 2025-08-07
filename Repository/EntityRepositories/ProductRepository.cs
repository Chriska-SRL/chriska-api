using BusinessLogic.Common;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
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
                                   s.MapsAddress AS SupplierMapsAddress, s.Observations AS SupplierObservations,
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
                                   s.MapsAddress AS SupplierMapsAddress, s.Observations AS SupplierObservations,
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


    }
}
