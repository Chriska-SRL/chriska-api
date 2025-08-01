using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
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
                "INSERT INTO Products (InternalCode, BarCode, Name, UnitType, Price, Description, TemperatureCondition, Stock, AvailableStock, Observations, SubCategoryId, BrandId) " +
                "OUTPUT INSERTED.Id VALUES (@InternalCode, @BarCode, @Name, @UnitType, @Price, @Description, @TemperatureCondition, @Stock, @AvailableStock, @Observations, @SubCategoryId, @BrandId)",
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
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AvailableStock", product.AviableStock);
                    cmd.Parameters.AddWithValue("@Observations", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return product;
        }

        #endregion

        #region Update

        public async Task<Product> UpdateAsync(Product product)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET InternalCode = @InternalCode, BarCode = @BarCode, Name = @Name, UnitType = @UnitType, Price = @Price, Description = @Description, TemperatureCondition = @TemperatureCondition, Stock = @Stock, AvailableStock = @AvailableStock, Observations = @Observations, SubCategoryId = @SubCategoryId, BrandId = @BrandId " +
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
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AvailableStock", product.AviableStock);
                    cmd.Parameters.AddWithValue("@Observations", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                }
            );

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
            return await ExecuteReadAsync(
                baseQuery: @"SELECT p.*, 
                                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription, 
                                   b.Id AS BrandId, b.Name AS BrandName, b.description AS BrandDescription,
                                   c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription, c.Address AS CategoryAddress,
                                   s.Id AS SupplierId, s.Name AS SupplierName, s.RazonSocial AS SupplierRazónSocial, s.Address AS SupplierAddress, 
                                   s.Phone AS SupplierPhone, s.Email AS SupplierEmail, s.ContactName AS SupplierContactName, s.RUT AS SupplierRUT, 
                                   s.MapsAddress AS SupplierMapsAddress, s.Observations AS SupplierObservations    
                            FROM Products p
                            INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                            INNER JOIN Categories c ON sc.CategoryId = c.Id
                            INNER JOIN Brands b ON p.BrandId = b.Id
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
                tableAlias: "p"
            );
        }


        #endregion

        #region GetById
        //no trae las cuentas bancarias de los proveedores
        public async Task<Product?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"SELECT p.*, 
                                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription, 
                                   b.Id AS BrandId, b.Name AS BrandName, b.description AS BrandDescription,
                                   c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription, c.Address AS CategoryAddress,
                                   s.Id AS SupplierId, s.Name AS SupplierName, s.RazonSocial AS SupplierRazónSocial, s.Address AS SupplierAddress, 
                                   s.Phone AS SupplierPhone, s.Email AS SupplierEmail, s.ContactName AS SupplierContactName, s.RUT AS SupplierRUT, 
                                   s.MapsAddress AS SupplierMapsAddress, s.Observations AS SupplierObservations    
                            FROM Products p
                            INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                            INNER JOIN Categories c ON sc.CategoryId = c.Id
                            INNER JOIN Brands b ON p.BrandId = b.Id
                            LEFT JOIN Products_Suppliers ps ON p.Id = ps.ProductId
                            LEFT JOIN Suppliers s ON ps.SupplierId = s.Id
                            WHERE p.Id = @Id",
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

                    return product;
                },
                options: new QueryOptions(),
                tableAlias: "p",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
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

    }
}
