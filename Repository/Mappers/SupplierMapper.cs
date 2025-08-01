using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier FromReader(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("Id"));
            string name = reader.GetString(reader.GetOrdinal("Name"));
            string rut = reader.GetString(reader.GetOrdinal("RUT"));
            string razonSocial = reader.GetString(reader.GetOrdinal("RazonSocial"));
            string address = reader.GetString(reader.GetOrdinal("Address"));
            string mapsAddress = reader.IsDBNull(reader.GetOrdinal("MapsAddress")) ? string.Empty : reader.GetString(reader.GetOrdinal("MapsAddress"));
            string phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone"));
            string contactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContactName"));
            string email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email"));
            string observations = reader.IsDBNull(reader.GetOrdinal("Observations")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observations"));

            var auditInfo =  AuditInfoMapper.FromReader(reader);

            return new Supplier(id, name, rut, razonSocial, address, mapsAddress, phone, contactName, email, observations, new List<BankAccount>(), auditInfo);
        }
    }
}
