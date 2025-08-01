using BusinessLogic.Común;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

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
    
    }
}
