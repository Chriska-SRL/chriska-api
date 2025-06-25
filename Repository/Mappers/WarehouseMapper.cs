using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class WarehouseMapper
    {
        public static Warehouse FromReader(SqlDataReader reader, string prefix = "")
        {
            return new Warehouse(
                id: reader.GetInt32(reader.GetOrdinal(prefix + "Id")),
                name: reader.GetString(reader.GetOrdinal(prefix + "Name")),
                description: reader.GetString(reader.GetOrdinal(prefix + "Description")),
                address: reader.GetString(reader.GetOrdinal(prefix + "Address")),
                shelves: new List<Shelve>()
            );
        }
    }
}
