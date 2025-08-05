using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Repository.Logging;
using Repository.Mappers;
using BusinessLogic.Common;

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

            return new Category(newId, category.Name, category.Description,new List<SubCategory>() ,category.AuditInfo);
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
            var allowedFilters = new[] { "Name","Description" };

            return await ExecuteReadAsync(
                baseQuery: @"
            SELECT c.Id, c.Name, c.Description,
                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription
            FROM Categories c
            LEFT JOIN SubCategories sc ON sc.CategoryId = c.Id AND sc.IsDeleted = 0",
                map: reader =>
                {
                    var categories = new Dictionary<int, Category>();

                    while (reader.Read())
                    {
                        var categoryId = reader.GetInt32(reader.GetOrdinal("Id"));

                        if (!categories.TryGetValue(categoryId, out var category))
                        {
                            category = CategoryMapper.FromReader(reader);
                            category.SubCategories = new List<SubCategory>(); 
                            categories[categoryId] = category;
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("SubCategoryId")))
                        {
                            var subCategory = SubCategoryMapper.FromReaderForCategory(reader);
                            category.SubCategories.Add(subCategory);
                        }
                    }

                    return categories.Values.ToList();
                },
                options: options,
                tableAlias: "c",
                allowedFilterColumns: allowedFilters
            );
        }

        #endregion

        #region GetById


        public async Task<Category?> GetByIdAsync(int id)
        {
            return await ExecuteReadAsync(
                baseQuery: @"
            SELECT c.Id, c.Name, c.Description,
                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription
            FROM Categories c
            LEFT JOIN SubCategories sc ON sc.CategoryId = c.Id AND sc.IsDeleted = 0
            WHERE c.Id = @Id",
                map: reader =>
                {
                    Category? category = null;

                    while (reader.Read())
                    {
                        if (category == null)
                        {
                            category = CategoryMapper.FromReader(reader);
                            category.SubCategories = new List<SubCategory>();
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("SubCategoryId")))
                        {
                            var subCategory = SubCategoryMapper.FromReaderForCategory(reader);
                            category.SubCategories.Add(subCategory);
                        }
                    }

                    return category;
                },
                options: new QueryOptions(),
                tableAlias: "c",
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
                baseQuery: @"
            SELECT c.Id, c.Name, c.Description,
                   sc.Id AS SubCategoryId, sc.Name AS SubCategoryName, sc.Description AS SubCategoryDescription
            FROM Categories c
            LEFT JOIN SubCategories sc ON sc.CategoryId = c.Id  AND sc.IsDeleted = 0
            WHERE c.Name = @Name",
                map: reader =>
                {
                    Category? category = null;

                    while (reader.Read())
                    {
                        if (category == null)
                        {
                            category = CategoryMapper.FromReader(reader);
                            category.SubCategories = new List<SubCategory>();
                        }

                        if (!reader.IsDBNull(reader.GetOrdinal("SubCategoryId")))
                        {
                            var subCategory = SubCategoryMapper.FromReaderForCategory(reader);
                            category.SubCategories.Add(subCategory);
                        }
                    }

                    return category;
                },
                options: new QueryOptions(),
                tableAlias: "c",
                configureCommand: cmd =>
                {
                    cmd.Parameters.AddWithValue("@Name", name);
                }
            );
        }

        #endregion
    }
}
