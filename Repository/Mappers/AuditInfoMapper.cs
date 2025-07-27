using BusinessLogic.Common;
using BusinessLogic.Común;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class AuditInfoMapper
    {
        public static AuditInfo FromReader(SqlDataReader reader)
        {
            return new AuditInfo
            {
                CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                CreatedLocation = Location.FromString(reader.GetString(reader.GetOrdinal("CreatedLocation"))),
                UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                UpdatedBy = reader.IsDBNull(reader.GetOrdinal("UpdatedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("UpdatedBy")),
                UpdatedLocation = Location.FromString(reader.IsDBNull(reader.GetOrdinal("UpdatedLocation")) ? null : reader.GetString(reader.GetOrdinal("UpdatedLocation"))),
                DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                DeletedBy = reader.IsDBNull(reader.GetOrdinal("DeletedBy")) ? 0 : reader.GetInt32(reader.GetOrdinal("DeletedBy")),
                DeletedLocation = Location.FromString(reader.IsDBNull(reader.GetOrdinal("DeletedLocation")) ? null : reader.GetString(reader.GetOrdinal("DeletedLocation")))
            };
        }
    }
}
