using BusinessLogic.Dominio;
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
                category: new Category(
                    id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                    name: string.Empty // Se puede completar luego si es necesario
                )
            );
        }
    }
}
