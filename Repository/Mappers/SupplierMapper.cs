using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class SupplierMapper
    {
        public static Supplier FromReader(SqlDataReader reader)
        {
            return new Supplier(
            id: reader.GetInt32(reader.GetOrdinal("Id")),
            name: reader.GetString(reader.GetOrdinal("Name")),
            razonSocial: reader.GetString(reader.GetOrdinal("RazonSocial")),
            rut: reader.GetString(reader.GetOrdinal("RUT")),
            contactName: reader.GetString(reader.GetOrdinal("ContactName")),
            phone: reader.GetString(reader.GetOrdinal("Phone")),
            email: reader.IsDBNull(reader.GetOrdinal("Email")) ? null : reader.GetString(reader.GetOrdinal("Email")),
            address: reader.IsDBNull(reader.GetOrdinal("Address")) ? null : reader.GetString(reader.GetOrdinal("Address")),
            mapsAddress: reader.IsDBNull(reader.GetOrdinal("mapsAddress")) ? null : reader.GetString(reader.GetOrdinal("mapsAddress")),
            bank: reader.GetString(reader.GetOrdinal("Bank")),
            bankAccount: reader.GetString(reader.GetOrdinal("BankAccount")),
            observations: reader.IsDBNull(reader.GetOrdinal("Observations")) ? null : reader.GetString(reader.GetOrdinal("Observations")),
            products: new List<Product>(),
            payments: new List<Payment>(),
            purchases: new List<Purchase>(),
            daysToDeliver: new List<Day>()
);
        }
    }
}
