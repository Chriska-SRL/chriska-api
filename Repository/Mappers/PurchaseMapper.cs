using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class PurchaseMapper
    {
        public static Purchase? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{origin ?? ""}{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            // Si viene con prefijo pero no hay fila/columna válida, no mapeamos
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

            // Mapeo de campos principales
            int id = r.GetInt32(r.GetOrdinal(Col("Id")));
            DateTime date = r.GetDateTime(r.GetOrdinal(Col("Date")));
            string observations = S(Col("Observations"));
            string? invoiceNumber = r.IsDBNull(r.GetOrdinal(Col("InvoiceNumber"))) ? null : r.GetString(r.GetOrdinal(Col("InvoiceNumber")));

            // Mapeo de usuario
            var user = UserMapper.FromReader(r, "User");

            // Mapeo de proveedor
            var supplier = SupplierMapper.FromReader(r, "Supplier");

            // Auditoría
            var auditInfo = AuditInfoMapper.FromReader(r);

            // La lista de items se completa en el repositorio (por referencia)
            return new Purchase(
                id: id,
                date: date,
                observations: observations,
                user: user,
                productItems: new List<ProductItem>(),
                supplier: supplier,
                auditInfo: auditInfo,
                invoiceNumber: invoiceNumber
            );
        }
    }
}
