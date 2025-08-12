using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class DiscountMapper
    {
        public static Discount FromReader(SqlDataReader r, string? prefix = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));
            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);

            return new Discount(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                description: S(Col("Description")),
                expirationDate: r.GetDateTime(r.GetOrdinal(Col("ExpirationDate"))),
                productQuantity: r.GetInt32(r.GetOrdinal(Col("ProductQuantity"))),
                percentage: r.GetDecimal(r.GetOrdinal(Col("Percentage"))),
                products: new List<Product>(),
                clients: new List<Client>(),
                status: Parse<DiscountStatus>("Status"),
                brand: BrandMapper.FromReader(r, "Brand", "D"), // DBrand*
                subCategory: SubCategoryMapper.FromReader(r, "SubCategory", "D"), // DSubCategory* + DCategory*
                zone: ZoneMapper.FromReader(r, "Zone", "D"), // DZone*
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
