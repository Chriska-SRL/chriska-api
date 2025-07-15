using BusinessLogic.Común;
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
                auditInfo: new AuditInfo
                {
                    CreatedAt = reader.GetDateTime(reader.GetOrdinal("CreatedAt")),
                    CreatedBy = reader.GetInt32(reader.GetOrdinal("CreatedBy")),
                    CreatedLocation = reader.GetString(reader.GetOrdinal("CreatedLocation")),
                    UpdatedAt = reader.IsDBNull(reader.GetOrdinal("UpdatedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("UpdatedAt")),
                    UpdatedBy = reader.GetInt32(reader.GetOrdinal("UpdatedBy")),
                    UpdatedLocation = reader.IsDBNull(reader.GetOrdinal("UpdatedLocation")) ? null : reader.GetString(reader.GetOrdinal("UpdatedLocation")),
                    DeletedAt = reader.IsDBNull(reader.GetOrdinal("DeletedAt")) ? (DateTime?)null : reader.GetDateTime(reader.GetOrdinal("DeletedAt")),
                    DeletedBy = reader.GetInt32(reader.GetOrdinal("DeletedBy")),
                    DeletedLocation = reader.IsDBNull(reader.GetOrdinal("DeletedLocation")) ? null : reader.GetString(reader.GetOrdinal("DeletedLocation"))
                }   
            );
        }
    }
}
