using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class BrandRepository : Repository<Brand, Brand.UpdatableData>, IBrandRepository
    {
        public BrandRepository(string connectionString, ILogger<Brand> logger)
            : base(connectionString, logger) { }

        public Brand Add(Brand brand)
        {
            int newId = ExecuteWriteWithAudit(
                "INSERT INTO Brands (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)",
                brand,
                AuditAction.Insert,
                cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", brand.Name);
                    cmd.Parameters.AddWithValue("@Description", brand.Description);
                },
                cmd => (int)cmd.ExecuteScalar()
            );

            return new Brand(newId, brand.Name, brand.Description, brand.AuditInfo);
        }

        public Brand Update(Brand brand)
        {
            int rows = ExecuteWriteWithAudit(
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

        public Brand Delete(Brand brand)
        {
            if (brand == null)
                throw new ArgumentNullException(nameof(brand), "La marca no puede ser nula.");

            int rows = ExecuteWriteWithAudit(
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

        public List<Brand> GetAll()
        {
            return ExecuteRead(
               tableName: "Brands",
               map: reader =>
               {
                   var brands = new List<Brand>();
                   while (reader.Read())
                   {
                       brands.Add(BrandMapper.FromReader(reader));
                   }
                   return brands;
               }
           );
        }

        public Brand? GetById(int id)
        {
            return ExecuteRead(
                tableName: "Brands",
                map: reader =>
                {
                    if (reader.Read())
                        return BrandMapper.FromReader(reader);

                    return null;
                },
                filters: new Dictionary<string, string>
                {
                    ["Id"] = id.ToString()
                }
            );
        }


        public Brand? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Brand> GetAll(Dictionary<string, string>? filters)
        {
            return ExecuteRead(
                tableName: "Brands",
                map: reader =>
                {
                    var brands = new List<Brand>();
                    while (reader.Read())
                    {
                        brands.Add(BrandMapper.FromReader(reader));
                    }
                    return brands;
                },
                filters: filters
            );
        }

    }
}
