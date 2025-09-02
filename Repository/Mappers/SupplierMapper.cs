using BusinessLogic.Common;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier? FromReader(SqlDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string? GetStringOrNull(string c) => r.IsDBNull(r.GetOrdinal(c)) ? null : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try
                {
                    int o = r.GetOrdinal(Col("Id"));
                    if (r.IsDBNull(o)) return null;
                }
                catch
                {
                    return null;
                }
            }

            int id = r.GetInt32(r.GetOrdinal(Col("Id")));
            string name = r.GetString(r.GetOrdinal(Col("Name")));
            string rut = r.GetString(r.GetOrdinal(Col("RUT")));
            string razonSocial = r.GetString(r.GetOrdinal(Col("RazonSocial")));
            string address = r.GetString(r.GetOrdinal(Col("Address")));

            Location location = Location.FromString(GetStringOrNull(Col("Location")));
            string phone = GetStringOrNull(Col("Phone")) ?? string.Empty;
            string contactName = GetStringOrNull(Col("ContactName")) ?? string.Empty;
            string email = GetStringOrNull(Col("Email")) ?? string.Empty;
            string observations = GetStringOrNull(Col("Observations")) ?? string.Empty;

            var auditInfo = prefix is null ? AuditInfoMapper.FromReader(r) : new AuditInfo();

            return new Supplier(id, name, rut, razonSocial, address, location, phone, contactName, email, observations, new List<BankAccount>(), auditInfo);
        }
    }
}
