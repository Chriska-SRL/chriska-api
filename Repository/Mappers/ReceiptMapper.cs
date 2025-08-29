using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ReceiptMapper
    {
        public static Receipt? FromReader(SqlDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string? S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? null : r.GetString(r.GetOrdinal(c));

            // Si usamos prefix (cuando viene de joins), validamos que haya Id
            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            var id = r.GetInt32(r.GetOrdinal(Col("Id")));
            var date = r.GetDateTime(r.GetOrdinal(Col("Date")));
            var amount = r.GetDecimal(r.GetOrdinal(Col("Amount")));
            var notes = S(Col("Notes"));

            var client = ClientMapper.FromReader(r, "Client");

            var receipt = new Receipt(
                id,
                date,
                amount,
                notes ?? string.Empty,
                client,
                prefix is null ? AuditInfoMapper.FromReader(r) : null
            );

            return receipt;
        }
    }
}
