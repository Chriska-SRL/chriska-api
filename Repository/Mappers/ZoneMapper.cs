using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ZoneMapper
    {
        public static Zone? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{origin ?? ""}{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            var zone = new Zone(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                name: S(Col("Name")),
                description: S(Col("Description")),
                image: S(Col("ImageUrl")),
                deliveryDays: new List<Day>(),
                requestDays: new List<Day>(),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );

            if (!r.IsDBNull(r.GetOrdinal(Col("DeliveryDays"))))
                zone.DeliveryDays = S(Col("DeliveryDays"))
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim(), true)).ToList();

            if (!r.IsDBNull(r.GetOrdinal(Col("RequestDays"))))
                zone.RequestDays = S(Col("RequestDays"))
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(d => Enum.Parse<Day>(d.Trim(), true)).ToList();

            return zone;
        }
    }
}
