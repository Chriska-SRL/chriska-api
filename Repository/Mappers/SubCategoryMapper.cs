using BusinessLogic.Común;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Repository.Mappers
{
    public static class SubCategoryMapper
    {
        public static SubCategory FromReader(SqlDataReader reader)
        {
            return new SubCategory(
             id: reader.GetInt32(reader.GetOrdinal("Id")),
             name: reader.GetString(reader.GetOrdinal("Name")),
             description: reader.GetString(reader.GetOrdinal("Description")),
             category: CategoryMapper.FromReaderForSubCategory(reader),
             auditInfo: AuditInfoMapper.FromReader(reader)

         );
        }
        public static SubCategory FromReaderForCategory(SqlDataReader reader)
        {
            return new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: new Category(0),
                auditInfo: new AuditInfo()
            );
        }

    }
}
