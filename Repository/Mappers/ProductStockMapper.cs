using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ProductStockMapper
    {
        public static ProductStock FromReader(SqlDataReader reader)
        {
            return new ProductStock(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                quantity: reader.GetInt32(reader.GetOrdinal("Quantity")),
                product: new Product(reader.GetInt32(reader.GetOrdinal("ProductId"))),
                shelve: new Shelve(reader.GetInt32(reader.GetOrdinal("ShelveId")))
            );
        }
    }
}
