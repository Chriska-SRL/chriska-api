using BusinessLogic.Common.Enums;
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
                client: ClientMapper.FromReader(r,"Client"),
                status: Parse<Status>("Status"),
                date: r.GetDateTime(r.GetOrdinal(Col("Date"))),
                confirmedDate: r.IsDBNull(r.GetOrdinal(Col("ConfirmedDate"))) ? null : r.GetDateTime(r.GetOrdinal(Col("ConfirmedDate"))),
                observation: S(Col("Observations")),
                user: UserMapper.FromReader(r,"User"),
                productItems: new List<ProductItem>(),
                delivery: DeliveryMapper.FromReader(r,"Delivery",origin),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null             
            );
        }
    }
}
