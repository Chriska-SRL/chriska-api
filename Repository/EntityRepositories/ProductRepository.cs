using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Mappers;
using Repository.Logging;
using BusinessLogic.Dominio;

namespace Repository.EntityRepositories
{
    public class ProductRepository : Repository<Product, Product.UpdatableData>, IProductRepository
    {
        public ProductRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger)
        {
        }

        #region Add

        public async Task<Product> AddAsync(Product product)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Products (Name, Barcode, InternalCode, UnitType, Price, Description, TemperatureCondition, Stock, AviableStock, Image, Observation, SubCategoryId, BrandId, CreatedAt, CreatedBy, CreatedLocation) " +
                "OUTPUT INSERTED.Id " +
                "VALUES (@Name, @Barcode, @InternalCode, @UnitType, @Price, @Description, @TemperatureCondition, @Stock, @AviableStock, @Image, @Observation, @SubCategoryId, @BrandId, @CreatedAt, @CreatedBy, @CreatedLocation)",
                product,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Barcode", (object?)product.Barcode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InternalCode", product.InternalCode);
                    cmd.Parameters.AddWithValue("@UnitType", (int)product.UnitType);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@TemperatureCondition", (int)product.TemperatureCondition);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AviableStock", product.AviableStock);
                    cmd.Parameters.AddWithValue("@Image", product.Image);
                    cmd.Parameters.AddWithValue("@Observation", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                    cmd.Parameters.AddWithValue("@UpdatedAt", product.AuditInfo.UpdatedAt);
                    cmd.Parameters.AddWithValue("@UpdatedBy", product.AuditInfo.UpdatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedLocation", (object?)product.AuditInfo.UpdatedLocation ?? DBNull.Value);

                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            product.SetInternalCode(); // después de asignar el ID

            return new Product(newId, product.Barcode, product.Name, product.Price, product.Image, product.Stock, product.AviableStock, product.Description, product.UnitType, product.TemperatureCondition, product.Observation, product.SubCategory, product.Brand, product.Suppliers, product.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Product> UpdateAsync(Product product)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET Name = @Name, Barcode = @Barcode, InternalCode = @InternalCode, UnitType = @UnitType, Price = @Price, Description = @Description, TemperatureCondition = @TemperatureCondition, " +
                "Stock = @Stock, AviableStock = @AviableStock, Image = @Image, Observation = @Observation, SubCategoryId = @SubCategoryId, BrandId = @BrandId, " +
                "UpdatedAt = @UpdatedAt, UpdatedBy = @UpdatedBy, UpdatedLocation = @UpdatedLocation WHERE Id = @Id",
                product,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@Name", product.Name);
                    cmd.Parameters.AddWithValue("@Barcode", (object?)product.Barcode ?? DBNull.Value);
                    cmd.Parameters.AddWithValue("@InternalCode", product.InternalCode);
                    cmd.Parameters.AddWithValue("@UnitType", (int)product.UnitType);
                    cmd.Parameters.AddWithValue("@Price", product.Price);
                    cmd.Parameters.AddWithValue("@Description", product.Description);
                    cmd.Parameters.AddWithValue("@TemperatureCondition", (int)product.TemperatureCondition);
                    cmd.Parameters.AddWithValue("@Stock", product.Stock);
                    cmd.Parameters.AddWithValue("@AviableStock", product.AviableStock);
                    cmd.Parameters.AddWithValue("@Image", product.Image);
                    cmd.Parameters.AddWithValue("@Observation", product.Observation);
                    cmd.Parameters.AddWithValue("@SubCategoryId", product.SubCategory.Id);
                    cmd.Parameters.AddWithValue("@BrandId", product.Brand.Id);
                    cmd.Parameters.AddWithValue("@UpdatedAt", product.AuditInfo.UpdatedAt);
                    cmd.Parameters.AddWithValue("@UpdatedBy", product.AuditInfo.UpdatedBy);
                    cmd.Parameters.AddWithValue("@UpdatedLocation", (object?)product.AuditInfo.UpdatedLocation ?? DBNull.Value);

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
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Products SET IsDeleted = 1, DeletedAt = @DeletedAt, DeletedBy = @DeletedBy, DeletedLocation = @DeletedLocation WHERE Id = @Id",
                product,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", product.Id);
                    cmd.Parameters.AddWithValue("@DeletedAt", product.AuditInfo.DeletedAt);
                    cmd.Parameters.AddWithValue("@DeletedBy", product.AuditInfo.DeletedBy);
                    cmd.Parameters.AddWithValue("@DeletedLocation", (object?)product.AuditInfo.DeletedLocation ?? DBNull.Value);

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
                baseQuery: "SELECT * FROM Products",
                map: reader =>
                {
                    var products = new List<Product>();
                    while (reader.Read())
                    {
                        products.Add(ProductMapper.FromReader(reader));
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
                baseQuery: "SELECT * FROM Products WHERE Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return ProductMapper.FromReader(reader);
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
