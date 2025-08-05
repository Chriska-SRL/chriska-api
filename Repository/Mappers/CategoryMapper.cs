using BusinessLogic.Common;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class CategoryMapper
    {
        public static Category FromReader(SqlDataReader reader)
        {
            return new Category(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                subCategories: new List<SubCategory>(),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );

        }
        public static Category FromReaderForSubCategory(SqlDataReader reader)
        {
            return new Category(
                id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                name: reader.GetString(reader.GetOrdinal("CategoryName")),
                description: reader.GetString(reader.GetOrdinal("CategoryDescription")),
                subCategories: new List<SubCategory>(),
                auditInfo: new AuditInfo()
            );
        }
        
    }
}
