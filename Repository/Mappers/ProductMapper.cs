using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ProductMapper
    {
        public static Product FromReader(SqlDataReader reader)
        {
            // Categoria (NOT NULL)
            var category = new Category(
                id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                name: reader.GetString(reader.GetOrdinal("CategoryName")),
                description: reader.GetString(reader.GetOrdinal("CategoryDescription")),
                subCategories: new List<SubCategory>(),
                auditInfo: new AuditInfo()
            );

            // Subcategoria (NOT NULL)
            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category,
                auditInfo: new AuditInfo()
            );

            // Marca (NOT NULL)
            var brand = new Brand(
                id: reader.GetInt32(reader.GetOrdinal("BrandId")),
                name: reader.GetString(reader.GetOrdinal("BrandName")),
                description: reader.GetString(reader.GetOrdinal("BrandDescription")),
                auditInfo: new AuditInfo()
            );

            // UnitType (enum) - NOT NULL
            string unitTypeStr = reader.GetString(reader.GetOrdinal("UnitType")).Trim();
            UnitType unitType = unitTypeStr switch
            {
                "Unit" => UnitType.Unit,
                "Kilo" => UnitType.Kilo,
                _ => UnitType.None
            };

            // TemperatureCondition (enum) - NOT NULL
            string tempStr = reader.GetString(reader.GetOrdinal("TemperatureCondition")).Trim();
            TemperatureCondition tempCondition = tempStr switch
            {
                "Cold" => TemperatureCondition.Cold,
                "Frozen" => TemperatureCondition.Frozen,
                "Ambient" => TemperatureCondition.Ambient,
                _ => TemperatureCondition.None
            };

            // Producto
            return new Product(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                barcode: reader.GetString(reader.GetOrdinal("Barcode")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                image: reader.GetString(reader.GetOrdinal("ImageUrl")),
                stock: reader.GetInt32(reader.GetOrdinal("Stock")),
                availableStocks: reader.GetInt32(reader.GetOrdinal("AvailableStock")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                unitType: unitType,
                temperatureCondition: tempCondition,
                estimatedWeight: reader.GetInt32(reader.GetOrdinal("EstimatedWeight")),
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                subCategory: subCategory,
                brand: brand,
                suppliers: new List<Supplier>(),
                auditInfo: AuditInfoMapper.FromReader(reader)
            );
        }
    }
}
