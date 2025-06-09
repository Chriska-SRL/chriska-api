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
                var categories = new List<Category>();

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Categories", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        categories.Add(CategoryMapper.FromReader(reader));
                    }
                }

                return categories;
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

                using (var command = new SqlCommand("SELECT Id, Name, Description FROM Categories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    return CategoryMapper.FromReader(reader);
                }
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
