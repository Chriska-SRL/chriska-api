using BusinessLogic.Domain;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;

namespace Repository.EntityRepositories
{
    public class BrandRepository : Repository<Brand>, IBrandRepository
    {
        public BrandRepository(string connectionString, ILogger<Brand> logger)
            : base(connectionString, logger) { }

        public Brand Add(Brand brand)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    INSERT INTO Brands (Name, Description)
                    OUTPUT INSERTED.Id
                    VALUES (@Name, @Description)", connection);

                command.Parameters.AddWithValue("@Name", brand.Name);
                command.Parameters.AddWithValue("@Description", brand.Description);

                connection.Open();
                var result = command.ExecuteScalar();

                if (result == null || result == DBNull.Value)
                    throw new InvalidOperationException("No se pudo insertar la marca.");

                int brandId = (int)result;

                return new Brand(brandId, brand.Name, brand.Description);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al insertar la marca.");
                throw new ApplicationException("Error SQL al insertar la marca.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al insertar la marca.");
                throw new ApplicationException("Error inesperado al insertar la marca.", ex);
            }
        }

        public Brand Delete(int id)
        {
            try
            {
                var brand = GetById(id);
                if (brand == null)
                {
                    _logger.LogWarning($"No se encontró la marca con ID {id} para eliminar.");
                    return null;
                }

                using var connection = CreateConnection();
                using var command = new SqlCommand("DELETE FROM Brands WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                command.ExecuteNonQuery();

                _logger.LogInformation($"Marca con ID {id} eliminada correctamente.");
                return brand;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al eliminar la marca.");
                throw new ApplicationException("Error SQL al eliminar la marca.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al eliminar la marca.");
                throw new ApplicationException("Error inesperado al eliminar la marca.", ex);
            }
        }

        public List<Brand> GetAll()
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand("SELECT * FROM Brands", connection);

                connection.Open();
                using var reader = command.ExecuteReader();

                var brands = new List<Brand>();

                while (reader.Read())
                {
                    brands.Add(BrandMapper.FromReader(reader));
                }

                return brands;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al obtener todas las marcas.");
                throw new ApplicationException("Error SQL al obtener las marcas.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al obtener las marcas.");
                throw new ApplicationException("Error inesperado al obtener las marcas.", ex);
            }
        }

        public Brand GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand("SELECT * FROM Brands WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);

                connection.Open();
                using var reader = command.ExecuteReader();

                if (!reader.Read()) return null;

                return BrandMapper.FromReader(reader);
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al buscar la marca por ID.");
                throw new ApplicationException("Error SQL al buscar la marca por ID.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al buscar la marca por ID.");
                throw new ApplicationException("Error inesperado al buscar la marca por ID.", ex);
            }
        }

        public Brand Update(Brand brand)
        {
            try
            {
                using var connection = CreateConnection();
                using var command = new SqlCommand(@"
                    UPDATE Brands SET
                        Name = @Name,
                        Description = @Description
                    WHERE Id = @Id", connection);

                command.Parameters.AddWithValue("@Id", brand.Id);
                command.Parameters.AddWithValue("@Name", brand.Name);
                command.Parameters.AddWithValue("@Description", brand.Description);

                connection.Open();
                var rows = command.ExecuteNonQuery();

                if (rows == 0)
                    throw new InvalidOperationException("No se pudo actualizar la marca.");

                return brand;
            }
            catch (SqlException ex)
            {
                _logger.LogError(ex, "Error al actualizar la marca.");
                throw new ApplicationException("Error SQL al actualizar la marca.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error inesperado al actualizar la marca.");
                throw new ApplicationException("Error inesperado al actualizar la marca.", ex);
            }
        }
    }
}
