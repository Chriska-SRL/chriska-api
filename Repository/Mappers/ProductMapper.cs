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
                description: reader.GetString(reader.GetOrdinal("CategoryDescription"))
            );

            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category
            );

            return new Product(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                internalCode: reader.GetString(reader.GetOrdinal("InternalCode")),
                barcode: reader.GetString(reader.GetOrdinal("BarCode")),
                name: reader.GetString(reader.GetOrdinal("Name")),
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                image: reader.GetString(reader.GetOrdinal("Image")),
                stock: reader.GetInt32(reader.GetOrdinal("Stock")),
                description: reader.GetString(reader.GetOrdinal("Description")),
                unitType: reader.GetString(reader.GetOrdinal("UnitType")),
                temperatureCondition: reader.GetString(reader.GetOrdinal("TemperatureCondition")),
                observation: reader.GetString(reader.GetOrdinal("Observations")),
                subCategory: subCategory,
                suppliers: new List<Supplier>()
            );
        }
    }
}
