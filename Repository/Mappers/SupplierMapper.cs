using BusinessLogic.Common;
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
            Location location = Location.FromString(reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")));
            string phone = reader.IsDBNull(reader.GetOrdinal("Phone")) ? string.Empty : reader.GetString(reader.GetOrdinal("Phone"));
            string contactName = reader.IsDBNull(reader.GetOrdinal("ContactName")) ? string.Empty : reader.GetString(reader.GetOrdinal("ContactName"));
            string email = reader.IsDBNull(reader.GetOrdinal("Email")) ? string.Empty : reader.GetString(reader.GetOrdinal("Email"));
            string observations = reader.IsDBNull(reader.GetOrdinal("Observations")) ? string.Empty : reader.GetString(reader.GetOrdinal("Observations"));

            var auditInfo = AuditInfoMapper.FromReader(reader);

            return new Supplier(id, name, rut, razonSocial, address, location, phone, contactName, email, observations, new List<BankAccount>(), auditInfo);
        }

        public static Supplier FromReaderForProduct(SqlDataReader reader)
        {
            int id = reader.GetInt32(reader.GetOrdinal("SupplierId"));
            string name = reader.GetString(reader.GetOrdinal("SupplierName"));
            string rut = reader.GetString(reader.GetOrdinal("SupplierRUT"));
            string razonSocial = reader.GetString(reader.GetOrdinal("SupplierRazonSocial"));
            string address = reader.GetString(reader.GetOrdinal("SupplierAddress"));
            Location location = Location.FromString(reader.IsDBNull(reader.GetOrdinal("Location")) ? null : reader.GetString(reader.GetOrdinal("Location")));
            string phone = reader.IsDBNull(reader.GetOrdinal("SupplierPhone")) ? string.Empty : reader.GetString(reader.GetOrdinal("SupplierPhone"));
            string contactName = reader.IsDBNull(reader.GetOrdinal("SupplierContactName")) ? string.Empty : reader.GetString(reader.GetOrdinal("SupplierContactName"));
            string email = reader.IsDBNull(reader.GetOrdinal("SupplierEmail")) ? string.Empty : reader.GetString(reader.GetOrdinal("SupplierEmail"));
            string observations = reader.IsDBNull(reader.GetOrdinal("SupplierObservations")) ? string.Empty : reader.GetString(reader.GetOrdinal("SupplierObservations"));

            return new Supplier(id, name, rut, razonSocial, address, location, phone, contactName, email, observations, new List<BankAccount>(), new AuditInfo());
        }
    }
}
