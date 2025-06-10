using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsShelve;
using BusinessLogic.DTOs.DTOsStockMovement;
using BusinessLogic.DTOs.DTOsUser;

namespace BusinessLogic.Común.Mappers
{
    public static class StockMovementMapper
    {
        public static StockMovement toDomain(AddStockMovementRequest addStockMovement)
        {
            return new StockMovement(
                id: 0,
                date: addStockMovement.Date,
                quantity: addStockMovement.Quantity,
                type: addStockMovement.Type,
                reason: addStockMovement.Reason,
                shelve: new Shelve(
                    id: addStockMovement.ShelveId,
                    description: string.Empty,
                    warehouse: null,
                    productStocks: null,
                    stockMovements: null
                ),
                user: new User(
                    id: addStockMovement.UserId,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: null
                ),
                product: new Product(
                    id: addStockMovement.ProductId,
                    internalCode: string.Empty,
                    barcode: string.Empty,
                    name: string.Empty,
                    price: 0,
                    image: string.Empty,
                    stock: 0,
                    description: string.Empty,
                    unitType: string.Empty,
                    temperatureCondition: string.Empty,
                    observation: string.Empty,
                    subCategory: null,
                    suppliers: null
                )
            );
        }
        public static StockMovement.UpdatableData toDomain(UpdateStockMovementRequest updateStockMovement)
        {
            return new StockMovement.UpdatableData
            {
                Date = updateStockMovement.Date,
                Quantity = updateStockMovement.Quantity,
                Type = updateStockMovement.Type,
                Reason = updateStockMovement.Reason,
                Shelve = new Shelve(
                    id: updateStockMovement.ShelveId,
                    description: string.Empty,
                    warehouse: null,
                    productStocks: null,
                    stockMovements: null
                ),
                User = new User(
                    id: updateStockMovement.UserId,
                    name: string.Empty,
                    username: string.Empty,
                    password: string.Empty,
                    isEnabled: true,
                    role: null,
                    requests: null
                ),
                Product = new Product(
                    id: updateStockMovement.ProductId,
                    internalCode: string.Empty,
                    barcode: string.Empty,
                    name: string.Empty,
                    price: 0,
                    image: string.Empty,
                    stock: 0,
                    description: string.Empty,
                    unitType: string.Empty,
                    temperatureCondition: string.Empty,
                    observation: string.Empty,
                    subCategory: null,
                    suppliers: null
                )
            };
        }
        public static StockMovementResponse toResponse(StockMovement stockMovement)
        {
            return new StockMovementResponse
            {
                Date = stockMovement.Date,
                Quantity = stockMovement.Quantity,
                Type = stockMovement.Type,
                Reason = stockMovement.Reason,
                Shelve = new ShelveResponse
                {
                    Description = stockMovement.Shelve.Description
                },
                User = new UserResponse
                {
                    Id = stockMovement.User.Id,
                    Name = stockMovement.User.Name,
                    Username = stockMovement.User.Username
                },
                Product = new ProductResponse
                {
                    Id = stockMovement.Product.Id,
                    InternalCode = stockMovement.Product.InternalCode,
                    Barcode = stockMovement.Product.Barcode,
                    Name = stockMovement.Product.Name,
                    Price = stockMovement.Product.Price,
                    Image = stockMovement.Product.Image,
                    Stock = stockMovement.Product.Stock,
                    Description = stockMovement.Product.Description,
                    UnitType = stockMovement.Product.UnitType,
                    TemperatureCondition = stockMovement.Product.TemperatureCondition,
                    Observation = stockMovement.Product.Observation
                }
            };
        }
    }
}
