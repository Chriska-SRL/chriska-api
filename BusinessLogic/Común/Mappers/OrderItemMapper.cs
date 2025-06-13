using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;

namespace BusinessLogic.Común.Mappers
{
    public static class OrderItemMapper
    {
        public static OrderItem ToDomain(AddOrderItemRequest dto)
        {
            return new OrderItem(
                id: 0,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice,
                product: new Product(dto.ProductId)
            );
        }

        public static OrderItem.UpdatableData ToDomain(UpdateOrderItemRequest dto)
        {
            return new OrderItem.UpdatableData
            {
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice,
                Product = new Product(dto.ProductId)
            };
        }

        public static OrderItemResponse ToResponse(OrderItem domain)
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
