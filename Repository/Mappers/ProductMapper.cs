using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class ProductMapper
    {
        public static Product FromReader(SqlDataReader reader)
        {
            // Cargar la categoría
            var category = new Category(
                id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                name: reader.GetString(reader.GetOrdinal("CategoryName")),
                description: reader.GetString(reader.GetOrdinal("CategoryDescription")),
                subCategories: new List<SubCategory>()
                ,
                auditInfo:null
            );

            // Cargar la subcategoría
            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category,
                auditInfo: null
            );

            // Cargar la marca
            var brand = new Brand(
                id: reader.GetInt32(reader.GetOrdinal("BrandId")),
                name: reader.GetString(reader.GetOrdinal("BrandName")),
                description: reader.GetString(reader.GetOrdinal("BrandDescription")),
                auditInfo: null
            );

            // Mapeo del tipo de unidad (UnitType)
            string unitTypeStr = reader.GetString(reader.GetOrdinal("UnitType")).Trim();
            UnitType unitType = unitTypeStr switch
            {
                "Unit" => UnitType.Unit,
                "Kilo" => UnitType.Kilo,
                _ => UnitType.None
            };

            // Mapeo de la condición de temperatura (TemperatureCondition)
            string tempStr = reader.GetString(reader.GetOrdinal("TemperatureCondition")).Trim();
            TemperatureCondition tempCondition = tempStr switch
            {
                "Cold" => TemperatureCondition.Cold,
                "Frozen" => TemperatureCondition.Frozen,
                "Ambient" => TemperatureCondition.Ambient,
                _ => TemperatureCondition.None
            };

            // Cargar el código de barras
            string? barcode = reader.IsDBNull(reader.GetOrdinal("BarCode"))
                ? null
                : reader.GetString(reader.GetOrdinal("BarCode"));

            // Obtener la imagen y la URL (si está disponible)
            string imageFileName = reader.IsDBNull(reader.GetOrdinal("ImageFileName")) ? null : reader.GetString(reader.GetOrdinal("ImageFileName"));
            string imageBlobName = reader.IsDBNull(reader.GetOrdinal("ImageBlobName")) ? null : reader.GetString(reader.GetOrdinal("ImageBlobName"));

            // Asignar la URL de la imagen si existe
            string? imageUrl = null;
            if (!string.IsNullOrEmpty(imageBlobName))
            {
                imageUrl = imageBlobName;
            }

            // Crear y devolver el producto
            return new Product(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                barcode: barcode,
                name: reader.GetString(reader.GetOrdinal("Name")),
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                image: imageUrl ?? imageFileName, // Usamos la URL de la imagen si existe
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
