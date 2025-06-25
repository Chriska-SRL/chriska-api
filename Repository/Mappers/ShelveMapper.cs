using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ShelveMapper
    {
        public static Shelve FromReader(SqlDataReader reader, string prefix = "")
        {
            return new Shelve(
                id: reader.GetInt32(reader.GetOrdinal(prefix + "Id")),
                name: reader.GetString(reader.GetOrdinal(prefix + "Name")),
                description: reader.GetString(reader.GetOrdinal(prefix + "Description")),
                warehouse: new Warehouse(reader.GetInt32(reader.GetOrdinal(prefix + "WarehouseId"))),
                productStocks: new List<ProductStock>(),
                stockMovements: new List<StockMovement>()
            );
        }
    }
}
