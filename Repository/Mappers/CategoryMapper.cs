using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class CategoryMapper
    {
        public static Category? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{origin ?? ""}{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            return new Category(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                name: S(Col("Name")),
                description: S(Col("Description")),
                subCategories: new List<SubCategory>(),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
