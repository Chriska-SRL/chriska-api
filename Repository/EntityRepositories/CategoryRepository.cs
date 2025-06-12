using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class CategoryRepository : Repository<CategoryRepository>, ICategoryRepository
    {
        public CategoryRepository(string connectionString, ILogger<CategoryRepository> logger) : base(connectionString, logger)
        {
        }

        public Category Add(Category category)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"INSERT INTO Categories (Name, Description) OUTPUT INSERTED.Id VALUES (@Name, @Description)", connection);
                command.Parameters.AddWithValue("@Name", category.Name);
                command.Parameters.AddWithValue("@Description", category.Description);

                connection.Open();
                int id = (int)command.ExecuteScalar();

                return new Category(id, category.Name, category.Description);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Category? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar la categoría con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var category = GetById(id);
                if (category == null)
                {
                    _logger.LogWarning($"No se encontró la categoría con ID {id} para eliminar.");
                    return null;
                }

                using (var command = new SqlCommand("DELETE FROM Categories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                _logger.LogInformation($"Categoría con ID {id} eliminada correctamente.");
                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public List<Category> GetAll()
        {
            try
            {
                var categories = new Dictionary<int, Category>();
                var subCategories = new List<SubCategory>();

                using var connection = CreateConnection();
                connection.Open();

                // Traer categorías
                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Categories", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var category = CategoryMapper.FromReader(reader);
                        category.SubCategories = new List<SubCategory>();
                        categories[category.Id] = category;
                    }
                }

                // Traer subcategorías
                using (var subCommand = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories", connection))
                using (var subReader = subCommand.ExecuteReader())
                {
                    while (subReader.Read())
                    {
                        var rawSub = SubCategoryMapper.FromReader(subReader);
                        if (categories.TryGetValue(rawSub.Category.Id, out var parentCategory))
                        {
                            var subWithCategory = new SubCategory(
                                id: rawSub.Id,
                                name: rawSub.Name,
                                description: rawSub.Description,
                                category: parentCategory
                            );
                            parentCategory.SubCategories.Add(subWithCategory);
                        }
                    }
                }

                return categories.Values.ToList();
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Category? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                Category? category = null;

                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Categories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        category = CategoryMapper.FromReader(reader);
                        category.SubCategories = new List<SubCategory>();
                    }
                    else
                    {
                        return null;
                    }
                }

                using (var subCommand = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories WHERE CategoryId = @CategoryId", connection))
                {
                    subCommand.Parameters.AddWithValue("@CategoryId", id);
                    using var subReader = subCommand.ExecuteReader();
                    while (subReader.Read())
                    {
                        var rawSub = SubCategoryMapper.FromReader(subReader);
                        var subWithCategory = new SubCategory(
                            id: rawSub.Id,
                            name: rawSub.Name,
                            description: rawSub.Description,
                            category: category!
                        );
                        category!.SubCategories.Add(subWithCategory);
                    }
                }

                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Category GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                Category? category = null;

                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Categories WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        category = CategoryMapper.FromReader(reader);
                        category.SubCategories = new List<SubCategory>();
                    }
                    else
                    {
                        return null;
                    }
                }

                using (var subCommand = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories WHERE CategoryId = @CategoryId", connection))
                {
                    subCommand.Parameters.AddWithValue("@CategoryId", category.Id);
                    using var subReader = subCommand.ExecuteReader();
                    while (subReader.Read())
                    {
                        var rawSub = SubCategoryMapper.FromReader(subReader);
                        var subWithCategory = new SubCategory(
                            id: rawSub.Id,
                            name: rawSub.Name,
                            description: rawSub.Description,
                            category: category!
                        );
                        category!.SubCategories.Add(subWithCategory);
                    }
                }

                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }

        public Category Update(Category category)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var updateCommand = new SqlCommand("UPDATE Categories SET Name = @Name, Description = @Description WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", category.Name);
                    updateCommand.Parameters.AddWithValue("@Description", category.Description);
                    updateCommand.Parameters.AddWithValue("@Id", category.Id);

                    int affectedRows = updateCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                        throw new InvalidOperationException($"No se encontró la categoría con ID {category.Id} para actualizar.");
                }

                return category;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al acceder a la base de datos.");
                throw new ApplicationException("Error al acceder a la base de datos.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado.");
                throw new ApplicationException("Ocurrió un error inesperado.", ex);
            }
        }
    }
}
