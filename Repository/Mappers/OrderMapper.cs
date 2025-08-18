using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class OrderMapper
    {
        public static Order? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);

            var confirmedDate = r.IsDBNull(r.GetOrdinal(Col("ConfirmedDate")))
                ? default(DateTime)
                : r.GetDateTime(r.GetOrdinal(Col("ConfirmedDate")));

            var order = new Order
            (
                r.GetInt32(r.GetOrdinal(Col("Id"))),
                ClientMapper.FromReader(r, "Client"),
                Parse<Status>("Status"),
                r.GetDateTime(r.GetOrdinal(Col("Date"))),
                S(Col("Observations")),
                UserMapper.FromReader(r, "User"),
                new List<ProductItem>(),
                AuditInfoMapper.FromReader(r),
                confirmedDate,
                r.IsDBNull(r.GetOrdinal(Col("Crates"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("Crates"))),
                null,
                null
            );

            return order;
        }
    }
}