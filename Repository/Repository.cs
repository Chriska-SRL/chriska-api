using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public abstract class Repository<T>
    {
        private readonly string _connectionString;
        protected readonly ILogger<T> _logger;

        protected Repository(string connectionString, ILogger<T> logger)
        {
            _connectionString = connectionString;
            _logger = logger;
        }

        protected SqlConnection CreateConnection()
        {
            return new SqlConnection(_connectionString);
        }
    }
}