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
                try
                {
                    var o = r.GetOrdinal(Col("Id"));
                    if (r.IsDBNull(o)) return null;
                }
                catch { return null; }
            }

            // Parse genérico para enums
            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);

            return new Order(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                client: ClientMapper.FromReader(r, "Client", origin),
                status: Parse<Status>("Status"),
                date: r.GetDateTime(r.GetOrdinal(Col("Date"))),
                observations: S(Col("Observation")),
                user: UserMapper.FromReader(r, "User"),
                productItems: new List<ProductItem>(), // Se carga en otra query si aplica
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null,
                confirmedDate: r.GetDateTime(r.GetOrdinal(Col("ConfirmedDate"))),
                crates: r.GetInt32(r.GetOrdinal(Col("Crates"))),
                orderRequest: OrderRequestMapper.FromReader(r, "OrderRequest", origin),
                delivery: DeliveryMapper.FromReader(r, "Delivery", origin)
            );
        }
    }
}
