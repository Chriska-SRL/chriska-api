using BusinessLogic.Domain;
using BusinessLogic.Common.Enums;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ClientReceiptMapper
    {
        public static ClientReceipt? FromReader(SqlDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string? GetStringOrNull(string c) => r.IsDBNull(r.GetOrdinal(c)) ? null : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try
                {
                    int o = r.GetOrdinal(Col("Id"));
                    if (r.IsDBNull(o)) return null;
                }
                catch
                {
                    return null;
                }
            }

            int id = r.GetInt32(r.GetOrdinal(Col("Id")));
            DateTime date = r.GetDateTime(r.GetOrdinal(Col("Date")));
            decimal amount = r.GetDecimal(r.GetOrdinal(Col("Amount")));
            string notes = GetStringOrNull(Col("Notes")) ?? string.Empty;

            int ordPm = r.GetOrdinal(Col("PaymentMethod"));
            if (r.IsDBNull(ordPm))
                throw new InvalidOperationException("PaymentMethod no puede ser null.");

            string pmStr = r.GetString(ordPm);
            if (!Enum.TryParse(pmStr.Trim(), true, out PaymentMethod paymentMethod))
                throw new InvalidOperationException($"Valor inválido de PaymentMethod: '{pmStr}'.");

            var client = ClientMapper.FromReader(r, "Client");

            var receipt = new ClientReceipt(
                id,
                date,
                amount,
                notes,
                paymentMethod,
                prefix is null ? AuditInfoMapper.FromReader(r) : null,
                client
            );

            return receipt;
        }
    }
}
