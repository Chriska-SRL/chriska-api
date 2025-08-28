using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public class ProductItemMapper
    {
        public static ProductItem? FromReader(SqlDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";

            try
            {
                var prodIdx = r.GetOrdinal("ProductId");
                if (r.IsDBNull(prodIdx)) return null;
            }
            catch
            {
                return null;
            }

            var item = new ProductItem
                (
                    r.GetDecimal(r.GetOrdinal(Col("Quantity"))),
                    r.IsDBNull(r.GetOrdinal(Col("Weight"))) ? (int?)null : r.GetInt32(r.GetOrdinal(Col("Weight"))),
                    r.GetDecimal(r.GetOrdinal(Col("UnitPrice"))),
                    r.GetDecimal(r.GetOrdinal(Col("Discount"))),
                    ProductMapper.FromReader(r, "Product")
                );

            return item;
        }
    }
}

