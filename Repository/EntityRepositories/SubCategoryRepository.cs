using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Repository.Mappers;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository.EntityRepositories
{
    public class SubCategoryRepository : Repository<SubCategoryRepository>, ISubCategoryRepository
    {
        public SubCategoryRepository(string connectionString, ILogger<SubCategoryRepository> logger) : base(connectionString, logger)
        {
        }

        public SubCategory Add(SubCategory subCategory)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO SubCategories (Name, CategoryId) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @CategoryId)", connection);

                command.Parameters.AddWithValue("@Name", subCategory.Name);
                command.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);

                connection.Open();
                int id = (int)command.ExecuteScalar();

                return new SubCategory(id, subCategory.Name, subCategory.Category);
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

        public SubCategory? Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Intentando eliminar la subcategoría con ID {id}.");
                using var connection = CreateConnection();
                connection.Open();

                var subCategory = GetById(id);
                if (subCategory == null)
                {
                    _logger.LogWarning($"No se encontró la subcategoría con ID {id} para eliminar.");
                    return null;
                }

                using (var command = new SqlCommand("DELETE FROM SubCategories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    command.ExecuteNonQuery();
                }

                _logger.LogInformation($"Subcategoría con ID {id} eliminada correctamente.");
                return subCategory;
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

        public List<SubCategory> GetAll()
        {
            try
            {
                var subCategories = new List<SubCategory>();

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name, CategoryId FROM SubCategories", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subCategories.Add(SubCategoryMapper.FromReader(reader));
                    }
                }

                return subCategories;
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

        public SubCategory? GetById(int id)
        {
            try
            {
                SubCategory subCategory;

                using var connection = CreateConnection();
                connection.Open();

                using (var command = new SqlCommand("SELECT Id, Name, CategoryId FROM SubCategories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();
                    if (!reader.Read()) return null;

                    subCategory = SubCategoryMapper.FromReader(reader);
                }

                return subCategory;
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

        public SubCategory Update(SubCategory subCategory)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var updateCommand = new SqlCommand("UPDATE SubCategories SET Name = @Name, CategoryId = @CategoryId WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", subCategory.Name);
                    updateCommand.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);
                    updateCommand.Parameters.AddWithValue("@Id", subCategory.Id);
                    updateCommand.ExecuteNonQuery();
        }

                return subCategory;
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
