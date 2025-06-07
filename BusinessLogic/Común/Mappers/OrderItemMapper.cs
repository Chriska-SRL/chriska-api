using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;

namespace BusinessLogic.Común.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItem toDomain(AddOrderItemRequest dto)
        {
            return new OrderItem(
                id: 0,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice,
                product: new Product(
                    id: dto.ProductId,
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
                    suppliers: new List<Supplier>()
                )
            );
        }
        public static OrderItem.UpdatableData toDomain(UpdateOrderItemRequest dto)
        {
            return new OrderItem.UpdatableData
            {
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Product = new Product(
                    id: dto.ProductId,
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
                    suppliers: new List<Supplier>()
                )
            };
        }
        public static OrderItemResponse toResponse(OrderItem domain)
        {
            return new OrderItemResponse
            {
                Quantity = domain.Quantity,
                UnitPrice = domain.UnitPrice,
                Product = new ProductResponse
                {
                    Id = domain.Product.Id,
                    InternalCode = domain.Product.InternalCode,
                    Barcode = domain.Product.Barcode,
                    Name = domain.Product.Name,
                    Price = domain.Product.Price,
                    Image = domain.Product.Image,
                    Stock = domain.Product.Stock,
                    Description = domain.Product.Description,
                    UnitType = domain.Product.UnitType,
                    TemperatureCondition = domain.Product.TemperatureCondition,
                    Observation = domain.Product.Observation
                }

            };
        }
    }
}
