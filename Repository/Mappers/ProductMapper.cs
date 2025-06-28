using BusinessLogic.Dominio;
using BusinessLogic.Común.Enums;
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
                description: reader.GetString(reader.GetOrdinal("CategoryDescription"))
            );

            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category
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

            return new Product(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                barcode: reader.GetString(reader.GetOrdinal("BarCode")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                image: reader.GetString(reader.GetOrdinal("Image")),
                stock: reader.GetInt32(reader.GetOrdinal("Stock")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                unitType: unitType,
                temperatureCondition: tempCondition,
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                subCategory: subCategory,
                suppliers: new List<Supplier>() // los proveedores se asignan aparte
            );
        }
    }
}
