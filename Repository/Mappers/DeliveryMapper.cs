using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class DeliveryMapper
    {
        public static Delivery? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);


            var delivery = new Delivery(
             id: r.GetInt32(r.GetOrdinal(Col("Id"))),
             client: ClientMapper.FromReader(r, "Client"),
             status: Parse<Status>("Status"),
             confirmedDate: r.IsDBNull(r.GetOrdinal(Col("ConfirmedDate"))) ? (DateTime?)null : r.GetDateTime(r.GetOrdinal(Col("ConfirmedDate"))),
             date: r.GetDateTime(r.GetOrdinal(Col("Date"))),
             observation: S(Col("Observations")),
             user: UserMapper.FromReader(r, "User"),
             productItems: new List<ProductItem>(),
             auditInfo: AuditInfoMapper.FromReader(r),
             crates: r.IsDBNull(r.GetOrdinal(Col("Crates"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("Crates"))),
             payment: r.GetDecimal(r.GetOrdinal(Col("Payment"))),
             order: OrderMapper.FromReader(r, "Order")
         );

            return delivery;

        }
    }
}
