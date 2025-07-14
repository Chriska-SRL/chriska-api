using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class BrandMapper
    {
        public static Brand FromReader(SqlDataReader reader)
        {
            return new Brand(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }
    }
}
