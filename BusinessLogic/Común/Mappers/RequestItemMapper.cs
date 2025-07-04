using BusinessLogic.Dominio;
using BusinessLogic.DTOs.DTOsBrand;
using BusinessLogic.DTOs.DTOsProduct;
using BusinessLogic.DTOs.DTOsRequestItem;
using BusinessLogic.DTOs.DTOsSubCategory;

namespace BusinessLogic.Común.Mappers
{
    public static class RequestItemMapper
    {
        public static RequestItem ToDomain(AddRequestItem_Request dto)
        {
            return new RequestItem(
                id: 0,
                quantity: dto.Quantity,
                unitPrice: dto.UnitPrice,
                product: new Product(dto.ProductId)
            );
        }

        public static RequestItem.UpdatableData ToDomain(UpdateRequestItem_Request dto)
        {
            return new RequestItem.UpdatableData
            {
                Quantity = dto.Quantity,
                UnitPrice = dto.UnitPrice
            };
        }

        public static RequestItemResponse ToResponse(RequestItem requestItem)
        {
            return new RequestItemResponse
            {
                Quantity = requestItem.Quantity,
                UnitPrice = requestItem.UnitPrice,
                Product = new ProductResponse
                {
                    Id = requestItem.Product.Id,
                    InternalCode = requestItem.Product.InternalCode,
                    Barcode = requestItem.Product.Barcode,
                    Name = requestItem.Product.Name,
                    Price = requestItem.Product.Price,
                    Image = requestItem.Product.Image,
                    Stock = requestItem.Product.Stock,
                    Description = requestItem.Product.Description,
                    UnitType = requestItem.Product.UnitType,
                    TemperatureCondition = requestItem.Product.TemperatureCondition,
                    Observation = requestItem.Product.Observation,
                    SubCategory = new SubCategoryResponse
                    {
                        Id = requestItem.Product.SubCategory.Id,
                        Name = requestItem.Product.SubCategory.Name
                    },
                    Brand = new BrandResponse
                    {
                        Id = requestItem.Product.Brand.Id,
                        Name = requestItem.Product.Brand.Name

                    }
                }
            };
        }
    }
}
