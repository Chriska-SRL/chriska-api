using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Dominio;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ProductMapper
    {
        public static Product FromReader(SqlDataReader reader)
        {
            var category = new Category(
                id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                name: reader.GetString(reader.GetOrdinal("CategoryName")),
                description: reader.GetString(reader.GetOrdinal("CategoryDescription")),
                auditInfo: new AuditInfo()
            );

            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category,
                auditInfo: new AuditInfo()
            );

            var brand = new Brand(
                id: reader.GetInt32(reader.GetOrdinal("BrandId")),
                name: reader.GetString(reader.GetOrdinal("BrandName")),
                description: reader.GetString(reader.GetOrdinal("BrandDescription")),
                auditInfo: new AuditInfo()
            );

            string unitTypeStr = reader.GetString(reader.GetOrdinal("UnitType")).Trim();
            UnitType unitType = unitTypeStr switch
            {
                "Unit" => UnitType.Unit,
                "Kilo" => UnitType.Kilo,
                _ => UnitType.None
            };

            string tempStr = reader.GetString(reader.GetOrdinal("TemperatureCondition")).Trim();
            TemperatureCondition tempCondition = tempStr switch
            {
                "Cold" => TemperatureCondition.Cold,
                "Frozen" => TemperatureCondition.Frozen,
                "Ambient" => TemperatureCondition.Ambient,
                _ => TemperatureCondition.None
            };

            string? barcode = reader.IsDBNull(reader.GetOrdinal("BarCode"))
                ? null
                : reader.GetString(reader.GetOrdinal("BarCode"));

            return new Product(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                barcode: barcode,
                name: reader.GetString(reader.GetOrdinal("Name")),
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                image: reader.GetString(reader.GetOrdinal("Image")),
                stock: reader.GetInt32(reader.GetOrdinal("Stock")),
                aviableStock: reader.GetInt32(reader.GetOrdinal("AviableStock")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                unitType: unitType,
                temperatureCondition: tempCondition,
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                subCategory: subCategory,
                brand: brand,
                suppliers: new List<Supplier>(), 
                auditInfo: new AuditInfo()
            );
        }
    }
}
