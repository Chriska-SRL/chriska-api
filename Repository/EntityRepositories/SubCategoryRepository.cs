﻿using BusinessLogic.Dominio;
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
                    INSERT INTO SubCategories (Name, Description, CategoryId) 
                    OUTPUT INSERTED.Id 
                    VALUES (@Name, @Description, @CategoryId)", connection);

                command.Parameters.AddWithValue("@Name", subCategory.Name);
                command.Parameters.AddWithValue("@Description", subCategory.Description);
                command.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);

                connection.Open();
                int id = (int)command.ExecuteScalar();

                return new SubCategory(id, subCategory.Name, subCategory.Description, subCategory.Category);
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

        public SubCategory? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                SubCategory? subCategory = null;

                using (var command = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories WHERE Id = @Id", connection))
                {
                    command.Parameters.AddWithValue("@Id", id);
                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        subCategory = SubCategoryMapper.FromReader(reader);
                    else
                        return null;
                }

                if (subCategory != null)
                    subCategory.Category = GetCategory(subCategory.Category.Id);

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

                using (var command = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories", connection))
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        subCategories.Add(SubCategoryMapper.FromReader(reader));
                    }
                }

                foreach (var subCategory in subCategories)
                {
                    subCategory.Category = GetCategory(subCategory.Category.Id);
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


        public SubCategory Update(SubCategory subCategory)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using (var updateCommand = new SqlCommand("UPDATE SubCategories SET Name = @Name, Description = @Description, CategoryId = @CategoryId WHERE Id = @Id", connection))
                {
                    updateCommand.Parameters.AddWithValue("@Name", subCategory.Name);
                    updateCommand.Parameters.AddWithValue("@Description", subCategory.Description);
                    updateCommand.Parameters.AddWithValue("@CategoryId", subCategory.Category.Id);
                    updateCommand.Parameters.AddWithValue("@Id", subCategory.Id);

                    int affectedRows = updateCommand.ExecuteNonQuery();
                    if (affectedRows == 0)
                        throw new InvalidOperationException($"No se encontró la subcategoría con ID {subCategory.Id} para actualizar.");
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
        private Category GetCategory(int categoryId)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT Id, Name, Description FROM Categories WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", categoryId);

                using var reader = command.ExecuteReader();
                if (!reader.Read()) throw new InvalidOperationException($"Categoría con ID {categoryId} no encontrada.");

                return CategoryMapper.FromReader(reader);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener la categoría.");
                throw;
            }
        }

        public SubCategory GetByName(string name)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                SubCategory? subCategory = null;

                using (var command = new SqlCommand("SELECT Id, Name, Description, CategoryId FROM SubCategories WHERE Name = @Name", connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    using var reader = command.ExecuteReader();

                    if (reader.Read())
                        subCategory = SubCategoryMapper.FromReader(reader);
                    else
                        return null;
                }

                if (subCategory != null)
                    subCategory.Category = GetCategory(subCategory.Category.Id);

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
