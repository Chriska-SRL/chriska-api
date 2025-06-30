using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;
using System.Data.SqlClient;

namespace Repository.Mappers
{
    public static class PurchaseMapper
    {
        public static Purchase FromReader(SqlDataReader reader)
        {
            var supplier = new Supplier(reader.GetInt32(reader.GetOrdinal("SupplierId")))
            {
                Name = reader.GetString(reader.GetOrdinal("SupplierName"))
            };

            return new Purchase(
                reader.GetInt32(reader.GetOrdinal("Id")),
                reader.GetDateTime(reader.GetOrdinal("Date")),
                reader.GetString(reader.GetOrdinal("Status")),
                supplier
            );
        }
    }
}
