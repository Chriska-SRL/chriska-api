using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class SubCategoryRepository : Repository<SubCategory, SubCategory.UpdatableData>, ISubCategoryRepository
    {
        public SubCategoryRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<SubCategory> AddAsync(SubCategory subCategory)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO SubCategories (Name, Description, CategoryId) OUTPUT INSERTED.Id VALUES (@Name, @Description, @CategoryId)",
                subCategory,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", subCategory.Name);
                    cmd.Parameters.AddWithValue("@Description", subCategory.Description);
                    cmd.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new SubCategory(newId, subCategory.Name, subCategory.Description, subCategory.Category, subCategory.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<SubCategory> UpdateAsync(SubCategory subCategory)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE SubCategories SET Name = @Name, Description = @Description, CategoryId = @CategoryId WHERE Id = @Id",
                subCategory,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", subCategory.Id);
                    cmd.Parameters.AddWithValue("@Name", subCategory.Name);
                    cmd.Parameters.AddWithValue("@Description", subCategory.Description);
                    cmd.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la subcategoría con Id {subCategory.Id}");

            return subCategory;
        }

        #endregion

        #region Delete

        public async Task<SubCategory> DeleteAsync(SubCategory subCategory)
        {
            if (subCategory == null)
                throw new ArgumentNullException(nameof(subCategory), "La subcategoría no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE SubCategories SET IsDeleted = 1 WHERE Id = @Id",
                subCategory,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", subCategory.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la subcategoría con Id {subCategory.Id}");

            return subCategory;
        }

        #endregion

        #region GetAll

        public async Task<List<SubCategory>> GetAllAsync(QueryOptions options)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                SELECT sc.*, 
                       c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription
                FROM SubCategories sc
                INNER JOIN Categories c ON sc.CategoryId = c.Id",
                map: reader =>
                {
                    var subCategories = new List<SubCategory>();
                    while (reader.Read())
                    {
                        subCategories.Add(SubCategoryMapper.FromReader(reader));
                    }
                    return subCategories;
                },
                options: options
            );
        }


        #endregion

        #region GetById

        public async Task<SubCategory?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
                SELECT sc.*, 
                       c.Id AS CategoryId, c.Name AS CategoryName, c.Description AS CategoryDescription
                FROM SubCategories sc
                INNER JOIN Categories c ON sc.CategoryId = c.Id
                WHERE sc.Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return SubCategoryMapper.FromReader(reader);
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
