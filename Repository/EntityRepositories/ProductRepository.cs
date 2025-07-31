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
                "INSERT INTO Products (BarCode, Name, UnitType, Price, Description, TemperatureCondition, Stock, AvailableStock, Observations, SubCategoryId, BrandId) " +
                "OUTPUT INSERTED.Id VALUES (@BarCode, @Name, @UnitType, @Price, @Description, @TemperatureCondition, @Stock, @AvailableStock, @Observations, @SubCategoryId, @BrandId)",
                product,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
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

            // Asociar la imagen al producto
            product.SetInternalCode(); // Generar el código interno
            return product;
        }

        #endregion

        #region Update

        public async Task<Product> UpdateAsync(Product product)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET BarCode = @BarCode, Name = @Name, UnitType = @UnitType, Price = @Price, Description = @Description, TemperatureCondition = @TemperatureCondition, Stock = @Stock, AvailableStock = @AvailableStock, Observations = @Observations, SubCategoryId = @SubCategoryId, BrandId = @BrandId " +
                "WHERE Id = @Id",
                product,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
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
                "UPDATE Products SET IsDeleted = 1, DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation WHERE Id = @Id",
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

        public async Task<List<Product>> GetAllAsync(QueryOptions options)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT p.*, 
                           sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, 
                           b.Id AS BrandId, b.Name AS BrandName,
                           img.FileName AS ImageFileName, img.BlobName AS ImageBlobName
                    FROM Products p
                    INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    INNER JOIN Brands b ON p.BrandId = b.Id
                    LEFT JOIN Images img ON img.EntityType = 'products' AND img.EntityId = p.Id
                    WHERE p.IsDeleted = 0",
                map: reader =>
                {
                    var products = new List<Product>();
                    while (reader.Read())
                    {
                        var product = ProductMapper.FromReader(reader);
                        products.Add(product);
                    }
                    return products;
                },
                options: options
            );
        }

        #endregion

        #region GetById

        public async Task<Product?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                    SELECT p.*, 
                           sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, 
                           b.Id AS BrandId, b.Name AS BrandName,
                           img.FileName AS ImageFileName, img.BlobName AS ImageBlobName
                    FROM Products p
                    INNER JOIN SubCategories sc ON p.SubCategoryId = sc.Id
                    INNER JOIN Brands b ON p.BrandId = b.Id
                    LEFT JOIN Images img ON img.EntityType = 'products' AND img.EntityId = p.Id
                    WHERE p.Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                    {
                        return ProductMapper.FromReader(reader);
                    }
                    return null;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                }
            );
        }

        #endregion


    }
}
