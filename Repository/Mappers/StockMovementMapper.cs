using BusinessLogic.Common;
using BusinessLogic.Common.Enums;
using BusinessLogic.Domain;
using Microsoft.Data.SqlClient;

namespace Repository.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement FromReader(SqlDataReader reader)
        {
            string unitTypeStr = reader.GetString(reader.GetOrdinal("ProductUnitType")).Trim();
            UnitType unitType = unitTypeStr switch
            {
                "Unit" => UnitType.Unit,
                "Kilo" => UnitType.Kilo,
                _ => UnitType.None
            };

            string tempStr = reader.GetString(reader.GetOrdinal("ProductTemperatureCondition")).Trim();
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
                auditInfo: null
            );



            var warehouse = new Warehouse(
                id: reader.GetInt32(reader.GetOrdinal("WarehouseId")),
                name: reader.GetString(reader.GetOrdinal("WarehouseName")),
                description: reader.GetString(reader.GetOrdinal("WarehouseDescription")),
                shelves: new List<Shelve>(), 
                auditInfo: null
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
                auditInfo: new AuditInfo()
            );

            var user = new User(
                id: reader.GetInt32(reader.GetOrdinal("UserId")),
                name: reader.GetString(reader.GetOrdinal("UserName")),
                username: reader.GetString(reader.GetOrdinal("UserUsername")),
                isEnabled: reader.GetString(reader.GetOrdinal("UserIsEnabled")).Trim() == "T",
                needsPasswordChange: reader.GetString(reader.GetOrdinal("UserNeedsPasswordChange")).Trim() == "T",
                password: "",
                role: role,
                auditInfo: new AuditInfo()
            );


            var product = new Product(
                id: reader.GetInt32(reader.GetOrdinal("ProductId")),
                name: reader.GetString(reader.GetOrdinal("ProductName")),
                barcode: reader.GetString(reader.GetOrdinal("ProductBarCode")),
                unitType: unitType,
                price: reader.GetDecimal(reader.GetOrdinal("ProductPrice")),
                description: reader.GetString(reader.GetOrdinal("ProductDescription")),
                temperatureCondition: tempCondition,
                estimatedWeight: reader.GetInt32(reader.GetOrdinal("ProductEstimatedWeight")),
                stock: reader.GetDecimal(reader.GetOrdinal("ProductStock")),
                availableStocks: reader.GetDecimal(reader.GetOrdinal("ProductAvailableStock")),
                image: reader.GetString(reader.GetOrdinal("ProductImageUrl")),
                observations: reader.GetString(reader.GetOrdinal("ProductObservations")),
                subCategory: subCategory,
                brand: brand,
                shelve: shelve,
                suppliers: new List<Supplier>(),
                auditInfo: null
            );

            var stockMovement = new StockMovement(
                id: reader.GetInt32(reader.GetOrdinal("Id")),
                date: reader.GetDateTime(reader.GetOrdinal("Date")),
                quantity: reader.GetDecimal(reader.GetOrdinal("Quantity")),
                type: ParseType(reader.GetString(reader.GetOrdinal("Type")).Trim()),
                reasonType: ParseReasonType(reader.GetString(reader.GetOrdinal("ReasonType")).Trim()),
                reason: reader.GetString(reader.GetOrdinal("Reason")),
                user: user,
                product: product,
                auditInfo: AuditInfoMapper.FromReader(reader)
            );

            return stockMovement;
        }

        private static StockMovementType ParseType(string typeCode)
        {
            return typeCode switch
            {
                "Inbound" => StockMovementType.Inbound,
                "Outbound" => StockMovementType.Outbound,
                _ => throw new InvalidOperationException($"Tipo de movimiento desconocido: {typeCode}")
            };
        }

        private static ReasonType ParseReasonType(string reasonCode) => reasonCode switch
        {
            "Purchase" => ReasonType.Purchase,
            "Sale" => ReasonType.Sale,
            "Return" => ReasonType.Return,
            "Adjustment" => ReasonType.Adjustment,
            "DeliveryCancellation" => ReasonType.DeliveryCancellation,
            _ => throw new InvalidOperationException($"RasonType desconocido: {reasonCode}")
        };
    }
}
