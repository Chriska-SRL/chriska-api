using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class DistributionMapper
    {
        public static Distribution? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{origin ?? ""}{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            
            if (prefix != null)
            {
                try
                {
                    int o = r.GetOrdinal(Col("Id"));
                    if (r.IsDBNull(o)) return null;
                }
                catch { return null; }
            }

            var user = UserMapper.FromReader(r, "User");
            if (user == null) return null;

            var vehicle = VehicleMapper.FromReader(r, "Vehicle"); 
            if (vehicle == null) return null;

            return new Distribution(
                id: r.GetInt32(r.GetOrdinal(Col("DistributionId"))),
                observations: S(Col("Observations")),
                date: r.GetDateTime(r.GetOrdinal(Col("Date"))),
                user: user,
                vehicle: vehicle,
                zones: new List<Zone>(),
                distributionDeliveries: new List<DistributionDelivery>(),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
