using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ClientMapper
    {
        public static Client FromReader(SqlDataReader reader)
        {
            return new Client(
               reader.GetInt32(reader.GetOrdinal("Id")),
               reader.GetString(reader.GetOrdinal("Name")),
               reader.GetString(reader.GetOrdinal("RUT")),
               reader.GetString(reader.GetOrdinal("RazonSocial")),
               reader.GetString(reader.GetOrdinal("Address")),
               reader.GetString(reader.GetOrdinal("MapsAddress")),
               reader.GetString(reader.GetOrdinal("Schedule")),
               reader.GetString(reader.GetOrdinal("Phone")),
               reader.GetString(reader.GetOrdinal("ContactName")),
               reader.GetString(reader.GetOrdinal("Email")),
               reader.GetString(reader.GetOrdinal("Observations")),
               new List<BankAccount>(), 
               reader.GetInt32(reader.GetOrdinal("LoanedCrates")),
               reader.GetString(reader.GetOrdinal("Qualification")),
               ZoneMapper.FromReaderForClient(reader),
               AuditInfoMapper.FromReader(reader)
            );
        }
    }
}
