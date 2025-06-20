
using BusinessLogic.Dominio;
using BusinessLogic.Repository;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;
using Repository.Mappers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repository.EntityRepositories
{
    public class ZoneRepository : Repository<Zone>, IZoneRepository
    {
        public ZoneRepository(string connectionString, Microsoft.Extensions.Logging.ILogger<Zone> logger) : base(connectionString, logger)
        {
        }

        public Zone Add(Zone entity)
        {
            throw new NotImplementedException();
        }

        public Zone? Delete(int id)
        {
            throw new NotImplementedException();
        }

        public List<Zone> GetAll()
        {
            throw new NotImplementedException();
        }

        public Zone? GetById(int id)
        {
            try
            {
                using var connection = CreateConnection();
                connection.Open();

                using var command = new SqlCommand("SELECT Id, Name, Description FROM Zones WHERE Id = @Id", connection);
                command.Parameters.AddWithValue("@Id", id);
                using var reader = command.ExecuteReader();
                return reader.Read() ? ZoneMapper.FromReader(reader) : null;

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
        


        public Zone Update(Zone entity)
        {
            throw new NotImplementedException();
        }
    }
}
