using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Común;

namespace Repository.EntityRepositories
{
    public class CategoryRepository : Repository<Category, Category.UpdatableData>, ICategoryRepository
    {
        public CategoryRepository(string connectionString, AuditLogger auditLogger)
            : base(connectionString, auditLogger) { }

        #region Add

        public async Task<Category> AddAsync(Category category)
        {
            int newId = await ExecuteWriteWithAuditAsync(
                "INSERT INTO Categories (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)",
                category,
                AuditAction.Insert,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.Parameters.AddWithValue("@Description", category.Description);
                },
                async cmd => Convert.ToInt32(await cmd.ExecuteScalarAsync())
            );

            if (newId == 0)
                throw new InvalidOperationException("No se pudo obtener el Id insertado.");

            return new Category(newId, category.Name, category.Description, category.AuditInfo);
        }

        #endregion

        #region Update

        public async Task<Category> UpdateAsync(Category category)
        {
            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id",
                category,
                AuditAction.Update,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", category.Id);
                    cmd.Parameters.AddWithValue("@Name", category.Name);
                    cmd.Parameters.AddWithValue("@Description", category.Description);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo actualizar la categoría con Id {category.Id}");

            return category;
        }

        #endregion

        #region Delete

        public async Task<Category> DeleteAsync(Category category)
        {
            if (category == null)
                throw new ArgumentNullException(nameof(category), "La categoría no puede ser nula.");

            int rows = await ExecuteWriteWithAuditAsync(
                "UPDATE Categories SET IsDeleted = 1 WHERE Id = @Id",
                category,
                AuditAction.Delete,
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Id", category.Id);
                }
            );

            if (rows == 0)
                throw new InvalidOperationException($"No se pudo eliminar la categoría con Id {category.Id}");

            return category;
        }

        #endregion

        #region GetAll

        public async Task<List<Category>> GetAllAsync(QueryOptions options)
        {
            var allowedFilters = new[] { "Name" };
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Categories",
                map: reader =>
                {
                    var categories = new List<Category>();
                    while (reader.Read())
                    {
                        categories.Add(CategoryMapper.FromReader(reader));
                    }
                    return categories;
                },
                options: options,
                allowedFilterColumns: allowedFilters
            );
        }

        #endregion

        #region GetById

        public async Task<Category?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Categories WHERE Id = @Id",
                map: reader =>
                {
                    if (reader.Read())
                        return CategoryMapper.FromReader(reader);
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

        #region GetByName
        public async Task<Category?> GetByNameAsync(string name)
        {
            return await ExecuteReadAsync(
                baseQuery: "SELECT * FROM Categories WHERE Name = @Name AND IsDeleted = 0",
                map: reader =>
                {
                    if (reader.Read())
                        return CategoryMapper.FromReader(reader);
                    return null;
                },
                options: new QueryOptions(),
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
            );
        }

        #endregion
    }
}
