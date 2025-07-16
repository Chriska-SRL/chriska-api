using BusinessLogic.Común;
using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class BrandRepository : Repository<Brand, Brand.UpdatableData>, IBrandRepository
    {
        public BrandRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Brand> AddAsync(Brand brand)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Brands (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)",
                brand,
                AuditAction.Insert,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", brand.Name);
                    cmd.Parameters.AddWithValue("@Description", brand.Description);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Brand(newId, brand.Name, brand.Description, brand.AuditInfo);
        }


        #endregion

        #region Update

        public async Task<Brand> UpdateAsync(Brand brand)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Brands SET Name = @Name, Description = @Description WHERE Id = @Id",
                brand,
                AuditAction.Update,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", brand.Id);
                    cmd.Parameters.AddWithValue("@Name", brand.Name);
                    cmd.Parameters.AddWithValue("@Description", brand.Description);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la marca con Id {brand.Id}");

            return brand;
        }

        #endregion

        #region Delete

        public async Task<Brand> DeleteAsync(Brand brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand), "La marca no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Brands SET IsDeleted = 1 WHERE Id = @Id",
                brand,
                AuditAction.Delete,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", brand.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la marca con Id {brand.Id}");

            return brand;
        }

        #endregion

        #region GetAll

        public async Task<List<Brand>> GetAllAsync(QueryOptions options)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Brands",
                map: reader =>
                {
                    var brands = new List<Brand>();
                    while (reader.Read())
                    {
                        brands.Add(BrandMapper.FromReader(reader));
                    }
                    return brands;
                },
                options: options
            );
        }

        #endregion

        #region GetById

        public async Task<Brand?> GetByIdAsync(int id)
        {
            var options = new QueryOptions
            {
                Filters = new Dictionary<string, string>
                {
                    ["Id"] = id.ToString()
                }
            };

            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Brands",
                map: reader =>
                {
                    if (reader.Read())
                        return BrandMapper.FromReader(reader);
                    return null;
                },
                options: options
            );
        }

        #endregion
    }
}
