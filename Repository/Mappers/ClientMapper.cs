using BusinessLogic.Common;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
using System.Reflection.PortableExecutable;

namespace Repository.Mappers
{
    public static class ClientMapper
    {
        public static Client? FromReader(SqlDataReader r, string? prefix = null, string? originForZone = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            return new Client(
            r.GetInt32(r.GetOrdinal(Col("Id"))),
            S(Col("Name")),
            S(Col("RUT")),
            S(Col("RazonSocial")),
            S(Col("Address")),
            location : Location.FromString(r.IsDBNull(r.GetOrdinal("Location")) ? null : r.GetString(r.GetOrdinal("Location"))),
            S(Col("Schedule")),
            S(Col("Phone")),
            S(Col("ContactName")),
            S(Col("Email")),
            S(Col("Observations")),
            new List<BankAccount>(),
            r.IsDBNull(r.GetOrdinal(Col("LoanedCrates"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("LoanedCrates"))),
            S(Col("Qualification")),
            zone: ZoneMapper.FromReader(r, "Zone", originForZone), // p.ej. CZone*
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
