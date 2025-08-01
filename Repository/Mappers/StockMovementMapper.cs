using BusinessLogic.Común;
using BusinessLogic.Común.Enums;
using BusinessLogic.Domain;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement FromReader(SqlDataReader reader)
        {
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

            var category = new Category(
                id: reader.GetInt32(reader.GetOrdinal("CategoryId")),
                name: reader.GetString(reader.GetOrdinal("CategoryName")),
                description: reader.GetString(reader.GetOrdinal("CategoryDescription")),
                subCategories: new List<SubCategory>(),
                auditInfo: null
            );

            var subCategory = new SubCategory(
                id: reader.GetInt32(reader.GetOrdinal("SubCategoryId")),
                name: reader.GetString(reader.GetOrdinal("SubCategoryName")),
                description: reader.GetString(reader.GetOrdinal("SubCategoryDescription")),
                category: category,
                auditInfo: null
            );
            var brand = new Brand(
                id: reader.GetInt32(reader.GetOrdinal("BrandId")),
                name: reader.GetString(reader.GetOrdinal("BrandName")),
                description: reader.GetString(reader.GetOrdinal("BrandDescription")),
                auditInfo: new AuditInfo()
            );

            var product = new Product(
                id: reader.GetInt32(reader.GetOrdinal("ProductId")),
                name: reader.GetString(reader.GetOrdinal("ProductName")),
                barcode: reader.GetString(reader.GetOrdinal("BarCode")),
                unitType: unitType,
                price: reader.GetDecimal(reader.GetOrdinal("Price")),
                description: reader.GetString(reader.GetOrdinal("ProductDescription")),
                temperatureCondition: tempCondition,
                stock: reader.GetInt32(reader.GetOrdinal("ProductStock")),
                aviableStock: reader.GetInt32(reader.GetOrdinal("ProductAviableStock")),
                image: reader.GetString(reader.GetOrdinal("Image")),
                observations: reader.GetString(reader.GetOrdinal("Observations")),
                subCategory: subCategory,
                brand:brand,
                suppliers: new List<Supplier>(),
                auditInfo: new AuditInfo()
            );

            var warehouse = new Warehouse(
                id: reader.GetInt32(reader.GetOrdinal("WarehouseId")),
                name: reader.GetString(reader.GetOrdinal("WarehouseName")),
                description: reader.GetString(reader.GetOrdinal("WarehouseDescription")),
                address: reader.GetString(reader.GetOrdinal("Address")),
                shelves: new List<Shelve>(), 
                auditInfo: new AuditInfo()
            );

            var shelve = new Shelve(
                id: reader.GetInt32(reader.GetOrdinal("ShelveId")),
                name: reader.GetString(reader.GetOrdinal("ShelveName")),
                description: reader.GetString(reader.GetOrdinal("ShelveDescription")),
                warehouse: warehouse,
                stockMovements: new List<StockMovement>(), 
                auditInfo: new AuditInfo()

            );

            var role = new Role(
                id: reader.GetInt32(reader.GetOrdinal("RoleId")),
                name: reader.GetString(reader.GetOrdinal("RoleName")),
                description: reader.GetString(reader.GetOrdinal("RoleDescription")),
                permissions: new List<Permission>(),
                auditInfo: null
            );

            var user = new User(
                id: reader.GetInt32(reader.GetOrdinal("UserId")),
                name: reader.GetString(reader.GetOrdinal("UserName")),
                username: reader.GetString(reader.GetOrdinal("Username")),
                isEnabled: reader.GetString(reader.GetOrdinal("IsEnabled")).Trim() == "T",
                needsPasswordChange: reader.GetString(reader.GetOrdinal("NeedsPasswordChange")).Trim() == "T",
                password: reader.GetString(reader.GetOrdinal("Password")),
                role: role,
                auditInfo: null
            );

            var stockMovement = new StockMovement(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                date: reader.GetDateTime(reader.GetOrdinal("Date")),
                quantity: reader.GetInt32(reader.GetOrdinal("Quantity")),
                type: ParseType(reader.GetString(reader.GetOrdinal("Type")).Trim()),
                reason: reader.GetString(reader.GetOrdinal("Reason")),
                shelve: shelve,
                user: user,
                product: product,
                auditInfo: new AuditInfo()
            );

            return stockMovement;
        }

        private static StockMovementType ParseType(string typeCode)
        {
            return typeCode switch
            {
                "I" => StockMovementType.Ingreso,
                "E" => StockMovementType.Egreso,
                _ => throw new InvalidOperationException($"Tipo de movimiento desconocido: {typeCode}")
            };
        }
    }
}
