using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ProductMapper
    {
        // prefix: columnas del producto (ej: "Product"); origin: prefijo de relaciones (ej: "P")
        public static Product? FromReader(SqlDataReader r, string? prefix = null, string? origin = null)
        {
            string Col(string c) => $"{prefix ?? ""}{c}";
            string S(string c) => r.IsDBNull(r.GetOrdinal(c)) ? "" : r.GetString(r.GetOrdinal(c));

            if (prefix != null)
            {
                try { var o = r.GetOrdinal(Col("Id")); if (r.IsDBNull(o)) return null; } catch { return null; }
            }

            T Parse<T>(string c) where T : struct, Enum
                => Enum.Parse<T>(S(Col(c)).Trim(), true);

            return new Product(
                id: r.GetInt32(r.GetOrdinal(Col("Id"))),
                barcode: S(Col("Barcode")),
                name: S(Col("Name")),
                price: r.GetDecimal(r.GetOrdinal(Col("Price"))),
                image: S(Col("ImageUrl")),
                stock: r.IsDBNull(r.GetOrdinal(Col("Stock"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("Stock"))),
                availableStocks: r.IsDBNull(r.GetOrdinal(Col("AvailableStock"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("AvailableStock"))),
                description: S(Col("Description")),
                unitType: Parse<UnitType>("UnitType"),
                temperatureCondition: Parse<TemperatureCondition>("TemperatureCondition"),
                estimatedWeight: r.IsDBNull(r.GetOrdinal(Col("EstimatedWeight"))) ? 0 : r.GetInt32(r.GetOrdinal(Col("EstimatedWeight"))),
                observations: S(Col("Observations")),
                subCategory: SubCategoryMapper.FromReader(r, "SubCategory", origin), // ej PSubCategory*
                brand: BrandMapper.FromReader(r, "Brand", origin),       // ej PBrand*
                shelve: ShelveMapper.FromReader(r, "Shelve", origin),       // ej PShelve* (+ PWarehouse*)
                suppliers: new List<Supplier>(),
                auditInfo: prefix is null ? AuditInfoMapper.FromReader(r) : null
            );
        }
    }
}
