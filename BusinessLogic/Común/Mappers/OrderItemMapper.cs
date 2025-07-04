using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsCategory;
using BusinessLogic.DTOs.DTOsOrderItem;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsSubCategory;

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
        public static OrderItem.UpdatableData ToUpdatableData(UpdateOrderItemRequest dto)
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
                    Observation = domain.Product.Observation,
                    SubCategory = new SubCategoryResponse
                    {
                        Id = domain.Product.SubCategory.Id,
                        Name = domain.Product.SubCategory.Name,
                        Category = new CategoryResponse
                        {
                            Id = domain.Product.SubCategory.Category.Id,
                            Name = domain.Product.SubCategory.Category.Name
                        }
                    },
                    Brand = new BrandResponse
                    {
                        Id = domain.Product.Brand.Id,
                        Name = domain.Product.Brand.Name
                    }
                }
            };
        }
    }
}
