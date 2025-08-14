using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ReturnRequestMapper
    {
        public static ReturnRequest? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);

            return new ReturnRequest(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                client: new Client(
                    id: r.GetInt32(r.GetOrdinal(Col("ClientId"))),
                    name: S(Col("ClientName"))
                ),
                status: Parse<Status>("Status"),
                date: r.GetDateTime(r.GetOrdinal(Col("Date"))),
                confirmedDate: r.IsDBNull(r.GetOrdinal(Col("ConfirmedDate"))) ? null : r.GetDateTime(r.GetOrdinal(Col("ConfirmedDate"))),
                observation: S(Col("Observation")),
                user: r.IsDBNull(r.GetOrdinal(Col("UserId"))) ? null : new User(
                    id: r.GetInt32(r.GetOrdinal(Col("UserId"))),
                    username: S(Col("UserUsername"))
                ),
                productItems: new List<ProductItem>(), // Assuming ProductItem mapping is handled elsewhere
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null,
                delivery: new Delivery(
                    id: r.GetInt32(r.GetOrdinal(Col("DeliveryId")))
                )
            );
        }
    }
}
